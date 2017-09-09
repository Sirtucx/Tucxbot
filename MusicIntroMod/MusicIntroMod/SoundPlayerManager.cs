using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMPLib;
using System.IO;
using Newtonsoft.Json;
using TwitchIRC_TCP;
using Twitch_Websocket;
using Twitch_Websocket.Mod_Interfaces;

namespace MusicIntroMod
{
    class SoundPlayerManager : IChatMessageMod //IChannelInputMod
    {
        private Dictionary<string, string> m_UserSoundPaths;
        private List<string> m_sUsersPlayed;
        private Queue<string> m_sUserQueue;
        private IntroSettings m_IntroSettings;
        private WindowsMediaPlayer m_WMPlayer;

        public SoundPlayerManager()
        {
            m_UserSoundPaths = new Dictionary<string, string>();
            m_sUsersPlayed = new List<string>();
            m_sUserQueue = new Queue<string>();
            m_WMPlayer = new WindowsMediaPlayer();
            LoadUsers();
        }
        //public void ProcessMessage(string sChannel, string sUsername, string sMessage)
        //{
        //    PlaySound(sUsername);
        //}
        public void Process(ChatMessage chatMessage)
        {
            PlaySound(chatMessage.Username);
        }
        public void Shutdown()
        {
            m_sUserQueue.Clear();
            m_UserSoundPaths.Clear();
        }
        private void LoadUsers()
        {
            string sMusicPath = Environment.CurrentDirectory + "/Intro Music/IntroSettings.json";
            if (File.Exists(sMusicPath))
            {
                try
                {
                    using (FileStream fs = new FileStream(sMusicPath, FileMode.Open, FileAccess.Read))
                    {
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            if (sr.Peek() >= 0)
                            {
                                string rawData = sr.ReadToEnd();
                                m_IntroSettings = JsonConvert.DeserializeObject<IntroSettings>(rawData);

                                if (m_IntroSettings != null)
                                {
                                    foreach (User user in m_IntroSettings.UserData)
                                    {
                                        m_UserSoundPaths.Add(user.Username, Environment.CurrentDirectory + "/Intro Music/" + user.FileName);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                { }
            }
            else
            {
                Console.WriteLine("Error: Required File IntroSettings.json is missing from the Data folder. Please re-add this file here: " + sMusicPath + " to continue!");
            }
        }
        private void PlaySound(string sUsername)
        {
            if (m_UserSoundPaths.ContainsKey(sUsername))
            {
                if (!m_sUsersPlayed.Contains(sUsername))
                {
                    if (m_WMPlayer.playState == WMPPlayState.wmppsPlaying)
                    {
                        m_sUserQueue.Enqueue(sUsername);
                    }
                    else
                    {
                        if (m_sUserQueue.Count > 0)
                        {
                            string sQueuedUsername = m_sUserQueue.Dequeue();
                            m_WMPlayer.URL = m_UserSoundPaths[sQueuedUsername];
                            m_sUsersPlayed.Add(sQueuedUsername);
                        }
                        else
                        {
                            m_WMPlayer.URL = m_UserSoundPaths[sUsername];
                            m_sUsersPlayed.Add(sUsername);
                        }
                        m_WMPlayer.controls.play();
                    }
                }
                else
                {
                    if (m_WMPlayer.playState != WMPPlayState.wmppsPlaying)
                    {
                        if (m_sUserQueue.Count > 0)
                        {
                            string sQueuedUsername = m_sUserQueue.Dequeue();
                            m_WMPlayer.URL = m_UserSoundPaths[sQueuedUsername];
                            m_sUsersPlayed.Add(sQueuedUsername);
                            m_WMPlayer.controls.play();
                        }
                    }
                }
            }
        }
    }
}
