using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Void.Request.Manipulators
{
    /// <summary>
    /// An ICarrierRequestManipulator that will manipulate the void parameters of a VoidRequest object.
    /// </summary>
    public class FedExVoidParametersManipulator : ICarrierRequestManipulator
    {
        public void Manipulate(CarrierRequest request)
        {
            // Make sure all of the properties we'll be accessing have been created
            ValidateRequest(request);

            ShipmentEntity shipment = request.ShipmentEntity;

            // We can safely cast this since we've passed validation
            DeleteShipmentRequest nativeRequest = request.NativeRequest as DeleteShipmentRequest;

            TrackingIdType trackingIdType;

            FedExServiceType serviceType = (FedExServiceType)shipment.FedEx.Service;
            if (FedExUtility.IsGroundService(serviceType))
            {
                trackingIdType = TrackingIdType.GROUND;
            }
            else if (serviceType == FedExServiceType.SmartPost)
            {
                trackingIdType = TrackingIdType.USPS;
            }
            else
            {
                trackingIdType = TrackingIdType.EXPRESS;
            }

            nativeRequest.TrackingId = new TrackingId
            {
                TrackingNumber = FedExUtility.GetTrackingNumberForApi(shipment.TrackingNumber, shipment.FedEx),
                TrackingIdType = trackingIdType,
                TrackingIdTypeSpecified = true
            };

            nativeRequest.DeletionControl = DeletionControlType.DELETE_ALL_PACKAGES;
        }

        /// <summary>
        /// Validates the request making sure it is not null and of the correct type.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="CarrierException">An unexpected request type was provided.</exception>
        private static void ValidateRequest(CarrierRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            if (request.ShipmentEntity == null)
            {
                throw new InvalidOperationException("ValidateRequest received a request.ShipmentEntity that was null.");
            }

            // The native FedEx request type should be a DeleteShipmentRequest
            DeleteShipmentRequest nativeRequest = request.NativeRequest as DeleteShipmentRequest;
            if (nativeRequest == null)
            {
                // Abort - we have an unexpected native request
                throw new CarrierException("An unexpected request type was provided.");
            }
        }
    }
}
