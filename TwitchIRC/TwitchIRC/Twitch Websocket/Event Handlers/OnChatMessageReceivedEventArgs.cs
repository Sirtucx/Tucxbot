using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch_Websocket
{
    public class OnChatMessageReceivedEventArgs : EventArgs
    {
        public ChatMessage ChatMessage { get; protected set; }

        public OnChatMessageReceivedEventArgs(ChatMessage chatMessage)
        {
            ChatMessage = chatMessage;
        }
    }
}
