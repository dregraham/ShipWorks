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
    public partial class AmazonOrderEntity : IAmazonOrder
    {
        /// <summary>
        /// True if the order is an Amazon Prime order, false otherwise
        /// </summary>
        string IAmazonOrder.AmazonOrderID
        {
            get { return this.AmazonOrderID; }
        }

        /// <summary>
        /// The Amazon Order ID from Amazon
        /// </summary>
        bool IAmazonOrder.IsPrime
        {
            get { return this.IsPrime == (int) AmazonMwsIsPrime.Yes; }
        }

        /// <summary>
        /// List of IAmazonOrderItem representing the Amazon order items
        /// </summary>
        public IEnumerable<IAmazonOrderItem> AmazonOrderItems
        {
            get { return OrderItems.Select(s => s as IAmazonOrderItem); }
        }
    }
}
