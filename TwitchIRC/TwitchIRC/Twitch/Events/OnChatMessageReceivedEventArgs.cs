namespace Twitch.Events
{
    using System;
    using Containers;
    
    public class OnChatMessageReceivedEventArgs : EventArgs
    {
        public ChatMessage ChatMessage { get; }

        public OnChatMessageReceivedEventArgs(ChatMessage chatMessage)
        {
            ChatMessage = chatMessage;
        }
    }
}
