using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.Amazon
{
    /// <summary>
    /// Implemented by OrderItemEntities that could be Amazon Order Items (Orders from Amazon or CA maybe others in the future)
    /// </summary>
    public interface IAmazonOrderItem
    {
        /// <summary>
        /// The Amazon order item code
        /// </summary>
        string AmazonOrderItemCode { get; }

        /// <summary>
        /// Quantity of the order item
        /// </summary>
        double Quantity { get; }
    }
}
