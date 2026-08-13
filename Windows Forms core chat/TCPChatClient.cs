 using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

//reference: https://github.com/AbleOpus/NetworkingSamples/blob/master/MultiClient/Program.cs
namespace Windows_Forms_Chat
{
    public class TCPChatClient : TCPChatBase
    {
        //public static TCPChatClient tcpChatClient;
        public Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public ClientSocket clientSocket = new ClientSocket();


        public int serverPort;
        public string serverIP;
        public ClientState myState = ClientState.LOGIN;
        
        public Color inColor = Color.Black;

        public static TCPChatClient CreateInstance(int port, int serverPort, string serverIP, RichTextBox chatTextBox, TicTacToe ttt)
        {
            TCPChatClient tcp = null;
            //if port values are valid and ip worth attempting to join
            if (port > 0 && port < 65535 && 
                serverPort > 0 && serverPort < 65535 && 
                serverIP.Length > 0 &&
                chatTextBox != null)
            {
                tcp = new TCPChatClient();
                tcp.port = port;
                tcp.serverPort = serverPort;
                tcp.serverIP = serverIP;
                tcp.ChatTextBox = chatTextBox;
                tcp.clientSocket.socket = tcp.socket;
                tcp.ticTacToe = ttt;

            }

            return tcp;
        }

        public void ConnectToServer()
        {
            int attempts = 0;
            
            while (!socket.Connected)
            {
                try 
                {
                    attempts++;
                    SetChat("Connection attempt " + attempts);
                    // Change IPAddress.Loopback to a remote IP to connect to a remote host.
                    myState = ClientState.CHATTING;
                    clientSocket.state = ClientState.CHATTING;
                    socket.Connect(serverIP, serverPort);                  
                }
                catch (SocketException)
                {
                    ChatTextBox.Text = "";
                }
            }

            //Console.Clear();
            //keep open thread for receiving data
            clientSocket.socket.BeginReceive(clientSocket.buffer, 0, ClientSocket.BUFFER_SIZE, SocketFlags.None, ReceiveCallback, clientSocket);
        }

        public void SendString(string text)
        {
            if (socket != null && socket.IsBound) // can't send if disposed
            {
                byte[] buffer = Encoding.ASCII.GetBytes(text);
                socket.Send(buffer, 0, buffer.Length, SocketFlags.None);
            }
        }

        public void ReceiveCallback(IAsyncResult AR) // recieve message from the server
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
                return;
            }

            // GUARD TO STOP INFINITE 0-BYTE RECURSION LOOPS:
            if (received == 0)
            {
                currentClientSocket.socket.Close();
                return;
            }

            //read bytes from packet
            byte[] recBuf = new byte[received];
            Array.Copy(currentClientSocket.buffer, recBuf, received);
            //convert to string so we can work with it
            string text = Encoding.ASCII.GetString(recBuf);
            Console.WriteLine("Received Text: " + text);

            string[] param = text.ToLower().Split(' ');

            switch (param[0])
            {
                case "!state":
                    if (param[1].Equals("0"))
                    {
                        myState = ClientState.LOGIN;
                    }
                    else if (param[1].Equals("1")) {
                        myState = ClientState.CHATTING;
                    }
                    else if (param[1].Equals("2"))
                    {
                        myState = ClientState.PLAYING;
                    }
                    AddToChat("State Updated to: " + myState.ToString());
                    break;

                case "!player1":
                    AddToChat("Joined Tic-Tac-Toe as Player 1 (cross)");
                    ticTacToe.playerTileType = TileType.cross;
                    ticTacToe.playerName = "Player1";
                    break;

                case "!player2":
                    AddToChat("Joined Tic-Tac-Toe as Player 2 (naught)");
                    ticTacToe.playerTileType = TileType.naught;
                    ticTacToe.playerName = "Player2";
                    break;
                case "!yourturn":
                    AddToChat("It is your turn " + ticTacToe.playerName);
                    ticTacToe.myTurn = true;
                    break;
                case "!otherturn":
                    AddToChat("It is the Opponents turn.");
                    break;
                case "!board":
                    string boardstate = param[1];
                    AddToChat("Board Update: " + boardstate);
                    ticTacToe.StringToGrid(boardstate);
                    break;
            }

            if (text == "!mod")
            {
                AddToChat("\"You are promoted to moderator!");
                currentClientSocket.moderator = true;
            }
     
            if (text == "!kick")  // Kick the user
            {
                AddToChat("\"You have been kicked!");
                clientSocket.connectionLost = true;
                socket.DisconnectAsync(true); // Disconnect Client side
                socket.Close();
                socket.Dispose();
                return;
            }

            if (text == "!exit")
            {
                AddToChat("\"You have disconnected");
                clientSocket.connectionLost = true;
                socket.DisconnectAsync(true); // Disconnect Client side
                socket.Close();
                socket.Dispose();
                return;
            }

            // reject duplicate username on connect
            if (text == "connection denied")
            {
                AddToChat("\"You have been rejected that username is taken!");
                clientSocket.connectionLost = true;
                socket.DisconnectAsync(true); // Disconnect Client side
                socket.Close();
                socket.Dispose();
                return;
            }
            // reject duplicate username on change, don't disconnect
            if (text == "change name denied")
            {
                AddToChat("\"that username is taken!");
            }
            // remove color data from text but use it to set inColor

            if (text.Contains("#color"))
            {
                int index = text.LastIndexOf("#color");

                string colInfo = text.Substring(index);
                var targetColor = colInfo.Replace("#color", "").Trim();

                // Only set color and strip if a valid color string was appended (e.g. !colorBlack)
                if (!string.IsNullOrEmpty(targetColor))
                {
                    inColor = Color.FromName(targetColor);
                    text = text.Substring(0, index); // Safely strip trailing color tag
                }
            }
            else
            {
                inColor = Color.Black; // Defaults to black.
            }
            //text is from server but could have been broadcast from the other clients
            // add text with users color
            AddToChat( text, inColor);
            //we just received a message from this socket, better keep an ear out with another thread for the next one
            currentClientSocket.socket.BeginReceive(currentClientSocket.buffer, 0, ClientSocket.BUFFER_SIZE, SocketFlags.None, ReceiveCallback, currentClientSocket);
        }

        public void SendMoveAttemptToServer(int i)
        {
            SendString("!move " + i.ToString());
        }
        public void Close()
        {
            socket.Close();
        }
    }

}
