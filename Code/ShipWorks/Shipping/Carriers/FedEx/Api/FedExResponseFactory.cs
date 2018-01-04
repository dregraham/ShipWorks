using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Close.Response;
using ShipWorks.Shipping.Carriers.FedEx.Api.Close.Response.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Api.Registration.Response;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Api.Tracking.Response;
using ShipWorks.Shipping.Carriers.FedEx.Api.Tracking.Response.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Api.Void.Response;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Close;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Track;

namespace ShipWorks.Shipping.Carriers.FedEx.Api
{
    /// <summary>
    /// An implementation of the ICarrierResponseFactory for FedEx.
    /// </summary>
    [Component]
    public class FedExResponseFactory : IFedExResponseFactory
    {
        /// <summary>
        /// Creates an ICarrierResponse that will be to represent the carrier-specific result of a
        /// carrier API request when performing the end of day ground close.
        /// </summary>
        /// <param name="nativeResponse">The native response.</param>
        /// <param name="request">The request object that submitted the API request.</param>
        /// <returns>An ICarrierResponse representing the response of a ground close request.</returns>
        /// <exception cref="CarrierException">An unexpected response type was provided to create a FedExGroundCloseResponse.</exception>
        public ICarrierResponse CreateGroundCloseResponse(object nativeResponse, CarrierRequest request)
        {
            GroundCloseReply closeReply = nativeResponse as GroundCloseReply;

            if (closeReply == null)
            {
                // We can't create a FedExGroundResponse without a GroundCloseReply type
                throw new CarrierException("An unexpected response type was provided to create a FedExGroundCloseResponse.");
            }

            List<IFedExCloseResponseManipulator> manipulators = new List<IFedExCloseResponseManipulator>
            {
                new FedExGroundCloseReportManipulator()
            };

            return new FedExGroundCloseResponse(manipulators, closeReply, request);
        }

        /// <summary>
        /// Creates an ICarrierResponse that will be to represent the carrier-specific result of a
        /// carrier API request when performing the end of day SmartPost close.
        /// </summary>
        /// <param name="nativeResponse">The native response.</param>
        /// <param name="request">The request object that submitted the API request.</param>
        /// <returns>An ICarrierResponse representing the response of a SmartPost request.</returns>
        /// <exception cref="CarrierException">An unexpected response type was provided to create a FedExSmartPostCloseResponse.</exception>
        public ICarrierResponse CreateSmartPostCloseResponse(object nativeResponse, CarrierRequest request)
        {
            SmartPostCloseReply closeReply = nativeResponse as SmartPostCloseReply;

            if (closeReply == null)
            {
                // We can't create a FedExGroundResponse without a SmartPostCloseReply type
                throw new CarrierException("An unexpected response type was provided to create a FedExSmartPostCloseResponse.");
            }

            List<IFedExCloseResponseManipulator> manipulators = new List<IFedExCloseResponseManipulator>
            {
                new FedExSmartPostCloseEntityManipulator()
            };

            return new FedExSmartPostCloseResponse(manipulators, closeReply, request);
        }

        /// <summary>
        /// Creates an ICarrierResponse that will be to represent the carrier-specific result of a
        /// carrier API request when performing the void.
        /// </summary>
        /// <param name="nativeResponse">The native response.</param>
        /// <param name="request">The request object that submitted the API request.</param>
        /// <returns>An ICarrierResponse representing the response of a void request.</returns>
        public ICarrierResponse CreateVoidResponse(object nativeResponse, CarrierRequest request)
        {
            ShipmentReply voidShipmentReply = nativeResponse as ShipmentReply;

            if (voidShipmentReply == null)
            {
                // We can't create a FedExVoidResponse without a ShipmentReply type
                throw new CarrierException("An unexpected response type was provided to create a FedExVoidResponse.");
            }

            return new FedExVoidResponse(voidShipmentReply, request);
        }

        /// <summary>
        /// Creates an ICarrierResponse that will be to represent the carrier-specific result of a
        /// carrier API request when registering a new CSP user.
        /// </summary>
        /// <param name="nativeResponse">The native response.</param>
        /// <param name="request">The request object that submitted the API request.</param>
        /// <returns>An ICarrierResponse representing the response of a void request.</returns>
        public ICarrierResponse CreateRegisterUserResponse(object nativeResponse, CarrierRequest request)
        {
            RegisterWebUserReply registerReply = nativeResponse as RegisterWebUserReply;
            if (registerReply == null)
            {
                // We can't create a FedExRegisterUserResponse without a RegisterWebCspUserReply type
                throw new CarrierException("An unexpected response type was provided to create a FedExRegisterUserResponse.");
            }

            return new FedExRegisterCspUserResponse(registerReply, request);
        }

        /// <summary>
        /// Creates an ICarrierResponse that will be to represent the carrier-specific result of a
        /// carrier API request when subscribe a shipper to FedEx use the FedEx services.
        /// </summary>
        /// <param name="nativeResponse">The native response.</param>
        /// <param name="request">The request object that submitted the API request.</param>
        /// <returns>An ICarrierResponse representing the response of a void request.</returns>
        public ICarrierResponse CreateSubscriptionResponse(object nativeResponse, CarrierRequest request)
        {
            SubscriptionReply subscriptionReply = nativeResponse as SubscriptionReply;
            if (subscriptionReply == null)
            {
                // We can't create a FedExSubscriptionResponse without a SubscriptionReply type
                throw new CarrierException("An unexpected response type was provided to create a FedExSubscriptionResponse.");
            }

            return new FedExSubscriptionResponse(subscriptionReply, request);
        }

        /// <summary>
        /// Creates an ICarrierResponse that will be to represent the carrier-specific result of a
        /// carrier API request when tracking a shipment.
        /// </summary>
        /// <param name="nativeResponse">The native response.</param>
        /// <param name="request">The request object that submitted the API request.</param>
        /// <returns>An ICarrierResponse representing the response of a track request.</returns>
        public ICarrierResponse CreateTrackResponse(object nativeResponse, CarrierRequest request)
        {
            TrackReply trackReply = nativeResponse as TrackReply;

            if (trackReply == null)
            {
                // We can't create a FedExTrackResponse without a TrackReply type
                throw new CarrierException("An unexpected response type was provided to create a FedExTrackingResponse.");
            }

            List<IFedExTrackingResponseManipulator> manipulators = new List<IFedExTrackingResponseManipulator>
            {
                new FedExTrackingResponseManipulator()
            };

            return new FedExTrackingResponse(manipulators, request.ShipmentEntity, trackReply, request);
        }
    }
}
