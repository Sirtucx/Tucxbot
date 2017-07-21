using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TwitchIRC;

namespace TucxbotForm
{
    public partial class MainForm : Form
    {
        Twitch myInstance;
        public MainForm()
        {
            InitializeComponent();


            myInstance = Twitch.Instance;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            myInstance.CloseConnection();
        }

        private void btnJoin_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tBoxJoin.Text))
            {
                string sNewText = tBoxJoin.Text.Trim().ToLower();
                myInstance.JoinChannel(sNewText);
            }
        }

        private void btnLeave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tBoxLeave.Text))
            {
                string sNewText = tBoxLeave.Text.Trim().ToLower();
                myInstance.LeaveChannel(sNewText);
            }
        }
    }
}
