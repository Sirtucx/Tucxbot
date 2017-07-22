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
            this.tBoxLeave = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rTBoxChat = new System.Windows.Forms.RichTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rTBoxEvents = new System.Windows.Forms.RichTextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnJoin
            // 
            this.btnJoin.Enabled = false;
            this.btnJoin.Location = new System.Drawing.Point(13, 69);
            this.btnJoin.Name = "btnJoin";
            this.btnJoin.Size = new System.Drawing.Size(75, 23);
            this.btnJoin.TabIndex = 0;
            this.btnJoin.Text = "Join";
            this.btnJoin.UseVisualStyleBackColor = true;
            this.btnJoin.Click += new System.EventHandler(this.btnJoin_Click);
            // 
            // btnLeave
            // 
            this.btnLeave.Enabled = false;
            this.btnLeave.Location = new System.Drawing.Point(246, 69);
            this.btnLeave.Name = "btnLeave";
            this.btnLeave.Size = new System.Drawing.Size(75, 23);
            this.btnLeave.TabIndex = 1;
            this.btnLeave.Text = "Leave";
            this.btnLeave.UseVisualStyleBackColor = true;
            this.btnLeave.Click += new System.EventHandler(this.btnLeave_Click);
            // 
            // tBoxJoin
            // 
            this.tBoxJoin.Location = new System.Drawing.Point(13, 33);
            this.tBoxJoin.Name = "tBoxJoin";
            this.tBoxJoin.Size = new System.Drawing.Size(75, 20);
            this.tBoxJoin.TabIndex = 2;
            // 
            // tBoxLeave
            // 
            this.tBoxLeave.Location = new System.Drawing.Point(245, 33);
            this.tBoxLeave.Name = "tBoxLeave";
            this.tBoxLeave.Size = new System.Drawing.Size(75, 20);
            this.tBoxLeave.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rTBoxChat);
            this.groupBox1.Location = new System.Drawing.Point(14, 180);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(307, 223);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Chat";
            // 
            // rTBoxChannels
            // 
            this.rTBoxChat.Location = new System.Drawing.Point(7, 19);
            this.rTBoxChat.Name = "rTBoxChannels";
            this.rTBoxChat.ReadOnly = true;
            this.rTBoxChat.Size = new System.Drawing.Size(288, 198);
            this.rTBoxChat.TabIndex = 0;
            this.rTBoxChat.Text = "";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rTBoxEvents);
            this.groupBox2.Location = new System.Drawing.Point(13, 99);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(302, 75);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Event Logs";
            // 
            // rTBoxEvents
            // 
            this.rTBoxEvents.Location = new System.Drawing.Point(9, 16);
            this.rTBoxEvents.Name = "rTBoxEvents";
            this.rTBoxEvents.ReadOnly = true;
            this.rTBoxEvents.Size = new System.Drawing.Size(287, 50);
            this.rTBoxEvents.TabIndex = 0;
            this.rTBoxEvents.Text = "";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(332, 415);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tBoxLeave);
            this.Controls.Add(this.tBoxJoin);
            this.Controls.Add(this.btnLeave);
            this.Controls.Add(this.btnJoin);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnJoin;
        private System.Windows.Forms.Button btnLeave;
        private System.Windows.Forms.TextBox tBoxJoin;
        private System.Windows.Forms.TextBox tBoxLeave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox rTBoxChat;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RichTextBox rTBoxEvents;
    }
}

