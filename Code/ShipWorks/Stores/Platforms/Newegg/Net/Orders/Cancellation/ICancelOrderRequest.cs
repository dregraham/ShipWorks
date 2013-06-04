using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Cancellation.Response;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Response;

namespace ShipWorks.Stores.Platforms.Newegg.Net.Orders.Cancellation
{
    /// <summary>
    /// Interface for cancelling an order.
    /// </summary>
    public interface ICancelOrderRequest
    {
        /// <summary>
        /// Cancels a Newegg order.
        /// </summary>
        /// <param name="neweggOrder">The newegg order to be cancelled.</param>
        /// <param name="reason">The reason for cancelling the order.</param>
        /// <returns>A CancellationResult object.</returns>
        CancellationResult Cancel(Order neweggOrder, CancellationReason reason);
    }
}
