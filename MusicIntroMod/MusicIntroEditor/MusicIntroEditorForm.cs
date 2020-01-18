using System.Collections.Generic;
using System.Linq;

namespace MusicIntroEditor
{
    using System;
    using System.Windows.Forms;
    using System.IO;
    using MusicIntroMod;
    using Newtonsoft.Json;
    
    public partial class MusicIntroEditorForm : Form
    {
        private IntroSettings m_loadedSettings;
        public MusicIntroEditorForm()
        {
            InitializeComponent();
        }

        private bool LoadSettingsFile(out string fileName)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Environment.CurrentDirectory;
            ofd.Filter = @"JSON Files (*.json)|*.json";
            ofd.FilterIndex = 0;
            bool hasLoadedSettings = false;
            
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader reader = new StreamReader(ofd.OpenFile()))
                {
                    string rawJson = reader.ReadToEnd();

                    IntroSettings tempSettings = JsonConvert.DeserializeObject<IntroSettings>(rawJson);
                    reader.Close();
                    if (tempSettings != null)
                    {
                        m_loadedSettings = tempSettings;
                        hasLoadedSettings = true;
                    }
                }
            }
            
            fileName = ofd.FileName;
            return hasLoadedSettings;
        }
        private void Log(string sMessage)
        {
            logRichTextBox.Text += $@"{sMessage}\n";
            logRichTextBox.SelectionStart = logRichTextBox.Text.Length;
            logRichTextBox.ScrollToCaret();
        }
        private void Clear()
        {
            groupAddUser.Visible = false;
            pathTextBox.Text = "";
            userTextBox.Text = "";
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            Clear();
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.InitialDirectory = Environment.CurrentDirectory;
            fileDialog.Filter = @"JSON File (*.json)|*.json";
            fileDialog.FilterIndex = 0;
            
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamWriter writer = new StreamWriter(fileDialog.FileName, false);

                m_loadedSettings = new IntroSettings();
                string defaultJson = JsonConvert.SerializeObject(m_loadedSettings, Formatting.Indented);


                writer.Write(defaultJson);
                writer.Flush();
                writer.Close();

                if (m_loadedSettings != null)
                {
                    groupAddUser.Visible = true;
                    groupGenerate.Visible = false;
                    groupContents.Visible = false;
                    userComboBox.Items.Clear();
                    existingPathTextBox.Text = "";
                    Log($"Generated JSON file at {fileDialog.FileName}");
                }
            }
        }
        private void OpenButton_Click(object sender, EventArgs e)
        {
            Clear();
            
            if (LoadSettingsFile(out string fileName) && m_loadedSettings != null)
            {
                groupGenerate.Visible = false;
                groupAddUser.Visible = true;
                groupContents.Visible = m_loadedSettings.UserData.Count > 0;

                if (m_loadedSettings.UserData.Count > 0)
                {
                    saveButton.Visible = true;
                    userComboBox.Items.Clear();
                    foreach (User user in m_loadedSettings.UserData)
                    {
                        userComboBox.Items.Add(user.Username);
                    }
                    userComboBox.SelectedIndex = 0;
                    existingPathTextBox.Text = m_loadedSettings.UserData[0].FileName;
                    existingPathTextBox.SelectionStart = logRichTextBox.Text.Length;
                    existingPathTextBox.ScrollToCaret();
                }
                Log($"Loaded Settings ({fileName})");
            }
            else
            {
                Log($"Failed to load file ({fileName})");
            }
        }
        private void LoadMusicButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Environment.CurrentDirectory;
            ofd.Filter = @"MP3 files (*.mp3)|*.mp3|Windows Audio Files (*.wav)|*.wav";
            ofd.FilterIndex = 0;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string fileName = ofd.FileName;
                pathTextBox.Text = fileName;
                pathTextBox.SelectionStart = logRichTextBox.Text.Length;
                pathTextBox.ScrollToCaret();
            }
        }
        private void AddUserButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(pathTextBox.Text) || string.IsNullOrEmpty(userTextBox.Text))
            {
                return;
            }
            

            List<User> matchingUsers = m_loadedSettings.UserData.Where(user => user.Username == userTextBox.Text).ToList();
            bool hasFoundUser = matchingUsers.Count > 0;
            
            foreach(User user in matchingUsers)
            {
                user.FileName = pathTextBox.Text;
                Log($"Edited {userTextBox.Text} in Settings");
            }
            
            if (!hasFoundUser)
            {
                m_loadedSettings.UserData.Add(new User(userTextBox.Text, pathTextBox.Text));
                Log($"Added {userTextBox.Text} to Settings");
            }

            groupContents.Visible = true;
            saveButton.Visible = true;
            userComboBox.Items.Clear();
            foreach (User user in m_loadedSettings.UserData)
            {
                userComboBox.Items.Add(user.Username);
            }
            userComboBox.SelectedIndex = 0;
            existingPathTextBox.Text = m_loadedSettings.UserData[0].FileName;
            existingPathTextBox.SelectionStart = logRichTextBox.Text.Length;
            existingPathTextBox.ScrollToCaret();
        }
        private void UserComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            existingPathTextBox.Text = m_loadedSettings.UserData[userComboBox.SelectedIndex].FileName;
        }
        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.InitialDirectory = Environment.CurrentDirectory;
            fileDialog.Filter = @"JSON File (*.json)|*.json";
            fileDialog.FilterIndex = 0;

            if (fileDialog.ShowDialog() != DialogResult.OK)
            {
                Log($@"Failed to save {fileDialog.FileName}!");
                return;
            }
            using (StreamWriter writer = new StreamWriter(fileDialog.FileName, false))
            {
                writer.Write(JsonConvert.SerializeObject(m_loadedSettings));
                writer.Flush();
                writer.Close();
                Log($"Saved Settings ({fileDialog.FileName}");
            }
        }
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            Log($"Removed {m_loadedSettings.UserData[userComboBox.SelectedIndex].Username}");
            m_loadedSettings.UserData.RemoveAt(userComboBox.SelectedIndex);
            if (m_loadedSettings.UserData.Count > 0)
            {
                userComboBox.Items.Clear();
                foreach (User user in m_loadedSettings.UserData)
                {
                    userComboBox.Items.Add(user.Username);
                }
                userComboBox.SelectedIndex = 0;
                existingPathTextBox.Text = m_loadedSettings.UserData[0].FileName;
                existingPathTextBox.SelectionStart = logRichTextBox.Text.Length;
                existingPathTextBox.ScrollToCaret();
            }
            else
            {
                groupContents.Visible = false;
            }
        }
    }
}
