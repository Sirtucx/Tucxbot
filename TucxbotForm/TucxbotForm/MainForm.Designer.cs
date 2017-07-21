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
            this.SuspendLayout();
            // 
            // btnJoin
            // 
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
            this.tBoxLeave.Location = new System.Drawing.Point(246, 33);
            this.tBoxLeave.Name = "tBoxLeave";
            this.tBoxLeave.Size = new System.Drawing.Size(75, 20);
            this.tBoxLeave.TabIndex = 3;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 104);
            this.Controls.Add(this.tBoxLeave);
            this.Controls.Add(this.tBoxJoin);
            this.Controls.Add(this.btnLeave);
            this.Controls.Add(this.btnJoin);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnJoin;
        private System.Windows.Forms.Button btnLeave;
        private System.Windows.Forms.TextBox tBoxJoin;
        private System.Windows.Forms.TextBox tBoxLeave;
    }
}

