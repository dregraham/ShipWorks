using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.GlobalShipAddress
{
    /// <summary>
    /// Processes SearchLocationsReply from FedEx
    /// </summary>
    public interface IFedExGlobalShipAddressResponse
    {
        /// <summary>
        /// Process the response
        /// </summary>
        GenericResult<DistanceAndLocationDetail[]> Process();
    }
}