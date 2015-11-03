using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    /// <summary>
    /// Manipulator for adding shipper information to the FedEx request
    /// </summary>
    public class FedExHoldAtLocationManipulator : FedExShippingRequestManipulatorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FedExHoldAtLocationManipulator" /> class and 
        /// uses the the FedExSettingsRepository.
        /// </summary>
        public FedExHoldAtLocationManipulator()
            : base(new FedExSettingsRepository())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExShippingRequestManipulatorBase" /> class.
        /// </summary>
        /// <param name="fedExSettings">The fed ex settings.</param>
        public FedExHoldAtLocationManipulator(FedExSettings fedExSettings) :base(fedExSettings)
        {
        }

        /// <summary>
        /// Add the Shipper info to the FedEx carrier request
        /// </summary>
        /// <param name="request">The FedEx carrier request</param>
        [NDependIgnoreLongMethod]
        public override void Manipulate(CarrierRequest request)
        {
            // Get the RequestedShipment object for the request
            RequestedShipment requestedShipment = FedExRequestManipulatorUtilities.GetShipServiceRequestedShipment(request);

            // Get the contact and address for the shipment
            ShipmentEntity shipment = request.ShipmentEntity;

            FedExShipmentEntity fedExShipment = shipment.FedEx;

            if (fedExShipment.FedExHoldAtLocationEnabled)
            {
                InitializeRequest(request);

                if (requestedShipment.SpecialServicesRequested.SpecialServiceTypes == null)
                {
                    requestedShipment.SpecialServicesRequested.SpecialServiceTypes = new ShipmentSpecialServiceType[0];
                }

                // Add the hold at location service type
                List<ShipmentSpecialServiceType> serviceTypes = requestedShipment.SpecialServicesRequested.SpecialServiceTypes.ToList();
                serviceTypes.Add(ShipmentSpecialServiceType.HOLD_AT_LOCATION);
                requestedShipment.SpecialServicesRequested.SpecialServiceTypes = serviceTypes.ToArray();

                HoldAtLocationDetail holdAtLocationDetail = requestedShipment.SpecialServicesRequested.HoldAtLocationDetail;

                holdAtLocationDetail.PhoneNumber = shipment.ShipPhone;

                holdAtLocationDetail.LocationContactAndAddress.Address.City = fedExShipment.HoldCity;
                holdAtLocationDetail.LocationContactAndAddress.Address.CountryCode = fedExShipment.HoldCountryCode;
                holdAtLocationDetail.LocationContactAndAddress.Address.PostalCode = fedExShipment.HoldPostalCode;
                holdAtLocationDetail.LocationContactAndAddress.Address.Residential = fedExShipment.HoldResidential.HasValue && fedExShipment.HoldResidential.Value;
                holdAtLocationDetail.LocationContactAndAddress.Address.ResidentialSpecified = fedExShipment.HoldResidential.HasValue;
                holdAtLocationDetail.LocationContactAndAddress.Address.StateOrProvinceCode = fedExShipment.HoldStateOrProvinceCode;

                List<string> streetLines = new List<string>
                {
                    fedExShipment.HoldStreet1
                };
                if (!string.IsNullOrWhiteSpace(fedExShipment.HoldStreet2))
                {
                    streetLines.Add(fedExShipment.HoldStreet2);
                }
                // FedEx API does not support three lines in the street address

                // TODO: I don't think the fields below are required as they are not returned by the FedEx sample API.
                // UrbanizationCode
                // ContactId
                // PagerNumber
                // PersonName
                // PhoneExtension
                // PhoneNumber
                // Title
                // HoldPhoneNumber

                holdAtLocationDetail.LocationContactAndAddress.Address.StreetLines = streetLines.ToArray();
                holdAtLocationDetail.LocationContactAndAddress.Address.UrbanizationCode = fedExShipment.HoldUrbanizationCode;

                holdAtLocationDetail.LocationContactAndAddress.Contact.CompanyName = fedExShipment.HoldCompanyName;
                holdAtLocationDetail.LocationContactAndAddress.Contact.ContactId = fedExShipment.HoldContactId;
                holdAtLocationDetail.LocationContactAndAddress.Contact.EMailAddress = fedExShipment.HoldEmailAddress;
                holdAtLocationDetail.LocationContactAndAddress.Contact.FaxNumber = fedExShipment.HoldFaxNumber;
                holdAtLocationDetail.LocationContactAndAddress.Contact.PagerNumber = fedExShipment.HoldPagerNumber;
                holdAtLocationDetail.LocationContactAndAddress.Contact.PersonName = fedExShipment.HoldPersonName;
                holdAtLocationDetail.LocationContactAndAddress.Contact.PhoneExtension = fedExShipment.HoldPhoneExtension;
                holdAtLocationDetail.LocationContactAndAddress.Contact.PhoneNumber = fedExShipment.HoldPhoneNumber;
                holdAtLocationDetail.LocationContactAndAddress.Contact.Title = fedExShipment.HoldTitle;

                holdAtLocationDetail.LocationTypeSpecified = fedExShipment.HoldLocationType.HasValue;
                if (fedExShipment.HoldLocationType.HasValue)
                {
                    holdAtLocationDetail.LocationType = (FedExLocationType) fedExShipment.HoldLocationType;
                }
            }
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="CarrierException">An unexpected request type was provided.</exception>
        private static void InitializeRequest(CarrierRequest request)
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

            if (nativeRequest.RequestedShipment == null)
            {
                // We'll be manipulating properties of the requested shipment, so make sure it's been created
                nativeRequest.RequestedShipment = new RequestedShipment();
            }

            if (nativeRequest.RequestedShipment.SpecialServicesRequested == null)
            {
                // We'll be manipulating properties of the special services, so make sure it's been created
                nativeRequest.RequestedShipment.SpecialServicesRequested = new ShipmentSpecialServicesRequested();
            }

            if (nativeRequest.RequestedShipment.SpecialServicesRequested.HoldAtLocationDetail == null)
            {
                // We'll be manipulating properties of the hold at location detail, so make sure it's been created
                nativeRequest.RequestedShipment.SpecialServicesRequested.HoldAtLocationDetail = new HoldAtLocationDetail();
            }

            HoldAtLocationDetail holdAtLocationDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.HoldAtLocationDetail;

            if (holdAtLocationDetail.LocationContactAndAddress == null)
            {
                // We'll be manipulating properties of the contact and address, so make sure it's been created
                holdAtLocationDetail.LocationContactAndAddress = new ContactAndAddress();
            }

            if (holdAtLocationDetail.LocationContactAndAddress.Address == null)
            {
                // We'll be manipulating properties of the address, so make sure it's been created
                holdAtLocationDetail.LocationContactAndAddress.Address = new Address();
            }

            if (holdAtLocationDetail.LocationContactAndAddress.Contact == null)
            {
                // We'll be manipulating properties of the contact, so make sure it's been created
                holdAtLocationDetail.LocationContactAndAddress.Contact = new Contact();
            }
        }
    }
}
