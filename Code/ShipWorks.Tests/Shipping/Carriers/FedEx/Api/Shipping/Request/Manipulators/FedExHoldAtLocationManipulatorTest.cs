using System.Collections.Generic;
using System.Linq;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    public class FedExHoldAtLocationManipulatorTest
    {
        private FedExHoldAtLocationManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private ProcessShipmentRequest nativeRequest;
        private ShipmentEntity shipmentEntity;

        public FedExHoldAtLocationManipulatorTest()
        {
            shipmentEntity = BuildFedExShipmentEntity.SetupRequestShipmentEntity();

            nativeRequest = new ProcessShipmentRequest();
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject = new FedExHoldAtLocationManipulator();
        }

        [Fact]
        public void Manipulate_HoldAtLocationDetailIsNull_WhenShipmentEntityHoldAtLocationIsNull()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.Null(nativeRequest.RequestedShipment.SpecialServicesRequested);
        }

        [Fact]
        public void Manipulate_HoldAtLocationDetailIsNotCreated_WhenShipmentEntityHoldAtLocationEnabledisFalse()
        {
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);
            Assert.Null(nativeRequest.RequestedShipment.SpecialServicesRequested);
        }

        [Fact]
        public void Manipulate_HoldAtLocationDetailIsNotNull_ShipmentEntityHoldAtLocationIsFullyPopulated()
        {
            shipmentEntity.FedEx = GetFedExShipmentWithFullHoldAtLocationEntity();
            
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            testObject.Manipulate(carrierRequest.Object);

            Assert.NotNull(nativeRequest.RequestedShipment.SpecialServicesRequested.HoldAtLocationDetail);
        }

        [Fact]
        public void Manipulate_HoldAtLocationDetailValuesMatchShipmentEntity_ShipmentEntityHoldAtLocationIsFullyPopulated()
        {
            shipmentEntity.FedEx = GetFedExShipmentWithFullHoldAtLocationEntity();
            
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            testObject.Manipulate(carrierRequest.Object);

            HoldAtLocationDetail holdAtLocationDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.HoldAtLocationDetail;

            Assert.Equal(1, nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.Count(t => t == ShipmentSpecialServiceType.HOLD_AT_LOCATION));

            // Check address values
            Assert.Equal(shipmentEntity.FedEx.HoldCity, holdAtLocationDetail.LocationContactAndAddress.Address.City);
            Assert.Equal(shipmentEntity.FedEx.HoldCountryCode, holdAtLocationDetail.LocationContactAndAddress.Address.CountryCode);
            Assert.Equal(shipmentEntity.FedEx.HoldPostalCode, holdAtLocationDetail.LocationContactAndAddress.Address.PostalCode);
            Assert.Equal(shipmentEntity.FedEx.HoldResidential, holdAtLocationDetail.LocationContactAndAddress.Address.Residential);
            Assert.Equal(shipmentEntity.FedEx.HoldResidential.HasValue, holdAtLocationDetail.LocationContactAndAddress.Address.ResidentialSpecified);
            Assert.Equal(shipmentEntity.FedEx.HoldStateOrProvinceCode, holdAtLocationDetail.LocationContactAndAddress.Address.StateOrProvinceCode);
            Assert.Equal(shipmentEntity.FedEx.HoldUrbanizationCode, holdAtLocationDetail.LocationContactAndAddress.Address.UrbanizationCode);
                                                 
            // Check each street line            
            Assert.Equal(2, holdAtLocationDetail.LocationContactAndAddress.Address.StreetLines.Length);
            Assert.Equal(shipmentEntity.FedEx.HoldStreet1, holdAtLocationDetail.LocationContactAndAddress.Address.StreetLines[0]);
            Assert.Equal(shipmentEntity.FedEx.HoldStreet2, holdAtLocationDetail.LocationContactAndAddress.Address.StreetLines[1]);
            
                                                 
            // Check contact values              
            Assert.Equal(shipmentEntity.FedEx.HoldCompanyName, holdAtLocationDetail.LocationContactAndAddress.Contact.CompanyName);
            Assert.Equal(shipmentEntity.FedEx.HoldContactId, holdAtLocationDetail.LocationContactAndAddress.Contact.ContactId);
            Assert.Equal(shipmentEntity.FedEx.HoldEmailAddress, holdAtLocationDetail.LocationContactAndAddress.Contact.EMailAddress);
            Assert.Equal(shipmentEntity.FedEx.HoldFaxNumber, holdAtLocationDetail.LocationContactAndAddress.Contact.FaxNumber);
            Assert.Equal(shipmentEntity.FedEx.HoldPagerNumber, holdAtLocationDetail.LocationContactAndAddress.Contact.PagerNumber);
            Assert.Equal(shipmentEntity.FedEx.HoldPersonName, holdAtLocationDetail.LocationContactAndAddress.Contact.PersonName);
            Assert.Equal(shipmentEntity.FedEx.HoldPhoneExtension, holdAtLocationDetail.LocationContactAndAddress.Contact.PhoneExtension);
            Assert.Equal(shipmentEntity.FedEx.HoldPhoneNumber, holdAtLocationDetail.LocationContactAndAddress.Contact.PhoneNumber);
            Assert.Equal(shipmentEntity.FedEx.HoldTitle, holdAtLocationDetail.LocationContactAndAddress.Contact.Title);

            // Check location type and phone number
            Assert.Equal(shipmentEntity.FedEx.HoldLocationType, (int)holdAtLocationDetail.LocationType);
            Assert.Equal(shipmentEntity.FedEx.HoldLocationType.HasValue, holdAtLocationDetail.LocationTypeSpecified);
            Assert.Equal(shipmentEntity.ShipPhone, holdAtLocationDetail.PhoneNumber);
        }

        [Fact]
        public void Manipulate_HoldAtLocationDetailLocationTypeNull_HoldAtLocationDetailLocationTypeSpecifiedIsFalse()
        {
            shipmentEntity.FedEx = GetFedExShipmentWithFullHoldAtLocationEntity();

            // Null LocationType so we can verify LocationTypeSpecified is false
            shipmentEntity.FedEx.HoldLocationType = null;
            
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            testObject.Manipulate(carrierRequest.Object);

            HoldAtLocationDetail holdAtLocationDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.HoldAtLocationDetail;

            Assert.Equal(false, holdAtLocationDetail.LocationTypeSpecified);
        }

        [Fact]
        public void Manipulate_HoldAtLocationDetailLocationTypeIsNotNull_HoldAtLocationDetailLocationTypeSpecifiedIsTrue()
        {
            shipmentEntity.FedEx = GetFedExShipmentWithFullHoldAtLocationEntity();

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            testObject.Manipulate(carrierRequest.Object);

            HoldAtLocationDetail holdAtLocationDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.HoldAtLocationDetail;

            Assert.Equal(true, holdAtLocationDetail.LocationTypeSpecified);
        }

        [Fact]
        public void Manipulate_HoldAtLocationDetailResidentialNull_HoldAtLocationDetailResidentialSpecifiedIsFalse()
        {
            shipmentEntity.FedEx = GetFedExShipmentWithFullHoldAtLocationEntity();

            // Null Residential so we can verify ResidentialSpecified is false
            shipmentEntity.FedEx.HoldResidential = null;


            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            testObject.Manipulate(carrierRequest.Object);

            HoldAtLocationDetail holdAtLocationDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.HoldAtLocationDetail;

            Assert.Equal(false, holdAtLocationDetail.LocationContactAndAddress.Address.ResidentialSpecified);
        }

        [Fact]
        public void Manipulate_HoldAtLocationDetailResidentialIsNotNull_HoldAtLocationDetailResidentialSpecifiedIsTrue()
        {
            shipmentEntity.FedEx = GetFedExShipmentWithFullHoldAtLocationEntity();

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            testObject.Manipulate(carrierRequest.Object);

            HoldAtLocationDetail holdAtLocationDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.HoldAtLocationDetail;

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
                HoldLocationType = (int)FedExLocationType.FEDEX_EXPRESS_STATION,
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
