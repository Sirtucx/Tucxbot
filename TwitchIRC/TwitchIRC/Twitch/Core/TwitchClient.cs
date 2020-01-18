namespace Twitch.Core
{
    using System;
    using WebSocketSharp;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using IRC;
    using Events;
    using Containers;

    public class TwitchClient : ITwitchClient
    {
        public Action OnTwitchConnected, OnTwitchLoginFailed;

        public event EventHandler<OnSubscriptionEventArgs> OnSubscriptionReceived;
        public event EventHandler<OnChatMessageReceivedEventArgs> OnChatMessageReceived;
        public event EventHandler<OnWhisperMessageReceivedEventArgs> OnWhisperMessageReceived;
        public event EventHandler<OnUserLeaveEventArgs> OnUserLeaveEvent;
        public event EventHandler<OnBotJoinedChannelEventArgs> OnBotJoinedChannel;
        public event EventHandler<OnUserJoinedEventArgs> OnUserJoinedEvent;

        private static TwitchClient s_TwitchClient;
        private static bool s_IsInitialized;
        
        private List<string> m_channelsJoined;
        private readonly string[] m_messageSeparators = { "\r\n" };
        private readonly WebSocket m_websocketClient;
        
        public IrcCredentials Credentials { get; }

        public static TwitchClient GetInstance(IrcCredentials twitchCredentials = null)
        {
            if (s_TwitchClient == null)
            {
                s_TwitchClient = new TwitchClient(twitchCredentials);

                if (!s_IsInitialized)
                {
                    s_TwitchClient = null;
                }
            }
            return s_TwitchClient;
        }
        
        private TwitchClient(IrcCredentials twitchCredentials)
        {
            s_IsInitialized = false;
            if (twitchCredentials == null || !twitchCredentials.Valid)
            {
                return;   
            }

            Credentials = twitchCredentials;
            m_websocketClient = new WebSocket($"ws://{Credentials.TwitchHost}:{Credentials.TwitchPort}");
            Initialize();
        }

        private void Initialize()
        {
            m_channelsJoined = new List<string>();

            m_websocketClient.OnOpen += OnWebsocketClientConnected;
            m_websocketClient.OnMessage += OnWebsocketClientReceivedMessage;
            m_websocketClient.OnClose += OnWebsocketClientDisconnected;
            m_websocketClient.OnError += OnWebsocketClientError;
            s_IsInitialized = true;
        }
        
        private void OnWebsocketClientConnected (object sender, object e)
        {
            m_websocketClient.Send(IRCHelper.GetPasswordSubmission(Credentials.TwitchOAuth));
            m_websocketClient.Send(IRCHelper.GetNicknameSubmission(Credentials.TwitchUsername));
            m_websocketClient.Send(IRCHelper.GetUsernameSubmission(Credentials.TwitchUsername, 0));

            m_websocketClient.Send(IRCHelper.MembershipRequest);
            m_websocketClient.Send(IRCHelper.CommandRequest);
            m_websocketClient.Send(IRCHelper.TagsRequest);
            OnUserLeaveEvent += OnUserLeft;
        }
        private void OnWebsocketClientDisconnected(object sender, CloseEventArgs e)
        {
            OnUserLeaveEvent -= OnUserLeft;
        }
        private void OnWebsocketClientError(object sender, ErrorEventArgs e)
        {
            Reconnect();
            System.Threading.Thread.Sleep(2000);
        }
        
        private void OnWebsocketClientReceivedMessage(object sender, MessageEventArgs e)
        {
            string[] splitMessages = e.Data.Split(m_messageSeparators, StringSplitOptions.None);

            foreach (string line in splitMessages)
            {
                if (line.Length > 1 && e.IsText)
                {
                    ParseResponse(line);
                }
            }
        }
        
        private void OnUserLeft(object sender, OnUserLeaveEventArgs e)
        {
            if (e.Username == Credentials.TwitchUsername && m_channelsJoined.Contains(e.Channel))
            {
                m_channelsJoined.Remove(e.Channel);
            }
        }

        private void ParseResponse(string ircRawMessage)
        {
            //Console.WriteLine(ircRawMessage);

            foreach (string channelName in m_channelsJoined)
            {
                IRCParser.TwitchChannelMessageType channelMessageType = IRCParser.GetChannelMessageType(ircRawMessage, channelName);

                if (channelMessageType != IRCParser.TwitchChannelMessageType.None)
                {
                    HandleChannelMessage(ircRawMessage, channelName, channelMessageType);
                    return;
                }
            }

            IRCParser.TwitchMessageType twitchMessageType = IRCParser.GetNonChannelMessageType(ircRawMessage, Credentials.TwitchUsername);
            HandleNonChannelMessage(ircRawMessage, twitchMessageType);
        }

        private void HandleChannelMessage(string ircRawMessage, string channelName, IRCParser.TwitchChannelMessageType channelMessageType)
        {
            switch (channelMessageType)
                {
                    case IRCParser.TwitchChannelMessageType.Usernotice:
                    {
                        UserNotice usernotice = new UserNotice(ircRawMessage);
                        if (usernotice.MessageID != UserNotice.SubscriptionType.None)
                        {
                            OnSubscriptionEventArgs subEvent = new OnSubscriptionEventArgs(usernotice);
                            OnSubscriptionReceived?.Invoke(this, subEvent);
                        }

                        return;
                    }
                    case IRCParser.TwitchChannelMessageType.ChannelMessage:
                    {
                        ChatMessage chatMessage = new ChatMessage(ircRawMessage);
                        OnChatMessageReceivedEventArgs onChatReceivedEvent = new OnChatMessageReceivedEventArgs(chatMessage);
                        OnChatMessageReceived?.Invoke(this, onChatReceivedEvent);
                        return;
                    }
                    case IRCParser.TwitchChannelMessageType.LeftChannel:
                    {
                        // USER LEFT CHANNEL
                        //:tucxbot!tucxbot@tucxbot.tmi.twitch.tv PART #sirtucx
                        string sUsername = ircRawMessage.Substring(1, ircRawMessage.IndexOf('!') - 1);
                        OnUserLeaveEventArgs onUserLeaveEventArgs = new OnUserLeaveEventArgs(sUsername, channelName);
                        OnUserLeaveEvent?.Invoke(this, onUserLeaveEventArgs);
                        return;
                    }
                    case IRCParser.TwitchChannelMessageType.Mode:
                    {
                        string[] sModeSplit = ircRawMessage.Split(' ');

                        if (sModeSplit[3] == "+o")
                        {
                            // TODO: MOD JOINED CHANNEL
                        }
                        else if (sModeSplit[3] == "-o")
                        {
                            // TODO: MOD LEFT CHANNEL
                        }
                        else
                        {
                            // TODO: UNKNOWN
                        }

                        return;
                    }
                    case IRCParser.TwitchChannelMessageType.Notice:
                    {
                        if (ircRawMessage.Contains("Improperly formatted auth"))
                        {
                            // TODO: AUTH ERROR
                        }
                        else if (ircRawMessage.Contains("has gone offline"))
                        {
                            // TODO: HOST LEFT
                        }
                        else if (ircRawMessage.Contains("The moderators of this room are:"))
                        {
                            // TODO: LIST ALL MODERATORS
                        }
                        else if (ircRawMessage.Contains("Your color has been changed."))
                        {
                            // TODO: DETECT COLOR CHANGE
                        } 
                        return;
                    }

                    case IRCParser.TwitchChannelMessageType.RoomState:
                    {
                        // TODO: ROOM STATE CHANGED
                        return;
                    }

                    case IRCParser.TwitchChannelMessageType.UserState:
                    {
                        // USER JOINED CHANNEL
                        // Example:  @badges=staff/1;color=#0D4200;display-name=ronni;emote-sets=0,33,50,237,793,2126,3517,4578,5569,9400,10337,12239;mod=1;subscriber=1;turbo=1;user-type=staff :tmi.twitch.tv USERSTATE #dallas
                        UserState userState = new UserState(ircRawMessage);
                        OnUserJoinedEventArgs onUserJoinedEventArgs = new OnUserJoinedEventArgs(userState);
                        OnUserJoinedEvent?.Invoke(this, onUserJoinedEventArgs);
                        return;
                    }
                    case IRCParser.TwitchChannelMessageType.ClearChat:
                    {
                        // TODO: HANDLE BANS/TIMEOUTS
                        return;
                    }
                }
        }
        private void HandleNonChannelMessage(string ircRawMessage, IRCParser.TwitchMessageType twitchMessageType)
        {
            switch (twitchMessageType)
            {
                case IRCParser.TwitchMessageType.Ping:
                {
                    m_websocketClient.Send("PONG :tmi.twitch.tv");
                    break;
                }
                case IRCParser.TwitchMessageType.Notice:
                    break;
                case IRCParser.TwitchMessageType.WhisperMessage:
                {
                    WhisperMessage whisperMessage = new WhisperMessage(ircRawMessage);
                    OnWhisperMessageReceivedEventArgs whisperEvent = new OnWhisperMessageReceivedEventArgs(whisperMessage);
                    OnWhisperMessageReceived?.Invoke(this, whisperEvent);
                    break;
                }
                case IRCParser.TwitchMessageType.JoinedChannel:
                {
                    string sChannel = ircRawMessage.Substring(ircRawMessage.IndexOf('#') + 1, ircRawMessage.IndexOf(" :") - ((ircRawMessage.IndexOf('#') + 1)));
                    OnBotJoinedChannel?.Invoke(this, new OnBotJoinedChannelEventArgs(sChannel));
                    m_channelsJoined.Add(sChannel);
                    break;
                }
                case IRCParser.TwitchMessageType.LoginSuccessful:
                {
                    // LOGIN SUCCESSFUL
                    OnTwitchConnected?.Invoke();
                    break;
                }
                case IRCParser.TwitchMessageType.LoginFailure:
                {
                    // LOGIN FAILED
                    OnTwitchLoginFailed?.Invoke();
                    break;
                }
            }
        }
        
        public void Connect()
        {
            m_websocketClient.Connect();
        }
        public void Disconnect()
        {
            Task.Factory.StartNew(() => 
            {
                for(int i = 0; i < m_channelsJoined.Count; ++i)
                {
                    LeaveChannel(m_channelsJoined[i]);
                }
                m_websocketClient.Close();
            });
        }
        private void Reconnect()
        {
            if (m_websocketClient.IsAlive)
            {
                m_websocketClient.Close();
            }
            m_websocketClient.Connect();
        }

        public void JoinChannel(string channelName)
        {
            channelName = channelName.ToLower();
            if (!m_channelsJoined.Contains(channelName))
            {
                m_websocketClient.Send(IRCHelper.GetJoinChannelCommand(channelName));
            }
        }
        public void LeaveChannel(string channelName)
        {
            channelName = channelName.ToLower();
            if (m_channelsJoined.Contains(channelName))
            {
                m_websocketClient.Send(IRCHelper.GetLeaveChannelCommand(channelName));
            }
        }
        public void SendChatMessage(string sChannel, string sMessage)
        {
            m_websocketClient.Send(IRCHelper.GetChannelMessageCommand(sChannel, sMessage));
        }
        public void SendWhisperMessage(string sUsername, string sMessage)
        {
            m_websocketClient.Send(IRCHelper.GetWhisperCommand(sUsername, sMessage));
        }
    }
}
