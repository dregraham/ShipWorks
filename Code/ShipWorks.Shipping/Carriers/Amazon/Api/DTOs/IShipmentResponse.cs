namespace ShipWorks.Shipping.Carriers.Amazon.Api.DTOs
{
    public interface IShipmentResponse
    {
        ResponseMetadata ResponseMetadata { get; set; }
        string Xmlns { get; set; }
    }
}