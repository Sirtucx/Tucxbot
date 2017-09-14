using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch_Websocket
{
    public class OnUserLeaveEventArgs : EventArgs
    {
        public string Username { get; protected set; }
        public string Channel { get; protected set; }
        public OnUserLeaveEventArgs(string sUsername, string sChannel)
        {
            Username = sUsername;
            Channel = sChannel;
        }
    }
}
