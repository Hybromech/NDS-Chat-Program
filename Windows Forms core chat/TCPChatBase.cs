using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Configuration;

namespace Windows_Forms_Chat
{

    public class TCPChatBase
    {
        public RichTextBox ChatTextBox;
        public int port;
        public void SetChat(string str)
        {
            ChatTextBox.Invoke((Action)delegate
            {
                ChatTextBox.Text = str;
                ChatTextBox.AppendText(Environment.NewLine);
            });
        }
        public void AddToChat(string str)
        {
            //dumb https://iandotnet.wordpress.com/tag/multithreading-how-to-update-textbox-on-gui-from-another-thread/
            ChatTextBox.Invoke((Action)delegate
            {
                ChatTextBox.AppendText(str);
                ChatTextBox.AppendText(Environment.NewLine);
            });
        }
        public void AddToChat(string str, Color color)
        {
            ChatTextBox.Invoke((Action)delegate
            {
                // Move selection cursor to the very end
                ChatTextBox.SelectionStart = ChatTextBox.TextLength;
                ChatTextBox.SelectionLength = 0;

                // Set the color for ONLY the next appended text
                ChatTextBox.SelectionColor = color;

                // Append the message and newline
                ChatTextBox.AppendText(str + Environment.NewLine);

                // Auto-scroll to the bottom of the chat
                ChatTextBox.ScrollToCaret();
            });
        }
    }
}
