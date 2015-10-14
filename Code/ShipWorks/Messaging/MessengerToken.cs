using System;

namespace ShipWorks.Core.Messaging
{
    /// <summary>
    /// Token that allows the messenger to track handlers
    /// </summary>
    public class MessengerToken : IDisposable
    {
        private readonly IDisposable disposable;

        public MessengerToken(IDisposable disposable)
        {
            this.disposable = disposable;
        }

        public void Dispose()
        {
            disposable?.Dispose();
        }
    }
}