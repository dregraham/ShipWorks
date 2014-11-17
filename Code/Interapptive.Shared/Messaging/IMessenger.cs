﻿using System;

namespace Interapptive.Shared.Messaging
{
    /// <summary>
    /// Defines a messenger interface
    /// </summary>
    public interface IMessenger
    {
        /// <summary>
        /// Handle a message using the specified handler
        /// </summary>
        MessengerToken Handle<T>(Action<T> handler) where T : IShipWorksMessage;

        /// <summary>
        /// Send a message to any listeners
        /// </summary>
        void Send<T>(T message) where T : IShipWorksMessage;

        /// <summary>
        /// Remove a handler based on the token
        /// </summary>
        /// <param name="token"></param>
        void Remove(MessengerToken token);
    }
}