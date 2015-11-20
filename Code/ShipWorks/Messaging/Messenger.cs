using System;
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
        public void Send<T>(T message) where T : IShipWorksMessage => observer?.OnNext(message);

        /// <summary>
        /// Subscribe to the message stream
        /// </summary>
        public IDisposable Subscribe(IObserver<IShipWorksMessage> observer) => 
            messageStream.Subscribe(observer);
    }
}
