using System;
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
        IObservable<OrderSelectionChangingMessage> OrderChangingStream();

        /// <summary>
        /// Gets a stream of the most recent order changed messages
        /// </summary>
        IObservable<OrderSelectionChangedMessage> ShipmentLoadedStream();
    }
}