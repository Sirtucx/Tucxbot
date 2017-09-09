using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TwitchIRC_TCP
{
    public class IRCConnection
    {
        public delegate void TwitchClientThread();
        public StreamReader Input
        {
            get
            {
                return m_ClientInputStream;
            }
        }
        public StreamWriter Output
        {
            get
            {
                return m_ClientOutputStream;
            }
        }
        public bool Initialized
        {
            get
            {
                return m_bInitialized;
            }
            set
            {
                m_bInitialized = value;
            }
        }

        private TcpClient m_TCPClient;                  // Connection to Twitch IRC Servers
        private NetworkStream m_ClientNetworkStream;    // Data Stream for the TCPClient
        private StreamReader m_ClientInputStream;       // Read Stream from the NetworkStream
        private StreamWriter m_ClientOutputStream;      // Write Stream to the NetworkStream
        private Thread m_ClientThread;                  // Thread to handle all of the reading
        private string m_sUsername, m_sOAuthKey;        // Username & OAuth Key
        private bool m_bInitialized;                    // Whether we have a successful connection

        public IRCConnection(string sIRCServer, int iPortNumber, string sEncoding, string sUsername, string sOAuthKey, TwitchClientThread threadFunction)
        {
            m_TCPClient = new TcpClient(sIRCServer, iPortNumber);
            m_ClientNetworkStream = m_TCPClient.GetStream();
            m_ClientInputStream = new StreamReader(m_ClientNetworkStream, Encoding.GetEncoding(sEncoding));
            m_ClientOutputStream = new StreamWriter(m_ClientNetworkStream, Encoding.GetEncoding(sEncoding));

            m_ClientThread = new Thread(new ThreadStart(threadFunction));
            m_ClientThread.Name = "Twitch IRC Thread: " + sIRCServer;
            m_sUsername = sUsername;
            m_sOAuthKey = sOAuthKey;
            Initialized = false;
        }

        public void Write(string sMessage)
        {
            Output.WriteLine(sMessage);
            Output.Flush();
        }
        public string Read()
        {
            return Input.ReadLine();
        }

        public bool Start()
        {
            if (!m_ClientThread.IsAlive)
            {
                Console.WriteLine("IRCConnection starting thread: " + m_ClientThread.Name);
                m_ClientThread.Start();
                StartConnection();
                return true;
            }
            return false;
        }
        public void StartConnection()
        {
            Console.WriteLine("Connecting to Twitch");
            Write("USER " + m_sUsername + "tmi twitch :" + m_sUsername);
            Write("PASS " + m_sOAuthKey);
            Write("NICK " + m_sUsername.ToLower());
        }
        public void CloseConnection()
        {
            Console.WriteLine("Disconnecting from Twitch");
            Write("QUIT\n");
        }

        public void Close()
        {
            Output.Close();
            Input.Close();
            m_ClientNetworkStream.Close();
            m_TCPClient.Close();
        }

    }
}
