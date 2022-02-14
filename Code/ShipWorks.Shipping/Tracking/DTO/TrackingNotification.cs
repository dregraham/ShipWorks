using System;
using System.Reflection;
using Interapptive.Shared.Enums;

namespace ShipWorks.Shipping.Tracking.DTO
{
    [Obfuscation]
    public class TrackingNotification
    {
        public string TrackingNumber { get; set; }
        public string TrackingStatus { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public DateTime? ActualDeliveryDate { get; set; }
        public DateTime HubTimestamp { get; set; }
    }
}