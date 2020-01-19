namespace QuizGameMod
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualBasic;
    using System.Windows.Forms;
    using System.IO;
    using Newtonsoft.Json;
    using System.Data.SqlClient;
    
    public class QuizContentLoader
    {
        public enum LoadType
        {
            None = -1,
            FileBased_JSON = 0,
            SQL = 1
        }

        private LoadType m_currentLoadType;
        private QuizGameCollection m_quizGameContent;
        private DatabaseHandler m_databaseHandler;
        private Dictionary<string, List<QuizGameInfo>> m_usedQuestions;
        private Dictionary<string, List<QuizGameInfo>> m_allQuestions;
        private bool m_loadedContent;
        private Random m_rng;
        
        public bool LoadedContent => m_loadedContent;

        public QuizContentLoader(LoadType loadType)
        {
            m_loadedContent = false;
            m_rng = new Random();
            m_currentLoadType = LoadType.None;
            m_usedQuestions = new Dictionary<string, List<QuizGameInfo>>();
            m_allQuestions = new Dictionary<string, List<QuizGameInfo>>();
            HandleLoadType(loadType);
        }

        protected void HandleLoadType(LoadType loadType)
        {
            switch (loadType)
            {
                case LoadType.FileBased_JSON:
                    {
                        LoadFileBased_JSON();
                        break;
                    }
                case LoadType.SQL:
                    {
                        LoadSql();
                        break;
                    }
            }
        }

        private void LoadFileBased_JSON()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Load your Quiz Game JSON file.";
            fileDialog.InitialDirectory = Environment.CurrentDirectory;
            fileDialog.Filter = "JSON File (*.json)|*.json";
            fileDialog.FilterIndex = 0;
            bool selectedValidFile = false;

            while (!selectedValidFile)
            {
                DialogResult result = fileDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    selectedValidFile = ValidateJsonFile(fileDialog.FileName);
                }
                else if (result == DialogResult.Cancel)
                {
                    Interaction.MsgBox($"Please reload this mod to select a new file and try again.");
                    break;
                }
            }
        }

        private bool ValidateJsonFile(string fileName)
        {
            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    if (sr.Peek() >= 0)
                    {
                        string rawData = sr.ReadToEnd();
                        m_quizGameContent = JsonConvert.DeserializeObject<QuizGameCollection>(rawData);

                        if (m_quizGameContent != null)
                        {
                            m_loadedContent = true;
                            m_currentLoadType = LoadType.FileBased_JSON;
                            return true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Interaction.MsgBox($"Unfortunately, there was an issue loading\n{fileName}. It was most likely not formatted properly. Please use a JSON parser to fix your quiz content file. A new JSON template file has been generated for you QuizGameContent_Template.json\nOnce you have done so please reload this mod and try again.");

                using (StreamWriter sw = new StreamWriter("QuizGameContent_Template.json"))
                {
                    sw.Write(JsonConvert.SerializeObject(new QuizGameCollection(), Formatting.Indented));
                }
                m_loadedContent = false;
                return true;
            }

            return false;
        }

        private void LoadSql()
        {
            m_databaseHandler = new DatabaseHandler();

            if (m_databaseHandler.ConnectionReady)
            {
                m_loadedContent = true;
                m_currentLoadType = LoadType.SQL;
            }
            else
            {
                Interaction.MsgBox($"Please reload this mod to select a new file and try again.");
                m_loadedContent = false;
            }
        }

        public QuizGameInfo GetNewQuestion(string channelName)
        {
            switch (m_currentLoadType)
            {
                case LoadType.FileBased_JSON:
                {
                    return GetFileBasedQuestion(channelName);
                }
                case LoadType.SQL:
                {
                    return GetSqlBasedQuestion(channelName);
                }
                default:
                {
                    return null;
                }
            }
        }

        private QuizGameInfo GetFileBasedQuestion(string channelName)
        {
            List<QuizGameInfo> randomQuestionList = new List<QuizGameInfo>();
            QuizGameInfo randomQuestion = null;

            if (m_quizGameContent.QuizGames.ContainsKey("default"))
            {
                randomQuestionList.AddRange(m_quizGameContent.QuizGames["default"]);
            }

            if (m_quizGameContent.QuizGames.ContainsKey(channelName))
            {
                randomQuestionList.AddRange(m_quizGameContent.QuizGames[channelName]);
            }

            if (m_usedQuestions.ContainsKey(channelName))
            {
                if (m_usedQuestions[channelName].Count == randomQuestionList.Count)
                {
                    m_usedQuestions[channelName].Clear();
                }
            }
            else
            {
                m_usedQuestions.Add(channelName, new List<QuizGameInfo>());
            }

            while (randomQuestion == null)
            {
                int iRandomIndex = m_rng.Next(0, randomQuestionList.Count);

                if (!m_usedQuestions[channelName].Contains(randomQuestionList[iRandomIndex]))
                {
                    randomQuestion = randomQuestionList[iRandomIndex];
                    m_usedQuestions[channelName].Add(randomQuestion);
                }
            }
            return randomQuestion;
        }

        private QuizGameInfo GetSqlBasedQuestion(string channelName)
        {
            if (!m_allQuestions.ContainsKey(channelName))
            {
                SqlDataReader channelOverrideData = m_databaseHandler.GetSelectData(new string[] { "*" }, "QuizCategoryOverride", $"WHERE channel like {channelName.ToLower()}");

                if (channelOverrideData != null)
                {
                    string condition = "";
                    if (channelOverrideData.HasRows)
                    {
                        channelOverrideData.Read();
                        string categoryOverrides = channelOverrideData["category_list"].ToString();
                        string[] categories = categoryOverrides.Split(',');

                        condition = "WHERE ";

                        for (int i = 0; i < categories.Length; ++i)
                        {
                            condition += $"category like \'{categories[i]}\' {(i < categories.Length - 1 ? "or " : "")}";
                        }
                    }

                    SqlDataReader questionListData = m_databaseHandler.GetSelectData(new string[] { "*" }, "Questions", condition);

                    if (questionListData != null)
                    {
                        m_allQuestions.Add(channelName, new List<QuizGameInfo>());
                        while (questionListData.Read())
                        {
                            QuizGameInfo newQuestion = new QuizGameInfo(questionListData["question"].ToString(), questionListData["answer"].ToString(), (int)questionListData["winners"], 0);
                            m_allQuestions[channelName].Add(newQuestion);
                        }
                    }
                    else
                    {
                        // TODO: Something went wrong in the Question Query
                        throw new Exception($"QuizContentLoader.GetSqlBasedQuestion({channelName}), Something went wrong in the Question Query");
                    }
                }
                else
                {
                    // TODO: Something went wrong in the Channel Override Query
                    throw new Exception($"QuizContentLoader.GetSqlBasedQuestion({channelName}), Something went wrong in the Channel Override Query");
                }
            }

            QuizGameInfo question;
            do
            {
                question = m_allQuestions[channelName][m_rng.Next(0, m_allQuestions[channelName].Count)];
            }
            while (m_usedQuestions[channelName].Contains(question));

            m_usedQuestions[channelName].Add(question);
            return question;
        }
    }
}
