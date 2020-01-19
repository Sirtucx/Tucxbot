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
        private string m_connectionString;
        public bool ConnectionReady { get; private set; }

        public DatabaseHandler()
        {
            ConnectionReady = false;
            LoadConnectionData();
        }

        private void CreateSaveData()
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
                    m_connectionString = sConnectionString;
                    ConnectionReady = true;
                }
            }
        }

        private void LoadSaveData()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "txt files (*.txt)|*.txt|*.txt";
            openFileDialog.FilterIndex = 0;
            openFileDialog.InitialDirectory = Environment.CurrentDirectory;

            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            
            using (StreamReader reader = new StreamReader(openFileDialog.OpenFile()))
            {
                string sRawData = reader.ReadLine();
                
                if (sRawData == null)
                {
                    return;
                }
                
                string[] sData = sRawData.Split(':');

                if (sData.Length != 2)
                {
                    return;
                }
                
                m_connectionString = sData[1];
                ConnectionReady = true;
            }
        }

        private void LoadConnectionData()
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

        public SqlDataReader GetAllData(string tableName)
        {
            return GetSelectData(new string[] { "*" }, tableName, string.Empty);
        }
        public SqlDataReader GetSelectData(string[] dataNames, string tableName, string conditions)
        {
            SqlDataReader reader;
            if (!ConnectionReady)
            {
                return null;
            }
            
            using (SqlConnection connection = new SqlConnection(m_connectionString))
            {
                string sCommand = $"SELECT ";
                if (dataNames != null)
                {
                    for (int i = 0; i < dataNames.Length; ++i)
                    {
                        sCommand += dataNames[i] + (i < dataNames.Length - 1 ? ", " : "");
                    }
                }

                conditions += $"FROM {tableName}";

                if (!string.IsNullOrEmpty(conditions))
                {
                    if (!conditions.ToUpper().Contains("WHERE"))
                    {
                        conditions = $" WHERE {conditions}";
                    }
                }

                sCommand += conditions;
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
            return reader;
        }
        private void RunStoredProcedure(string storedProcedureName, Dictionary<string, object> parameters = null)
        {
            SqlDataReader temp = null;
            RunStoredProcedure(storedProcedureName, out temp, parameters);
        }
        private void RunStoredProcedure(string storedProcedureName, out SqlDataReader sqlData, Dictionary<string, object> parameters = null)
        {
            if (ConnectionReady)
            {
                using (SqlConnection connection = new SqlConnection(m_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        if (parameters != null)
                        {

                            foreach (KeyValuePair<string, object> kvp in parameters)
                            {
                                command.Parameters.Add(new SqlParameter($"@{kvp.Key}", kvp.Value));
                            }
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
