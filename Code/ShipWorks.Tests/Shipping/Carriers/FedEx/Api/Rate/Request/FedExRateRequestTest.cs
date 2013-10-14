using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Request;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Rate.Request
{
    [TestClass]
    public class FedExRateRequestTest
    {
        private FedExRateRequest testObject;

        private Mock<IFedExServiceGateway> fedExService;
        private Mock<ICarrierResponse> carrierResponse;
        private Mock<ICarrierResponseFactory> responseFactory;
        private Mock<ICarrierSettingsRepository> settingsRepository;

        private Mock<ICarrierRequestManipulator> firstManipulator;
        private Mock<ICarrierRequestManipulator> secondManipulator;

        private ShipmentEntity shipmentEntity;
        private FedExAccountEntity account;

        [TestInitialize]
        public void Initialize()
        {
            shipmentEntity = new ShipmentEntity();
            account = new FedExAccountEntity { AccountNumber = "1234", MeterNumber = "45453" };

            settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>())).Returns(account);

            fedExService = new Mock<IFedExServiceGateway>();
            fedExService.Setup(s => s.GetRates(It.IsAny<RateRequest>())).Returns(new RateReply());

            carrierResponse = new Mock<ICarrierResponse>();

            responseFactory = new Mock<ICarrierResponseFactory>();
            responseFactory.Setup(f => f.CreateShipResponse(It.IsAny<RateReply>(), It.IsAny<CarrierRequest>(), shipmentEntity)).Returns(carrierResponse.Object);

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
        
        [TestMethod]
        public void CarrierAccountEntity_DelegatesToRepositoryForAccount_Test()
        {
            object obj = testObject.CarrierAccountEntity;

            // The constructor has already been called in the initialize method, so just make sure the
            // repository was used to fetch the account
            settingsRepository.Verify(r => r.GetAccount(testObject.ShipmentEntity), Times.Once());
        }
        
        [TestMethod]
        public void CarrierAccountEntity_IsNotNull_Test()
        {
            Assert.IsNotNull(testObject.CarrierAccountEntity as FedExAccountEntity);
        }

        public void CarrierAccountEntity_IsAccountRetrievedFromRepository_Test()
        {
            Assert.AreEqual(account, testObject.CarrierAccountEntity);
        }

        [TestMethod]
        public void Submit_DelegatesToManipulators_Test()
        {
            // No additional setup needed since it was performed in Initialize()
            ICarrierResponse response = testObject.Submit();

            // Verify all of the manipulators were called and our test object was passed to them
            firstManipulator.Verify(m => m.Manipulate(testObject), Times.Once());
            secondManipulator.Verify(m => m.Manipulate(testObject), Times.Once());
        }

        [TestMethod]
        public void Submit_DelegatesToFedExService_Test()
        {
            // No additional setup needed since it was performed in Initialize()
            ICarrierResponse response = testObject.Submit();

            // Verify that the close method was called using the test object's native request
            fedExService.Verify(s => s.GetRates(testObject.NativeRequest as RateRequest), Times.Once());
        }

        [TestMethod]
        public void Submit_DelegatesToResponseFactory_WhenCreatingRateResponse_Test()
        {
            // No additional setup needed since it was performed in Initialize()
            ICarrierResponse response = testObject.Submit();

            // Verify the rate response is created via the response factory using the test object
            responseFactory.Verify(f => f.CreateRateResponse(It.IsAny<RateReply>(), testObject), Times.Once());
        }
    }
}
