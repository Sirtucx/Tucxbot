namespace SubscriberNotificationMod
{
    using System;
    using System.Windows.Forms;
    using Newtonsoft.Json;
    using System.IO;
    using Microsoft.VisualBasic;
    using Twitch.Core;
    using Twitch.Mods;
    using Twitch.Containers;
    
    class SubscriberNotification : SubscriberMod
    {
        private TwitchClient m_twitchClient;
        private SubscriberNotificationSetting m_subSettings;

        public SubscriberNotification()
        {
            LoadUsers();
        }

        private void LoadUsers()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Load your Music Intro Settings file.";
            fileDialog.InitialDirectory = Environment.CurrentDirectory;
            fileDialog.Filter = "JSON File (*.json)|*.json";
            fileDialog.FilterIndex = 0;
            bool hasLoadedValidFile = false;

            while (!hasLoadedValidFile)
            {
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    hasLoadedValidFile = ValidateSettings(fileDialog.FileName);
                }
            }
        }

        private bool ValidateSettings(string fileName)
        {
            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    if (sr.Peek() >= 0)
                    {
                        string rawData = sr.ReadToEnd();
                        m_subSettings = JsonConvert.DeserializeObject<SubscriberNotificationSetting>(rawData);

                        if (m_subSettings != null)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Interaction.MsgBox($"Unfortunately, there was an issue loading\n{fileName}. It was most likely not formatted properly. Please use a JSON parser to fix your settings file.\nOnce you have done so please reload this mod and try again.");
                return true;
            }

            return false;
        }
        
        protected override void ProcessChatMessage(UserNotice subNotice)
        {
            if (m_twitchClient == null)
            {
                m_twitchClient = TwitchClient.GetInstance();
            }

            if (m_subSettings != null)
            {
                string sMessage = m_subSettings.GetMessage(subNotice);

                m_twitchClient.SendChatMessage(subNotice.Channel, sMessage);
            }
        }

        public override void Shutdown()
        {
            m_twitchClient = null;
            m_subSettings = null;
        }
    }
}
