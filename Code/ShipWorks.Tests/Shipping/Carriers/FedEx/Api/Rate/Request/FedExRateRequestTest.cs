using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Rate.Request
{
    public class FedExRateRequestTest
    {
        private FedExRateRequest testObject;

        private Mock<IFedExServiceGateway> fedExService;
        private Mock<IFedExResponseFactory> responseFactory;
        private Mock<ICarrierSettingsRepository> settingsRepository;

        private Mock<ICarrierRequestManipulator> firstManipulator;
        private Mock<ICarrierRequestManipulator> secondManipulator;

        private ShipmentEntity shipmentEntity;
        private FedExAccountEntity account;

        public FedExRateRequestTest()
        {
            shipmentEntity = new ShipmentEntity();
            shipmentEntity.FedEx = new FedExShipmentEntity() { ReferencePO = "testPO" };

            account = new FedExAccountEntity { AccountNumber = "1234", MeterNumber = "45453" };

            settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>())).Returns(account);

            fedExService = new Mock<IFedExServiceGateway>();
            fedExService.Setup(s => s.GetRates(It.IsAny<RateRequest>(), shipmentEntity)).Returns(new RateReply());

            responseFactory = new Mock<IFedExResponseFactory>();

            firstManipulator = new Mock<ICarrierRequestManipulator>();
            firstManipulator.Setup(m => m.Manipulate(It.IsAny<CarrierRequest>()));

            secondManipulator = new Mock<ICarrierRequestManipulator>();
            secondManipulator.Setup(m => m.Manipulate(It.IsAny<CarrierRequest>()));

            // Create some mocked manipulators for testing purposes
            List<ICarrierRequestManipulator> manipulators = new List<ICarrierRequestManipulator>()
            {
                firstManipulator.Object,
                secondManipulator.Object
            };

            testObject = new FedExRateRequest(manipulators, shipmentEntity, fedExService.Object, responseFactory.Object, settingsRepository.Object);
        }

        [Fact]
        public void CarrierAccountEntity_DelegatesToRepositoryForAccount()
        {
            object obj = testObject.CarrierAccountEntity;

            // The constructor has already been called in the initialize method, so just make sure the
            // repository was used to fetch the account
            settingsRepository.Verify(r => r.GetAccount(testObject.ShipmentEntity), Times.Once());
        }

        [Fact]
        public void CarrierAccountEntity_IsNotNull()
        {
            Assert.NotNull(testObject.CarrierAccountEntity as FedExAccountEntity);
        }

        [Fact]
        public void CarrierAccountEntity_IsAccountRetrievedFromRepository()
        {
            Assert.Equal(account, testObject.CarrierAccountEntity);
        }

        [Fact]
        public void Submit_DelegatesToManipulators()
        {
            // No additional setup needed since it was performed in Initialize()
            ICarrierResponse response = testObject.Submit();

            // Verify all of the manipulators were called and our test object was passed to them
            firstManipulator.Verify(m => m.Manipulate(testObject), Times.Once());
            secondManipulator.Verify(m => m.Manipulate(testObject), Times.Once());
        }

        [Fact]
        public void Submit_DelegatesToFedExService()
        {
            // No additional setup needed since it was performed in Initialize()
            ICarrierResponse response = testObject.Submit();

            // Verify that the close method was called using the test object's native request
            fedExService.Verify(s => s.GetRates(testObject.NativeRequest as RateRequest, shipmentEntity), Times.Once());
        }

        [Fact]
        public void Submit_DelegatesToResponseFactory_WhenCreatingRateResponse()
        {
            // No additional setup needed since it was performed in Initialize()
            ICarrierResponse response = testObject.Submit();

            // Verify the rate response is created via the response factory using the test object
            responseFactory.Verify(f => f.CreateRateResponse(It.IsAny<RateReply>(), testObject), Times.Once());
        }
    }
}
