namespace MusicIntroMod
{
    using System;
    using System.Collections.Generic;
    using WMPLib;
    using System.IO;
    using Newtonsoft.Json;
    using System.Threading;
    using System.Windows.Forms;
    using Microsoft.VisualBasic;
    using Twitch.Mods;
    using Twitch.Containers;
    
    public class SoundPlayerManager : ChatMessageMod
    {
        private List<string> m_usersPlayed;
        private IntroSettings m_introSettings;
        private Thread m_playerThread;
        private bool m_isActive;
        
        private readonly Queue<string> m_userQueue;
        private readonly Dictionary<string, string> m_userSoundPaths;
        private readonly WindowsMediaPlayer m_windowsMediaPlayer;

        public SoundPlayerManager()
        {
            m_userSoundPaths = new Dictionary<string, string>();
            m_usersPlayed = new List<string>();
            m_userQueue = new Queue<string>();
            m_windowsMediaPlayer = new WindowsMediaPlayer();
            LoadUsers();
            m_isActive = true;
            m_playerThread = new Thread(SoundThread);
            m_playerThread.Start();
        }

        protected override void ProcessChatMessage(ChatMessage chatMessage)
        {
            bool hasCommandBeenTyped = false;

            if (chatMessage.Username == chatMessage.Channel)
            {
                switch (chatMessage.Message)
                {
                    case "!mim_skip":
                    {
                        Skip();
                        hasCommandBeenTyped = true;
                        break;
                    }
                    case "!mim_skipall":
                    {
                        SkipAll();
                        hasCommandBeenTyped = true;
                        break;
                    }
                    case "!mim_reloadall":
                    {
                        m_usersPlayed.Clear();
                        hasCommandBeenTyped = true;
                        break;
                    }
                    default:
                    {
                        break;
                    }
                }
            }

            if (!hasCommandBeenTyped)
            {
                PlaySound(chatMessage.Username);
            }
        }
        public override void Shutdown()
        {
            m_isActive = false;
            m_userQueue.Clear();
            m_userSoundPaths.Clear();
            m_windowsMediaPlayer.controls.stop();
            m_playerThread = null;
        }
        
        private void Skip()
        {
            if (m_windowsMediaPlayer.playState == WMPPlayState.wmppsPlaying)
            {
                m_windowsMediaPlayer.controls.stop();
            }
        }
        private void SkipAll()
        {
            foreach (KeyValuePair <string, string> kvp in m_userSoundPaths)
            {
                if (m_usersPlayed == null)
                {
                    m_usersPlayed = new List<string>();
                }
                if (!m_usersPlayed.Contains(kvp.Key))
                {
                    m_usersPlayed.Add(kvp.Key);
                }
            }

            m_userQueue?.Clear();
            Skip();
        }
        private void LoadUsers()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Load your Music Intro Settings file.";
            fileDialog.InitialDirectory = Environment.CurrentDirectory;
            fileDialog.Filter = "JSON File (*.json)|*.json";
            fileDialog.FilterIndex = 0;
            bool hasSelectedValidFile = false;

            do
            {
                if (fileDialog.ShowDialog() != DialogResult.OK)
                {
                    continue;
                }

                hasSelectedValidFile = ValidateSettingsFile(fileDialog.FileName);
            }
            while (!hasSelectedValidFile);
        }
        private bool ValidateSettingsFile(string fileName)
        {
            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    if (sr.Peek() < 0)
                    {
                        return false;
                    }

                    string rawData = sr.ReadToEnd();
                    m_introSettings = JsonConvert.DeserializeObject<IntroSettings>(rawData);

                    if (m_introSettings != null)
                    {
                        foreach (User user in m_introSettings.UserData)
                        {
                            m_userSoundPaths.Add(user.Username.ToLower(), user.FileName);
                        }
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Interaction.MsgBox(
                    $"Unfortunately, there was an issue loading\n{fileName}. It was most likely not formatted properly. Please use a JSON parser to fix your settings file.\nOnce you have done so please reload this mod and try again.");
                return true;
            }

            return false;
        }
        private void PlaySound(string username)
        {
            if (m_userSoundPaths.ContainsKey(username))
            {
                if (!m_usersPlayed.Contains(username))
                {
                    m_userQueue.Enqueue(username);
                }
            }
        }
        private void SoundThread()
        {
            while (m_isActive)
            {
                try
                {
                    if (m_windowsMediaPlayer.playState != WMPPlayState.wmppsPlaying)
                    {
                        if (m_userQueue.Count > 0)
                        {
                            string queuedUsername = m_userQueue.Dequeue();
                            m_windowsMediaPlayer.URL = m_userSoundPaths[queuedUsername];
                            m_usersPlayed.Add(queuedUsername);
                            m_windowsMediaPlayer.controls.play();
                        }
                    }
                }
                catch (Exception e)
                {
                    // ignored
                }
            }
        }
    }
}
