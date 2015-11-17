using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace ShipWorks.Shipping.UI.MessageHandlers
{
    /// <summary>
    /// Handle wiring up the order selection changed handler
    /// </summary>
    public class OrderSelectionChangedHandler : IMessenger
    {
        private readonly IMessenger messenger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="messenger"></param>
        public OrderSelectionChangedHandler(IMessenger messenger)
        {
            this.messenger = messenger;
        }

        /// <summary>
        /// Gets a stream of order changing messages
        /// </summary>
        /// <returns></returns>
        public virtual IObservable<OrderSelectionChangingMessage> OrderChangingStream() =>
            messenger.OfType<OrderSelectionChangingMessage>();

        /// <summary>
        /// Gets a stream of the most recent order changed messages
        /// </summary>
        public virtual IObservable<OrderSelectionChangedMessage> ShipmentLoadedStream()
        {
            return messenger.OfType<OrderSelectionChangingMessage>()
                .CombineLatest(messenger.OfType<OrderSelectionChangedMessage>(), (x, y) => new { OrderIdList = x.OrderIdList, Message = y })
                .Where(x => x.OrderIdList.Intersect(x.Message.LoadedOrderSelection.Select(y => y.Order?.OrderID ?? -1)).Any())
                .Select(x => x.Message);
        }

        /// <summary>
        /// Send a message to the underlying messenger
        /// </summary>
        public virtual void Send<T>(T message) where T : IShipWorksMessage => messenger.Send(message);

        /// <summary>
        /// Subscribe to the event stream
        /// </summary>
        public virtual IDisposable Subscribe(IObserver<IShipWorksMessage> observer) => messenger.Subscribe(observer);
    }
}
