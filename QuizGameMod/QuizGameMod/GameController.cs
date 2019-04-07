using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twitch_Websocket;
using Twitch_Websocket.Mod_Interfaces;
using System.Threading;
using Microsoft.VisualBasic;
using System.Windows.Forms;


namespace QuizGameMod
{
    public class GameController : IWhisperMessageMod, IChatMessageMod
    {
        public Action<string, string, int> OnWinnerAchieved;

        protected const string m_sSubscribeCommand = "!game_sub";
        protected const string m_sUnsubscribeCommand = "!game_unsub";
        protected const string m_sAddGameCommand = "!game_add";
        protected const string m_sRemoveGameCommand = "!game_remove";
        protected const string m_sUnsubscribeAllCommand = "!game_unsub_all";

        protected const int m_TickRate = 333;
        protected const int m_HintCooldown = 30000;
        protected const int m_GameCooldown = 150000;

        protected QuizContentLoader m_ContentLoader;
        protected Dictionary<string, QuizGame> m_QuizGames;
        protected Dictionary<string, List<string>> m_SubscribedUsers;
        protected Thread m_MainThread;
        protected bool m_bIsActive;
        protected object m_LockObject;

        public GameController ()
        {
            m_QuizGames = new Dictionary<string, QuizGame>();
            m_SubscribedUsers = new Dictionary<string, List<string>>();
            LoadData();

            if (m_bIsActive)
            {
                m_MainThread = new Thread(MainThreadFunction);
                m_LockObject = new object();
                m_MainThread.Start();
            }
        }

        protected void LoadData()
        {
            m_bIsActive = false;

            string sMessage = "Please enter how you want your quiz content to be loaded: ";
            string[] sLoadTypes = Enum.GetNames(typeof(QuizContentLoader.LoadType));
            for (int i = 0; i < sLoadTypes.Length; ++i)
            {
                sMessage += $"\nFor {sLoadTypes[i]}, type {i}";
            }


            string sResponse = Interaction.InputBox(sMessage, "Select Content Load Type");
            QuizContentLoader.LoadType contentLoadType;

            if (Enum.TryParse<QuizContentLoader.LoadType>(sResponse, out contentLoadType))
            {
                m_ContentLoader = new QuizContentLoader(contentLoadType);
            }

            m_bIsActive = m_ContentLoader != null ? m_ContentLoader.LoadedContent : false;
        }

        public virtual void Process(WhisperMessage whisperMessage)
        {
            // !game_unsub_all
            if (CheckCommand(m_sUnsubscribeAllCommand, whisperMessage.Message))
            {
                if (m_SubscribedUsers.ContainsKey(whisperMessage.DisplayName))
                {
                    m_SubscribedUsers.Remove(whisperMessage.DisplayName);
                }
            }

            // Submitting a guess
            else
            {
                if (m_SubscribedUsers.ContainsKey(whisperMessage.DisplayName))
                {
                    for (int i  = 0; i < m_SubscribedUsers[whisperMessage.DisplayName].Count; ++i)
                    {
                        if (m_QuizGames.ContainsKey(m_SubscribedUsers[whisperMessage.DisplayName][i]))
                        {
                            // User has guessed correctly
                            if (m_QuizGames[m_SubscribedUsers[whisperMessage.DisplayName][i]].CheckAnswer(whisperMessage.Message))
                            {
                                OnWinnerAchieved?.Invoke(m_SubscribedUsers[whisperMessage.DisplayName][i], whisperMessage.DisplayName, m_QuizGames[m_SubscribedUsers[whisperMessage.DisplayName][i]].Winners.Count + 1);

                                // If there are any remaining winner slots left?
                                if (m_QuizGames[m_SubscribedUsers[whisperMessage.DisplayName][i]].IsQualifiedWinner())
                                {
                                    m_QuizGames[m_SubscribedUsers[whisperMessage.DisplayName][i]].AddWinner(whisperMessage.DisplayName);

                                    // If there are no more qualified winner slots
                                    if (!m_QuizGames[m_SubscribedUsers[whisperMessage.DisplayName][i]].IsQualifiedWinner())
                                    {
                                        // Display a message in the channel the game was "played" on, showcasing the winners
                                        SendEndGameMessage(m_SubscribedUsers[whisperMessage.DisplayName][i], m_QuizGames[m_SubscribedUsers[whisperMessage.DisplayName][i]].Winners);

                                        // End the game
                                        m_QuizGames[m_SubscribedUsers[whisperMessage.DisplayName][i]].SetGameCooldown(m_GameCooldown);
                                        m_QuizGames[m_SubscribedUsers[whisperMessage.DisplayName][i]].EndGame();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public virtual void Process(ChatMessage chatMessage)
        {
            // !game_sub
            if (CheckCommand(m_sSubscribeCommand, chatMessage.Message))
            {
                if (!m_SubscribedUsers.ContainsKey(chatMessage.DisplayName))
                {
                    m_SubscribedUsers.Add(chatMessage.DisplayName, new List<string>());
                }
                m_SubscribedUsers[chatMessage.DisplayName].Add(chatMessage.Channel);


                if (m_QuizGames.ContainsKey(chatMessage.Channel))
                {
                    if (!m_QuizGames[chatMessage.Channel].ReadyForNewGame())
                    {
                        // Tell the new subscribed player the current instruction
                        TwitchClient.GetInstance().SendWhisperMessage(chatMessage.DisplayName, GetGameHintMessage(chatMessage.Channel));
                    }
                }
            }

            // !game_unsub
            else if (CheckCommand(m_sUnsubscribeCommand, chatMessage.Message))
            {
                if (m_SubscribedUsers.ContainsKey(chatMessage.DisplayName))
                {
                    if (m_SubscribedUsers[chatMessage.DisplayName].Contains(chatMessage.Channel))
                    {
                        m_SubscribedUsers[chatMessage.DisplayName].Remove(chatMessage.Channel);
                    }
                }
            }

            // !game_add
            else if (CheckCommand(m_sAddGameCommand, chatMessage.Message) && chatMessage.Mod)
            {
                if (!m_QuizGames.ContainsKey(chatMessage.Channel))
                {
                    m_QuizGames.Add(chatMessage.Channel, new QuizGame());
                }
            }

            // !game_remove
            else if (CheckCommand(m_sRemoveGameCommand, chatMessage.Message) && chatMessage.Mod)
            {
                if (m_QuizGames.ContainsKey(chatMessage.Channel))
                {
                    m_QuizGames[chatMessage.Channel].EndGame();
                    m_QuizGames.Remove(chatMessage.Channel);
                }
            }
        }

        public virtual void Shutdown()
        {
            foreach(KeyValuePair<string, QuizGame> kvp in m_QuizGames)
            {
                kvp.Value.EndGame();
            }
            m_QuizGames.Clear();
            m_SubscribedUsers.Clear();
            m_bIsActive = false;
        }

        protected bool CheckCommand(string sCommand, string sMessage)
        {
            if (sMessage.Length >= sCommand.Length)
            {
                if (sMessage.Substring(0, sCommand.Length).ToLower() == sCommand)
                {
                    return true;
                }
            }
            return false;
        }

        protected void MainThreadFunction()
        {
            while (m_bIsActive)
            {
                lock(m_LockObject)
                {
                    foreach(KeyValuePair<string, QuizGame> kvp in m_QuizGames)
                    {
                        kvp.Value.Tick(m_TickRate);

                        if (kvp.Value.ReadyForNewGame())
                        {
                            // Get a new question for this quiz game.
                            QuizGame_Info newGameQuestion = GetNewQuestion(kvp.Key);

                            // Set the hint cooldown
                            kvp.Value.SetHintCooldown(m_HintCooldown);

                            // Set the quiz up
                            kvp.Value.StartNewGame(newGameQuestion);

                            // Send message in channel
                            SendNewGameMessage(kvp.Key);
                        }

                        if (kvp.Value.ReadyForHint())
                        {
                            foreach (KeyValuePair<string, List<string>> subbedUsers in m_SubscribedUsers)
                            {
                                if (subbedUsers.Value.Contains(kvp.Key))
                                {
                                    TwitchClient.GetInstance().SendWhisperMessage(subbedUsers.Key, GetGameHintMessage(kvp.Key));
                                }
                            }

                            kvp.Value.SetHintCooldown(m_HintCooldown);
                        }
                    }
                }
                Thread.Sleep(m_TickRate);
            }
        }

        protected string GetGameHintMessage(string sChannel)
        {
            string sMessage = "";

            sMessage = $"{m_QuizGames[sChannel].Instructions} {m_QuizGames[sChannel].GetHint()}";

            return sMessage;
        }

        protected void SendNewGameMessage(string sChannel)
        {
            string sMessage = $"A new game will be starting in {m_HintCooldown / 1000} seconds. If you are not subbed to whispers from {TwitchClient.GetInstance().Credentials.TwitchUsername}'s whispers, type {m_sSubscribeCommand} to be notified and receive questions/hints. To unsubcribe from notifications type {m_sUnsubscribeCommand} in chat, or whisper {m_sUnsubscribeAllCommand} to {TwitchClient.GetInstance().Credentials.TwitchUsername} to unsubscribe from all games you are notified for.";
            TwitchClient.GetInstance().SendChatMessage(sChannel, sMessage);
        }

        protected void SendEndGameMessage(string sChannel, List<string> winners)
        {
            string sMessage = "Congrats to ";

            for (int i = 0; i < winners.Count; ++i)
            {
                sMessage += $"{(i + 1)}. @{winners[i]} ";
            }

            sMessage += $"for correctly guessing ({m_QuizGames[sChannel].Answer}).";

            TwitchClient.GetInstance().SendChatMessage(sChannel, sMessage);
        }

        protected virtual QuizGame_Info GetNewQuestion(string sChannel)
        {
            return m_ContentLoader.GetNewQuestion(sChannel);
        }
    }
}
