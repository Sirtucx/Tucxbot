namespace Twitch.Mods
{
    using Containers;
    
    public abstract class UserJoinedMod : IMod
    {
        protected abstract void ProcessChatMessage(UserState userState);

        public void Process(params object[] parameters)
        {
            ProcessChatMessage((UserState)parameters[0]);
        }
        public abstract void Shutdown();
    }
}