using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.GlobalShipAddress.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.GlobalShipAddress.Request.Manipulators
{
    [TestClass]
    public class FedExGlobalShipAddressAddressManipulatorTest
    {
        private FedExGlobalShipAddressAddressManipulator testObject;
        
        private Mock<CarrierRequest> mockCarrierRequest;
        private SearchLocationsRequest request;
        private ShipmentEntity shipmentEntity;
        private Mock<ICarrierSettingsRepository> settingsRepository;
        

        [TestInitialize]
        public void Initialize()
        {
            shipmentEntity = BuildFedExShipmentEntity.SetupRequestShipmentEntity();

            request = new SearchLocationsRequest();

            mockCarrierRequest = new Mock<CarrierRequest>(null, shipmentEntity, request);
            settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>())).Returns(new FedExAccountEntity());

            testObject = new FedExGlobalShipAddressAddressManipulator();
        }

        [TestMethod]
        public void Manipulate_HasAddress_AddressInShipment_Test()
        {
            testObject.Manipulate(mockCarrierRequest.Object);

            Assert.IsNotNull(request.Address);
        }

        [TestMethod]
        public void Manipulate_HasAddressStreetLines_AddressStreetLinesInShipment_Test()
        {
            testObject.Manipulate(mockCarrierRequest.Object);

            Assert.IsNotNull(request.Address.StreetLines != null);
            Assert.AreEqual(2, request.Address.StreetLines.Length);
        }

        [TestMethod]
        public void Manipulate_StreetLinesMatchRequest_AddressStreetLinesInShipment_Test()
        {
            testObject.Manipulate(mockCarrierRequest.Object);

            Assert.AreEqual(shipmentEntity.ShipStreet1, request.Address.StreetLines[0]);
            Assert.AreEqual(shipmentEntity.ShipStreet2, request.Address.StreetLines[1]);
        }

        [TestMethod]
        public void Manipulate_CityCorrect_AddressInRequestHasCity_Test()
        {
            testObject.Manipulate(mockCarrierRequest.Object);

            Assert.AreEqual(shipmentEntity.ShipCity, request.Address.City);
        }

        [TestMethod]
        public void Manipulate_StateCorrect_AddressInRequestHasState_Test()
        {
            testObject.Manipulate(mockCarrierRequest.Object);

            Assert.AreEqual(shipmentEntity.ShipStateProvCode, request.Address.StateOrProvinceCode);
        }

        [TestMethod]
        public void Manipulate_ZipCorrect_AddressInRequestHasZip_Test()
        {
            testObject.Manipulate(mockCarrierRequest.Object);

            Assert.AreEqual(shipmentEntity.ShipPostalCode, request.Address.PostalCode);
        }

        [TestMethod]
        public void Manipulate_CountryCorrect_AddressInRequestHasCountry_Test()
        {
            testObject.Manipulate(mockCarrierRequest.Object);

            Assert.AreEqual(shipmentEntity.ShipCountryCode, request.Address.CountryCode);
        }

        [TestMethod]
        public void Manipulate_ResidentialCorrect_AddressInRequestHasResidentialResult_Test()
        {
            testObject.Manipulate(mockCarrierRequest.Object);

            Assert.AreEqual(shipmentEntity.ResidentialResult, request.Address.Residential);
        }

        [TestMethod]
        public void Manipulate_OtherCriteriaCorrect_ShipmentValid_Test()
        {
            testObject.Manipulate(mockCarrierRequest.Object);

            Assert.AreEqual(LocationsSearchCriteriaType.ADDRESS, request.LocationsSearchCriterion);
            Assert.IsTrue(request.LocationsSearchCriterionSpecified);
        }

        [TestMethod]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WrongRequestType_Test()
        {
            CarrierRequest wrongRequest = new FedExShipRequest(null, shipmentEntity, null, null, settingsRepository.Object, new ProcessShipmentRequest());
            testObject.Manipulate(wrongRequest);
        }
    }
}
