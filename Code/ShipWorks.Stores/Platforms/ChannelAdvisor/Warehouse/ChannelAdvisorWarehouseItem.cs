using ShipWorks.Warehouse.DTO.Orders;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.Warehouse
{
    /// <summary>
    /// ChannelAdvisor warehouse item
    /// </summary>
    public class ChannelAdvisorWarehouseItem : WarehouseOrderItem
    {
        /// <summary>
        /// The MarketplaceName of the ChannelAdvisorOrderItem
        /// </summary>
        public string MarketplaceName { get; set; }

        /// <summary>
        /// The MarketplaceStoreName of the ChannelAdvisorOrderItem
        /// </summary>
        public string MarketplaceStoreName { get; set; }

        /// <summary>
        /// The MarketplaceBuyerID of the ChannelAdvisorOrderItem
        /// </summary>
        public string MarketplaceBuyerID { get; set; }

        /// <summary>
        /// The MarketplaceSalesID of the ChannelAdvisorOrderItem
        /// </summary>
        public string MarketplaceSalesID { get; set; }

        /// <summary>
        /// The Classification of the ChannelAdvisorOrderItem
        /// </summary>
        public string Classification { get; set; }

        /// <summary>
        /// The DistributionCenter of the ChannelAdvisorOrderItem
        /// </summary>
        public string DistributionCenter { get; set; }

        /// <summary>
        /// The IsFBA of the ChannelAdvisorOrderItem
        /// </summary>
        public bool IsFBA { get; set; }

        /// <summary>
        /// The DistributionCenterID of the ChannelAdvisorOrderItem
        /// </summary>
        public long DistributionCenterID { get; set; }

        /// <summary>
        /// The DistributionCenterName of the ChannelAdvisorOrderItem
        /// </summary>
        public string DistributionCenterName { get; set; }
    }
}
