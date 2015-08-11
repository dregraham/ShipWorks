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

        [TestInitialize]
        public void Initialize()
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
        [ExpectedException((typeof(FedExApiCarrierException)))]
        public void Process_ThrowsFedExApiException_WhenSeverityIsError_Test()
        {
            nativeResponse.HighestSeverity = NotificationSeverityType.ERROR;
            nativeResponse.Notifications = new Notification[] {new Notification {Message = "message"}};
            
            testObject.Process();
        }

        [Fact]
        [ExpectedException((typeof(FedExApiCarrierException)))]
        public void Process_ThrowsFedExApiException_WhenSeverityIsFailure_Test()
        {
            nativeResponse.HighestSeverity = NotificationSeverityType.FAILURE;
            nativeResponse.Notifications = new Notification[] { new Notification { Message = "message" } };

            testObject.Process();
        }

        [Fact]
        public void Process_GetsAccountFromRequest_Test()
        {
            testObject.Process();

            carrierRequest.Verify(r =>r.CarrierAccountEntity, Times.Once());
        }

        [Fact]
        public void Process_SetsMeterNumberOfAccountFromRequest_Test()
        {
            testObject.Process();

            FedExAccountEntity requestAccount = carrierRequest.Object.CarrierAccountEntity as FedExAccountEntity;
            Assert.AreEqual(nativeResponse.MeterDetail.MeterNumber, requestAccount.MeterNumber);
        }
    }
}
