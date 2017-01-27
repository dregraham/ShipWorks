using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Interapptive.Shared.Messaging;
using ShipWorks.Core.Messaging;

namespace ShipWorks.Tests.Shared
{
    /// <summary>
    /// Message that can be used for testing
    /// </summary>
    [SuppressMessage("Code Analysis", "CA2213: Disposable fields should be disposed",
        Justification = "Subject *does* get disposed, so this feels like a false positive")]
    public class TestMessenger : IMessenger, IDisposable
    {
        /// <summary>
        /// Messages sent by this messenger
        /// </summary>
        public List<IShipWorksMessage> SentMessages { get; }

        /// <summary>
        /// The subject of the message
        /// </summary>
        readonly Subject<IShipWorksMessage> subject = new Subject<IShipWorksMessage>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TestMessenger"/> class.
        /// </summary>
        public TestMessenger()
        {
            SentMessages = new List<IShipWorksMessage>();
        }

        /// <summary>
        /// Send a message to any listeners
        /// </summary>
        public void Send<T>(T message, string caller = "") where T : IShipWorksMessage
        {
            subject.OnNext(message);
            SentMessages.Add(message);
        }

        /// <summary>
        /// Returns messages as observable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IObservable<T> AsObservable<T>() where T : IShipWorksMessage => subject.OfType<T>();

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() => subject.Dispose();

        /// <summary>
        /// Notifies the provider that an observer is to receive notifications.
        /// </summary>
        public IDisposable Subscribe(IObserver<IShipWorksMessage> observer) =>
            subject.Subscribe(observer);
    }
}
