using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch.Containers
{
    public class LoginData
    {
        public string Username;
        public string OAuth;

        public LoginData(string sUsername, string sOAuth)
        {
            Username = sUsername;
            OAuth = sOAuth;
        }
    }
}
