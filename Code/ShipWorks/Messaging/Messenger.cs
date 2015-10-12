using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace ShipWorks.Core.Messaging
{
    /// <summary>
    /// Messenger that allows messages to be sent and handled from anywhere in the application
    /// </summary>
    public class Messenger : IMessenger
    {
        private readonly object lockObj = new object();
        private readonly ConcurrentDictionary<Type, List<MessageHandler>> handlers = new ConcurrentDictionary<Type, List<MessageHandler>>();
        private readonly IObservable<IShipWorksMessage> messageStream;

        /// <summary>
        /// Create the static instance
        /// </summary>
        static Messenger()
        {
            Current = new Messenger();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Messenger()
        {
            messageStream = Observable.Create<IShipWorksMessage>(x =>
            {
                MessengerToken token = Handle<IShipWorksMessage>(this, x.OnNext);
                return Disposable.Create(() => Remove(token));
            }).Publish().RefCount();
        }

        /// <summary>
        /// Get the current messenger instance
        /// </summary>
        public static IMessenger Current { get; private set; }

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
        /// Get a reference to the messenger as an observable stream of messages
        /// </summary>
        public IObservable<T> AsObservable<T>() where T : IShipWorksMessage => messageStream.OfType<T>();

        /// <summary>
        /// Send a message to any listeners
        /// </summary>
        public void Send<T>(T message) where T : IShipWorksMessage
        {
            List<MessageHandler> handlersToRemove = new List<MessageHandler>();
            handlersToRemove.AddRange(SendInternal(typeof(IShipWorksMessage), message));
            handlersToRemove.AddRange(SendInternal(typeof(T), message));

            if (handlersToRemove.Any())
            {
                lock (lockObj)
                {
                    Remove(handlersToRemove);
                }
            }
        }

        /// <summary>
        /// Send messages to a specific type
        /// </summary>
        private IEnumerable<MessageHandler> SendInternal<T>(Type messageType, T message) where T : IShipWorksMessage
        {
            List<MessageHandler> handlerList;
            if (!handlers.TryGetValue(messageType, out handlerList))
            {
                return Enumerable.Empty<MessageHandler>();
            }

            return handlerList.Where(handler => !handler.Handle(message)).ToList();
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
        /// Handle a message using the specified handler
        /// </summary>
        /// <returns>Token that can be used to remove the handler later</returns>
        private MessengerToken Handle(object owner, Type messageType, object handler)
        {
            lock (lockObj)
            {
                List<MessageHandler> handlerList = handlers.GetOrAdd(messageType, x => new List<MessageHandler>());
                MessageHandler messageHandler = handlerList.FirstOrDefault(x => x.ReferencesHandler(owner, handler));

                // If this handler isn't already registered, go ahead and register it
                if (messageHandler == null)
                {
                    messageHandler = new MessageHandler(messageType, owner, handler, new MessengerToken());

                    handlerList.Add(messageHandler);
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
            // Kill the handlers immediately in case we can't remove them right away
            foreach (MessageHandler handler in handlersToRemove)
            {
                handler.Kill();
            }

            foreach (var handler in handlersToRemove.GroupBy(x => x.MessageType))
            {
                List<MessageHandler> handlerList = null;
                if (handlers.TryGetValue(handler.Key, out handlerList))
                {
                    // We're replacing the list of handlers instead of modifying it so that any current enumeration doesn't break
                    // If the original list has changed since we retrieved it, we'll just rely on the next cleanup cycle to deal
                    // with it.  The handlers have already been killed.
                    handlers.TryUpdate(handler.Key, handlerList.Except(handlersToRemove).ToList(), handlerList);
                }
            }
        }

        /// <summary>
        /// Clean up any dead references
        /// </summary>
        private void Cleanup() =>
            Remove(handlers.SelectMany(x => x.Value).Where(x => !x.IsAlive).ToList());

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
            public bool Handle<T>(T message) where T : IShipWorksMessage
            {
                if (!IsAlive)
                {
                    return false;
                }

                Action<IShipWorksMessage> interfaceAction = action as Action<IShipWorksMessage>;
                if (interfaceAction != null)
                {
                    interfaceAction(message);
                }
                else
                {
                    ((Action<T>)action)(message);
                }

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
            /// Kill the handler so that it can be cleaned up
            /// </summary>
            internal void Kill() => handlerReference.Target = null;

            /// <summary>
            /// Gets whether the handler's owner is still alive
            /// </summary>
            public bool IsAlive => handlerReference.Target != null;
        }
    }
}
