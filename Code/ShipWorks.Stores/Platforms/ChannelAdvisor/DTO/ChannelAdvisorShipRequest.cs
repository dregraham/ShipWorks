using System;
using ShipWorks.Stores.Platforms.ChannelAdvisor.Enums;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.DTO
{
    /// <summary>
    /// Ship Request to send to ChannelAdvisor
    /// </summary>
    public class ChannelAdvisorShipRequest
    {
        public int OrderID { get; set; }

        public DateTime ShippedDateUtc { get; set; }

        public string TrackingNumber { get; set; }

        public string ShippingCarrier { get; set; }

        public string ShippingClass { get; set; }

        public ChannelAdvisorFulfillmentDeliveryStatus DeliveryStatus { get; set; }

        public int DistributionCenterID { get; set; }

        public string SellerFulfillmentID { get; set; }
    }
}