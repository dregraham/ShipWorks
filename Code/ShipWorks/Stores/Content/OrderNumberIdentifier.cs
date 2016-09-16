using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// Used to identify online orders based on their OrderNumber
    /// </summary>
    public class OrderNumberIdentifier : OrderIdentifier
    {
        long orderNumber;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderNumberIdentifier(long orderNumber)
        {
            this.orderNumber = orderNumber;
        }

        /// <summary>
        /// The OrderNumber being identified
        /// </summary>
        public long OrderNumber
        {
            get { return orderNumber; }
        }

        /// <summary>
        /// String representation of the object
        /// </summary>
        public override string ToString()
        {
            return string.Format("OrderNumber:{0}", orderNumber);
        }

        /// <summary>
        /// Apply the order number value to the specified order
        /// </summary>
        public override void ApplyTo(OrderEntity order)
        {
            order.OrderNumber = orderNumber;
        }

        /// <summary>
        /// Add the OrderNumber to the download history for the store
        /// </summary>
        public override void ApplyTo(DownloadDetailEntity downloadDetail)
        {
            downloadDetail.OrderNumber = orderNumber;
        }
    }
}
