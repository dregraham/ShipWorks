using Xunit;
using Moq;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Response;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Rate.Response
{
    public class FedExRateResponseTest
    {
        private FedExRateResponse testObject;

        private RateReply nativeResponse;
        private Mock<CarrierRequest> carrierRequest;

        public FedExRateResponseTest()
        {
            nativeResponse = new RateReply
            {
                HighestSeverity = NotificationSeverityType.SUCCESS,
                Notifications = new Notification[]
                {
                    new Notification() {Code = "0"}
                },
                RateReplyDetails = new RateReplyDetail[0]
            };

            carrierRequest = new Mock<CarrierRequest>(null, null);

            testObject = new FedExRateResponse(nativeResponse, carrierRequest.Object);
        }

        [Fact]
        public void Request_ReturnsRequestProvidedToConstructor_Test()
        {
            Assert.Equal(carrierRequest.Object, testObject.Request);
        }

        [Fact]
        public void NativeResponse_ReturnsRateReplyProvidedToConstructor_Test()
        {
            Assert.Equal(nativeResponse, testObject.NativeResponse);
        }

        [Fact]
        public void Process_ThrowsFedExApiException_WhenResponseContainsError_Test()
        {
            nativeResponse.HighestSeverity = NotificationSeverityType.ERROR;
            Assert.Throws<FedExApiCarrierException>(() => testObject.Process());
        }

        [Fact]
        public void Process_ThrowsFedExApiException_WhenResponseContainsFailure_Test()
        {
            nativeResponse.HighestSeverity = NotificationSeverityType.FAILURE;
            Assert.Throws<FedExApiCarrierException>(() => testObject.Process());
        }

        [Fact]
        public void Process_ThrowsFedExApiException_WhenRateReplyDetailsIsNull_Test()
        {
            nativeResponse.RateReplyDetails = null;
            Assert.Throws<FedExException>(() => testObject.Process());
        }

        [Fact]
        public void Process_ThrowsFedExApiException_WhenRateReplyIsNull_AndNotificationCodeIs556_Test()
        {
            nativeResponse.RateReplyDetails = null;
            nativeResponse.Notifications[0].Code = "556";
            Assert.Throws<FedExException>(() => testObject.Process());
        }

        [Fact]
        public void Process_ThrowsFedExApiException_WhenRateReplyIsNull_AndNotificationCodeIs557_Test()
        {
            nativeResponse.RateReplyDetails = null;
            nativeResponse.Notifications[0].Code = "557";
            Assert.Throws<FedExException>(() => testObject.Process());
        }

        [Fact]
        public void Process_ThrowsFedExApiException_WhenRateReplyIsNull_AndNotificationCodeIs558_Test()
        {
            nativeResponse.RateReplyDetails = null;
            nativeResponse.Notifications[0].Code = "558";
            Assert.Throws<FedExException>(() => testObject.Process());
        }
    }
}
