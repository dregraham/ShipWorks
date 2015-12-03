using System;
using System.Linq;
using System.Reactive.Linq;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages;

namespace ShipWorks.Shipping.UI.MessageHandlers
{
    /// <summary>
    /// Handle wiring up the order selection changed handler
    /// </summary>
    public class OrderSelectionChangedHandler : IOrderSelectionChangedHandler
    {
        private readonly IObservable<IShipWorksMessage> messageStream;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderSelectionChangedHandler(IObservable<IShipWorksMessage> messageStream)
        {
            this.messageStream = messageStream;
        }

        /// <summary>
        /// Gets a stream of order changing messages
        /// </summary>
        public virtual IObservable<OrderSelectionChangingMessage> OrderChangingStream() =>
            messageStream.OfType<OrderSelectionChangingMessage>();

        /// <summary>
        /// Gets a stream of the most recent order changed messages
        /// </summary>
        public virtual IObservable<OrderSelectionChangedMessage> ShipmentLoadedStream()
        {
            return messageStream.OfType<OrderSelectionChangingMessage>()
                .CombineLatest(messageStream.OfType<OrderSelectionChangedMessage>(), (x, y) => new { OrderIdList = x.OrderIdList, Message = y })
                .Where(x => x.OrderIdList.Intersect(x.Message.LoadedOrderSelection.Select(y => y.Order?.OrderID ?? -1)).Any())
                .Select(x => x.Message);
        }
    }
}
