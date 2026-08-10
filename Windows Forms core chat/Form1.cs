using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net; 
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Windows_Forms_CORE_CHAT_UGH;

//https://www.youtube.com/watch?v=xgLRe7QV6QI&ab_channel=HazardEditHazardEdit
namespace Windows_Forms_Chat
{
    public partial class Form1 : Form
    {
        TicTacToe ticTacToe = new TicTacToe();
        TCPChatServer server = null;
        TCPChatClient client = null;

        public Form1()
        {
            InitializeComponent();
            // Enable the form to catch key events before controls do
            this.KeyPreview = true;
            // Explicitly link the KeyDown event to the method
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
        }

        // Setup Key press enter to send chat
        // Silence the default ding sound
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // Bind the key 'enter' to send button
            if (e.KeyCode == Keys.Enter)
            {
                SendButton.PerformClick();
                // stops the default beep in KeyDown
                e.SuppressKeyPress = true;
                e.Handled = true; //# Prevents the key from triggering other OS/control events
            }
        }
        private void PlayCustomSound()
        {
            try
            {
                // Play a custom .wav file
                string path = Path.Combine(AppContext.BaseDirectory, "soundAssets", "message.wav");
                using (SoundPlayer player = new SoundPlayer(path))
                {
                    player.Play();
                }
            }
            catch (Exception ex)
            {
                // Handle file missing or audio errors silently
                Console.WriteLine("missing or audio errors");
            }
        }
        public bool CanHostOrJoin()
        {
            if (server == null && client == null)
                return true;
            else
                return false;
        }

        private void HostButton_Click(object sender, EventArgs e)
        {
            if (CanHostOrJoin())
            {
                try
                {
                    int port = int.Parse(MyPortTextBox.Text);
                    server = TCPChatServer.createInstance(port, ChatTextBox);
                    //oh no, errors
                    if (server == null)
                        throw new Exception("Incorrect port value!");//thrown exceptions should exit the try and land in next catch

                    server.SetupServer();
                    this.Text = "Server " + server.port.ToString(); // Modify the form title to Server + port number.
                    DatabaseAccess.Database_Connect(); // open a database connection and create a Users table if it does not exist.
                }
                catch (Exception ex)
                {
                    ChatTextBox.Text += "Error: " + ex;
                    ChatTextBox.AppendText(Environment.NewLine);
                }
            }

        }

        private void JoinButton_Click(object sender, EventArgs e)
        {
            if (client != null)
            {
                if (client.clientSocket.connectionLost == true)
                    client = null;
            }
            if (CanHostOrJoin() == false)
            {
                return;
            }

            // Validate username FIRST before spinning up network code

            if (string.IsNullOrEmpty(UsernameTextbox.Text) || string.IsNullOrEmpty(PasswordTextbox.Text))
            {
                // Guard against client being null if this is the first run
                if (client != null)
                    client.AddToChat("Error can't join, Username or Password not specified!");
                else
                    ChatTextBox.AppendText("Error can't join, Username not specified!" + Environment.NewLine);

                return;
            }

            try
            {
                // Disconnect/Reset any existing singleton instance so a clean one can form

                int port = int.Parse(MyPortTextBox.Text);
                int serverPort = int.Parse(serverPortTextBox.Text);

                client = TCPChatClient.CreateInstance(port, serverPort, ServerIPTextBox.Text, ChatTextBox);

                if (client == null)
                    throw new Exception("Incorrect port value or client instance failed!");
      
                // Connect and update UI
                client.ConnectToServer();
                this.Text = "Client " + UsernameTextbox.Text;

                // Move this safely INSIDE the try block so it only runs on a valid connection
                string username = UsernameTextbox.Text;
                string password = PasswordTextbox.Text;
                // --- This needs to change to check for subscribed users in the database ---.
                
                // Check if user exists, if not then add the user with credentials.
                if (DatabaseAccess.DoesUserExist(username, password) == false)
                {
                    client.SendString("Adding user " + username);
                    DatabaseAccess.AddUser(username, password);
                    client.SendString("!username " + username); // set username if its available else disconnect
                }
                else {
                   
                    client.SendString("Username or password already in use");
                    client.SendString("!username " + username); // set username if its available else disconnect
                }
            }
            catch (Exception ex)
            {
                client = null;
                // Use AppendText so it automatically scrolls to the bottom
                ChatTextBox.AppendText("Error: " + ex.Message + Environment.NewLine);
            }
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            if (client != null && client.clientSocket.connectionLost == false)
                client.SendString(TypeTextBox.Text);

            else if (server != null)
                server.LocalMessage(TypeTextBox.Text);

            PlayCustomSound();  // Play custom sound

            TypeTextBox.Clear();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //On form loaded
            ticTacToe.buttons.Add(button1);
            ticTacToe.buttons.Add(button2);
            ticTacToe.buttons.Add(button3);
            ticTacToe.buttons.Add(button4);
            ticTacToe.buttons.Add(button5);
            ticTacToe.buttons.Add(button6);
            ticTacToe.buttons.Add(button7);
            ticTacToe.buttons.Add(button8);
            ticTacToe.buttons.Add(button9);
        }

        private void AttemptMove(int i)
        {
            if (ticTacToe.myTurn)
            {
                bool validMove = ticTacToe.SetTile(i, ticTacToe.playerTileType);
                if (validMove)
                {
                    //tell server about it
                    //ticTacToe.myTurn = false;//call this too when ready with server
                }
                //example, do something similar from server
                GameState gs = ticTacToe.GetGameState();
                if (gs == GameState.crossWins)
                {
                    ChatTextBox.AppendText("X wins!");
                    ChatTextBox.AppendText(Environment.NewLine);
                    ticTacToe.ResetBoard();
                }
                if (gs == GameState.naughtWins)
                {
                    ChatTextBox.AppendText(") wins!");
                    ChatTextBox.AppendText(Environment.NewLine);
                    ticTacToe.ResetBoard();
                }
                if (gs == GameState.draw)
                {
                    ChatTextBox.AppendText("Draw!");
                    ChatTextBox.AppendText(Environment.NewLine);
                    ticTacToe.ResetBoard();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AttemptMove(0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AttemptMove(1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AttemptMove(2);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AttemptMove(3);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AttemptMove(4);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            AttemptMove(5);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            AttemptMove(6);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            AttemptMove(7);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            AttemptMove(8);
        }

        private void Disconnect_Click(object sender, EventArgs e)
        {

        }
        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
