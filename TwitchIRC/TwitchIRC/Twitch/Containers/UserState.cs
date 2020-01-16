namespace Twitch.Containers
{
    using IRC;
    
    public class UserState
    {
        public string Raw { get; protected set; }
        public string Channel { get; protected set; }

        public BadgeCollection Badges { get; protected set; }
        public string ColorHex { get; protected set; }
        public string DisplayName { get; protected set; }
        public bool Mod { get; protected set; }

        public UserState(string sIRCRaw)
        {
            // Raw IRC string
            Raw = sIRCRaw;
            // Channel notice was sent in
            Channel = sIRCRaw.Substring(sIRCRaw.IndexOf('#', sIRCRaw.IndexOf("PRIVMSG")) + 1, sIRCRaw.IndexOf(' ', sIRCRaw.IndexOf('#', sIRCRaw.IndexOf("PRIVMSG")) + 1) - (sIRCRaw.IndexOf('#', sIRCRaw.IndexOf("PRIVMSG")) + 1));
            // Badges
            Badges = new BadgeCollection(IRCParser.GetTwitchTagsValue(sIRCRaw, "@badges"));
            // Color
            ColorHex = IRCParser.GetTwitchTagsValue(sIRCRaw, "color");
            // Display Name
            DisplayName = IRCParser.GetTwitchTagsValue(sIRCRaw, "display-name").Replace(" ", "");
            // Mod Status
            Mod = IRCParser.GetTwitchTagsValue(sIRCRaw, "mod") == "1";
        }
    }
}
