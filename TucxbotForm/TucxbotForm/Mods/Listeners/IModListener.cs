namespace TucxbotForm.Listeners
{
    using System;

    public interface IModListener
    {
        event Action<object[]> OnInputReceived;
        void RegisterEvents();
        void UnregisterEvents();
    }
}