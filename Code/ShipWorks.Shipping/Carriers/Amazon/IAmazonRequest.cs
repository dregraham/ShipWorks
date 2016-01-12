using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Interface for Amazon Shipping requests
    /// </summary>
    public interface IAmazonShipmentRequest
    {
        /// <summary>
        /// Submits the request
        /// </summary>
        AmazonShipment Submit(ShipmentEntity shipment);
    }
}