using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;

namespace QuizGameMod
{
    public class QuizContentLoader
    {
        public enum LoadType
        {
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

        protected QuizGameCollection m_QuizGameContent;
        protected Dictionary<string, List<QuizGame_Info>> m_UsedQuestions;
        protected bool m_bLoadedContent;
        protected Random m_RNG;

        public QuizContentLoader(LoadType loadType)
        {
            m_bLoadedContent = false;
            m_RNG = new Random();
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
            Interaction.MsgBox($"Unfortunately, this part of the mod is not developed yet. If you would like to use the JSON version please reload the mod and type 0 as an option to load.");
            m_bLoadedContent = false;
        }

        protected QuizGame_Info GetNewQuestion(string sChannel)
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
    }
}
