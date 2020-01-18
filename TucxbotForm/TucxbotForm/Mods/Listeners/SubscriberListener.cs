namespace TucxbotForm.Listeners
{
    using System;
    using Twitch.Core;
    using Twitch.Events;
    
    public class SubscriberListener : IModListener
    {
        public event Action<object[]> OnInputReceived;
        
        private readonly ITwitchClient m_twitchClient;
        
        public SubscriberListener(ITwitchClient twitchClient)
        {
            m_twitchClient = twitchClient;
        }
        
        private void OnSubscriberReceived(object sender, OnSubscriptionEventArgs e)
        {
            OnInputReceived?.Invoke(new object[] { e.UserNotice });
        }
        
        public void RegisterEvents()
        {
            m_twitchClient.OnSubscriptionReceived += OnSubscriberReceived;
        }

        public void UnregisterEvents()
        {
            m_twitchClient.OnSubscriptionReceived -= OnSubscriberReceived;
        }
    }
}