using System;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.Shopify
{
    /// <summary>
    /// Uniquely identifies an Shopify order in the database
    /// </summary>
    public class ShopifyOrderIdentifier : OrderIdentifier
    {
        // Shopify's Order ID
        private readonly long shopifyOrderIdentifier;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="shopifyOrderID"></param>
        public ShopifyOrderIdentifier(long shopifyOrderID)
        {
            shopifyOrderIdentifier = shopifyOrderID;
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

            ShopifyOrderEntity shopifyOrder = order as ShopifyOrderEntity;

            if (shopifyOrder == null)
            {
                throw new InvalidOperationException("A non Shopify order was passed to the Shopify order identifier.");
            }

            shopifyOrder.ShopifyOrderID = shopifyOrderIdentifier;
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

            downloadDetail.ExtraBigIntData1 = shopifyOrderIdentifier;
        }

        /// <summary>
        /// Create an entity query that can be used to retrieve the search record for a combined order
        /// </summary>
        public override QuerySpec CreateCombinedSearchQuery(QueryFactory factory) =>
            CreateCombinedSearchQueryInternal(factory,
                factory.ShopifyOrderSearch,
                ShopifyOrderSearchFields.OriginalOrderID,
                ShopifyOrderSearchFields.ShopifyOrderID == shopifyOrderIdentifier);

        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString() =>
            string.Format("ShopifyOrderID:{0}", shopifyOrderIdentifier);

        /// <summary>
        /// Value to use when auditing
        /// </summary>
        public override string AuditValue => shopifyOrderIdentifier.ToString();
    }
}
