using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.OpenAccount.Api.Request;
using ShipWorks.Shipping.Carriers.UPS.WebServices.OpenAccount;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.OpenAccount.Api.Request
{
    public class UpsOpenAccountRequestTest
    {
        private UpsOpenAccountRequest testObject;

        private Mock<IUpsServiceGateway> upsService;
        private Mock<ICarrierResponse> carrierResponse;
        private Mock<ICarrierResponseFactory> responseFactory;

        private Mock<ICarrierRequestManipulator> firstManipulator;
        private Mock<ICarrierRequestManipulator> secondManipulator;
        private List<ICarrierRequestManipulator> requestManipulators;

        public UpsOpenAccountRequestTest()
        {
            upsService = new Mock<IUpsServiceGateway>();
            upsService.Setup(s => s.OpenAccount(It.IsAny<OpenAccountRequest>())).Returns(new OpenAccountResponse());

            carrierResponse = new Mock<ICarrierResponse>();

            responseFactory = new Mock<ICarrierResponseFactory>();
            responseFactory.Setup(f => f.CreateSubscriptionResponse(It.IsAny<OpenAccountResponse>(), It.IsAny<CarrierRequest>())).Returns(carrierResponse.Object);

            firstManipulator = new Mock<ICarrierRequestManipulator>();
            firstManipulator.Setup(m => m.Manipulate(It.IsAny<CarrierRequest>()));

            secondManipulator = new Mock<ICarrierRequestManipulator>();
            secondManipulator.Setup(m => m.Manipulate(It.IsAny<CarrierRequest>()));

            requestManipulators = new List<ICarrierRequestManipulator>() { firstManipulator.Object, secondManipulator.Object };

            testObject = new UpsOpenAccountRequest(requestManipulators, upsService.Object, responseFactory.Object, new OpenAccountRequest(), new UpsAccountEntity());
        }

        [Fact]
        public void Submit_ManipulatorsExecuted()
        {
            ICarrierResponse response = testObject.Submit();

            firstManipulator.Verify(s => s.Manipulate(testObject), Times.Once());
            secondManipulator.Verify(m => m.Manipulate(testObject), Times.Once());
        }

        [Fact]
        public void Submit_DelegatesToUpsOpenAccountService()
        {
            // No additional setup needed since it was performed in Initialize()
            ICarrierResponse response = testObject.Submit();

            // Verify that the subscribe method was called using the test object's native request
            upsService.Verify(s => s.OpenAccount(testObject.NativeRequest as OpenAccountRequest), Times.Once());
        }

        [Fact]
        public void Submit_DelegatesToResponseFactory_WhenCreatingResponse()
        {
            // No additional setup needed since it was performed in Initialize()
            testObject.Submit();

            // Verify the response is created via the response factory using the test object
            responseFactory.Verify(f => f.CreateSubscriptionResponse(It.IsAny<OpenAccountResponse>(), testObject), Times.Once());
        }
    }
}