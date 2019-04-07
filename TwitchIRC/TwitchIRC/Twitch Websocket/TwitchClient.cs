namespace Twitch_Websocket
{
    using System;
    using WebSocketSharp;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public class TwitchClient
    {
        #region Singleton
        private static TwitchClient m_Instance;
        private static bool m_bInitialized;
        public static TwitchClient GetInstance(IRCCredentials twitchCredentials = null)
        {
            if (m_Instance == null)
            {
                m_Instance = new TwitchClient(twitchCredentials);
            }
            return m_Instance;
        }
        #endregion Singleton

        public Action OnTwitchConnected, OnTwitchLoginFailed;

        #region Events
        public event EventHandler<OnSubscriberEventArgs> OnSubscriberEvent;
        public event EventHandler<OnChatMessageReceivedEventArgs> OnChatMessageReceived;
        public event EventHandler<OnWhisperMessageReceivedEventArgs> OnWhisperMessageReceived;
        public event EventHandler<OnUserLeaveEventArgs> OnUserLeaveEvent;
        public event EventHandler<OnBotJoinedChannelEventArgs> OnBotJoinedChannelEvent;
        public event EventHandler<OnUserJoinedEventArgs> OnUserJoinedEvent;
        #endregion Events

        private WebSocket m_Client;
        public IRCCredentials Credentials { get; protected set; }
        private List<string> m_sChannelsJoined;

        private void Initialize()
        {
            m_Client = new WebSocket($"ws://{Credentials.TwitchHost}:{Credentials.TwitchPort}");
            m_Client.OnOpen += OnClientConnected;
            m_Client.OnMessage += OnClientReceivedMessage;
            m_Client.OnClose += OnClientDisconnected;
            m_Client.OnError += OnClientError;
            m_sChannelsJoined = new List<string>();
            m_bInitialized = true;
        }

        private void Cleanup()
        {
            m_Client.OnOpen    -= OnClientConnected;
            m_Client.OnMessage -= OnClientReceivedMessage;
            m_Client.OnClose   -= OnClientDisconnected;
            m_Client.OnError   -= OnClientError;
        }

        private TwitchClient(IRCCredentials twitchCredentials)
        {
            m_bInitialized = false;
            if (twitchCredentials == null)
            {
                return;   
            }
            else
            {
                if (!twitchCredentials.Valid)
                {
                    return;
                }
            }

            Credentials = twitchCredentials;
        }

        #region Client Connection Functions / Connection Event Handlers
        private void OnClientConnected (object sender, object e)
        {
            m_Client.Send(IRCHelper.Pass(Credentials.TwitchOAuth));
            m_Client.Send(IRCHelper.Nick(Credentials.TwitchUsername));
            m_Client.Send(IRCHelper.User(Credentials.TwitchUsername, 0));

            m_Client.Send(IRCHelper.Membership);
            m_Client.Send(IRCHelper.Command);
            m_Client.Send(IRCHelper.Tags);
        }
        private void OnClientDisconnected(object sender, CloseEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void OnClientError(object sender, ErrorEventArgs e)
        {
            Console.WriteLine(e.Message);
            //Reconnect();
            //System.Threading.Thread.Sleep(2000);
        }

        public void Connect()
        {
            if (!m_bInitialized)
            {
                Initialize();
                m_Client.Connect();
            }  
        }
        public void Disconnect()
        {
            Task.Factory.StartNew(() => 
            {
                for(int i = 0; i < m_sChannelsJoined.Count; ++i)
                {
                    LeaveChannel(m_sChannelsJoined[i]);
                }
                Cleanup();
                m_Client.Close();
                ((IDisposable)m_Client).Dispose();
                m_bInitialized = false;
            });
        }
        #endregion Client Connection Functions / Connection Event Handlers


        private void OnClientReceivedMessage(object sender, MessageEventArgs e)
        {
            string[] sSeparators = new string[] { "\r\n" };
            string[] sLines = e.Data.Split(sSeparators, StringSplitOptions.None);

            foreach (string line in sLines)
            {
                if (line.Length > 1)
                {
                    if (e.IsText)
                    {
                        //Console.WriteLine(line);
                        ParseResponse(line);
                    }
                }
            }
        }

        public void JoinChannel(string sChannel)
        {
            sChannel = sChannel.ToLower();
            if (!m_sChannelsJoined.Contains(sChannel))
            {
                m_Client.Send(IRCHelper.Join(sChannel));
            }
        }
        public void LeaveChannel(string sChannel)
        {
            sChannel = sChannel.ToLower();
            if (m_sChannelsJoined.Contains(sChannel))
            {
                m_Client.Send(IRCHelper.Part(sChannel));
            }
        }

        private void ParseResponse(string sIRCRaw)
        {
            Console.WriteLine(sIRCRaw);
            string sMessageType;

            foreach (string sChannel in m_sChannelsJoined)
            {
                sMessageType = IRCParser.GetMessageType(sIRCRaw, sChannel);

                if (!string.IsNullOrEmpty(sMessageType))
                {
                    //Console.WriteLine($"Message Type: {sMessageType}");

                    #region USERNOTICE
                    if (sMessageType == "USERNOTICE")
                    {
                        UserNotice usernotice = new UserNotice(sIRCRaw);

                        #region Detect Subscriber

                        if (usernotice.MessageID != UserNotice.SubscriptionType.None)
                        {
                            OnSubscriberEventArgs subEvent = new OnSubscriberEventArgs(usernotice);
                            OnSubscriberEvent?.Invoke(this, subEvent);
                            return;
                        }
                        #endregion Detect Subscriber

                    }
                    #endregion USERNOTICE
                    #region PRIVMSG
                    else if (sMessageType == "PRIVMSG")
                    {
                        ChatMessage chatMessage = new ChatMessage(sIRCRaw);
                        OnChatMessageReceivedEventArgs onChatReceivedEvent = new OnChatMessageReceivedEventArgs(chatMessage);
                        OnChatMessageReceived?.Invoke(this, onChatReceivedEvent);
                        return;
                    }
                    #endregion PRIVMSG
                    #region PART
                    else if (sMessageType == "PART")
                    {
                        // USER LEFT CHANNEL
                        //:tucxbot!tucxbot@tucxbot.tmi.twitch.tv PART #sirtucx
                        string sUsername = sIRCRaw.Substring(1, sIRCRaw.IndexOf('!') - 1);
                        OnUserLeaveEventArgs onUserLeaveEventArgs = new OnUserLeaveEventArgs(sUsername, sChannel);
                        OnUserLeaveEvent?.Invoke(this, onUserLeaveEventArgs);

                        if (sUsername == Credentials.TwitchUsername && m_sChannelsJoined.Contains(sChannel))
                        {
                            m_sChannelsJoined.Remove(sChannel);
                        }
                        return;
                    }
                    #endregion PART
                    #region MODE
                    else if (sMessageType == "MODE")
                    {
                        if (sIRCRaw.Contains(" "))
                        {
                            string[] sModeSplit = sIRCRaw.Split(' ');

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
                        }
                    }
                    #endregion MODE
                    #region NOTICE
                    else if (sMessageType == "NOTICE")
                    {
                        if (sIRCRaw.Contains("Improperly formatted auth"))
                        {
                            // TODO: AUTH ERROR
                        }
                        else if (sIRCRaw.Contains("has gone offline"))
                        {
                            // TODO: HOST LEFT
                        }
                        else if (sIRCRaw.Contains("The moderators of this room are:"))
                        {
                            // TODO: LIST ALL MODERATORS
                        }
                        else if (sIRCRaw.Contains("Your color has been changed."))
                        {
                            // TODO: DETECT COLOR CHANGE
                        } 
                    }
                    #endregion NOTICE
                    #region ROOMSTATE
                    else if (sMessageType == "ROOMSTATE")
                    {
                        // TODO: ROOM STATE CHANGED
                    }
                    #endregion ROOMSTATE
                    #region USERSTATE
                    else if (sMessageType == "USERSTATE")
                    {
                        // USER JOINED CHANNEL
                        // Example:  @badges=staff/1;color=#0D4200;display-name=ronni;emote-sets=0,33,50,237,793,2126,3517,4578,5569,9400,10337,12239;mod=1;subscriber=1;turbo=1;user-type=staff :tmi.twitch.tv USERSTATE #dallas
                        UserState userState = new UserState(sIRCRaw);
                        OnUserJoinedEventArgs onUserJoinedEventArgs = new OnUserJoinedEventArgs(userState);
                        OnUserJoinedEvent?.Invoke(this, onUserJoinedEventArgs);
                    }
                    #endregion USERSTATE
                    #region CLEARCHAT
                    else if (sMessageType == "CLEARCHAT")
                    {
                        // TODO: HANDLE BANS/TIMEOUTS
                    }
                    #endregion CLEARCHAT
                }
            }
 
            #region Non-Channel Messages
            #region PING
            if (sIRCRaw.Contains(IRCHelper.Ping))
            {
                m_Client.Send("PONG :tmi.twitch.tv");
            }
            #endregion PING

            sMessageType = IRCParser.GetMessageType(sIRCRaw, "");
            //Console.WriteLine($"Other Message Type: {sMessageType}");

            #region WHISPER
            if (sMessageType == "WHISPER")
            {
                WhisperMessage whisperMessage = new WhisperMessage(sIRCRaw);
                OnWhisperMessageReceivedEventArgs whisperEvent = new OnWhisperMessageReceivedEventArgs(whisperMessage);
                OnWhisperMessageReceived?.Invoke(this, whisperEvent);
            }
            #endregion WHISPER

            #region Bot joined channel
            if (sMessageType == Credentials.TwitchUsername)
            {
                if (sIRCRaw.Contains($"{Credentials.TwitchUsername}.tmi.twitch.tv"))
                {
                    string sUserConfirmation = sIRCRaw.Substring(1, sIRCRaw.IndexOf('.') - 1);
                    if (sUserConfirmation == Credentials.TwitchUsername)
                    {
                        string sMessageContents = sIRCRaw.Substring(sIRCRaw.IndexOf(':', 1) + 1, sIRCRaw.Length - (sIRCRaw.IndexOf(':', 1) + 1));

                        #region On Bot Joined Channel
                        //:tucxbot.tmi.twitch.tv 366 tucxbot #sirtucx :End of /NAMES list
                        if (sMessageContents == "End of /NAMES list")
                        {
                            string sChannel = sIRCRaw.Substring(sIRCRaw.IndexOf('#') + 1, sIRCRaw.IndexOf(" :") - ((sIRCRaw.IndexOf('#') + 1)));
                            OnBotJoinedChannelEvent?.Invoke(this, new OnBotJoinedChannelEventArgs(sChannel));
                            m_sChannelsJoined.Add(sChannel);
                        }
                        #endregion On Bot Joined Channel
                    }
                }
            }
            #endregion Bot joined channel

            #region Bot connected to twitch
            if (sMessageType == Credentials.TwitchUsername)
            {
                if (sIRCRaw.Contains("You are in a maze of twisty passages, all alike."))
                {
                    // LOGIN SUCCESSFUL
                    OnTwitchConnected?.Invoke();
                }
            }
            #endregion Bot connected to twitch

            #region NOTICE
            if (sMessageType == "NOTICE")
            {
                if (sIRCRaw.Contains("Login authentication failed"))
                {
                    // LOGIN FAILED
                    OnTwitchLoginFailed?.Invoke();
                }
            }
            #endregion NOTICE

             

            #endregion
        }

        public void SendChatMessage(string sChannel, string sMessage)
        {
            m_Client.Send(IRCHelper.Privmsg(sChannel, sMessage));
        }
        public void SendWhisperMessage(string sUsername, string sMessage)
        {
            m_Client.Send(IRCHelper.Whisper(sUsername, sMessage));
        }
    }
}
