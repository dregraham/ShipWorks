using Moq;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using Xunit;

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

            testObject = new FedExRateResponse(nativeResponse);
        }

        [Fact]
        public void Process_ReturnsRateReplyProvidedToConstructor()
        {
            var response = testObject.Process();
            Assert.Equal(nativeResponse, response.Value);
        }

        [Theory]
        [InlineData(NotificationSeverityType.ERROR)]
        [InlineData(NotificationSeverityType.FAILURE)]
        public void Process_ThrowsFedExApiException_WhenResponseContainsError(NotificationSeverityType failureType)
        {
            nativeResponse.HighestSeverity = failureType;
            var result = testObject.Process();
            Assert.True(result.Failure);
            Assert.IsAssignableFrom<FedExApiCarrierException>(result.Exception);
        }

        [Fact]
        public void Process_ReturnsFedExApiException_WhenRateReplyDetailsIsNull()
        {
            nativeResponse.RateReplyDetails = null;
            var result = testObject.Process();
            Assert.True(result.Failure);
            Assert.IsAssignableFrom<FedExException>(result.Exception);
            Assert.Equal("FedEx did not return any rates for the shipment.", result.Exception.Message);
        }

        [Theory]
        [InlineData("556")]
        [InlineData("557")]
        [InlineData("558")]
        public void Process_ReturnsFedExApiExceptionWithMessage_WhenRateReplyIsNull(string code)
        {
            nativeResponse.RateReplyDetails = null;
            nativeResponse.Notifications[0].Code = code;
            var result = testObject.Process();
            Assert.True(result.Failure);
            Assert.IsAssignableFrom<FedExException>(result.Exception);
            Assert.Equal("There are no FedEx services available for the selected shipment options.", result.Exception.Message);
        }
    }
}
