namespace Twitch.Containers
{
    using IRC;
    
    public class UserState
    {
        public string RawMessage { get; }
        public string Channel { get; }
        public BadgeCollection Badges { get; }
        public string ColorHex { get; }
        public string DisplayName { get; }
        public bool Mod { get; }

        public UserState(string ircRawMessage)
        {
            // RawMessage IRC string
            RawMessage = ircRawMessage;
            // Channel notice was sent in
            int startIndex = ircRawMessage.IndexOf('#') + 1;
            Channel = ircRawMessage.Substring(startIndex);
            // Badges
            Badges = new BadgeCollection(IRCParser.GetTwitchTagsValue(ircRawMessage, "@badges"));
            // Color
            ColorHex = IRCParser.GetTwitchTagsValue(ircRawMessage, "color");
            // Display Name
            DisplayName = IRCParser.GetTwitchTagsValue(ircRawMessage, "display-name").Replace(" ", "");
            // Mod Status
            Mod = IRCParser.GetTwitchTagsValue(ircRawMessage, "mod") == "1";
        }
    }
}
