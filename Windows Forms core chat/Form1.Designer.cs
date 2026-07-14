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
            ChatTextBox = new System.Windows.Forms.TextBox();
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
            button10 = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(15, 14);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(64, 21);
            label1.TabIndex = 0;
            label1.Text = "My Port";
            // 
            // MyPortTextBox
            // 
            MyPortTextBox.Location = new System.Drawing.Point(15, 39);
            MyPortTextBox.Name = "MyPortTextBox";
            MyPortTextBox.Size = new System.Drawing.Size(140, 29);
            MyPortTextBox.TabIndex = 1;
            MyPortTextBox.Text = "6666";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(282, 14);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(87, 21);
            label2.TabIndex = 2;
            label2.Text = "Server Port";
            // 
            // serverPortTextBox
            // 
            serverPortTextBox.Location = new System.Drawing.Point(282, 39);
            serverPortTextBox.Name = "serverPortTextBox";
            serverPortTextBox.Size = new System.Drawing.Size(140, 29);
            serverPortTextBox.TabIndex = 3;
            serverPortTextBox.Text = "6666";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(476, 14);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(70, 21);
            label3.TabIndex = 4;
            label3.Text = "server IP";
            // 
            // ServerIPTextBox
            // 
            ServerIPTextBox.Location = new System.Drawing.Point(476, 39);
            ServerIPTextBox.Name = "ServerIPTextBox";
            ServerIPTextBox.Size = new System.Drawing.Size(178, 29);
            ServerIPTextBox.TabIndex = 5;
            ServerIPTextBox.Text = "127.0.0.1";
            // 
            // ChatTextBox
            // 
            ChatTextBox.Location = new System.Drawing.Point(14, 175);
            ChatTextBox.Multiline = true;
            ChatTextBox.Name = "ChatTextBox";
            ChatTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            ChatTextBox.Size = new System.Drawing.Size(680, 215);
            ChatTextBox.TabIndex = 6;
            ChatTextBox.Text = "\r\n";
            // 
            // TypeTextBox
            // 
            TypeTextBox.Location = new System.Drawing.Point(68, 409);
            TypeTextBox.Name = "TypeTextBox";
            TypeTextBox.Size = new System.Drawing.Size(504, 29);
            TypeTextBox.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(14, 409);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(45, 21);
            label4.TabIndex = 8;
            label4.Text = "Chat:";
            // 
            // HostButton
            // 
            HostButton.Location = new System.Drawing.Point(15, 96);
            HostButton.Name = "HostButton";
            HostButton.Size = new System.Drawing.Size(106, 30);
            HostButton.TabIndex = 9;
            HostButton.Text = "Host Server";
            HostButton.UseVisualStyleBackColor = true;
            HostButton.Click += HostButton_Click;
            // 
            // JoinButton
            // 
            JoinButton.Location = new System.Drawing.Point(282, 96);
            JoinButton.Name = "JoinButton";
            JoinButton.Size = new System.Drawing.Size(106, 30);
            JoinButton.TabIndex = 10;
            JoinButton.Text = "Join Server";
            JoinButton.UseVisualStyleBackColor = true;
            JoinButton.Click += JoinButton_Click;
            // 
            // SendButton
            // 
            SendButton.Location = new System.Drawing.Point(588, 409);
            SendButton.Name = "SendButton";
            SendButton.Size = new System.Drawing.Size(106, 30);
            SendButton.TabIndex = 11;
            SendButton.Text = "Send";
            SendButton.UseVisualStyleBackColor = true;
            SendButton.Click += SendButton_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(195, 36);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(32, 21);
            label5.TabIndex = 12;
            label5.Text = "OR";
            // 
            // button1
            // 
            button1.BackColor = System.Drawing.Color.PaleTurquoise;
            button1.Font = new System.Drawing.Font("Segoe UI", 19F);
            button1.Location = new System.Drawing.Point(822, 36);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(106, 103);
            button1.TabIndex = 13;
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.BackColor = System.Drawing.Color.PaleTurquoise;
            button2.Font = new System.Drawing.Font("Segoe UI", 19F);
            button2.Location = new System.Drawing.Point(935, 36);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(106, 103);
            button2.TabIndex = 13;
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.BackColor = System.Drawing.Color.PaleTurquoise;
            button3.Font = new System.Drawing.Font("Segoe UI", 19F);
            button3.Location = new System.Drawing.Point(1047, 36);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(106, 103);
            button3.TabIndex = 13;
            button3.UseVisualStyleBackColor = false;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.BackColor = System.Drawing.Color.PaleTurquoise;
            button4.Font = new System.Drawing.Font("Segoe UI", 19F);
            button4.Location = new System.Drawing.Point(822, 145);
            button4.Name = "button4";
            button4.Size = new System.Drawing.Size(106, 103);
            button4.TabIndex = 13;
            button4.UseVisualStyleBackColor = false;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.BackColor = System.Drawing.Color.PaleTurquoise;
            button5.Font = new System.Drawing.Font("Segoe UI", 19F);
            button5.Location = new System.Drawing.Point(935, 145);
            button5.Name = "button5";
            button5.Size = new System.Drawing.Size(106, 103);
            button5.TabIndex = 13;
            button5.UseVisualStyleBackColor = false;
            button5.Click += button5_Click;
            // 
            // button6
            // 
            button6.BackColor = System.Drawing.Color.PaleTurquoise;
            button6.Font = new System.Drawing.Font("Segoe UI", 19F);
            button6.Location = new System.Drawing.Point(1047, 145);
            button6.Name = "button6";
            button6.Size = new System.Drawing.Size(106, 103);
            button6.TabIndex = 13;
            button6.UseVisualStyleBackColor = false;
            button6.Click += button6_Click;
            // 
            // button7
            // 
            button7.BackColor = System.Drawing.Color.PaleTurquoise;
            button7.Font = new System.Drawing.Font("Segoe UI", 19F);
            button7.Location = new System.Drawing.Point(822, 254);
            button7.Name = "button7";
            button7.Size = new System.Drawing.Size(106, 103);
            button7.TabIndex = 13;
            button7.UseVisualStyleBackColor = false;
            button7.Click += button7_Click;
            // 
            // button8
            // 
            button8.BackColor = System.Drawing.Color.PaleTurquoise;
            button8.Font = new System.Drawing.Font("Segoe UI", 19F);
            button8.Location = new System.Drawing.Point(935, 254);
            button8.Name = "button8";
            button8.Size = new System.Drawing.Size(106, 103);
            button8.TabIndex = 13;
            button8.UseVisualStyleBackColor = false;
            button8.Click += button8_Click;
            // 
            // button9
            // 
            button9.BackColor = System.Drawing.Color.PaleTurquoise;
            button9.Font = new System.Drawing.Font("Segoe UI", 19F);
            button9.Location = new System.Drawing.Point(1047, 254);
            button9.Name = "button9";
            button9.Size = new System.Drawing.Size(106, 103);
            button9.TabIndex = 13;
            button9.UseVisualStyleBackColor = false;
            button9.Click += button9_Click;
            // 
            // button10
            // 
            button10.Location = new System.Drawing.Point(747, 382);
            button10.Name = "button10";
            button10.Size = new System.Drawing.Size(75, 23);
            button10.TabIndex = 14;
            button10.Text = "button10";
            button10.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1297, 502);
            Controls.Add(button10);
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
            Controls.Add(ChatTextBox);
            Controls.Add(ServerIPTextBox);
            Controls.Add(label3);
            Controls.Add(serverPortTextBox);
            Controls.Add(label2);
            Controls.Add(MyPortTextBox);
            Controls.Add(label1);
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
        private System.Windows.Forms.TextBox ChatTextBox;
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
        private System.Windows.Forms.Button button10;
    }
}

