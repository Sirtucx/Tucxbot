namespace Twitch.Events
{
    using System;
    using Containers;
    
    public class OnUserJoinedEventArgs : EventArgs
    {
        public UserState UserState { get; }

        public OnUserJoinedEventArgs(UserState userState)
        {
            UserState = userState;
        }
    }
}
