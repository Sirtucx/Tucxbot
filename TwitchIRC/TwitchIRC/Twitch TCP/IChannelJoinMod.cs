using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchIRC_TCP
{
    public interface IChannelJoinMod
    {
        void ProcessJoin(string sChannel);
        void Shutdown();
    }
}
