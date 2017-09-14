using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch_Websocket
{
    public class OnBotJoinedChannelEventArgs : EventArgs
    {
        public string Channel { get; protected set; }
        public OnBotJoinedChannelEventArgs(string sChannel)
        {
            Channel = sChannel;
        }
    }
}
