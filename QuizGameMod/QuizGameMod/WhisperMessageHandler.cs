namespace QuizGameMod
{
    using Twitch.Mods;
    using Twitch.Containers;
    
    public partial class WhisperMessageHandler : WhisperMessageMod
    {
        protected override void ProcessWhisperMessage(WhisperMessage whisperMessage)
        {
            GameController.Instance.ProcessWhisperMessage(whisperMessage);
        }

        public override void Shutdown()
        {
        }
    }
}