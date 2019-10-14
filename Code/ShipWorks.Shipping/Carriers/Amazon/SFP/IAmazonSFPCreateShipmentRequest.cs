using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.SFP.Api.DTOs;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP
{
    /// <summary>
    /// Interface for Amazon Shipping requests
    /// </summary>
    public interface IAmazonSFPCreateShipmentRequest
    {
        /// <summary>
        /// Submits the request
        /// </summary>
        AmazonShipment Submit(ShipmentEntity shipment, TelemetricResult<IDownloadedLabelData> telemetricResult);
    }
}