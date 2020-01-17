namespace TucxbotForm.Listeners
{
    using System;
    using Twitch.Core;
    using Twitch.Events;
    
    public class WhisperMessageListener : IModListener
    {
        public event Action<object[]> OnInputReceived;
        
        private readonly ITwitchClient m_twitchClient;
        
        public WhisperMessageListener(ITwitchClient twitchClient)
        {
            m_twitchClient = twitchClient;
        }
        
        private void OnWhisperMessageReceived(object sender, OnWhisperMessageReceivedEventArgs e)
        {
            OnInputReceived?.Invoke(new object[] { e.WhisperMessage });
        }
        
        public void RegisterEvents()
        {
            m_twitchClient.OnWhisperMessageReceived += OnWhisperMessageReceived;
        }

        public void UnregisterEvents()
        {
            m_twitchClient.OnWhisperMessageReceived -= OnWhisperMessageReceived;
        }
    }
}