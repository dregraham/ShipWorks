using System;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.Groupon
{
    /// <summary>
    /// Uniquely identifies an GrouponStore order in the database
    /// </summary>
    public class GrouponOrderIdentifier : OrderIdentifier
    {
        // GrouponStore's Order ID
        private readonly string grouponStoreOrderId;

        /// <summary>
        /// Constructor
        /// </summary>
        public GrouponOrderIdentifier(string grouponStoreOrderId)
        {
            this.grouponStoreOrderId = grouponStoreOrderId;
        }

        /// <summary>
        /// Apply the order number to the order provided
        /// </summary>
        public override void ApplyTo(OrderEntity order)
        {
            GrouponOrderEntity grouponStoreOrder = order as GrouponOrderEntity;

            if (grouponStoreOrder == null)
            {
                throw new InvalidOperationException("A non GrouponStore order was passed to the GrouponStore order identifier.");
            }

            grouponStoreOrder.GrouponOrderID = grouponStoreOrderId;
        }

        /// <summary>
        /// Apply the order number to the download log entity
        /// </summary>
        public override void ApplyTo(DownloadDetailEntity downloadDetail)
        {
            downloadDetail.ExtraStringData1 = grouponStoreOrderId;
        }

        /// <summary>
        /// Create an entity query that can be used to retrieve the search record for a combined order
        /// </summary>
        public override QuerySpec CreateCombinedSearchQuery(QueryFactory factory) =>
            factory.GrouponOrderSearch.Where(GrouponOrderSearchFields.GrouponOrderID == grouponStoreOrderId);

        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString() => $"GrouponStoreOrderID:{grouponStoreOrderId}";
    }
}
