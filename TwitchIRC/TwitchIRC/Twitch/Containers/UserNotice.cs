namespace Twitch.Containers
{
    using IRC;
    
    public class UserNotice
    {
        public enum SubscriptionType
        {
            None,
            Sub,
            Resub,
            Charity,
            Gift
        }

        public enum SubscriptionPlan
        {
            None,
            Prime,
            Tier1,
            Tier2,
            Tier3
        }

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
        public string ColorHex { get; }
        public string DisplayName { get; }
        public EmoteCollection Emotes { get; }
        public string Message { get; }
        public bool Mod { get; }
        public SubscriptionType MessageID { get; }
        public int ResubConsecutiveMonths { get; }
        public SubscriptionPlan SubPlan { get; } = SubscriptionPlan.None;
        public string SubscriptionPlanName { get; }
        public string GiftedSubscriptionRecipientName { get; }
        public int GiftedSubscriptionsCount { get; }
        public int ChannelID { get; }
        public bool Subscriber { get; }
        public string SystemMessage { get; }
        public bool Turbo { get; }
        public string Username { get; }
        public int UserID { get; }
        public UserType UsersType { get; }

        public UserNotice(string ircRawMessage)
        {
            // RawMessage IRC string
            RawMessage = ircRawMessage;

            // Channel notice was sent in
            int startIndex = ircRawMessage.IndexOf('#', ircRawMessage.IndexOf("USERNOTICE")) + 1;
            int length = ircRawMessage.IndexOf(':', startIndex);

            if (length == -1)
            {
                length = ircRawMessage.Length;
            }
            Channel = ircRawMessage.Substring(startIndex, length - startIndex);

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
            Message = messageSplit[0].Replace($"{messageSplit[0]}#{Channel} :", "");
            // Mod Status
            Mod = IRCParser.GetTwitchTagsValue(ircRawMessage, "mod") == "1";

            // Message ID
            string messageIdRaw = IRCParser.GetTwitchTagsValue(ircRawMessage, "msg-id");
            switch (messageIdRaw)
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
                case "subgift":
                    {
                        MessageID = SubscriptionType.Gift;
                        break;
                    }
                case "charity":
                    {
                        MessageID = SubscriptionType.Charity;
                        break;
                    }
            }
            // Resub Consecutive Months
            if (int.TryParse(IRCParser.GetTwitchTagsValue(ircRawMessage, "msg-param-months"), out int resubCount))
            {
                ResubConsecutiveMonths = resubCount;
            }

            // Sub Plan
            string subPlanRaw = IRCParser.GetTwitchTagsValue(ircRawMessage, "msg-param-sub-plan");
            switch (subPlanRaw.ToLower())
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
            // Sub Gift Recipient Name
            GiftedSubscriptionRecipientName = IRCParser.GetTwitchTagsValue(ircRawMessage, "msg-param-recipient-display-name");
            // Gifted Subscription Count
            if (int.TryParse(IRCParser.GetTwitchTagsValue(ircRawMessage, "msg-param-sender-count"), out int giftedSubCount))
            {
                GiftedSubscriptionsCount = giftedSubCount + 1;
            }
            // Sub Plan Name
            SubscriptionPlanName = IRCParser.GetTwitchTagsValue(ircRawMessage, "msg-param-sub-plan-name").Replace("\\s", " ");
            // Channel ID (Room ID)
            ChannelID = int.Parse(IRCParser.GetTwitchTagsValue(ircRawMessage, "room-id"));
            // Subscriber Status
            Subscriber = IRCParser.GetTwitchTagsValue(ircRawMessage, "subscriber") == "1";
            // System Message
            SystemMessage = IRCParser.GetTwitchTagsValue(ircRawMessage, "system-msg");
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

            Username = DisplayName;
        }
    }
}
