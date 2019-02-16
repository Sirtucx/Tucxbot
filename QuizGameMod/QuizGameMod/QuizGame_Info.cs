using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizGameMod
{
    public class QuizGame_Info
    {
        public string Question;
        public string Answer;
        public int NumberOfWinners;
        public int GameType;

        public QuizGame_Info(string sQuestion, string sAnswer, int iNumberOfWinners, int iGameType)
        {
            Question = sQuestion;
            Answer = sAnswer;
            NumberOfWinners = iNumberOfWinners;
            GameType = iGameType;
        }
    }
}
