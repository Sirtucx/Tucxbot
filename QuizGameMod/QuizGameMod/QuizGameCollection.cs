using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizGameMod
{
    public class QuizGameCollection
    {
        public Dictionary<string, List<QuizGame_Info>> QuizGames;

        public QuizGameCollection()
        {
            QuizGames = new Dictionary<string, List<QuizGame_Info>>();
            QuizGames.Add("default", new List<QuizGame_Info>());
            QuizGames.Add("YourChannelHere", new List<QuizGame_Info>());
            QuizGames["default"].Add(new QuizGame_Info("Here is a question for all channels", "This is the answer", 1, 0));
            QuizGames["YourChannelHere"].Add(new QuizGame_Info("Here is your channel specific question here", "This is the channel answer", 1, 0));
        }
    }
}
