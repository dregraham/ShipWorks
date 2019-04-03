using System.Collections.Generic;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Api.UploadDocuments.Request;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.UploadDocument;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.UploadDocuments.Request
{
    public class FedExUploadImagesRequestTest
    {
        private readonly FedExUploadImagesRequest testObject;
        private readonly FedExAccountEntity account;

        private readonly Mock<IFedExServiceGateway> fedExService;
        private readonly Mock<ICarrierResponse> carrierResponse;
        private readonly Mock<IFedExResponseFactory> responseFactory;
        private readonly Mock<ICarrierRequestManipulator> firstManipulator;
        private readonly Mock<ICarrierRequestManipulator> secondManipulator;

        public FedExUploadImagesRequestTest()
        {
            fedExService = new Mock<IFedExServiceGateway>();
            carrierResponse = new Mock<ICarrierResponse>();
            responseFactory = new Mock<IFedExResponseFactory>();
            firstManipulator = new Mock<ICarrierRequestManipulator>();
            secondManipulator = new Mock<ICarrierRequestManipulator>();

            account = new FedExAccountEntity { AccountNumber = "12345", MeterNumber = "12345" };

            fedExService.Setup(s => s.UploadImages(It.IsAny<UploadImagesRequest>())).Returns(new UploadImagesReply());

            responseFactory.Setup(f => f.CreateUploadImagesResponse(It.IsAny<UploadImagesReply>(), 
                It.IsAny<CarrierRequest>())).Returns(carrierResponse.Object);

            firstManipulator.Setup(m => m.Manipulate(It.IsAny<CarrierRequest>()));
            secondManipulator.Setup(m => m.Manipulate(It.IsAny<CarrierRequest>()));

            // Create some mocked manipulators for testing purposes
            List<ICarrierRequestManipulator> manipulators = new List<ICarrierRequestManipulator>()
            {
                firstManipulator.Object,
                secondManipulator.Object
            };

            testObject = new FedExUploadImagesRequest(manipulators, fedExService.Object, responseFactory.Object, account);
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

            // Verify that the track method was called using the test object's native request
            fedExService.Verify(s => s.UploadImages(testObject.NativeRequest as UploadImagesRequest), Times.Once());
        }

        [Fact]
        public void Submit_DelegatesToResponseFactory_WhenCreatingUploadImagesResponse()
        {
            // No additional setup needed since it was performed in Initialize()
            ICarrierResponse response = testObject.Submit();

            // Verify the track response is created via the response factory 
            responseFactory.Verify(f => f.CreateUploadImagesResponse(It.IsAny<UploadImagesReply>(), testObject), Times.Once());
        }
    }
}