using System.Collections.Generic;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.GlobalShipAddress.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.GlobalShipAddress.Request
{
    public class FedExGlobalShipAddressRequestTest
    {
        private readonly Mock<IFedExServiceGateway> mockService = new Mock<IFedExServiceGateway>();

        private Mock<ICarrierRequestManipulator> firstManipulator;
        private Mock<IFedExResponseFactory> responseFactory;
        private Mock<ICarrierRequestManipulator> secondManipulator;
        private FedExGlobalShipAddressRequest testObject;
        private FedExAccountEntity account;

        public FedExGlobalShipAddressRequestTest()
        {
            account = new FedExAccountEntity { AccountNumber = "1234", MeterNumber = "45453" };

            mockService.Setup(service => service.GlobalShipAddressInquiry(It.IsAny<SearchLocationsRequest>()));

            firstManipulator = new Mock<ICarrierRequestManipulator>();
            firstManipulator.Setup(m => m.Manipulate(It.IsAny<CarrierRequest>()));

            secondManipulator = new Mock<ICarrierRequestManipulator>();
            secondManipulator.Setup(m => m.Manipulate(It.IsAny<CarrierRequest>()));

            responseFactory = new Mock<IFedExResponseFactory>();
            responseFactory.Setup(x => x.CreateGlobalShipAddressResponse(It.IsAny<SearchLocationsReply>(), testObject));

            // Create some mocked manipulators for testing purposes
            List<ICarrierRequestManipulator> manipulators = new List<ICarrierRequestManipulator>
            {
                firstManipulator.Object,
                secondManipulator.Object
            };

            testObject = new FedExGlobalShipAddressRequest(manipulators, new ShipmentEntity(), responseFactory.Object, mockService.Object, account);
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
        public void Submit_ManipulatesManipulators_HasTwoManipulators()
        {
            testObject.Submit();

            firstManipulator.Verify(m => m.Manipulate(testObject), Times.Once());
            secondManipulator.Verify(m => m.Manipulate(testObject), Times.Once());
        }

        [Fact]
        public void Submit_GatewayCalled_UponSubmit()
        {
            testObject.Submit();

            mockService.Verify(service => service.GlobalShipAddressInquiry(It.IsAny<SearchLocationsRequest>()), Times.Once());
        }
    }
}
