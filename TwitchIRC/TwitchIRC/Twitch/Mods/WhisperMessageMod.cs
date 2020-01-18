namespace Twitch.Mods
{
    using Containers;

    public abstract class WhisperMessageMod : IMod
    {
        protected abstract void ProcessChatMessage(WhisperMessage chatMessage);

        public void Process(params object[] parameters)
        {
            ProcessChatMessage((WhisperMessage)parameters[0]);
        }
        public abstract void Shutdown();
    }
}