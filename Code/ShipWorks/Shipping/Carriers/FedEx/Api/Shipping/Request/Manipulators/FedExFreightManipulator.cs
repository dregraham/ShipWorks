using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    /// <summary>
    /// Add Dry Ice information to shipment
    /// </summary>
    public class FedExFreightManipulator : FedExShippingRequestManipulatorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FedExFreightManipulator" /> class and 
        /// uses the the FedExSettingsRepository.
        /// </summary>
        public FedExFreightManipulator()
            : base(new FedExSettingsRepository())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExFreightManipulator" /> class.
        /// </summary>
        /// <param name="fedExSettings">The fed ex settings.</param>
        public FedExFreightManipulator(FedExSettings fedExSettings)
            : base(fedExSettings)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExFreightManipulator" /> class.
        /// </summary>
        public FedExFreightManipulator(ICarrierSettingsRepository settingsRepository) : base(settingsRepository)
        {
        }

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request being manipulated.</param>
        public override void Manipulate(CarrierRequest request)
        {
            Validate(request);

            FedExShipmentEntity fedex = request.ShipmentEntity.FedEx;
            // If we aren't freight, just return
            if (!FedExUtility.IsFreightService((FedExServiceType)fedex.Service))
            {
                return;
            }

            IFedExNativeShipmentRequest nativeRequest = InitializeShipmentRequest(request, fedex);

            // Add the express freight detail
            CreateFedExExpressFreightDetailManipulations(nativeRequest.RequestedShipment, fedex);
        }

        /// <summary>
        /// Add options for express freight
        /// </summary>
        private void CreateFedExExpressFreightDetailManipulations(RequestedShipment requestedShipment, FedExShipmentEntity fedex)
        {
            ExpressFreightDetail expressFreightDetail = requestedShipment.ExpressFreightDetail;
            expressFreightDetail.BookingConfirmationNumber = fedex.FreightBookingNumber;

            List<ShipmentSpecialServiceType> specialServiceTypes = new List<ShipmentSpecialServiceType>();
            specialServiceTypes.AddRange(requestedShipment.SpecialServicesRequested.SpecialServiceTypes);

            // So far, this needs to stay US...  even if shipping CA => CA
            if (fedex.Shipment.AdjustedShipCountryCode() == "US")
            {
                if (fedex.FreightInsideDelivery && !specialServiceTypes.Contains(ShipmentSpecialServiceType.INSIDE_DELIVERY))
                {
                    specialServiceTypes.Add(ShipmentSpecialServiceType.INSIDE_DELIVERY);
                }

                if (fedex.FreightInsidePickup && !specialServiceTypes.Contains(ShipmentSpecialServiceType.INSIDE_PICKUP))
                {
                    specialServiceTypes.Add(ShipmentSpecialServiceType.INSIDE_PICKUP);
                }
            }
            else if (fedex.FreightLoadAndCount > 0)
            {
                expressFreightDetail.ShippersLoadAndCount = fedex.FreightLoadAndCount.ToString();
            }

            // For certification, so far, everyting is true.  
            if (SettingsRepository.IsInterapptiveUser)
            {
                expressFreightDetail.PackingListEnclosed = true;
            }

            // Add shipping document types 
            AddShippingDocumentTypes(requestedShipment);

            // Set the special service types on the requested shipment
            requestedShipment.SpecialServicesRequested.SpecialServiceTypes = specialServiceTypes.ToArray();
        }

        /// <summary>
        /// Validates the specified request.
        /// </summary>
        private static void Validate(CarrierRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (!(request.NativeRequest is IFedExNativeShipmentRequest))
            {
                // Abort - we have an unexpected native request
                throw new CarrierException("An unexpected request type was provided.");
            }

            if (request.ShipmentEntity == null)
            {
                throw new CarrierException("request.ShipmentEntity is null");
            }

            if (request.ShipmentEntity.FedEx == null)
            {
                throw new CarrierException("request.ShipmentEntity.FedEx is null");
            }
        }

        /// <summary>
        /// Initialize the request properties needed for freight
        /// </summary>
        private static IFedExNativeShipmentRequest InitializeShipmentRequest(CarrierRequest request, FedExShipmentEntity fedex)
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

            // Make sure the ExpressFreightDetail is there
            if (nativeRequest.RequestedShipment.ExpressFreightDetail == null)
            {
                nativeRequest.RequestedShipment.ExpressFreightDetail = new ExpressFreightDetail();
            }

            // Make sure the SpecialSericesRequested is there
            if (nativeRequest.RequestedShipment.SpecialServicesRequested == null)
            {
                nativeRequest.RequestedShipment.SpecialServicesRequested = new ShipmentSpecialServicesRequested();
            }

            // Make sure the SpecialServiceTypes is there
            if (nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes == null)
            {
                nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = new ShipmentSpecialServiceType[0];
            }

            // Make sure the ShippingDocumentSpecification is there
            if (nativeRequest.RequestedShipment.ShippingDocumentSpecification == null)
            {
                nativeRequest.RequestedShipment.ShippingDocumentSpecification = new ShippingDocumentSpecification();
            }

            // Make sure the ShippingDocumentTypes is there
            if (nativeRequest.RequestedShipment.ShippingDocumentSpecification.ShippingDocumentTypes == null)
            {
                nativeRequest.RequestedShipment.ShippingDocumentSpecification.ShippingDocumentTypes = new RequestedShippingDocumentType[0];
            }

            return nativeRequest;
        }

        /// <summary>
        /// Adds the label shipping document type if it's not already present.
        /// </summary>
        private static void AddShippingDocumentTypes(RequestedShipment requestedShipment)
        {
            List<RequestedShippingDocumentType> shippingDocumentTypes = new List<RequestedShippingDocumentType>();
            shippingDocumentTypes.AddRange(requestedShipment.ShippingDocumentSpecification.ShippingDocumentTypes);

            if (!shippingDocumentTypes.Contains(RequestedShippingDocumentType.LABEL))
            {
                shippingDocumentTypes.Add(RequestedShippingDocumentType.LABEL);
            }
            
            requestedShipment.ShippingDocumentSpecification.ShippingDocumentTypes = shippingDocumentTypes.ToArray();
        }
    }
}
