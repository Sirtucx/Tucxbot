namespace MusicIntroEditor
{
    partial class MusicIntroEditorForm
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
            this.groupGenerate = new System.Windows.Forms.GroupBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnOpen = new System.Windows.Forms.Button();
            this.groupAddUser = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAddUser = new System.Windows.Forms.Button();
            this.btnLoadMusic = new System.Windows.Forms.Button();
            this.userTextBox = new System.Windows.Forms.TextBox();
            this.pathTextBox = new System.Windows.Forms.TextBox();
            this.groupContents = new System.Windows.Forms.GroupBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.existingPathTextBox = new System.Windows.Forms.TextBox();
            this.userComboBox = new System.Windows.Forms.ComboBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.logRichTextBox = new System.Windows.Forms.RichTextBox();
            this.groupGenerate.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupAddUser.SuspendLayout();
            this.groupContents.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupGenerate
            // 
            this.groupGenerate.Controls.Add(this.btnGenerate);
            this.groupGenerate.Location = new System.Drawing.Point(13, 13);
            this.groupGenerate.Name = "groupGenerate";
            this.groupGenerate.Size = new System.Drawing.Size(127, 54);
            this.groupGenerate.TabIndex = 0;
            this.groupGenerate.TabStop = false;
            this.groupGenerate.Text = "Generate Settings File";
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(7, 20);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(114, 23);
            this.btnGenerate.TabIndex = 0;
            this.btnGenerate.Text = "Save to";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.GenerateButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnOpen);
            this.groupBox2.Location = new System.Drawing.Point(13, 73);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(127, 61);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Open Settings File";
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(7, 19);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(114, 26);
            this.btnOpen.TabIndex = 0;
            this.btnOpen.Text = "Open";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.OpenButton_Click);
            // 
            // groupAddUser
            // 
            this.groupAddUser.Controls.Add(this.label2);
            this.groupAddUser.Controls.Add(this.label1);
            this.groupAddUser.Controls.Add(this.btnAddUser);
            this.groupAddUser.Controls.Add(this.btnLoadMusic);
            this.groupAddUser.Controls.Add(this.userTextBox);
            this.groupAddUser.Controls.Add(this.pathTextBox);
            this.groupAddUser.Location = new System.Drawing.Point(157, 18);
            this.groupAddUser.Name = "groupAddUser";
            this.groupAddUser.Size = new System.Drawing.Size(296, 129);
            this.groupAddUser.TabIndex = 2;
            this.groupAddUser.TabStop = false;
            this.groupAddUser.Text = "Add/Edit User";
            this.groupAddUser.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Enter username";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Load Music File:";
            // 
            // btnAddUser
            // 
            this.btnAddUser.Location = new System.Drawing.Point(7, 89);
            this.btnAddUser.Name = "btnAddUser";
            this.btnAddUser.Size = new System.Drawing.Size(102, 23);
            this.btnAddUser.TabIndex = 3;
            this.btnAddUser.Text = "Add/Edit User";
            this.btnAddUser.UseVisualStyleBackColor = true;
            this.btnAddUser.Click += new System.EventHandler(this.AddUserButton_Click);
            // 
            // btnLoadMusic
            // 
            this.btnLoadMusic.Location = new System.Drawing.Point(7, 36);
            this.btnLoadMusic.Name = "btnLoadMusic";
            this.btnLoadMusic.Size = new System.Drawing.Size(65, 23);
            this.btnLoadMusic.TabIndex = 2;
            this.btnLoadMusic.Text = "Open File";
            this.btnLoadMusic.UseVisualStyleBackColor = true;
            this.btnLoadMusic.Click += new System.EventHandler(this.LoadMusicButton_Click);
            // 
            // userTextBox
            // 
            this.userTextBox.Location = new System.Drawing.Point(138, 91);
            this.userTextBox.Name = "userTextBox";
            this.userTextBox.Size = new System.Drawing.Size(152, 20);
            this.userTextBox.TabIndex = 1;
            // 
            // pathTextBox
            // 
            this.pathTextBox.Location = new System.Drawing.Point(78, 39);
            this.pathTextBox.Name = "pathTextBox";
            this.pathTextBox.ReadOnly = true;
            this.pathTextBox.Size = new System.Drawing.Size(212, 20);
            this.pathTextBox.TabIndex = 0;
            // 
            // groupContents
            // 
            this.groupContents.Controls.Add(this.btnDelete);
            this.groupContents.Controls.Add(this.label4);
            this.groupContents.Controls.Add(this.label3);
            this.groupContents.Controls.Add(this.existingPathTextBox);
            this.groupContents.Controls.Add(this.userComboBox);
            this.groupContents.Location = new System.Drawing.Point(20, 154);
            this.groupContents.Name = "groupContents";
            this.groupContents.Size = new System.Drawing.Size(433, 103);
            this.groupContents.TabIndex = 3;
            this.groupContents.TabStop = false;
            this.groupContents.Text = "User Data";
            this.groupContents.Visible = false;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(12, 71);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(415, 23);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "Remove";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(155, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Music Path";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Username";
            // 
            // existingPathTextBox
            // 
            this.existingPathTextBox.Location = new System.Drawing.Point(155, 43);
            this.existingPathTextBox.Name = "existingPathTextBox";
            this.existingPathTextBox.ReadOnly = true;
            this.existingPathTextBox.Size = new System.Drawing.Size(272, 20);
            this.existingPathTextBox.TabIndex = 1;
            // 
            // userComboBox
            // 
            this.userComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.userComboBox.FormattingEnabled = true;
            this.userComboBox.Location = new System.Drawing.Point(12, 43);
            this.userComboBox.Name = "userComboBox";
            this.userComboBox.Size = new System.Drawing.Size(121, 21);
            this.userComboBox.TabIndex = 0;
            this.userComboBox.SelectedIndexChanged += new System.EventHandler(this.UserComboBox_SelectedIndexChanged);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(20, 263);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(427, 33);
            this.saveButton.TabIndex = 4;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Visible = false;
            this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // logRichTextBox
            // 
            this.logRichTextBox.Location = new System.Drawing.Point(20, 316);
            this.logRichTextBox.Name = "logRichTextBox";
            this.logRichTextBox.Size = new System.Drawing.Size(427, 96);
            this.logRichTextBox.TabIndex = 5;
            this.logRichTextBox.Text = "";
            // 
            // MusicIntroEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(465, 437);
            this.Controls.Add(this.logRichTextBox);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.groupContents);
            this.Controls.Add(this.groupAddUser);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupGenerate);
            this.Name = "MusicIntroEditorForm";
            this.Text = "Music Intro Mod Editor";
            this.groupGenerate.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupAddUser.ResumeLayout(false);
            this.groupAddUser.PerformLayout();
            this.groupContents.ResumeLayout(false);
            this.groupContents.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupGenerate;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.GroupBox groupAddUser;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAddUser;
        private System.Windows.Forms.Button btnLoadMusic;
        private System.Windows.Forms.TextBox userTextBox;
        private System.Windows.Forms.TextBox pathTextBox;
        private System.Windows.Forms.GroupBox groupContents;
        private System.Windows.Forms.TextBox existingPathTextBox;
        private System.Windows.Forms.ComboBox userComboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.RichTextBox logRichTextBox;
        private System.Windows.Forms.Button btnDelete;
    }
}

