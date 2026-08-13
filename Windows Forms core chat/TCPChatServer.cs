using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows_Forms_CORE_CHAT_UGH;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

//https://github.com/AbleOpus/NetworkingSamples/blob/master/MultiServer/Program.cs
namespace Windows_Forms_Chat
{
    public class TCPChatServer : TCPChatBase
    {

        public Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //connected clients
        public List<ClientSocket> clientSockets = new List<ClientSocket>();

        public string player1 = null;
        public string player2 = null;
        public string currentTurn = null;

        public static TCPChatServer createInstance(int port, RichTextBox chatTextBox, TicTacToe ttt)
        {
            TCPChatServer tcp = null;
            //setup if port within range and valid chat box given
            if (port > 0 && port < 65535 && chatTextBox != null)
            {
                tcp = new TCPChatServer();
                tcp.port = port;
                tcp.ChatTextBox = chatTextBox;
                tcp.ticTacToe = ttt;
            }

            //return empty if user not enter useful details
            return tcp;
        }

        public void SetupServer()
        {
            ChatTextBox.Text += "Setting up server...\n";
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
            serverSocket.Listen(0);
            //kick off thread to read connecting clients, when one connects, it'll call out AcceptCallback function
            serverSocket.BeginAccept(AcceptCallback, this);
            ChatTextBox.Text += "Server setup complete\n";
        }

        public void CloseAllSockets()
        {
            foreach (ClientSocket clientSocket in clientSockets)
            {
                clientSocket.socket.Shutdown(SocketShutdown.Both);
                clientSocket.socket.Close();
            }
            clientSockets.Clear();
            serverSocket.Close();
        }

        public void AcceptCallback(IAsyncResult AR)
        {
            Socket joiningSocket;

            try
            {
                joiningSocket = serverSocket.EndAccept(AR);
            }
            catch (ObjectDisposedException) // I cannot seem to avoid this (on exit when properly closing sockets)
            {
                return;
            }

            ClientSocket newClientSocket = new ClientSocket();
            newClientSocket.socket = joiningSocket;

            clientSockets.Add(newClientSocket);
            //start a thread to listen out for this new joining socket. Therefore there is a thread open for each client
            joiningSocket.BeginReceive(newClientSocket.buffer, 0, ClientSocket.BUFFER_SIZE, SocketFlags.None, ReceiveCallback, newClientSocket);
            AddToChat("Client connected, waiting for request...");
            newClientSocket.state = ClientState.CHATTING;

            //we finished this accept thread, better kick off another so more people can join
            serverSocket.BeginAccept(AcceptCallback, null);
        }

        public void ReceiveCallback(IAsyncResult AR) // recieve message from the client
        {
            ClientSocket currentClientSocket = (ClientSocket)AR.AsyncState;

            int received;

            try
            {
                received = currentClientSocket.socket.EndReceive(AR);
            }
            catch (SocketException)
            {
                AddToChat("Client forcefully disconnected");
                // Don't shutdown because the socket may be disposed and its disconnected anyway.
                currentClientSocket.socket.Close();
                clientSockets.Remove(currentClientSocket);
                return;
            }
            catch (ObjectDisposedException)
            {
                return; // Socket was disposed elsewhere
            }

            // CHECK FOR DISCONNECT / ZERO BYTES
            if (received == 0)
            {
                AddToChat(currentClientSocket.username + " disconnected.");
                currentClientSocket.socket.Close();
                clientSockets.Remove(currentClientSocket);
                return; // Break the recursion
            }

            byte[] recBuf = new byte[received];
            Array.Copy(currentClientSocket.buffer, recBuf, received);
            string text = Encoding.ASCII.GetString(recBuf);

            AddToChat(currentClientSocket.username + ": " + text); // Add message to server.
            // separate into functions later.

            string[] param = text.ToLower().Split(' '); // Split the text by space.

            switch (param[0])
            {
                case "!user": // Change the username if possible
                    if (param.Length > 1)
                    {
                        string username = param[1];

                        // Check if username is available.

                        bool username_free = true;
                        foreach (var u in clientSockets)
                        {
                            if (u.username == username)
                            {
                                username_free = false;
                                break;
                            }
                        }

                        if (username_free) // Since it's free change the username
                        {
                            SendToAll(currentClientSocket.username + " has changed their username to " + param[1], null);
                            currentClientSocket.username = username; // set user specifed username.
                            byte[] usernameSet = Encoding.ASCII.GetBytes("Username set to: " + username);
                            currentClientSocket.socket.Send(usernameSet);
                        }
                        else  // Send error to client
                        {        
                            byte[] usernameError = Encoding.ASCII.GetBytes("change name denied");
                            currentClientSocket.socket.Send(usernameError);
                        }
                    }
                    else // No username provided prompt the user
                    {
                        byte[] usernameError = Encoding.ASCII.GetBytes("Please provide a username.");
                        currentClientSocket.socket.Send(usernameError);
                    }
                    break;

                    case "!join": // Join the game.
                    // is client in chatting state?
                    if (currentClientSocket.state != ClientState.CHATTING)
                    {
                        AddToChat("Current state is " + currentClientSocket.state);
                        byte[] data9 = Encoding.ASCII.GetBytes("Incorrect Client State");
                        currentClientSocket.socket.Send(data9);
                        break;
                    }

                    // Check if a player position is available?
                    if (player1 != null && player2 != null)
                    {
                        byte[] data10 = Encoding.ASCII.GetBytes("2 players have already joined");
                        currentClientSocket.socket.Send(data10);
                        break;
                    }
                    else if (player1 == null) 
                    {
                        // Join client as player 1
                        player1 = currentClientSocket.username;
                        UpdateClientState(currentClientSocket, ClientState.PLAYING);
                        byte[] data11 = Encoding.ASCII.GetBytes("!player1");
                        currentClientSocket.socket.Send(data11);
                    }

                    else if (player2 == null)
                    {
                        // Join client as player 1
                        player2 = currentClientSocket.username;
                        UpdateClientState(currentClientSocket, ClientState.PLAYING);
                        byte[] data12 = Encoding.ASCII.GetBytes("!player2");
                        currentClientSocket.socket.Send(data12);
                    }

                    // Start the game if we have both players
                    if (player1 != null && player2 != null)
                    {
                        currentTurn = player1;
                        ClearTicTacToe(currentClientSocket);
                        Task.Delay(200).ContinueWith(t => SendToAll("GAME START! " + player1 + "(cross) vs. " + player2 + " (naught).", currentClientSocket));
                        Task.Delay(300).ContinueWith(t => SetPlayerTurn(currentTurn, currentClientSocket));
                    }

                    break;

                case "!scores":

                    // Display sorted scores.
                    var scores = DatabaseAccess.GetScoreInfo();
                    // Combine the list into one block of text and send to all
                    SendToAll(string.Join(Environment.NewLine, scores), null);
                    break;

                case "!move":
                    if (param.Length == 2)
                    {
                        int tile = int.Parse(param[1]);
                        TileType type = TileType.blank;
                        if (currentClientSocket.username.Equals(player1))
                        {
                            type = TileType.cross;
                            AddToChat("Player 1 (cross) attempts move at " + tile + "!");
                        }
                        else if (currentClientSocket.username.Equals(player2))
                        {
                            type = TileType.naught;
                            AddToChat("Player 2 (naught) attempts move at " + tile + "!");
                        }
                        else
                        {
                            // If client attempting move is not a player, abort.
                            break;
                        }
                        bool validmove = ticTacToe.SetTile(tile, type);
                        if (validmove)
                        {
                            // 1. Update the game board.
                            // 2. Send updated board to all clients.
                            string gameboard = ticTacToe.GridToString();
                            SendToAll("!board " + gameboard, currentClientSocket);
                            // 3. Check if game is over...
                            GameState gs = ticTacToe.GetGameState();
                            if (gs == GameState.playing)
                            {
                                // 4. If not over, begin next players turn
                                if (currentTurn.Equals(player1))
                                {
                                    currentTurn = player2;
                                }
                                else
                                {
                                    currentTurn = player1;
                                }
                                Task.Delay(100).ContinueWith(t => SetPlayerTurn(currentTurn, currentClientSocket));
                            }
                            else
                            {
                                if (gs == GameState.crossWins)
                                {
                                    Task.Delay(50).ContinueWith(t => SendToAll("GAME END: " + player1 + " (cross) wins!", currentClientSocket));                   
                                    DatabaseAccess.UserWon(player1);  // Database: cross user won
                                    DatabaseAccess.UserLost(player2); // Database: naught user lost
                                }
                                if (gs == GameState.naughtWins)
                                {
                                    Task.Delay(50).ContinueWith(t => SendToAll("GAME END: " + player2 + " (naught) wins!", currentClientSocket));
                                    DatabaseAccess.UserWon(player2);  // Database: naught user won
                                    DatabaseAccess.UserLost(player1); // Database: cross user lost
                                }
                                if (gs == GameState.draw)
                                {
                                    Task.Delay(50).ContinueWith(t => SendToAll("GAME END: Draw!", currentClientSocket));
                                    // Database: naught user draw
                                    // Database: cross user draw
                                    DatabaseAccess.UsersDraw(player1, player2);
                                }

                                // End the game and return players to chatting state
                                EndTicTacToe();

                                // TODO: Update Database with scores
                            }
                        }
                    }
                    break;
                case "!username": // This is only hit when user presses join button and sets up the username if possible disconnecting the user otherwise.
                    ConnectClient(text, currentClientSocket);
                    break;

                case "!mod": // Tell user only the server can elevate to moderator!
                    SendToTarget("Only the server can elevate to moderator!", currentClientSocket.username, currentClientSocket); // Inform illegal action to client.

                    break;
                case "!kick": // Check to see if the user is a moderator if so kick desired user.

                    if (currentClientSocket.moderator == true)
                    {
                        // Kick the client and Send message.                     
                        foreach (var cs in clientSockets)
                        {
                            if (cs.username == param[1]) // Kick specifed user
                            {
                                SendToTarget("!kick", param[1], null); // Kick client side
                                AddToChat(cs.username + " disconnected");

                                cs.socket.DisconnectAsync(true); // Disconnect Server side
                                cs.socket.Close();
                                cs.socket.Dispose(); // Free up recsources

                                clientSockets.Remove(cs); // Remove from list
                                break;
                            }
                        }
                    }
                    else
                    {
                        byte[] Message = Encoding.ASCII.GetBytes("Only moderators can kick!");
                        currentClientSocket.socket.Send(Message); // Send Kick message
                    }
                    break;

                case "!color":  // Allow user to set their text color.
                    // Send message command to the client with color setting information.
                    if (param.Length >= 2)
                    {
                        currentClientSocket.textColor = param[1]; // set this clients color.             
                        SendToTarget("You set your color to " + param[1], currentClientSocket.username, currentClientSocket);
                    }
                    break;
                case "!commands":
                    byte[] data = Encoding.ASCII.GetBytes("Commands are !user !whisper !who !color !join !mod !mods !kick !about !scores");
                    
                    currentClientSocket.socket.Send(data);
                    AddToChat("Commands sent to client");
                    break;
                case "!exit":
                    // Always Shutdown before closing
                    SendToTarget("!exit", currentClientSocket.username, null); // exit client side
                    AddToChat("Client disconnected");
                    currentClientSocket.socket.DisconnectAsync(true); // Disconnect Server side
                    //currentClientSocket.socket.Shutdown(SocketShutdown.Both); Causes error for some reason
                    currentClientSocket.socket.Close();
                    currentClientSocket.socket.Dispose(); // Free up recsources 
                    clientSockets.Remove(currentClientSocket);
                    
                    return;
                case "!who":

                    // send back messages containing the names of the connected users to the client
                    SendToTarget("Connected Users:", currentClientSocket.username, null);
                    foreach (var socket in clientSockets)
                    {
                        SendToTarget(socket.username, currentClientSocket.username, null);
                    }
                    break;
                case "!about":
                    // send information back to the client about its creator, purpose and year of development
                    SendToTarget("Modified by Andrew Jonas, the purpose of this is to learn networking through fixing a TCP chat program. Created 30/07/2026.", currentClientSocket.username, null);
                    break;
                case "!whisper": //# Send messages to specific users.
                    if (param.Length >= 2)
                    {       // targetUser, message1, message2, message3
                        string targetUser = param[1];
                        int takeFromRight = param.Length - 2;
                        var rightElements = param.Skip(param.Length - takeFromRight);
                        string result = string.Join(" ", rightElements);
                        string message = "[Whisper from " + currentClientSocket.username + ']' + " " + result;

                        SendToTarget(message, targetUser, currentClientSocket); // Send to target user
                        SendToTarget(message, currentClientSocket.username, currentClientSocket); // Send to the calling user
                    }
                    else
                    {
                        // Reply with error.
                    }
                    break;
                case "kill":
                    DisconnectClient(currentClientSocket);
                    break;
                default:
                    // normal message broadcast out to all clients, also send color data.
                    SendToAll(currentClientSocket.username + ": " + text + "#color" + currentClientSocket.textColor, currentClientSocket);
                    break;
            }
            //we just received a message from this socket, better keep an ear out with another thread for the next one
            try
            {
                currentClientSocket.socket.BeginReceive(currentClientSocket.buffer, 0, ClientSocket.BUFFER_SIZE, SocketFlags.None, ReceiveCallback, currentClientSocket);
            }
            catch
            {
                AddToChat("Socket Disposed");
            }
        }

        public void LocalMessage(string str) // handle server related messages
        {
            string[] param = str.ToLower().Split(' ');
            switch (param[0])
            {
                case "!kick": // Kick the client and Send message.             

                    foreach (var cs in clientSockets)
                    {
                        if (cs.username == param[1]) // Kick specifed user
                        {
                            SendToTarget("!kick", param[1], null); // Kick client side
                            AddToChat(cs.username + " Kicked");

                            cs.socket.DisconnectAsync(true); // Disconnect Client side
                            cs.socket.Close();
                            cs.socket.Dispose();

                            clientSockets.Remove(cs);

                            break;
                        }
                    }
                    break;

                case "!mods": // Show a list of mods
                    AddToChat("List of Moderators");
                    foreach (var cs in clientSockets)
                    {
                        if (cs.moderator)
                        {
                            AddToChat(cs.username);
                        }
                    }
                    break;

                case "!mod": // elevate specific user to mod
                    AddToChat("Server designates a moderator.");
                    if (param.Length > 1) // there must be two words, command and username.
                    {
                        string target = param[1];
                        // check if target exists in connected clients
                        ClientSocket targetSocket = GetUsername(target);
                        if (targetSocket == null)
                        {
                            // couldn't find username
                            AddToChat("Target username does not exist");
                        }
                        else
                        {
                            // promote/demote
                            targetSocket.moderator = !targetSocket.moderator; // Toggle targetsocket.
                            if (targetSocket.moderator == true)
                            {
                                // promoted to moderator
                                SendToAll(target + " promoted to moderator!", null);
                            }
                            else
                            {
                                // demoted from moderator
                                SendToAll(target + " no longer a moderator!", null);
                            }
                        }
                    }
                    else
                    {
                        AddToChat("Please specify a username");
                    }
                    break;
                default:
                    SendToAll("SERVER: " + str, null);
                    AddToChat("SERVER: " + str);
                    break;
            }
        }

        public ClientSocket GetUsername(string targetUsername) // If client cannot be found return null
        {
            foreach (ClientSocket c in clientSockets)
            {
                if (c.username.Equals(targetUsername))
                    return c;
            }
            return null;
        }
        public void SendToAll(string str, ClientSocket from)
        {
            foreach (ClientSocket c in clientSockets)
            {
                if (from == null || !from.socket.Equals(c))
                {
                    byte[] data = Encoding.ASCII.GetBytes(str);
                    try
                    {
                        c.socket.Send(data);
                    }
                    catch (ObjectDisposedException) // Object disposed 
                    {
                        AddToChat("socket disposed");
                        break; // if socket is disposed mid iteration then break out of loop.
                    }
                    catch (SocketException) // Handel socket errors
                    {
                        AddToChat("socket error");
                        break; // if other socket errors mid iteration then break out of loop.
                    }
                }
            }
        }

        public void SendToTarget(string str, string target, ClientSocket from)
        {
            foreach (ClientSocket c in clientSockets)
            {
                if (from == null || !from.socket.Equals(c))
                {
                    if (c.username.Equals(target))
                    {
                        byte[] data = Encoding.ASCII.GetBytes(str);
                        c.socket.Send(data);
                        break;
                    }
                }
            }
        }

        public void ConnectClient(string text, ClientSocket currentClientSocket) // Connect the client if the username is free otherwise disconnect.
        {
            if (currentClientSocket.username != "none") // don't proceed if the user has already set their username.
            {
                SendToTarget("Your username is already set", currentClientSocket.username, null);
                return;
            }
            string[] param = text.ToLower().Split(' '); // Split the text by space.
            // chage the username
            if (param.Length > 1) // Fail safe against reading beyond the array.
            {

                string username = param[1];

                // Check if username is available.

                //bool username_free = true;
                //foreach (var u in clientSockets)
                //{
                //    if (u.username == username)
                //    {
                //        username_free = false;
                //        break;
                //    }
                //}

                // Check if username has been registered in this instance

                bool username_free = true;

                foreach (var u in clientSockets)
                {
                    if (u.username == username)
                    {
                        username_free = false;
                        break;
                    }
                }

                currentClientSocket.username = username;
                AddToChat("Setting client " + currentClientSocket.username + " to " + username);

                if (username_free)
                {
                    byte[] usernameSet = Encoding.ASCII.GetBytes("Username set to: " + username);
                    currentClientSocket.socket.Send(usernameSet);
                    SendToTarget("Connected", username, currentClientSocket);
                }
                else
                {
                    // Send error and disconnect the client.
                    byte[] usernameError = Encoding.ASCII.GetBytes("connection denied");
                    currentClientSocket.socket.Send(usernameError);
                    DisconnectClient(currentClientSocket);
                }

            }
            else // No username provided
            {
                byte[] usernameError = Encoding.ASCII.GetBytes("Please provide a username.");
                currentClientSocket.socket.Send(usernameError);
            }
        }
        public void DisconnectClient(ClientSocket currentClientSocket)
        {
            try
            {
                if (currentClientSocket.socket.Connected)
                {
                    currentClientSocket.socket.Shutdown(SocketShutdown.Both);
                }
            }
            catch (Exception) { /* Handle or log if needed */ }
            finally
            {
                currentClientSocket.socket.Close();
                clientSockets.Remove(currentClientSocket);
            }
        }

        public void UpdateClientState(ClientSocket client, ClientState state)
        {
            client.state = state; // ensure client and server state are matched.
            byte[] data8 = Encoding.ASCII.GetBytes("!state " + (int)client.state);
            client.socket.Send(data8);
        }

        public void ClearTicTacToe(ClientSocket currentClientSocket)
        {
            ticTacToe.ResetBoard();
            Task.Delay(25).ContinueWith(t => SendToAll("!board " + ticTacToe.GridToString(), currentClientSocket));
        }
        public void SetPlayerTurn(string username, ClientSocket currentClientSocket)
        {
            SendToTarget("!yourturn", username, currentClientSocket);
        }
        public void EndTicTacToe()
        {
            foreach (ClientSocket c in clientSockets)
            {
                if (c.username.Equals(player1) || c.username.Equals(player2))
                {
                    UpdateClientState(c, ClientState.CHATTING);
                }
            }
            currentTurn = null;
            player1 = null;
            player2 = null;
        }
    }
}
