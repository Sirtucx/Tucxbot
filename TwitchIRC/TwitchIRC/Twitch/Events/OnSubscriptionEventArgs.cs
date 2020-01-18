namespace Twitch.Events
{
    using System;
    using Containers;
    
    public class OnSubscriptionEventArgs : EventArgs
    {
        public UserNotice UserNotice { get; protected set; }

        public OnSubscriptionEventArgs(UserNotice usernotice)
        {
            UserNotice = usernotice;
        }
    }
}
