namespace Twitch.Mods
{
    using Containers;
    
    public abstract class ChatMessageMod : IMod
    {
        protected abstract void ProcessChatMessage(ChatMessage chatMessage);

        public void Process(params object[] parameters)
        {
            ProcessChatMessage((ChatMessage)parameters[0]);
        }
        public abstract void Shutdown();
    }
}