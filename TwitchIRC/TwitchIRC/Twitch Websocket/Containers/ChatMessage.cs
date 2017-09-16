using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch_Websocket
{
    public class ChatMessage
    {
        public string Raw { get; protected set; }
        public string Channel { get; protected set; }

        public BadgeCollection Badges { get; protected set; }
        public int Bits { get; protected set; }
        public string ColorHex { get; protected set; }
        public string DisplayName { get; protected set; }
        public EmoteCollection Emotes { get; protected set; }
        public string ID { get; protected set; }
        public string Message { get; protected set; }
        public bool Mod { get; protected set; }
        public int ChannelID { get; protected set; }
        public bool Subscriber { get; protected set; }
        public bool Turbo { get; protected set; }
        public string Username { get; protected set; }
        public int UserID { get; protected set; }
        public enum UserType { Moderator, GlobalModerator, Admin, Staff, Viewer }
        public UserType UsersType { get; protected set; }

        public ChatMessage(string sIRCRaw)
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
            // Emotes Used
            Emotes = new EmoteCollection(IRCParser.GetTwitchTagsValue(sIRCRaw, "emotes"));
            // Message
            string[] sMessageSplit = sIRCRaw.Split(new string[] { $"#{Channel} :" }, System.StringSplitOptions.None);
            Message = sMessageSplit[1];
            // Mod Status
            Mod = IRCParser.GetTwitchTagsValue(sIRCRaw, "mod") == "1";
            // Channel ID (Room ID)
            ChannelID = int.Parse(IRCParser.GetTwitchTagsValue(sIRCRaw, "room-id"));
            // Subscriber Status
            Subscriber = IRCParser.GetTwitchTagsValue(sIRCRaw, "subscriber") == "1";
            // Username
            string[] sTagSplit = sIRCRaw.Split(' ');
            Username = sTagSplit[1].Substring(sTagSplit[1].IndexOf('!') + 1, (sTagSplit[1].IndexOf('@')) - (sTagSplit[1].IndexOf('!') + 1));
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
        }
    }
}
