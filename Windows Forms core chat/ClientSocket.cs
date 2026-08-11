using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Sockets;
using System.Text;

namespace Windows_Forms_Chat
{
    public enum ClientState
    { 
        LOGIN,
        CHATTING,
        PLAYING
    }
    public class ClientSocket
    {
        //add other attributes to this, e.g username, what state the client is in etc
        public ClientState state = ClientState.LOGIN;
        public string username = "none"; // stores this clients name.
        public string textColor = "Black";
        public bool moderator;
        public bool connectionLost = false;
        public Socket socket;
        public const int BUFFER_SIZE = 2048;
        public byte[] buffer = new byte[BUFFER_SIZE];
    }
}
