using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Registration.Response;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Registration;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.v2013.Registration.Response
{
    [TestClass]
    public class FedExVersionCaptureResponseTest
    {
        private FedExVersionCaptureResponse testObject;

        private VersionCaptureReply reply;
        private Mock<CarrierRequest> carrierRequest;

        [TestInitialize]
        public void Initialize()
        {

            reply = new VersionCaptureReply()
            {
                HighestSeverity = NotificationSeverityType.SUCCESS,
                Notifications = new Notification[]
                {
                    new Notification()
                    {
                        Message = "Hello World!"
                    }
                }
            };

            carrierRequest = new Mock<CarrierRequest>(null, null);
            testObject = new FedExVersionCaptureResponse(reply, carrierRequest.Object);
        }

        [TestMethod]
        public void Process_NoErrorThrown_NoErrorInReply_Test()
        {
            testObject.Process();
        }

        [TestMethod]
        [ExpectedException(typeof(FedExApiCarrierException))]
        public void Process_ErrorThrown_ErrorInReply_Test()
        {
            reply.HighestSeverity=NotificationSeverityType.FAILURE;

            testObject.Process();
        }
    }
}
