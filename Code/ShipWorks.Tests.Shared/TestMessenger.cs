using System;
using ShipWorks.Core.Messaging;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace ShipWorks.Tests.Shared
{
    /// <summary>
    /// Message that can be used for testing
    /// </summary>
    public class TestMessenger : IMessenger, IDisposable
    {
        readonly Subject<IShipWorksMessage> subject = new Subject<IShipWorksMessage>();

        public void Send<T>(T message) where T : IShipWorksMessage => subject.OnNext(message);

        public IObservable<T> AsObservable<T>() where T : IShipWorksMessage => subject.OfType<T>();

        public void Dispose() => subject.Dispose();

        public IDisposable Subscribe(IObserver<IShipWorksMessage> observer) =>
            subject.Subscribe(observer);
    }
}
