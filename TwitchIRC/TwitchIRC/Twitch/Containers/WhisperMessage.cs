namespace Twitch.Containers
{
    using IRC;
    using Core;
    
    //Sample Whisper
    //@badges=premium/1;color=#FFFFFF;display-name=Sirtucx;emotes=438858:22-28;message-id=310;thread-id=53321443_103840464;turbo=1;user-id=53321443;user-type= :sirtucx!sirtucx@sirtucx.tmi.twitch.tv WHISPER tucxbot :How about now Tucxbot sirtucW
    public class WhisperMessage
    {
        public string Raw { get; protected set; }
        public BadgeCollection Badges { get; protected set; }
        public string ColorHex { get; protected set; }
        public string DisplayName { get; protected set; }
        public EmoteCollection Emotes { get; protected set; }
        public int ID { get; protected set; }
        public bool Turbo { get; protected set; }
        public int UserID { get; protected set; }
        public enum UserType { Moderator, GlobalModerator, Admin, Staff, Viewer }
        public UserType UsersType { get; protected set; }

        public string Username { get; protected set; }
        public string Message { get; protected set; }

        public WhisperMessage(string sIRCRaw)
        {
            // Raw IRC string
            Raw = sIRCRaw;

            // Badges
            Badges = new BadgeCollection(IRCParser.GetTwitchTagsValue(sIRCRaw, "@badges"));
            // Color
            ColorHex = IRCParser.GetTwitchTagsValue(sIRCRaw, "color");
            // Display Name
            DisplayName = IRCParser.GetTwitchTagsValue(sIRCRaw, "display-name").Replace(" ", "");
            // Emotes Used
            Emotes = new EmoteCollection(IRCParser.GetTwitchTagsValue(sIRCRaw, "emotes"));
            // Message ID
            ID = int.Parse(IRCParser.GetTwitchTagsValue(sIRCRaw, "id"));
            // Twitch Turbo/Prime Status
            Turbo = IRCParser.GetTwitchTagsValue(sIRCRaw, "turbo") == "1";
            // User ID
            UserID = int.Parse(IRCParser.GetTwitchTagsValue(sIRCRaw, "user-id"));
            // User Type
            string sUserTypeRaw = IRCParser.GetTwitchTagsValue(sIRCRaw, "user-type");
            switch (sUserTypeRaw)
            {
                case "mod":
                    {
                        UsersType = UserType.Moderator;
                        break;
                    }
                case "global_mod":
                    {
                        UsersType = UserType.GlobalModerator;
                        break;
                    }
                case "admin":
                    {
                        UsersType = UserType.Admin;
                        break;
                    }
                case "staff":
                    {
                        UsersType = UserType.Staff;
                        break;
                    }
                default:
                    {
                        UsersType = UserType.Viewer;
                        break;
                    }
            }

            string[] sUserTagSplit = sIRCRaw.Split('!');
            string[] sTmiSplit = sUserTagSplit[1].Split('@');
            Username = sTmiSplit[0];

            Message = sIRCRaw.Replace($"{sUserTagSplit[0]}!{Username}@{Username}.tmi.twitch.tv WHISPER {TwitchClient.GetInstance().Credentials.TwitchUsername.ToLower()} :", "");
        }
    }
}
