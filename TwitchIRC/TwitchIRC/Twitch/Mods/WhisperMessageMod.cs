namespace Twitch.Mods
{
    using Containers;

    public abstract class WhisperMessageMod : IMod
    {
        protected abstract void ProcessWhisperMessage(WhisperMessage whisperMessage);

        public void Process(params object[] parameters)
        {
            ProcessWhisperMessage((WhisperMessage)parameters[0]);
        }
        public abstract void Shutdown();
    }
}