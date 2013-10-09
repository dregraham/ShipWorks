using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request
{
    public interface IFedExNativeShipmentRequest
    {
        RequestedShipment RequestedShipment { get; set; }
        ClientDetail ClientDetail { get; set; }
        TransactionDetail TransactionDetail { get; set; }
        VersionId Version { get; set; }
        WebAuthenticationDetail WebAuthenticationDetail { get; set; }
    }
}