using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch_Websocket
{
    public class OnWhisperMessageReceivedEventArgs : EventArgs
    {
        public WhisperMessage WhisperMessage { get; protected set; }

        public OnWhisperMessageReceivedEventArgs(WhisperMessage whisperMessage)
        {
            WhisperMessage = whisperMessage;
        }
    }
}
