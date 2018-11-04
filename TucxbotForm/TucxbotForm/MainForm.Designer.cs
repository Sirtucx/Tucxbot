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
            this.m_EventGroup = new System.Windows.Forms.GroupBox();
            this.m_WhisperGroup = new System.Windows.Forms.GroupBox();
            this.m_WhisperLabel = new System.Windows.Forms.Label();
            this.m_WhisperInputTB = new System.Windows.Forms.RichTextBox();
            this.m_WhisperOutputTB = new System.Windows.Forms.RichTextBox();
            this.m_WhisperUserTB = new System.Windows.Forms.TextBox();
            this.m_ChannelGroup = new System.Windows.Forms.GroupBox();
            this.m_ChatChannelLabel = new System.Windows.Forms.Label();
            this.m_LeaveChannelButton = new System.Windows.Forms.Button();
            this.m_ChannelMessageSelectCB = new System.Windows.Forms.ComboBox();
            this.m_JoinChannelButton = new System.Windows.Forms.Button();
            this.m_ChannelInputTB = new System.Windows.Forms.RichTextBox();
            this.m_ChannelOutputTB = new System.Windows.Forms.RichTextBox();
            this.m_ChannelLeaveCB = new System.Windows.Forms.ComboBox();
            this.m_ChannelJoinTB = new System.Windows.Forms.TextBox();
            this.m_ConnectionGroup = new System.Windows.Forms.GroupBox();
            this.m_DisconnectButton = new System.Windows.Forms.Button();
            this.m_LoginButton = new System.Windows.Forms.Button();
            this.m_ConnectButton = new System.Windows.Forms.Button();
            this.versionTab = new System.Windows.Forms.TabControl();
            this.websocketPage.SuspendLayout();
            this.m_ModGroup.SuspendLayout();
            this.m_WhisperGroup.SuspendLayout();
            this.m_ChannelGroup.SuspendLayout();
            this.m_ConnectionGroup.SuspendLayout();
            this.versionTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // websocketPage
            // 
            this.websocketPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.websocketPage.Controls.Add(this.m_ModGroup);
            this.websocketPage.Controls.Add(this.m_EventGroup);
            this.websocketPage.Controls.Add(this.m_WhisperGroup);
            this.websocketPage.Controls.Add(this.m_ChannelGroup);
            this.websocketPage.Controls.Add(this.m_ConnectionGroup);
            this.websocketPage.ForeColor = System.Drawing.SystemColors.ControlText;
            this.websocketPage.Location = new System.Drawing.Point(4, 22);
            this.websocketPage.Name = "websocketPage";
            this.websocketPage.Padding = new System.Windows.Forms.Padding(3);
            this.websocketPage.Size = new System.Drawing.Size(767, 519);
            this.websocketPage.TabIndex = 1;
            this.websocketPage.Text = "WebSocket";
            // 
            // m_ModGroup
            // 
            this.m_ModGroup.Controls.Add(this.m_UnloadModButton);
            this.m_ModGroup.Controls.Add(this.m_UnloadModCB);
            this.m_ModGroup.Controls.Add(this.m_LoadModButton);
            this.m_ModGroup.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.m_ModGroup.Location = new System.Drawing.Point(7, 99);
            this.m_ModGroup.Name = "m_ModGroup";
            this.m_ModGroup.Size = new System.Drawing.Size(282, 103);
            this.m_ModGroup.TabIndex = 8;
            this.m_ModGroup.TabStop = false;
            this.m_ModGroup.Text = "Load / Remove Mods";
            // 
            // m_UnloadModButton
            // 
            this.m_UnloadModButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.m_UnloadModButton.Location = new System.Drawing.Point(185, 28);
            this.m_UnloadModButton.Name = "m_UnloadModButton";
            this.m_UnloadModButton.Size = new System.Drawing.Size(91, 20);
            this.m_UnloadModButton.TabIndex = 2;
            this.m_UnloadModButton.Text = "Unload Mod";
            this.m_UnloadModButton.UseVisualStyleBackColor = true;
            this.m_UnloadModButton.Click += new System.EventHandler(this.UnloadModButton_Click);
            // 
            // m_UnloadModCB
            // 
            this.m_UnloadModCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_UnloadModCB.ForeColor = System.Drawing.SystemColors.ControlText;
            this.m_UnloadModCB.FormattingEnabled = true;
            this.m_UnloadModCB.Location = new System.Drawing.Point(6, 28);
            this.m_UnloadModCB.Name = "m_UnloadModCB";
            this.m_UnloadModCB.Size = new System.Drawing.Size(173, 21);
            this.m_UnloadModCB.TabIndex = 1;
            // 
            // m_LoadModButton
            // 
            this.m_LoadModButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.m_LoadModButton.Location = new System.Drawing.Point(6, 69);
            this.m_LoadModButton.Name = "m_LoadModButton";
            this.m_LoadModButton.Size = new System.Drawing.Size(270, 28);
            this.m_LoadModButton.TabIndex = 0;
            this.m_LoadModButton.Text = "Load Mods";
            this.m_LoadModButton.UseVisualStyleBackColor = true;
            this.m_LoadModButton.Click += new System.EventHandler(this.LoadModButton_Click);
            // 
            // m_EventGroup
            // 
            this.m_EventGroup.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.m_EventGroup.Location = new System.Drawing.Point(7, 208);
            this.m_EventGroup.Name = "m_EventGroup";
            this.m_EventGroup.Size = new System.Drawing.Size(282, 297);
            this.m_EventGroup.TabIndex = 7;
            this.m_EventGroup.TabStop = false;
            this.m_EventGroup.Text = "Events";
            this.m_EventGroup.Visible = false;
            // 
            // m_WhisperGroup
            // 
            this.m_WhisperGroup.Controls.Add(this.m_WhisperLabel);
            this.m_WhisperGroup.Controls.Add(this.m_WhisperInputTB);
            this.m_WhisperGroup.Controls.Add(this.m_WhisperOutputTB);
            this.m_WhisperGroup.Controls.Add(this.m_WhisperUserTB);
            this.m_WhisperGroup.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.m_WhisperGroup.Location = new System.Drawing.Point(532, 13);
            this.m_WhisperGroup.Name = "m_WhisperGroup";
            this.m_WhisperGroup.Size = new System.Drawing.Size(230, 498);
            this.m_WhisperGroup.TabIndex = 6;
            this.m_WhisperGroup.TabStop = false;
            this.m_WhisperGroup.Text = "Whispers";
            this.m_WhisperGroup.Visible = false;
            // 
            // m_WhisperLabel
            // 
            this.m_WhisperLabel.AutoSize = true;
            this.m_WhisperLabel.Location = new System.Drawing.Point(17, 28);
            this.m_WhisperLabel.Name = "m_WhisperLabel";
            this.m_WhisperLabel.Size = new System.Drawing.Size(74, 13);
            this.m_WhisperLabel.TabIndex = 10;
            this.m_WhisperLabel.Text = "Whisper User:";
            // 
            // m_WhisperInputTB
            // 
            this.m_WhisperInputTB.Location = new System.Drawing.Point(8, 54);
            this.m_WhisperInputTB.Name = "m_WhisperInputTB";
            this.m_WhisperInputTB.Size = new System.Drawing.Size(216, 54);
            this.m_WhisperInputTB.TabIndex = 6;
            this.m_WhisperInputTB.Text = "";
            this.m_WhisperInputTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.WhisperInputTB_KeyPress);
            // 
            // m_WhisperOutputTB
            // 
            this.m_WhisperOutputTB.Location = new System.Drawing.Point(8, 114);
            this.m_WhisperOutputTB.Name = "m_WhisperOutputTB";
            this.m_WhisperOutputTB.ReadOnly = true;
            this.m_WhisperOutputTB.Size = new System.Drawing.Size(216, 378);
            this.m_WhisperOutputTB.TabIndex = 7;
            this.m_WhisperOutputTB.Text = "";
            // 
            // m_WhisperUserTB
            // 
            this.m_WhisperUserTB.Location = new System.Drawing.Point(111, 24);
            this.m_WhisperUserTB.Name = "m_WhisperUserTB";
            this.m_WhisperUserTB.Size = new System.Drawing.Size(113, 20);
            this.m_WhisperUserTB.TabIndex = 5;
            // 
            // m_ChannelGroup
            // 
            this.m_ChannelGroup.Controls.Add(this.m_ChatChannelLabel);
            this.m_ChannelGroup.Controls.Add(this.m_LeaveChannelButton);
            this.m_ChannelGroup.Controls.Add(this.m_ChannelMessageSelectCB);
            this.m_ChannelGroup.Controls.Add(this.m_JoinChannelButton);
            this.m_ChannelGroup.Controls.Add(this.m_ChannelInputTB);
            this.m_ChannelGroup.Controls.Add(this.m_ChannelOutputTB);
            this.m_ChannelGroup.Controls.Add(this.m_ChannelLeaveCB);
            this.m_ChannelGroup.Controls.Add(this.m_ChannelJoinTB);
            this.m_ChannelGroup.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.m_ChannelGroup.Location = new System.Drawing.Point(297, 13);
            this.m_ChannelGroup.Name = "m_ChannelGroup";
            this.m_ChannelGroup.Size = new System.Drawing.Size(230, 498);
            this.m_ChannelGroup.TabIndex = 2;
            this.m_ChannelGroup.TabStop = false;
            this.m_ChannelGroup.Text = "Public Twitch Chat";
            this.m_ChannelGroup.Visible = false;
            // 
            // m_ChatChannelLabel
            // 
            this.m_ChatChannelLabel.AutoSize = true;
            this.m_ChatChannelLabel.Location = new System.Drawing.Point(6, 107);
            this.m_ChatChannelLabel.Name = "m_ChatChannelLabel";
            this.m_ChatChannelLabel.Size = new System.Drawing.Size(93, 13);
            this.m_ChatChannelLabel.TabIndex = 7;
            this.m_ChatChannelLabel.Text = "Post a message in";
            // 
            // m_LeaveChannelButton
            // 
            this.m_LeaveChannelButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.m_LeaveChannelButton.Location = new System.Drawing.Point(133, 52);
            this.m_LeaveChannelButton.Name = "m_LeaveChannelButton";
            this.m_LeaveChannelButton.Size = new System.Drawing.Size(89, 21);
            this.m_LeaveChannelButton.TabIndex = 6;
            this.m_LeaveChannelButton.Text = "Leave";
            this.m_LeaveChannelButton.UseVisualStyleBackColor = true;
            this.m_LeaveChannelButton.Visible = false;
            this.m_LeaveChannelButton.Click += new System.EventHandler(this.LeaveChannelButton_Click);
            // 
            // m_ChannelMessageSelectCB
            // 
            this.m_ChannelMessageSelectCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_ChannelMessageSelectCB.ForeColor = System.Drawing.SystemColors.ControlText;
            this.m_ChannelMessageSelectCB.FormattingEnabled = true;
            this.m_ChannelMessageSelectCB.Location = new System.Drawing.Point(105, 104);
            this.m_ChannelMessageSelectCB.Name = "m_ChannelMessageSelectCB";
            this.m_ChannelMessageSelectCB.Size = new System.Drawing.Size(117, 21);
            this.m_ChannelMessageSelectCB.TabIndex = 4;
            // 
            // m_JoinChannelButton
            // 
            this.m_JoinChannelButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.m_JoinChannelButton.Location = new System.Drawing.Point(133, 26);
            this.m_JoinChannelButton.Name = "m_JoinChannelButton";
            this.m_JoinChannelButton.Size = new System.Drawing.Size(89, 21);
            this.m_JoinChannelButton.TabIndex = 3;
            this.m_JoinChannelButton.TabStop = false;
            this.m_JoinChannelButton.Text = "Join";
            this.m_JoinChannelButton.UseVisualStyleBackColor = true;
            this.m_JoinChannelButton.Click += new System.EventHandler(this.JoinChannelButton_Click);
            // 
            // m_ChannelInputTB
            // 
            this.m_ChannelInputTB.Location = new System.Drawing.Point(6, 135);
            this.m_ChannelInputTB.Name = "m_ChannelInputTB";
            this.m_ChannelInputTB.Size = new System.Drawing.Size(218, 54);
            this.m_ChannelInputTB.TabIndex = 9;
            this.m_ChannelInputTB.Text = "";
            this.m_ChannelInputTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ChannelMessageInput_KeyPress);
            this.m_ChannelInputTB.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ChannelMessageInput_KeyUp);
            // 
            // m_ChannelOutputTB
            // 
            this.m_ChannelOutputTB.Location = new System.Drawing.Point(6, 195);
            this.m_ChannelOutputTB.Name = "m_ChannelOutputTB";
            this.m_ChannelOutputTB.ReadOnly = true;
            this.m_ChannelOutputTB.Size = new System.Drawing.Size(218, 297);
            this.m_ChannelOutputTB.TabIndex = 10;
            this.m_ChannelOutputTB.Text = "";
            // 
            // m_ChannelLeaveCB
            // 
            this.m_ChannelLeaveCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_ChannelLeaveCB.ForeColor = System.Drawing.SystemColors.ControlText;
            this.m_ChannelLeaveCB.FormattingEnabled = true;
            this.m_ChannelLeaveCB.Location = new System.Drawing.Point(6, 52);
            this.m_ChannelLeaveCB.Name = "m_ChannelLeaveCB";
            this.m_ChannelLeaveCB.Size = new System.Drawing.Size(121, 21);
            this.m_ChannelLeaveCB.TabIndex = 5;
            this.m_ChannelLeaveCB.Visible = false;
            // 
            // m_ChannelJoinTB
            // 
            this.m_ChannelJoinTB.Location = new System.Drawing.Point(6, 26);
            this.m_ChannelJoinTB.Name = "m_ChannelJoinTB";
            this.m_ChannelJoinTB.Size = new System.Drawing.Size(121, 20);
            this.m_ChannelJoinTB.TabIndex = 8;
            this.m_ChannelJoinTB.Text = "Channel to join";
            this.m_ChannelJoinTB.Enter += new System.EventHandler(this.JoinChannelTB_Enter);
            this.m_ChannelJoinTB.Leave += new System.EventHandler(this.JoinChannelTB_Leave);
            // 
            // m_ConnectionGroup
            // 
            this.m_ConnectionGroup.Controls.Add(this.m_DisconnectButton);
            this.m_ConnectionGroup.Controls.Add(this.m_LoginButton);
            this.m_ConnectionGroup.Controls.Add(this.m_ConnectButton);
            this.m_ConnectionGroup.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.m_ConnectionGroup.Location = new System.Drawing.Point(7, 13);
            this.m_ConnectionGroup.Name = "m_ConnectionGroup";
            this.m_ConnectionGroup.Size = new System.Drawing.Size(282, 79);
            this.m_ConnectionGroup.TabIndex = 0;
            this.m_ConnectionGroup.TabStop = false;
            this.m_ConnectionGroup.Text = "Connection to Twitch";
            // 
            // m_DisconnectButton
            // 
            this.m_DisconnectButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.m_DisconnectButton.Location = new System.Drawing.Point(147, 51);
            this.m_DisconnectButton.Name = "m_DisconnectButton";
            this.m_DisconnectButton.Size = new System.Drawing.Size(129, 20);
            this.m_DisconnectButton.TabIndex = 2;
            this.m_DisconnectButton.Text = "Disconnect";
            this.m_DisconnectButton.UseVisualStyleBackColor = true;
            this.m_DisconnectButton.Visible = false;
            this.m_DisconnectButton.Click += new System.EventHandler(this.DisconnectButton_Click);
            // 
            // m_LoginButton
            // 
            this.m_LoginButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.m_LoginButton.Location = new System.Drawing.Point(6, 19);
            this.m_LoginButton.Name = "m_LoginButton";
            this.m_LoginButton.Size = new System.Drawing.Size(270, 28);
            this.m_LoginButton.TabIndex = 1;
            this.m_LoginButton.Text = "Login";
            this.m_LoginButton.UseVisualStyleBackColor = true;
            this.m_LoginButton.Click += new System.EventHandler(this.LoginButton_Click);
            // 
            // m_ConnectButton
            // 
            this.m_ConnectButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.m_ConnectButton.Location = new System.Drawing.Point(6, 51);
            this.m_ConnectButton.Name = "m_ConnectButton";
            this.m_ConnectButton.Size = new System.Drawing.Size(129, 20);
            this.m_ConnectButton.TabIndex = 0;
            this.m_ConnectButton.Text = "Connect";
            this.m_ConnectButton.UseVisualStyleBackColor = true;
            this.m_ConnectButton.Visible = false;
            this.m_ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // versionTab
            // 
            this.versionTab.Controls.Add(this.websocketPage);
            this.versionTab.Location = new System.Drawing.Point(7, 13);
            this.versionTab.Name = "versionTab";
            this.versionTab.SelectedIndex = 0;
            this.versionTab.Size = new System.Drawing.Size(775, 545);
            this.versionTab.TabIndex = 8;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.versionTab);
            this.Name = "MainForm";
            this.Text = "Tucxbot Open Source";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.websocketPage.ResumeLayout(false);
            this.m_ModGroup.ResumeLayout(false);
            this.m_WhisperGroup.ResumeLayout(false);
            this.m_WhisperGroup.PerformLayout();
            this.m_ChannelGroup.ResumeLayout(false);
            this.m_ChannelGroup.PerformLayout();
            this.m_ConnectionGroup.ResumeLayout(false);
            this.versionTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage websocketPage;
        private System.Windows.Forms.GroupBox m_ChannelGroup;
        private System.Windows.Forms.TextBox m_WhisperUserTB;
        private System.Windows.Forms.ComboBox m_ChannelMessageSelectCB;
        private System.Windows.Forms.RichTextBox m_ChannelInputTB;
        private System.Windows.Forms.RichTextBox m_ChannelOutputTB;
        private System.Windows.Forms.Button m_LeaveChannelButton;
        private System.Windows.Forms.ComboBox m_ChannelLeaveCB;
        private System.Windows.Forms.Button m_JoinChannelButton;
        private System.Windows.Forms.TextBox m_ChannelJoinTB;
        private System.Windows.Forms.GroupBox m_ConnectionGroup;
        private System.Windows.Forms.Button m_LoginButton;
        private System.Windows.Forms.Button m_ConnectButton;
        private System.Windows.Forms.TabControl versionTab;
        private System.Windows.Forms.Button m_DisconnectButton;
        private System.Windows.Forms.GroupBox m_WhisperGroup;
        private System.Windows.Forms.RichTextBox m_WhisperInputTB;
        private System.Windows.Forms.RichTextBox m_WhisperOutputTB;
        private System.Windows.Forms.Label m_ChatChannelLabel;
        private System.Windows.Forms.Label m_WhisperLabel;
        private System.Windows.Forms.GroupBox m_EventGroup;
        private System.Windows.Forms.GroupBox m_ModGroup;
        private System.Windows.Forms.Button m_LoadModButton;
        private System.Windows.Forms.Button m_UnloadModButton;
        private System.Windows.Forms.ComboBox m_UnloadModCB;
    }
}

