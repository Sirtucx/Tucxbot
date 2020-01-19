namespace QuizGameMod
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Microsoft.VisualBasic;
    using Twitch.Containers;
    using Twitch.Core;
    
    public class GameController
    {
        public Action<string, string, int> OnWinnerAchieved;

        private static GameController instance;

        private const string SUBSCRIBE_COMMAND = "!game_sub";
        private const string UNSUBSCRIBE_COMMAND = "!game_unsub";
        private const string ADD_GAME_COMMAND = "!game_add";
        private const string REMOVE_GAME_COMMAND = "!game_remove";
        private const string UNSUBSCRIBE_ALL_COMMAND = "!game_unsub_all";
        private const int TICK_RATE = 333;
        private const int HINT_COOLDOWN = 30000;
        private const int GAME_COOLDOWN = 150000;

        private QuizContentLoader m_contentLoader;
        private Thread m_mainThread;
        private bool m_isActive;
        
        private readonly object m_lockObject;
        private readonly Dictionary<string, QuizGame> m_quizGames;
        private readonly Dictionary<string, List<string>> m_subscribedUsers;
        
        public static GameController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameController();
                }

                return instance;
            }
        }

        private GameController ()
        {
            m_quizGames = new Dictionary<string, QuizGame>();
            m_subscribedUsers = new Dictionary<string, List<string>>();
            LoadData();

            if (m_isActive)
            {
                m_mainThread = new Thread(MainThreadFunction);
                m_lockObject = new object();
                m_mainThread.Start();
            }
        }

        private void LoadData()
        {
            m_isActive = false;

            string message = "Please enter how you want your quiz content to be loaded: ";
            string[] loadTypes = Enum.GetNames(typeof(QuizContentLoader.LoadType));
            for (int i = 0; i < loadTypes.Length; ++i)
            {
                message += $"\nFor {loadTypes[i]}, type {i}";
            }
            
            string response = Interaction.InputBox(message, "Select Content Load Type");

            if (Enum.TryParse(response, out QuizContentLoader.LoadType contentLoadType))
            {
                m_contentLoader = new QuizContentLoader(contentLoadType);
            }

            m_isActive = m_contentLoader?.LoadedContent ?? false;
        }
        public void Shutdown()
        {
            foreach(KeyValuePair<string, QuizGame> kvp in m_quizGames)
            {
                kvp.Value.EndGame();
            }
            m_quizGames.Clear();
            m_subscribedUsers.Clear();
            m_isActive = false;
            m_mainThread = null;
        }

        public virtual void ProcessWhisperMessage(WhisperMessage whisperMessage)
        {
            // !game_unsub_all
            if (CheckCommand(UNSUBSCRIBE_ALL_COMMAND, whisperMessage.Message))
            {
                UnsubscribeUserToGame(whisperMessage.Username);
            }

            // Submitting a guess
            else
            {
                if (!HasUserSubmittedACorrectAnswer(whisperMessage.DisplayName, whisperMessage.Message, out string channelName))
                {
                    return;
                }
                
                OnWinnerAchieved?.Invoke(channelName, whisperMessage.DisplayName, m_quizGames[channelName].Winners.Count + 1);

                // If there are any remaining winner slots left?
                if (!m_quizGames[channelName].IsQualifiedWinner())
                {
                    return;
                }
                
                m_quizGames[channelName].AddWinner(whisperMessage.DisplayName);

                // If there are no more qualified winner slots
                if (m_quizGames[channelName].IsQualifiedWinner())
                {
                    return;
                }
                
                // Display a message in the channel the game was "played" on, showcasing the winners
                SendEndGameMessage(channelName,
                    m_quizGames[channelName].Winners);

                // End the game
                m_quizGames[channelName].SetGameCooldown(GAME_COOLDOWN);
                m_quizGames[channelName].EndGame();
            }
        }

        private bool HasUserSubmittedACorrectAnswer(string username, string whisperMessage, out string channelName)
        {
            if (m_subscribedUsers.ContainsKey(username))
            {
                for (int i = 0; i < m_subscribedUsers[username].Count; ++i)
                {
                    if (!m_quizGames.ContainsKey(m_subscribedUsers[username][i]) || !m_quizGames[m_subscribedUsers[username][i]].CheckAnswer(whisperMessage))
                    {
                        continue;
                    }
                    
                    // User has guessed correctly
                    channelName = m_subscribedUsers[username][i];
                    return true;
                }
            }

            channelName = "";
            return false;
        }
        
        public virtual void ProcessChatMessage(ChatMessage chatMessage)
        {
            // !game_sub
            if (CheckCommand(SUBSCRIBE_COMMAND, chatMessage.Message))
            {
                SubscribeUserToGame(chatMessage.DisplayName, chatMessage.Channel);
            }

            // !game_unsub
            else if (CheckCommand(UNSUBSCRIBE_COMMAND, chatMessage.Message))
            {
                UnsubscribeUserToGame(chatMessage.DisplayName, chatMessage.Channel);
            }

            else if (chatMessage.Mod)
            {
                // !game_add
                if (CheckCommand(ADD_GAME_COMMAND, chatMessage.Message))
                {
                    AddNewQuizGameToChannel(chatMessage.Channel);
                }

                // !game_remove
                else if (CheckCommand(REMOVE_GAME_COMMAND, chatMessage.Message))
                {
                    RemoveQuizGameFromChannel(chatMessage.Channel);
                }
            }
        }
        
        private void SubscribeUserToGame(string username, string channelName)
        {
            if (!m_subscribedUsers.ContainsKey(username))
            {
                m_subscribedUsers.Add(username, new List<string>());
            }
            m_subscribedUsers[username].Add(channelName);


            if (m_quizGames.ContainsKey(channelName))
            {
                if (!m_quizGames[channelName].ReadyForNewGame())
                {
                    // Tell the new subscribed player the current instruction
                    TwitchClient.GetInstance().SendWhisperMessage(username, GetGameHintMessage(channelName));
                }
            }
        }
        private void UnsubscribeUserToGame(string username, string channelName = "")
        {
            if (!m_subscribedUsers.ContainsKey(username))
            {
                return;
            }
            
            if (m_subscribedUsers[username].Contains(channelName))
            {
                m_subscribedUsers[username].Remove(channelName);
            }
            else if (string.IsNullOrEmpty(channelName))
            {
                m_subscribedUsers.Remove(username);
            }
        }
        private void AddNewQuizGameToChannel(string channelName)
        {
            if (!m_quizGames.ContainsKey(channelName))
            {
                m_quizGames.Add(channelName, new QuizGame());
            }
        }
        private void RemoveQuizGameFromChannel(string channelName)
        {
            if (m_quizGames.ContainsKey(channelName))
            {
                m_quizGames[channelName].EndGame();
                m_quizGames.Remove(channelName);
            }
        }
        private bool CheckCommand(string sCommand, string sMessage)
        {
            if (sMessage.Length < sCommand.Length)
            {
                return false;
            }
            
            return sMessage.Substring(0, sCommand.Length).ToLower() == sCommand;
        }
        private void MainThreadFunction()
        {
            while (m_isActive)
            {
                lock(m_lockObject)
                {
                    foreach(KeyValuePair<string, QuizGame> kvp in m_quizGames)
                    {
                        kvp.Value.Tick(TICK_RATE);

                        PrepareNewGameIfReady(kvp.Key, kvp.Value);
                        SendHintToSubbedUsers(kvp.Key, kvp.Value);
                    }
                }
                Thread.Sleep(TICK_RATE);
            }
        }
        private void PrepareNewGameIfReady(string channelName, QuizGame quizGame)
        {
            if (quizGame.ReadyForNewGame())
            {
                // Get a new question for this quiz game.
                QuizGameInfo newGameQuestion = GetNewQuestion(channelName);

                // Set the hint cooldown
                quizGame.SetHintCooldown(HINT_COOLDOWN);

                // Set the quiz up
                quizGame.StartNewGame(newGameQuestion);

                // Send message in channel
                SendNewGameMessage(channelName);
            }
        }
        private void SendHintToSubbedUsers(string channelName, QuizGame quizGame)
        {
            if (quizGame.ReadyForHint())
            {
                foreach (KeyValuePair<string, List<string>> subbedUsers in m_subscribedUsers)
                {
                    if (subbedUsers.Value.Contains(channelName))
                    {
                        TwitchClient.GetInstance().SendWhisperMessage(subbedUsers.Key, GetGameHintMessage(channelName));
                    }
                }

                quizGame.SetHintCooldown(HINT_COOLDOWN);
            }
        }
        private string GetGameHintMessage(string channelName)
        {
            return $"{m_quizGames[channelName].Instructions} {m_quizGames[channelName].GetHint()}";
        }
        private void SendNewGameMessage(string channelName)
        {
            string sMessage = $"A new game will be starting in {HINT_COOLDOWN / 1000} seconds. If you are not subbed to whispers from {TwitchClient.GetInstance().Credentials.TwitchUsername}'s whispers, type {SUBSCRIBE_COMMAND} to be notified and receive questions/hints. To unsubcribe from notifications type {UNSUBSCRIBE_COMMAND} in chat, or whisper {UNSUBSCRIBE_ALL_COMMAND} to {TwitchClient.GetInstance().Credentials.TwitchUsername} to unsubscribe from all games you are notified for.";
            TwitchClient.GetInstance().SendChatMessage(channelName, sMessage);
        }
        private void SendEndGameMessage(string channelName, List<string> winners)
        {
            string sMessage = "Congrats to ";

            for (int i = 0; i < winners.Count; ++i)
            {
                sMessage += $"{(i + 1)}. @{winners[i]} ";
            }

            sMessage += $"for correctly guessing ({m_quizGames[channelName].Answer}).";

            TwitchClient.GetInstance().SendChatMessage(channelName, sMessage);
        }
        private QuizGameInfo GetNewQuestion(string channelName)
        {
            return m_contentLoader.GetNewQuestion(channelName);
        }
    }
}
