namespace Twitch.Containers
{
    using System.Collections.Generic;
    
    public class EmoteCollection
    {
        private readonly Dictionary<Emote, int> EmotesUsed;

        public EmoteCollection(string emoteValueRaw)
        {
            if (!string.IsNullOrEmpty(emoteValueRaw))
            {
                string[] emoteSplit = emoteValueRaw.Split('/');

                foreach (string emoteValues in emoteSplit)
                {
                    string[] emoteKeyValue = emoteValues.Split(':');
                    Emote emote = new Emote(int.Parse(emoteKeyValue[0]));

                    string[] indexes = emoteKeyValue[1].Split(',');

                    if (EmotesUsed == null)
                    {
                        EmotesUsed = new Dictionary<Emote, int>();
                    }
                    EmotesUsed.Add(emote, indexes.Length);
                }
            }
        }

        public int HowManyTimesEmoteUsed(int emoteId)
        {
            if (EmotesUsed != null)
            {
                if (EmotesUsed.ContainsKey(new Emote(emoteId)))
                {
                    return EmotesUsed[new Emote(emoteId)];
                }
            }
            return 0;
        }
    }
}
