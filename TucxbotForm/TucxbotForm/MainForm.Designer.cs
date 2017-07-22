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
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tTBoxWhispers = new System.Windows.Forms.RichTextBox();
            this.rTBoxChannels = new System.Windows.Forms.RichTextBox();
            this.lblGate = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
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
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tTBoxWhispers);
            this.groupBox1.Controls.Add(this.rTBoxChannels);
            this.groupBox1.Location = new System.Drawing.Point(14, 180);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(307, 223);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Chat Boxes";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(162, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Whispers";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Channel";
            // 
            // tTBoxWhispers
            // 
            this.tTBoxWhispers.Enabled = false;
            this.tTBoxWhispers.Location = new System.Drawing.Point(162, 37);
            this.tTBoxWhispers.Name = "tTBoxWhispers";
            this.tTBoxWhispers.Size = new System.Drawing.Size(139, 180);
            this.tTBoxWhispers.TabIndex = 1;
            this.tTBoxWhispers.Text = "";
            // 
            // rTBoxChannels
            // 
            this.rTBoxChannels.Enabled = false;
            this.rTBoxChannels.Location = new System.Drawing.Point(7, 37);
            this.rTBoxChannels.Name = "rTBoxChannels";
            this.rTBoxChannels.Size = new System.Drawing.Size(139, 180);
            this.rTBoxChannels.TabIndex = 0;
            this.rTBoxChannels.Text = "";
            // 
            // lblGate
            // 
            this.lblGate.AutoSize = true;
            this.lblGate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGate.Location = new System.Drawing.Point(12, 106);
            this.lblGate.MaximumSize = new System.Drawing.Size(300, 35);
            this.lblGate.Name = "lblGate";
            this.lblGate.Size = new System.Drawing.Size(0, 13);
            this.lblGate.TabIndex = 5;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(332, 415);
            this.Controls.Add(this.lblGate);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tBoxLeave);
            this.Controls.Add(this.tBoxJoin);
            this.Controls.Add(this.btnLeave);
            this.Controls.Add(this.btnJoin);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnJoin;
        private System.Windows.Forms.Button btnLeave;
        private System.Windows.Forms.TextBox tBoxJoin;
        private System.Windows.Forms.TextBox tBoxLeave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox tTBoxWhispers;
        private System.Windows.Forms.RichTextBox rTBoxChannels;
        private System.Windows.Forms.Label lblGate;
    }
}

