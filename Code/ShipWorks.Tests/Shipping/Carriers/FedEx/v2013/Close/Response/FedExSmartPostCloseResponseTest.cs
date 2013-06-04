using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Close.Response;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Close.Response.Manipulators;
using Moq;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Close;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.v2013.Close.Response
{
    [TestClass]
    public class FedExSmartPostCloseResponseTest
    {
        private FedExSmartPostCloseResponse testObject;

        private List<IFedExCloseResponseManipulator> manipulators;
        private Mock<IFedExCloseResponseManipulator> firstManipulator;
        private Mock<IFedExCloseResponseManipulator> secondManipulator;

        private SmartPostCloseReply nativeSmartPostResponse;

        private Mock<CarrierRequest> carrierRequest;

        [TestInitialize]
        public void Initialize()
        {
            firstManipulator = new Mock<IFedExCloseResponseManipulator>();
            firstManipulator.Setup(m => m.Manipulate(It.IsAny<ICarrierResponse>(), It.IsAny<FedExEndOfDayCloseEntity>()));

            secondManipulator = new Mock<IFedExCloseResponseManipulator>();
            secondManipulator.Setup(m => m.Manipulate(It.IsAny<ICarrierResponse>(), It.IsAny<FedExEndOfDayCloseEntity>()));

            manipulators = new List<IFedExCloseResponseManipulator>
            {
                firstManipulator.Object,
                secondManipulator.Object
            };

            carrierRequest = new Mock<CarrierRequest>(null, null);

            nativeSmartPostResponse = new SmartPostCloseReply();

            testObject = new FedExSmartPostCloseResponse(manipulators, nativeSmartPostResponse, carrierRequest.Object);
        }

        [TestMethod]
        public void Request_ReturnsCarrierRequest_Test()
        {
            CarrierRequest request = testObject.Request;

            Assert.AreEqual(carrierRequest.Object, request);
        }

        [TestMethod]
        public void NativeResponse_ReturnsSmartPostCloseReply_Test()
        {
            object nativeRespose = testObject.NativeResponse;

            Assert.AreEqual(nativeRespose, nativeSmartPostResponse);
        }

        [TestMethod]
        [ExpectedException(typeof(FedExApiException))]
        public void Process_ThrowsFedExApiException_WhenReplyContainsError_Test()
        {
            nativeSmartPostResponse.HighestSeverity = NotificationSeverityType.ERROR;
            nativeSmartPostResponse.Notifications = new Notification[] { new Notification { Message = "some message", Code = "23" } };

            testObject.Process();
        }

        [TestMethod]
        [ExpectedException(typeof(FedExApiException))]
        public void Process_ThrowsFedExApiException_WhenReplyContainsFailure_Test()
        {
            nativeSmartPostResponse.HighestSeverity = NotificationSeverityType.FAILURE;
            nativeSmartPostResponse.Notifications = new Notification[] { new Notification { Message = "some message", Code = "23" } };

            testObject.Process();
        }

        [TestMethod]
        public void Process_DelegatesToManipulators_WhenNotificationsIsNull_Test()
        {
            nativeSmartPostResponse.HighestSeverity = NotificationSeverityType.SUCCESS;
            nativeSmartPostResponse.Notifications = null;
            testObject.Process();

            firstManipulator.Verify(m => m.Manipulate(testObject, It.IsAny<FedExEndOfDayCloseEntity>()), Times.Once());
            secondManipulator.Verify(m => m.Manipulate(testObject, It.IsAny<FedExEndOfDayCloseEntity>()), Times.Once());
        }

        [TestMethod]
        public void Process_DelegatesToManipulators_WhenNotificationsDoesNotContainCode9804_Test()
        {
            nativeSmartPostResponse.HighestSeverity = NotificationSeverityType.SUCCESS;
            nativeSmartPostResponse.Notifications = new Notification[] { new Notification { Code = "8" } };

            testObject.Process();

            firstManipulator.Verify(m => m.Manipulate(testObject, It.IsAny<FedExEndOfDayCloseEntity>()), Times.Once());
            secondManipulator.Verify(m => m.Manipulate(testObject, It.IsAny<FedExEndOfDayCloseEntity>()), Times.Once());
        }

        [TestMethod]
        public void Process_DoesNotDelegatesToManipulators_WhenNotificationsContainsCode9804_Test()
        {
            nativeSmartPostResponse.HighestSeverity = NotificationSeverityType.SUCCESS;
            nativeSmartPostResponse.Notifications = new Notification[] { new Notification { Code = "9804" } };

            testObject.Process();

            firstManipulator.Verify(m => m.Manipulate(testObject, It.IsAny<FedExEndOfDayCloseEntity>()), Times.Never());
            secondManipulator.Verify(m => m.Manipulate(testObject, It.IsAny<FedExEndOfDayCloseEntity>()), Times.Never());
        }

        [TestMethod]
        public void Process_CloseEntityIsNotNull_WhenCloseIsSuccessful_Test()
        {
            nativeSmartPostResponse.HighestSeverity = NotificationSeverityType.SUCCESS;
            nativeSmartPostResponse.Notifications = new Notification[] { new Notification { Code = "0" } };

            testObject = new FedExSmartPostCloseResponse(manipulators, nativeSmartPostResponse, carrierRequest.Object);
            testObject.Process();

            Assert.IsNotNull(testObject.CloseEntity);
        }

        [TestMethod]
        public void Process_CloseEntityIsNull_WhenNotificationsContainsCode9804_Test()
        {
            nativeSmartPostResponse.HighestSeverity = NotificationSeverityType.SUCCESS;
            nativeSmartPostResponse.Notifications = new Notification[] { new Notification { Code = "9804" } };

            testObject = new FedExSmartPostCloseResponse(manipulators, nativeSmartPostResponse, carrierRequest.Object);
            testObject.Process();

            Assert.IsNull(testObject.CloseEntity);
        }

        [TestMethod]
        public void Process_CloseEntityIsNotNull_WhenNotificationsIsNull_Test()
        {
            nativeSmartPostResponse.HighestSeverity = NotificationSeverityType.SUCCESS;
            nativeSmartPostResponse.Notifications = null;

            testObject.Process();

            Assert.IsNotNull(testObject.CloseEntity);
        }
    }
}
