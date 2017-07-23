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
using System.Reflection;
using System.IO;

namespace TucxbotForm
{
    public partial class MainForm : Form
    {
        private Twitch m_TwitchInstance;
        private int m_iPreviousIndex;
        private List<string> m_sPreviousChatMessages;
        private List<string> m_sJoinedChannels;
        private List<IChannelInputMod> m_ChannelInputMods;
        private List<IWhisperInputMod> m_WhisperInputMods;
        private List<IChannelJoinMod> m_ChannelJoinMods;

        public MainForm()
        {
            InitializeComponent();

            m_sJoinedChannels = new List<string>();
            m_sPreviousChatMessages = new List<string>();
            LoadMods();
            m_TwitchInstance = Twitch.Instance;
            m_TwitchInstance.OnChannelIRCConnected += OnChannelIRCConnected;
            m_TwitchInstance.OnChannelInput += OnChannelInput;
            m_TwitchInstance.OnWhisperInput += OnWhisperInput;
            m_TwitchInstance.OnChannelPart += OnChannelPart;
            m_TwitchInstance.OnChannelEnter += OnChannelEnter;
            m_TwitchInstance.StartConnection();
        }

        private void LoadMods()
        {
            if (!Directory.Exists(Environment.CurrentDirectory + "/Mods/"))
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + "/Mods/");
            }
            foreach (var file in Directory.EnumerateFiles(Environment.CurrentDirectory + "/Mods/", "*.dll"))
            {
                Assembly assembly = Assembly.LoadFile(file);
                foreach(Type type in assembly.GetTypes())
                {
                    if (typeof(IChannelInputMod).IsAssignableFrom(type))
                    {
                        if (m_ChannelInputMods == null)
                        {
                            m_ChannelInputMods = new List<IChannelInputMod>();
                        }
                        m_ChannelInputMods.Add(type.GetConstructor(new Type[] { }).Invoke(null) as IChannelInputMod);
                        Console.WriteLine("Loading Channel Input Mod: {0} to the project!", file);
                    }
                    else if (typeof(IWhisperInputMod).IsAssignableFrom(type))
                    {
                        if (m_WhisperInputMods == null)
                        {
                            m_WhisperInputMods = new List<IWhisperInputMod>();
                        }
                        m_WhisperInputMods.Add(type.GetConstructor(new Type[] { }).Invoke(null) as IWhisperInputMod);
                        Console.WriteLine("Loading Whisper Input Mod: {0} to the project!", file);
                    }
                    else if (typeof(IChannelJoinMod).IsAssignableFrom(type))
                    {
                        if (m_ChannelJoinMods == null)
                        {
                            m_ChannelJoinMods = new List<IChannelJoinMod>();
                        }
                        m_ChannelJoinMods.Add(type.GetConstructor(new Type[] { }).Invoke(null) as IChannelJoinMod);
                        Console.WriteLine("Loading Channel Join Mod: {0} to the project!", file);
                    }
                }
            }
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
            if (e.Username == m_TwitchInstance.Username)
            {
                if (m_ChannelJoinMods != null)
                {
                    foreach (IChannelJoinMod cjMod in m_ChannelJoinMods)
                    {
                        cjMod.ProcessJoin(e.Target);
                    }
                }
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
            if (m_ChannelInputMods != null)
            {
                foreach (IChannelInputMod cMod in m_ChannelInputMods)
                {
                    cMod.ProcessMessage(e.Target, e.Sender, e.Message);
                }
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
            if (m_WhisperInputMods != null)
            {
                foreach (IWhisperInputMod wMod in m_WhisperInputMods)
                {
                    wMod.ProcessMessage(e.Sender, e.Message);
                }
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

                    cBoxChannels.Items.Add(sNewText);
                    cBoxJoinedChannels.Items.Add(sNewText);
                    cBoxChannels.Enabled = true;
                    cBoxJoinedChannels.Enabled = true;

                    if (cBoxChannels.Items.Count == 1)
                    {
                        cBoxChannels.SelectedIndex = 0;
                    }
                    if (cBoxJoinedChannels.Items.Count == 1)
                    {
                        cBoxJoinedChannels.SelectedIndex = 0;
                    }
                }
                tBoxJoin.Clear();
            }
        }
        private void btnLeave_Click(object sender, EventArgs e)
        {
            string sChannel = cBoxChannels.Items[cBoxChannels.SelectedIndex].ToString();
            m_TwitchInstance.LeaveChannel(sChannel);

            cBoxChannels.Items.Remove(sChannel);
            cBoxJoinedChannels.Items.Remove(sChannel);
            m_sJoinedChannels.Remove(sChannel);


            if (cBoxChannels.Items.Count == 0)
            {
                cBoxChannels.Enabled = false;
            }
            else
            {
                cBoxChannels.SelectedIndex = 0;
            }

            if (cBoxJoinedChannels.Items.Count == 0)
            {
                cBoxJoinedChannels.Enabled = false;
            }
            else
            {
                cBoxJoinedChannels.SelectedIndex = 0;
            }
        }
        private void rBtnChat_CheckedChanged(object sender, EventArgs e)
        {
            cBoxJoinedChannels.Visible = true;
            tBoxWhisper.Visible = false;
            tBoxWhisper.Text = "Username to whisper";
        }
        private void rBtnWhisper_CheckedChanged(object sender, EventArgs e)
        {
            cBoxJoinedChannels.Visible = false;
            tBoxWhisper.Visible = true;
            cBoxChannels.Text = "Select a channel";
        }
        private void tBoxWhisper_MouseClick(object sender, MouseEventArgs e)
        {
            tBoxWhisper.Clear();
        }
        private void tBoxWhisper_Enter(object sender, EventArgs e)
        {
            tBoxWhisper.Clear();
        }
        private void tBoxWhisper_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tBoxWhisper.Text))
            {
                tBoxWhisper.Text = "Username to whisper";
            }
        }
        private void tBoxJoin_Enter(object sender, EventArgs e)
        {
            tBoxJoin.Clear();
        }
        private void tBoxJoin_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tBoxJoin.Text))
            {
                tBoxJoin.Text = "Channel to join";
            }
        }
        private void rTBoxChatOutput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return && !string.IsNullOrEmpty(rTBoxChatOutput.Text))
            {
                string sMessage = rTBoxChatOutput.Text.Trim();
                m_sPreviousChatMessages.Add(sMessage);
                rTBoxChatOutput.Clear();

                string sTarget = (rBtnChat.Checked ? cBoxJoinedChannels.SelectedItem.ToString() : tBoxWhisper.Text);

                if (rBtnChat.Checked)
                {
                    m_TwitchInstance.SendMessage(sTarget, sMessage);
                    rTBoxChat.AppendText("#" + sTarget + "|(" + m_TwitchInstance.Username + "): " + sMessage + "\n");
                }
                else
                {
                    m_TwitchInstance.SendWhisper(sTarget, sMessage);
                    rTBoxChat.AppendText(m_TwitchInstance.Username + ": " + sMessage + "\n");
                }
                
            }
            
        }
        private void rTBoxChatOutput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up && m_sPreviousChatMessages.Count > 0)
            {
                if (m_iPreviousIndex == m_sPreviousChatMessages.Count)
                {
                    m_iPreviousIndex -= 1;
                }
                rTBoxChatOutput.Text = m_sPreviousChatMessages[m_sPreviousChatMessages.Count - 1 - m_iPreviousIndex];
                ++m_iPreviousIndex;
            }
        }
    }
}
