using System;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Api;


namespace ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    /// <summary>
    /// Add Express Freight information to shipment
    /// </summary>
    public class FedExRateExpressFreightManipulator : IFedExRateRequestManipulator
    {
        private readonly IFedExSettingsRepository settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExRateExpressFreightManipulator" /> class.
        /// </summary>
        /// <param name="settingsRepository">The settings repository.</param>
        public FedExRateExpressFreightManipulator(IFedExSettingsRepository settings)
        {
            this.settings = settings;
        }

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        public RateRequest Manipulate(IShipmentEntity shipment, RateRequest request)
        {
            if (!ShouldApply(shipment, FedExRateRequestOptions.None))
            {
                return request;
            }

            Validate(request, shipment);

            IFedExShipmentEntity fedex = shipment.FedEx;

            // Make sure all of the properties we'll be accessing have been created
            InitializeRequest(request);

            // Add the express freight detail
            CreateFedExExpressFreightDetailManipulations(request.RequestedShipment, fedex);

            return request;
        }

        /// <summary>
        /// Does this manipulator apply to the shipment
        /// </summary>
        /// <returns></returns>
        public bool ShouldApply(IShipmentEntity shipment, FedExRateRequestOptions options)
        {
            return FedExUtility.IsFreightExpressService(shipment.FedEx.Service);
        }

        /// <summary>
        /// Add options for express freight
        /// </summary>
        private void CreateFedExExpressFreightDetailManipulations(RequestedShipment requestedShipment, IFedExShipmentEntity fedex)
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

            if (fedex.FreightLoadAndCount > 0)
            {
                expressFreightDetail.ShippersLoadAndCount = fedex.FreightLoadAndCount.ToString();
            }

            // For certification, so far, everything is true.  
            if (settings.IsInterapptiveUser)
            {
                expressFreightDetail.PackingListEnclosed = true;
                expressFreightDetail.PackingListEnclosedSpecified = true;
            }

            // Add shipping document types 
            AddShippingDocumentTypes(requestedShipment);

            // Set the special service types on the requested shipment
            requestedShipment.SpecialServicesRequested.SpecialServiceTypes = specialServiceTypes.ToArray();
        }

        /// <summary>
        /// Validates the specified request.
        /// </summary>
        private static void Validate(RateRequest request, IShipmentEntity shipment)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (shipment == null)
            {
                throw new CarrierException("request.ShipmentEntity is null");
            }

            if (shipment.FedEx == null)
            {
                throw new CarrierException("request.ShipmentEntity.FedEx is null");
            }
        }

        /// <summary>
        /// Initialize the request properties needed for freight
        /// </summary>
        private void InitializeRequest(RateRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            // The native FedEx request type should be a RateRequest
            if (request == null)
            {
                // Abort - we have an unexpected native request
                throw new CarrierException("An unexpected request type was provided.");
            }

            // Make sure the RequestedShipment is there
            if (request.RequestedShipment == null)
            {
                // We'll be manipulating properties of the requested shipment, so make sure it's been created
                request.RequestedShipment = new RequestedShipment();
            }

            // Make sure the ExpressFreightDetail is there
            if (request.RequestedShipment.ExpressFreightDetail == null)
            {
                request.RequestedShipment.ExpressFreightDetail = new ExpressFreightDetail();
            }

            // Make sure the SpecialSericesRequested is there
            if (request.RequestedShipment.SpecialServicesRequested == null)
            {
                request.RequestedShipment.SpecialServicesRequested = new ShipmentSpecialServicesRequested();
            }

            // Make sure the SpecialServiceTypes is there
            if (request.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes == null)
            {
                request.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = new ShipmentSpecialServiceType[0];
            }

            // Make sure the ShippingDocumentSpecification is there
            if (request.RequestedShipment.ShippingDocumentSpecification == null)
            {
                request.RequestedShipment.ShippingDocumentSpecification = new ShippingDocumentSpecification();
            }

            // Make sure the ShippingDocumentTypes is there
            if (request.RequestedShipment.ShippingDocumentSpecification.ShippingDocumentTypes == null)
            {
                request.RequestedShipment.ShippingDocumentSpecification.ShippingDocumentTypes = new RequestedShippingDocumentType[0];
            }
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
