namespace Twitch.Containers
{
    using System.Collections.Generic;
    
    public class BadgeCollection
    {
        public enum BadgeType
        {
            Null,
            Admin,
            Bit,
            Broadcaster,
            Global_Mod,
            Moderator,
            Subscriber,
            Staff,
            TurboPrime
        }

        public KeyValuePair<BadgeType, int> AdminBadge { get; protected set; }
        public KeyValuePair<BadgeType, int> BitBadge { get; protected set; }
        public KeyValuePair<BadgeType, int> BroadcasterBadge { get; protected set; }
        public KeyValuePair<BadgeType, int> GlobalModBadge { get; protected set; }
        public KeyValuePair<BadgeType, int> ModeratorBadge { get; protected set; }
        public KeyValuePair<BadgeType, int> SubscriberBadge { get; protected set; }
        public KeyValuePair<BadgeType, int> StaffBadge { get; protected set; }
        public KeyValuePair<BadgeType, int> TurboPrimeBadge { get; protected set; }

        public BadgeCollection(string sBadgeListRaw)
        {
            if (!string.IsNullOrEmpty(sBadgeListRaw))
            {
                string[] sBadgeList = sBadgeListRaw.Split(',');

                foreach (string sBadgeRaw in sBadgeList)
                {
                    string[] sBadge = sBadgeRaw.Split('/');

                    switch (sBadge[0])
                    {
                        case "admin":
                            {
                                AdminBadge = new KeyValuePair<BadgeType, int>(BadgeType.Admin, int.Parse(sBadge[1]));
                                break;
                            }
                        case "bits":
                            {
                                BitBadge = new KeyValuePair<BadgeType, int>(BadgeType.Bit, int.Parse(sBadge[1]));
                                break;
                            }
                        case "broadcaster":
                            {
                                BroadcasterBadge = new KeyValuePair<BadgeType, int>(BadgeType.Broadcaster, int.Parse(sBadge[1]));
                                break;
                            }
                        case "global_mod":
                            {
                                GlobalModBadge = new KeyValuePair<BadgeType, int>(BadgeType.Global_Mod, int.Parse(sBadge[1]));
                                break;
                            }
                        case "moderator":
                            {
                                ModeratorBadge = new KeyValuePair<BadgeType, int>(BadgeType.Moderator, int.Parse(sBadge[1]));
                                break;
                            }
                        case "staff":
                            {
                                StaffBadge = new KeyValuePair<BadgeType, int>(BadgeType.Staff, int.Parse(sBadge[1]));
                                break;
                            }
                        case "subscriber":
                            {
                                SubscriberBadge = new KeyValuePair<BadgeType, int>(BadgeType.Subscriber, int.Parse(sBadge[1]));
                                break;
                            }
                        case "premium":
                            {
                                TurboPrimeBadge = new KeyValuePair<BadgeType, int>(BadgeType.TurboPrime, int.Parse(sBadge[1]));
                                break;
                            }
                    }
                }
            }
        }
    }
}
