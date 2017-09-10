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

                if (!m_bInitialized)
                {
                    m_Instance = null;
                }
            }
            return m_Instance;
        }
        #endregion Singleton

        public Action OnTwitchConnected;

        #region Events
        public event EventHandler<OnSubscriberEventArgs> OnSubscriberEvent;
        public event EventHandler<OnChatMessageReceivedEventArgs> OnChatMessageReceived;
        public event EventHandler<OnWhisperMessageReceivedEventArgs> OnWhisperMessageReceived;
        #endregion Events

        private WebSocket m_Client;
        public IRCCredentials Credentials { get; protected set; }
        private List<string> m_sChannelsJoined;

        private void Initialize()
        {
            m_sChannelsJoined = new List<string>();
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
            m_Client = new WebSocket($"ws://{Credentials.TwitchHost}:{Credentials.TwitchPort}");
            m_Client.OnOpen += OnClientConnected;
            m_Client.OnMessage += OnClientReceivedMessage;
            m_Client.OnClose += OnClientDisconnected;
            m_Client.OnError += OnClientError;
            Initialize();
            m_bInitialized = true;
        }

        #region Client Connection Functions / Connection Event Handlers
        private void OnClientConnected (object sender, object e)
        {
            OnTwitchConnected?.Invoke();
            m_Client.Send(IRCHelper.Pass(Credentials.TwitchOAuth));
            m_Client.Send(IRCHelper.Nick(Credentials.TwitchUsername));
            m_Client.Send(IRCHelper.User(Credentials.TwitchUsername, 0));

            m_Client.Send(IRCHelper.Membership);
            m_Client.Send(IRCHelper.Command);
            m_Client.Send(IRCHelper.Tags);
        }
        private void OnClientDisconnected(object sender, CloseEventArgs e)
        {

        }
        private void OnClientError(object sender, ErrorEventArgs e)
        {
            Reconnect();
            System.Threading.Thread.Sleep(2000);
        }

        public void Connect()
        {
            m_Client.Connect();
        }
        public void Disconnect()
        {
            Task.Factory.StartNew(() => { m_Client.Close(); });
        }
        public void Reconnect()
        {
            if (m_Client.IsAlive)
            {
                m_Client.Close();
            }
            m_Client.Connect();
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
                m_sChannelsJoined.Add(sChannel);
                m_Client.Send(IRCHelper.Join(sChannel));
            }
        }
        public void LeaveChannel(string sChannel)
        {
            sChannel = sChannel.ToLower();
            if (m_sChannelsJoined.Contains(sChannel))
            {
                m_sChannelsJoined.Remove(sChannel);
                m_Client.Send(IRCHelper.Part(sChannel));
            }
        }

        private void ParseResponse(string sIRCRaw)
        {
            //Console.WriteLine(sIRCRaw);
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
                    #region JOIN
                    else if (sMessageType == "JOIN")
                    {
                        // TODO: USER JOINED CHANNEL
                    }
                    #endregion JOIN
                    #region PART
                    else if (sMessageType == "PART")
                    {
                        // TODO: USER LEFT CHANNEL
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
                        // TODO: USER STATE CHANGED
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
                m_Client.Send("PONG");
            }
            #endregion PING

            sMessageType = IRCParser.GetMessageType(sIRCRaw, "");
            
            #region WHISPER
            if (sMessageType == "WHISPER")
            {
                WhisperMessage whisperMessage = new WhisperMessage(sIRCRaw);
                OnWhisperMessageReceivedEventArgs whisperEvent = new OnWhisperMessageReceivedEventArgs(whisperMessage);
                OnWhisperMessageReceived?.Invoke(this, whisperEvent);
            }
            #endregion WHISPER

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
