using System;
using System.Collections.Generic;
using ShipWorks.Core.Messaging;

namespace ShipWorks.Tests.Shared
{
    /// <summary>
    /// Message that can be used for testing
    /// </summary>
    public class TestMessenger : IMessenger
    {
        readonly Dictionary<Type, object> handlers = new Dictionary<Type, object>();

        public MessengerToken Handle<T>(object owner, Action<T> handler) where T : IShipWorksMessage
        {
            handlers.Add(typeof(T), handler);
            return new MessengerToken();
        }

        public void Send<T>(T message) where T : IShipWorksMessage
        {
            ((Action<T>)handlers[typeof(T)])(message);
        }

        public void Remove(MessengerToken token)
        {
            // Not implement in test
        }

        public MessengerToken Handle(object owner, Type messageType, Action<IShipWorksMessage> handler)
        {
            handlers.Add(messageType, handler);
            return new MessengerToken();
        }

        public void Remove(object owner, Type messageType)
        {
            // Not implement in test
        }

        public IObservable<T> AsObservable<T>() where T : IShipWorksMessage
        {
            return null;
        }
    }
}
