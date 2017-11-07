using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.GlobalShipAddress.Request.Manipulators
{
    /// <summary>
    /// Request Manipulator to add Constraint information to nativeRequest.
    /// </summary>
    public class FedExGlobalShipAddressConstraintManipulator : IFedExGlobalShipAddressRequestManipulator
    {
        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public GenericResult<SearchLocationsRequest> Manipulate(IShipmentEntity shipment, SearchLocationsRequest request)
        {
            SupportedRedirectToHoldServiceType supportedRedirectToHoldServiceType = GetSupportedRedirectToHoldServiceType(shipment.FedEx.Service);

            request.Constraints = new SearchLocationConstraints
            {
                SupportedRedirectToHoldServices = new[]
                {
                    supportedRedirectToHoldServiceType
                },
                RadiusDistance = new Distance()
                {
                    Units = DistanceUnits.MI,
                    UnitsSpecified = true,
                    Value = 500.0,
                    ValueSpecified = true
                }
            };

            request.MultipleMatchesActionSpecified = true;
            request.MultipleMatchesAction = MultipleMatchesActionType.RETURN_ALL;

            return request;
        }

        /// <summary>
        /// Gets the type of hold required for shipment.
        /// </summary>
        private SupportedRedirectToHoldServiceType GetSupportedRedirectToHoldServiceType(int service)
        {
            FedExServiceType fedExService = (FedExServiceType) service;

            switch (fedExService)
            {
                case FedExServiceType.FedExGround:
                case FedExServiceType.FedExInternationalGround:
                    return SupportedRedirectToHoldServiceType.FEDEX_GROUND;
                case FedExServiceType.GroundHomeDelivery:
                    return SupportedRedirectToHoldServiceType.FEDEX_GROUND_HOME_DELIVERY;
                default:
                    return SupportedRedirectToHoldServiceType.FEDEX_EXPRESS;
            }
        }
    }
}
