namespace Twitch.IRC
{
    using System.Linq;
    using System.Text.RegularExpressions;
    using Twitch.Core;
    
    public static class IRCParser
    {
        public enum TwitchChannelMessageType
        {
            None = -1,
            Usernotice,
            ChannelMessage,
            LeftChannel,
            Mode,
            Notice,
            RoomState,
            UserState,
            ClearChat,
        }

        public enum TwitchMessageType
        {
            None = -1,
            Ping,
            Notice,
            WhisperMessage,
            JoinedChannel,
            LoginSuccessful,
            LoginFailure
        }

        private static string[] s_TwitchChannelMessageTypeValues = 
        {
          "USERNOTICE",
          "PRIVMSG",
          "PART",
          "MODE",
          "NOTICE",
          "ROOMSTATE",
          "USERSTATE",
          "CLEARCHAT"
        };

        private static readonly string s_SuccessfulChannelJoinMessage = "End of /NAMES list";
        private static readonly string s_SuccessfulLoginMessage = "You are in a maze of twisty passages, all alike.";
        private static readonly string s_LoginFailureMessage = "Login authentication failed";

        
        public static TwitchChannelMessageType GetChannelMessageType(string ircRawMessage, string channelName)
        {
            string[] channelSplitter = Regex.Split(ircRawMessage, $" #{channelName}");
            string[] contentSplitter = channelSplitter[0].Split(' ');
            string readType = contentSplitter[contentSplitter.Length - 1];

            return (TwitchChannelMessageType) s_TwitchChannelMessageTypeValues.ToList().IndexOf(readType);
        }

        public static TwitchMessageType GetNonChannelMessageType(string ircRawMessage, string botUsername)
        {
            if (ircRawMessage.Contains(IRCHelper.PingRequest))
            {
                return TwitchMessageType.Ping;
            }
            
            string[] splitMessages = ircRawMessage.Split(' ');
            if (splitMessages.Length > 1 && splitMessages[1] == s_TwitchChannelMessageTypeValues[(int) TwitchChannelMessageType.Notice])
            {
                if (ircRawMessage.Contains(s_LoginFailureMessage))
                {
                    return TwitchMessageType.LoginFailure;
                }
                return TwitchMessageType.Notice;
            }

            if (splitMessages.Length <= 2)
            {
                return TwitchMessageType.None;
            }

            if (IsWhisperMessageType(splitMessages))
            {
                return TwitchMessageType.WhisperMessage;
            }
            
            if (splitMessages[2] == botUsername)
            {
                if (IsChannelJoinType(ircRawMessage, botUsername))
                {
                    return TwitchMessageType.JoinedChannel;
                }
                
                if (ircRawMessage.Contains(s_SuccessfulLoginMessage))
                {
                    return TwitchMessageType.LoginSuccessful;
                }
            }

            return TwitchMessageType.None;
        }

        private static bool IsWhisperMessageType(string[] splitMessages)
        {
            if (splitMessages[1] == s_TwitchChannelMessageTypeValues[(int) TwitchMessageType.WhisperMessage] ||
                splitMessages[2] == s_TwitchChannelMessageTypeValues[(int) TwitchMessageType.WhisperMessage])
            {
                int whisperIndex = splitMessages.ToList().IndexOf(s_TwitchChannelMessageTypeValues[(int) TwitchMessageType.WhisperMessage]);
                string[] userSplit = splitMessages[whisperIndex + 1].Split(':');

                return userSplit[0] == TwitchClient.GetInstance().Credentials.TwitchUsername.ToLower();
            }

            return false;
        }

        private static bool IsChannelJoinType(string ircRawMessage, string botUsername)
        {
            if (ircRawMessage.Contains($"{botUsername}.tmi.twitch.tv"))
            {
                string sUserConfirmation = ircRawMessage.Substring(1, ircRawMessage.IndexOf('.') - 1);
                if (sUserConfirmation == botUsername)
                {
                    string sMessageContents = ircRawMessage.Substring(ircRawMessage.IndexOf(':', 1) + 1, ircRawMessage.Length - (ircRawMessage.IndexOf(':', 1) + 1));
                    
                    //:tucxbot.tmi.twitch.tv 366 tucxbot #sirtucx :End of /NAMES list
                    return sMessageContents == s_SuccessfulChannelJoinMessage ;
                }
            }

            return false;
        }
        
        public static string GetTwitchTagsValue(string ircRawMessage, string key)
        {
            string[] parts = ircRawMessage.Split(';');
            foreach (string part in parts)
            {
                if (part.Contains($"{key}="))
                {
                    string[] keyValue = part.Split('=');
                    return keyValue[1];
                }
            }
            return null;
        }
    }
}
