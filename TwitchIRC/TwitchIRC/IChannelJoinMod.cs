using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchIRC
{
    public interface IChannelJoinMod
    {
        void ProcessJoin(string sChannel);
    }
}
