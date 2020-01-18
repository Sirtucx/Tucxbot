namespace Twitch.IRC
{
    using System.Text.RegularExpressions;
    
    public class IRCHelper
    {
        private static readonly Regex NicknameRegex = new Regex(@"^[A-Za-z\[\]\\`_^{|}][A-Za-z0-9\[\]\\`_\-^{|}]+$", RegexOptions.Compiled);
        public static string MembershipRequest => "CAP REQ twitch.tv/membership";
        public static string CommandRequest => "CAP REQ twitch.tv/commands";
        public static string TagsRequest => "CAP REQ twitch.tv/tags";
        public static string PingRequest => "PING :tmi.twitch.tv";
        public static string Quit => "QUIT";

        public static bool IsValidNickname(string sNickname)
        {
            return (sNickname.Length > 0 && NicknameRegex.Match(sNickname).Success);
        }
        public static string GetPasswordSubmission(string sPassword)
        {
            return $"PASS {sPassword}";
        }
        public static string GetNicknameSubmission (string sNickname)
        {
            return $"NICK {sNickname}";
        }
        public static string GetUsernameSubmission(string sUsername, int iUserMode = 0)
        {
            return $"USER {sUsername} {iUserMode.ToString()} * :{sUsername}";
        }
        public static string GetChannelMessageCommand(string sChannel, string sMessage)
        {
            return $"PRIVMSG #{sChannel} :{sMessage}";
        }
        public static string GetWhisperCommand(string sUsername, string sMessage)
        {
            return $"PRIVMSG #jtv :/w {sUsername} {sMessage}";
        }
        public static string GetNoticeCommand(string sChannel, string sMessage)
        {
            return $"NOTICE #{sChannel} :{sMessage}";
        }
        public static string GetJoinChannelCommand(string sChannel)
        {
            return $"JOIN #{sChannel}";
        }
        public static string GetLeaveChannelCommand(string sChannel)
        {
            return $"PART #{sChannel}";
        }
        public static string GetKickCommand(string sChannel, string sUsername, string sKickMessage)
        {
            return $"KICK #{sChannel} {sUsername} :{sKickMessage}";
        }
    }
}
