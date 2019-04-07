using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;
using Microsoft.VisualBasic;
using System.Windows.Forms;

namespace QuizGameMod
{
    public class DatabaseHandler
    {
        protected string m_sConnectionString;
        public bool ConnectionReady { get; protected set; }

        public DatabaseHandler()
        {
            ConnectionReady = false;
            LoadConnectionData();
        }

        protected void CreateSaveData()
        {
            string sConnectionString = Interaction.InputBox("Please enter your SQL connection string.", DefaultResponse: "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password = myPassword;");

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "txt files (*.txt)|*.txt|*.txt";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.InitialDirectory = Environment.CurrentDirectory;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter writer = new StreamWriter(saveFileDialog.OpenFile()))
                {
                    writer.WriteLine($"Connection String:{sConnectionString}");
                    m_sConnectionString = sConnectionString;
                    ConnectionReady = true;
                }
            }
        }

        protected void LoadSaveData()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "txt files (*.txt)|*.txt|*.txt";
            openFileDialog.FilterIndex = 0;
            openFileDialog.InitialDirectory = Environment.CurrentDirectory;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader reader = new StreamReader(openFileDialog.OpenFile()))
                {
                    string sRawData = reader.ReadLine();
                    if (sRawData != null)
                    {
                        string[] sData = sRawData.Split(':');

                        if (sData.Length == 2)
                        {
                            m_sConnectionString = sData[1];
                            ConnectionReady = true;
                        }
                    }
                }
            }
        }

        protected void LoadConnectionData()
        {
            DialogResult loadFilePreparation = MessageBox.Show("Would you like to create a new file to store your SQL connection string?", "Create SQL Save File", MessageBoxButtons.YesNo);

            if (loadFilePreparation == DialogResult.Yes)
            {
                CreateSaveData();
            }
            else if (loadFilePreparation == DialogResult.No)
            {
                LoadSaveData();
            }
        }

        public SqlDataReader GetAllData(string sTableName)
        {
            return GetSelectData(new string[] { "*" }, sTableName, string.Empty);
        }
        public SqlDataReader GetSelectData(string[] sDataNames, string sTableName, string sConditions)
        {
            SqlDataReader reader = null;
            if (ConnectionReady)
            {
                using (SqlConnection connection = new SqlConnection(m_sConnectionString))
                {
                    string sCommand = $"SELECT ";
                    if (sDataNames != null)
                    {
                        for (int i = 0; i < sDataNames.Length; ++i)
                        {
                            sCommand += sDataNames[i] + (i < sDataNames.Length - 1 ? ", " : "");
                        }
                    }

                    sConditions += $"FROM {sTableName}";

                    if (!string.IsNullOrEmpty(sConditions))
                    {
                        if (!sConditions.ToUpper().Contains("WHERE"))
                        {
                            sConditions = $" WHERE {sConditions}";
                        }
                    }

                    sCommand += sConditions;
                    using (SqlCommand command = new SqlCommand(sCommand, connection))
                    {
                        try
                        {
                            connection.Open();
                            reader = command.ExecuteReader();
                        }
                        catch (Exception e)
                        {
                            // TODO: Write to a log file.
                            return null;
                        }
                    }
                }
            }
            return reader;
        }
        protected void RunStoredProceedure(string sStoredProceedureName, Dictionary<string, object> parameters = null)
        {
            SqlDataReader temp = null;
            RunStoredProceedure(sStoredProceedureName, out temp, parameters);
        }
        protected void RunStoredProceedure(string sStoredProceedureName, out SqlDataReader sqlData, Dictionary<string, object> parameters = null)
        {
            if (ConnectionReady)
            {
                using (SqlConnection connection = new SqlConnection(m_sConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(sStoredProceedureName, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        foreach (KeyValuePair<string, object> kvp in parameters)
                        {
                            command.Parameters.Add(new SqlParameter($"@{kvp.Key}", kvp.Value));
                        }

                        try
                        {
                            connection.Open();
                            sqlData = command.ExecuteReader();
                        }
                        catch(Exception e)
                        {
                            sqlData = null;
                            // TODO: Write to a log file.
                        }
                    }
                }
            }
            else
            {
                sqlData = null;
            }
        }
    }
}
