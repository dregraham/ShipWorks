namespace ShipWorks.Stores.Platforms.Walmart.OnlineUpdating
{
    /// <summary>
    /// Combined identifier for Walmart
    /// </summary>
    public class WalmartCombinedIdentifier
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="purchaseOrderID"></param>
        public WalmartCombinedIdentifier(long orderID, string purchaseOrderID)
        {
            OriginalOrderID = orderID;
            PurchaseOrderID = purchaseOrderID;
        }

        /// <summary>
        /// Order ID associated with the purchase
        /// </summary>
        public long OriginalOrderID { get; }

        /// <summary>
        /// Purchase order ID
        /// </summary>
        public string PurchaseOrderID { get; }
    }
}