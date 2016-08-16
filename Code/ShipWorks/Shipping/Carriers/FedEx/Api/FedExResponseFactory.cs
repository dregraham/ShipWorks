using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Close.Response;
using ShipWorks.Shipping.Carriers.FedEx.Api.Close.Response.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Api.GlobalShipAddress.Response;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Response;
using ShipWorks.Shipping.Carriers.FedEx.Api.Registration.Response;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Api.Tracking.Response;
using ShipWorks.Shipping.Carriers.FedEx.Api.Tracking.Response.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Api.Void.Response;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Close;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Track;

namespace ShipWorks.Shipping.Carriers.FedEx.Api
{
    /// <summary>
    /// An implementation of the ICarrierResponseFactory for FedEx.
    /// </summary>
    public class FedExResponseFactory : ICarrierResponseFactory
    {
        private readonly ILabelRepository labelRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExResponseFactory(ILabelRepository labelRepository)
        {
            this.labelRepository = labelRepository;
        }

        /// <summary>
        /// Creates an ICarrierResponse that will be to represent the carrier-specific result of a
        /// carrier API request to ship an order/create a label.
        /// </summary>
        /// <param name="nativeResponse">The native response (WSDL object, raw XML, etc.) that is received from the carrier.</param>
        /// <param name="request">The request object that submitted the API request.</param>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <returns> An ICarrierResponse representing the response of a shipment request.</returns>
        /// <exception cref="CarrierException">An unexpected response type was provided to create a FedExShipResponse.</exception>
        public ICarrierResponse CreateShipResponse(object nativeResponse, CarrierRequest request, ShipmentEntity shipmentEntity)
        {
            IFedExNativeShipmentReply processShipmentReply = nativeResponse as IFedExNativeShipmentReply;

            if (processShipmentReply == null)
            {
                // We can't create a FedExShipResponse without a ProcessShipmentReply type
                throw new CarrierException("An unexpected response type was provided to create a FedExShipResponse.");
            }

            // Add the appropriate shipment manipulators that will be needed to process the ship response
            List<ICarrierResponseManipulator> shipmentManipulators = new List<ICarrierResponseManipulator>
            {
                new FedExShipmentTrackingManipulator(),
                new FedExShipmentCodManipulator(),
                new FedExShipmentCostManipulator()
            };
            
            return new FedExShipResponse(processShipmentReply, request, shipmentEntity, labelRepository, shipmentManipulators);
        }

        /// <summary>
        /// Creates the ICarrierResponse that will be to represent the carrier-specific result of a
        /// carrier API request to get Pickup Location if supported by API
        /// </summary>
        /// <param name="nativeResponse">The native response object received from the carrier.</param>
        /// <param name="request">The request object that submitted the API request.</param>
        /// <returns>An ICarrierResponse representing the response of a Pickup Location request.</returns>
        /// <exception cref="CarrierException">An unexpected response type was provided to create a FedExGlobalShipAddressResponse.</exception>
        public ICarrierResponse CreateGlobalShipAddressResponse(object nativeResponse, CarrierRequest request)
        {
            if (nativeResponse == null)
            {
                throw new ArgumentNullException("nativeResponse", "Null argument passed to FedExResponseFactory.CreateGlobalShipAddressResponse");
            }

            SearchLocationsReply locationsReply = nativeResponse as SearchLocationsReply;

            if (locationsReply == null)
            {
                // We can't create a FedExGlobalShipAddressResponse without a SearchLocationsReply
                throw new CarrierException("An unexpected response type was provided to create a FedExGlobalShipAddressResponse.");
            }

            return new FedExGlobalShipAddressResponse(locationsReply, request);
        }


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

            //FedExEndOfDayCloseEntity closeEntity = null;
            return new FedExGroundCloseResponse(manipulators, closeReply, request);//, closeEntity);
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
        /// carrier API request for shipping rates.
        /// </summary>
        /// <param name="nativeResponse">The native response.</param>
        /// <param name="request">The request object that submitted the API request.</param>
        /// <returns>An ICarrierResponse representing the response of a void request.</returns>
        public ICarrierResponse CreateRateResponse(object nativeResponse, CarrierRequest request)
        {
            RateReply rateReply = nativeResponse as RateReply;
            if (rateReply == null)
            {
                // We can't create a FedExRateResponse without a RateReply type
                throw new CarrierException("An unexpected response type was provided to create a FedExRateResponse.");
            }

            return new FedExRateResponse(rateReply, request);
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
