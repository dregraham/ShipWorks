using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Void.Response;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.v2013.Void.Response
{
    [TestClass]
    public class FedExVoidResponseTest
    {
        private FedExVoidResponse testObject;
        private ShipmentReply nativeResponse;

        private Mock<CarrierRequest> carrierRequest;

        [TestInitialize]
        public void Initialize()
        {
            carrierRequest = new Mock<CarrierRequest>(null, null);

            nativeResponse = new ShipmentReply();

            testObject = new FedExVoidResponse(nativeResponse, carrierRequest.Object);
        }

        [TestMethod]
        public void Request_ReturnsCarrierRequest_Test()
        {
            CarrierRequest request = testObject.Request;

            Assert.AreEqual(carrierRequest.Object, request);
        }

        [TestMethod]
        public void NativeResponse_ReturnsVoidReply_Test()
        {
            object nativeRespose = testObject.NativeResponse;

            Assert.AreEqual(nativeRespose, nativeResponse);
        }

        [TestMethod]
        [ExpectedException(typeof(FedExApiCarrierException))]
        public void Process_ThrowsFedExApiException_WhenReplyContainsError_Test()
        {
            nativeResponse.HighestSeverity = NotificationSeverityType.ERROR;
            nativeResponse.Notifications = new Notification[] { new Notification { Message = "some message", Code = "23" } };

            testObject.Process();
        }

        [TestMethod]
        [ExpectedException(typeof(FedExApiCarrierException))]
        public void Process_ThrowsFedExApiException_WhenReplyContainsFailure_Test()
        {
            nativeResponse.HighestSeverity = NotificationSeverityType.FAILURE;
            nativeResponse.Notifications = new Notification[] { new Notification { Message = "some message", Code = "23" } };

            testObject.Process();
        }

        [TestMethod]
        public void Process_DelegatesToManipulators_WhenNotificationsIsNull_Test()
        {
            nativeResponse.HighestSeverity = NotificationSeverityType.SUCCESS;
            nativeResponse.Notifications = null;

            testObject.Process();

            //firstManipulator.Verify(m => m.Manipulate(testObject, voidEntity), Times.Once());
            //secondManipulator.Verify(m => m.Manipulate(testObject, voidEntity), Times.Once());
        }

        [TestMethod]
        public void Process_DelegatesToManipulators_WhenNotificationsDoesNotContainCode9804_Test()
        {
            nativeResponse.HighestSeverity = NotificationSeverityType.SUCCESS;
            nativeResponse.Notifications = new Notification[] { new Notification { Code = "8" } };

            testObject.Process();

            //firstManipulator.Verify(m => m.Manipulate(testObject, voidEntity), Times.Once());
            //secondManipulator.Verify(m => m.Manipulate(testObject, voidEntity), Times.Once());
        }

        [TestMethod]
        public void Process_DoesNotDelegatesToManipulators_WhenNotificationsContainsCode9804_Test()
        {
            nativeResponse.HighestSeverity = NotificationSeverityType.SUCCESS;
            nativeResponse.Notifications = new Notification[] { new Notification { Code = "9804" } };

            testObject.Process();

            //firstManipulator.Verify(m => m.Manipulate(testObject, voidEntity), Times.Never());
            //secondManipulator.Verify(m => m.Manipulate(testObject, voidEntity), Times.Never());
        }
    }
}
