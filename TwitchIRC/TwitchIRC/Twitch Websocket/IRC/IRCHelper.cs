using System.Text.RegularExpressions;

namespace Twitch_Websocket
{
    public class IRCHelper
    {
        private static Regex m_NicknameRegex = new Regex(@"^[A-Za-z\[\]\\`_^{|}][A-Za-z0-9\[\]\\`_\-^{|}]+$", RegexOptions.Compiled);
        public static string Membership
        {
            get
            {
                return "CAP REQ twitch.tv/membership";
            }
        }
        public static string Command
        {
            get
            {
                return "CAP REQ twitch.tv/commands";
            }
        }
        public static string Tags
        {
            get
            {
                return "CAP REQ twitch.tv/tags";
            }
        }
        public static string Ping
        {
            get
            {
                return $"PING :tmi.twitch.tv";
            }
        }
        public static string Quit()
        {
            return "QUIT";
        }

        public static bool IsValidNickname(string sNickname)
        {
            return (sNickname.Length > 0 && m_NicknameRegex.Match(sNickname).Success);
        }
        public static string Pass(string sPassword)
        {
            return $"PASS {sPassword}";
        }
        public static string Nick (string sNickname)
        {
            return $"NICK {sNickname}";
        }
        public static string User(string sUsername, int iUserMode = 0)
        {
            return $"USER {sUsername} {iUserMode.ToString()} * :{sUsername}";
        }
        public static string Privmsg(string sChannel, string sMessage)
        {
            return $"PRIVMSG #{sChannel} :{sMessage}";
        }
        public static string Notice(string sChannel, string sMessage)
        {
            return $"NOTICE #{sChannel} :{sMessage}";
        }
        public static string Join(string sChannel)
        {
            return $"JOIN #{sChannel}";
        }
        public static string Part(string sChannel)
        {
            return $"PART #{sChannel}";
        }
        public static string Kick(string sChannel, string sUsername, string sKickMessage)
        {
            return $"KICK #{sChannel} {sUsername} :{sKickMessage}";
        }
    }
}
