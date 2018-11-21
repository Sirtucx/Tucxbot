using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twitch_Websocket;
using Twitch_Websocket.Mod_Interfaces;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;
using Microsoft.VisualBasic;

namespace SubscriberNotificationMod
{
    class SubscriberNotification : IOnSubscriberMod
    {
        private TwitchClient m_TwitchClient;
        private SubscriberNotificationSetting m_SubSettings;

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
                                m_SubSettings = JsonConvert.DeserializeObject<SubscriberNotificationSetting>(rawData);

                                if (m_SubSettings != null)
                                {
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

        public void Process(UserNotice usernotice)
        {
            if (m_TwitchClient == null)
            {
                m_TwitchClient = TwitchClient.GetInstance();
            }

            if (m_SubSettings != null)
            {
                string sMessage = m_SubSettings.GetMessage(usernotice);

                m_TwitchClient.SendChatMessage(usernotice.Channel, sMessage);
            }
        }
        public void Shutdown()
        {

        }
    }
}
