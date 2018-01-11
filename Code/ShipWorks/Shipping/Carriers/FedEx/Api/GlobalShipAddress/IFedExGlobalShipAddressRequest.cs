using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.GlobalShipAddress
{
    /// <summary>
    /// GlobalShipAddress Request - Used to get Hold At Location addresses.
    /// </summary>
    public interface IFedExGlobalShipAddressRequest
    {
        /// <summary>
        /// Submits the request to the carrier API.
        /// </summary>
        /// <returns>
        /// The GlobalShipReply or an error
        /// </returns>
        GenericResult<IFedExGlobalShipAddressResponse> Submit(IShipmentEntity shipment);
    }
}