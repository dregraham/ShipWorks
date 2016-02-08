using System;
using System.Net;
using ShipWorks.Shipping.Insurance.InsureShip.Net;
using log4net;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance.InsureShip;
using ShipWorks.Shipping.Insurance.InsureShip.Net.Void;

namespace ShipWorks.Tests.Shipping.Insurance.InsureShip.Net.Void
{
    public class InsureShipVoidPolicyResponseTest
    {
        private InsureShipVoidPolicyResponse testObject;

        private Mock<IInsureShipSettings> settings;
        private Mock<InsureShipRequestBase> request;

        private Mock<IInsureShipResponseFactory> responseFactory;

        private Mock<ILog> log;

        private ShipmentEntity shipment;

        public InsureShipVoidPolicyResponseTest()
        {
            shipment = new ShipmentEntity(100031);

            settings = new Mock<IInsureShipSettings>();
            settings.Setup(s => s.UseTestServer).Returns(true);
            settings.Setup(s => s.DistributorID).Returns("D00002");
            settings.Setup(s => s.Username).Returns("test2");
            settings.Setup(s => s.Password).Returns("password");
            settings.Setup(s => s.ApiUrl).Returns(new Uri("https://int.insureship.com/api/"));

            log = new Mock<ILog>();
            log.Setup(l => l.Error(It.IsAny<object>()));

            responseFactory = new Mock<IInsureShipResponseFactory>();

            request = new Mock<InsureShipRequestBase>(responseFactory.Object, shipment, new InsureShipAffiliate("test", "test"), settings.Object, log.Object, "VoidPolicy");
            request.Setup(r => r.ResponseStatusCode).Returns(HttpStatusCode.NoContent);

            testObject = new InsureShipVoidPolicyResponse(request.Object, log.Object);
        }

        [Fact]
        public void Process_UsesRawResponse_FromRequest()
        {
            testObject.Process();

            request.Verify(r => r.ResponseStatusCode, Times.Once());
        }

        [Fact]
        public void Process_ThrowsInsureShipResponseException_WhenStatusCodeIsNotExpected()
        {
            request.Setup(r => r.ResponseStatusCode).Returns(HttpStatusCode.Found);

            Assert.Throws<InsureShipResponseException>(() => testObject.Process());
        }

        [Fact]
        public void Process_LogsMessage_WhenStatusCodeIsNotRecognized()
        {
            request.Setup(r => r.ResponseStatusCode).Returns(HttpStatusCode.Found);

            try
            {
                testObject.Process();
            }
            catch (InsureShipResponseException)
            { }

            log.Verify(l => l.Error("An unknown response code was received from the InsureShip API while attempting to void shipment 100031: 302"));
        }

        [Fact]
        public void Process_ThrowsInsureShipResponseException_WhenStatusCodeIsRecongized_ButNotSuccessful()
        {
            request.Setup(r => r.ResponseStatusCode).Returns(HttpStatusCode.Conflict);

            Assert.Throws<InsureShipResponseException>(() => testObject.Process());
        }

        [Fact]
        public void Process_ThrowsInsureShipResponseException_WhenStatusCodeIs419_ButNotSuccessful()
        {
            // Called out specifically since there is not an HttpStatusCode entry for 419
            request.Setup(r => r.ResponseStatusCode).Returns((HttpStatusCode)419);

            Assert.Throws<InsureShipResponseException>(() => testObject.Process());
        }

        [Fact]
        public void Process_LogsMessage_WhenStatusCodeIs419_ButNotSuccessful()
        {
            // Called out specifically since there is not an HttpStatusCode entry for 419
            request.Setup(r => r.ResponseStatusCode).Returns((HttpStatusCode)419);

            try
            {
                testObject.Process();
            }
            catch (InsureShipResponseException)
            { }

            log.Verify(l => l.Error("An error occurred trying to void a policy for shipment 100031 with the InsureShip API: 419"));
        }

        [Fact]
        public void Process_LogsMessage_WhenStatusCodeIsRecongized_ButNotSuccessful()
        {
            request.Setup(r => r.ResponseStatusCode).Returns(HttpStatusCode.Conflict);

            try
            {
                testObject.Process();
            }
            catch (InsureShipResponseException)
            { }

            log.Verify(l => l.Error("An error occurred trying to void a policy for shipment 100031 with the InsureShip API: 409"));
        }

        [Fact]
        public void Process_SuccessfulResponse()
        {
            // Response code of 204 is success
            request.Setup(r => r.ResponseStatusCode).Returns(HttpStatusCode.NoContent);

            testObject.Process();
        }

        [Fact]
        public void Process_SuccessfulResponseWith200()
        {
            // Response code of 200 is success as well
            request.Setup(r => r.ResponseStatusCode).Returns(HttpStatusCode.OK);

            testObject.Process();
        }
    }
}
