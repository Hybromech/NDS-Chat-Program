using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
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

        public static TCPChatServer createInstance(int port, RichTextBox chatTextBox)
        {
            TCPChatServer tcp = null;
            //setup if port within range and valid chat box given
            if (port > 0 && port < 65535 && chatTextBox != null)
            {
                tcp = new TCPChatServer();
                tcp.port = port;
                tcp.ChatTextBox = chatTextBox;

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
                    byte[] data = Encoding.ASCII.GetBytes("Commands are !user !mod !mods !kick !exit !who !about !whisper !color");
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

                        SendToTarget(message, targetUser, currentClientSocket);

                    }
                    else
                    {
                        // Reply with error.
                    }
                    break;
                default:
                    // normal message broadcast out to all clients, also send color data.
                    SendToAll(currentClientSocket.username + ": " + text + "!color" + currentClientSocket.textColor, currentClientSocket);
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

                // Check if username has been registered

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
                    currentClientSocket.socket.DisconnectAsync(true); // creates stack overflow for some reason!
                    currentClientSocket.socket.Close();
                    currentClientSocket.socket.Dispose();
                }

            }
            else // No username provided
            {
                byte[] usernameError = Encoding.ASCII.GetBytes("Please provide a username.");
                currentClientSocket.socket.Send(usernameError);
            }
        }
    }
}
