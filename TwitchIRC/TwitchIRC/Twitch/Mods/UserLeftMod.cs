namespace Twitch.Mods
{
    using Containers;
    
    public abstract class UserLeftMod : IMod
    {
        protected abstract void ProcessChatMessage(string channelName, string username);

        public void Process(params object[] parameters)
        {
            ProcessChatMessage(parameters[0].ToString(), parameters[1].ToString());
        }
        public abstract void Shutdown();
    }
}