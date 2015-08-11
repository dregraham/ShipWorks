using Xunit;
using Moq;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Registration.Response;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Registration.Response
{
    public class FedExVersionCaptureResponseTest
    {
        private FedExVersionCaptureResponse testObject;

        private VersionCaptureReply reply;
        private Mock<CarrierRequest> carrierRequest;

        public FedExVersionCaptureResponseTest()
        {

            reply = new VersionCaptureReply()
            {
                HighestSeverity = NotificationSeverityType.SUCCESS,
                Notifications = new Notification[]
                {
                    new Notification()
                    {
                        Message = "Hello World!"
                    }
                }
            };

            carrierRequest = new Mock<CarrierRequest>(null, null);
            testObject = new FedExVersionCaptureResponse(reply, carrierRequest.Object);
        }

        [Fact]
        public void Process_NoErrorThrown_NoErrorInReply_Test()
        {
            testObject.Process();
        }

        [Fact]
        public void Process_ErrorThrown_ErrorInReply_Test()
        {
            reply.HighestSeverity = NotificationSeverityType.FAILURE;

            Assert.Throws<FedExApiCarrierException>(() => testObject.Process());
        }
    }
}
