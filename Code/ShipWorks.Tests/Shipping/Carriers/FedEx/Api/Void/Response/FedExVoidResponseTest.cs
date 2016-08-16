using Xunit;
using Moq;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Void.Response;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Void.Response
{
    public class FedExVoidResponseTest
    {
        private FedExVoidResponse testObject;
        private ShipmentReply nativeResponse;

        private Mock<CarrierRequest> carrierRequest;

        public FedExVoidResponseTest()
        {
            carrierRequest = new Mock<CarrierRequest>(null, null);

            nativeResponse = new ShipmentReply();

            testObject = new FedExVoidResponse(nativeResponse, carrierRequest.Object);
        }

        [Fact]
        public void Request_ReturnsCarrierRequest()
        {
            CarrierRequest request = testObject.Request;

            Assert.Equal(carrierRequest.Object, request);
        }

        [Fact]
        public void NativeResponse_ReturnsVoidReply()
        {
            object nativeRespose = testObject.NativeResponse;

            Assert.Equal(nativeRespose, nativeResponse);
        }

        [Fact]
        public void Process_ThrowsFedExApiException_WhenReplyContainsError()
        {
            nativeResponse.HighestSeverity = NotificationSeverityType.ERROR;
            nativeResponse.Notifications = new Notification[] { new Notification { Message = "some message", Code = "23" } };

            Assert.Throws<FedExApiCarrierException>(() => testObject.Process());
        }

        [Fact]
        public void Process_ThrowsFedExApiException_WhenReplyContainsFailure()
        {
            nativeResponse.HighestSeverity = NotificationSeverityType.FAILURE;
            nativeResponse.Notifications = new Notification[] { new Notification { Message = "some message", Code = "23" } };

            Assert.Throws<FedExApiCarrierException>(() => testObject.Process());
        }

        [Fact]
        public void Process_DelegatesToManipulators_WhenNotificationsIsNull()
        {
            nativeResponse.HighestSeverity = NotificationSeverityType.SUCCESS;
            nativeResponse.Notifications = null;

            testObject.Process();

            //firstManipulator.Verify(m => m.Manipulate(testObject, voidEntity), Times.Once());
            //secondManipulator.Verify(m => m.Manipulate(testObject, voidEntity), Times.Once());
        }

        [Fact]
        public void Process_DelegatesToManipulators_WhenNotificationsDoesNotContainCode9804()
        {
            nativeResponse.HighestSeverity = NotificationSeverityType.SUCCESS;
            nativeResponse.Notifications = new Notification[] { new Notification { Code = "8" } };

            testObject.Process();

            //firstManipulator.Verify(m => m.Manipulate(testObject, voidEntity), Times.Once());
            //secondManipulator.Verify(m => m.Manipulate(testObject, voidEntity), Times.Once());
        }

        [Fact]
        public void Process_DoesNotDelegatesToManipulators_WhenNotificationsContainsCode9804()
        {
            nativeResponse.HighestSeverity = NotificationSeverityType.SUCCESS;
            nativeResponse.Notifications = new Notification[] { new Notification { Code = "9804" } };

            testObject.Process();

            //firstManipulator.Verify(m => m.Manipulate(testObject, voidEntity), Times.Never());
            //secondManipulator.Verify(m => m.Manipulate(testObject, voidEntity), Times.Never());
        }
    }
}
