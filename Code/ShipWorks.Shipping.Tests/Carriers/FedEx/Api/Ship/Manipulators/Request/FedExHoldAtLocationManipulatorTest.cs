using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    public class FedExHoldAtLocationManipulatorTest
    {
        private FedExHoldAtLocationManipulator testObject;
        private ShipmentEntity shipment;

        public FedExHoldAtLocationManipulatorTest()
        {
            shipment = BuildFedExShipmentEntity.SetupRequestShipmentEntity();

            testObject = new FedExHoldAtLocationManipulator();
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(true, true)]
        public void ShouldApply_ReturnsFalse_WhenShipmentEntityHoldAtLocationEnabledisFalse(bool holdAtLocation, bool expected)
        {
            var testShipment = Create.Shipment().AsFedEx(f => f.Set(x => x.FedExHoldAtLocationEnabled, holdAtLocation)).Build();
            var result = testObject.ShouldApply(testShipment);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Manipulate_HoldAtLocationDetailValuesMatchShipmentEntity_ShipmentEntityHoldAtLocationIsFullyPopulated()
        {
            shipment.FedEx = GetFedExShipmentWithFullHoldAtLocationEntity();

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            HoldAtLocationDetail holdAtLocationDetail = result.Value.RequestedShipment.SpecialServicesRequested.HoldAtLocationDetail;

            Assert.Equal(1, result.Value.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.Count(t => t == ShipmentSpecialServiceType.HOLD_AT_LOCATION));

            // Check address values
            Assert.Equal(shipment.FedEx.HoldCity, holdAtLocationDetail.LocationContactAndAddress.Address.City);
            Assert.Equal(shipment.FedEx.HoldCountryCode, holdAtLocationDetail.LocationContactAndAddress.Address.CountryCode);
            Assert.Equal(shipment.FedEx.HoldPostalCode, holdAtLocationDetail.LocationContactAndAddress.Address.PostalCode);
            Assert.Equal(shipment.FedEx.HoldResidential, holdAtLocationDetail.LocationContactAndAddress.Address.Residential);
            Assert.Equal(shipment.FedEx.HoldResidential.HasValue, holdAtLocationDetail.LocationContactAndAddress.Address.ResidentialSpecified);
            Assert.Equal(shipment.FedEx.HoldStateOrProvinceCode, holdAtLocationDetail.LocationContactAndAddress.Address.StateOrProvinceCode);
            Assert.Equal(shipment.FedEx.HoldUrbanizationCode, holdAtLocationDetail.LocationContactAndAddress.Address.UrbanizationCode);

            // Check each street line            
            Assert.Equal(2, holdAtLocationDetail.LocationContactAndAddress.Address.StreetLines.Length);
            Assert.Equal(shipment.FedEx.HoldStreet1, holdAtLocationDetail.LocationContactAndAddress.Address.StreetLines[0]);
            Assert.Equal(shipment.FedEx.HoldStreet2, holdAtLocationDetail.LocationContactAndAddress.Address.StreetLines[1]);

            // Check contact values              
            Assert.Equal(shipment.FedEx.HoldCompanyName, holdAtLocationDetail.LocationContactAndAddress.Contact.CompanyName);
            Assert.Equal(shipment.FedEx.HoldContactId, holdAtLocationDetail.LocationContactAndAddress.Contact.ContactId);
            Assert.Equal(shipment.FedEx.HoldEmailAddress, holdAtLocationDetail.LocationContactAndAddress.Contact.EMailAddress);
            Assert.Equal(shipment.FedEx.HoldFaxNumber, holdAtLocationDetail.LocationContactAndAddress.Contact.FaxNumber);
            Assert.Equal(shipment.FedEx.HoldPagerNumber, holdAtLocationDetail.LocationContactAndAddress.Contact.PagerNumber);
            Assert.Equal(shipment.FedEx.HoldPersonName, holdAtLocationDetail.LocationContactAndAddress.Contact.PersonName);
            Assert.Equal(shipment.FedEx.HoldPhoneExtension, holdAtLocationDetail.LocationContactAndAddress.Contact.PhoneExtension);
            Assert.Equal(shipment.FedEx.HoldPhoneNumber, holdAtLocationDetail.LocationContactAndAddress.Contact.PhoneNumber);
            Assert.Equal(shipment.FedEx.HoldTitle, holdAtLocationDetail.LocationContactAndAddress.Contact.Title);

            // Check location type and phone number
            Assert.Equal(shipment.FedEx.HoldLocationType, (int) holdAtLocationDetail.LocationType);
            Assert.Equal(shipment.FedEx.HoldLocationType.HasValue, holdAtLocationDetail.LocationTypeSpecified);
            Assert.Equal(shipment.ShipPhone, holdAtLocationDetail.PhoneNumber);
        }

        [Fact]
        public void Manipulate_HoldAtLocationDetailLocationTypeNull_HoldAtLocationDetailLocationTypeSpecifiedIsFalse()
        {
            shipment.FedEx = GetFedExShipmentWithFullHoldAtLocationEntity();

            // Null LocationType so we can verify LocationTypeSpecified is false
            shipment.FedEx.HoldLocationType = null;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            HoldAtLocationDetail holdAtLocationDetail = result.Value.RequestedShipment.SpecialServicesRequested.HoldAtLocationDetail;

            Assert.Equal(false, holdAtLocationDetail.LocationTypeSpecified);
        }

        [Fact]
        public void Manipulate_HoldAtLocationDetailLocationTypeIsNotNull_HoldAtLocationDetailLocationTypeSpecifiedIsTrue()
        {
            shipment.FedEx = GetFedExShipmentWithFullHoldAtLocationEntity();

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            HoldAtLocationDetail holdAtLocationDetail = result.Value.RequestedShipment.SpecialServicesRequested.HoldAtLocationDetail;

            Assert.Equal(true, holdAtLocationDetail.LocationTypeSpecified);
        }

        [Fact]
        public void Manipulate_HoldAtLocationDetailResidentialNull_HoldAtLocationDetailResidentialSpecifiedIsFalse()
        {
            shipment.FedEx = GetFedExShipmentWithFullHoldAtLocationEntity();

            // Null Residential so we can verify ResidentialSpecified is false
            shipment.FedEx.HoldResidential = null;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            HoldAtLocationDetail holdAtLocationDetail = result.Value.RequestedShipment.SpecialServicesRequested.HoldAtLocationDetail;

            Assert.Equal(false, holdAtLocationDetail.LocationContactAndAddress.Address.ResidentialSpecified);
        }

        [Fact]
        public void Manipulate_HoldAtLocationDetailResidentialIsNotNull_HoldAtLocationDetailResidentialSpecifiedIsTrue()
        {
            shipment.FedEx = GetFedExShipmentWithFullHoldAtLocationEntity();

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            HoldAtLocationDetail holdAtLocationDetail = result.Value.RequestedShipment.SpecialServicesRequested.HoldAtLocationDetail;

            Assert.Equal(true, holdAtLocationDetail.LocationContactAndAddress.Address.ResidentialSpecified);
        }

        /// <summary>
        /// Creates and fully populates a FedExHoldAtLocationEntity
        /// </summary>
        /// <returns>A fully populated FedExHoldAtLocationEntity</returns>
        private FedExShipmentEntity GetFedExShipmentWithFullHoldAtLocationEntity()
        {
            FedExShipmentEntity fedExShipmentEntity = new FedExShipmentEntity
            {
                FedExHoldAtLocationEnabled = true,
                HoldCity = "St. Louis",
                HoldCompanyName = "Fed Ex",
                HoldContactId = "33",
                HoldCountryCode = "US",
                HoldEmailAddress = "Contact33@fedex.com",
                HoldFaxNumber = "8881234567",
                HoldLocationId = "Location33",
                HoldLocationType = (int) FedExLocationType.FEDEX_EXPRESS_STATION,
                HoldPagerNumber = "7771234567",
                HoldPersonName = "Fedex Clerk",
                HoldPhoneExtension = "124",
                HoldPhoneNumber = "8001234567",
                HoldPostalCode = "63102",
                HoldResidential = false,
                HoldStateOrProvinceCode = "MO",
                HoldStreet1 = "123 Main",
                HoldStreet2 = "Suite 200",
                HoldStreet3 = "Attn: FedEx Clerk",
                HoldTitle = "Clerk",
                HoldUrbanizationCode = "UC_MO"
            };

            return fedExShipmentEntity;
        }
    }
}
