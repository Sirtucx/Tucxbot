namespace Twitch.Containers
{
    using System;
    using IRC;
    
    public class ChatMessage
    {
        public enum UserType
        {
            Moderator,
            GlobalModerator,
            Admin,
            Staff,
            Viewer
        }

        public string RawMessage { get; }
        public string Channel { get; }
        public BadgeCollection Badges { get; }
        public int Bits { get; }
        public string ColorHex { get; }
        public string DisplayName { get; }
        public EmoteCollection Emotes { get; }
        public string Id { get; }
        public string Message { get; }
        public bool Mod { get; }
        public int ChannelId { get; }
        public bool Subscriber { get; }
        public bool Turbo { get; }
        public string Username { get; }
        public int UserId { get; }
        public UserType UsersType { get; }

        public ChatMessage(string ircRawMessage)
        {
            // RawMessage IRC string
            RawMessage = ircRawMessage;
            // Channel notice was sent in
            Channel = ircRawMessage.Substring(ircRawMessage.IndexOf('#', ircRawMessage.IndexOf("PRIVMSG")) + 1, ircRawMessage.IndexOf(' ', ircRawMessage.IndexOf('#', ircRawMessage.IndexOf("PRIVMSG")) + 1) - (ircRawMessage.IndexOf('#', ircRawMessage.IndexOf("PRIVMSG")) + 1));
            // Bits (Total)
            int numberOfBits = 0;
            Int32.TryParse(IRCParser.GetTwitchTagsValue(ircRawMessage, "bits"), out numberOfBits);
            Bits = numberOfBits;
            // Badges
            Badges = new BadgeCollection(IRCParser.GetTwitchTagsValue(ircRawMessage, "@badges"));
            // Color
            ColorHex = IRCParser.GetTwitchTagsValue(ircRawMessage, "color");
            // Display Name
            DisplayName = IRCParser.GetTwitchTagsValue(ircRawMessage, "display-name").Replace(" ", "");
            // Emotes Used
            Emotes = new EmoteCollection(IRCParser.GetTwitchTagsValue(ircRawMessage, "emotes"));
            // Message
            string[] messageSplit = ircRawMessage.Split(new string[] { $"#{Channel} :" }, System.StringSplitOptions.None);
            Message = messageSplit[1];
            // Mod Status
            Mod = IRCParser.GetTwitchTagsValue(ircRawMessage, "mod") == "1";
            // Channel ID (Room ID)
            ChannelId = int.Parse(IRCParser.GetTwitchTagsValue(ircRawMessage, "room-id"));
            // Subscriber Status
            Subscriber = IRCParser.GetTwitchTagsValue(ircRawMessage, "subscriber") == "1";
            // Username
            string[] tagSplit = ircRawMessage.Split(' ');
            Username = tagSplit[1].Substring(tagSplit[1].IndexOf('!') + 1, (tagSplit[1].IndexOf('@')) - (tagSplit[1].IndexOf('!') + 1));
            // Twitch Turbo/Prime Status
            Turbo = IRCParser.GetTwitchTagsValue(ircRawMessage, "turbo") == "1";
            // User ID
            UserId = int.Parse(IRCParser.GetTwitchTagsValue(ircRawMessage, "user-id"));
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
        }
    }
}
