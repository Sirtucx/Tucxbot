namespace SubscriberNotificationEditor
{
    partial class SubscriberNotificationEditorForm
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
            this.m_GenerateGroup = new System.Windows.Forms.GroupBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.m_OpenGroup = new System.Windows.Forms.GroupBox();
            this.btnOpen = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.m_AddUserButton = new System.Windows.Forms.Button();
            this.m_UserTB = new System.Windows.Forms.TextBox();
            this.m_UserGroup = new System.Windows.Forms.GroupBox();
            this.m_EditButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.m_MessageInputTB = new System.Windows.Forms.RichTextBox();
            this.m_MessageTypeCB = new System.Windows.Forms.ComboBox();
            this.m_DeleteButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.m_UserCB = new System.Windows.Forms.ComboBox();
            this.m_SaveButton = new System.Windows.Forms.Button();
            this.rtBoxLog = new System.Windows.Forms.RichTextBox();
            this.m_GenerateGroup.SuspendLayout();
            this.m_OpenGroup.SuspendLayout();
            this.m_UserGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_GenerateGroup
            // 
            this.m_GenerateGroup.Controls.Add(this.btnGenerate);
            this.m_GenerateGroup.Location = new System.Drawing.Point(12, 9);
            this.m_GenerateGroup.Name = "m_GenerateGroup";
            this.m_GenerateGroup.Size = new System.Drawing.Size(168, 73);
            this.m_GenerateGroup.TabIndex = 0;
            this.m_GenerateGroup.TabStop = false;
            this.m_GenerateGroup.Text = "Generate Settings File";
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(7, 21);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(155, 40);
            this.btnGenerate.TabIndex = 0;
            this.btnGenerate.Text = "Save to";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.GenerateButton_Click);
            // 
            // m_OpenGroup
            // 
            this.m_OpenGroup.Controls.Add(this.btnOpen);
            this.m_OpenGroup.Location = new System.Drawing.Point(208, 9);
            this.m_OpenGroup.Name = "m_OpenGroup";
            this.m_OpenGroup.Size = new System.Drawing.Size(168, 73);
            this.m_OpenGroup.TabIndex = 1;
            this.m_OpenGroup.TabStop = false;
            this.m_OpenGroup.Text = "Open Settings File";
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(7, 21);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(155, 40);
            this.btnOpen.TabIndex = 0;
            this.btnOpen.Text = "Open";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.OpenButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Enter username";
            // 
            // m_AddUserButton
            // 
            this.m_AddUserButton.Location = new System.Drawing.Point(133, 30);
            this.m_AddUserButton.Name = "m_AddUserButton";
            this.m_AddUserButton.Size = new System.Drawing.Size(218, 20);
            this.m_AddUserButton.TabIndex = 3;
            this.m_AddUserButton.Text = "Add";
            this.m_AddUserButton.UseVisualStyleBackColor = true;
            this.m_AddUserButton.Click += new System.EventHandler(this.AddUserButton_Click);
            // 
            // m_UserTB
            // 
            this.m_UserTB.Location = new System.Drawing.Point(7, 31);
            this.m_UserTB.Name = "m_UserTB";
            this.m_UserTB.Size = new System.Drawing.Size(120, 20);
            this.m_UserTB.TabIndex = 1;
            // 
            // m_UserGroup
            // 
            this.m_UserGroup.Controls.Add(this.m_EditButton);
            this.m_UserGroup.Controls.Add(this.label1);
            this.m_UserGroup.Controls.Add(this.m_MessageInputTB);
            this.m_UserGroup.Controls.Add(this.m_MessageTypeCB);
            this.m_UserGroup.Controls.Add(this.label2);
            this.m_UserGroup.Controls.Add(this.m_DeleteButton);
            this.m_UserGroup.Controls.Add(this.m_AddUserButton);
            this.m_UserGroup.Controls.Add(this.label3);
            this.m_UserGroup.Controls.Add(this.m_UserTB);
            this.m_UserGroup.Controls.Add(this.m_UserCB);
            this.m_UserGroup.Location = new System.Drawing.Point(19, 91);
            this.m_UserGroup.Name = "m_UserGroup";
            this.m_UserGroup.Size = new System.Drawing.Size(357, 169);
            this.m_UserGroup.TabIndex = 3;
            this.m_UserGroup.TabStop = false;
            this.m_UserGroup.Text = "User Data";
            this.m_UserGroup.Visible = false;
            // 
            // m_EditButton
            // 
            this.m_EditButton.Location = new System.Drawing.Point(7, 139);
            this.m_EditButton.Name = "m_EditButton";
            this.m_EditButton.Size = new System.Drawing.Size(120, 23);
            this.m_EditButton.TabIndex = 9;
            this.m_EditButton.Text = "Save Entry";
            this.m_EditButton.UseVisualStyleBackColor = true;
            this.m_EditButton.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Message Type";
            // 
            // m_MessageInputTB
            // 
            this.m_MessageInputTB.Location = new System.Drawing.Point(133, 98);
            this.m_MessageInputTB.Name = "m_MessageInputTB";
            this.m_MessageInputTB.Size = new System.Drawing.Size(218, 65);
            this.m_MessageInputTB.TabIndex = 7;
            this.m_MessageInputTB.Text = "";
            // 
            // m_MessageTypeCB
            // 
            this.m_MessageTypeCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_MessageTypeCB.FormattingEnabled = true;
            this.m_MessageTypeCB.Items.AddRange(new object[] {
            "Tier One Message",
            "Tier Two Message",
            "Tier Three Message",
            "Twitch Prime Message",
            "Gifting Message",
            "Substreak Message"});
            this.m_MessageTypeCB.Location = new System.Drawing.Point(6, 111);
            this.m_MessageTypeCB.Name = "m_MessageTypeCB";
            this.m_MessageTypeCB.Size = new System.Drawing.Size(121, 21);
            this.m_MessageTypeCB.TabIndex = 6;
            this.m_MessageTypeCB.SelectedIndexChanged += new System.EventHandler(this.MessageTypeCB_SelectedIndexChanged);
            // 
            // m_DeleteButton
            // 
            this.m_DeleteButton.Location = new System.Drawing.Point(133, 70);
            this.m_DeleteButton.Name = "m_DeleteButton";
            this.m_DeleteButton.Size = new System.Drawing.Size(218, 21);
            this.m_DeleteButton.TabIndex = 4;
            this.m_DeleteButton.Text = "Remove";
            this.m_DeleteButton.UseVisualStyleBackColor = true;
            this.m_DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Username";
            // 
            // m_UserCB
            // 
            this.m_UserCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_UserCB.FormattingEnabled = true;
            this.m_UserCB.Location = new System.Drawing.Point(6, 71);
            this.m_UserCB.Name = "m_UserCB";
            this.m_UserCB.Size = new System.Drawing.Size(121, 21);
            this.m_UserCB.TabIndex = 0;
            // 
            // m_SaveButton
            // 
            this.m_SaveButton.Location = new System.Drawing.Point(19, 266);
            this.m_SaveButton.Name = "m_SaveButton";
            this.m_SaveButton.Size = new System.Drawing.Size(356, 33);
            this.m_SaveButton.TabIndex = 4;
            this.m_SaveButton.Text = "Save";
            this.m_SaveButton.UseVisualStyleBackColor = true;
            this.m_SaveButton.Visible = false;
            this.m_SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // rtBoxLog
            // 
            this.rtBoxLog.Location = new System.Drawing.Point(19, 316);
            this.rtBoxLog.Name = "rtBoxLog";
            this.rtBoxLog.Size = new System.Drawing.Size(356, 89);
            this.rtBoxLog.TabIndex = 5;
            this.rtBoxLog.Text = "";
            // 
            // SubscriberNotificationEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, 409);
            this.Controls.Add(this.rtBoxLog);
            this.Controls.Add(this.m_SaveButton);
            this.Controls.Add(this.m_UserGroup);
            this.Controls.Add(this.m_OpenGroup);
            this.Controls.Add(this.m_GenerateGroup);
            this.Name = "SubscriberNotificationEditorForm";
            this.Text = "Music Intro Mod Editor";
            this.m_GenerateGroup.ResumeLayout(false);
            this.m_OpenGroup.ResumeLayout(false);
            this.m_UserGroup.ResumeLayout(false);
            this.m_UserGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox m_GenerateGroup;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.GroupBox m_OpenGroup;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button m_AddUserButton;
        private System.Windows.Forms.TextBox m_UserTB;
        private System.Windows.Forms.GroupBox m_UserGroup;
        private System.Windows.Forms.ComboBox m_UserCB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button m_SaveButton;
        private System.Windows.Forms.RichTextBox rtBoxLog;
        private System.Windows.Forms.Button m_DeleteButton;
        private System.Windows.Forms.ComboBox m_MessageTypeCB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox m_MessageInputTB;
        private System.Windows.Forms.Button m_EditButton;
    }
}