namespace Twitch.Interfaces
{
    using Containers;
    
    public interface IWhisperMessageMod
    {
        void Process(WhisperMessage whisperMessage);
        void Shutdown();
    }
}
