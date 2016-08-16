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


        public FedExGlobalShipAddressAddressManipulatorTest()
        {
            shipmentEntity = BuildFedExShipmentEntity.SetupRequestShipmentEntity();

            request = new SearchLocationsRequest();

            mockCarrierRequest = new Mock<CarrierRequest>(null, shipmentEntity, request);
            settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>())).Returns(new FedExAccountEntity());

            testObject = new FedExGlobalShipAddressAddressManipulator();
        }

        [Fact]
        public void Manipulate_HasAddress_AddressInShipment()
        {
            testObject.Manipulate(mockCarrierRequest.Object);

            Assert.NotNull(request.Address);
        }

        [Fact]
        public void Manipulate_HasAddressStreetLines_AddressStreetLinesInShipment()
        {
            testObject.Manipulate(mockCarrierRequest.Object);

            Assert.NotNull(request.Address.StreetLines != null);
            Assert.Equal(2, request.Address.StreetLines.Length);
        }

        [Fact]
        public void Manipulate_StreetLinesMatchRequest_AddressStreetLinesInShipment()
        {
            testObject.Manipulate(mockCarrierRequest.Object);

            Assert.Equal(shipmentEntity.ShipStreet1, request.Address.StreetLines[0]);
            Assert.Equal(shipmentEntity.ShipStreet2, request.Address.StreetLines[1]);
        }

        [Fact]
        public void Manipulate_CityCorrect_AddressInRequestHasCity()
        {
            testObject.Manipulate(mockCarrierRequest.Object);

            Assert.Equal(shipmentEntity.ShipCity, request.Address.City);
        }

        [Fact]
        public void Manipulate_StateCorrect_AddressInRequestHasState()
        {
            testObject.Manipulate(mockCarrierRequest.Object);

            Assert.Equal(shipmentEntity.ShipStateProvCode, request.Address.StateOrProvinceCode);
        }

        [Fact]
        public void Manipulate_ZipCorrect_AddressInRequestHasZip()
        {
            testObject.Manipulate(mockCarrierRequest.Object);

            Assert.Equal(shipmentEntity.ShipPostalCode, request.Address.PostalCode);
        }

        [Fact]
        public void Manipulate_CountryCorrect_AddressInRequestHasCountry()
        {
            testObject.Manipulate(mockCarrierRequest.Object);

            Assert.Equal(shipmentEntity.ShipCountryCode, request.Address.CountryCode);
        }

        [Fact]
        public void Manipulate_ResidentialCorrect_AddressInRequestHasResidentialResult()
        {
            testObject.Manipulate(mockCarrierRequest.Object);

            Assert.Equal(shipmentEntity.ResidentialResult, request.Address.Residential);
        }

        [Fact]
        public void Manipulate_OtherCriteriaCorrect_ShipmentValid()
        {
            testObject.Manipulate(mockCarrierRequest.Object);

            Assert.Equal(LocationsSearchCriteriaType.ADDRESS, request.LocationsSearchCriterion);
            Assert.True(request.LocationsSearchCriterionSpecified);
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WrongRequestType()
        {
            CarrierRequest wrongRequest = new FedExShipRequest(null, shipmentEntity, null, null, settingsRepository.Object, new ProcessShipmentRequest());
            Assert.Throws<CarrierException>(() => testObject.Manipulate(wrongRequest));
        }
    }
}
