using System;
using Interapptive.Shared.Enums;

namespace ShipWorks.Shipping.Tracking
{
    public class TrackingNotification
    {
        public string TrackingNumber { get; set; }
        public TrackingStatus TrackingStatus { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public DateTime? ActualDeliveryDate { get; set; }
        public string CarrierStatusDescription { get; set; }
        public DateTime HubTimestamp { get; set; }
    }
}