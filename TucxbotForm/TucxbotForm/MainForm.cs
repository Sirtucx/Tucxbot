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
        Twitch m_TwitchInstance;
        List<string> m_sJoinedChannels;
        public MainForm()
        {
            InitializeComponent();

            m_sJoinedChannels = new List<string>();
            m_TwitchInstance = Twitch.Instance;
            m_TwitchInstance.OnChannelIRCConnected += OnChannelIRCConnected;
            m_TwitchInstance.OnChannelInput += OnChannelInput;
            m_TwitchInstance.OnWhisperInput += OnWhisperInput;
            m_TwitchInstance.OnChannelPart += OnChannelPart;
            m_TwitchInstance.OnChannelEnter += OnChannelEnter;
            m_TwitchInstance.StartConnection();
        }

        private void OnChannelIRCConnected()
        {
            if (btnJoin.InvokeRequired)
            {
                btnJoin.Invoke(new Action(delegate { OnChannelIRCConnected(); }));
                return;
            }
            btnJoin.Enabled = true;
        }
        private void OnChannelEnter(object sender, ChannelGateInteractionEventArgs e)
        {
            if (rTBoxEvents.InvokeRequired)
            {
                rTBoxEvents.Invoke(new Action(delegate { OnChannelEnter(sender, e); }));
                return;
            }
            rTBoxEvents.AppendText(e.Username + " has joined " + e.Target + "\'s channel!\n");
        }
        private void OnChannelPart(object sender, ChannelGateInteractionEventArgs e)
        {
            if (rTBoxEvents.InvokeRequired)
            {
                rTBoxEvents.Invoke(new Action(delegate { OnChannelPart(sender, e); }));
                return;
            }
            rTBoxEvents.AppendText(e.Username + " has left " + e.Target + "\'s channel!\n");
        }
        private void OnChannelInput(object sender, ChatInputEventArgs e)
        {
            if (rTBoxChat.InvokeRequired)
            {
                rTBoxChat.Invoke(new Action(delegate { OnChannelInput(sender, e); }));
                return;
            }

            rTBoxChat.AppendText("#" + e.Target + "|(" + e.Sender + "): " + e.Message + "\n");
        }
        private void OnWhisperInput(object sender, ChatInputEventArgs e)
        {
            if (rTBoxChat.InvokeRequired)
            {
                rTBoxChat.Invoke(new Action(delegate { OnWhisperInput(sender, e); }));
                return;
            }

            rTBoxChat.AppendText(e.Sender + ": " + e.Message + "\n");
        }



        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach(string channel in m_sJoinedChannels)
            {
                m_TwitchInstance.LeaveChannel(channel);
            }
            m_TwitchInstance.OnChannelInput -= OnChannelInput;
            m_TwitchInstance.OnWhisperInput -= OnWhisperInput;
            m_TwitchInstance.OnChannelPart -= OnChannelPart;
            m_TwitchInstance.OnChannelEnter -= OnChannelEnter;
            m_TwitchInstance.OnChannelIRCConnected -= OnChannelIRCConnected;
            m_TwitchInstance.CloseConnection();
        }

        private void btnJoin_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tBoxJoin.Text))
            {
                string sNewText = tBoxJoin.Text.Trim().ToLower();
                if (!m_sJoinedChannels.Contains(sNewText))
                {
                    m_sJoinedChannels.Add(sNewText);
                    m_TwitchInstance.JoinChannel(sNewText);
                    btnLeave.Enabled = true;
                }
                tBoxJoin.Clear();

                
            }
        }

        private void btnLeave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tBoxLeave.Text))
            {
                string sNewText = tBoxLeave.Text.Trim().ToLower();
                if (m_sJoinedChannels.Contains(sNewText))
                {
                    m_sJoinedChannels.Remove(sNewText);
                    m_TwitchInstance.LeaveChannel(sNewText);

                    if (m_sJoinedChannels.Count == 0)
                    {
                        btnLeave.Enabled = false;
                    }
                }
                tBoxLeave.Clear();
            }
        }
    }
}
