using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twitch_Websocket;
using Twitch_Websocket.Mod_Interfaces;

namespace SubscriberNotificationMod
{
    class SubscriberNotification : IOnSubscriberMod
    {
        TwitchClient m_TwitchClient;
        public void Process(UserNotice usernotice)
        {
            if (m_TwitchClient == null)
            {
                m_TwitchClient = TwitchClient.GetInstance();
            }
            string sMessage = $"MrDestructoid Thank you {usernotice.Username} for";

            if (usernotice.MessageID != UserNotice.SubscriptionType.Gift)
            {
                switch (usernotice.SubPlan)
                {
                    case UserNotice.SubscriptionPlan.Prime:
                        {
                            sMessage = $"{sMessage} subscribing to the channel using Twitch Prime";
                            break;
                        }
                    case UserNotice.SubscriptionPlan.Tier1:
                        {
                            sMessage = $"{sMessage} subscribing to the channel";
                            break;
                        }
                    case UserNotice.SubscriptionPlan.Tier2:
                        {
                            sMessage = $"{sMessage} becoming the SUPER SHINY SUB";
                            break;
                        }
                    case UserNotice.SubscriptionPlan.Tier3:
                        {
                            sMessage = $"{sMessage} becoming the ULTIMATE SUB";
                            break;
                        }
                }

                if (usernotice.ResubConsecutiveMonths > 1)
                {
                    sMessage = $"{sMessage} and thank you for remaining subscribed for {usernotice.ResubConsecutiveMonths} months! MrDestructoid";
                }
            }
            else
            {
                sMessage = $"{sMessage} gifting {usernotice.GiftedSubscriptionRecipientName}";

                switch (usernotice.SubPlan)
                {
                    case UserNotice.SubscriptionPlan.Tier1:
                        {
                            sMessage = $"{sMessage} a sub!";
                            break;
                        }
                    case UserNotice.SubscriptionPlan.Tier2:
                        {
                            sMessage = $"{sMessage} a SUPER SHINY SUB!";
                            break;
                        }
                    case UserNotice.SubscriptionPlan.Tier3:
                        {
                            sMessage = $"{sMessage} the ULTIMATE SUB!";
                            break;
                        }
                }

                sMessage = $"{sMessage} {usernotice.Username} has gifted a total of {usernotice.GiftedSubscriptionsCount} subs in this channel! MrDestructoid";
            }

            m_TwitchClient.SendChatMessage(usernotice.Channel, sMessage);
        }
        public void Shutdown()
        {

        }
    }
}
