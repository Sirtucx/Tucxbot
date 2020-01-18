namespace TucxbotForm
{
    using System;
    
    public interface IModHandler
    {
        void LoadMod(string modName, Type modType);
        void UnloadMod(string modName);
        void HandleInput(object[] parameters);
        void Shutdown();
    }
}