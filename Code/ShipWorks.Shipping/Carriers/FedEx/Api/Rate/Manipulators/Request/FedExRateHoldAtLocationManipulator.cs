using System.Linq;
using Interapptive.Shared.Collections;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    /// <summary>
    /// Manipulator for adding shipper information to the FedEx request
    /// </summary>
    public class FedExRateHoldAtLocationManipulator : IFedExRateRequestManipulator
    {
        /// <summary>
        /// Should the manipulator be applied
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, FedExRateRequestOptions options) =>
            shipment.FedEx.FedExHoldAtLocationEnabled;

        /// <summary>
        /// Add the Shipper info to the FedEx carrier request
        /// </summary>
        public RateRequest Manipulate(IShipmentEntity shipment, RateRequest request)
        {
            InitializeRequest(request);
            var specialServices = request.RequestedShipment.SpecialServicesRequested;

            specialServices.SpecialServiceTypes = specialServices.SpecialServiceTypes
                .Append(ShipmentSpecialServiceType.HOLD_AT_LOCATION).ToArray();

            HoldAtLocationDetail holdAtLocationDetail = specialServices.HoldAtLocationDetail;

            holdAtLocationDetail.PhoneNumber = shipment.ShipPhone;
            holdAtLocationDetail.LocationContactAndAddress.Address = BuildAddress(shipment.FedEx);
            holdAtLocationDetail.LocationContactAndAddress.Contact = BuildContact(shipment.FedEx);

            holdAtLocationDetail.LocationTypeSpecified = shipment.FedEx.HoldLocationType.HasValue;
            if (shipment.FedEx.HoldLocationType.HasValue)
            {
                holdAtLocationDetail.LocationType = (FedExLocationType) shipment.FedEx.HoldLocationType;
            }

            return request;
        }

        /// <summary>
        /// Build the contact element
        /// </summary>
        private Contact BuildContact(IFedExShipmentEntity fedExShipment)
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
        /// Build the address element
        /// </summary>
        private Address BuildAddress(IFedExShipmentEntity fedExShipment)
        {
            // FedEx API does not support three lines in the street address
            var lines = string.IsNullOrWhiteSpace(fedExShipment.HoldStreet2) ?
                new[] { fedExShipment.HoldStreet1 } :
                new[] { fedExShipment.HoldStreet1, fedExShipment.HoldStreet2 };

            return new Address
            {
                City = fedExShipment.HoldCity,
                CountryCode = fedExShipment.HoldCountryCode,
                PostalCode = fedExShipment.HoldPostalCode,
                Residential = fedExShipment.HoldResidential.HasValue && fedExShipment.HoldResidential.Value,
                ResidentialSpecified = fedExShipment.HoldResidential.HasValue,
                StateOrProvinceCode = fedExShipment.HoldStateOrProvinceCode,
                StreetLines = lines,
                UrbanizationCode = fedExShipment.HoldUrbanizationCode,
            };
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        private static void InitializeRequest(RateRequest request)
        {
            var specialServices = request.Ensure(x => x.RequestedShipment)
                .Ensure(x => x.SpecialServicesRequested);
            specialServices.Ensure(x => x.SpecialServiceTypes);
            specialServices.Ensure(x => x.HoldAtLocationDetail)
                .Ensure(x => x.LocationContactAndAddress);
        }
    }
}
