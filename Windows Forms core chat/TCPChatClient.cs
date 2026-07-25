 using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

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
        public Color textColor = Color.Black;

        public static TCPChatClient CreateInstance(int port, int serverPort, string serverIP, RichTextBox chatTextBox)
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
            byte[] buffer = Encoding.ASCII.GetBytes(text);
            socket.Send(buffer, 0, buffer.Length, SocketFlags.None);
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
            //read bytes from packet
            byte[] recBuf = new byte[received];
            Array.Copy(currentClientSocket.buffer, recBuf, received);
            //convert to string so we can work with it
            string text = Encoding.ASCII.GetString(recBuf);
            Console.WriteLine("Received Text: " + text);

            // reject duplicate username
            if (text == "That username is taken.")
            {
                AddToChat("\"You have been rejected that username is taken!");
                clientSocket.connectionLost = true;
                socket.DisconnectAsync(true);
                socket.Close();
                return;
            }
            // check to see if the client is trying to change text color.
            if (text.Contains("color"))
            {
                var targetColor = text.Replace("!color", ""); // remove color substring from the text.
                textColor = Color.FromName(targetColor); // set this clients color.
            }
            //text is from server but could have been broadcast from the other clients
            // add text with users color
            AddToChat( text, textColor );
            //we just received a message from this socket, better keep an ear out with another thread for the next one
            currentClientSocket.socket.BeginReceive(currentClientSocket.buffer, 0, ClientSocket.BUFFER_SIZE, SocketFlags.None, ReceiveCallback, currentClientSocket);
        }
        public void Close()
        {
            socket.Close();
        }
    }

}
