using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchIRC
{
    public class ChatInputEventArgs
    {
        public string Target;
        public string Message;
        public string Sender;
    }
}
