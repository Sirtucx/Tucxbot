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
using MusicIntroMod;
using Newtonsoft.Json;

namespace MusicIntroEditor
{
    public partial class MusicIntroEditorForm : Form
    {
        private IntroSettings m_LoadedSettings;
        public MusicIntroEditorForm()
        {
            InitializeComponent();
        }

        private void Log(string sMessage)
        {
            rtBoxLog.Text += $"{sMessage}\n";
            rtBoxLog.SelectionStart = rtBoxLog.Text.Length;
            rtBoxLog.ScrollToCaret();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            Clear();
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.InitialDirectory = Environment.CurrentDirectory;
            fileDialog.Filter = "JSON File (*.json)|*.json";
            fileDialog.FilterIndex = 0;
            
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamWriter writer = new StreamWriter(fileDialog.FileName, false);

                m_LoadedSettings = new IntroSettings();
                string defaultJSON = JsonConvert.SerializeObject(m_LoadedSettings, Formatting.Indented);


                writer.Write(defaultJSON);
                writer.Flush();
                writer.Close();

                if (m_LoadedSettings != null)
                {
                    groupAddUser.Visible = true;
                    groupGenerate.Visible = false;
                    groupContents.Visible = false;
                    cBoxUsers.Items.Clear();
                    tBoxExistingPath.Text = "";
                    Log($"Generated JSON file at {fileDialog.FileName}");
                }
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
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

                    IntroSettings tempSettings = JsonConvert.DeserializeObject<IntroSettings>(rawJson);
                    reader.Close();
                    if (tempSettings != null)
                    {
                        m_LoadedSettings = tempSettings;
                    }
                }
            }

            if (m_LoadedSettings != null)
            {
                groupGenerate.Visible = false;
                groupAddUser.Visible = true;
                groupContents.Visible = m_LoadedSettings.UserData.Count > 0;

                if (m_LoadedSettings.UserData.Count > 0)
                {
                    btnSave.Visible = true;
                    cBoxUsers.Items.Clear();
                    foreach (User user in m_LoadedSettings.UserData)
                    {
                        cBoxUsers.Items.Add(user.Username);
                    }
                    cBoxUsers.SelectedIndex = 0;
                    tBoxExistingPath.Text = m_LoadedSettings.UserData[0].FileName;
                    tBoxExistingPath.SelectionStart = rtBoxLog.Text.Length;
                    tBoxExistingPath.ScrollToCaret();
                }
                Log($"Loaded Settings ({ofd.FileName})");
            }
            else
            {
                Log($"Failed to load file ({ofd.FileName})");
            }
        }

        private void btnLoadMusic_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Environment.CurrentDirectory;
            ofd.Filter = "MP3 files (*.mp3)|*.mp3|Windows Audio Files (*.wav)|*.wav";
            ofd.FilterIndex = 0;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string fileName = ofd.FileName;
                tBoxPath.Text = fileName;
                tBoxPath.SelectionStart = rtBoxLog.Text.Length;
                tBoxPath.ScrollToCaret();
            }
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tBoxPath.Text))
            {
                if (!string.IsNullOrEmpty(tBoxUser.Text))
                {
                    bool bFound = false;
                    foreach(User user in m_LoadedSettings.UserData)
                    {
                        if (user.Username == tBoxUser.Text)
                        {
                            user.FileName = tBoxPath.Text;
                            bFound = true;
                            Log($"Edited {tBoxUser.Text} in Settings");
                        }
                    }
                    if (!bFound)
                    {
                        m_LoadedSettings.UserData.Add(new User(tBoxUser.Text, tBoxPath.Text));
                        Log($"Added {tBoxUser.Text} to Settings");
                    }

                    groupContents.Visible = true;
                    btnSave.Visible = true;
                    cBoxUsers.Items.Clear();
                    foreach (User user in m_LoadedSettings.UserData)
                    {
                        cBoxUsers.Items.Add(user.Username);
                    }
                    cBoxUsers.SelectedIndex = 0;
                    tBoxExistingPath.Text = m_LoadedSettings.UserData[0].FileName;
                    tBoxExistingPath.SelectionStart = rtBoxLog.Text.Length;
                    tBoxExistingPath.ScrollToCaret();

                }
            }
        }

        private void Clear()
        {
            groupAddUser.Visible = false;
            tBoxPath.Text = "";
            tBoxUser.Text = "";
        }

        private void cBoxUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            tBoxExistingPath.Text = m_LoadedSettings.UserData[cBoxUsers.SelectedIndex].FileName;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.InitialDirectory = Environment.CurrentDirectory;
            fileDialog.Filter = "JSON File (*.json)|*.json";
            fileDialog.FilterIndex = 0;

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamWriter writer = new StreamWriter(fileDialog.FileName, false);
                writer.Write(JsonConvert.SerializeObject(m_LoadedSettings));
                writer.Flush();
                writer.Close();
                Log($"Saved Settings ({fileDialog.FileName}");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Log($"Removed {m_LoadedSettings.UserData[cBoxUsers.SelectedIndex].Username}");
            m_LoadedSettings.UserData.RemoveAt(cBoxUsers.SelectedIndex);
            if (m_LoadedSettings.UserData.Count > 0)
            {
                cBoxUsers.Items.Clear();
                foreach (User user in m_LoadedSettings.UserData)
                {
                    cBoxUsers.Items.Add(user.Username);
                }
                cBoxUsers.SelectedIndex = 0;
                tBoxExistingPath.Text = m_LoadedSettings.UserData[0].FileName;
                tBoxExistingPath.SelectionStart = rtBoxLog.Text.Length;
                tBoxExistingPath.ScrollToCaret();
            }
            else
            {
                groupContents.Visible = false;
            }
        }
    }
}
