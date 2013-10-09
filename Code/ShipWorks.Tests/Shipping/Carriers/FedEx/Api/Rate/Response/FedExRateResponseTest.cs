using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Response;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Rate;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Rate.Response
{
    [TestClass]
    public class FedExRateResponseTest
    {
        private FedExRateResponse testObject;
        
        private RateReply nativeResponse;
        private Mock<CarrierRequest> carrierRequest;

        [TestInitialize]
        public void Initialize()
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

        [TestMethod]
        public void Request_ReturnsRequestProvidedToConstructor_Test()
        {
            Assert.AreEqual(carrierRequest.Object, testObject.Request);
        }

        [TestMethod]
        public void NativeResponse_ReturnsRateReplyProvidedToConstructor_Test()
        {
            Assert.AreEqual(nativeResponse, testObject.NativeResponse);
        }

        [TestMethod]
        [ExpectedException(typeof(FedExApiCarrierException))]
        public void Process_ThrowsFedExApiException_WhenResponseContainsError_Test()
        {
            nativeResponse.HighestSeverity = NotificationSeverityType.ERROR;
            testObject.Process();
        }

        [TestMethod]
        [ExpectedException(typeof(FedExApiCarrierException))]
        public void Process_ThrowsFedExApiException_WhenResponseContainsFailure_Test()
        {
            nativeResponse.HighestSeverity = NotificationSeverityType.FAILURE;
            testObject.Process();
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void Process_ThrowsFedExApiException_WhenRateReplyDetailsIsNull_Test()
        {
            nativeResponse.RateReplyDetails = null;
            testObject.Process();
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void Process_ThrowsFedExApiException_WhenRateReplyIsNull_AndNotificationCodeIs556_Test()
        {
            nativeResponse.RateReplyDetails = null;
            nativeResponse.Notifications[0].Code = "556";
            testObject.Process();
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void Process_ThrowsFedExApiException_WhenRateReplyIsNull_AndNotificationCodeIs557_Test()
        {
            nativeResponse.RateReplyDetails = null;
            nativeResponse.Notifications[0].Code = "557";
            testObject.Process();
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void Process_ThrowsFedExApiException_WhenRateReplyIsNull_AndNotificationCodeIs558_Test()
        {
            nativeResponse.RateReplyDetails = null;
            nativeResponse.Notifications[0].Code = "558";
            testObject.Process();
        }
    }
}
