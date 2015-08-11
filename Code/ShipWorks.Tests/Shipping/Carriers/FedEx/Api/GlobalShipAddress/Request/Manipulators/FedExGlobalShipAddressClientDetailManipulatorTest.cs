using System;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.GlobalShipAddress.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress;
using ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.GlobalShipAddress.Request.Manipulators
{
    public class FedExGlobalShipAddressClientDetailManipulatorTest
    {
        private FedExGlobalShipAddressClientDetailManipulator testObject;

        private Mock<CarrierRequest> mockCarrierRequest;
        private SearchLocationsRequest nativeRequest;
        private ShipmentEntity shipmentEntity;
        private Mock<ICarrierSettingsRepository> mockSettingsRepository;
        private FedExAccountEntity account;

        [TestInitialize]
        public void Initialize()
        {
            account = new FedExAccountEntity {AccountNumber = "123", MeterNumber = "456"};

            shipmentEntity = BuildFedExShipmentEntity.SetupRequestShipmentEntity();

            nativeRequest = new SearchLocationsRequest();


            mockSettingsRepository = new Mock<ICarrierSettingsRepository>();
            mockSettingsRepository.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>()))
                                  .Returns(account);


            mockCarrierRequest = new Mock<CarrierRequest>(null, shipmentEntity, nativeRequest);
            mockCarrierRequest.Setup(r => r.CarrierAccountEntity).Returns(account);

            testObject = new FedExGlobalShipAddressClientDetailManipulator(mockSettingsRepository.Object);
        }

        [Fact]
        public void Manipulate_ClientProductInformationIsCorrect_DefaultClientProductDetails_Test()
        {
            testObject.Manipulate(mockCarrierRequest.Object);

            Assert.AreEqual("ITSW", nativeRequest.ClientDetail.ClientProductId);
            Assert.AreEqual("5236", nativeRequest.ClientDetail.ClientProductVersion);
        }

        [Fact]
        public void Manipulate_AccountNumberIsCorrect_AccountIdIs123_Test()
        {
            testObject.Manipulate(mockCarrierRequest.Object);

            Assert.AreEqual("123", nativeRequest.ClientDetail.AccountNumber);
        }

        [Fact]
        public void Manipulate_MeterNumberIsCorrect_MeterNumberIs456_Test()
        {
            testObject.Manipulate(mockCarrierRequest.Object);

            Assert.AreEqual("456", nativeRequest.ClientDetail.MeterNumber);
        }

        [Fact]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Manipulate_ThrowsException_WhenCarrierRequestIsNull_Test()
        {
            testObject.Manipulate(null);
        }

        [Fact]
        public void Manipulate_DelegatesToRequestForFedExAccount_Test()
        {
            testObject.Manipulate(mockCarrierRequest.Object);

            mockCarrierRequest.Verify(r => r.CarrierAccountEntity, Times.Once());
        }

        [Fact]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsException_WhenNativeRequestIsNull_Test()
        {
            // Set the native request to null
            mockCarrierRequest = new Mock<CarrierRequest>(null, shipmentEntity, null);

            testObject.Manipulate(mockCarrierRequest.Object);
        }

        [Fact]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsException_WhenNativeRequestIsNotSearchLocationsRequest_Test()
        {
            // Set the native request to null
            mockCarrierRequest = new Mock<CarrierRequest>(null, shipmentEntity, new SearchLocationsReply());

            testObject.Manipulate(mockCarrierRequest.Object);
        }
    }
}
