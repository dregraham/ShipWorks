using Autofac.Extras.Attributed;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages;
using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace ShipWorks.Shipping.UI.MessageHandlers
{
    public class OrderSelectionChangedHandler : IDisposable
    {
        private readonly IMessenger messenger;
        private readonly IScheduler subscribeOnScheduler;
        private readonly IScheduler observeOnScheduler;
        IDisposable subscription;

        public OrderSelectionChangedHandler(IMessenger messenger, 
            [WithKey(typeof(TaskPoolScheduler))] IScheduler subscribeOn, 
            [WithKey(typeof(DispatcherScheduler))] IScheduler observeOn)
        {
            this.messenger = messenger;
            subscribeOnScheduler = subscribeOn;
            observeOnScheduler = observeOn;
        }

        public void Listen(Action<OrderSelectionChangedMessage> action)
        {
            subscription = messenger.AsObservable<OrderSelectionChangedMessage>()
                .ObserveOn(observeOnScheduler)
                .SubscribeOn(subscribeOnScheduler)
                .Subscribe(action);
        }

        public void Dispose()
        {
            subscription?.Dispose();
        }
    }
}
