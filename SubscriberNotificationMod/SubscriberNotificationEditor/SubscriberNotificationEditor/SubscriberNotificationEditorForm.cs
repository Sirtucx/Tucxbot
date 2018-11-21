using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using SubscriberNotificationMod;

namespace SubscriberNotificationEditor
{
    public partial class SubscriberNotificationEditorForm : Form
    {
        protected SubscriberNotificationSetting m_LoadedSettings;
        public SubscriberNotificationEditorForm()
        {
            InitializeComponent();
        }

        private void Log(string sMessage)
        {
            rtBoxLog.Text += $"{sMessage}\n";
            rtBoxLog.SelectionStart = rtBoxLog.Text.Length;
            rtBoxLog.ScrollToCaret();
        }

        private void Clear()
        {
            m_UserGroup.Visible = false;
            m_UserTB.Text = "";
            m_MessageInputTB.Text = "";
        }
        
        private void OpenButton_Click(object sender, EventArgs e)
        {
            Clear();
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Environment.CurrentDirectory;
            ofd.Filter = "JSON Files (*.json)|*.json";
            ofd.FilterIndex = 0;


            if (ofd.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader;
                if ((reader = new StreamReader(ofd.OpenFile())) != null)
                {
                    string rawJson = reader.ReadToEnd();

                    SubscriberNotificationSetting tempSettings = JsonConvert.DeserializeObject<SubscriberNotificationSetting>(rawJson);
                    reader.Close();
                    if (tempSettings != null)
                    {
                        m_LoadedSettings = tempSettings;
                    }
                }
            }

            if (m_LoadedSettings != null)
            {
                m_GenerateGroup.Visible = false;
                m_UserGroup.Visible = true;
                m_UserGroup.Visible = m_LoadedSettings.SubscriberMessages.Count > 0;

                if (m_LoadedSettings.SubscriberMessages.Count > 0)
                {
                    m_SaveButton.Visible = true;
                    m_UserCB.Items.Clear();
                    foreach (KeyValuePair<string, SubscriberMessage> user in m_LoadedSettings.SubscriberMessages)
                    {
                        m_UserCB.Items.Add(user.Key);
                    }
                    m_UserCB.SelectedIndex = 0;
                    m_MessageTypeCB.SelectedIndex = 0;
                }
                Log($"Loaded Settings ({ofd.FileName})");
            }
            else
            {
                Log($"Failed to load file ({ofd.FileName})");
            }
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            Clear();
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.InitialDirectory = Environment.CurrentDirectory;
            fileDialog.Filter = "JSON File (*.json)|*.json";
            fileDialog.FilterIndex = 0;

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamWriter writer = new StreamWriter(fileDialog.FileName, false);

                m_LoadedSettings = new SubscriberNotificationSetting();
                string defaultJSON = JsonConvert.SerializeObject(m_LoadedSettings, Formatting.Indented);


                writer.Write(defaultJSON);
                writer.Flush();
                writer.Close();

                if (m_LoadedSettings != null)
                {
                    m_UserGroup.Visible = true;
                    m_GenerateGroup.Visible = false;
                    m_UserCB.Items.Clear();
                    m_MessageTypeCB.SelectedIndex = 0;
                    Log($"Generated JSON file at {fileDialog.FileName}");
                }
            }
        }

        private void AddUserButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(m_UserTB.Text))
            {
                if (!m_LoadedSettings.SubscriberMessages.ContainsKey(m_UserTB.Text))
                {
                    m_LoadedSettings.SubscriberMessages.Add(m_UserTB.Text.ToLower(), new SubscriberMessage(m_UserTB.Text.ToLower()));
                    m_UserCB.Items.Add(m_UserTB.Text.ToLower());
                    m_UserCB.SelectedIndex = 0;
                    Log($"Added {m_UserTB.Text} to Settings, Press the Save button to write to the file.");
                }
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (m_UserCB.Items.Count > 1)
            {
                if (m_UserCB.Items[m_UserCB.SelectedIndex].ToString() != "default")
                {
                    m_LoadedSettings.SubscriberMessages.Remove(m_UserCB.Items[m_UserCB.SelectedIndex].ToString());
                    m_UserCB.Items.RemoveAt(m_UserCB.SelectedIndex);
                    m_UserCB.SelectedIndex = 0;
                    Log($"Removed {m_UserCB.Items[m_UserCB.SelectedIndex].ToString()} from Settings, Press the Save button to write to the file.");
                }
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.InitialDirectory = Environment.CurrentDirectory;
            fileDialog.Filter = "JSON File (*.json)|*.json";
            fileDialog.FilterIndex = 0;

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamWriter writer = new StreamWriter(fileDialog.FileName, false);
                writer.Write(JsonConvert.SerializeObject(m_LoadedSettings, Formatting.Indented));
                writer.Flush();
                writer.Close();
                Log($"Saved Settings ({fileDialog.FileName}");
            }
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(m_MessageInputTB.Text))
            {
                string sUsername = m_UserCB.Items[m_UserCB.SelectedIndex].ToString();

                switch (m_MessageTypeCB.SelectedIndex)
                {
                    case 0:
                        {
                            m_LoadedSettings.SubscriberMessages[sUsername].TierOneMessage = m_MessageInputTB.Text;
                            break;
                        }
                    case 1:
                        {
                            m_LoadedSettings.SubscriberMessages[sUsername].TierTwoMessage = m_MessageInputTB.Text;
                            break;
                        }
                    case 2:
                        {
                            m_LoadedSettings.SubscriberMessages[sUsername].TierThreeMessage = m_MessageInputTB.Text;
                            break;
                        }
                    case 3:
                        {
                            m_LoadedSettings.SubscriberMessages[sUsername].PrimeMessage = m_MessageInputTB.Text;
                            break;
                        }
                    case 4:
                        {
                            m_LoadedSettings.SubscriberMessages[sUsername].GiftingMessage = m_MessageInputTB.Text;
                            break;
                        }
                    case 5:
                        {
                            m_LoadedSettings.SubscriberMessages[sUsername].SubStreakMessage = m_MessageInputTB.Text;
                            break;
                        }
                }
                Log($"Updated {m_MessageTypeCB.Items[m_MessageTypeCB.SelectedIndex].ToString()} for {sUsername}, press the Save button to save the current changes.");
            }
        }

        private void MessageTypeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sUsername = m_UserCB.Items[m_UserCB.SelectedIndex].ToString();
            switch (m_MessageTypeCB.SelectedIndex)
            {
                case 0:
                    {
                        m_MessageInputTB.Text = m_LoadedSettings.SubscriberMessages[sUsername].TierOneMessage;
                        break;
                    }
                case 1:
                    {
                        m_MessageInputTB.Text = m_LoadedSettings.SubscriberMessages[sUsername].TierTwoMessage;
                        break;
                    }
                case 2:
                    {
                        m_MessageInputTB.Text = m_LoadedSettings.SubscriberMessages[sUsername].TierThreeMessage;
                        break;
                    }
                case 3:
                    {
                        m_MessageInputTB.Text = m_LoadedSettings.SubscriberMessages[sUsername].PrimeMessage;
                        break;
                    }
                case 4:
                    {
                        m_MessageInputTB.Text = m_LoadedSettings.SubscriberMessages[sUsername].GiftingMessage;
                        break;
                    }
                case 5:
                    {
                        m_MessageInputTB.Text = m_LoadedSettings.SubscriberMessages[sUsername].SubStreakMessage;
                        break;
                    }
            }
        }
    }
}
