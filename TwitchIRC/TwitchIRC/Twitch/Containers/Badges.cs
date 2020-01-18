namespace Twitch.Containers
{
    using System.Collections.Generic;
    using System;
    using System.Linq;
    
    public class BadgeCollection
    {
        public enum BadgeType
        {
            Null = -1,
            Admin,
            Bit,
            Broadcaster,
            GlobalMod,
            Moderator,
            Subscriber,
            Staff,
            TurboPrime
        }

        private readonly string[] m_badgeIdentifiers = new[]
        {
            "admin",
            "bits",
            "broadcaster",
            "global_mod",
            "moderator",
            "subscriber",
            "staff",
            "premium"
        };

        private readonly List<Tuple<BadgeType, int>> m_badgeList;

        public Tuple<BadgeType, int> AdminBadge =>       m_badgeList[(int) BadgeType.Admin];
        public Tuple<BadgeType, int> BitBadge =>         m_badgeList[(int) BadgeType.Bit];
        public Tuple<BadgeType, int> BroadcasterBadge => m_badgeList[(int) BadgeType.Broadcaster];
        public Tuple<BadgeType, int> GlobalModBadge =>   m_badgeList[(int) BadgeType.GlobalMod];
        public Tuple<BadgeType, int> ModeratorBadge =>   m_badgeList[(int) BadgeType.Moderator];
        public Tuple<BadgeType, int> SubscriberBadge =>  m_badgeList[(int) BadgeType.Subscriber];
        public Tuple<BadgeType, int> StaffBadge =>       m_badgeList[(int) BadgeType.Staff];
        public Tuple<BadgeType, int> TurboPrimeBadge =>  m_badgeList[(int) BadgeType.TurboPrime];

        public BadgeCollection(string badgeListRaw)
        {
            m_badgeList = new List<Tuple<BadgeType, int>>();
            for (int i = 0; i < m_badgeIdentifiers.Length; ++i)
            {
                m_badgeList.Add(null);
            }

            if (string.IsNullOrEmpty(badgeListRaw))
            {
                return;
            }
            
            List<string> identifierList = m_badgeIdentifiers.ToList();
            
            string[] badgeList = badgeListRaw.Split(',');

            foreach (string badgeRaw in badgeList)
            {
                string[] badge = badgeRaw.Split('/');

                int identifierIndex = identifierList.IndexOf(badge[0]);

                if (identifierIndex != -1)
                {
                    m_badgeList[identifierIndex] = new Tuple<BadgeType, int>((BadgeType)identifierIndex, int.Parse(badge[1]));
                }
            }
        }
    }
}
