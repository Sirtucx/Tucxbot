namespace Twitch.Interfaces
{
    public interface IOnUserLeaveMod
    {
        void Process(string sChannel, string sUsername);
        void Shutdown();
    }
}
