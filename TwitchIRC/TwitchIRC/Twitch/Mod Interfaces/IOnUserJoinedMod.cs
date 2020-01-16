namespace Twitch.Interfaces
{
    using Containers;
    
    public interface IOnUserJoinedMod
    {
        void Process(UserState userState);
        void Shutdown();
    }
}
