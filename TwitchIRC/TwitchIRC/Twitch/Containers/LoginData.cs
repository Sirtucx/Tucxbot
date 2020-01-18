using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch.Containers
{
    public class LoginData
    {
        public readonly string Username;
        public readonly string OAuth;

        public LoginData(string username, string oAuth)
        {
            Username = username;
            OAuth = oAuth;
        }
    }
}
