using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Stores.Platforms.Amazon;

namespace ShipWorks.Shipping.Carriers.Amazon.Api
{
    /// <summary>
    /// Factory for creating the ShipmentRequestDetails used for the Amazon Shipping Api
    /// </summary>
    public interface IAmazonShipmentRequestDetailsFactory
    {
        /// <summary>
        /// Creates the ShipmentRequestDetails.
        /// </summary>
        ShipmentRequestDetails Create(ShipmentEntity shipment, IAmazonOrder order);
    }
}