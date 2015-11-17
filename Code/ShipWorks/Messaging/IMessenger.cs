using System;

namespace ShipWorks.Core.Messaging
{
    /// <summary>
    /// Defines a messenger interface
    /// </summary>
    public interface IMessenger : IObservable<IShipWorksMessage>
    {
        /// <summary>
        /// Send a message to any listeners
        /// </summary>
        void Send<T>(T message) where T : IShipWorksMessage;
    }
}