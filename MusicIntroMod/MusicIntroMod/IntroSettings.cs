using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicIntroMod
{
    public class IntroSettings
    {
        public List<User> UserData;
        public IntroSettings()
        {
            UserData = new List<User>();
        }
    }
    public class User
    {
        public string Username;
        public string FileName;

        public User (string sUsername, string sFileName)
        {
            Username = sUsername;
            FileName = sFileName;
        }
    }
}
