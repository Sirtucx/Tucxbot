using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch_Websocket
{
    public class OnSubscriberEventArgs : EventArgs
    {
        public UserNotice UserNotice { get; protected set; }

        public OnSubscriberEventArgs(UserNotice usernotice)
        {
            UserNotice = usernotice;
        }
    }
}
