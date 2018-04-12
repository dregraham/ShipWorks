using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Enums;
using ShipWorks.Stores.Platforms.Amazon;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Implemented by OrderEntities that could be Amazon Orders (Orders from Amazon or CA maybe others in the future)
    /// </summary>
    public partial class ChannelAdvisorOrderEntity : IAmazonOrder
    {
        /// <summary>
        /// True if the order is an Amazon Prime order, false otherwise
        /// </summary>
        string IAmazonOrder.AmazonOrderID => CustomOrderIdentifier;

        /// <summary>
        /// The Amazon Order ID from Amazon
        /// </summary>
        bool IAmazonOrder.IsPrime => IsPrime == (int) AmazonIsPrime.Yes;

        /// <summary>
        /// List of IAmazonOrderItem representing the Amazon order items
        /// </summary>
        public IEnumerable<IAmazonOrderItem> AmazonOrderItems
        {
            get { return OrderItems.Select(s => s as IAmazonOrderItem); }
        }

        /// <summary>
        /// Should the order be treated as same day
        /// </summary>
        public bool IsSameDay()
        {
            return IsPrime == (int) AmazonIsPrime.Yes &&
                   RequestedShipping != null &&
                   (RequestedShipping.EndsWith("sameday", StringComparison.OrdinalIgnoreCase) ||
                    RequestedShipping.EndsWith("same day", StringComparison.OrdinalIgnoreCase));
        }
    }
}
