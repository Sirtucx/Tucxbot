namespace TucxbotForm
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnJoin = new System.Windows.Forms.Button();
            this.btnLeave = new System.Windows.Forms.Button();
            this.tBoxJoin = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rTBoxChat = new System.Windows.Forms.RichTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rTBoxEvents = new System.Windows.Forms.RichTextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tBoxWhisper = new System.Windows.Forms.TextBox();
            this.rBtnWhisper = new System.Windows.Forms.RadioButton();
            this.rBtnChat = new System.Windows.Forms.RadioButton();
            this.rTBoxChatOutput = new System.Windows.Forms.RichTextBox();
            this.cBoxChannels = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cBoxJoinedChannels = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnJoin
            // 
            this.btnJoin.Enabled = false;
            this.btnJoin.Location = new System.Drawing.Point(7, 48);
            this.btnJoin.Name = "btnJoin";
            this.btnJoin.Size = new System.Drawing.Size(133, 23);
            this.btnJoin.TabIndex = 0;
            this.btnJoin.TabStop = false;
            this.btnJoin.Text = "Join";
            this.btnJoin.UseVisualStyleBackColor = true;
            this.btnJoin.Click += new System.EventHandler(this.btnJoin_Click);
            // 
            // btnLeave
            // 
            this.btnLeave.Enabled = false;
            this.btnLeave.Location = new System.Drawing.Point(168, 48);
            this.btnLeave.Name = "btnLeave";
            this.btnLeave.Size = new System.Drawing.Size(133, 23);
            this.btnLeave.TabIndex = 1;
            this.btnLeave.TabStop = false;
            this.btnLeave.Text = "Leave";
            this.btnLeave.UseVisualStyleBackColor = true;
            this.btnLeave.Click += new System.EventHandler(this.btnLeave_Click);
            // 
            // tBoxJoin
            // 
            this.tBoxJoin.Location = new System.Drawing.Point(7, 22);
            this.tBoxJoin.Name = "tBoxJoin";
            this.tBoxJoin.Size = new System.Drawing.Size(133, 20);
            this.tBoxJoin.TabIndex = 2;
            this.tBoxJoin.Text = "Channel to join";
            this.tBoxJoin.Enter += new System.EventHandler(this.tBoxJoin_Enter);
            this.tBoxJoin.Leave += new System.EventHandler(this.tBoxJoin_Leave);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rTBoxChat);
            this.groupBox1.Location = new System.Drawing.Point(14, 194);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(307, 224);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Chat";
            // 
            // rTBoxChat
            // 
            this.rTBoxChat.Location = new System.Drawing.Point(7, 19);
            this.rTBoxChat.Name = "rTBoxChat";
            this.rTBoxChat.ReadOnly = true;
            this.rTBoxChat.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rTBoxChat.Size = new System.Drawing.Size(288, 198);
            this.rTBoxChat.TabIndex = 0;
            this.rTBoxChat.TabStop = false;
            this.rTBoxChat.Text = "";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rTBoxEvents);
            this.groupBox2.Location = new System.Drawing.Point(327, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(199, 412);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Event Logs";
            // 
            // rTBoxEvents
            // 
            this.rTBoxEvents.Location = new System.Drawing.Point(6, 19);
            this.rTBoxEvents.Name = "rTBoxEvents";
            this.rTBoxEvents.ReadOnly = true;
            this.rTBoxEvents.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rTBoxEvents.Size = new System.Drawing.Size(184, 385);
            this.rTBoxEvents.TabIndex = 0;
            this.rTBoxEvents.TabStop = false;
            this.rTBoxEvents.Text = "";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tBoxWhisper);
            this.groupBox3.Controls.Add(this.cBoxJoinedChannels);
            this.groupBox3.Controls.Add(this.rBtnWhisper);
            this.groupBox3.Controls.Add(this.rBtnChat);
            this.groupBox3.Controls.Add(this.rTBoxChatOutput);
            this.groupBox3.Location = new System.Drawing.Point(14, 96);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(307, 94);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Chat Input";
            // 
            // tBoxWhisper
            // 
            this.tBoxWhisper.Location = new System.Drawing.Point(168, 15);
            this.tBoxWhisper.Name = "tBoxWhisper";
            this.tBoxWhisper.Size = new System.Drawing.Size(133, 20);
            this.tBoxWhisper.TabIndex = 4;
            this.tBoxWhisper.Text = "Username to whisper";
            this.tBoxWhisper.Visible = false;
            this.tBoxWhisper.Enter += new System.EventHandler(this.tBoxWhisper_Enter);
            this.tBoxWhisper.Leave += new System.EventHandler(this.tBoxWhisper_Leave);
            // 
            // rBtnWhisper
            // 
            this.rBtnWhisper.AutoSize = true;
            this.rBtnWhisper.Location = new System.Drawing.Point(60, 19);
            this.rBtnWhisper.Name = "rBtnWhisper";
            this.rBtnWhisper.Size = new System.Drawing.Size(64, 17);
            this.rBtnWhisper.TabIndex = 2;
            this.rBtnWhisper.Text = "Whisper";
            this.rBtnWhisper.UseVisualStyleBackColor = true;
            this.rBtnWhisper.CheckedChanged += new System.EventHandler(this.rBtnWhisper_CheckedChanged);
            // 
            // rBtnChat
            // 
            this.rBtnChat.AutoSize = true;
            this.rBtnChat.Checked = true;
            this.rBtnChat.Location = new System.Drawing.Point(7, 20);
            this.rBtnChat.Name = "rBtnChat";
            this.rBtnChat.Size = new System.Drawing.Size(47, 17);
            this.rBtnChat.TabIndex = 1;
            this.rBtnChat.TabStop = true;
            this.rBtnChat.Text = "Chat";
            this.rBtnChat.UseVisualStyleBackColor = true;
            this.rBtnChat.CheckedChanged += new System.EventHandler(this.rBtnChat_CheckedChanged);
            // 
            // rTBoxChatOutput
            // 
            this.rTBoxChatOutput.Location = new System.Drawing.Point(7, 42);
            this.rTBoxChatOutput.Name = "rTBoxChatOutput";
            this.rTBoxChatOutput.Size = new System.Drawing.Size(288, 46);
            this.rTBoxChatOutput.TabIndex = 0;
            this.rTBoxChatOutput.TabStop = false;
            this.rTBoxChatOutput.Text = "";
            this.rTBoxChatOutput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.rTBoxChatOutput_KeyPress);
            this.rTBoxChatOutput.KeyUp += new System.Windows.Forms.KeyEventHandler(this.rTBoxChatOutput_KeyUp);
            // 
            // cBoxChannels
            // 
            this.cBoxChannels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBoxChannels.Enabled = false;
            this.cBoxChannels.FormattingEnabled = true;
            this.cBoxChannels.Location = new System.Drawing.Point(168, 21);
            this.cBoxChannels.Name = "cBoxChannels";
            this.cBoxChannels.Size = new System.Drawing.Size(133, 21);
            this.cBoxChannels.TabIndex = 4;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tBoxJoin);
            this.groupBox4.Controls.Add(this.cBoxChannels);
            this.groupBox4.Controls.Add(this.btnJoin);
            this.groupBox4.Controls.Add(this.btnLeave);
            this.groupBox4.Location = new System.Drawing.Point(14, 6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(307, 84);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Join/Leave";
            // 
            // cBoxJoinedChannels
            // 
            this.cBoxJoinedChannels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBoxJoinedChannels.Enabled = false;
            this.cBoxJoinedChannels.FormattingEnabled = true;
            this.cBoxJoinedChannels.Location = new System.Drawing.Point(168, 15);
            this.cBoxJoinedChannels.Name = "cBoxJoinedChannels";
            this.cBoxJoinedChannels.Size = new System.Drawing.Size(133, 21);
            this.cBoxJoinedChannels.TabIndex = 5;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(529, 422);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "MainForm";
            this.Text = "Tucxbot Open Source";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnJoin;
        private System.Windows.Forms.Button btnLeave;
        private System.Windows.Forms.TextBox tBoxJoin;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox rTBoxChat;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RichTextBox rTBoxEvents;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rBtnWhisper;
        private System.Windows.Forms.RadioButton rBtnChat;
        private System.Windows.Forms.RichTextBox rTBoxChatOutput;
        private System.Windows.Forms.TextBox tBoxWhisper;
        private System.Windows.Forms.ComboBox cBoxChannels;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox cBoxJoinedChannels;
    }
}

