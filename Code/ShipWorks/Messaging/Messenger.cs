using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Messaging.Logging;

namespace ShipWorks.Core.Messaging
{
    /// <summary>
    /// Messenger that allows messages to be sent and handled from anywhere in the application
    /// </summary>
    public class Messenger : IMessenger
    {
        private readonly IObservable<IShipWorksMessage> messageStream;
        private IObserver<IShipWorksMessage> observer;

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
                observer = x;
                return Disposable.Create(() => observer = null);
            }).Publish().RefCount();
        }

        /// <summary>
        /// Get the current messenger instance
        /// </summary>
        public static IMessenger Current { get; private set; }

        /// <summary>
        /// Send a message to any listeners
        /// </summary>
        public void Send<T>(T message, [CallerMemberName] string callerName = "") where T : IShipWorksMessage
        {
            MessageLogger.Current.LogSend(message, callerName);
            observer?.OnNext(message);
        }

        /// <summary>
        /// Subscribe to the message stream
        /// </summary>
        public IDisposable Subscribe(IObserver<IShipWorksMessage> observer) =>
            messageStream.Subscribe(observer);
    }
}
