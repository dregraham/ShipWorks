using System;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.Rakuten
{
    /// <summary>
    /// Uniquely identifies a Rakuten order in the database
    /// </summary>
    public class RakutenOrderIdentifier : OrderIdentifier
    {
        // Shopify's Order ID
        private readonly string rakutenOrderIdentifier;

        /// <summary>
        /// Constructor
        /// </summary>
        public RakutenOrderIdentifier(string rakutenOrderNumber)
        {
            rakutenOrderIdentifier = rakutenOrderNumber;
        }

        /// <summary>
        /// Apply the order number to the order provided
        /// </summary>
        public override void ApplyTo(OrderEntity order)
        {
            if (order == null)
            {
                throw new ArgumentNullException("order", "order is required");
            }

            RakutenOrderEntity rakutenOrder = order as RakutenOrderEntity;

            if (rakutenOrder == null)
            {
                throw new InvalidOperationException("A non Rakuten order was passed to the Rakuten order identifier.");
            }

            rakutenOrder.RakutenOrderNumber = rakutenOrderIdentifier;
        }

        /// <summary>
        /// Apply the order number to the download log entity
        /// </summary>
        public override void ApplyTo(DownloadDetailEntity downloadDetail)
        {
            if (downloadDetail == null)
            {
                throw new ArgumentNullException("downloadDetail", "order is downloadDetail");
            }

            downloadDetail.ExtraStringData1 = rakutenOrderIdentifier;
        }

        /// <summary>
        /// Create an entity query that can be used to retrieve the search record for a combined order
        /// </summary>
        public override QuerySpec CreateCombinedSearchQuery(QueryFactory factory) =>
            CreateCombinedSearchQueryInternal(factory,
                factory.ShopifyOrderSearch,
                RakutenOrderSearchFields.OriginalOrderID,
                RakutenOrderSearchFields.RakutenOrderNumber == rakutenOrderIdentifier);

        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString() =>
            string.Format("RakutenOrderNumber:{0}", rakutenOrderIdentifier);
    }
}
