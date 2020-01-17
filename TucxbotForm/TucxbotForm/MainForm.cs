using TucxbotForm.Listeners;

namespace TucxbotForm
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using System.Reflection;
    using System.IO;
    using Newtonsoft.Json;
    using Twitch.Containers;
    using Twitch.Core;
    using Twitch.Events;
    using Twitch.Mods;
    using Twitch.Interfaces;
    
    public partial class MainForm : Form
    {
        private enum ConnectedState
        {
            NotConnected,
            AttemptingConnection,
            Connected
        }
        
        private TwitchClient m_twitchClient;
        private LoginData m_loginData;
        private int m_iPreviousIndex;
        private List<IModHandler> m_modManagers;
        private Dictionary<string, IOnSubscriberMod> m_onSubscriberMods;
        private Dictionary<string, IOnUserJoinedMod> m_onUserJoinedMods;
        private Dictionary<string, IOnUserLeaveMod> m_onUserLeaveMods;
        private ConnectedState m_connectedState;

        private readonly List<string> m_previousChatMessages;

        public MainForm()
        {
            InitializeComponent();
            m_previousChatMessages = new List<string>();
            m_connectedState = ConnectedState.NotConnected;
            InitializeModManagers();
            AutoLogin();
        }

        private void InitializeModManagers()
        {
            m_modManagers = new List<IModHandler>();
            m_modManagers.Add(new ModManager<ChatMessageMod, ChatMessageListener>(m_twitchClient));
            m_modManagers.Add(new ModManager<WhisperMessageMod, WhisperMessageListener>(m_twitchClient));
        }
        
        private void AutoLogin()
        {
            try
            {
                m_loginData = LoadCredentials();
                m_connectButton.Visible = !string.IsNullOrEmpty(m_loginData.Username) && !string.IsNullOrEmpty(m_loginData.OAuth);
            }
            catch (Exception e)
            {
                m_loginData = SaveLoginData();
            }
        }
        private LoginData LoadCredentials()
        {
            LoginData loginData = null;
            if (File.Exists($"{Environment.CurrentDirectory}/Data/Settings.json"))
            {
                StreamReader sr = new StreamReader($"{Environment.CurrentDirectory}/Data/Settings.json");
                string sContent = sr.ReadToEnd();
                sr.Close();

               loginData = JsonConvert.DeserializeObject<LoginData>(sContent);
            }
            else
            {
                loginData = SaveLoginData();
            }
            return loginData;
        }
        private LoginData SaveLoginData(string sUsername = "", string sOAuth = "")
        {
            LoginData loginData = new LoginData(sUsername, sOAuth);
            string jsonContent = JsonConvert.SerializeObject(loginData, Formatting.Indented);

            using (StreamWriter writer = new StreamWriter($"{Environment.CurrentDirectory}/Data/Settings.json", false))
            {
                writer.Write(jsonContent);
                writer.Flush();
                writer.Close();
            }
            return loginData;
        }
        private void ConnectClient()
        {
            m_twitchClient?.Connect();
        }
        private void DisconnectClient()
        {
            m_leaveChannelButton.Visible = false;
            m_channelLeaveCB.Items.Clear();
            m_channelLeaveCB.ResetText();
            m_channelLeaveCB.Visible = false;

            m_channelMessageSelectCB.Items.Clear();
            m_channelMessageSelectCB.ResetText();
            m_channelGroup.Visible = false;

            m_whisperGroup.Visible = false;

            m_eventGroup.Visible = false;

            m_connectButton.Enabled = true;
            m_connectedState = ConnectedState.NotConnected;
            UnRegisterWebEvents();
            m_twitchClient?.Disconnect();
        }
        private void CloseClient()
        {
            DisconnectClient();
        }

        #region Mods
        private void LoadMod()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog.Filter = "Mod Library Files (*.dll)|*.dll";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                int startIndex = filePath.LastIndexOf('\\') + 1;
                int endIndex = filePath.LastIndexOf('.');
                string fileName = filePath.Substring(startIndex, endIndex - startIndex);
                Assembly assembly = Assembly.LoadFile(filePath);
                foreach (Type type in assembly.GetTypes())
                {
                    foreach (IModHandler modManager in m_modManagers)
                    {
                        modManager.LoadMod(fileName, type);
                    }
                }

                m_UnloadModCB.Items.Add(fileName);
                m_UnloadModCB.SelectedIndex = 0;
            }
        }
        private void ShutdownMods()
        {
            foreach (IModHandler modManager in m_modManagers)
            {
                modManager.Shutdown();
            }
        }
        private void RegisterWebEvents()
        {
            m_twitchClient.OnWhisperMessageReceived += TwitchClient_OnWhisperMessageReceived;
            m_twitchClient.OnChatMessageReceived += TwitchClient_OnChatMessageReceived;
            m_twitchClient.OnBotJoinedChannel += TwitchClientOnBotJoinedChannel;
            m_twitchClient.OnUserLeaveEvent += TwitchClient_OnUserLeaveEvent;
            m_twitchClient.OnSubscriptionReceived += TwitchClientOnSubscriptionReceived;
            m_twitchClient.OnUserJoinedEvent += TwitchClient_OnUserJoinedEvent;
        }

        private void UnRegisterWebEvents()
        {
            if (m_twitchClient != null)
            {
                m_twitchClient.OnWhisperMessageReceived -= TwitchClient_OnWhisperMessageReceived;
                m_twitchClient.OnChatMessageReceived -= TwitchClient_OnChatMessageReceived;
                m_twitchClient.OnBotJoinedChannel -= TwitchClientOnBotJoinedChannel;
                m_twitchClient.OnSubscriptionReceived -= TwitchClientOnSubscriptionReceived;
                m_twitchClient.OnTwitchConnected -= TwitchClient_OnTwitchClientConnected;
                m_twitchClient.OnUserJoinedEvent -= TwitchClient_OnUserJoinedEvent;
            }
        }
        #endregion Mods

        #region Twitch Client Events
        private void TwitchClient_OnTwitchClientConnected()
        {
            if (m_connectButton.InvokeRequired)
            {
                m_connectButton.Invoke(new Action(delegate { TwitchClient_OnTwitchClientConnected(); }));
                return;
            }
            m_connectedState = ConnectedState.Connected;
            m_connectButton.Visible = false;
            m_DisconnectButton.Visible = true;
            m_whisperGroup.Visible = true;
            m_channelGroup.Visible = true;
            RegisterWebEvents();
        }
        private void TwitchClient_OnLoginFailed()
        {
            m_connectedState = ConnectedState.NotConnected;
            m_twitchClient.OnTwitchLoginFailed -= TwitchClient_OnLoginFailed;
            m_twitchClient.OnTwitchConnected -= TwitchClient_OnTwitchClientConnected;
        }
        private void TwitchClientOnSubscriptionReceived(object sender, OnSubscriptionEventArgs e)
        {
            if (m_onSubscriberMods != null)
            {
                foreach (KeyValuePair<string, IOnSubscriberMod> mod in m_onSubscriberMods)
                {
                    mod.Value.Process(e.UserNotice);
                }
            }
        }
        private void TwitchClientOnBotJoinedChannel(object sender, OnBotJoinedChannelEventArgs e)
        {
            if (!m_channelLeaveCB.Items.Contains(e.ChannelName))
            {
                if (m_channelLeaveCB.InvokeRequired)
                {
                    m_channelLeaveCB.Invoke(new Action(delegate { TwitchClientOnBotJoinedChannel(sender, e); }));
                    return;
                }
                m_channelLeaveCB.Items.Add(e.ChannelName);
                m_channelLeaveCB.SelectedIndex = 0;
                m_ChannelJoinTB.Clear();
                m_channelLeaveCB.Visible = true;
                m_leaveChannelButton.Visible = true;

                m_channelGroup.Visible = true;
                m_channelMessageSelectCB.Items.Add(e.ChannelName);
                m_channelMessageSelectCB.SelectedIndex = 0;

                if (!m_eventGroup.Visible)
                {
                    if (m_eventGroup.InvokeRequired)
                    {
                        m_eventGroup.Invoke(new Action(delegate { TwitchClientOnBotJoinedChannel(sender, e); }));
                        return;
                    }
                    m_eventGroup.Visible = true;
                }
            }
        }
        private void TwitchClient_OnUserLeaveEvent(object sender, OnUserLeaveEventArgs e)
        {
            if (m_channelLeaveCB.InvokeRequired)
            {
                m_channelLeaveCB.Invoke(new Action(delegate { TwitchClient_OnUserLeaveEvent(sender, e); }));
                return;
            }

            if (e.Username == m_twitchClient.Credentials.TwitchUsername)
            {
                m_channelLeaveCB.Items.Remove(e.Channel);

                m_channelLeaveCB.Visible = m_channelLeaveCB.Items.Count > 0;
                if (m_channelLeaveCB.Visible)
                {
                    m_channelLeaveCB.SelectedIndex = 0;
                }
                m_leaveChannelButton.Visible = m_channelLeaveCB.Items.Count > 0;

                m_channelMessageSelectCB.Items.Remove(e.Channel);
            }
        }
        private void TwitchClient_OnChatMessageReceived(object sender, OnChatMessageReceivedEventArgs e)
        {
            if (m_ChannelOutputTB.InvokeRequired)
            {
                m_ChannelOutputTB.Invoke(new Action(delegate { TwitchClient_OnChatMessageReceived(sender, e); }));
                return;
            }
            m_ChannelOutputTB.Text += $"#{e.ChatMessage.Channel}| {e.ChatMessage.Username}: {e.ChatMessage.Message}\n";
            
            Console.WriteLine($"#{e.ChatMessage.Channel}\n{e.ChatMessage.Username}: {e.ChatMessage.Message}");
        }
        private void TwitchClient_OnWhisperMessageReceived(object sender, OnWhisperMessageReceivedEventArgs e)
        {
            if (m_WhisperOutputTB.InvokeRequired)
            {
                m_WhisperOutputTB.Invoke(new Action(delegate { TwitchClient_OnWhisperMessageReceived(sender, e); }));
                return;
            }
            m_WhisperOutputTB.Text += $"{e.WhisperMessage.Username} whispered: {e.WhisperMessage.Message}\n";
            
            Console.WriteLine($"{e.WhisperMessage.Username} whispered: {e.WhisperMessage.Message}");
        }
        private void TwitchClient_OnUserJoinedEvent(object sender, OnUserJoinedEventArgs e)
        {
            if (m_onUserJoinedMods != null)
            {
                foreach(KeyValuePair<string, IOnUserJoinedMod> mod in m_onUserJoinedMods)
                {
                    mod.Value.Process(e.UserState);
                }
            }
        }
        #endregion Twitch Client Events

        #region Connect Group
        private void LoginButton_Click(object sender, EventArgs e)
        {
            string sUsername = Microsoft.VisualBasic.Interaction.InputBox($"Please enter your bot\'s username:", "Login Information", XPos: Screen.PrimaryScreen.WorkingArea.Width / 2, YPos: Screen.PrimaryScreen.WorkingArea.Height / 2).ToLower();
            string sOAuth = Microsoft.VisualBasic.Interaction.InputBox($"Please enter your bot\'s OAuth Key, this can be found here: https://twitchapps.com/tmi/", "Login Information", XPos: Screen.PrimaryScreen.WorkingArea.Width / 2, YPos: Screen.PrimaryScreen.WorkingArea.Height / 2);

            m_loginData = SaveLoginData(sUsername, sOAuth);

            if (!string.IsNullOrEmpty(sUsername) && !string.IsNullOrEmpty(sOAuth))
            {
                AutoLogin();
            }
        }
        private void ConnectButton_Click(object sender, EventArgs e)
        {
            m_twitchClient = TwitchClient.GetInstance(new IRCCredentials(m_loginData.Username, m_loginData.OAuth));

            if (m_twitchClient != null)
            {
                if (m_connectedState == ConnectedState.NotConnected)
                {
                    m_connectedState = ConnectedState.AttemptingConnection;
                    m_twitchClient.OnTwitchLoginFailed += TwitchClient_OnLoginFailed;
                    m_twitchClient.OnTwitchConnected += TwitchClient_OnTwitchClientConnected;
                }
            }

            m_twitchClient?.Connect();

        }
        private void DisconnectButton_Click(object sender, EventArgs e)
        {
            CloseClient();
            m_DisconnectButton.Visible = false;
            m_connectButton.Visible = true;
        }
        #endregion Connect Group

        #region Whisper Group
        private void WhisperInputTB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return && !string.IsNullOrEmpty(m_WhisperInputTB.Text))
            {
                string sTarget = m_WhisperUserTB.Text;
                string sMessage = m_WhisperInputTB.Text.Trim().Replace("\n", "");
                m_WhisperInputTB.Clear();

                m_twitchClient.SendWhisperMessage(sTarget, sMessage);
                m_WhisperOutputTB.AppendText($"{m_twitchClient.Credentials.TwitchUsername} : {sMessage}\n");
            }
        }
        #endregion Whisper Group

        #region Public Twitch Chat Group
        private void JoinChannelButton_Click(object sender, EventArgs e)
        {
            m_twitchClient?.JoinChannel(m_ChannelJoinTB.Text);
        }
        private void JoinChannelTB_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(m_ChannelJoinTB.Text))
            {
                m_ChannelJoinTB.Text = "Channel to join";
            }
        }
        private void JoinChannelTB_Enter(object sender, EventArgs e)
        {
            m_ChannelJoinTB.Clear();
        }
        private void LeaveChannelButton_Click(object sender, EventArgs e)
        {
            if (m_channelLeaveCB.Items.Count > 0)
            {
                m_twitchClient.LeaveChannel(m_channelLeaveCB.Items[m_channelLeaveCB.SelectedIndex].ToString());
            }
        }
        private void ChannelMessageInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return && !string.IsNullOrEmpty(m_ChannelInputTB.Text))
            {
                string sMessage = m_ChannelInputTB.Text.Trim().Replace("\n", "");
                m_previousChatMessages.Add(sMessage);
                m_ChannelInputTB.Clear();

                string sTarget = m_channelMessageSelectCB.SelectedItem.ToString();

                m_twitchClient.SendChatMessage(sTarget, sMessage);
                m_ChannelOutputTB.AppendText($"#{sTarget}|({m_twitchClient.Credentials.TwitchUsername}): {sMessage}\n");


                m_iPreviousIndex = 0;
            }
        }
        private void ChannelMessageInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up && m_previousChatMessages.Count > 0)
            {
                if (m_iPreviousIndex == m_previousChatMessages.Count)
                {
                    m_iPreviousIndex -= 1;
                }
                m_ChannelInputTB.Text = m_previousChatMessages[m_previousChatMessages.Count - 1 - m_iPreviousIndex];
                ++m_iPreviousIndex;
            }
        }
        #endregion Public Twitch Chat Group

        #region Mod Group
        private void LoadModButton_Click(object sender, EventArgs e)
        {
            LoadMod();
        }
        private void UnloadModButton_Click(object sender, EventArgs e)
        {
            if (m_UnloadModCB.Items.Count > 0)
            {
                string sModName = m_UnloadModCB.Items[m_UnloadModCB.SelectedIndex].ToString();

                if (m_onSubscriberMods != null)
                {
                    if (m_onSubscriberMods.ContainsKey(sModName))
                    {
                        m_onSubscriberMods[sModName].Shutdown();
                        m_onSubscriberMods.Remove(sModName);
                    }
                }
                if (m_onUserJoinedMods != null)
                {
                    if (m_onUserJoinedMods.ContainsKey(sModName))
                    {
                        m_onUserJoinedMods[sModName].Shutdown();
                        m_onUserJoinedMods.Remove(sModName);
                    }
                }
                if (m_onUserLeaveMods != null)
                {
                    if (m_onUserLeaveMods.ContainsKey(sModName))
                    {
                        m_onUserLeaveMods[sModName].Shutdown();
                        m_onUserLeaveMods.Remove(sModName);
                    }
                }

                m_UnloadModCB.Items.Remove(sModName);
            }
        }
        #endregion Mod Group


        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ShutdownMods();
            CloseClient();
        }

        
    }
}
