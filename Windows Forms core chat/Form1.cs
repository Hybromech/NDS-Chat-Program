using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net; 
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

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
            //# Enable the form to catch key events before controls do
            this.KeyPreview = true;
            // Explicitly link the KeyDown event to the method
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
        }

        //# Setup Key press enter to send chat
        //# Silence the default ding sound
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // Bind the key 'enter' to send button
            if (e.KeyCode == Keys.Enter)
            {
                SendButton.PerformClick();
                // THIS stops the default beep in KeyDown
                e.SuppressKeyPress = true;
                e.Handled = true; //# Prevents the key from triggering other OS/control events
            }
        }
        private void PlayCustomSound()
        {
            try
            {
                 // Play a custom .wav file
                 // make the path relative!.
                 using (SoundPlayer player = new SoundPlayer("G:\\Andrew Adata\\Bachelor of Software Engineering\\Networking and Database\\A2\\GitHub\\NDS-Chat-Program\\soundAssets\\message.wav"))
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
                }
                catch (Exception ex)
                {
                    ChatTextBox.Text += "Error: " + ex ;
                    ChatTextBox.AppendText(Environment.NewLine);
                }
            }

        }

        private void JoinButton_Click(object sender, EventArgs e)
        {
            if (!CanHostOrJoin()) return;

            // 1. Validate username FIRST before spinning up network code
            if (string.IsNullOrEmpty(UsernameTextbox.Text))
            {
                // Guard against client being null if this is the first run
                if (client != null)
                    client.AddToChat("Error can't join, Username not specified!");
                else
                    ChatTextBox.AppendText("Error can't join, Username not specified!" + Environment.NewLine);

                return;
            }

            try
            {
                // 2. Disconnect/Reset any existing singleton instance so a clean one can form
                // (You may need to add a Reset or Dispose method to your TCPChatClient class)
                // TCPChatClient.ResetInstance(); 

                int port = int.Parse(MyPortTextBox.Text);
                int serverPort = int.Parse(serverPortTextBox.Text);

                client = TCPChatClient.CreateInstance(port, serverPort, ServerIPTextBox.Text, ChatTextBox);

                if (client == null)
                    throw new Exception("Incorrect port value or client instance failed!");

                // 3. Connect and update UI
                client.ConnectToServer();
                this.Text = "Client " + UsernameTextbox.Text;

                // 4. Move this safely INSIDE the try block so it only runs on a valid connection
                string username = UsernameTextbox.Text;
                client.SendString("!username " + username);
            }
            catch (Exception ex)
            {
                client = null;
                // Best practice: Use AppendText so it automatically scrolls to the bottom
                ChatTextBox.AppendText("Error: " + ex.Message + Environment.NewLine);
            }
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            if (client != null)
                client.SendString(TypeTextBox.Text);
            else if (server != null)
                server.SendToAll(TypeTextBox.Text, null);

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
    }
}
