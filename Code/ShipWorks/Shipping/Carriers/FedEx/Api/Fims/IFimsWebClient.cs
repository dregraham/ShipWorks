using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Fims
{
    /// <summary>
    /// Interface for working with a Fims Web Client
    /// </summary>
    public interface IFimsWebClient
    {
        /// <summary>
        /// Ships a FIMS shipment
        /// </summary>
        TelemetricResult<IFimsShipResponse> Ship(IFimsShipRequest fimsShipRequest);
    }
}
