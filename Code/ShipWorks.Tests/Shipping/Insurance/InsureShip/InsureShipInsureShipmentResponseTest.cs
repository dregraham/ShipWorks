using System;
using System.Net;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance.InsureShip;

namespace ShipWorks.Tests.Shipping.Insurance.InsureShip
{
    [TestClass]
    public class InsureShipInsureShipmentResponseTest
    {
        private InsureShipInsureShipmentResponse testObject;

        private Mock<IInsureShipSettings> settings;
        private Mock<InsureShipRequestBase> request;

        private Mock<IInsureShipResponseFactory> responseFactory;
        Mock<HttpWebResponse> response = new Mock<HttpWebResponse>();

        private Mock<ILog> log;

        private ShipmentEntity shipment;

        [TestInitialize]
        public void Initialize()
        {
            shipment = new ShipmentEntity(100031);

            settings = new Mock<IInsureShipSettings>();
            settings.Setup(s => s.UseTestServer).Returns(true);
            settings.Setup(s => s.DistributorID).Returns("D00002");
            settings.Setup(s => s.Username).Returns("test2");
            settings.Setup(s => s.Password).Returns("password");
            settings.Setup(s => s.Url).Returns(new Uri("https://int.insureship.com/api/"));
            
            log = new Mock<ILog>();
            log.Setup(l => l.Error(It.IsAny<object>()));

            responseFactory = new Mock<IInsureShipResponseFactory>();

            response = new Mock<HttpWebResponse>();
            response.Setup(r => r.StatusCode).Returns(HttpStatusCode.NoContent);

            request = new Mock<InsureShipRequestBase>(responseFactory.Object, shipment, new InsureShipAffiliate("test", "test"), settings.Object, log.Object);
            request.Setup(r => r.RawResponse).Returns(response.Object);

            testObject = new InsureShipInsureShipmentResponse(request.Object, log.Object);
        }

        [TestMethod]        
        public void Process_UsesRawResponse_FromRequest_Test()
        {
            testObject.Process();

            request.Verify(r => r.RawResponse, Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(InsureShipResponseException))]
        public void Process_ThrowsInsureShipResponseException_WhenStatusCodeIsNotExpected_Test()
        {
            response.Setup(r => r.StatusCode).Returns(HttpStatusCode.Found);

            testObject.Process();
        }

        [TestMethod]        
        public void Process_LogsMessage_WhenStatusCodeIsNotRecognized_Test()
        {
            response.Setup(r => r.StatusCode).Returns(HttpStatusCode.Found);

            try
            {
                testObject.Process();
            }
            catch (InsureShipResponseException)
            { }

            log.Verify(l => l.Error("An unknown response code was received from the InsureShip API for shipment 100031: 302"));
        }

        [TestMethod]
        [ExpectedException(typeof(InsureShipResponseException))]
        public void Process_ThrowsInsureShipResponseException_WhenStatusCodeIsRecongized_ButNotSuccessful_Test()
        {
            response.Setup(r => r.StatusCode).Returns(HttpStatusCode.Conflict);

            testObject.Process();
        }

        [TestMethod]
        [ExpectedException(typeof(InsureShipResponseException))]
        public void Process_ThrowsInsureShipResponseException_WhenStatusCodeIs419_ButNotSuccessful_Test()
        {
            // Called out specifically since there is not an HttpStatusCode entry for 419
            response.Setup(r => r.StatusCode).Returns((HttpStatusCode)419);

            testObject.Process();
        }

        [TestMethod]
        public void Process_LogsMessage_WhenStatusCodeIs419_ButNotSuccessful_Test()
        {
            // Called out specifically since there is not an HttpStatusCode entry for 419
            response.Setup(r => r.StatusCode).Returns((HttpStatusCode)419);

            try
            {
                testObject.Process();
            }
            catch (InsureShipResponseException)
            { }

            log.Verify(l => l.Error("An error occurred trying to insure shipment 100031 with the InsureShip API: 419"));
        }

        [TestMethod]
        public void Process_LogsMessage_WhenStatusCodeIsRecongized_ButNotSuccessful_Test()
        {
            response.Setup(r => r.StatusCode).Returns(HttpStatusCode.Conflict);

            try
            {
                testObject.Process();
            }
            catch (InsureShipResponseException)
            { }

            log.Verify(l => l.Error("An error occurred trying to insure shipment 100031 with the InsureShip API: 409"));
        }

        [TestMethod]
        public void Process_SuccessfulResponse_Test()
        {
            // Response code of 204 is success
            response.Setup(r => r.StatusCode).Returns(HttpStatusCode.NoContent);

            testObject.Process();
        }
    }
}
