using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    /// <summary>
    /// An implementation of the ICarrierRequestManipulator interface that configures the 
    /// FedEx IFedExNativeShipmentRequest object with home delivery options depending on the 
    /// shipment entity.
    /// </summary>
    public class FedExHomeDeliveryManipulator : FedExShippingRequestManipulatorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FedExHomeDeliveryManipulator" /> class.
        /// </summary>
        public FedExHomeDeliveryManipulator()
            : this(new FedExSettings(new FedExSettingsRepository()))
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExHomeDeliveryManipulator" /> class.
        /// </summary>
        /// <param name="fedExSettings">The fed ex settings.</param>
        public FedExHomeDeliveryManipulator(FedExSettings fedExSettings)
            : base(fedExSettings)
        {
        }

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public override void Manipulate(CarrierRequest request)
        {
            // Make sure all of the properties we'll be accessing have been created
            InitializeRequest(request);

            // We can safely cast this since we've passed initialization
            IFedExNativeShipmentRequest nativeRequest = request.NativeRequest as IFedExNativeShipmentRequest;
            
            FedExShipmentEntity fedExShipment = request.ShipmentEntity.FedEx;
            FedExHomeDeliveryType homeDeliveryType = (FedExHomeDeliveryType) fedExShipment.HomeDeliveryType;

            if (fedExShipment.Service == (int) FedExServiceType.GroundHomeDelivery && homeDeliveryType != FedExHomeDeliveryType.None)
            {
                // This shipment is configured for home delivery, so configure the service tyep and home delivery details
                ConfigureServiceType(nativeRequest);
                ConfigureHomeDeliveryDetails(nativeRequest, homeDeliveryType, fedExShipment);

                // Finally use the home delivery instructions for the shipment
                nativeRequest.RequestedShipment.DeliveryInstructions = fedExShipment.HomeDeliveryInstructions;
            }
        }

        /// <summary>
        /// Gets the service type list.
        /// </summary>
        /// <param name="nativeRequest">The native request.</param>
        private void ConfigureServiceType(IFedExNativeShipmentRequest nativeRequest)
        {
            if (nativeRequest.RequestedShipment.SpecialServicesRequested == null)
            {
                // Special service types hang off the special services requested object
                nativeRequest.RequestedShipment.SpecialServicesRequested = new ShipmentSpecialServicesRequested();
            }

            if (nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes == null)
            {
                // Initialize the service types
                nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = new ShipmentSpecialServiceType[0];
            }

            // Convert the service types to a list now that we're sure the service types have been created and 
            // add the home premium service
            List<ShipmentSpecialServiceType> serviceTypes = nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.ToList();
            serviceTypes.Add(ShipmentSpecialServiceType.HOME_DELIVERY_PREMIUM);

            nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = serviceTypes.ToArray();
        }

        /// <summary>
        /// Configures the home delivery details of the IFedExNativeShipmentRequest.
        /// </summary>
        /// <param name="nativeRequest">The native request.</param>
        /// <param name="homeDeliveryType">Type of the home delivery.</param>
        /// <param name="fedExShipment">The FedEx shipment.</param>
        private void ConfigureHomeDeliveryDetails(IFedExNativeShipmentRequest nativeRequest, FedExHomeDeliveryType homeDeliveryType, FedExShipmentEntity fedExShipment)
        {
            // Figure out the FedEx API premium type value
            HomeDeliveryPremiumType premiumType;
            switch (homeDeliveryType)
            {
                case FedExHomeDeliveryType.DateCertain:
                    premiumType = HomeDeliveryPremiumType.DATE_CERTAIN;
                    break;
                case FedExHomeDeliveryType.Evening:
                    premiumType = HomeDeliveryPremiumType.EVENING;
                    break;
                default:
                    premiumType = HomeDeliveryPremiumType.APPOINTMENT;
                    break;
            }

            HomeDeliveryPremiumDetail homeDeliveryDetail = new HomeDeliveryPremiumDetail();
            homeDeliveryDetail.HomeDeliveryPremiumType = premiumType;
            homeDeliveryDetail.PhoneNumber = fedExShipment.HomeDeliveryPhone;

            if (homeDeliveryType == FedExHomeDeliveryType.DateCertain)
            {
                homeDeliveryDetail.Date = fedExShipment.HomeDeliveryDate;
                homeDeliveryDetail.DateSpecified = true;
            }

            nativeRequest.RequestedShipment.SpecialServicesRequested.HomeDeliveryPremiumDetail = homeDeliveryDetail;
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="CarrierException">An unexpected request type was provided.</exception>
        private void InitializeRequest(CarrierRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            // The native FedEx request type should be a IFedExNativeShipmentRequest
            IFedExNativeShipmentRequest nativeRequest = request.NativeRequest as IFedExNativeShipmentRequest;
            if (nativeRequest == null)
            {
                // Abort - we have an unexpected native request
                throw new CarrierException("An unexpected request type was provided.");
            }

            // Make sure the RequestedShipment is there
            if (nativeRequest.RequestedShipment == null)
            {
                // We'll be manipulating properties of the requested shipment, so make sure it's been created
                nativeRequest.RequestedShipment = new RequestedShipment();
            }
        }
    }
}
