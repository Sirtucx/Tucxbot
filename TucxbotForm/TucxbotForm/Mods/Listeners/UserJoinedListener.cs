namespace TucxbotForm.Listeners
{
    using System;
    using Twitch.Core;
    using Twitch.Events;
    
    public class UserJoinedListener : IModListener
    {
        public event Action<object[]> OnInputReceived;
        
        private readonly ITwitchClient m_twitchClient;
        
        public UserJoinedListener(ITwitchClient twitchClient)
        {
            m_twitchClient = twitchClient;
        }
        
        private void OnUserJoined(object sender, OnUserJoinedEventArgs e)
        {
            OnInputReceived?.Invoke(new object[] { e.UserState });
        }
        
        public void RegisterEvents()
        {
            m_twitchClient.OnUserJoinedEvent += OnUserJoined;
        }

        public void UnregisterEvents()
        {
            m_twitchClient.OnUserJoinedEvent -= OnUserJoined;
        }
    }
}