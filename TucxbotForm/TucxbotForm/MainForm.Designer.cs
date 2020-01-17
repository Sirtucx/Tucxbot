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
            this.websocketPage = new System.Windows.Forms.TabPage();
            this.m_ModGroup = new System.Windows.Forms.GroupBox();
            this.m_UnloadModButton = new System.Windows.Forms.Button();
            this.m_UnloadModCB = new System.Windows.Forms.ComboBox();
            this.m_LoadModButton = new System.Windows.Forms.Button();
            this.m_eventGroup = new System.Windows.Forms.GroupBox();
            this.m_whisperGroup = new System.Windows.Forms.GroupBox();
            this.m_WhisperLabel = new System.Windows.Forms.Label();
            this.m_WhisperInputTB = new System.Windows.Forms.RichTextBox();
            this.m_WhisperOutputTB = new System.Windows.Forms.RichTextBox();
            this.m_WhisperUserTB = new System.Windows.Forms.TextBox();
            this.m_channelGroup = new System.Windows.Forms.GroupBox();
            this.m_ChatChannelLabel = new System.Windows.Forms.Label();
            this.m_leaveChannelButton = new System.Windows.Forms.Button();
            this.m_channelMessageSelectCB = new System.Windows.Forms.ComboBox();
            this.m_JoinChannelButton = new System.Windows.Forms.Button();
            this.m_ChannelInputTB = new System.Windows.Forms.RichTextBox();
            this.m_ChannelOutputTB = new System.Windows.Forms.RichTextBox();
            this.m_channelLeaveCB = new System.Windows.Forms.ComboBox();
            this.m_ChannelJoinTB = new System.Windows.Forms.TextBox();
            this.m_ConnectionGroup = new System.Windows.Forms.GroupBox();
            this.m_DisconnectButton = new System.Windows.Forms.Button();
            this.m_LoginButton = new System.Windows.Forms.Button();
            this.m_connectButton = new System.Windows.Forms.Button();
            this.versionTab = new System.Windows.Forms.TabControl();
            this.websocketPage.SuspendLayout();
            this.m_ModGroup.SuspendLayout();
            this.m_whisperGroup.SuspendLayout();
            this.m_channelGroup.SuspendLayout();
            this.m_ConnectionGroup.SuspendLayout();
            this.versionTab.SuspendLayout();
            this.SuspendLayout();
            this.websocketPage.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (64)))), ((int) (((byte) (64)))), ((int) (((byte) (64)))));
            this.websocketPage.Controls.Add(this.m_ModGroup);
            this.websocketPage.Controls.Add(this.m_eventGroup);
            this.websocketPage.Controls.Add(this.m_whisperGroup);
            this.websocketPage.Controls.Add(this.m_channelGroup);
            this.websocketPage.Controls.Add(this.m_ConnectionGroup);
            this.websocketPage.ForeColor = System.Drawing.SystemColors.ControlText;
            this.websocketPage.Location = new System.Drawing.Point(4, 24);
            this.websocketPage.Name = "websocketPage";
            this.websocketPage.Padding = new System.Windows.Forms.Padding(3);
            this.websocketPage.Size = new System.Drawing.Size(896, 601);
            this.websocketPage.TabIndex = 1;
            this.websocketPage.Text = "WebSocket";
            this.m_ModGroup.Controls.Add(this.m_UnloadModButton);
            this.m_ModGroup.Controls.Add(this.m_UnloadModCB);
            this.m_ModGroup.Controls.Add(this.m_LoadModButton);
            this.m_ModGroup.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.m_ModGroup.Location = new System.Drawing.Point(8, 114);
            this.m_ModGroup.Name = "m_ModGroup";
            this.m_ModGroup.Size = new System.Drawing.Size(329, 119);
            this.m_ModGroup.TabIndex = 8;
            this.m_ModGroup.TabStop = false;
            this.m_ModGroup.Text = "Load / Remove Mods";
            this.m_UnloadModButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.m_UnloadModButton.Location = new System.Drawing.Point(216, 32);
            this.m_UnloadModButton.Name = "m_UnloadModButton";
            this.m_UnloadModButton.Size = new System.Drawing.Size(106, 23);
            this.m_UnloadModButton.TabIndex = 2;
            this.m_UnloadModButton.Text = "Unload Mod";
            this.m_UnloadModButton.UseVisualStyleBackColor = true;
            this.m_UnloadModButton.Click += new System.EventHandler(this.UnloadModButton_Click);
            this.m_UnloadModCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_UnloadModCB.ForeColor = System.Drawing.SystemColors.ControlText;
            this.m_UnloadModCB.FormattingEnabled = true;
            this.m_UnloadModCB.Location = new System.Drawing.Point(7, 32);
            this.m_UnloadModCB.Name = "m_UnloadModCB";
            this.m_UnloadModCB.Size = new System.Drawing.Size(201, 23);
            this.m_UnloadModCB.TabIndex = 1;
            this.m_LoadModButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.m_LoadModButton.Location = new System.Drawing.Point(7, 80);
            this.m_LoadModButton.Name = "m_LoadModButton";
            this.m_LoadModButton.Size = new System.Drawing.Size(315, 32);
            this.m_LoadModButton.TabIndex = 0;
            this.m_LoadModButton.Text = "Load Mods";
            this.m_LoadModButton.UseVisualStyleBackColor = true;
            this.m_LoadModButton.Click += new System.EventHandler(this.LoadModButton_Click);
            this.m_eventGroup.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.m_eventGroup.Location = new System.Drawing.Point(8, 240);
            this.m_eventGroup.Name = "m_eventGroup";
            this.m_eventGroup.Size = new System.Drawing.Size(329, 343);
            this.m_eventGroup.TabIndex = 7;
            this.m_eventGroup.TabStop = false;
            this.m_eventGroup.Text = "Events";
            this.m_eventGroup.Visible = false;
            this.m_whisperGroup.Controls.Add(this.m_WhisperLabel);
            this.m_whisperGroup.Controls.Add(this.m_WhisperInputTB);
            this.m_whisperGroup.Controls.Add(this.m_WhisperOutputTB);
            this.m_whisperGroup.Controls.Add(this.m_WhisperUserTB);
            this.m_whisperGroup.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.m_whisperGroup.Location = new System.Drawing.Point(621, 15);
            this.m_whisperGroup.Name = "m_whisperGroup";
            this.m_whisperGroup.Size = new System.Drawing.Size(268, 575);
            this.m_whisperGroup.TabIndex = 6;
            this.m_whisperGroup.TabStop = false;
            this.m_whisperGroup.Text = "Whispers";
            this.m_whisperGroup.Visible = false;
            this.m_WhisperLabel.AutoSize = true;
            this.m_WhisperLabel.Location = new System.Drawing.Point(20, 32);
            this.m_WhisperLabel.Name = "m_WhisperLabel";
            this.m_WhisperLabel.Size = new System.Drawing.Size(79, 15);
            this.m_WhisperLabel.TabIndex = 10;
            this.m_WhisperLabel.Text = "Whisper User:";
            this.m_WhisperInputTB.Location = new System.Drawing.Point(9, 62);
            this.m_WhisperInputTB.Name = "m_WhisperInputTB";
            this.m_WhisperInputTB.Size = new System.Drawing.Size(251, 62);
            this.m_WhisperInputTB.TabIndex = 6;
            this.m_WhisperInputTB.Text = "";
            this.m_WhisperInputTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.WhisperInputTB_KeyPress);
            this.m_WhisperOutputTB.Location = new System.Drawing.Point(9, 132);
            this.m_WhisperOutputTB.Name = "m_WhisperOutputTB";
            this.m_WhisperOutputTB.ReadOnly = true;
            this.m_WhisperOutputTB.Size = new System.Drawing.Size(251, 436);
            this.m_WhisperOutputTB.TabIndex = 7;
            this.m_WhisperOutputTB.Text = "";
            this.m_WhisperUserTB.Location = new System.Drawing.Point(129, 28);
            this.m_WhisperUserTB.Name = "m_WhisperUserTB";
            this.m_WhisperUserTB.Size = new System.Drawing.Size(131, 23);
            this.m_WhisperUserTB.TabIndex = 5;
            this.m_channelGroup.Controls.Add(this.m_ChatChannelLabel);
            this.m_channelGroup.Controls.Add(this.m_leaveChannelButton);
            this.m_channelGroup.Controls.Add(this.m_channelMessageSelectCB);
            this.m_channelGroup.Controls.Add(this.m_JoinChannelButton);
            this.m_channelGroup.Controls.Add(this.m_ChannelInputTB);
            this.m_channelGroup.Controls.Add(this.m_ChannelOutputTB);
            this.m_channelGroup.Controls.Add(this.m_channelLeaveCB);
            this.m_channelGroup.Controls.Add(this.m_ChannelJoinTB);
            this.m_channelGroup.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.m_channelGroup.Location = new System.Drawing.Point(346, 15);
            this.m_channelGroup.Name = "m_channelGroup";
            this.m_channelGroup.Size = new System.Drawing.Size(268, 575);
            this.m_channelGroup.TabIndex = 2;
            this.m_channelGroup.TabStop = false;
            this.m_channelGroup.Text = "Public Twitch Chat";
            this.m_channelGroup.Visible = false;
            this.m_ChatChannelLabel.AutoSize = true;
            this.m_ChatChannelLabel.Location = new System.Drawing.Point(7, 123);
            this.m_ChatChannelLabel.Name = "m_ChatChannelLabel";
            this.m_ChatChannelLabel.Size = new System.Drawing.Size(101, 15);
            this.m_ChatChannelLabel.TabIndex = 7;
            this.m_ChatChannelLabel.Text = "Post a message in";
            this.m_leaveChannelButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.m_leaveChannelButton.Location = new System.Drawing.Point(155, 60);
            this.m_leaveChannelButton.Name = "m_leaveChannelButton";
            this.m_leaveChannelButton.Size = new System.Drawing.Size(104, 24);
            this.m_leaveChannelButton.TabIndex = 6;
            this.m_leaveChannelButton.Text = "Leave";
            this.m_leaveChannelButton.UseVisualStyleBackColor = true;
            this.m_leaveChannelButton.Visible = false;
            this.m_leaveChannelButton.Click += new System.EventHandler(this.LeaveChannelButton_Click);
            this.m_channelMessageSelectCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_channelMessageSelectCB.ForeColor = System.Drawing.SystemColors.ControlText;
            this.m_channelMessageSelectCB.FormattingEnabled = true;
            this.m_channelMessageSelectCB.Location = new System.Drawing.Point(122, 120);
            this.m_channelMessageSelectCB.Name = "m_channelMessageSelectCB";
            this.m_channelMessageSelectCB.Size = new System.Drawing.Size(136, 23);
            this.m_channelMessageSelectCB.TabIndex = 4;
            this.m_JoinChannelButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.m_JoinChannelButton.Location = new System.Drawing.Point(155, 30);
            this.m_JoinChannelButton.Name = "m_JoinChannelButton";
            this.m_JoinChannelButton.Size = new System.Drawing.Size(104, 24);
            this.m_JoinChannelButton.TabIndex = 3;
            this.m_JoinChannelButton.TabStop = false;
            this.m_JoinChannelButton.Text = "Join";
            this.m_JoinChannelButton.UseVisualStyleBackColor = true;
            this.m_JoinChannelButton.Click += new System.EventHandler(this.JoinChannelButton_Click);
            this.m_ChannelInputTB.Location = new System.Drawing.Point(7, 156);
            this.m_ChannelInputTB.Name = "m_ChannelInputTB";
            this.m_ChannelInputTB.Size = new System.Drawing.Size(254, 62);
            this.m_ChannelInputTB.TabIndex = 9;
            this.m_ChannelInputTB.Text = "";
            this.m_ChannelInputTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ChannelMessageInput_KeyPress);
            this.m_ChannelInputTB.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ChannelMessageInput_KeyUp);
            this.m_ChannelOutputTB.Location = new System.Drawing.Point(7, 225);
            this.m_ChannelOutputTB.Name = "m_ChannelOutputTB";
            this.m_ChannelOutputTB.ReadOnly = true;
            this.m_ChannelOutputTB.Size = new System.Drawing.Size(254, 342);
            this.m_ChannelOutputTB.TabIndex = 10;
            this.m_ChannelOutputTB.Text = "";
            this.m_channelLeaveCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_channelLeaveCB.ForeColor = System.Drawing.SystemColors.ControlText;
            this.m_channelLeaveCB.FormattingEnabled = true;
            this.m_channelLeaveCB.Location = new System.Drawing.Point(7, 60);
            this.m_channelLeaveCB.Name = "m_channelLeaveCB";
            this.m_channelLeaveCB.Size = new System.Drawing.Size(140, 23);
            this.m_channelLeaveCB.TabIndex = 5;
            this.m_channelLeaveCB.Visible = false;
            this.m_ChannelJoinTB.Location = new System.Drawing.Point(7, 30);
            this.m_ChannelJoinTB.Name = "m_ChannelJoinTB";
            this.m_ChannelJoinTB.Size = new System.Drawing.Size(140, 23);
            this.m_ChannelJoinTB.TabIndex = 8;
            this.m_ChannelJoinTB.Text = "Channel to join";
            this.m_ChannelJoinTB.Enter += new System.EventHandler(this.JoinChannelTB_Enter);
            this.m_ChannelJoinTB.Leave += new System.EventHandler(this.JoinChannelTB_Leave);
            this.m_ConnectionGroup.Controls.Add(this.m_DisconnectButton);
            this.m_ConnectionGroup.Controls.Add(this.m_LoginButton);
            this.m_ConnectionGroup.Controls.Add(this.m_connectButton);
            this.m_ConnectionGroup.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.m_ConnectionGroup.Location = new System.Drawing.Point(8, 15);
            this.m_ConnectionGroup.Name = "m_ConnectionGroup";
            this.m_ConnectionGroup.Size = new System.Drawing.Size(329, 91);
            this.m_ConnectionGroup.TabIndex = 0;
            this.m_ConnectionGroup.TabStop = false;
            this.m_ConnectionGroup.Text = "Connection to Twitch";
            this.m_DisconnectButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.m_DisconnectButton.Location = new System.Drawing.Point(171, 59);
            this.m_DisconnectButton.Name = "m_DisconnectButton";
            this.m_DisconnectButton.Size = new System.Drawing.Size(150, 23);
            this.m_DisconnectButton.TabIndex = 2;
            this.m_DisconnectButton.Text = "Disconnect";
            this.m_DisconnectButton.UseVisualStyleBackColor = true;
            this.m_DisconnectButton.Visible = false;
            this.m_DisconnectButton.Click += new System.EventHandler(this.DisconnectButton_Click);
            this.m_LoginButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.m_LoginButton.Location = new System.Drawing.Point(7, 22);
            this.m_LoginButton.Name = "m_LoginButton";
            this.m_LoginButton.Size = new System.Drawing.Size(315, 32);
            this.m_LoginButton.TabIndex = 1;
            this.m_LoginButton.Text = "Login";
            this.m_LoginButton.UseVisualStyleBackColor = true;
            this.m_LoginButton.Click += new System.EventHandler(this.LoginButton_Click);
            this.m_connectButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.m_connectButton.Location = new System.Drawing.Point(7, 59);
            this.m_connectButton.Name = "m_connectButton";
            this.m_connectButton.Size = new System.Drawing.Size(150, 23);
            this.m_connectButton.TabIndex = 0;
            this.m_connectButton.Text = "Connect";
            this.m_connectButton.UseVisualStyleBackColor = true;
            this.m_connectButton.Visible = false;
            this.m_connectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            this.versionTab.Controls.Add(this.websocketPage);
            this.versionTab.Location = new System.Drawing.Point(8, 15);
            this.versionTab.Name = "versionTab";
            this.versionTab.SelectedIndex = 0;
            this.versionTab.Size = new System.Drawing.Size(904, 629);
            this.versionTab.TabIndex = 8;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(915, 647);
            this.Controls.Add(this.versionTab);
            this.Name = "MainForm";
            this.Text = "Tucxbot Open Source";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.websocketPage.ResumeLayout(false);
            this.m_ModGroup.ResumeLayout(false);
            this.m_whisperGroup.ResumeLayout(false);
            this.m_whisperGroup.PerformLayout();
            this.m_channelGroup.ResumeLayout(false);
            this.m_channelGroup.PerformLayout();
            this.m_ConnectionGroup.ResumeLayout(false);
            this.versionTab.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TabPage websocketPage;
        private System.Windows.Forms.GroupBox m_channelGroup;
        private System.Windows.Forms.TextBox m_WhisperUserTB;
        private System.Windows.Forms.ComboBox m_channelMessageSelectCB;
        private System.Windows.Forms.RichTextBox m_ChannelInputTB;
        private System.Windows.Forms.RichTextBox m_ChannelOutputTB;
        private System.Windows.Forms.Button m_leaveChannelButton;
        private System.Windows.Forms.ComboBox m_channelLeaveCB;
        private System.Windows.Forms.Button m_JoinChannelButton;
        private System.Windows.Forms.TextBox m_ChannelJoinTB;
        private System.Windows.Forms.GroupBox m_ConnectionGroup;
        private System.Windows.Forms.Button m_LoginButton;
        private System.Windows.Forms.Button m_connectButton;
        private System.Windows.Forms.TabControl versionTab;
        private System.Windows.Forms.Button m_DisconnectButton;
        private System.Windows.Forms.GroupBox m_whisperGroup;
        private System.Windows.Forms.RichTextBox m_WhisperInputTB;
        private System.Windows.Forms.RichTextBox m_WhisperOutputTB;
        private System.Windows.Forms.Label m_ChatChannelLabel;
        private System.Windows.Forms.Label m_WhisperLabel;
        private System.Windows.Forms.GroupBox m_eventGroup;
        private System.Windows.Forms.GroupBox m_ModGroup;
        private System.Windows.Forms.Button m_LoadModButton;
        private System.Windows.Forms.Button m_UnloadModButton;
        private System.Windows.Forms.ComboBox m_UnloadModCB;
    }
}

