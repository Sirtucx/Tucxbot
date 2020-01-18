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
    using Listeners;
    
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
        private int m_previousIndex;
        private List<IModHandler> m_modManagers;
        private ConnectedState m_connectedState;

        private readonly List<string> m_previousChatMessages;

        public MainForm()
        {
            InitializeComponent();
            m_previousChatMessages = new List<string>();
            m_connectedState = ConnectedState.NotConnected;
            AutoLogin();
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
                using (StreamReader sr = new StreamReader($"{Environment.CurrentDirectory}/Data/Settings.json"))
                {
                    string content = sr.ReadToEnd();
                    sr.Close();
                    loginData = JsonConvert.DeserializeObject<LoginData>(content);
                }
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
            m_twitchClient = TwitchClient.GetInstance(new IrcCredentials(m_loginData.Username, m_loginData.OAuth));

            if (m_twitchClient != null)
            {
                if (m_connectedState == ConnectedState.NotConnected)
                {
                    m_connectedState = ConnectedState.AttemptingConnection;
                    m_twitchClient.OnTwitchLoginFailed += OnLoginFailed;
                    m_twitchClient.OnTwitchConnected += OnTwitchClientConnected;
                }
            }

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
            UnregisterIrcEvents();
            m_twitchClient?.Disconnect();
        }
        private void CloseClient()
        {
            DisconnectClient();
        }

        private void RegisterIrcEvents()
        {
            m_twitchClient.OnWhisperMessageReceived += OnWhisperMessageReceived;
            m_twitchClient.OnChatMessageReceived += OnChatMessageReceived;
            m_twitchClient.OnBotJoinedChannel += OnBotJoinedChannel;
            m_twitchClient.OnUserLeaveEvent += OnUserLeaveEvent;
        }
        private void UnregisterIrcEvents()
        {
            if (m_twitchClient != null)
            {
                m_twitchClient.OnWhisperMessageReceived -= OnWhisperMessageReceived;
                m_twitchClient.OnChatMessageReceived -= OnChatMessageReceived;
                m_twitchClient.OnBotJoinedChannel -= OnBotJoinedChannel;
                m_twitchClient.OnTwitchConnected -= OnTwitchClientConnected;
            }
        }
        
        #region Mods
        private void InitializeModManagers()
        {
            m_modManagers = new List<IModHandler>();
            m_modManagers.Add(new ModManager<ChatMessageMod, ChatMessageListener>(m_twitchClient));
            m_modManagers.Add(new ModManager<WhisperMessageMod, WhisperMessageListener>(m_twitchClient));
            m_modManagers.Add(new ModManager<SubscriberMod, SubscriberListener>(m_twitchClient));
            m_modManagers.Add(new ModManager<UserJoinedMod, UserJoinedListener>(m_twitchClient));
            m_modManagers.Add(new ModManager<UserLeftMod, UserLeftListener>(m_twitchClient));
            
        }
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

                m_unloadModCB.Items.Add(fileName);
                m_unloadModCB.SelectedIndex = 0;
            }
        }
        private void UnloadMod()
        {
            if (m_unloadModCB.Items.Count > 0)
            {
                string modName = m_unloadModCB.Items[m_unloadModCB.SelectedIndex].ToString();
                foreach (IModHandler modManager in m_modManagers)
                {
                    modManager.UnloadMod(modName);
                }

                m_unloadModCB.Items.Remove(modName);
            }
        }
        private void ShutdownMods()
        {
            foreach (IModHandler modManager in m_modManagers)
            {
                modManager.Shutdown();
            }
            m_modManagers.Clear();
            m_modManagers = null;
        }
        #endregion Mods

        #region Twitch Client Events
        private void OnTwitchClientConnected()
        {
            if (m_connectButton.InvokeRequired)
            {
                m_connectButton.Invoke(new Action(OnTwitchClientConnected));
                return;
            }
            m_connectedState = ConnectedState.Connected;
            m_connectButton.Visible = false;
            m_disconnectButton.Visible = true;
            m_whisperGroup.Visible = true;
            m_channelGroup.Visible = true;
            InitializeModManagers();
            RegisterIrcEvents();
        }
        private void OnLoginFailed()
        {
            m_connectedState = ConnectedState.NotConnected;
            m_twitchClient.OnTwitchLoginFailed -= OnLoginFailed;
            m_twitchClient.OnTwitchConnected -= OnTwitchClientConnected;
        }
        private void OnBotJoinedChannel(object sender, OnBotJoinedChannelEventArgs e)
        {
            if (m_channelLeaveCB.InvokeRequired || m_eventGroup.InvokeRequired)
            {
                m_channelLeaveCB.Invoke(new Action(delegate { OnBotJoinedChannel(sender, e); }));
                return;
            }

            if (m_channelLeaveCB.Items.Contains(e.ChannelName))
            {
                return;
            }
            
            m_channelLeaveCB.Items.Add(e.ChannelName);
            m_channelLeaveCB.SelectedIndex = 0;
            m_channelJoinTB.Clear();
            m_channelLeaveCB.Visible = true;
            m_leaveChannelButton.Visible = true;

            m_channelGroup.Visible = true;
            m_channelMessageSelectCB.Items.Add(e.ChannelName);
            m_channelMessageSelectCB.SelectedIndex = 0;

            if (!m_eventGroup.Visible)
            {
                m_eventGroup.Visible = true;
            }
        }
        private void OnUserLeaveEvent(object sender, OnUserLeaveEventArgs e)
        {
            if (m_channelLeaveCB.InvokeRequired)
            {
                m_channelLeaveCB.Invoke(new Action(delegate { OnUserLeaveEvent(sender, e); }));
                return;
            }

            if (e.Username != m_twitchClient.Credentials.TwitchUsername)
            {
                return;
            }
            
            m_channelLeaveCB.Items.Remove(e.Channel);

            m_channelLeaveCB.Visible = m_channelLeaveCB.Items.Count > 0;
            if (m_channelLeaveCB.Visible)
            {
                m_channelLeaveCB.SelectedIndex = 0;
            }
            m_leaveChannelButton.Visible = m_channelLeaveCB.Items.Count > 0;

            m_channelMessageSelectCB.Items.Remove(e.Channel);
        }
        private void OnChatMessageReceived(object sender, OnChatMessageReceivedEventArgs e)
        {
            if (m_channelOutputTB.InvokeRequired)
            {
                m_channelOutputTB.Invoke(new Action(delegate { OnChatMessageReceived(sender, e); }));
                return;
            }
            m_channelOutputTB.Text += $"#{e.ChatMessage.Channel}| {e.ChatMessage.Username}: {e.ChatMessage.Message}\n";
            
            Console.WriteLine($"#{e.ChatMessage.Channel}\n{e.ChatMessage.Username}: {e.ChatMessage.Message}");
        }
        private void OnWhisperMessageReceived(object sender, OnWhisperMessageReceivedEventArgs e)
        {
            if (m_whisperOutputTB.InvokeRequired)
            {
                m_whisperOutputTB.Invoke(new Action(delegate { OnWhisperMessageReceived(sender, e); }));
                return;
            }
            m_whisperOutputTB.Text += $"{e.WhisperMessage.Username} whispered: {e.WhisperMessage.Message}\n";
            
            Console.WriteLine($"{e.WhisperMessage.Username} whispered: {e.WhisperMessage.Message}");
        }
        #endregion Twitch Client Events

        #region Connect Group
        private void LoginButton_Click(object sender, EventArgs e)
        {
            string username = Microsoft.VisualBasic.Interaction.InputBox($"Please enter your bot\'s username:", "Login Information", XPos: Screen.PrimaryScreen.WorkingArea.Width / 2, YPos: Screen.PrimaryScreen.WorkingArea.Height / 2).ToLower();
            string oAuth = Microsoft.VisualBasic.Interaction.InputBox($"Please enter your bot\'s OAuth Key, this can be found here: https://twitchapps.com/tmi/", "Login Information", XPos: Screen.PrimaryScreen.WorkingArea.Width / 2, YPos: Screen.PrimaryScreen.WorkingArea.Height / 2);

            m_loginData = SaveLoginData(username, oAuth);

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(oAuth))
            {
                AutoLogin();
            }
        }
        private void ConnectButton_Click(object sender, EventArgs e)
        {
            ConnectClient();

        }
        private void DisconnectButton_Click(object sender, EventArgs e)
        {
            CloseClient();
            m_disconnectButton.Visible = false;
            m_connectButton.Visible = true;
        }
        #endregion Connect Group

        #region Whisper Group
        private void WhisperInputTB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return && !string.IsNullOrEmpty(m_whisperInputTB.Text))
            {
                string target = m_WhisperUserTB.Text;
                string message = m_whisperInputTB.Text.Trim().Replace("\n", "");
                m_whisperInputTB.Clear();

                m_twitchClient.SendWhisperMessage(target, message);
                m_whisperOutputTB.AppendText($"{m_twitchClient.Credentials.TwitchUsername} : {message}\n");
            }
        }
        #endregion Whisper Group

        #region Public Twitch Chat Group
        private void JoinChannelButton_Click(object sender, EventArgs e)
        {
            m_twitchClient?.JoinChannel(m_channelJoinTB.Text);
        }
        private void JoinChannelTB_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(m_channelJoinTB.Text))
            {
                m_channelJoinTB.Text = @"Channel to join";
            }
        }
        private void JoinChannelTB_Enter(object sender, EventArgs e)
        {
            m_channelJoinTB.Clear();
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
            if (e.KeyChar == (char)Keys.Return && !string.IsNullOrEmpty(m_channelInputTB.Text))
            {
                string message = m_channelInputTB.Text.Trim().Replace("\n", "");
                m_previousChatMessages.Add(message);
                m_channelInputTB.Clear();

                string target = m_channelMessageSelectCB.SelectedItem.ToString();

                m_twitchClient.SendChatMessage(target, message);
                m_channelOutputTB.AppendText($"#{target}|({m_twitchClient.Credentials.TwitchUsername}): {message}\n");


                m_previousIndex = 0;
            }
        }
        private void ChannelMessageInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up && m_previousChatMessages.Count > 0)
            {
                if (m_previousIndex == m_previousChatMessages.Count)
                {
                    m_previousIndex -= 1;
                }
                m_channelInputTB.Text = m_previousChatMessages[m_previousChatMessages.Count - 1 - m_previousIndex];
                ++m_previousIndex;
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
            UnloadMod();
        }
        #endregion Mod Group
        
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ShutdownMods();
            CloseClient();
        }
    }
}
