using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages;
using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace ShipWorks.Shipping.UI.MessageHandlers
{
    /// <summary>
    /// Handle wiring up the order selection changed handler
    /// </summary>
    public class OrderSelectionChangedHandler : IDisposable
    {
        private readonly IMessenger messenger;
        private readonly IScheduler subscribeOnScheduler;
        private readonly IScheduler observeOnScheduler;
        IDisposable subscription;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="messenger"></param>
        public OrderSelectionChangedHandler(IMessenger messenger) : this(messenger, TaskPoolScheduler.Default, DispatcherScheduler.Current)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderSelectionChangedHandler(IMessenger messenger, IScheduler subscribeOn, IScheduler observeOn)
        {
            this.messenger = messenger;
            subscribeOnScheduler = subscribeOn;
            observeOnScheduler = observeOn;
        }

        /// <summary>
        /// Listen for the message
        /// </summary>
        public virtual void Listen(Action<OrderSelectionChangedMessage> action)
        {
            subscription = messenger.AsObservable<OrderSelectionChangedMessage>()
                .ObserveOn(observeOnScheduler)
                .SubscribeOn(subscribeOnScheduler)
                .Subscribe(action);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            subscription?.Dispose();
            subscription = null;
        }
    }
}
