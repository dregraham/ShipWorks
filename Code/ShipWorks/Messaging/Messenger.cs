using System;
using System.Collections.Generic;
using System.Linq;

namespace ShipWorks.Core.Messaging
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
        public MessengerToken Handle<T>(object owner, Action<T> handler) where T : IShipWorksMessage =>
            Handle(owner, typeof(T), handler);

        /// <summary>
        /// Handle a message using the specified handler
        /// </summary>
        /// <returns>Token that can be used to remove the handler later</returns>
        public MessengerToken Handle(object owner, Type messageType, Action<IShipWorksMessage> handler) =>
            Handle(owner, messageType, (object)handler);

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
        /// Remove all handlers of a message type for a given owner
        /// </summary>
        public void Remove(object owner, Type messageType)
        {
            lock (lockObj)
            {
                List<MessageHandler> handlerList;
                if (!handlers.TryGetValue(messageType, out handlerList))
                {
                    return;
                }
                
                Remove(handlerList.Where(x => x.IsOwnedBy(owner)).ToList());
            }
        }

        /// <summary>
        /// Remove a handler based on the handler method
        /// </summary>
        public void Remove<T>(object owner, Action<T> handler)
        {
            lock (lockObj)
            {
                Remove(handlers.SelectMany(x => x.Value).Where(x => x.ReferencesHandler(owner, handler)).ToList());
            }
        }

        /// <summary>
        /// Handle a message using the specified handler
        /// </summary>
        /// <returns>Token that can be used to remove the handler later</returns>
        private MessengerToken Handle(object owner, Type messageType, object handler)
        {
            lock (lockObj)
            {
                if (!handlers.ContainsKey(messageType))
                {
                    handlers.Add(messageType, new List<MessageHandler>());
                }

                MessageHandler messageHandler = handlers[messageType].FirstOrDefault(x => x.ReferencesHandler(owner, handler));

                // If this handler isn't already registered, go ahead and register it
                if (messageHandler == null)
                {
                    messageHandler = new MessageHandler(messageType, owner, handler, new MessengerToken());

                    handlers[messageType].Add(messageHandler);
                }

                Cleanup();

                return messageHandler.Token;
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
        /// Clean up any dead references
        /// </summary>
        private void Cleanup()
        {
            Remove(handlers.SelectMany(x => x.Value).Where(x => !x.IsAlive).ToList());
        }

        /// <summary>
        /// Associate a handler with a type to make removal easier
        /// </summary>
        private class MessageHandler
        {
            /// <summary>
            /// Store a weak reference to the owner of the action so that the messenger doesn't cause the
            /// object to stay around longer than intended
            /// </summary>
            private readonly WeakReference handlerReference;
            private readonly object action;

            /// <summary>
            /// Constructor
            /// </summary>
            public MessageHandler(Type messageType, object owner, object handler, MessengerToken token)
            {
                MessageType = messageType;
                Token = token;
                action = handler;
                handlerReference = new WeakReference(owner);
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
                if (!handlerReference.IsAlive || handlerReference.Target == null)
                {
                    return false;
                }

                ((Action<T>)action)(message);
                return true;
            }

            /// <summary>
            /// Checks whether the given handler is what is used by this object
            /// </summary>
            public bool ReferencesHandler(object owner, object handler)
            {
                return ReferenceEquals(handlerReference.Target, owner) && 
                    ReferenceEquals(action, handler);
            }

            /// <summary>
            /// Checks whether the handler is owned by the specified object
            /// </summary>
            public bool IsOwnedBy(object owner) => ReferenceEquals(handlerReference.Target, owner);

            /// <summary>
            /// Gets whether the handler's owner is still alive
            /// </summary>
            public bool IsAlive
            {
                get
                {
                    return handlerReference != null && handlerReference.Target != null;
                }
            }
        }
    }
}
