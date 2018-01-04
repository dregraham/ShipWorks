using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.GlobalShipAddress.Request.Manipulators
{
    /// <summary>
    /// An implementation of the ICarrierRequestManipulator interface that will manipulate the
    /// Version information of a SearchLocationsRequest object.
    /// </summary>
    public class FedExGlobalShipAddressVersionManipulator : IFedExGlobalShipAddressRequestManipulator
    {
        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public GenericResult<SearchLocationsRequest> Manipulate(IShipmentEntity shipment, SearchLocationsRequest request)
        {
            request.Version = new VersionId()
            {
                ServiceId = "gsai",
                Major = 2,
                Intermediate = 0,
                Minor = 0
            };

            return request;
        }
    }
}
