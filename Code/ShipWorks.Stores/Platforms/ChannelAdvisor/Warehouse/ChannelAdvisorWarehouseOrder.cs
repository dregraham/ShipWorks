using System.Collections.Generic;
using ShipWorks.Warehouse.DTO.Orders;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.Warehouse
{
    /// <summary>
    /// ChannelAdvisor warehouse order
    /// </summary>
    public class ChannelAdvisorWarehouseOrder : WarehouseOrder
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelAdvisorWarehouseOrder()
        {
            Items = new List<ChannelAdvisorWarehouseItem>();
        }

        /// <summary>
        /// The CustomOrderIdentifier of the ChannelAdvisorOrder
        /// </summary>
        public string CustomOrderIdentifier { get; set; }

        /// <summary>
        /// The ResellerID of the ChannelAdvisorOrder
        /// </summary>
        public string ResellerID { get; set; }

        /// <summary>
        /// The OnlineShippingStatus of the ChannelAdvisorOrder
        /// </summary>
        public int OnlineShippingStatus { get; set; }

        /// <summary>
        /// The OnlineCheckoutStatus of the ChannelAdvisorOrder
        /// </summary>
        public int OnlineCheckoutStatus { get; set; }

        /// <summary>
        /// The OnlinePaymentStatus of the ChannelAdvisorOrder
        /// </summary>
        public int OnlinePaymentStatus { get; set; }

        /// <summary>
        /// The FlagStyle of the ChannelAdvisorOrder
        /// </summary>
        public string FlagStyle { get; set; }

        /// <summary>
        /// The FlagDescription of the ChannelAdvisorOrder
        /// </summary>
        public string FlagDescription { get; set; }

        /// <summary>
        /// The FlagType of the ChannelAdvisorOrder
        /// </summary>
        public int FlagType { get; set; }

        /// <summary>
        /// The MarketplaceNames of the ChannelAdvisorOrder
        /// </summary>
        public string MarketplaceNames { get; set; }

        /// <summary>
        /// The IsPrime of the ChannelAdvisorOrder
        /// </summary>
        public int IsPrime { get; set; }

        /// <summary>
        /// The items in the order
        /// </summary>
        public override IEnumerable<WarehouseOrderItem> Items { get; set; }
    }
}
