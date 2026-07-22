using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

//https://github.com/AbleOpus/NetworkingSamples/blob/master/MultiServer/Program.cs
namespace Windows_Forms_Chat
{
    public class TCPChatServer : TCPChatBase
    {
        
        public Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //connected clients
        public List<ClientSocket> clientSockets = new List<ClientSocket>();

        public static TCPChatServer createInstance(int port, TextBox chatTextBox)
        {
            TCPChatServer tcp = null;
            //setup if port within range and valid chat box given
            if (port > 0 && port < 65535 && chatTextBox != null)
            {
                tcp = new TCPChatServer();
                tcp.port = port;
                tcp.chatTextBox = chatTextBox;

            }

            //return empty if user not enter useful details
            return tcp;
        }

        public void SetupServer()
        {
            chatTextBox.Text += "Setting up server...\n";
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
            serverSocket.Listen(0);
            //kick off thread to read connecting clients, when one connects, it'll call out AcceptCallback function
            serverSocket.BeginAccept(AcceptCallback, this);
            chatTextBox.Text += "Server setup complete\n";
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
                case "!user":
                    if (param.Length > 1)
                    {
                        // change the username
                        SendToAll(currentClientSocket.username + " has changed their username to " + param[1],null);
                        currentClientSocket.username = param[1]; // set user specifed username.
                    }
                    break;
                       
                case "!mod":
                    // Only the server can elevate to moderator!
                    SendToTarget("Only the server can elevate to moderator!", currentClientSocket.username, currentClientSocket); // Infrom illegal action to client.

                    break;
                case "!kick":
                    //
                    break;
                case "!username":
                    if (param.Length > 1) // Fail safe against reading beyond the array.
                    {
                        
                        string username = param[1];

                        // Check if username is available.

                        bool username_free = true;
                        foreach (var u in clientSockets)
                        {
                            if (u.username == username)
                                username_free = false;
                            break;
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
                            byte[] usernameError = Encoding.ASCII.GetBytes("That username is taken.");                    
                            currentClientSocket.socket.Send(usernameError);
                            currentClientSocket.socket.DisconnectAsync(true); // creates stack overflow for some reason!
                        }
                        
                    }
                    else // No username provided
                    {
                        byte[] usernameError = Encoding.ASCII.GetBytes("Please provide a username.");
                        currentClientSocket.socket.Send(usernameError);
                    }
                    break;
                case "!commands":
                    byte[] data = Encoding.ASCII.GetBytes("Commands are !commands !about !who !whisper !exit");
                    currentClientSocket.socket.Send(data);
                    AddToChat("Commands sent to client");
                    break;
                case "!exit":
                    // Always Shutdown before closing
                    currentClientSocket.socket.Shutdown(SocketShutdown.Both);
                    currentClientSocket.socket.Close();
                    clientSockets.Remove(currentClientSocket);
                    AddToChat("Client disconnected");
                    return;
                case "!who":
                    break;
                case "!about":
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
                    //normal message broadcast out to all clients
                    SendToAll(currentClientSocket.username + ": " + text, currentClientSocket);
                    break;
            }
            //we just received a message from this socket, better keep an ear out with another thread for the next one
            currentClientSocket.socket.BeginReceive(currentClientSocket.buffer, 0, ClientSocket.BUFFER_SIZE, SocketFlags.None, ReceiveCallback, currentClientSocket);
        }

        public void LocalMessage(string str)
        {
            string[] param = str.ToLower().Split(' ');
            switch (param[0])
            {
                case "!mod":
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
            foreach(ClientSocket c in clientSockets)
            {
                if(from == null || !from.socket.Equals(c))
                {
                    byte[] data = Encoding.ASCII.GetBytes(str);
                    c.socket.Send(data);
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
        
    }
}
