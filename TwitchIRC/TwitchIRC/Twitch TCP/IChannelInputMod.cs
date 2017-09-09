using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchIRC_TCP
{
    public interface IChannelInputMod
    {
        void ProcessMessage(string sChannel, string sUsername, string sMessage);
        void Shutdown();
    }
}
