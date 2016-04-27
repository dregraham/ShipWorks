using System;
using Interapptive.Shared.Messaging.TrackedObservable;
using ShipWorks.Messaging.Messages;

namespace ShipWorks.Shipping.UI.MessageHandlers
{
    /// <summary>
    /// Handle wiring up the order selection changed handler
    /// </summary>
    public interface IOrderSelectionChangedHandler
    {
        /// <summary>
        /// Gets a stream of order changing messages
        /// </summary>
        IObservable<IMessageTracker<OrderSelectionChangingMessage>> OrderChangingStream();

        /// <summary>
        /// Gets a stream of the most recent order changed messages
        /// </summary>
        IObservable<IMessageTracker<OrderSelectionChangedMessage>> ShipmentLoadedStream();
    }
}