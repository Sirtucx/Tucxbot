using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch_Websocket
{
    public class OnUserJoinedEventArgs : EventArgs
    {
        public UserState UserState { get; protected set; }

        public OnUserJoinedEventArgs(UserState userState)
        {
            UserState = userState;
        }
    }
}
