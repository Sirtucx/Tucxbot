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

        private bool m_bInitialized;
        private Settings m_Settings;

        private Twitch()
        {
            m_bInitialized = false;
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
                                    Console.WriteLine("Username: " + m_Settings.Username);
                                    Console.WriteLine("OAuth: " + m_Settings.OAuth);
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
    }
}
