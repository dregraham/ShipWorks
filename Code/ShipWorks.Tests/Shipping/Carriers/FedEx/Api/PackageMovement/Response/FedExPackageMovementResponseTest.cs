using Xunit;
using Moq;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.PackageMovement.Response;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.PackageMovement;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.PackageMovement.Response
{
    public class FedExPackageMovementResponseTest
    {
        FedExPackageMovementResponse testObject;
        PostalCodeInquiryReply reply;

        private Mock<CarrierRequest> carrierRequest;

        public FedExPackageMovementResponseTest()
        {

            reply = new PostalCodeInquiryReply()
            {
                HighestSeverity = NotificationSeverityType.SUCCESS,
                Notifications = new[]
                {
                    new Notification()
                    {
                        Message = "Hi Mom!"
                    }
                },
                ExpressDescription = new PostalCodeServiceAreaDescription()
                {
                    LocationId = "90210"
                }
            };

            carrierRequest = new Mock<CarrierRequest>(null, null);
            testObject = new FedExPackageMovementResponse(reply, carrierRequest.Object);

        }

        [Fact]
        public void Process_LocationIdWillBeSet_ReplyWillShowSuccessAndLocation()
        {
            testObject.Process();

            Assert.Equal(reply.ExpressDescription.LocationId, testObject.LocationID);
        }

        [Fact]
        public void Process_ExceptionWillBeThrown_ReplyWillContainErrorInHighestSeverity()
        {
            reply.HighestSeverity = NotificationSeverityType.FAILURE;

            Assert.Throws<FedExApiCarrierException>(() => testObject.Process());
        }
    }
}
