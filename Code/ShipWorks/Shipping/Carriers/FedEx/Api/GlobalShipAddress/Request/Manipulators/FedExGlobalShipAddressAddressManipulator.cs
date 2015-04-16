using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.GlobalShipAddress.Request.Manipulators
{
    /// <summary>
    /// Manipulator for adding shipment address to nativeRequest.
    /// </summary>
    public class FedExGlobalShipAddressAddressManipulator:ICarrierRequestManipulator
    {
        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        /// <exception cref="CarrierException">An unexpected request type was provided.</exception>
        public void Manipulate(CarrierRequest request)
        {
            SearchLocationsRequest nativeRequest = request.NativeRequest as SearchLocationsRequest;

            if (nativeRequest == null)
            {
                throw new CarrierException("An unexpected request type was provided.");
            }

            nativeRequest.Address = new Address()
            {
                StreetLines = new[]
                {
                    request.ShipmentEntity.ShipStreet1,
                    request.ShipmentEntity.ShipStreet2
                },
                City = request.ShipmentEntity.ShipCity,
                StateOrProvinceCode = request.ShipmentEntity.ShipStateProvCode,
                PostalCode = request.ShipmentEntity.ShipPostalCode,
                CountryCode = request.ShipmentEntity.AdjustedShipCountryCode(),
                Residential = request.ShipmentEntity.ResidentialResult
            };

            nativeRequest.LocationsSearchCriterion = LocationsSearchCriteriaType.ADDRESS;
            nativeRequest.LocationsSearchCriterionSpecified = true;
        }
    }
}
