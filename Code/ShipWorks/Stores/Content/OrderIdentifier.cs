using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// Base class used for identifying orders in a way that is unique to each store type.  For instance,
    /// osCommerce orders can be identified using OrderNumber, but Amazon orders are identifier using the
    /// AmazonOrderID.
    /// </summary>
    abstract public class OrderIdentifier
    {
        /// <summary>
        /// Apply the order identifier values to the order
        /// </summary>
        public abstract void ApplyTo(OrderEntity order);

        /// <summary>
        /// Apply the order identifier values to the download history entry
        /// </summary>
        public abstract void ApplyTo(DownloadDetailEntity downloadDetail);
    }
}
