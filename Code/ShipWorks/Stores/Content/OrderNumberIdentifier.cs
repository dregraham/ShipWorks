using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// Used to identify online orders based on their OrderNumber
    /// </summary>
    public class OrderNumberIdentifier : OrderIdentifier
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderNumberIdentifier(long orderNumber)
        {
            OrderNumber = orderNumber;
        }

        /// <summary>
        /// The OrderNumber being identified
        /// </summary>
        public long OrderNumber { get; }

        /// <summary>
        /// String representation of the object
        /// </summary>
        public override string ToString() => $"OrderNumber:{OrderNumber}";

        /// <summary>
        /// Apply the order number value to the specified order
        /// </summary>
        public override void ApplyTo(OrderEntity order)
        {
            order.OrderNumber = OrderNumber;
        }

        /// <summary>
        /// Add the OrderNumber to the download history for the store
        /// </summary>
        public override void ApplyTo(DownloadDetailEntity downloadDetail)
        {
            downloadDetail.OrderNumber = OrderNumber;
        }

        /// <summary>
        /// Create an entity query that can be used to retrieve the search record for a combined order
        /// </summary>
        public override QuerySpec CreateCombinedSearchQuery(QueryFactory factory) =>
            factory.OrderSearch.Where(OrderSearchFields.OrderNumber == OrderNumber);
    }
}
