using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace QuizGameMod
{
    public class QuizContentLoader
    {
        public enum LoadType
        {
            None = -1,
            FileBased_JSON = 0,
            SQL = 1
        }

        public bool LoadedContent
        {
            get
            {
                return m_bLoadedContent;
            }
        }
        protected LoadType m_CurrentLoadType;
        protected QuizGameCollection m_QuizGameContent;
        protected DatabaseHandler m_DatabaseHandler;
        protected Dictionary<string, List<QuizGame_Info>> m_UsedQuestions;
        protected Dictionary<string, List<QuizGame_Info>> m_AllQuestions;
        protected bool m_bLoadedContent;
        protected Random m_RNG;

        public QuizContentLoader(LoadType loadType)
        {
            m_bLoadedContent = false;
            m_RNG = new Random();
            m_CurrentLoadType = LoadType.None;
            m_UsedQuestions = new Dictionary<string, List<QuizGame_Info>>();
            m_AllQuestions = new Dictionary<string, List<QuizGame_Info>>();
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
                        LoadSQL();
                        break;
                    }
            }
        }

        protected void LoadFileBased_JSON()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Load your Quiz Game JSON file.";
            fileDialog.InitialDirectory = Environment.CurrentDirectory;
            fileDialog.Filter = "JSON File (*.json)|*.json";
            fileDialog.FilterIndex = 0;
            bool bSelectedFile = false;

            while (!bSelectedFile)
            {
                DialogResult result = fileDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    try
                    {
                        using (StreamReader sr = new StreamReader(fileDialog.FileName))
                        {
                            if (sr.Peek() >= 0)
                            {
                                string rawData = sr.ReadToEnd();
                                m_QuizGameContent = JsonConvert.DeserializeObject<QuizGameCollection>(rawData);

                                if (m_QuizGameContent != null)
                                {
                                    bSelectedFile = true;
                                    m_bLoadedContent = true;
                                    m_CurrentLoadType = LoadType.FileBased_JSON;
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Interaction.MsgBox($"Unfortunately, there was an issue loading\n{fileDialog.FileName}. It was most likely not formatted properly. Please use a JSON parser to fix your quiz content file. A new JSON template file has been generated for you QuizGameContent_Template.json\nOnce you have done so please reload this mod and try again.");

                        using (StreamWriter sw = new StreamWriter("QuizGameContent_Template.json"))
                        {
                            sw.Write(JsonConvert.SerializeObject(new QuizGameCollection(), Formatting.Indented));
                        }
                        bSelectedFile = true;
                        m_bLoadedContent = false;
                    }
                }
                else if (result == DialogResult.Cancel)
                {
                    Interaction.MsgBox($"Please reload this mod to select a new file and try again.");
                    break;
                }
            }
        }

        protected void LoadSQL()
        {
            m_DatabaseHandler = new DatabaseHandler();

            if (m_DatabaseHandler.ConnectionReady)
            {
                m_bLoadedContent = true;
                m_CurrentLoadType = LoadType.SQL;
            }
            else
            {
                Interaction.MsgBox($"Please reload this mod to select a new file and try again.");
                m_bLoadedContent = false;
            }
        }

        public QuizGame_Info GetNewQuestion(string sChannel)
        {
            if (m_CurrentLoadType == LoadType.FileBased_JSON)
            {
                return GetFileBasedQuestion(sChannel);
            }
            else if (m_CurrentLoadType == LoadType.SQL)
            {
                return GetSQLBasedQuestion(sChannel);
            }
            else
            {
                return null;
            }
        }

        protected QuizGame_Info GetFileBasedQuestion(string sChannel)
        {
            List<QuizGame_Info> randomQuestionList = new List<QuizGame_Info>();
            QuizGame_Info randomQuestion = null;

            if (m_QuizGameContent.QuizGames.ContainsKey("default"))
            {
                randomQuestionList.AddRange(m_QuizGameContent.QuizGames["default"]);
            }

            if (m_QuizGameContent.QuizGames.ContainsKey(sChannel))
            {
                randomQuestionList.AddRange(m_QuizGameContent.QuizGames[sChannel]);
            }

            if (m_UsedQuestions.ContainsKey(sChannel))
            {
                if (m_UsedQuestions[sChannel].Count == randomQuestionList.Count)
                {
                    m_UsedQuestions[sChannel].Clear();
                }
            }
            else
            {
                m_UsedQuestions.Add(sChannel, new List<QuizGame_Info>());
            }

            while (randomQuestion == null)
            {
                int iRandomIndex = m_RNG.Next(0, randomQuestionList.Count);

                if (!m_UsedQuestions[sChannel].Contains(randomQuestionList[iRandomIndex]))
                {
                    randomQuestion = randomQuestionList[iRandomIndex];
                    m_UsedQuestions[sChannel].Add(randomQuestion);
                }
            }
            return randomQuestion;
        }

        protected QuizGame_Info GetSQLBasedQuestion(string sChannel)
        {
            if (!m_AllQuestions.ContainsKey(sChannel))
            {
                SqlDataReader channelOverrideData = m_DatabaseHandler.GetSelectData(new string[] { "*" }, "QuizCategoryOverride", $"WHERE channel like {sChannel.ToLower()}");

                if (channelOverrideData != null)
                {
                    string sCondition = "";
                    if (channelOverrideData.HasRows)
                    {
                        channelOverrideData.Read();
                        string categoryOverrides = channelOverrideData["category_list"].ToString();
                        string[] categories = categoryOverrides.Split(',');

                        sCondition = "WHERE ";

                        for (int i = 0; i < categories.Length; ++i)
                        {
                            sCondition += $"category like \'{categories[i]}\' {(i < categories.Length - 1 ? "or " : "")}";
                        }
                    }

                    SqlDataReader questionListData = m_DatabaseHandler.GetSelectData(new string[] { "*" }, "Questions", sCondition);

                    if (questionListData != null)
                    {
                        m_AllQuestions.Add(sChannel, new List<QuizGame_Info>());
                        while (questionListData.Read())
                        {
                            QuizGame_Info newQuestion = new QuizGame_Info(questionListData["question"].ToString(), questionListData["answer"].ToString(), (int)questionListData["winners"], 0);
                            m_AllQuestions[sChannel].Add(newQuestion);
                        }
                    }
                    else
                    {
                        // TODO: Something went wrong in the Question Query
                        throw new Exception($"QuizContentLoader.GetSQLBasedQuestion({sChannel}), Something went wrong in the Question Query");
                    }
                }
                else
                {
                    // TODO: Something went wrong in the Channel Override Query
                    throw new Exception($"QuizContentLoader.GetSQLBasedQuestion({sChannel}), Something went wrong in the Channel Override Query");
                }
            }

            QuizGame_Info question;
            do
            {
                question = m_AllQuestions[sChannel][m_RNG.Next(0, m_AllQuestions[sChannel].Count)];
            }
            while (m_UsedQuestions[sChannel].Contains(question));

            m_UsedQuestions[sChannel].Add(question);
            return question;
        }
    }
}
