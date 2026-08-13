namespace Windows_Forms_Chat
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new System.Windows.Forms.Label();
            MyPortTextBox = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            serverPortTextBox = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            ServerIPTextBox = new System.Windows.Forms.TextBox();
            TypeTextBox = new System.Windows.Forms.TextBox();
            label4 = new System.Windows.Forms.Label();
            HostButton = new System.Windows.Forms.Button();
            JoinButton = new System.Windows.Forms.Button();
            SendButton = new System.Windows.Forms.Button();
            label5 = new System.Windows.Forms.Label();
            button1 = new System.Windows.Forms.Button();
            button2 = new System.Windows.Forms.Button();
            button3 = new System.Windows.Forms.Button();
            button4 = new System.Windows.Forms.Button();
            button5 = new System.Windows.Forms.Button();
            button6 = new System.Windows.Forms.Button();
            button7 = new System.Windows.Forms.Button();
            button8 = new System.Windows.Forms.Button();
            button9 = new System.Windows.Forms.Button();
            UsernameTextbox = new System.Windows.Forms.TextBox();
            label6 = new System.Windows.Forms.Label();
            ChatTextBox = new System.Windows.Forms.RichTextBox();
            PasswordTextbox = new System.Windows.Forms.TextBox();
            label7 = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Comic Sans MS", 14.25F);
            label1.Location = new System.Drawing.Point(17, 17);
            label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(82, 26);
            label1.TabIndex = 0;
            label1.Text = "My Port";
            // 
            // MyPortTextBox
            // 
            MyPortTextBox.Location = new System.Drawing.Point(17, 47);
            MyPortTextBox.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            MyPortTextBox.Name = "MyPortTextBox";
            MyPortTextBox.Size = new System.Drawing.Size(171, 33);
            MyPortTextBox.TabIndex = 1;
            MyPortTextBox.Text = "6666";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Comic Sans MS", 14.25F);
            label2.Location = new System.Drawing.Point(299, 15);
            label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(115, 26);
            label2.TabIndex = 2;
            label2.Text = "Server Port";
            // 
            // serverPortTextBox
            // 
            serverPortTextBox.Location = new System.Drawing.Point(299, 45);
            serverPortTextBox.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            serverPortTextBox.Name = "serverPortTextBox";
            serverPortTextBox.Size = new System.Drawing.Size(171, 33);
            serverPortTextBox.TabIndex = 3;
            serverPortTextBox.Text = "6666";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new System.Drawing.Font("Comic Sans MS", 14.25F);
            label3.Location = new System.Drawing.Point(630, 13);
            label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(94, 26);
            label3.TabIndex = 4;
            label3.Text = "server IP";
            // 
            // ServerIPTextBox
            // 
            ServerIPTextBox.Location = new System.Drawing.Point(630, 43);
            ServerIPTextBox.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            ServerIPTextBox.Name = "ServerIPTextBox";
            ServerIPTextBox.Size = new System.Drawing.Size(218, 33);
            ServerIPTextBox.TabIndex = 5;
            ServerIPTextBox.Text = "127.0.0.1";
            // 
            // TypeTextBox
            // 
            TypeTextBox.Location = new System.Drawing.Point(79, 389);
            TypeTextBox.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            TypeTextBox.Name = "TypeTextBox";
            TypeTextBox.Size = new System.Drawing.Size(626, 33);
            TypeTextBox.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new System.Drawing.Font("Comic Sans MS", 14.25F);
            label4.Location = new System.Drawing.Point(14, 390);
            label4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(57, 26);
            label4.TabIndex = 8;
            label4.Text = "Chat:";
            // 
            // HostButton
            // 
            HostButton.Cursor = System.Windows.Forms.Cursors.Hand;
            HostButton.Font = new System.Drawing.Font("Comic Sans MS", 14.25F);
            HostButton.Location = new System.Drawing.Point(17, 113);
            HostButton.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            HostButton.Name = "HostButton";
            HostButton.Size = new System.Drawing.Size(130, 37);
            HostButton.TabIndex = 9;
            HostButton.Text = "Host Server";
            HostButton.UseVisualStyleBackColor = true;
            HostButton.Click += HostButton_Click;
            // 
            // JoinButton
            // 
            JoinButton.Cursor = System.Windows.Forms.Cursors.Hand;
            JoinButton.Font = new System.Drawing.Font("Comic Sans MS", 14.25F);
            JoinButton.Location = new System.Drawing.Point(299, 112);
            JoinButton.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            JoinButton.Name = "JoinButton";
            JoinButton.Size = new System.Drawing.Size(130, 37);
            JoinButton.TabIndex = 10;
            JoinButton.Text = "Join Server";
            JoinButton.UseVisualStyleBackColor = true;
            JoinButton.Click += JoinButton_Click;
            // 
            // SendButton
            // 
            SendButton.Cursor = System.Windows.Forms.Cursors.Hand;
            SendButton.Font = new System.Drawing.Font("Comic Sans MS", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            SendButton.Location = new System.Drawing.Point(718, 387);
            SendButton.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            SendButton.Name = "SendButton";
            SendButton.Size = new System.Drawing.Size(130, 37);
            SendButton.TabIndex = 11;
            SendButton.Text = "Send";
            SendButton.UseVisualStyleBackColor = true;
            SendButton.Click += SendButton_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new System.Drawing.Font("Comic Sans MS", 14.25F);
            label5.Location = new System.Drawing.Point(225, 45);
            label5.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(39, 26);
            label5.TabIndex = 12;
            label5.Text = "OR";
            // 
            // button1
            // 
            button1.AllowDrop = true;
            button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            button1.AutoSize = true;
            button1.BackColor = System.Drawing.Color.LightSteelBlue;
            button1.Cursor = System.Windows.Forms.Cursors.Hand;
            button1.Font = new System.Drawing.Font("Segoe UI", 19F);
            button1.Location = new System.Drawing.Point(855, 160);
            button1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(79, 83);
            button1.TabIndex = 13;
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.AllowDrop = true;
            button2.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            button2.AutoSize = true;
            button2.BackColor = System.Drawing.Color.LightSteelBlue;
            button2.Cursor = System.Windows.Forms.Cursors.Hand;
            button2.Font = new System.Drawing.Font("Segoe UI", 19F);
            button2.Location = new System.Drawing.Point(942, 160);
            button2.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(79, 83);
            button2.TabIndex = 13;
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.AllowDrop = true;
            button3.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            button3.AutoSize = true;
            button3.BackColor = System.Drawing.Color.LightSteelBlue;
            button3.Cursor = System.Windows.Forms.Cursors.Hand;
            button3.Font = new System.Drawing.Font("Segoe UI", 19F);
            button3.Location = new System.Drawing.Point(1030, 160);
            button3.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(79, 83);
            button3.TabIndex = 13;
            button3.UseVisualStyleBackColor = false;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.AllowDrop = true;
            button4.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            button4.AutoSize = true;
            button4.BackColor = System.Drawing.Color.LightSteelBlue;
            button4.Cursor = System.Windows.Forms.Cursors.Hand;
            button4.Font = new System.Drawing.Font("Segoe UI", 19F);
            button4.Location = new System.Drawing.Point(855, 250);
            button4.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            button4.Name = "button4";
            button4.Size = new System.Drawing.Size(79, 83);
            button4.TabIndex = 13;
            button4.UseVisualStyleBackColor = false;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.AllowDrop = true;
            button5.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            button5.AutoSize = true;
            button5.BackColor = System.Drawing.Color.LightSteelBlue;
            button5.Cursor = System.Windows.Forms.Cursors.Hand;
            button5.Font = new System.Drawing.Font("Segoe UI", 19F);
            button5.Location = new System.Drawing.Point(942, 250);
            button5.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            button5.Name = "button5";
            button5.Size = new System.Drawing.Size(79, 83);
            button5.TabIndex = 13;
            button5.UseVisualStyleBackColor = false;
            button5.Click += button5_Click;
            // 
            // button6
            // 
            button6.AllowDrop = true;
            button6.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            button6.AutoSize = true;
            button6.BackColor = System.Drawing.Color.LightSteelBlue;
            button6.Cursor = System.Windows.Forms.Cursors.Hand;
            button6.Font = new System.Drawing.Font("Segoe UI", 19F);
            button6.Location = new System.Drawing.Point(1030, 250);
            button6.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            button6.Name = "button6";
            button6.Size = new System.Drawing.Size(79, 83);
            button6.TabIndex = 13;
            button6.UseVisualStyleBackColor = false;
            button6.Click += button6_Click;
            // 
            // button7
            // 
            button7.AllowDrop = true;
            button7.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            button7.AutoSize = true;
            button7.BackColor = System.Drawing.Color.LightSteelBlue;
            button7.Cursor = System.Windows.Forms.Cursors.Hand;
            button7.Font = new System.Drawing.Font("Segoe UI", 19F);
            button7.Location = new System.Drawing.Point(855, 340);
            button7.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            button7.Name = "button7";
            button7.Size = new System.Drawing.Size(79, 83);
            button7.TabIndex = 13;
            button7.UseVisualStyleBackColor = false;
            button7.Click += button7_Click;
            // 
            // button8
            // 
            button8.AllowDrop = true;
            button8.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            button8.AutoSize = true;
            button8.BackColor = System.Drawing.Color.LightSteelBlue;
            button8.Cursor = System.Windows.Forms.Cursors.Hand;
            button8.Font = new System.Drawing.Font("Segoe UI", 19F);
            button8.Location = new System.Drawing.Point(942, 340);
            button8.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            button8.Name = "button8";
            button8.Size = new System.Drawing.Size(79, 83);
            button8.TabIndex = 13;
            button8.UseVisualStyleBackColor = false;
            button8.Click += button8_Click;
            // 
            // button9
            // 
            button9.AllowDrop = true;
            button9.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            button9.AutoSize = true;
            button9.BackColor = System.Drawing.Color.LightSteelBlue;
            button9.Cursor = System.Windows.Forms.Cursors.Hand;
            button9.Font = new System.Drawing.Font("Segoe UI", 19F);
            button9.Location = new System.Drawing.Point(1030, 340);
            button9.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            button9.Name = "button9";
            button9.Size = new System.Drawing.Size(79, 83);
            button9.TabIndex = 13;
            button9.UseVisualStyleBackColor = false;
            button9.Click += button9_Click;
            // 
            // UsernameTextbox
            // 
            UsernameTextbox.Location = new System.Drawing.Point(630, 110);
            UsernameTextbox.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            UsernameTextbox.Name = "UsernameTextbox";
            UsernameTextbox.Size = new System.Drawing.Size(218, 33);
            UsernameTextbox.TabIndex = 15;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new System.Drawing.Font("Comic Sans MS", 14.25F);
            label6.Location = new System.Drawing.Point(630, 83);
            label6.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(99, 26);
            label6.TabIndex = 16;
            label6.Text = "Username";
            // 
            // ChatTextBox
            // 
            ChatTextBox.Location = new System.Drawing.Point(17, 157);
            ChatTextBox.Name = "ChatTextBox";
            ChatTextBox.Size = new System.Drawing.Size(831, 224);
            ChatTextBox.TabIndex = 17;
            ChatTextBox.Text = "";
            // 
            // PasswordTextbox
            // 
            PasswordTextbox.Location = new System.Drawing.Point(451, 110);
            PasswordTextbox.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            PasswordTextbox.Name = "PasswordTextbox";
            PasswordTextbox.Size = new System.Drawing.Size(167, 33);
            PasswordTextbox.TabIndex = 18;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new System.Drawing.Font("Comic Sans MS", 14.25F);
            label7.Location = new System.Drawing.Point(451, 82);
            label7.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(94, 26);
            label7.TabIndex = 19;
            label7.Text = "Password";
            label7.Click += label7_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.DodgerBlue;
            ClientSize = new System.Drawing.Size(1122, 428);
            Controls.Add(label7);
            Controls.Add(PasswordTextbox);
            Controls.Add(ChatTextBox);
            Controls.Add(label6);
            Controls.Add(UsernameTextbox);
            Controls.Add(button9);
            Controls.Add(button8);
            Controls.Add(button7);
            Controls.Add(button6);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label5);
            Controls.Add(SendButton);
            Controls.Add(JoinButton);
            Controls.Add(HostButton);
            Controls.Add(label4);
            Controls.Add(TypeTextBox);
            Controls.Add(ServerIPTextBox);
            Controls.Add(label3);
            Controls.Add(serverPortTextBox);
            Controls.Add(label2);
            Controls.Add(MyPortTextBox);
            Controls.Add(label1);
            Cursor = System.Windows.Forms.Cursors.PanNW;
            Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox MyPortTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox serverPortTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox ServerIPTextBox;
        private System.Windows.Forms.TextBox TypeTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button HostButton;
        private System.Windows.Forms.Button JoinButton;
        private System.Windows.Forms.Button SendButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.TextBox UsernameTextbox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RichTextBox ChatTextBox;
        private System.Windows.Forms.TextBox PasswordTextbox;
        private System.Windows.Forms.Label label7;
    }
}

