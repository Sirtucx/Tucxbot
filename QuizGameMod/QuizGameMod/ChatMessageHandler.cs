namespace QuizGameMod
{
    using Twitch.Mods;
    using Twitch.Containers;
    
    public partial class ChatMessageHandler : ChatMessageMod
    {
        protected override void ProcessChatMessage(ChatMessage chatMessage)
        {
            GameController.Instance.ProcessChatMessage(chatMessage);
        }

        public override void Shutdown()
        {
            GameController.Instance.Shutdown();
        }
    }
}