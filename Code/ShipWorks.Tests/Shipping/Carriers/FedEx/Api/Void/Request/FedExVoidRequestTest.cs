using System.Collections.Generic;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Api.Void.Request;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Void.Request
{
    public class FedExVoidRequestTest
    {
        private FedExVoidRequest testObject;

        private Mock<IFedExServiceGateway> fedExService;
        private Mock<IFedExResponseFactory> responseFactory;

        private Mock<ICarrierRequestManipulator> firstManipulator;
        private Mock<ICarrierRequestManipulator> secondManipulator;

        private ShipmentEntity shipmentEntity;
        private FedExAccountEntity account;

        public FedExVoidRequestTest()
        {
            shipmentEntity = new ShipmentEntity();
            account = new FedExAccountEntity { AccountNumber = "1234", MeterNumber = "45453" };

            fedExService = new Mock<IFedExServiceGateway>();
            fedExService.Setup(s => s.Void(It.IsAny<DeleteShipmentRequest>())).Returns(new ShipmentReply());

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

            testObject = new FedExVoidRequest(manipulators, shipmentEntity, fedExService.Object, responseFactory.Object, account);
        }

        [Fact]
        public void CarrierAccountEntity_IsNotNull()
        {
            Assert.NotNull(testObject.CarrierAccountEntity as FedExAccountEntity);
        }

        [Fact]
        public void CarrierAccountEntity_ReturnsAccountProvidedInConstructor()
        {
            Assert.Equal(account, testObject.CarrierAccountEntity as FedExAccountEntity);
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
            fedExService.Verify(s => s.Void(testObject.NativeRequest as DeleteShipmentRequest), Times.Once());
        }

        [Fact]
        public void Submit_DelegatesToResponseFactory_WhenCreatingVoidResponse()
        {
            // No additional setup needed since it was performed in Initialize()
            ICarrierResponse response = testObject.Submit();

            // Verify the ship response is created via the response factory using the test object's shipment entity
            responseFactory.Verify(f => f.CreateVoidResponse(It.IsAny<ShipmentReply>(), testObject), Times.Once());
        }
    }
}
