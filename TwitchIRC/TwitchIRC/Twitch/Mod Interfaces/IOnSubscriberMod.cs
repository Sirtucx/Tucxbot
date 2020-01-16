namespace Twitch.Interfaces
{
    using Containers;
    
    public interface IOnSubscriberMod
    {
        void Process(UserNotice userNotice);
        void Shutdown();
    }
}
