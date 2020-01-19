namespace QuizGameMod
{
    using System.Collections.Generic;
    
    public class QuizGame
    {
       private QuizGameInfo m_currentGameContent;
       private int m_numberOfWinners;
       private int m_currentCooldown;
       private int m_hintCooldown;
        
       private readonly List<string> m_currentWinners;

       public string Answer => m_currentGameContent.Answer;
       public string Instructions => m_currentGameContent?.Question;
       public List<string> Winners => m_currentWinners;

        public QuizGame ()
        {
            m_currentGameContent = null;
            m_currentWinners = new List<string>();
        }

        public void StartNewGame(QuizGameInfo quizGameInfo)
        {
            if (m_currentGameContent != null)
            {
                return;
            }

            m_currentGameContent = quizGameInfo;
            m_numberOfWinners = quizGameInfo.NumberOfWinners;
            m_currentWinners.Clear();
        }

        public bool CheckAnswer(string userAnswer)
        {
            return userAnswer.ToLower() == m_currentGameContent?.Answer.ToLower();
        }

        public void EndGame()
        {
            m_currentGameContent = null;
        }

        public bool ReadyForNewGame()
        {
            return m_currentCooldown <= 0;
        }

        public bool ReadyForHint()
        {
            return m_hintCooldown <= 0;
        }

        public void Tick(int tickRate)
        {
            if (m_currentGameContent == null)
            {
                m_currentCooldown -= tickRate;
            }
            else
            {
                m_hintCooldown -= tickRate;
            }
        }

        public void SetGameCooldown(int cooldown)
        {
            m_currentCooldown = cooldown;
        }

        public void SetHintCooldown(int cooldown)
        {
            m_hintCooldown = cooldown;
        }

        public void AddWinner(string username)
        {
            if (!m_currentWinners.Contains(username))
            {
                m_currentWinners.Add(username);
            }
        }

        public bool IsQualifiedWinner()
        {
            return m_currentWinners.Count < m_numberOfWinners;
        }

        public virtual string GetHint()
        {
            return m_currentGameContent?.Answer;
        }
    }
}
