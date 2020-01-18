namespace Twitch.Mods
{
    using Containers;
    
    public abstract class SubscriberMod : IMod
    {
        protected abstract void ProcessChatMessage(UserNotice subNotice);

        public void Process(params object[] parameters)
        {
            ProcessChatMessage((UserNotice)parameters[0]);
        }
        public abstract void Shutdown();
    }
}