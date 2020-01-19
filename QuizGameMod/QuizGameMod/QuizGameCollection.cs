namespace QuizGameMod
{
    using System.Collections.Generic;
    
    public class QuizGameCollection
    {
        public readonly Dictionary<string, List<QuizGameInfo>> QuizGames;

        public QuizGameCollection()
        {
            QuizGames = new Dictionary<string, List<QuizGameInfo>>
            {
                {"default", new List<QuizGameInfo>()}, {"YourChannelHere", new List<QuizGameInfo>()}
            };
            QuizGames["default"].Add(new QuizGameInfo("Here is a question for all channels", "This is the answer", 1, 0));
            QuizGames["YourChannelHere"].Add(new QuizGameInfo("Here is your channel specific question here", "This is the channel answer", 1, 0));
        }
    }
}
