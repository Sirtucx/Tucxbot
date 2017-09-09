using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch_Websocket
{
    public class EmoteCollection
    {
        private Dictionary<Emote, int> EmotesUsed;

        public EmoteCollection(string sEmoteValueRaw)
        {
            if (!string.IsNullOrEmpty(sEmoteValueRaw))
            {
                string[] sEmoteSplit = sEmoteValueRaw.Split('/');

                foreach (string emoteValues in sEmoteSplit)
                {
                    string[] sEmoteKeyValue = emoteValues.Split(':');
                    Emote emote = new Emote(int.Parse(sEmoteKeyValue[0]));

                    string[] sIndexes = sEmoteKeyValue[1].Split(',');

                    if (EmotesUsed == null)
                    {
                        EmotesUsed = new Dictionary<Emote, int>();
                    }
                    EmotesUsed.Add(emote, sIndexes.Length);
                }
            }
        }

        public int HowManyTimesEmoteUsed(int EmoteID)
        {
            if (EmotesUsed != null)
            {
                if (EmotesUsed.ContainsKey(new Emote(EmoteID)))
                {
                    return EmotesUsed[new Emote(EmoteID)];
                }
            }
            return 0;
        }
    }
}
