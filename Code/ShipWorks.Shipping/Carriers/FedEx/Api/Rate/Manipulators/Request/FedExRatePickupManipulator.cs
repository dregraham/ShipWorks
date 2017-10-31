using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    /// <summary>
    /// An ICarrierRequestManipulator implementation that modifies the dropoff type property of the
    /// FedEx API's RateRequest object.
    /// </summary>
    public class FedExRatePickupManipulator : ICarrierRequestManipulator
    {
        /// <summary>
        /// Add the Recipient info to the FedEx carrier request
        /// </summary>
        /// <param name="request">The FedEx carrier request</param>
        public void Manipulate(CarrierRequest request)
        {
            // Make sure all of the properties we'll be accessing have been created
            InitializeRequest(request);

            // We can safely cast this since we've passed initialization
            RateRequest nativeRequest = request.NativeRequest as RateRequest;

            // Set the drop off type for the shipment
            nativeRequest.RequestedShipment.DropoffType = GetDropoffType(request.ShipmentEntity.FedEx);
            nativeRequest.RequestedShipment.DropoffTypeSpecified = true;
        }

        /// <summary>
        /// Gets the type of the dropoff.
        /// </summary>
        /// <param name="fedExShipment">The fed ex shipment.</param>
        /// <returns>The FedEx API DropoffType value.</returns>
        /// <exception cref="System.InvalidOperationException">Invalid FedEx ServiceType</exception>
        private DropoffType GetDropoffType(FedExShipmentEntity fedExShipment)
        {
            switch ((FedExDropoffType)fedExShipment.DropoffType)
            {
                case FedExDropoffType.BusinessServiceCenter: return DropoffType.BUSINESS_SERVICE_CENTER;
                case FedExDropoffType.DropBox: return DropoffType.DROP_BOX;
                case FedExDropoffType.RegularPickup: return DropoffType.REGULAR_PICKUP;
                case FedExDropoffType.RequestCourier: return DropoffType.REQUEST_COURIER;
                case FedExDropoffType.Station: return DropoffType.STATION;
            }

            throw new InvalidOperationException("Invalid FedEx ServiceType " + (FedExDropoffType)fedExShipment.DropoffType);
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="CarrierException">An unexpected request type was provided.</exception>
        public void InitializeRequest(CarrierRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            // The native FedEx request type should be a RateRequest
            RateRequest nativeRequest = request.NativeRequest as RateRequest;
            if (nativeRequest == null)
            {
                // Abort - we have an unexpected native request
                throw new CarrierException("An unexpected request type was provided.");
            }

            if (nativeRequest.RequestedShipment == null)
            {
                nativeRequest.RequestedShipment = new RequestedShipment();
            }
        }
    }
}
