namespace Twitch_Websocket
{
    public class UserNotice
    {
        public string Raw { get; protected set; }
        public string Channel { get; protected set; }

        public BadgeCollection Badges { get; protected set; }
        public string ColorHex { get; protected set; }
        public string DisplayName { get; protected set; }
        public EmoteCollection Emotes { get; protected set; }
        public string Message { get; protected set; }
        public bool Mod { get; protected set; }
        public enum SubscriptionType { None, Sub, Resub, Charity }
        public SubscriptionType MessageID { get; protected set; }
        public int ResubConsecutiveMonths { get; protected set; }
        public enum SubscriptionPlan { None, Prime, Tier1, Tier2, Tier3 }
        public SubscriptionPlan SubPlan { get; protected set; } = SubscriptionPlan.None;
        public string SubscriptionPlanName { get; protected set; }
        public int ChannelID { get; protected set; }
        public bool Subscriber { get; protected set; }
        public string SystemMessage { get; protected set; }
        public bool Turbo { get; protected set; }
        public string Username { get; protected set; }
        public int UserID { get; protected set; }
        public enum UserType { Moderator, GlobalModerator, Admin, Staff, Viewer }
        public UserType UsersType { get; protected set; }

        public UserNotice(string sIRCRaw)
        {
            // Raw IRC string
            Raw = sIRCRaw;

            // Channel notice was sent in
            string[] sChannelSplit = sIRCRaw.Split('#');
            Channel = sChannelSplit[1].Substring(0, sChannelSplit[1].IndexOf(' '));

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
            Message = sMessageSplit[0].Replace($"{sMessageSplit[0]}#{Channel} :", "");
            // Mod Status
            Mod = IRCParser.GetTwitchTagsValue(sIRCRaw, "mod") == "1";

            // Message ID
            string sMessageIDRaw = IRCParser.GetTwitchTagsValue(sIRCRaw, "msg-id");
            switch (sMessageIDRaw)
            {
                case "sub":
                    {
                        MessageID = SubscriptionType.Sub;
                        break;
                    }
                case "resub":
                    {
                        MessageID = SubscriptionType.Resub;
                        break;
                    }
                case "charity":
                    {
                        MessageID = SubscriptionType.Charity;
                        break;
                    }
            }
            // Resub Consecutive Months
            ResubConsecutiveMonths = int.Parse(IRCParser.GetTwitchTagsValue(sIRCRaw, "mes-param-months"));

            // Sub Plan
            string sSubPlanRaw = IRCParser.GetTwitchTagsValue(sIRCRaw, "msg-param-sub-plan");
            switch (sSubPlanRaw.ToLower())
            {
                case "prime":
                    {
                        SubPlan = SubscriptionPlan.Prime;
                        break;
                    }
                case "1000":
                    {
                        // Tier 1
                        SubPlan = SubscriptionPlan.Tier1;
                        break;
                    }
                case "2000":
                    {
                        // Tier 2
                        SubPlan = SubscriptionPlan.Tier2;
                        break;
                    }
                case "3000":
                    {
                        // Tier 3
                        SubPlan = SubscriptionPlan.Tier3;
                        break;
                    }
            }

            // Sub Plan Name
            SubscriptionPlanName = IRCParser.GetTwitchTagsValue(sIRCRaw, "msg-param-sub-plan-name").Replace("\\s", " ");
            // Channel ID (Room ID)
            ChannelID = int.Parse(IRCParser.GetTwitchTagsValue(sIRCRaw, "room-id"));
            // Subscriber Status
            Subscriber = IRCParser.GetTwitchTagsValue(sIRCRaw, "subscriber") == "1";
            // System Message
            SystemMessage = IRCParser.GetTwitchTagsValue(sIRCRaw, "system-msg");
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
