using System;
using System.Linq;
using System.Reactive.Linq;
using Interapptive.Shared.Collections;
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
                .Select(GetMatchingChangedMessages)
                .Switch();
        }

        /// <summary>
        /// Get an observable of selection changed messages that match the order ids of the changing message
        /// </summary>
        private IObservable<OrderSelectionChangedMessage> GetMatchingChangedMessages(OrderSelectionChangingMessage changingMessage)
        {
            return messageStream.OfType<OrderSelectionChangedMessage>()
                .Where(changedMessage => DoMessagesMatch(changingMessage, changedMessage));
        }

        /// <summary>
        /// Test whether the order ids of the changing message matches the changed message
        /// </summary>
        private static bool DoMessagesMatch(OrderSelectionChangingMessage changingMessage,
            OrderSelectionChangedMessage changedMessage)
        {
            return changingMessage.OrderIdList.Count() == changedMessage.LoadedOrderSelection.Count() &&
                changingMessage.OrderIdList.Except(changedMessage.LoadedOrderSelection.Select(x => x.OrderID)).None();
        }
    }
}
