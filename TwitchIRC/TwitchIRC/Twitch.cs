using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace TwitchIRC
{
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

        private bool m_bRunning;
        private Settings m_Settings;

        private Twitch()
        {
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
                                    m_ChannelIRC.Start();
                                    m_WhisperIRC = new IRCConnection("irc.chat.twitch.tv", 6667, "iso8859-1", m_Settings.Username, m_Settings.OAuth, WhisperThread);
                                    m_WhisperIRC.Start();
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
            string sTempData = "";

            while (m_bRunning && ((sTempData = m_ChannelIRC.Read()) != null))
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
                        }
                    }
                    else
                    {
                        if (sTempData.Contains("End of /NAMES list") && !sTempData.Contains("PRIVMSG"))
                        {
                            Console.WriteLine("Tucxbot has joined a channel!");
                            m_ChannelIRC.Write("PRIVMSG #sirtucx :Hello World MrDestructoid");
                        }
                        else if (sTempData.Contains("tmi.twitch.tv PART #") && !sTempData.Contains("PRIVMSG"))
                        {
                            // Tucxbot has left the channel
                        }
                        else if (sTempData.Contains("PRIVMSG"))
                        {
                            //:sirtucx!sirtucx@sirtucx.tmi.twitch.tv PRIVMSG #sirtucx :What ??
                            string sUsername = sTempData.Substring(1, sTempData.IndexOf('!') - 1);

                            int iStart = sTempData.IndexOf('#') + 1;
                            int iEnd = sTempData.IndexOf(':', iStart) - 1;
                            string sChannel = sTempData.Substring(iStart, iEnd - iStart);

                            iStart = iEnd + 1;
                            iEnd = sTempData.Length;
                            string sMessage = sTempData.Substring(iStart, iEnd - iStart);

                            Console.WriteLine("User -> {0}|Channel -> {1}|Message -> {2}", sUsername, sChannel, sMessage);
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
        private void WhisperThread()
        {
            m_WhisperIRC.Write("CAP REQ :twitch.tv/commands");
            string sTempData = "";

            while (m_bRunning && ((sTempData = m_WhisperIRC.Read()) != null))
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
                            Console.WriteLine("Someone has whispered us!");
                        }
                        else if (sTempData.Contains("PING") && !sTempData.Contains("WHISPER"))
                        {
                            m_WhisperIRC.Write("PONG :tmi.twitch.tv");
                        }
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        public void JoinChannel(string sChannel)
        {
            if (m_ChannelIRC.Initialized)
            {
                m_ChannelIRC.Write("JOIN #" + sChannel + "\n");
                Console.WriteLine("Tucxbot is attempting to join " + sChannel + "\'s channel!");
            }
        }
        public void LeaveChannel(string sChannel)
        {
            if (m_ChannelIRC.Initialized)
            {
                m_ChannelIRC.Write("PART #" + sChannel + "\n");
                Console.WriteLine("Tucxbot has left " + sChannel + "\'s channel!");
            }
        }

        public void CloseConnection()
        {
            m_WhisperIRC.CloseConnection();
            m_ChannelIRC.CloseConnection();
            m_bRunning = false;
        }
    }
}
