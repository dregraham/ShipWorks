using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.Newegg.Net.Orders.Cancellation
{
    /// <summary>
    /// The different reasons that a Newegg order can be cancelled.
    /// </summary>
    public enum CancellationReason
    {
        OutOfStock = 24,

        CustomerRequestedToCancel = 72,

        PriceError = 73,

        UnableToFulfillOrder = 74
    }
}
