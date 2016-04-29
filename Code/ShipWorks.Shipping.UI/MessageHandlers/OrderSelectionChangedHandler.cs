using System;
using System.Linq;
using System.Reactive.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Messaging.TrackedObservable;
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
        public virtual IObservable<IMessageTracker<OrderSelectionChangingMessage>> OrderChangingStream() =>
            messageStream.OfType<OrderSelectionChangingMessage>().Trackable();

        /// <summary>
        /// Gets a stream of the most recent order changed messages
        /// </summary>
        public virtual IObservable<IMessageTracker<OrderSelectionChangedMessage>> ShipmentLoadedStream()
        {
            return messageStream.OfType<OrderSelectionChangingMessage>()
                .Trackable()
                .Select(this, GetMatchingChangedMessages)
                .Switch(this);
        }

        /// <summary>
        /// Get an observable of selection changed messages that match the order ids of the changing message
        /// </summary>
        private IObservable<IMessageTracker<OrderSelectionChangedMessage>> GetMatchingChangedMessages(OrderSelectionChangingMessage changingMessage)
        {
            return messageStream.OfType<OrderSelectionChangedMessage>()
                .Trackable()
                .Select(this, x => new { Changed = x, Changing = changingMessage })
                .Where(this, x => DoMessagesMatch(x.Changing, x.Changed))
                .Select(this, x => x.Changed);
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
