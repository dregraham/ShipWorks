using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.GlobalShipAddress.Request.Manipulators
{
    /// <summary>
    /// Manipulator for adding shipment address to nativeRequest.
    /// </summary>
    public class FedExGlobalShipAddressAddressManipulator : IFedExGlobalShipAddressRequestManipulator
    {
        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        public GenericResult<SearchLocationsRequest> Manipulate(IShipmentEntity shipment, SearchLocationsRequest request)
        {
            request.Address = new Address()
            {
                StreetLines = new[]
                {
                    shipment.ShipStreet1,
                    shipment.ShipStreet2
                },
                City = shipment.ShipCity,
                StateOrProvinceCode = shipment.ShipStateProvCode,
                PostalCode = shipment.ShipPostalCode,
                CountryCode = shipment.AdjustedShipCountryCode(),
                Residential = shipment.ResidentialResult
            };

            request.LocationsSearchCriterion = LocationsSearchCriteriaType.ADDRESS;
            request.LocationsSearchCriterionSpecified = true;

            return request;
        }
    }
}
