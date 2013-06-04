using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.Newegg.Enums
{
    /// <summary>
    /// Corresponds to the numerical value of an order's status returned from Newegg.
    /// </summary>
    public enum NeweggOrderStatus
    {
        Unshipped = 0,

        PartiallyShipped = 1,

        Shipped = 2,

        Invoiced = 3,

        Voided = 4
    }
}
