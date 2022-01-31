namespace ShipWorks.Shipping.Tracking.DTO
{
    public class TrackingRequest
    {
        public string WarehouseId { get; set; }
        public string TrackingNumber { get; set; }
        public string CarrierCode { get; set; }
    }
}