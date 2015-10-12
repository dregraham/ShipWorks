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
    public class OrderSelectionChangedHandler
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
        public IObservable<OrderSelectionChangingMessage> OrderChangingStream() =>
            messenger.AsObservable<OrderSelectionChangingMessage>();

        /// <summary>
        /// Gets a stream of the most recent order changed messages
        /// </summary>
        public IObservable<OrderSelectionChangedMessage> ShipmentLoadedStream()
        {
            return messenger.AsObservable<OrderSelectionChangingMessage>()
                .CombineLatest(messenger.AsObservable<OrderSelectionChangedMessage>(), (x, y) => new { OrderIdList = x.OrderIdList, Message = y })
                .Where(x => x.OrderIdList.Intersect(x.Message.LoadedOrderSelection.Select(y => y.Order?.OrderID ?? -1)).Any())
                .Select(x => x.Message);
        }
    }
}
