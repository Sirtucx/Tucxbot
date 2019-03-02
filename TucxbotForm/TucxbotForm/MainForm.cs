using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using Twitch_Websocket;
using Twitch_Websocket.Mod_Interfaces;
using Newtonsoft.Json;

namespace TucxbotForm
{
    public partial class MainForm : Form
    {
        private TwitchClient m_TwitchClient;
        private LoginData m_LoginData;
        private int m_iPreviousIndex;
        private List<string> m_sPreviousChatMessages;
        private Dictionary<string, IChatMessageMod> m_ChannelMessageMods;
        private Dictionary<string, IWhisperMessageMod> m_WhisperMessageMods;
        private Dictionary<string, IOnSubscriberMod> m_OnSubscriberMods;
        private Dictionary<string, IOnUserJoinedMod> m_OnUserJoinedMods;
        private Dictionary<string, IOnUserLeaveMod> m_OnUserLeaveMods;
        private enum CONNECTED_STATE
        {
            NOT_CONNECTED,
            ATTEMPTING_CONNECTION,
            CONNECTED
        }
        private CONNECTED_STATE m_ConnectedState;


        public MainForm()
        {
            
            InitializeComponent();
            m_sPreviousChatMessages = new List<string>();
            m_ConnectedState = CONNECTED_STATE.NOT_CONNECTED;
            AutoLogin();
        }

        private void AutoLogin()
        {
            try
            {
                m_LoginData = LoadCredentials();
                m_ConnectButton.Visible = !string.IsNullOrEmpty(m_LoginData.Username) && !string.IsNullOrEmpty(m_LoginData.OAuth);
            }
            catch (Exception e)
            {
                m_LoginData = SaveLoginData();
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
            string sJSONContent = JsonConvert.SerializeObject(loginData, Formatting.Indented);

            using (StreamWriter writer = new StreamWriter($"{Environment.CurrentDirectory}/Data/Settings.json", false))
            {
                writer.Write(sJSONContent);
                writer.Flush();
                writer.Close();
            }
            return loginData;
        }
        private void ConnectWeb()
        {
            m_TwitchClient?.Connect();
        }
        private void DisconnectWeb()
        {
            m_LeaveChannelButton.Visible = false;
            m_ChannelLeaveCB.Items.Clear();
            m_ChannelLeaveCB.ResetText();
            m_ChannelLeaveCB.Visible = false;

            m_ChannelMessageSelectCB.Items.Clear();
            m_ChannelMessageSelectCB.ResetText();
            m_ChannelGroup.Visible = false;

            m_WhisperGroup.Visible = false;

            m_EventGroup.Visible = false;

            m_ConnectButton.Enabled = true;
            m_ConnectedState = CONNECTED_STATE.NOT_CONNECTED;
            UnRegisterWebEvents();
            m_TwitchClient?.Disconnect();
        }
        private void CloseWeb()
        {
            DisconnectWeb();
        }

        #region Mods
        private void LoadMod()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog.Filter = "Mod Library Files (*.dll)|*.dll";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string sFilePath = openFileDialog.FileName;
                int iStartIndex = sFilePath.LastIndexOf('\\') + 1;
                int iEndIndex = sFilePath.LastIndexOf('.');
                string sFileName = sFilePath.Substring(iStartIndex, iEndIndex - iStartIndex);
                Assembly assembly = Assembly.LoadFile(sFilePath);
                foreach (Type type in assembly.GetTypes())
                {
                    if (typeof(IChatMessageMod).IsAssignableFrom(type))
                    {
                        if (m_ChannelMessageMods == null)
                        {
                            m_ChannelMessageMods = new Dictionary<string, IChatMessageMod>();
                        }
                        m_ChannelMessageMods.Add(sFileName, type.GetConstructor(new Type[] { }).Invoke(null) as IChatMessageMod);
                        Console.WriteLine("Loading Channel Message Mod: {0} to the project!", sFileName);
                    }
                    else if (typeof(IWhisperMessageMod).IsAssignableFrom(type))
                    {
                        if (m_WhisperMessageMods == null)
                        {
                            m_WhisperMessageMods = new Dictionary<string, IWhisperMessageMod>();
                        }
                        m_WhisperMessageMods.Add(sFileName, type.GetConstructor(new Type[] { }).Invoke(null) as IWhisperMessageMod);
                        Console.WriteLine("Loading Whisper Message Mod: {0} to the project!", sFileName);
                    }
                    else if (typeof(IOnSubscriberMod).IsAssignableFrom(type))
                    {
                        if (m_OnSubscriberMods == null)
                        {
                            m_OnSubscriberMods = new Dictionary<string, IOnSubscriberMod>();
                        }
                        m_OnSubscriberMods.Add(sFileName, type.GetConstructor(new Type[] { }).Invoke(null) as IOnSubscriberMod);
                        Console.WriteLine("Loading OnSubscriber Mod: {0} to the project!", sFileName);
                    }
                    else if (typeof(IOnUserJoinedMod).IsAssignableFrom(type))
                    {
                        if (m_OnUserJoinedMods == null)
                        {
                            m_OnUserJoinedMods = new Dictionary<string, IOnUserJoinedMod>();
                        }
                        m_OnUserJoinedMods.Add(sFileName, type.GetConstructor(new Type[] { }).Invoke(null) as IOnUserJoinedMod);
                        Console.WriteLine("Loading OnUserJoined Mod: {0} to the project!", sFileName);
                    }
                    else if (typeof(IOnUserLeaveMod).IsAssignableFrom(type))
                    {
                        if (m_OnUserLeaveMods == null)
                        {
                            m_OnUserLeaveMods = new Dictionary<string, IOnUserLeaveMod>();
                        }
                        m_OnUserLeaveMods.Add(sFileName, type.GetConstructor(new Type[] { }).Invoke(null) as IOnUserLeaveMod);
                        Console.WriteLine("Loading OnUserLeave Mod: {0} to the project!", sFileName);
                    }
                }

                m_UnloadModCB.Items.Add(sFileName);
                m_UnloadModCB.SelectedIndex = 0;
            }
        }
        private void ShutdownMods()
        {
            if (m_ChannelMessageMods != null)
            {
                foreach (KeyValuePair<string, IChatMessageMod> kvp in m_ChannelMessageMods)
                {
                    kvp.Value.Shutdown();
                }
            }
            if (m_WhisperMessageMods != null)
            {
                foreach (KeyValuePair<string, IWhisperMessageMod> kvp in m_WhisperMessageMods)
                {
                    kvp.Value.Shutdown();
                }
            }
            if (m_OnSubscriberMods != null)
            {
                foreach (KeyValuePair<string, IOnSubscriberMod> kvp in m_OnSubscriberMods)
                {
                    kvp.Value.Shutdown();
                }
            }
        }
        private void RegisterWebEvents()
        {
            m_TwitchClient.OnWhisperMessageReceived += TwitchClient_OnWhisperMessageReceived;
            m_TwitchClient.OnChatMessageReceived += TwitchClient_OnChatMessageReceived;
            m_TwitchClient.OnBotJoinedChannelEvent += TwitchClient_OnBotJoinedChannelEvent;
            m_TwitchClient.OnUserLeaveEvent += TwitchClient_OnUserLeaveEvent;
            m_TwitchClient.OnSubscriberEvent += TwitchClient_OnSubscriberEvent;
            m_TwitchClient.OnUserJoinedEvent += TwitchClient_OnUserJoinedEvent;
        }
        private void UnRegisterWebEvents()
        {
            if (m_TwitchClient != null)
            {
                m_TwitchClient.OnWhisperMessageReceived -= TwitchClient_OnWhisperMessageReceived;
                m_TwitchClient.OnChatMessageReceived -= TwitchClient_OnChatMessageReceived;
                m_TwitchClient.OnBotJoinedChannelEvent -= TwitchClient_OnBotJoinedChannelEvent;
                m_TwitchClient.OnSubscriberEvent -= TwitchClient_OnSubscriberEvent;
                m_TwitchClient.OnTwitchConnected -= TwitchClient_OnTwitchClientConnected;
                m_TwitchClient.OnUserJoinedEvent -= TwitchClient_OnUserJoinedEvent;
            }
        }
        #endregion Mods

        #region Twitch Client Events
        private void TwitchClient_OnTwitchClientConnected()
        {
            if (m_ConnectButton.InvokeRequired)
            {
                m_ConnectButton.Invoke(new Action(delegate { TwitchClient_OnTwitchClientConnected(); }));
                return;
            }
            m_ConnectedState = CONNECTED_STATE.CONNECTED;
            m_ConnectButton.Visible = false;
            m_DisconnectButton.Visible = true;
            m_WhisperGroup.Visible = true;
            m_ChannelGroup.Visible = true;
            RegisterWebEvents();
        }
        private void TwitchClient_OnLoginFailed()
        {
            m_ConnectedState = CONNECTED_STATE.NOT_CONNECTED;
            m_TwitchClient.OnTwitchLoginFailed -= TwitchClient_OnLoginFailed;
            m_TwitchClient.OnTwitchConnected -= TwitchClient_OnTwitchClientConnected;
        }
        private void TwitchClient_OnSubscriberEvent(object sender, OnSubscriberEventArgs e)
        {
            if (m_OnSubscriberMods != null)
            {
                foreach (KeyValuePair<string, IOnSubscriberMod> mod in m_OnSubscriberMods)
                {
                    mod.Value.Process(e.UserNotice);
                }
            }
        }
        private void TwitchClient_OnBotJoinedChannelEvent(object sender, OnBotJoinedChannelEventArgs e)
        {
            if (!m_ChannelLeaveCB.Items.Contains(e.Channel))
            {
                if (m_ChannelLeaveCB.InvokeRequired)
                {
                    m_ChannelLeaveCB.Invoke(new Action(delegate { TwitchClient_OnBotJoinedChannelEvent(sender, e); }));
                    return;
                }
                m_ChannelLeaveCB.Items.Add(e.Channel);
                m_ChannelLeaveCB.SelectedIndex = 0;
                m_ChannelJoinTB.Clear();
                m_ChannelLeaveCB.Visible = true;
                m_LeaveChannelButton.Visible = true;

                m_ChannelGroup.Visible = true;
                m_ChannelMessageSelectCB.Items.Add(e.Channel);
                m_ChannelMessageSelectCB.SelectedIndex = 0;

                if (!m_EventGroup.Visible)
                {
                    if (m_EventGroup.InvokeRequired)
                    {
                        m_EventGroup.Invoke(new Action(delegate { TwitchClient_OnBotJoinedChannelEvent(sender, e); }));
                        return;
                    }
                    m_EventGroup.Visible = true;
                }
            }
        }
        private void TwitchClient_OnUserLeaveEvent(object sender, OnUserLeaveEventArgs e)
        {
            if (m_ChannelLeaveCB.InvokeRequired)
            {
                m_ChannelLeaveCB.Invoke(new Action(delegate { TwitchClient_OnUserLeaveEvent(sender, e); }));
                return;
            }

            if (e.Username == m_TwitchClient.Credentials.TwitchUsername)
            {
                m_ChannelLeaveCB.Items.Remove(e.Channel);

                m_ChannelLeaveCB.Visible = m_ChannelLeaveCB.Items.Count > 0;
                if (m_ChannelLeaveCB.Visible)
                {
                    m_ChannelLeaveCB.SelectedIndex = 0;
                }
                m_LeaveChannelButton.Visible = m_ChannelLeaveCB.Items.Count > 0;

                m_ChannelMessageSelectCB.Items.Remove(e.Channel);
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

            if (m_ChannelMessageMods != null)
            {
                foreach(KeyValuePair < string, IChatMessageMod> mod in m_ChannelMessageMods)
                {
                    mod.Value.Process(e.ChatMessage);
                }
            }
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

            if (m_WhisperMessageMods != null)
            {
                foreach (KeyValuePair<string, IWhisperMessageMod> mod in m_WhisperMessageMods)
                {
                    mod.Value.Process(e.WhisperMessage);
                }
            }
            Console.WriteLine($"{e.WhisperMessage.Username} whispered: {e.WhisperMessage.Message}");
        }
        private void TwitchClient_OnUserJoinedEvent(object sender, OnUserJoinedEventArgs e)
        {
            if (m_OnUserJoinedMods != null)
            {
                foreach(KeyValuePair<string, IOnUserJoinedMod> mod in m_OnUserJoinedMods)
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

            m_LoginData = SaveLoginData(sUsername, sOAuth);

            if (!string.IsNullOrEmpty(sUsername) && !string.IsNullOrEmpty(sOAuth))
            {
                AutoLogin();
            }
        }
        private void ConnectButton_Click(object sender, EventArgs e)
        {
            m_TwitchClient = TwitchClient.GetInstance(new IRCCredentials(m_LoginData.Username, m_LoginData.OAuth));

            if (m_TwitchClient != null)
            {
                if (m_ConnectedState == CONNECTED_STATE.NOT_CONNECTED)
                {
                    m_ConnectedState = CONNECTED_STATE.ATTEMPTING_CONNECTION;
                    m_TwitchClient.OnTwitchLoginFailed += TwitchClient_OnLoginFailed;
                    m_TwitchClient.OnTwitchConnected += TwitchClient_OnTwitchClientConnected;
                }
            }

            m_TwitchClient?.Connect();

        }
        private void DisconnectButton_Click(object sender, EventArgs e)
        {
            CloseWeb();
            m_DisconnectButton.Visible = false;
            m_ConnectButton.Visible = true;
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

                m_TwitchClient.SendWhisperMessage(sTarget, sMessage);
                m_WhisperOutputTB.AppendText($"{m_TwitchClient.Credentials.TwitchUsername} : {sMessage}\n");
            }
        }
        #endregion Whisper Group

        #region Public Twitch Chat Group
        private void JoinChannelButton_Click(object sender, EventArgs e)
        {
            m_TwitchClient?.JoinChannel(m_ChannelJoinTB.Text);
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
            if (m_ChannelLeaveCB.Items.Count > 0)
            {
                m_TwitchClient.LeaveChannel(m_ChannelLeaveCB.Items[m_ChannelLeaveCB.SelectedIndex].ToString());
            }
        }
        private void ChannelMessageInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return && !string.IsNullOrEmpty(m_ChannelInputTB.Text))
            {
                string sMessage = m_ChannelInputTB.Text.Trim().Replace("\n", "");
                m_sPreviousChatMessages.Add(sMessage);
                m_ChannelInputTB.Clear();

                string sTarget = m_ChannelMessageSelectCB.SelectedItem.ToString();

                m_TwitchClient.SendChatMessage(sTarget, sMessage);
                m_ChannelOutputTB.AppendText($"#{sTarget}|({m_TwitchClient.Credentials.TwitchUsername}): {sMessage}\n");


                m_iPreviousIndex = 0;
            }
        }
        private void ChannelMessageInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up && m_sPreviousChatMessages.Count > 0)
            {
                if (m_iPreviousIndex == m_sPreviousChatMessages.Count)
                {
                    m_iPreviousIndex -= 1;
                }
                m_ChannelInputTB.Text = m_sPreviousChatMessages[m_sPreviousChatMessages.Count - 1 - m_iPreviousIndex];
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
                if (m_ChannelMessageMods != null)
                {
                    if (m_ChannelMessageMods.ContainsKey(sModName))
                    {
                        m_ChannelMessageMods[sModName].Shutdown();
                        m_ChannelMessageMods.Remove(sModName);
                    }
                }
                if (m_WhisperMessageMods != null)
                {
                    if (m_WhisperMessageMods.ContainsKey(sModName))
                    {
                        m_WhisperMessageMods[sModName].Shutdown();
                        m_WhisperMessageMods.Remove(sModName);
                    }
                }
                if (m_OnSubscriberMods != null)
                {
                    if (m_OnSubscriberMods.ContainsKey(sModName))
                    {
                        m_OnSubscriberMods[sModName].Shutdown();
                        m_OnSubscriberMods.Remove(sModName);
                    }
                }
                if (m_OnUserJoinedMods != null)
                {
                    if (m_OnUserJoinedMods.ContainsKey(sModName))
                    {
                        m_OnUserJoinedMods[sModName].Shutdown();
                        m_OnUserJoinedMods.Remove(sModName);
                    }
                }
                if (m_OnUserLeaveMods != null)
                {
                    if (m_OnUserLeaveMods.ContainsKey(sModName))
                    {
                        m_OnUserLeaveMods[sModName].Shutdown();
                        m_OnUserLeaveMods.Remove(sModName);
                    }
                }

                m_UnloadModCB.Items.Remove(sModName);
            }
        }
        #endregion Mod Group


        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ShutdownMods();
            CloseWeb();
        }

        
    }
}
