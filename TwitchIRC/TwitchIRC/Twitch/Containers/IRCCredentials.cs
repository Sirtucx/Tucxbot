namespace Twitch.Containers
{
    using IRC;
    
    public class IRCCredentials
    {
        public string TwitchUsername;
        public string TwitchOAuth;
        public string TwitchHost;
        public int TwitchPort;
        public bool Valid;

        public IRCCredentials (string sTwitchUsername, string sTwitchOAuth, string sTwitchHost = "irc-ws.chat.twitch.tv", int iTwitchPort = 80)
        {
            Valid = IRCHelper.IsValidNickname(sTwitchUsername);
            TwitchUsername = sTwitchUsername.ToLower();
            if (!sTwitchOAuth.Contains(":"))
            {
                sTwitchOAuth = $"oauth:{sTwitchOAuth.Replace("oauth", "")}";
            }

            TwitchOAuth = sTwitchOAuth;
            TwitchHost = sTwitchHost;
            TwitchPort = iTwitchPort;
        }
    }
}
