using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.Walmart
{
    /// <summary>
    /// Uniquely identifies an Walmart order in the database
    /// </summary>
    /// <remarks>
    /// The reason we are using the order identifier instead of OrderNumberIdentifier
    /// is that when the Walmart integration was first released we stored the customer order number as the
    /// order number. We updated the integration to store PurchaseOrderNumber as the OrderNumber.
    ///
    /// If we use the OrderNumberIdentifier with the PurchaseOrderNumber and downloaded an existing order, the
    /// version in the order table would be ignored and a new order would be created.  Since there is an index on the PurchaseOrderId,
    /// this should be just as efficient as using the OrderNumberIdentifier.
    /// </remarks>
    public class WalmartOrderIdentifier : OrderIdentifier
    {
        private readonly string purchaseOrderId;

        /// <summary>
        /// Constructor
        /// </summary>
        public WalmartOrderIdentifier(string purchaseOrderId)
        {
            this.purchaseOrderId = purchaseOrderId;
        }

        /// <summary>
        /// Apply the order identifier values to the order
        /// </summary>
        public override void ApplyTo(OrderEntity order)
        {
            WalmartOrderEntity walmartOrder = order as WalmartOrderEntity;
            MethodConditions.EnsureArgumentIsNotNull(walmartOrder, nameof(walmartOrder));

            walmartOrder.PurchaseOrderID = purchaseOrderId;
        }

        /// <summary>
        /// Apply the order identifier values to the download history entry
        /// </summary>
        public override void ApplyTo(DownloadDetailEntity downloadDetail)
        {
            MethodConditions.EnsureArgumentIsNotNull(downloadDetail, nameof(downloadDetail));

            downloadDetail.ExtraStringData1 = purchaseOrderId;
        }

        /// <summary>
        /// Create an entity query that can be used to retrieve the search record for a combined order
        /// </summary>
        public override QuerySpec CreateCombinedSearchQuery(QueryFactory factory) =>
            factory.WalmartOrderSearch
                .Where(WalmartOrderSearchFields.PurchaseOrderID == purchaseOrderId);

        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString() => $"WalmartPurchaseOrderID:{purchaseOrderId}";
    }
}