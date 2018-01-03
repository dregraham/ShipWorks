using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.GlobalShipAddress
{
    /// <summary>
    /// Manipulator for FedEx GlobalAddressRequest
    /// </summary>
    [Service]
    public interface IFedExGlobalShipAddressRequestManipulator
    {
        /// <summary>
        /// Manipulate the request
        /// </summary>
        GenericResult<SearchLocationsRequest> Manipulate(IShipmentEntity shipment, SearchLocationsRequest request);
    }
}