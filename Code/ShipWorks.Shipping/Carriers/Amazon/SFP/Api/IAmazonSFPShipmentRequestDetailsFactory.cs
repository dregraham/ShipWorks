using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.SFP.Api.DTOs;
using ShipWorks.Stores.Platforms.Amazon;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP.Api
{
    /// <summary>
    /// Factory for creating the ShipmentRequestDetails used for the Amazon Shipping Api
    /// </summary>
    public interface IAmazonSFPShipmentRequestDetailsFactory
    {
        /// <summary>
        /// Creates the ShipmentRequestDetails.
        /// </summary>
        ShipmentRequestDetails Create(ShipmentEntity shipment, IAmazonOrder order);
    }
}