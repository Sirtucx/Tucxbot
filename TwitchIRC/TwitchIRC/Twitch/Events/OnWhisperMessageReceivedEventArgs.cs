namespace Twitch.Events
{
    using System;
    using Containers;
    
    public class OnWhisperMessageReceivedEventArgs : EventArgs
    {
        public WhisperMessage WhisperMessage { get; }

        public OnWhisperMessageReceivedEventArgs(WhisperMessage whisperMessage)
        {
            WhisperMessage = whisperMessage;
        }
    }
}
