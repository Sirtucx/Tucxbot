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
            myInstance.OnChannelIRCConnected += OnChannelIRCConnected;
            myInstance.OnChannelPart += OnChannelPart;
            myInstance.OnChannelEnter += OnChannelEnter;
            myInstance.StartConnection();
        }

        private void OnChannelIRCConnected()
        {
            if (btnJoin.InvokeRequired)
            {
                btnJoin.Invoke(new Action(delegate { OnChannelIRCConnected(); }));
            }
            btnJoin.Enabled = true;
        }

        private void OnChannelEnter(object sender, ChannelGateInteractionEventArgs e)
        {
            if (lblGate.InvokeRequired)
            {
                lblGate.Invoke(new Action(delegate { OnChannelEnter(sender, e); }));
            }
            lblGate.Text = e.Username + " has joined " + e.Target + "\'s channel!";
        }
        private void OnChannelPart(object sender, ChannelGateInteractionEventArgs e)
        {
            if (lblGate.InvokeRequired)
            {
                lblGate.Invoke(new Action(delegate { OnChannelPart(sender, e); }));
            }
            lblGate.Text = e.Username + " has left " + e.Target + "\'s channel!";
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            myInstance.OnChannelIRCConnected -= OnChannelIRCConnected;
            myInstance.CloseConnection();
        }

        private void btnJoin_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tBoxJoin.Text))
            {
                string sNewText = tBoxJoin.Text.Trim().ToLower();
                myInstance.JoinChannel(sNewText);
                tBoxJoin.Clear();
            }
        }

        private void btnLeave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tBoxLeave.Text))
            {
                string sNewText = tBoxLeave.Text.Trim().ToLower();
                myInstance.LeaveChannel(sNewText);
                tBoxLeave.Clear();
            }
        }
    }
}
