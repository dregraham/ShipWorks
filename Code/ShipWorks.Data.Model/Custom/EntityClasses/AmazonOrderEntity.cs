using System;
using System.Collections.Generic;
using System.Linq;
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
        string IAmazonOrder.AmazonOrderID => AmazonOrderID;

        /// <summary>
        /// The Amazon Order ID from Amazon
        /// </summary>
        bool IAmazonOrder.IsPrime
        {
            get { return IsPrime == (int) AmazonMwsIsPrime.Yes; }
        }

        /// <summary>
        /// List of IAmazonOrderItem representing the Amazon order items
        /// </summary>
        public IEnumerable<IAmazonOrderItem> AmazonOrderItems
        {
            get { return OrderItems.OfType<IAmazonOrderItem>(); }
        }

        /// <summary>
        /// Should the order be treated as same day
        /// </summary>
        /// <remarks>We have to do the date check here because we won't get rates from Amazon if we treat the shipment
        /// as same day, but the customer missed the delivery date.</remarks>
        public bool IsSameDay(Func<DateTime> getUtcNow)
        {
            return (RequestedShipping?.StartsWith("sameday", StringComparison.OrdinalIgnoreCase) ?? false) &&
                LatestExpectedDeliveryDate.HasValue &&
                LatestExpectedDeliveryDate.Value > getUtcNow();
        }
    }
}
