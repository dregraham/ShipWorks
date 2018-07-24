namespace ShipWorks.Stores.Platforms.Overstock.OnlineUpdating
{
    /// <summary>
    /// Combine details for an overstock order
    /// </summary>
    public struct OverstockOrderDetail
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OverstockOrderDetail(string orderNumberComplete, string salesChannelName, string warehouseCode, long originalOrderID, bool isManual)
        {
            this.OrderNumberComplete = orderNumberComplete;
            this.SalesChannelName = salesChannelName;
            this.WarehouseCode = warehouseCode;
            this.OriginalOrderID = originalOrderID;
            this.IsManual = isManual;
        }

        /// <summary>
        /// Order number complete
        /// </summary>
        public string OrderNumberComplete { get; }

        /// <summary>
        /// Sales channel name
        /// </summary>
        public string SalesChannelName { get; }

        /// <summary>
        /// Warehouse code
        /// </summary>
        public string WarehouseCode { get; }

        /// <summary>
        /// Original order ID
        /// </summary>
        public long OriginalOrderID { get; }

        /// <summary>
        /// Is manual
        /// </summary>
        public bool IsManual { get; }
    }
}