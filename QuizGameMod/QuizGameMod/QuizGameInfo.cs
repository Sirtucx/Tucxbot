namespace QuizGameMod
{
    public class QuizGameInfo
    {
        public readonly string Question;
        public readonly string Answer;
        public readonly int NumberOfWinners;
        private int m_gameType;

        public QuizGameInfo(string question, string answer, int numberOfWinners, int gameType)
        {
            Question = question;
            Answer = answer;
            NumberOfWinners = numberOfWinners;
            m_gameType = gameType;
        }
    }
}
