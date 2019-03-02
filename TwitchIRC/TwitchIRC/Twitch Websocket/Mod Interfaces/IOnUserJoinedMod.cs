using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch_Websocket.Mod_Interfaces
{
    public interface IOnUserJoinedMod
    {
        void Process(UserState userState);
        void Shutdown();
    }
}
