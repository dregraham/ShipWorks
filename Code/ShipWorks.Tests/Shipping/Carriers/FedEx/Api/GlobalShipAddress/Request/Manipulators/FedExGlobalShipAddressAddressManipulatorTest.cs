using Xunit;
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

        [Fact]
        public void Manipulate_HasAddress_AddressInShipment_Test()
        {
            testObject.Manipulate(mockCarrierRequest.Object);

            Assert.IsNotNull(request.Address);
        }

        [Fact]
        public void Manipulate_HasAddressStreetLines_AddressStreetLinesInShipment_Test()
        {
            testObject.Manipulate(mockCarrierRequest.Object);

            Assert.IsNotNull(request.Address.StreetLines != null);
            Assert.AreEqual(2, request.Address.StreetLines.Length);
        }

        [Fact]
        public void Manipulate_StreetLinesMatchRequest_AddressStreetLinesInShipment_Test()
        {
            testObject.Manipulate(mockCarrierRequest.Object);

            Assert.AreEqual(shipmentEntity.ShipStreet1, request.Address.StreetLines[0]);
            Assert.AreEqual(shipmentEntity.ShipStreet2, request.Address.StreetLines[1]);
        }

        [Fact]
        public void Manipulate_CityCorrect_AddressInRequestHasCity_Test()
        {
            testObject.Manipulate(mockCarrierRequest.Object);

            Assert.AreEqual(shipmentEntity.ShipCity, request.Address.City);
        }

        [Fact]
        public void Manipulate_StateCorrect_AddressInRequestHasState_Test()
        {
            testObject.Manipulate(mockCarrierRequest.Object);

            Assert.AreEqual(shipmentEntity.ShipStateProvCode, request.Address.StateOrProvinceCode);
        }

        [Fact]
        public void Manipulate_ZipCorrect_AddressInRequestHasZip_Test()
        {
            testObject.Manipulate(mockCarrierRequest.Object);

            Assert.AreEqual(shipmentEntity.ShipPostalCode, request.Address.PostalCode);
        }

        [Fact]
        public void Manipulate_CountryCorrect_AddressInRequestHasCountry_Test()
        {
            testObject.Manipulate(mockCarrierRequest.Object);

            Assert.AreEqual(shipmentEntity.ShipCountryCode, request.Address.CountryCode);
        }

        [Fact]
        public void Manipulate_ResidentialCorrect_AddressInRequestHasResidentialResult_Test()
        {
            testObject.Manipulate(mockCarrierRequest.Object);

            Assert.AreEqual(shipmentEntity.ResidentialResult, request.Address.Residential);
        }

        [Fact]
        public void Manipulate_OtherCriteriaCorrect_ShipmentValid_Test()
        {
            testObject.Manipulate(mockCarrierRequest.Object);

            Assert.AreEqual(LocationsSearchCriteriaType.ADDRESS, request.LocationsSearchCriterion);
            Assert.IsTrue(request.LocationsSearchCriterionSpecified);
        }

        [Fact]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WrongRequestType_Test()
        {
            CarrierRequest wrongRequest = new FedExShipRequest(null, shipmentEntity, null, null, settingsRepository.Object, new ProcessShipmentRequest());
            testObject.Manipulate(wrongRequest);
        }
    }
}
