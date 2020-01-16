namespace Twitch.Interfaces
{
    using Containers;
    
    public interface IChatMessageMod
    {
        void Process(ChatMessage chatMessage);
        void Shutdown();
    }
}
