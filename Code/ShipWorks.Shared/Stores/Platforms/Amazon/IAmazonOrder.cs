
using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.Amazon
{
    /// <summary>
    /// Implemented by OrderEntities that could be Amazon Orders (Orders from Amazon or CA maybe others in the future)
    /// </summary>
    public interface IAmazonOrder
    {
        /// <summary>
        /// True if the order is an Amazon Prime order, false otherwise
        /// </summary>
        bool IsPrime { get; }

        /// <summary>
        /// The Amazon Order ID from Amazon
        /// </summary>
        string AmazonOrderID { get; }

        /// <summary>
        /// List of IAmazonOrderItem representing the Amazon order items
        /// </summary>
        IEnumerable<IAmazonOrderItem> AmazonOrderItems { get; }
    }
}
