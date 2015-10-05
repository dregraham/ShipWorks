using System;

namespace ShipWorks.Core.Messaging
{
    /// <summary>
    /// Defines a messenger interface
    /// </summary>
    public interface IMessenger
    {
        /// <summary>
        /// Handle a message using the specified handler
        /// </summary>
        MessengerToken Handle<T>(object owner, Action<T> handler) where T : IShipWorksMessage;

        /// <summary>
        /// Handle a message using the specified handler
        /// </summary>
        MessengerToken Handle(object owner, Type messageType, Action<IShipWorksMessage> handler);

        /// <summary>
        /// Get a reference to the messenger as an observable stream of messages
        /// </summary>
        IObservable<T> AsObservable<T>() where T : IShipWorksMessage;

        /// <summary>
        /// Send a message to any listeners
        /// </summary>
        void Send<T>(T message) where T : IShipWorksMessage;

        /// <summary>
        /// Remove a handler based on the token
        /// </summary>
        /// <param name="token"></param>
        void Remove(MessengerToken token);

        /// <summary>
        /// Remove all handlers of a message type for a given owner
        /// </summary>
        void Remove(object owner, Type messageType);
    }
}