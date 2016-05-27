using System;
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
        readonly Subject<IShipWorksMessage> subject = new Subject<IShipWorksMessage>();

        public void Send<T>(T message, string caller = "") where T : IShipWorksMessage => subject.OnNext(message);

        public IObservable<T> AsObservable<T>() where T : IShipWorksMessage => subject.OfType<T>();

        public void Dispose() => subject.Dispose();

        public IDisposable Subscribe(IObserver<IShipWorksMessage> observer) =>
            subject.Subscribe(observer);
    }
}
