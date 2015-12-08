using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    public interface IAmazonShipmentRequest
    {
        AmazonShipment Submit(ShipmentEntity shipment);
    }
}