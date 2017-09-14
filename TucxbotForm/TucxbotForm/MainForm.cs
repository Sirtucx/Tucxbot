using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TwitchIRC_TCP;
using System.Reflection;
using System.IO;
using Twitch_Websocket;
using Twitch_Websocket.Mod_Interfaces;
using Newtonsoft.Json;

namespace TucxbotForm
{
    public partial class MainForm : Form
    {
        private Twitch m_TwitchInstance;
        private TwitchClient m_TwitchClient;
        private int m_iPreviousIndex;
        private List<string> m_sPreviousChatMessages;
        private List<string> m_sJoinedChannels;
        private List<IChannelInputMod> m_ChannelInputMods;
        private List<IWhisperInputMod> m_WhisperInputMods;
        private List<IChannelJoinMod> m_ChannelJoinMods;
        private List<IChatMessageMod> m_ChannelMessageMods;
        private List<IWhisperMessageMod> m_WhisperMessageMods;
        private List<IOnSubscriberMod> m_OnSubscriberMods;

        public MainForm()
        {
            InitializeComponent();
            LoadMods();
            m_TwitchInstance = Twitch.Instance;

            LoginData twitchClientCredentials = LoadCredentials();
            if (twitchClientCredentials == null)
            {
                webConnectionGroup.Visible = false;
                DialogResult warningDialog;
                string sMessage = $"You are either missing the required Settings.json file or it has not been filled in correctly. It has been created {Environment.CurrentDirectory}/Data/Settings.json\nPlease fill in Settings.json and reload the program!";
                warningDialog = MessageBox.Show(sMessage, "Tucxbot WebSocket Error");

                if (!File.Exists($"{Environment.CurrentDirectory}/Data/Settings.json"))
                {
                    twitchClientCredentials = new LoginData();
                    string sJSONContent = JsonConvert.SerializeObject(twitchClientCredentials, Formatting.Indented);

                    using (StreamWriter writer = new StreamWriter($"{Environment.CurrentDirectory}/Data/Settings.json", false))
                    {
                        writer.Write(sJSONContent);
                        writer.Flush();
                        writer.Close();
                    }
                }
            }
            else
            {
                m_TwitchClient = TwitchClient.GetInstance(new IRCCredentials(twitchClientCredentials.Username, twitchClientCredentials.OAuth));
            }
            
            m_sPreviousChatMessages = new List<string>();

            if (m_TwitchClient != null)
            {
                m_TwitchClient.OnTwitchConnected += OnTwitchClientConnected;
            }
        }
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            CloseTCP();
            ShutdownMods();
            CloseWeb();
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
                    #region TCP Mods
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
                    #endregion TCP Mods
                    
                    #region WebSocket Mods
                    if (typeof(IChatMessageMod).IsAssignableFrom(type))
                    {
                        if (m_ChannelMessageMods == null)
                        {
                            m_ChannelMessageMods = new List<IChatMessageMod>();
                        }
                        m_ChannelMessageMods.Add(type.GetConstructor(new Type[] { }).Invoke(null) as IChatMessageMod);
                        Console.WriteLine("Loading Channel Message Mod: {0} to the project!", file);
                    }
                    else if (typeof(IWhisperMessageMod).IsAssignableFrom(type))
                    {
                        if (m_WhisperMessageMods == null)
                        {
                            m_WhisperMessageMods = new List<IWhisperMessageMod>();
                        }
                        m_WhisperMessageMods.Add(type.GetConstructor(new Type[] { }).Invoke(null) as IWhisperMessageMod);
                        Console.WriteLine("Loading Whisper Message Mod: {0} to the project!", file);
                    }
                    else if (typeof(IOnSubscriberMod).IsAssignableFrom(type))
                    {
                        if (m_OnSubscriberMods == null)
                        {
                            m_OnSubscriberMods = new List<IOnSubscriberMod>();
                        }
                        m_OnSubscriberMods.Add(type.GetConstructor(new Type[] { }).Invoke(null) as IOnSubscriberMod);
                        Console.WriteLine("Loading OnSubscriber Mod: {0} to the project!", file);
                    }
                    #endregion WebSocket Mods
                }
            }
        }
        private void ShutdownMods()
        {
            if (m_ChannelInputMods != null)
            {
                foreach (IChannelInputMod icim in m_ChannelInputMods)
                {
                    icim.Shutdown();
                }
            }
            if (m_ChannelJoinMods != null)
            {
                foreach (IChannelJoinMod icjm in m_ChannelJoinMods)
                {
                    icjm.Shutdown();
                }
            }
            if (m_WhisperInputMods != null)
            {
                foreach (IWhisperInputMod iwim in m_WhisperInputMods)
                {
                    iwim.Shutdown();
                }
            }
            if (m_ChannelMessageMods != null)
            {
                foreach (IChatMessageMod mod in m_ChannelMessageMods)
                {
                    mod.Shutdown();
                }
            }
            if (m_WhisperMessageMods != null)
            {
                foreach (IChatMessageMod mod in m_WhisperMessageMods)
                {
                    mod.Shutdown();
                }
            }
            if (m_OnSubscriberMods != null)
            {
                foreach (IChatMessageMod mod in m_OnSubscriberMods)
                {
                    mod.Shutdown();
                }
            }
        }

        #region TCP
        #region TCP Twitch Form Events
        private void OnChannelIRCConnected()
        {
            if (tcpBtnJoin.InvokeRequired)
            {
                tcpBtnJoin.Invoke(new Action(delegate { OnChannelIRCConnected(); }));
                return;
            }
            tcpBtnJoin.Enabled = true;
            tcpBtnDisconnect.Enabled = true;
        }
        private void OnChannelEnter(object sender, ChannelGateInteractionEventArgs e)
        {
            if (tcpRTBoxEvents.InvokeRequired)
            {
                tcpRTBoxEvents.Invoke(new Action(delegate { OnChannelEnter(sender, e); }));
                return;
            }
            //if (e.Username == m_TwitchInstance.Username)
            //{
            //    if (m_ChannelJoinMods != null)
            //    {
            //        foreach (IChannelJoinMod cjMod in m_ChannelJoinMods)
            //        {
            //            cjMod.ProcessJoin(e.Target);
            //        }
            //    }
            //}

            tcpRTBoxEvents.AppendText(e.Username + " has joined " + e.Target + "\'s channel!\n");
        }
        private void OnChannelPart(object sender, ChannelGateInteractionEventArgs e)
        {
            if (tcpRTBoxEvents.InvokeRequired)
            {
                tcpRTBoxEvents.Invoke(new Action(delegate { OnChannelPart(sender, e); }));
                return;
            }
            tcpRTBoxEvents.AppendText(e.Username + " has left " + e.Target + "\'s channel!\n");
        }
        private void OnChannelInput(object sender, ChatInputEventArgs e)
        {
            if (tcpRTBoxChat.InvokeRequired)
            {
                tcpRTBoxChat.Invoke(new Action(delegate { OnChannelInput(sender, e); }));
                return;
            }
            //if (m_ChannelInputMods != null)
            //{
            //    foreach (IChannelInputMod cMod in m_ChannelInputMods)
            //    {
            //        cMod.ProcessMessage(e.Target, e.Sender, e.Message);
            //    }
            //}
            tcpRTBoxChat.AppendText("#" + e.Target + "|(" + e.Sender + "): " + e.Message + "\n");
        }
        private void OnWhisperInput(object sender, ChatInputEventArgs e)
        {
            if (tcpRTBoxChat.InvokeRequired)
            {
                tcpRTBoxChat.Invoke(new Action(delegate { OnWhisperInput(sender, e); }));
                return;
            }
            //if (m_WhisperInputMods != null)
            //{
            //    foreach (IWhisperInputMod wMod in m_WhisperInputMods)
            //    {
            //        wMod.ProcessMessage(e.Sender, e.Message);
            //    }
            //}
            tcpRTBoxChat.AppendText(e.Sender + ": " + e.Message + "\n");
        }
        #endregion TCP Twitch Form Events

        #region TCP Form Events
        private void TCP_BtnJoin_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tcpTBoxJoin.Text))
            {
                string sNewText = tcpTBoxJoin.Text.Trim().ToLower();
                if (!m_sJoinedChannels.Contains(sNewText))
                {
                    m_sJoinedChannels.Add(sNewText);
                    m_TwitchInstance.JoinChannel(sNewText);
                    tcpBtnLeave.Enabled = true;

                    tcpCBoxChannels.Items.Add(sNewText);
                    tcpCBoxJoinedChannels.Items.Add(sNewText);
                    tcpCBoxChannels.Enabled = true;
                    tcpCBoxJoinedChannels.Enabled = true;

                    if (tcpCBoxChannels.Items.Count == 1)
                    {
                        tcpCBoxChannels.SelectedIndex = 0;
                    }
                    if (tcpCBoxJoinedChannels.Items.Count == 1)
                    {
                        tcpCBoxJoinedChannels.SelectedIndex = 0;
                    }
                }
                tcpTBoxJoin.Clear();
            }
        }
        private void TCP_BtnLeave_Click(object sender, EventArgs e)
        {
            string sChannel = tcpCBoxChannels.Items[tcpCBoxChannels.SelectedIndex].ToString();
            m_TwitchInstance.LeaveChannel(sChannel);

            tcpCBoxChannels.Items.Remove(sChannel);
            tcpCBoxJoinedChannels.Items.Remove(sChannel);
            m_sJoinedChannels.Remove(sChannel);


            if (tcpCBoxChannels.Items.Count == 0)
            {
                tcpCBoxChannels.Enabled = false;
            }
            else
            {
                tcpCBoxChannels.SelectedIndex = 0;
            }

            if (tcpCBoxJoinedChannels.Items.Count == 0)
            {
                tcpCBoxJoinedChannels.Enabled = false;
            }
            else
            {
                tcpCBoxJoinedChannels.SelectedIndex = 0;
            }
        }
        private void TCP_RBtnChat_CheckedChanged(object sender, EventArgs e)
        {
            tcpCBoxJoinedChannels.Visible = true;
            tcpTBoxWhisper.Visible = false;
            tcpTBoxWhisper.Text = "Username to whisper";
        }
        private void TCP_RBtnWhisper_CheckedChanged(object sender, EventArgs e)
        {
            tcpCBoxJoinedChannels.Visible = false;
            tcpTBoxWhisper.Visible = true;
            tcpCBoxChannels.Text = "Select a channel";
        }
        private void TCP_TBoxWhisper_MouseClick(object sender, MouseEventArgs e)
        {
            tcpTBoxWhisper.Clear();
        }
        private void TCP_TBoxWhisper_Enter(object sender, EventArgs e)
        {
            tcpTBoxWhisper.Clear();
        }
        private void TCP_TBoxWhisper_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tcpTBoxWhisper.Text))
            {
                tcpTBoxWhisper.Text = "Username to whisper";
            }
        }
        private void TCP_TBoxJoin_Enter(object sender, EventArgs e)
        {
            tcpTBoxJoin.Clear();
        }
        private void TCP_TBoxJoin_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tcpTBoxJoin.Text))
            {
                tcpTBoxJoin.Text = "Channel to join";
            }
        }
        private void TCP_RTBoxChatOutput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return && !string.IsNullOrEmpty(tcpRTBoxChatOutput.Text))
            {
                string sMessage = tcpRTBoxChatOutput.Text.Trim();
                m_sPreviousChatMessages.Add(sMessage);
                tcpRTBoxChatOutput.Clear();

                string sTarget = (tcpRBtnChat.Checked ? tcpCBoxJoinedChannels.SelectedItem.ToString() : tcpTBoxWhisper.Text);

                if (tcpRBtnChat.Checked)
                {
                    m_TwitchInstance.SendMessage(sTarget, sMessage);
                    tcpRTBoxChat.AppendText("#" + sTarget + "|(" + m_TwitchInstance.Username + "): " + sMessage + "\n");
                }
                else
                {
                    m_TwitchInstance.SendWhisper(sTarget, sMessage);
                    tcpRTBoxChat.AppendText(m_TwitchInstance.Username + ": " + sMessage + "\n");
                }
                
            }
            
        }
        private void TCP_RTBoxChatOutput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up && m_sPreviousChatMessages.Count > 0)
            {
                if (m_iPreviousIndex == m_sPreviousChatMessages.Count)
                {
                    m_iPreviousIndex -= 1;
                }
                tcpRTBoxChatOutput.Text = m_sPreviousChatMessages[m_sPreviousChatMessages.Count - 1 - m_iPreviousIndex];
                ++m_iPreviousIndex;
            }
        }
        private void TCP_BtnConnect_Click(object sender, EventArgs e)
        {
            tcp_BtnConnect.Enabled = false;
            ConnectTCP();
        }
        private void TCP_BtnDisconnect_Click(object sender, EventArgs e)
        {
            tcp_BtnConnect.Enabled = true;
            tcpBtnDisconnect.Enabled = false;
            DisconnectTCP();
        }
        #endregion TCP Form Events

        private void ConnectTCP()
        {
            tcpBtnJoin.Enabled = false;
            m_sJoinedChannels = new List<string>();
            m_TwitchInstance.OnChannelIRCConnected += OnChannelIRCConnected;
            m_TwitchInstance.OnChannelInput += OnChannelInput;
            m_TwitchInstance.OnWhisperInput += OnWhisperInput;
            m_TwitchInstance.OnChannelPart += OnChannelPart;
            m_TwitchInstance.OnChannelEnter += OnChannelEnter;
            m_TwitchInstance.StartConnection();
        }
        private void DisconnectTCP()
        {
            tcpBtnJoin.Enabled = false;
            m_TwitchInstance.OnChannelInput -= OnChannelInput;
            m_TwitchInstance.OnWhisperInput -= OnWhisperInput;
            m_TwitchInstance.OnChannelPart -= OnChannelPart;
            m_TwitchInstance.OnChannelEnter -= OnChannelEnter;
            m_TwitchInstance.OnChannelIRCConnected -= OnChannelIRCConnected;
            m_TwitchInstance.CloseConnection();
        }
        private void CloseTCP()
        {
            DisconnectTCP();
            m_TwitchInstance.Close();
        }
        #endregion TCP

        #region WebSocket

        private LoginData LoadCredentials()
        {
            if (File.Exists($"{Environment.CurrentDirectory}/Data/Settings.json"))
            {
                StreamReader sr = new StreamReader($"{Environment.CurrentDirectory}/Data/Settings.json");
                string sContent = sr.ReadToEnd();

                LoginData loginData = JsonConvert.DeserializeObject<LoginData>(sContent);

                if (string.IsNullOrEmpty(loginData.Username) || string.IsNullOrEmpty(loginData.OAuth))
                {
                    return null;
                }
                return loginData;
            }
            return null;
        }
        private void ConnectWeb()
        {
            m_TwitchClient?.Connect();
        }
        private void DisconnectWeb()
        {
            webBtnConnect.Enabled = true;
            webBtnJoin.Enabled = false;
            UnRegisterWebEvents();
            m_TwitchClient?.Disconnect();
        }
        private void CloseWeb()
        {
            if (m_TwitchClient != null)
            {
                m_TwitchClient.OnTwitchConnected -= OnTwitchClientConnected;
            }
            DisconnectWeb();
        }

        #region Web Twitch Form Events
        private void OnTwitchClientConnected()
        {
            RegisterWebEvents();
            webJoinLeaveGroup.Visible = true;
        }

        private void RegisterWebEvents()
        {
            m_TwitchClient.OnWhisperMessageReceived += TwitchClient_OnWhisperMessageReceived;
            m_TwitchClient.OnChatMessageReceived += TwitchClient_OnChatMessageReceived;
            m_TwitchClient.OnBotJoinedChannelEvent += TwitchClient_OnBotJoinedChannelEvent;
            m_TwitchClient.OnUserLeaveEvent += TwitchClient_OnUserLeaveEvent;
        }
        private void UnRegisterWebEvents()
        {
            if (m_TwitchClient != null)
            {
                m_TwitchClient.OnWhisperMessageReceived -= TwitchClient_OnWhisperMessageReceived;
                m_TwitchClient.OnChatMessageReceived -= TwitchClient_OnChatMessageReceived;
                m_TwitchClient.OnBotJoinedChannelEvent -= TwitchClient_OnBotJoinedChannelEvent;
            }
        }

        private void TwitchClient_OnBotJoinedChannelEvent(object sender, OnBotJoinedChannelEventArgs e)
        {
            if (!webCBoxJoinedChannels.Items.Contains(e.Channel))
            {
                if (webCBoxJoinedChannels.InvokeRequired)
                {
                    webCBoxJoinedChannels.Invoke(new Action(delegate { TwitchClient_OnBotJoinedChannelEvent(sender, e); }));
                    return;
                }
                webCBoxJoinedChannels.Items.Add(e.Channel);
                webCBoxJoinedChannels.SelectedIndex = 0;
                webTBoxJoin.Clear();
                webCBoxJoinedChannels.Visible = true;
                webBtnLeave.Visible = true;

                webChatGroup.Visible = true;
                webCBoxPublicChannels.Items.Add(e.Channel);
                webCBoxPublicChannels.SelectedIndex = 0;
            }
        }
        private void TwitchClient_OnUserLeaveEvent(object sender, OnUserLeaveEventArgs e)
        {
            if (webCBoxJoinedChannels.InvokeRequired)
            {
                webCBoxJoinedChannels.Invoke(new Action(delegate { TwitchClient_OnUserLeaveEvent(sender, e); }));
                return;
            }

            if (e.Username == m_TwitchClient.Credentials.TwitchUsername)
            {
                webCBoxJoinedChannels.Items.Remove(e.Channel);

                webCBoxJoinedChannels.Visible = webCBoxJoinedChannels.Items.Count > 0;
                webBtnLeave.Visible = webCBoxJoinedChannels.Items.Count > 0;

                webCBoxPublicChannels.Items.Remove(e.Channel);
                webChatGroup.Visible = webCBoxJoinedChannels.Items.Count > 0;
            }
        }

        private void TwitchClient_OnChatMessageReceived(object sender, OnChatMessageReceivedEventArgs e)
        {
            if (m_ChannelMessageMods != null)
            {
                foreach(IChatMessageMod mod in m_ChannelMessageMods)
                {
                    mod.Process(e.ChatMessage);
                }
            }
            Console.WriteLine($"#{e.ChatMessage.Channel}\n{e.ChatMessage.Username}: {e.ChatMessage.Message}");
        }
        private void TwitchClient_OnWhisperMessageReceived(object sender, OnWhisperMessageReceivedEventArgs e)
        {
            if (m_WhisperMessageMods != null)
            {
                foreach (IWhisperMessageMod mod in m_WhisperMessageMods)
                {
                    mod.Process(e.WhisperMessage);
                }
            }
            Console.WriteLine($"{e.WhisperMessage.Username} whispered: {e.WhisperMessage.Message}");
        }
        #endregion Web Twitch Form Events

       #region Web Form Events
        private void Web_BtnConnect_Click(object sender, EventArgs e)
        {
            m_TwitchClient?.Connect();
            webBtnConnect.Enabled = false;
        }
        private void Web_BtnJoin_Click(object sender, EventArgs e)
        {
            m_TwitchClient?.JoinChannel(webTBoxJoin.Text);
        }
        private void Web_TBoxJoin_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(webTBoxJoin.Text))
            {
                webTBoxJoin.Text = "Channel to join";
            }
        }
        private void Web_TBoxJoin_Enter(object sender, EventArgs e)
        {
            webTBoxJoin.Clear();
        }
        private void Web_BtnLeave_Click(object sender, EventArgs e)
        {
            if (webCBoxJoinedChannels.Items.Count > 0)
            {
                m_TwitchClient.LeaveChannel(webCBoxJoinedChannels.Items[webCBoxJoinedChannels.SelectedIndex].ToString());
            }
        }
        private void Web_RBtnPublicChat_CheckedChanged(object sender, EventArgs e)
        {
            webCBoxPublicChannels.Visible = true;
            webTBoxWhisper.Visible = false;
        }
        private void Web_RBtnWhisper_CheckedChanged(object sender, EventArgs e)
        {
            webCBoxPublicChannels.Visible = false;
            webTBoxWhisper.Visible = true;
        }
        private void Web_RTBoxTwitchOutput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return && !string.IsNullOrEmpty(webRTBoxTwitchOutput.Text))
            {
                string sMessage = webRTBoxTwitchOutput.Text.Trim();
                m_sPreviousChatMessages.Add(sMessage);
                webRTBoxTwitchOutput.Clear();

                string sTarget = (webRBtnPublicChat.Checked ? webCBoxPublicChannels.SelectedItem.ToString() : webTBoxWhisper.Text);

                if (tcpRBtnChat.Checked)
                {
                    m_TwitchClient.SendChatMessage(sTarget, sMessage);
                    tcpRTBoxChat.AppendText($"#{sTarget}|({m_TwitchClient.Credentials.TwitchUsername}): {sMessage}\n");
                }
                else
                {
                    m_TwitchClient.SendWhisperMessage(sTarget, sMessage);
                    tcpRTBoxChat.AppendText($"{m_TwitchClient.Credentials.TwitchUsername} : {sMessage}\n");
                }
            }
        }

        private void Web_RTBoxTwitchOutput_KeyUp(object sender, KeyEventArgs e)
        {

        }
        #endregion region Web Form Events

        #endregion WebSocket


    }
}
