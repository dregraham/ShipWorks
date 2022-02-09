using System.Reflection;

namespace ShipWorks.Shipping.Tracking.DTO
{
    [Obfuscation]
    public class TrackingRequest
    {
        public string WarehouseId { get; set; }
        public string TrackingNumber { get; set; }
        public string CarrierCode { get; set; }
    }
}