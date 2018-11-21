using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twitch_Websocket;
using Twitch_Websocket.Mod_Interfaces;

namespace SubscriberNotificationMod
{
    public class SubscriberNotificationSetting
    {
        protected const string UsernameVariable = "{username}";
        protected const string SubStreakVariable = "{substreak}";
        protected const string GiftRecipient = "{giftusername}";
        protected const string GiftedSubCount = "{giftcount}";
        protected const string SubType = "{subtype}";

        public Dictionary<string, SubscriberMessage> SubscriberMessages;
       
        public SubscriberNotificationSetting()
        {
            SubscriberMessages = new Dictionary<string, SubscriberMessage>();
            SubscriberMessages.Add("default", new SubscriberMessage());
        }

        public string GetMessage(UserNotice twitchNotice)
        {
            string sUsername = twitchNotice.Username;

            if (SubscriberMessages.ContainsKey(sUsername.ToLower()))
            {
                return CreateMessage(SubscriberMessages[sUsername.ToLower()], twitchNotice);
            }
            else
            {
                return CreateMessage(SubscriberMessages["default"], twitchNotice);
            }
        }

        protected string CreateMessage(SubscriberMessage subscriberMessage, UserNotice twitchNotice)
        {
            string sUsername = twitchNotice.Username;
            UserNotice.SubscriptionType subscriptionType = twitchNotice.MessageID;
            UserNotice.SubscriptionPlan subscriptionPlan = twitchNotice.SubPlan;

            string sMessage = "";

            switch (subscriptionType)
            {
                case UserNotice.SubscriptionType.Gift:
                    {
                        sMessage = subscriberMessage.GiftingMessage;
                        break;
                    }
                case UserNotice.SubscriptionType.Sub:
                case UserNotice.SubscriptionType.Resub:
                    {
                        switch (subscriptionPlan)
                        {
                            case UserNotice.SubscriptionPlan.Prime:
                                {
                                    sMessage = subscriberMessage.PrimeMessage;
                                    break;
                                }
                            case UserNotice.SubscriptionPlan.Tier1:
                                {
                                    sMessage = subscriberMessage.TierOneMessage;
                                    break;
                                }
                            case UserNotice.SubscriptionPlan.Tier2:
                                {
                                    sMessage = subscriberMessage.TierTwoMessage;
                                    break;
                                }
                            case UserNotice.SubscriptionPlan.Tier3:
                                {
                                    sMessage = subscriberMessage.TierThreeMessage;
                                    break;
                                }
                        }
                        break;
                    }
            }

            if (subscriptionType == UserNotice.SubscriptionType.Resub)
            {
                sMessage += subscriberMessage.SubStreakMessage;
            }

            sMessage = sMessage.Replace(UsernameVariable, sUsername);
            sMessage = sMessage.Replace(SubStreakVariable, twitchNotice.ResubConsecutiveMonths.ToString());
            sMessage = sMessage.Replace(GiftRecipient, twitchNotice.GiftedSubscriptionRecipientName);
            sMessage = sMessage.Replace(GiftedSubCount, twitchNotice.GiftedSubscriptionsCount.ToString());
            sMessage = sMessage.Replace(SubType, twitchNotice.SubscriptionPlanName);

            return sMessage;
        }
    }

    public class SubscriberMessage
    {                                       
        public string Username;
        public string TierOneMessage =      "Thank you {username} for subscribing to the channel";
        public string TierTwoMessage =      "Thank you {username} for subscribing to the channel with a {subtype} sub";
        public string TierThreeMessage =    "Thank you {username} for subscribing to the channel with a {subtype} sub";
        public string PrimeMessage =        "Thank you {username} for subscribing to the channel using Twitch Prime";
        public string GiftingMessage =      "Thank you {username} for gifting {giftusername} a {subtype}";
        public string SubStreakMessage =    "and thank you for remaining subscribed for {substreak} months";

        public SubscriberMessage(string sUsername = "default")
        {
            Username = sUsername;
        }
    }
}
