using System;

namespace ShipWorks.Core.Messaging
{
    /// <summary>
    /// Defines a messenger interface
    /// </summary>
    public interface IMessenger
    {
        /// <summary>
        /// Get a reference to the messenger as an observable stream of messages
        /// </summary>
        IObservable<T> AsObservable<T>() where T : IShipWorksMessage;

        /// <summary>
        /// Send a message to any listeners
        /// </summary>
        void Send<T>(T message) where T : IShipWorksMessage;
    }
}