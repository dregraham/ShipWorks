using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    /// <summary>
    /// Manipulator for adding shipper information to the FedEx request
    /// </summary>
    public class FedExHoldAtLocationManipulator : IFedExShipRequestManipulator
    {
        /// <summary>
        /// Should the manipulator be applied
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, int sequenceNumber) =>
            shipment.FedEx.FedExHoldAtLocationEnabled;

        /// <summary>
        /// Add the Shipper info to the FedEx carrier request
        /// </summary>
        public GenericResult<ProcessShipmentRequest> Manipulate(IShipmentEntity shipment, ProcessShipmentRequest request, int sequenceNumber)
        {
            Initialize(request);

            if (request.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes == null)
            {
                request.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = new ShipmentSpecialServiceType[0];
            }

            // Add the hold at location service type
            List<ShipmentSpecialServiceType> serviceTypes = request.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.ToList();
            serviceTypes.Add(ShipmentSpecialServiceType.HOLD_AT_LOCATION);
            request.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = serviceTypes.ToArray();

            HoldAtLocationDetail holdAtLocationDetail = request.RequestedShipment.SpecialServicesRequested.HoldAtLocationDetail;

            holdAtLocationDetail.PhoneNumber = shipment.ShipPhone;
            holdAtLocationDetail.LocationContactAndAddress.Address = BuildAddress(shipment.FedEx);
            holdAtLocationDetail.LocationContactAndAddress.Contact = BuildContact(shipment.FedEx);

            return shipment.FedEx.HoldLocationType.Match(
                x => ApplyHoldAtLocationType((Enums.FedExLocationType) x, request),
                () => request);
        }

        /// <summary>
        /// Apply hold at location type
        /// </summary>
        private static GenericResult<ProcessShipmentRequest> ApplyHoldAtLocationType(Enums.FedExLocationType type, ProcessShipmentRequest request) =>
            GetLocationType(type).Map(x =>
                {
                    var detail = request.RequestedShipment.SpecialServicesRequested.HoldAtLocationDetail;
                    detail.LocationType = x;
                    detail.LocationTypeSpecified = true;
                    return request;
                });

        /// <summary>
        /// Build contact
        /// </summary>
        private static Contact BuildContact(IFedExShipmentEntity fedExShipment)
        {
            return new Contact
            {
                CompanyName = fedExShipment.HoldCompanyName,
                ContactId = fedExShipment.HoldContactId,
                EMailAddress = fedExShipment.HoldEmailAddress,
                FaxNumber = fedExShipment.HoldFaxNumber,
                PagerNumber = fedExShipment.HoldPagerNumber,
                PersonName = fedExShipment.HoldPersonName,
                PhoneExtension = fedExShipment.HoldPhoneExtension,
                PhoneNumber = fedExShipment.HoldPhoneNumber,
                Title = fedExShipment.HoldTitle,
            };
        }

        /// <summary>
        /// Build address
        /// </summary>
        private static Address BuildAddress(IFedExShipmentEntity fedExShipment)
        {
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

            return new Address
            {
                City = fedExShipment.HoldCity,
                CountryCode = fedExShipment.HoldCountryCode,
                PostalCode = fedExShipment.HoldPostalCode,
                Residential = fedExShipment.HoldResidential.HasValue && fedExShipment.HoldResidential.Value,
                ResidentialSpecified = fedExShipment.HoldResidential.HasValue,
                StateOrProvinceCode = fedExShipment.HoldStateOrProvinceCode,
                StreetLines = streetLines.ToArray(),
                UrbanizationCode = fedExShipment.HoldUrbanizationCode,
            };
        }

        /// <summary>
        /// Get the location type API value from ShipWorks location type
        /// </summary>
        private static GenericResult<WebServices.Ship.FedExLocationType> GetLocationType(Enums.FedExLocationType holdLocationType)
        {
            switch (holdLocationType)
            {
                case Enums.FedExLocationType.FedExExpressStation: return WebServices.Ship.FedExLocationType.FEDEX_EXPRESS_STATION;
                case Enums.FedExLocationType.FedExFreightServiceCenter: return WebServices.Ship.FedExLocationType.FEDEX_FREIGHT_SERVICE_CENTER;
                case Enums.FedExLocationType.FedExGroundTerminal: return WebServices.Ship.FedExLocationType.FEDEX_GROUND_TERMINAL;
                case Enums.FedExLocationType.FedExHomeDeliveryStation: return WebServices.Ship.FedExLocationType.FEDEX_HOME_DELIVERY_STATION;
                case Enums.FedExLocationType.FedExOffice: return WebServices.Ship.FedExLocationType.FEDEX_OFFICE;
                case Enums.FedExLocationType.FedExSmartPostHub: return WebServices.Ship.FedExLocationType.FEDEX_SMART_POST_HUB;
                default:
                    return new FedExException($"Invalid FedExLocationType: {holdLocationType}");
            }
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        private static void Initialize(ProcessShipmentRequest request)
        {
            var specialServices = request.Ensure(x => x.RequestedShipment)
                .Ensure(x => x.SpecialServicesRequested);
            specialServices.Ensure(x => x.SpecialServiceTypes);
            specialServices.Ensure(x => x.HoldAtLocationDetail)
                .Ensure(x => x.LocationContactAndAddress);
        }
    }
}
