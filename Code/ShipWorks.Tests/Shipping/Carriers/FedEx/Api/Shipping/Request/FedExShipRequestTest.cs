using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request
{
    public class FedExShipRequestTest
    {
        private FedExShipRequest testObject;
         
        private Mock<IFedExServiceGateway> fedExService;
        private Mock<ICarrierResponse> carrierResponse;
        private Mock<ICarrierResponseFactory> responseFactory;
        
        private Mock<ICarrierRequestManipulator> firstManipulator;
        private Mock<ICarrierRequestManipulator> secondManipulator;
        private Mock<ICarrierSettingsRepository> settingsRespository;
        private Mock<CarrierRequest> carrierRequest;

        private FedExAccountEntity account;

        ShipmentEntity shipmentEntity;

        public FedExShipRequestTest()
        {
            carrierRequest = new Mock<CarrierRequest>(null, null);
            shipmentEntity = new ShipmentEntity();
            account = new FedExAccountEntity { AccountNumber = "1234", MeterNumber = "45453" };

            fedExService = new Mock<IFedExServiceGateway>();
            fedExService.Setup(s => s.Ship(It.IsAny<ProcessShipmentRequest>())).Returns(new ProcessShipmentReply());

            settingsRespository = new Mock<ICarrierSettingsRepository>();
            settingsRespository.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>())).Returns(account);

            carrierResponse = new Mock<ICarrierResponse>();

            responseFactory = new Mock<ICarrierResponseFactory>();
            responseFactory.Setup(f => f.CreateShipResponse(It.IsAny<ProcessShipmentReply>(), carrierRequest.Object, shipmentEntity)).Returns(carrierResponse.Object);

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

            testObject = new FedExShipRequest(manipulators, shipmentEntity, fedExService.Object, responseFactory.Object, settingsRespository.Object, new ProcessShipmentRequest());
        }

        [Fact]
        public void CarrierAccountEntity_IsNotNull()
        {
            Assert.NotNull(testObject.CarrierAccountEntity as FedExAccountEntity);
        }

        [Fact]
        public void CarrierAccountEntity_DelegatesToSettingsRepository()
        {
            object carrierAccount = testObject.CarrierAccountEntity;

            // Verify the repository was used to obtain the account using the shipment 
            // entity provided to the request 
            settingsRespository.Verify(r => r.GetAccount(shipmentEntity), Times.Once());
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

            // Verify that the ship method was called using the test object's native request
            fedExService.Verify(s => s.Ship(testObject.NativeRequest as ProcessShipmentRequest), Times.Once());
        }

        [Fact]
        public void Submit_DelegatesToResponseFactory_WhenCreatingShipResponse()
        {
            // No additional setup needed since it was performed in Initialize()
            ICarrierResponse response = testObject.Submit();

            // Verify the ship response is created via the response factory using the test object's shipment entity
            responseFactory.Verify(f => f.CreateShipResponse(It.IsAny<ProcessShipmentReply>(), testObject, testObject.ShipmentEntity), Times.Once());
        }
    }
}
