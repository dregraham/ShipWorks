using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Close.Response;
using ShipWorks.Shipping.Carriers.FedEx.Api.Close.Response.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Close;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Close.Response
{
    public class FedExGroundCloseResponseTest
    {
        private FedExGroundCloseResponse testObject;

        private List<IFedExCloseResponseManipulator> manipulators;
        private Mock<IFedExCloseResponseManipulator> firstManipulator;
        private Mock<IFedExCloseResponseManipulator> secondManipulator;

        private GroundCloseReply nativeGroundResponse;

        private Mock<CarrierRequest> carrierRequest;


        public FedExGroundCloseResponseTest()
        {
            firstManipulator = new Mock<IFedExCloseResponseManipulator>();
            firstManipulator.Setup(m => m.Manipulate(It.IsAny<ICarrierResponse>(), It.IsAny<FedExEndOfDayCloseEntity>()));

            secondManipulator = new Mock<IFedExCloseResponseManipulator>();
            secondManipulator.Setup(m => m.Manipulate(It.IsAny<ICarrierResponse>(), It.IsAny<FedExEndOfDayCloseEntity>()));

            manipulators = new List<IFedExCloseResponseManipulator>
            {
                firstManipulator.Object,
                secondManipulator.Object
            };

            carrierRequest = new Mock<CarrierRequest>(null, null);

            nativeGroundResponse = new GroundCloseReply();

            testObject = new FedExGroundCloseResponse(manipulators, nativeGroundResponse, carrierRequest.Object);
        }

        [Fact]
        public void Request_ReturnsCarrierRequest()
        {
            CarrierRequest request = testObject.Request;

            Assert.Equal(carrierRequest.Object, request);
        }

        [Fact]
        public void NativeResponse_ReturnsGroundCloseReply()
        {
            object nativeRespose = testObject.NativeResponse;

            Assert.Equal(nativeRespose, nativeGroundResponse);
        }

        [Fact]
        public void Process_ThrowsFedExApiException_WhenReplyContainsError()
        {
            nativeGroundResponse.HighestSeverity = NotificationSeverityType.ERROR;
            nativeGroundResponse.Notifications = new Notification[] { new Notification { Message = "some message", Code = "23" } };

            Assert.Throws<FedExApiCarrierException>(() => testObject.Process());
        }

        [Fact]
        public void Process_ThrowsFedExApiException_WhenReplyContainsFailure()
        {
            nativeGroundResponse.HighestSeverity = NotificationSeverityType.FAILURE;
            nativeGroundResponse.Notifications = new Notification[] { new Notification { Message = "some message", Code = "23" } };

            Assert.Throws<FedExApiCarrierException>(() => testObject.Process());
        }

        [Fact]
        public void Process_DelegatesToManipulators_WhenNotificationsIsNull()
        {
            nativeGroundResponse.HighestSeverity = NotificationSeverityType.SUCCESS;
            nativeGroundResponse.Notifications = null;

            testObject.Process();

            firstManipulator.Verify(m => m.Manipulate(testObject, It.IsAny<FedExEndOfDayCloseEntity>()), Times.Once());
            secondManipulator.Verify(m => m.Manipulate(testObject, It.IsAny<FedExEndOfDayCloseEntity>()), Times.Once());
        }

        [Fact]
        public void Process_DelegatesToManipulators_WhenNotificationsDoesNotContainCode9804()
        {
            nativeGroundResponse.HighestSeverity = NotificationSeverityType.SUCCESS;
            nativeGroundResponse.Notifications = new Notification[] { new Notification { Code = "8" } };

            testObject.Process();

            firstManipulator.Verify(m => m.Manipulate(testObject, It.IsAny<FedExEndOfDayCloseEntity>()), Times.Once());
            secondManipulator.Verify(m => m.Manipulate(testObject, It.IsAny<FedExEndOfDayCloseEntity>()), Times.Once());
        }

        [Fact]
        public void Process_DoesNotDelegatesToManipulators_WhenNotificationsContainsCode9804()
        {
            nativeGroundResponse.HighestSeverity = NotificationSeverityType.SUCCESS;
            nativeGroundResponse.Notifications = new Notification[] { new Notification { Code = "9804" } };

            testObject.Process();

            firstManipulator.Verify(m => m.Manipulate(testObject, It.IsAny<FedExEndOfDayCloseEntity>()), Times.Never());
            secondManipulator.Verify(m => m.Manipulate(testObject, It.IsAny<FedExEndOfDayCloseEntity>()), Times.Never());
        }

        [Fact]
        public void Process_CloseEntityIsNotNull_WhenCloseIsSuccessful()
        {
            nativeGroundResponse.HighestSeverity = NotificationSeverityType.SUCCESS;
            nativeGroundResponse.Notifications = new Notification[] { new Notification { Code = "0" } };

            testObject = new FedExGroundCloseResponse(manipulators, nativeGroundResponse, carrierRequest.Object);
            testObject.Process();

            Assert.NotNull(testObject.CloseEntity);
        }

        [Fact]
        public void Process_CloseEntityIsNull_WhenNotificationsContainsCode9804()
        {
            nativeGroundResponse.HighestSeverity = NotificationSeverityType.SUCCESS;
            nativeGroundResponse.Notifications = new Notification[] { new Notification { Code = "9804" } };

            testObject = new FedExGroundCloseResponse(manipulators, nativeGroundResponse, carrierRequest.Object);
            testObject.Process();

            Assert.Null(testObject.CloseEntity);
        }

        [Fact]
        public void Process_CloseEntityIsNotNull_WhenNotificationsIsNull()
        {
            nativeGroundResponse.HighestSeverity = NotificationSeverityType.SUCCESS;
            nativeGroundResponse.Notifications = null;

            testObject.Process();

            Assert.NotNull(testObject.CloseEntity);
        }
    }
}
