using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using Autofac;
using Autofac.Features.OwnedInstances;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Messaging.Logging;
using Interapptive.Shared.Threading;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Core.Messaging
{
    /// <summary>
    /// Messenger that allows messages to be sent and handled from anywhere in the application
    /// </summary>
    [Component(SingleInstance = true)]
    public class Messenger : IMessenger
    {
        private readonly IObservable<IShipWorksMessage> messageStream;
        private IObserver<IShipWorksMessage> observer;

        /// <summary>
        /// Constructor
        /// </summary>
        public Messenger(ISchedulerProvider schedulerProvider)
        {
            Schedulers = schedulerProvider;

            messageStream = Observable.Create<IShipWorksMessage>(x =>
            {
                observer = x;
                return Disposable.Create(() => observer = null);
            }).Publish().RefCount();
        }
        
        /// <summary>
        /// Get the current messenger instance
        /// </summary>
        public static IMessenger Current =>
            IoC.UnsafeGlobalLifetimeScope.Resolve<Owned<IMessenger>>().Value;

        /// <summary>
        /// Available schedulers
        /// </summary>
        public ISchedulerProvider Schedulers { get; }

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
