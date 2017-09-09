using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch_Websocket
{
    public class Emote
    {
        public int ID { get; protected set; }
        private string BaseURL = "http://static-cdn.jtvnw.net/emoticons/v1/<emote ID>/<size>";

        public Emote(int id)
        {
            ID = id;
        }
    }
}
