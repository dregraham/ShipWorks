﻿using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.GlobalShipAddress;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.v2013.GlobalShipAddress.Request.Manipulators
{
    /// <summary>
    /// Request Manipulator to add Constraint information to nativeRequest.
    /// </summary>
    public class FedExGlobalShipAddressConstraintManipulator : ICarrierRequestManipulator
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

            SupportedRedirectToHoldServiceType supportedRedirectToHoldServiceType = GetSupportedRedirectToHoldServiceType(request.ShipmentEntity.FedEx.Service);

            nativeRequest.Constraints = new SearchLocationConstraints
            {
                SupportedRedirectToHoldServices = new[]
                {
                    supportedRedirectToHoldServiceType
                }
            };

            nativeRequest.MultipleMatchesActionSpecified = true;
            nativeRequest.MultipleMatchesAction = MultipleMatchesActionType.RETURN_ALL;
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
                    return SupportedRedirectToHoldServiceType.FEDEX_GROUND;
                case FedExServiceType.GroundHomeDelivery:
                    return SupportedRedirectToHoldServiceType.FEDEX_GROUND_HOME_DELIVERY;
                default:
                    return SupportedRedirectToHoldServiceType.FEDEX_EXPRESS;
            }
        }
    }
}
