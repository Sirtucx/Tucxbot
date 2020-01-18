namespace Twitch.Containers
{
    using IRC;
    using Core;
    
    //Sample Whisper
    //@badges=premium/1;color=#FFFFFF;display-name=Sirtucx;emotes=438858:22-28;message-id=310;thread-id=53321443_103840464;turbo=1;user-id=53321443;user-type= :sirtucx!sirtucx@sirtucx.tmi.twitch.tv WHISPER tucxbot :How about now Tucxbot sirtucW
    public class WhisperMessage
    {
        public string RawMessage { get; }
        public BadgeCollection Badges { get; }
        public string ColorHex { get; }
        public string DisplayName { get; }
        public EmoteCollection Emotes { get; }
        public int ID { get; }
        public bool Turbo { get; }
        public int UserID { get; }
        public enum UserType { Moderator, GlobalModerator, Admin, Staff, Viewer }
        public UserType UsersType { get; }

        public string Username { get; }
        public string Message { get; }

        public WhisperMessage(string ircRawMessage)
        {
            // RawMessage IRC string
            RawMessage = ircRawMessage;

            // Badges
            Badges = new BadgeCollection(IRCParser.GetTwitchTagsValue(ircRawMessage, "@badges"));
            // Color
            ColorHex = IRCParser.GetTwitchTagsValue(ircRawMessage, "color");
            // Display Name
            DisplayName = IRCParser.GetTwitchTagsValue(ircRawMessage, "display-name").Replace(" ", "");
            // Emotes Used
            Emotes = new EmoteCollection(IRCParser.GetTwitchTagsValue(ircRawMessage, "emotes"));
            // Message ID
            ID = int.Parse(IRCParser.GetTwitchTagsValue(ircRawMessage, "id"));
            // Twitch Turbo/Prime Status
            Turbo = IRCParser.GetTwitchTagsValue(ircRawMessage, "turbo") == "1";
            // User ID
            UserID = int.Parse(IRCParser.GetTwitchTagsValue(ircRawMessage, "user-id"));
            // User Type
            string userTypeRaw = IRCParser.GetTwitchTagsValue(ircRawMessage, "user-type");
            switch (userTypeRaw)
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

            string[] userTagSplit = ircRawMessage.Split('!');
            string[] tmiSplit = userTagSplit[1].Split('@');
            Username = tmiSplit[0];

            Message = ircRawMessage.Replace($"{userTagSplit[0]}!{Username}@{Username}.tmi.twitch.tv WHISPER {TwitchClient.GetInstance().Credentials.TwitchUsername.ToLower()} :", "");
        }
    }
}
