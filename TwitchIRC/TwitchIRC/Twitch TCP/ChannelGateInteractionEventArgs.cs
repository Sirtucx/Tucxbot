using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchIRC_TCP
{
    public class ChannelGateInteractionEventArgs : EventArgs
    {
        public string Target;
        public string Username;
        public bool Joining;
    }
}
