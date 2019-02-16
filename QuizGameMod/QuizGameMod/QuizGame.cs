using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twitch_Websocket;

namespace QuizGameMod
{
    public class QuizGame
    {
        public string Answer
        {
            get
            {
                return m_CurrentGameContent.Answer;
            }
        }
        public string Instructions
        {
            get
            {
                return m_CurrentGameContent?.Question;
            }
        }
        public List<string> Winners
        {
            get
            {
                return m_CurrentWinners;
            }
        }

        protected QuizGame_Info m_CurrentGameContent;
        protected List<string> m_CurrentWinners;
        protected int m_iNumberOfWinners;
        protected int m_iCurrentCooldown;
        protected int m_iHintCooldown;

        public QuizGame ()
        {
            m_CurrentGameContent = null;
            m_CurrentWinners = new List<string>();
        }

        public void StartNewGame(QuizGame_Info quizGame_Info)
        {
            if (m_CurrentGameContent == null)
            {
                m_CurrentGameContent = quizGame_Info;
                m_iNumberOfWinners = quizGame_Info.NumberOfWinners;
                m_CurrentWinners.Clear();
            }
        }

        public bool CheckAnswer(string userAnswer)
        {
            if (m_CurrentGameContent != null)
            {
                if (userAnswer.ToLower() == m_CurrentGameContent?.Answer.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        public void EndGame()
        {
            m_CurrentGameContent = null;
        }

        public bool ReadyForNewGame()
        {
            return m_iCurrentCooldown <= 0;
        }

        public bool ReadyForHint()
        {
            return m_iHintCooldown <= 0;
        }

        public void Tick(int tickRate)
        {
            if (m_CurrentGameContent == null)
            {
                m_iCurrentCooldown -= tickRate;
            }
            else
            {
                m_iHintCooldown -= tickRate;
            }
        }

        public void SetGameCooldown(int cooldown)
        {
            m_iCurrentCooldown = cooldown;
        }

        public void SetHintCooldown(int cooldown)
        {
            m_iHintCooldown = cooldown;
        }

        public void AddWinner(string username)
        {
            if (!m_CurrentWinners.Contains(username))
            {
                m_CurrentWinners.Add(username);
            }
        }

        public bool IsQualifiedWinner()
        {
            return m_CurrentWinners.Count < m_iNumberOfWinners;
        }

        public virtual string GetHint()
        {
            return m_CurrentGameContent?.Answer;
        }
    }
}
