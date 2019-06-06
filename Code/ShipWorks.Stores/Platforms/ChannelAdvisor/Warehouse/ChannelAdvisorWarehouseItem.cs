using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.Warehouse
{
    /// <summary>
    /// ChannelAdvisor warehouse item
    /// </summary>
    public class ChannelAdvisorWarehouseItem
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
        /// The MarketplaceBuyerId of the ChannelAdvisorOrderItem
        /// </summary>
        [JsonProperty("marketplaceBuyerId")]
        public string MarketplaceBuyerId { get; set; }

        /// <summary>
        /// The MarketplaceSalesId of the ChannelAdvisorOrderItem
        /// </summary>
        [JsonProperty("marketplaceSalesId")]
        public string MarketplaceSalesId { get; set; }

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
        /// The DistributionCenterId of the ChannelAdvisorOrderItem
        /// </summary>
        [JsonProperty("distributionCenterId")]
        public long DistributionCenterId { get; set; }

        /// <summary>
        /// The DistributionCenterName of the ChannelAdvisorOrderItem
        /// </summary>
        public string DistributionCenterName { get; set; }
    }
}
