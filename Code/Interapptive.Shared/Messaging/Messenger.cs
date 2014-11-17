using System;
using System.Collections.Generic;
using System.Linq;

namespace Interapptive.Shared.Messaging
{
    /// <summary>
    /// Messenger that allows messages to be sent and handled from anywhere in the application
    /// </summary>
    public class Messenger : IMessenger
    {
        private readonly object lockObj = new object();
        private readonly Dictionary<Type, List<MessageHandler>> handlers = new Dictionary<Type, List<MessageHandler>>();

        /// <summary>
        /// Create the static instance
        /// </summary>
        static Messenger()
        {
            Current = new Messenger();
        }

        /// <summary>
        /// Get the current messenger instance
        /// </summary>
        public static Messenger Current { get; private set; }

        /// <summary>
        /// Handle a message using the specified handler
        /// </summary>
        public MessengerToken Handle<T>(Action<T> handler) where T : IShipWorksMessage
        {
            Type messageType = typeof(T);


            lock (lockObj)
            {
                if (!handlers.ContainsKey(messageType))
                {
                    handlers.Add(messageType, new List<MessageHandler>());
                }

                MessageHandler messageHandler = handlers[messageType].FirstOrDefault(x => ReferenceEquals(x.Handler, handler));
                
                if (messageHandler == null)
                {
                    messageHandler = new MessageHandler(messageType, handler, new MessengerToken());

                    handlers[messageType].Add(messageHandler);
                }

                return messageHandler.Token;
            }
        }

        /// <summary>
        /// Send a message to any listeners
        /// </summary>
        public void Send<T>(T message) where T : IShipWorksMessage
        {
            Type messageType = typeof (T);

            List<MessageHandler> handlerList;
            if (!handlers.TryGetValue(messageType, out handlerList))
            {
                return;
            }

            lock (lockObj)
            {
                handlerList = handlerList.ToList();
            }

            foreach (Action<T> handler in handlerList.Select(x => x.Handler))
            {
                handler(message);
            }
        }

        /// <summary>
        /// Remove a handler based on the token
        /// </summary>
        /// <param name="token"></param>
        public void Remove(MessengerToken token)
        {
            lock (lockObj)
            {
                MessageHandler handler = handlers.SelectMany(x => x.Value).FirstOrDefault(x => x.Token == token);

                if (handler != null)
                {
                    handlers[handler.MessageType].Remove(handler);
                }
            }
        }

        /// <summary>
        /// Associate a handler with a type to make removal easier
        /// </summary>
        private class MessageHandler
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public MessageHandler(Type messageType, object handler, MessengerToken token)
            {
                MessageType = messageType;
                Handler = handler;
                Token = token;
            }

            /// <summary>
            /// Message type this handler is associated with
            /// </summary>
            public Type MessageType { get; private set; }

            /// <summary>
            /// Handler
            /// </summary>
            public object Handler { get; private set; }

            /// <summary>
            /// Cancelation token associated with this handler
            /// </summary>
            public MessengerToken Token { get; private set; }
        }
    }
}
