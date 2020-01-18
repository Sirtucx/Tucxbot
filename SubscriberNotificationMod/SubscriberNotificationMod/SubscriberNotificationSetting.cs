namespace SubscriberNotificationMod
{
    using System.Collections.Generic;
    using Twitch.Containers;
    
    public class SubscriberNotificationSetting
    {
        private const string USERNAME_VARIABLE = "{username}";
        private const string SUB_STREAK_VARIABLE = "{substreak}";
        private const string GIFT_RECIPIENT = "{giftusername}";
        private const string GIFTED_SUB_COUNT = "{giftcount}";
        private const string SUB_TYPE = "{subtype}";

        private readonly Dictionary<string, SubscriberMessage> m_subscriberMessages;
       
        public SubscriberNotificationSetting()
        {
            m_subscriberMessages = new Dictionary<string, SubscriberMessage>();
            m_subscriberMessages.Add("default", new SubscriberMessage());
        }

        public string GetMessage(UserNotice twitchNotice)
        {
            string sUsername = twitchNotice.Username;

            return CreateMessage(m_subscriberMessages.ContainsKey(sUsername.ToLower()) ? m_subscriberMessages[sUsername.ToLower()] : m_subscriberMessages["default"], twitchNotice);
        }

        private string CreateMessage(SubscriberMessage subscriberMessage, UserNotice twitchNotice)
        {
            string username = twitchNotice.Username;
            UserNotice.SubscriptionType subscriptionType = twitchNotice.MessageID;
            UserNotice.SubscriptionPlan subscriptionPlan = twitchNotice.SubPlan;

            string message = "";

            switch (subscriptionType)
            {
                case UserNotice.SubscriptionType.Gift:
                    {
                        message = subscriberMessage.GiftingMessage;
                        break;
                    }
                case UserNotice.SubscriptionType.Sub:
                case UserNotice.SubscriptionType.Resub:
                    {
                        switch (subscriptionPlan)
                        {
                            case UserNotice.SubscriptionPlan.Prime:
                                {
                                    message = subscriberMessage.PrimeMessage;
                                    break;
                                }
                            case UserNotice.SubscriptionPlan.Tier1:
                                {
                                    message = subscriberMessage.TierOneMessage;
                                    break;
                                }
                            case UserNotice.SubscriptionPlan.Tier2:
                                {
                                    message = subscriberMessage.TierTwoMessage;
                                    break;
                                }
                            case UserNotice.SubscriptionPlan.Tier3:
                                {
                                    message = subscriberMessage.TierThreeMessage;
                                    break;
                                }
                        }
                        break;
                    }
            }

            if (subscriptionType == UserNotice.SubscriptionType.Resub)
            {
                message += subscriberMessage.SubStreakMessage;
            }

            message = message.Replace(USERNAME_VARIABLE, subscriberMessage.Username);
            message = message.Replace(SUB_STREAK_VARIABLE, twitchNotice.ResubConsecutiveMonths.ToString());
            message = message.Replace(GIFT_RECIPIENT, twitchNotice.GiftedSubscriptionRecipientName);
            message = message.Replace(GIFTED_SUB_COUNT, twitchNotice.GiftedSubscriptionsCount.ToString());
            message = message.Replace(SUB_TYPE, twitchNotice.SubscriptionPlanName);

            return message;
        }
    }

    public class SubscriberMessage
    {                                       
        public readonly string Username;
        public readonly string TierOneMessage =      "Thank you {username} for subscribing to the channel";
        public readonly string TierTwoMessage =      "Thank you {username} for subscribing to the channel with a {subtype} sub";
        public readonly string TierThreeMessage =    "Thank you {username} for subscribing to the channel with a {subtype} sub";
        public readonly string PrimeMessage =        "Thank you {username} for subscribing to the channel using Twitch Prime";
        public readonly string GiftingMessage =      "Thank you {username} for gifting {giftusername} a {subtype}";
        public readonly string SubStreakMessage =    "and thank you for remaining subscribed for {substreak} months";

        public SubscriberMessage(string sUsername = "default")
        {
            Username = sUsername;
        }
    }
}
