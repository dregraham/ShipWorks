using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.Newegg.Enums
{
    /// <summary>
    /// Corresponds to the numerical value of an item's shipping status returned from Newegg.
    /// </summary>
    public enum NeweggItemShippingStatus
    {
        Null = 0,

        Unshipped = 1,

        Shipped = 2,

        Cancelled = 3
    }
}
