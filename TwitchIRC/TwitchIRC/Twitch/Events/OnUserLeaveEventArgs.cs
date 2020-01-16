namespace Twitch.Events
{
    using System;
    
    public class OnUserLeaveEventArgs : EventArgs
    {
        public string Username { get; }
        public string Channel { get; }
        public OnUserLeaveEventArgs(string username, string channel)
        {
            Username = username;
            Channel = channel;
        }
    }
}
