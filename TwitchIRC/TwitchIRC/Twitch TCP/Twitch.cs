namespace TwitchIRC_TCP
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    using Newtonsoft.Json;


    public class Twitch
    {
        private static Twitch m_Instance;
        public static Twitch Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new Twitch();
                }
                return m_Instance;
            }
        }

        public IRCConnection m_ChannelIRC, m_WhisperIRC;
        public event EventHandler<ChannelGateInteractionEventArgs> OnChannelEnter, OnChannelPart;
        public event EventHandler<ChatInputEventArgs> OnChannelInput, OnWhisperInput, OnChannelOutput, OnWhisperOutput;
        public Action OnChannelIRCConnected;
        public string Username
        {
            get
            {
                return m_Settings.Username;
            }
        }

        private ChannelGateInteractionEventArgs m_ChannelInteractionArgs;
        private ChatInputEventArgs m_ChatInputArgs, m_ChatOutputArgs;
        private bool m_bRunning;
        private Settings m_Settings;

        private Twitch()
        {
            m_ChannelInteractionArgs = new ChannelGateInteractionEventArgs();
            m_ChatInputArgs = new ChatInputEventArgs();
            m_ChatOutputArgs = new ChatInputEventArgs();
            m_bRunning = true;
            string sSettingsPath = Directory.GetCurrentDirectory() + "/Data/Settings.json";
            if (File.Exists(sSettingsPath))
            {
                try
                {
                    using (FileStream fs = new FileStream(sSettingsPath, FileMode.Open, FileAccess.Read))
                    {
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            if (sr.Peek() >= 0)
                            {
                                string rawData = sr.ReadToEnd();
                                m_Settings = JsonConvert.DeserializeObject<Settings>(rawData);

                                if (m_Settings != null)
                                {
                                    m_ChannelIRC = new IRCConnection("irc.twitch.tv", 6667, "iso8859-1", m_Settings.Username, m_Settings.OAuth, ChannelThread);
                                    m_WhisperIRC = new IRCConnection("irc.chat.twitch.tv", 6667, "iso8859-1", m_Settings.Username, m_Settings.OAuth, WhisperThread);
                                }
                            }
                        }
                    }
                }
                catch (FileNotFoundException fnfe)
                {
                    Console.WriteLine(fnfe.Message);
                    Console.WriteLine(sSettingsPath);
                }
            }
            else
            {
                Console.WriteLine("Error: Required File Settings.json is missing from the Data folder. Please re-add this file here: " + sSettingsPath + " to continue!");
            }
        }

        private void ChannelThread()
        {
            m_ChannelIRC.Write("CAP REQ :twitch.tv/membership");
            m_ChannelIRC.Write("CAP REQ :twitch.tv/commands");
            string sTempData = "";

            while (m_bRunning)
            {
                while (((sTempData = m_ChannelIRC.Read()) != null))
                {
                    try
                    {
                        Console.WriteLine(sTempData);

                        if (!m_ChannelIRC.Initialized)
                        {
                            if (sTempData.Contains("You are in a maze of twisty passages, all alike."))
                            {
                                Console.WriteLine("Initialized Channel IRC Connection!");
                                m_ChannelIRC.Initialized = true;
                                if (OnChannelIRCConnected != null)
                                {
                                    OnChannelIRCConnected();
                                }
                            }
                        }
                        else
                        {
                            if (sTempData.Contains("End of /NAMES list") && !sTempData.Contains("PRIVMSG"))
                            {
                                Console.WriteLine("Tucxbot has joined a channel!");
                                //m_ChannelIRC.Write("PRIVMSG #sirtucx :Hello World MrDestructoid");
                            }
                            else if (sTempData.Contains("tmi.twitch.tv PART #") && !sTempData.Contains("PRIVMSG"))
                            {
                                //:nightbot!nightbot@nightbot.tmi.twitch.tv PART #sirtucx
                                string sUsername = GetUsername(sTempData);
                                string sChannel = GetChannel(sTempData);
                                Console.WriteLine("{0} has left {1}\'s Channel", sUsername, sChannel);

                                m_ChannelInteractionArgs.Joining = false;
                                m_ChannelInteractionArgs.Target = sChannel;
                                m_ChannelInteractionArgs.Username = sUsername;

                                if (OnChannelPart != null)
                                {
                                    OnChannelPart(this, m_ChannelInteractionArgs);
                                }
                            }
                            else if (sTempData.Contains("tmi.twitch.tv JOIN #") && !sTempData.Contains("PRIVMSG"))
                            {
                                //:nightbot!nightbot@nightbot.tmi.twitch.tv JOIN #sirtucx
                                string sUsername = GetUsername(sTempData);
                                string sChannel = GetChannel(sTempData);
                                Console.WriteLine("{0} has joined {1}\'s Channel", sUsername, sChannel);

                                m_ChannelInteractionArgs.Joining = true;
                                m_ChannelInteractionArgs.Target = sChannel;
                                m_ChannelInteractionArgs.Username = sUsername;

                                if (OnChannelEnter != null)
                                {
                                    OnChannelEnter(this, m_ChannelInteractionArgs);
                                }
                            }
                            else if (sTempData.Contains(m_Settings.Username + " =") && !sTempData.Contains("PRIVMSG"))
                            {
                                if (GetUsername(sTempData) == m_Settings.Username)
                                {
                                    //:tucxbot.tmi.twitch.tv 353 tucxbot = #edemonster :matthewots lilmsqueenie carter4239 itsviikings tallpauldoll jackyb0i ndnl01 righand bunnydreams yunghotdawgwater jakel0809 dr_leeroy victordeigaard darkver666 razvan099 darkwhiskas arrow_46 afro_domo14 lotec25 dopestxrealzz ndnvdub dhgcarnage thepolockexpress mrfurteypunts roaringsmurf spacyyy doofyst cmayhem187 psz_ricky lolloki actabunnichoochoo__ rageogoblin thankphil cbmav 2600atari bluespanda1 stobi32 gunm4n27
                                    string sRawUserList = GetMessage(sTempData);
                                    string[] sUserList = sRawUserList.Split(' ');

                                    if (sUserList.Length > 0)
                                    {
                                        string sChannel = GetChannel(sTempData);
                                        foreach (string s in sUserList)
                                        {
                                            if (s == Username || s == sChannel)
                                            {
                                                continue;
                                            }
                                            Console.WriteLine("{0} is already in the channel!", s);

                                            m_ChannelInteractionArgs.Joining = true;
                                            m_ChannelInteractionArgs.Target = sChannel;
                                            m_ChannelInteractionArgs.Username = s;

                                            if (OnChannelEnter != null)
                                            {
                                                OnChannelEnter(this, m_ChannelInteractionArgs);
                                            }
                                        }
                                    }
                                }
                            }
                            else if (sTempData.Contains("PRIVMSG"))
                            {
                                //:sirtucx!sirtucx@sirtucx.tmi.twitch.tv PRIVMSG #sirtucx :What ??
                                string sUsername = GetUsername(sTempData);
                                string sChannel = GetChannel(sTempData);
                                string sMessage = GetMessage(sTempData);

                                Console.WriteLine("User -> {0}|Channel -> {1}|Message -> {2}", sUsername, sChannel, sMessage);
                                m_ChatInputArgs.Target = sChannel;
                                m_ChatInputArgs.Sender = sUsername;
                                m_ChatInputArgs.Message = sMessage;

                                if (OnChannelInput != null)
                                {
                                    OnChannelInput(this, m_ChatInputArgs);
                                }
                            }
                            else if (sTempData.Contains("PING") && !sTempData.Contains("PRIVMSG"))
                            {
                                m_ChannelIRC.Write("PONG :tmi.twitch.tv");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            m_ChannelIRC.Close();
        }
        private void WhisperThread()
        {
            m_WhisperIRC.Write("CAP REQ :twitch.tv/commands");
            string sTempData = "";

            while (m_bRunning)
            {
                while (((sTempData = m_WhisperIRC.Read()) != null))
                {
                    try
                    {
                        Console.WriteLine(sTempData);

                        if (!m_WhisperIRC.Initialized)
                        {
                            if (sTempData.Contains("You are in a maze of twisty passages, all alike."))
                            {
                                Console.WriteLine("Initialized Whisper IRC Connection!");
                                m_WhisperIRC.Initialized = true;
                            }
                        }
                        else
                        {
                            if (sTempData.Contains("WHISPER"))
                            {
                                //:sirtucx!sirtucx@sirtucx.tmi.twitch.tv WHISPER tucxbot :Hello

                                Console.WriteLine("Someone has whispered us!");
                                string sUsername = GetUsername(sTempData);
                                string sMessage = GetMessage(sTempData);

                                m_ChatInputArgs.Sender = sUsername;
                                m_ChatInputArgs.Message = sMessage;
                                m_ChatInputArgs.Target = Username;

                                if (OnWhisperInput != null)
                                {
                                    OnWhisperInput(this, m_ChatInputArgs);
                                }
                            }
                            else if (sTempData.Contains("PING") && !sTempData.Contains("WHISPER"))
                            {
                                m_WhisperIRC.Write("PONG :tmi.twitch.tv");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            m_WhisperIRC.Close();
        }
        public void JoinChannel(string sChannel)
        {
            if (m_ChannelIRC.Initialized)
            {
                m_ChannelIRC.Write("JOIN #" + sChannel + "\n");
                //Console.WriteLine("Tucxbot is attempting to join " + sChannel + "\'s channel!");
            }
        }
        public void LeaveChannel(string sChannel)
        {
            if (m_ChannelIRC.Initialized)
            {
                m_ChannelIRC.Write("PART #" + sChannel + "\n");
                //Console.WriteLine("Tucxbot has left " + sChannel + "\'s channel!");
            }
        }
        public void SendMessage(string sChannel, string sMessage)
        {
            if (m_ChannelIRC.Initialized)
            {
                m_ChannelIRC.Write("PRIVMSG #" + sChannel + " :" + sMessage);

                m_ChatOutputArgs.Target = sChannel;
                m_ChatOutputArgs.Message = sMessage;
                m_ChatOutputArgs.Sender = Username;

                if (OnChannelOutput != null)
                {
                    OnChannelOutput(this, m_ChatOutputArgs);
                }
            }
        }
        public void SendWhisper(string sUsername, string sMessage)
        {
            if (m_WhisperIRC.Initialized)
            {
                m_WhisperIRC.Write("PRIVMSG #jtv :/w " + sUsername + " " + sMessage);

                m_ChatOutputArgs.Target = sUsername;
                m_ChatOutputArgs.Message = sMessage;
                m_ChatOutputArgs.Sender = Username;

                if (OnWhisperOutput != null)
                {
                    OnWhisperOutput(this, m_ChatOutputArgs);
                }
            }
        }

        private string GetUsername(string sRawData)
        {
            int iStart = 1;
            int iEnd = sRawData.IndexOf('!', iStart);
            if (iEnd == -1)
            {
                iEnd = sRawData.IndexOf('.');
            }

            return sRawData.Substring(1, iEnd - iStart);
        }

        private string GetChannel(string sRawData)
        {
            int iStart = sRawData.IndexOf('#') + 1;
            int iEnd = sRawData.IndexOf(' ', iStart);

            if (iEnd == -1)
            {
                iEnd = sRawData.Length;
            }

            string sChannel = sRawData.Substring(iStart, iEnd - iStart);
            return sChannel;
        }
        private string GetMessage(string sRawData)
        {
            int iStart = sRawData.IndexOf('#');

            if (iStart == -1)
            {
                iStart = sRawData.IndexOf("WHISPER");
            }

            iStart = sRawData.IndexOf(':', iStart) + 1;
            string sMessage = sRawData.Substring(iStart, sRawData.Length - iStart);
            return sMessage;
        }
        
        public void StartConnection()
        {
            if (!m_ChannelIRC.Start())
            {
                m_ChannelIRC.StartConnection();
            }
            if (!m_WhisperIRC.Start())
            {
                m_WhisperIRC.StartConnection();
            }
        }
        public void CloseConnection()
        {
            m_WhisperIRC.CloseConnection();
            m_ChannelIRC.CloseConnection();
        }
        public void Close()
        {
            m_bRunning = false;
        }
    }
}
