using System;
using System.Collections.Generic;
using WMPLib;
using System.IO;
using Newtonsoft.Json;
using Twitch_Websocket;
using Twitch_Websocket.Mod_Interfaces;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace MusicIntroMod
{
    public class SoundPlayerManager : IChatMessageMod
    {
        private Dictionary<string, string> m_UserSoundPaths;
        private List<string> m_sUsersPlayed;
        private Queue<string> m_sUserQueue;
        private IntroSettings m_IntroSettings;
        private WindowsMediaPlayer m_WMPlayer;
        private Thread m_PlayerThread;
        private bool m_bIsActive;

        public SoundPlayerManager()
        {
            m_UserSoundPaths = new Dictionary<string, string>();
            m_sUsersPlayed = new List<string>();
            m_sUserQueue = new Queue<string>();
            m_WMPlayer = new WindowsMediaPlayer();
            LoadUsers();
            m_bIsActive = true;
            m_PlayerThread = new Thread(new ThreadStart(SoundThread));
            m_PlayerThread.Start();
        }
        public void Process(ChatMessage chatMessage)
        {
            bool bCommandTyped = false;

            if (chatMessage.Username == chatMessage.Channel)
            {
                if (chatMessage.Message == "!mim_skip")
                {
                    Skip();
                    bCommandTyped = true;
                }
                else if (chatMessage.Message == "!mim_skipall")
                {
                    SkipAll();
                    bCommandTyped = true;
                }
                else if (chatMessage.Message == "!mim_reloadall")
                {
                    m_sUsersPlayed.Clear();
                    bCommandTyped = true;
                }
            }

            if (!bCommandTyped)
            {
                PlaySound(chatMessage.Username);
            }
        }
        public void Shutdown()
        {
            m_bIsActive = false;
            m_sUserQueue.Clear();
            m_UserSoundPaths.Clear();
            m_WMPlayer.controls.stop();
        }
        private void Skip()
        {
            if (m_WMPlayer.playState == WMPPlayState.wmppsPlaying)
            {
                m_WMPlayer.controls.stop();
            }
        }
        private void SkipAll()
        {
            foreach (KeyValuePair <string, string> kvp in m_UserSoundPaths)
            {
                if (m_sUsersPlayed == null)
                {
                    m_sUsersPlayed = new List<string>();
                }
                if (!m_sUsersPlayed.Contains(kvp.Key))
                {
                    m_sUsersPlayed.Add(kvp.Key);
                }
            }

            if (m_sUserQueue != null)
            {
                m_sUserQueue.Clear();
            }
            Skip();
        }
        private void LoadUsers()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Load your Music Intro Settings file.";
            fileDialog.InitialDirectory = Environment.CurrentDirectory;
            fileDialog.Filter = "JSON File (*.json)|*.json";
            fileDialog.FilterIndex = 0;
            bool bSelectedFile = false;

            while (!bSelectedFile)
            {
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (StreamReader sr = new StreamReader(fileDialog.FileName))
                        {
                            if (sr.Peek() >= 0)
                            {
                                string rawData = sr.ReadToEnd();
                                m_IntroSettings = JsonConvert.DeserializeObject<IntroSettings>(rawData);

                                if (m_IntroSettings != null)
                                {
                                    foreach (User user in m_IntroSettings.UserData)
                                    {
                                        m_UserSoundPaths.Add(user.Username.ToLower(), user.FileName);
                                    }
                                    bSelectedFile = true;
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Interaction.MsgBox($"Unfortunately, there was an issue loading\n{fileDialog.FileName}. It was most likely not formatted properly. Please use a JSON parser to fix your settings file.\nOnce you have done so please reload this mod and try again.");
                        bSelectedFile = true;
                    }
                }
            }
        }
        private void PlaySound(string sUsername)
        {
            if (m_UserSoundPaths.ContainsKey(sUsername))
            {
                if (!m_sUsersPlayed.Contains(sUsername))
                {
                    m_sUserQueue.Enqueue(sUsername);
                }
            }
        }
        private void SoundThread()
        {
            while (m_bIsActive)
            {
                try
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
                catch (Exception e)
                {

                }
            }
        }
    }
}
