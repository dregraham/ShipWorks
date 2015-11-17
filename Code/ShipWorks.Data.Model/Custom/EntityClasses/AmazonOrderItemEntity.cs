using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Amazon;
using ShipWorks.Stores.Platforms.Amazon.Mws;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Implemented by OrderEntities that could be Amazon Orders (Orders from Amazon or CA maybe others in the future)
    /// </summary>
    public partial class AmazonOrderItemEntity : IAmazonOrderItem
    {
        /// <summary>
        /// The Amazon order item code
        /// </summary>
        string IAmazonOrderItem.AmazonOrderItemCode
        {
            get { return AmazonOrderItemCode; }
        }

        /// <summary>
        /// Quantity of the order item
        /// </summary>
        double IAmazonOrderItem.Quantity
        {
            get { return Quantity; }
        }
    }
}
