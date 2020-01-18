namespace Twitch.Containers
{
    using IRC;
    
    public class IrcCredentials
    {
        public readonly string TwitchUsername;
        public readonly string TwitchOAuth;
        public readonly string TwitchHost;
        public readonly int TwitchPort;
        public readonly bool Valid;

        public IrcCredentials (string twitchUsername, string twitchOAuth, string twitchHost = "irc-ws.chat.twitch.tv", int twitchPort = 80)
        {
            Valid = IRCHelper.IsValidNickname(twitchUsername);
            TwitchUsername = twitchUsername.ToLower();
            if (!twitchOAuth.Contains(":"))
            {
                twitchOAuth = $"oauth:{twitchOAuth.Replace("oauth", "")}";
            }

            TwitchOAuth = twitchOAuth;
            TwitchHost = twitchHost;
            TwitchPort = twitchPort;
        }
    }
}
