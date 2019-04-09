using Xunit;
using Moq;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.UploadDocuments.Response;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.UploadDocument;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.UploadDocuments.Response
{
    public class FedExUploadingImagesResponseTest
    {
        FedExUploadImagesResponse testObject;
        UploadImagesReply reply;

        private Mock<CarrierRequest> carrierRequest;

        public FedExUploadingImagesResponseTest()
        {
            reply = new UploadImagesReply
            {
                HighestSeverity = NotificationSeverityType.SUCCESS,
                Notifications = new[]
                {
                    new Notification
                    {
                        Message = "Foo"
                    }
                }
            };

            carrierRequest = new Mock<CarrierRequest>(null, null);
            testObject = new FedExUploadImagesResponse(reply, carrierRequest.Object);
        }

        [Fact]
        public void Process_ExceptionWillBeThrown_ReplyWillContainErrorInHighestSeverity()
        {
            reply.HighestSeverity = NotificationSeverityType.FAILURE;

            Assert.Throws<FedExApiCarrierException>(() => testObject.Process());
        }
    }
}