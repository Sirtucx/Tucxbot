namespace TucxbotForm
{
    using System;
    using System.Collections.Generic;
    using Twitch.Core;
    using Twitch.Mods;
    using Listeners;
    
    public class ModManager<TModType, TModListener> : IModHandler
        where TModType : IMod
        where TModListener : IModListener
    {
        private readonly Dictionary<string, TModType> m_modLibrary;
        private TModListener m_modListener;

        public ModManager(ITwitchClient twitchClient)
        {
            m_modLibrary = new Dictionary<string, TModType>();
            m_modListener = (TModListener) Activator.CreateInstance(typeof(TModListener), twitchClient);
            m_modListener.RegisterEvents();
            m_modListener.OnInputReceived += HandleInput;
        }

        public void LoadMod(string modName, Type modType)
        {
            if (modType != null && typeof(TModType).IsAssignableFrom(modType) && !m_modLibrary.ContainsKey(modName))
            {
                TModType mod = (TModType)modType.GetConstructor(new Type[] { })?.Invoke(null);
                m_modLibrary.Add(modName, mod);
            }
        }

        public void UnloadMod(string modName)
        {
            if (m_modLibrary.ContainsKey(modName))
            {
                TModType mod = m_modLibrary[modName];
                m_modLibrary.Remove(modName);
                mod.Shutdown();
            }
        }

        public void HandleInput(object[] parameters)
        {
            foreach (string key in m_modLibrary.Keys)
            {
                m_modLibrary[key].Process(parameters);
            }
        }

        public virtual void Shutdown()
        {
            m_modListener.OnInputReceived -= HandleInput;
            m_modListener.UnregisterEvents();
            foreach (KeyValuePair<string,TModType> keyValuePair in m_modLibrary)
            {
                keyValuePair.Value.Shutdown();
            }
            m_modLibrary.Clear();
        }
    }
}