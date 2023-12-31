using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Registration.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.Registration.Response;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Registration.Request
{
    public class FedExVersionCaptureRequestTest
    {
        private FedExVersionCaptureRequest testObject;

        Mock<ICarrierRequestManipulator> firstManipulator;
        Mock<ICarrierRequestManipulator> secondManipulator;
        private FedExAccountEntity account;

        public FedExVersionCaptureRequestTest()
        {
            account = new FedExAccountEntity { AccountNumber = "1234", MeterNumber = "45453" };

            Mock<IFedExServiceGateway> mockService = new Mock<IFedExServiceGateway>();
            mockService.Setup(service => service.VersionCapture(It.IsAny<VersionCaptureRequest>()));

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

            testObject = new FedExVersionCaptureRequest(manipulators, new ShipmentEntity(), "123", mockService.Object, account);
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
        public void Submit_FedExVersionCaptureResponseReturned_MakesValidRequest()
        {
            FedExVersionCaptureResponse response = (FedExVersionCaptureResponse) testObject.Submit();

            Assert.NotNull(response);
        }

        [Fact]
        public void Submit_LocationIDIsPopulatedInNativeRequest_LocationPassedInConstructor()
        {
            testObject.Submit();

            VersionCaptureRequest request = (VersionCaptureRequest) testObject.NativeRequest;

            Assert.Equal("123",request.OriginLocationId);
        }

        [Fact]
        public void Submit_DelegatesToManipulators()
        {
            // No additional setup needed since it was performed in Initialize()
            testObject.Submit();

            // Verify all of the manipulators were called and our test object was passed to them
            firstManipulator.Verify(m => m.Manipulate(testObject), Times.Once());
            secondManipulator.Verify(m => m.Manipulate(testObject), Times.Once());
        }
    }
}
