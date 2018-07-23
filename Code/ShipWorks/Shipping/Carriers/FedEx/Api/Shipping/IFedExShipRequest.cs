using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping
{
    /// <summary>
    /// FedEx create label request
    /// </summary>
    public interface IFedExShipRequest
    {
        /// <summary>
        /// Submit the request
        /// </summary>
        TelemetricResult<GenericResult<IFedExShipResponse>> Submit(ShipmentEntity shipment, int sequenceNumber);
    }
}
