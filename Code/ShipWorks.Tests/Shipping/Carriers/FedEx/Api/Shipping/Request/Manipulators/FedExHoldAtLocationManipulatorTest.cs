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

        [TestInitialize]
        public void Initialize()
        {
            shipmentEntity = BuildFedExShipmentEntity.SetupRequestShipmentEntity();

            nativeRequest = new ProcessShipmentRequest();
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject = new FedExHoldAtLocationManipulator();
        }

        [Fact]
        public void Manipulate_HoldAtLocationDetailIsNull_WhenShipmentEntityHoldAtLocationIsNull_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNull(nativeRequest.RequestedShipment.SpecialServicesRequested);
        }

        [Fact]
        public void Manipulate_HoldAtLocationDetailIsNotCreated_WhenShipmentEntityHoldAtLocationEnabledisFalse_Test()
        {
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);
            Assert.IsNull(nativeRequest.RequestedShipment.SpecialServicesRequested);
        }

        [Fact]
        public void Manipulate_HoldAtLocationDetailIsNotNull_ShipmentEntityHoldAtLocationIsFullyPopulated_Test()
        {
            shipmentEntity.FedEx = GetFedExShipmentWithFullHoldAtLocationEntity();
            
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNotNull(nativeRequest.RequestedShipment.SpecialServicesRequested.HoldAtLocationDetail);
        }

        [Fact]
        public void Manipulate_HoldAtLocationDetailValuesMatchShipmentEntity_ShipmentEntityHoldAtLocationIsFullyPopulated_Test()
        {
            shipmentEntity.FedEx = GetFedExShipmentWithFullHoldAtLocationEntity();
            
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            testObject.Manipulate(carrierRequest.Object);

            HoldAtLocationDetail holdAtLocationDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.HoldAtLocationDetail;

            Assert.AreEqual(1, nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.Count(t => t == ShipmentSpecialServiceType.HOLD_AT_LOCATION));

            // Check address values
            Assert.AreEqual(shipmentEntity.FedEx.HoldCity, holdAtLocationDetail.LocationContactAndAddress.Address.City);
            Assert.AreEqual(shipmentEntity.FedEx.HoldCountryCode, holdAtLocationDetail.LocationContactAndAddress.Address.CountryCode);
            Assert.AreEqual(shipmentEntity.FedEx.HoldPostalCode, holdAtLocationDetail.LocationContactAndAddress.Address.PostalCode);
            Assert.AreEqual(shipmentEntity.FedEx.HoldResidential, holdAtLocationDetail.LocationContactAndAddress.Address.Residential);
            Assert.AreEqual(shipmentEntity.FedEx.HoldResidential.HasValue, holdAtLocationDetail.LocationContactAndAddress.Address.ResidentialSpecified);
            Assert.AreEqual(shipmentEntity.FedEx.HoldStateOrProvinceCode, holdAtLocationDetail.LocationContactAndAddress.Address.StateOrProvinceCode);
            Assert.AreEqual(shipmentEntity.FedEx.HoldUrbanizationCode, holdAtLocationDetail.LocationContactAndAddress.Address.UrbanizationCode);
                                                 
            // Check each street line            
            Assert.AreEqual(2, holdAtLocationDetail.LocationContactAndAddress.Address.StreetLines.Length);
            Assert.AreEqual(shipmentEntity.FedEx.HoldStreet1, holdAtLocationDetail.LocationContactAndAddress.Address.StreetLines[0]);
            Assert.AreEqual(shipmentEntity.FedEx.HoldStreet2, holdAtLocationDetail.LocationContactAndAddress.Address.StreetLines[1]);
            
                                                 
            // Check contact values              
            Assert.AreEqual(shipmentEntity.FedEx.HoldCompanyName, holdAtLocationDetail.LocationContactAndAddress.Contact.CompanyName);
            Assert.AreEqual(shipmentEntity.FedEx.HoldContactId, holdAtLocationDetail.LocationContactAndAddress.Contact.ContactId);
            Assert.AreEqual(shipmentEntity.FedEx.HoldEmailAddress, holdAtLocationDetail.LocationContactAndAddress.Contact.EMailAddress);
            Assert.AreEqual(shipmentEntity.FedEx.HoldFaxNumber, holdAtLocationDetail.LocationContactAndAddress.Contact.FaxNumber);
            Assert.AreEqual(shipmentEntity.FedEx.HoldPagerNumber, holdAtLocationDetail.LocationContactAndAddress.Contact.PagerNumber);
            Assert.AreEqual(shipmentEntity.FedEx.HoldPersonName, holdAtLocationDetail.LocationContactAndAddress.Contact.PersonName);
            Assert.AreEqual(shipmentEntity.FedEx.HoldPhoneExtension, holdAtLocationDetail.LocationContactAndAddress.Contact.PhoneExtension);
            Assert.AreEqual(shipmentEntity.FedEx.HoldPhoneNumber, holdAtLocationDetail.LocationContactAndAddress.Contact.PhoneNumber);
            Assert.AreEqual(shipmentEntity.FedEx.HoldTitle, holdAtLocationDetail.LocationContactAndAddress.Contact.Title);

            // Check location type and phone number
            Assert.AreEqual(shipmentEntity.FedEx.HoldLocationType, (int)holdAtLocationDetail.LocationType);
            Assert.AreEqual(shipmentEntity.FedEx.HoldLocationType.HasValue, holdAtLocationDetail.LocationTypeSpecified);
            Assert.AreEqual(shipmentEntity.ShipPhone, holdAtLocationDetail.PhoneNumber);
        }

        [Fact]
        public void Manipulate_HoldAtLocationDetailLocationTypeNull_HoldAtLocationDetailLocationTypeSpecifiedIsFalse_Test()
        {
            shipmentEntity.FedEx = GetFedExShipmentWithFullHoldAtLocationEntity();

            // Null LocationType so we can verify LocationTypeSpecified is false
            shipmentEntity.FedEx.HoldLocationType = null;
            
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            testObject.Manipulate(carrierRequest.Object);

            HoldAtLocationDetail holdAtLocationDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.HoldAtLocationDetail;

            Assert.AreEqual(false, holdAtLocationDetail.LocationTypeSpecified);
        }

        [Fact]
        public void Manipulate_HoldAtLocationDetailLocationTypeIsNotNull_HoldAtLocationDetailLocationTypeSpecifiedIsTrue_Test()
        {
            shipmentEntity.FedEx = GetFedExShipmentWithFullHoldAtLocationEntity();

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            testObject.Manipulate(carrierRequest.Object);

            HoldAtLocationDetail holdAtLocationDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.HoldAtLocationDetail;

            Assert.AreEqual(true, holdAtLocationDetail.LocationTypeSpecified);
        }

        [Fact]
        public void Manipulate_HoldAtLocationDetailResidentialNull_HoldAtLocationDetailResidentialSpecifiedIsFalse_Test()
        {
            shipmentEntity.FedEx = GetFedExShipmentWithFullHoldAtLocationEntity();

            // Null Residential so we can verify ResidentialSpecified is false
            shipmentEntity.FedEx.HoldResidential = null;


            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            testObject.Manipulate(carrierRequest.Object);

            HoldAtLocationDetail holdAtLocationDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.HoldAtLocationDetail;

            Assert.AreEqual(false, holdAtLocationDetail.LocationContactAndAddress.Address.ResidentialSpecified);
        }

        [Fact]
        public void Manipulate_HoldAtLocationDetailResidentialIsNotNull_HoldAtLocationDetailResidentialSpecifiedIsTrue_Test()
        {
            shipmentEntity.FedEx = GetFedExShipmentWithFullHoldAtLocationEntity();

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            testObject.Manipulate(carrierRequest.Object);

            HoldAtLocationDetail holdAtLocationDetail = nativeRequest.RequestedShipment.SpecialServicesRequested.HoldAtLocationDetail;

            Assert.AreEqual(true, holdAtLocationDetail.LocationContactAndAddress.Address.ResidentialSpecified);
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
