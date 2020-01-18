namespace Twitch.Core
{
    using System;
    using Events;
    public interface ITwitchClient
    {
        event EventHandler<OnSubscriptionEventArgs> OnSubscriptionReceived;
        event EventHandler<OnChatMessageReceivedEventArgs> OnChatMessageReceived;
        event EventHandler<OnWhisperMessageReceivedEventArgs> OnWhisperMessageReceived;
        event EventHandler<OnUserLeaveEventArgs> OnUserLeaveEvent;
        event EventHandler<OnBotJoinedChannelEventArgs> OnBotJoinedChannel;
        event EventHandler<OnUserJoinedEventArgs> OnUserJoinedEvent;
    }
}