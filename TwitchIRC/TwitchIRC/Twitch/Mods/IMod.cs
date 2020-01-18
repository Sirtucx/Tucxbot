namespace Twitch.Mods
{
    public interface IMod
    {
        void Process(params object[] parameters);
        void Shutdown();
    }
}