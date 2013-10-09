using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Registration.Response;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Registration;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.v2013.Registration
{
    [TestClass]
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
                MeterNumber = "98765"
            };

            account = new FedExAccountEntity();

            carrierRequest = new Mock<CarrierRequest>(null, null);
            carrierRequest.Setup(r => r.CarrierAccountEntity).Returns(account);

            testObject = new FedExSubscriptionResponse(nativeResponse, carrierRequest.Object);
        }

        [TestMethod]
        [ExpectedException((typeof(FedExApiCarrierException)))]
        public void Process_ThrowsFedExApiException_WhenSeverityIsError_Test()
        {
            nativeResponse.HighestSeverity = NotificationSeverityType.ERROR;
            nativeResponse.Notifications = new Notification[] {new Notification {Message = "message"}};
            
            testObject.Process();
        }

        [TestMethod]
        [ExpectedException((typeof(FedExApiCarrierException)))]
        public void Process_ThrowsFedExApiException_WhenSeverityIsFailure_Test()
        {
            nativeResponse.HighestSeverity = NotificationSeverityType.FAILURE;
            nativeResponse.Notifications = new Notification[] { new Notification { Message = "message" } };

            testObject.Process();
        }

        [TestMethod]
        public void Process_GetsAccountFromRequest_Test()
        {
            testObject.Process();

            carrierRequest.Verify(r =>r.CarrierAccountEntity, Times.Once());
        }

        [TestMethod]
        public void Process_SetsMeterNumberOfAccountFromRequest_Test()
        {
            testObject.Process();

            FedExAccountEntity requestAccount = carrierRequest.Object.CarrierAccountEntity as FedExAccountEntity;
            Assert.AreEqual(nativeResponse.MeterNumber, requestAccount.MeterNumber);
        }
    }
}
