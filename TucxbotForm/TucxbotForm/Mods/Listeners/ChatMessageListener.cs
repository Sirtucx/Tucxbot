namespace TucxbotForm.Listeners
{
    using System;
    using Twitch.Core;
    using Twitch.Events;
    
    public class ChatMessageListener : IModListener
    {
        public event Action<object[]> OnInputReceived;
        
        private readonly ITwitchClient m_twitchClient;
        
        public ChatMessageListener(ITwitchClient twitchClient)
        {
            m_twitchClient = twitchClient;
        }
        
        private void OnChatMessageReceived(object sender, OnChatMessageReceivedEventArgs e)
        {
            OnInputReceived?.Invoke(new object[] { e.ChatMessage });
        }
        
        public void RegisterEvents()
        {
            m_twitchClient.OnChatMessageReceived += OnChatMessageReceived;
        }

        public void UnregisterEvents()
        {
            m_twitchClient.OnChatMessageReceived -= OnChatMessageReceived;
        }
    }
}