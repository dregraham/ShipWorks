using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Messaging;

namespace ShipWorks.Tests.Shared
{
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

        }
    }
}
