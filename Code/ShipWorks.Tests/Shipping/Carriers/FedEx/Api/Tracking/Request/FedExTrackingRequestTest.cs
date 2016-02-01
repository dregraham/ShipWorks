using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Tracking.Request;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Track;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Tracking.Request
{
    public class FedExTrackingRequestTest
    {
        private FedExTrackRequest testObject;
        
        private Mock<IFedExServiceGateway> fedExService;
        private Mock<ICarrierResponse> carrierResponse;
        private Mock<ICarrierResponseFactory> responseFactory;
        
        private Mock<ICarrierRequestManipulator> firstManipulator;
        private Mock<ICarrierRequestManipulator> secondManipulator;

        private ShipmentEntity shipmentEntity;
        private FedExAccountEntity account;

        public FedExTrackingRequestTest()
        {
            shipmentEntity = new ShipmentEntity();
            account = new FedExAccountEntity {AccountNumber = "1234", MeterNumber = "45453"};

            fedExService = new Mock<IFedExServiceGateway>();
            fedExService.Setup(s => s.Track(It.IsAny<TrackRequest>())).Returns(new TrackReply());

            carrierResponse = new Mock<ICarrierResponse>();

            responseFactory = new Mock<ICarrierResponseFactory>();
            responseFactory.Setup(f => f.CreateTrackResponse(It.IsAny<TrackReply>(), It.IsAny<CarrierRequest>())).Returns(carrierResponse.Object);

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

            testObject = new FedExTrackRequest(manipulators, shipmentEntity, fedExService.Object, responseFactory.Object, account);
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
            fedExService.Verify(s => s.Track(testObject.NativeRequest as TrackRequest), Times.Once());
        }

        [Fact]
        public void Submit_DelegatesToResponseFactory_WhenCreatingTrackingResponse()
        {
            // No additional setup needed since it was performed in Initialize()
            ICarrierResponse response = testObject.Submit();

            // Verify the track response is created via the response factory 
            responseFactory.Verify(f => f.CreateTrackResponse(It.IsAny<TrackReply>(), testObject), Times.Once());
        }
    }
}
