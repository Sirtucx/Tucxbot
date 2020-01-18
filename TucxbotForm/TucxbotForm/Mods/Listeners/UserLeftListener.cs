namespace TucxbotForm.Listeners
{
    using System;
    using Twitch.Core;
    using Twitch.Events;
    
    public class UserLeftListener : IModListener
    {
        public event Action<object[]> OnInputReceived;
        
        private readonly ITwitchClient m_twitchClient;
        
        public UserLeftListener(ITwitchClient twitchClient)
        {
            m_twitchClient = twitchClient;
        }
        
        private void OnUserLeft(object sender, OnUserLeaveEventArgs e)
        {
            OnInputReceived?.Invoke(new object[] { e.Channel, e.Username });
        }
        
        public void RegisterEvents()
        {
            m_twitchClient.OnUserLeaveEvent += OnUserLeft;
        }

        public void UnregisterEvents()
        {
            m_twitchClient.OnUserLeaveEvent -= OnUserLeft;
        }
    }
}