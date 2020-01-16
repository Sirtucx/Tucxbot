namespace Twitch.Events
{
    using System;
    
    public class OnBotJoinedChannelEventArgs : EventArgs
    {
        public string ChannelName { get; }
        public OnBotJoinedChannelEventArgs(string channelName)
        {
            ChannelName = channelName;
        }
    }
}
