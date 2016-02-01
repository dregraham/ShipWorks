using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Registration.Response;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Registration.Response
{
    public class FedExSubscriptionResponseTest
    {
        private FedExSubscriptionResponse testObject;

        private SubscriptionReply nativeResponse;
        private Mock<CarrierRequest> carrierRequest;

        private FedExAccountEntity account;

        public FedExSubscriptionResponseTest()
        {
            nativeResponse = new SubscriptionReply()
            {
                HighestSeverity = NotificationSeverityType.SUCCESS,
                MeterDetail = new MeterDetail() { MeterNumber = "98765" }
            };

            account = new FedExAccountEntity();

            carrierRequest = new Mock<CarrierRequest>(null, null);
            carrierRequest.Setup(r => r.CarrierAccountEntity).Returns(account);

            testObject = new FedExSubscriptionResponse(nativeResponse, carrierRequest.Object);
        }

        [Fact]
        public void Process_ThrowsFedExApiException_WhenSeverityIsError()
        {
            nativeResponse.HighestSeverity = NotificationSeverityType.ERROR;
            nativeResponse.Notifications = new Notification[] { new Notification { Message = "message" } };

            Assert.Throws<FedExApiCarrierException>(() => testObject.Process());
        }

        [Fact]
        public void Process_ThrowsFedExApiException_WhenSeverityIsFailure()
        {
            nativeResponse.HighestSeverity = NotificationSeverityType.FAILURE;
            nativeResponse.Notifications = new Notification[] { new Notification { Message = "message" } };

            Assert.Throws<FedExApiCarrierException>(() => testObject.Process());
        }

        [Fact]
        public void Process_GetsAccountFromRequest()
        {
            testObject.Process();

            carrierRequest.Verify(r => r.CarrierAccountEntity, Times.Once());
        }

        [Fact]
        public void Process_SetsMeterNumberOfAccountFromRequest()
        {
            testObject.Process();

            FedExAccountEntity requestAccount = carrierRequest.Object.CarrierAccountEntity as FedExAccountEntity;
            Assert.Equal(nativeResponse.MeterDetail.MeterNumber, requestAccount.MeterNumber);
        }
    }
}
