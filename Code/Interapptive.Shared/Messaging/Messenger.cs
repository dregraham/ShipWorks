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
        /// <returns>Token that can be used to remove the handler later</returns>
        public MessengerToken Handle<T>(Action<T> handler) where T : IShipWorksMessage
        {
            Type messageType = typeof(T);

            lock (lockObj)
            {
                if (!handlers.ContainsKey(messageType))
                {
                    handlers.Add(messageType, new List<MessageHandler>());
                }

                MessageHandler messageHandler = handlers[messageType].FirstOrDefault(x => x.ReferencesHandler(handler));
                
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

            // Get a copy of the list to ensure that it doesn't change while we're iterating through it
            lock (lockObj)
            {
                List<MessageHandler> invalidHandlers = handlerList.Where(handler => !handler.Handle(message)).ToList();

                foreach (MessageHandler handler in invalidHandlers)
                {
                    handlerList.Remove(handler);
                }   
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
                Remove(handlers.SelectMany(x => x.Value).Where(x => x.Token == token).ToList());
            }
        }

        /// <summary>
        /// Remove a handler based on the handler method
        /// </summary>
        public void Remove<T>(Action<T> handler)
        {
            lock (lockObj)
            {
                Remove(handlers.SelectMany(x => x.Value).Where(x => x.ReferencesHandler(handler)).ToList());
            }
        }

        /// <summary>
        /// Remove a collection of handlers from the handler collection
        /// </summary>
        private void Remove(IEnumerable<MessageHandler> handlersToRemove)
        {
            foreach (MessageHandler handler in handlersToRemove)
            {
                handlers[handler.MessageType].Remove(handler);  
            }
        }

        /// <summary>
        /// Associate a handler with a type to make removal easier
        /// </summary>
        private class MessageHandler
        {
            private readonly WeakReference handlerReference;

            /// <summary>
            /// Constructor
            /// </summary>
            public MessageHandler(Type messageType, object handler, MessengerToken token)
            {
                MessageType = messageType;
                Token = token;
                handlerReference = new WeakReference(handler);
            }

            /// <summary>
            /// Message type this handler is associated with
            /// </summary>
            public Type MessageType { get; private set; }

            /// <summary>
            /// Cancelation token associated with this handler
            /// </summary>
            public MessengerToken Token { get; private set; }

            /// <summary>
            /// Handle the specified message
            /// </summary>
            /// <returns>True if the message was handled, false if not.  If false, it means the handler was garbage collected
            /// and should be removed</returns>
            public bool Handle<T>(T message)
            {
                Action<T> handler = handlerReference.Target as Action<T>;

                if (!handlerReference.IsAlive || handler == null)
                {
                    return false;
                }

                handler(message);
                return true;
            }

            /// <summary>
            /// Checks whether the given handler is what is used by this object
            /// </summary>
            public bool ReferencesHandler(object handler)
            {
                return ReferenceEquals(handlerReference.Target, handler);
            }
        }
    }
}
