using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance.InsureShip;

namespace ShipWorks.Tests.Shipping.Insurance.InsureShip
{
    [TestClass]
    public class InsureShipmentResponseTest
    {
        private InsureShipmentResponse testObject;

        private Mock<IInsureShipSettings> settings = new Mock<IInsureShipSettings>();
        private Mock<InsureShipRequestBase> request;
        private Mock<IInsureShipResponseFactory> responseFactory;

        private Mock<ILog> log;
        private Mock<TextWriter> writer;

        private ShipmentEntity shipment;

        [TestInitialize]
        public void Initialize()
        {
            settings.Setup(s => s.UseTestServer).Returns(true);
            settings.Setup(s => s.DistributorID).Returns("D00002");
            settings.Setup(s => s.Username).Returns("test2");
            settings.Setup(s => s.Password).Returns("password");
            settings.Setup(s => s.Url).Returns(new Uri("https://int.insureship.com/api/"));

            shipment = new ShipmentEntity(100031);

            log = new Mock<ILog>();
            log.Setup(l => l.Error(It.IsAny<object>()));

            writer = new Mock<TextWriter>();
            responseFactory = new Mock<IInsureShipResponseFactory>();

            request = new Mock<InsureShipRequestBase>(responseFactory.Object, shipment, new InsureShipAffiliate("test", "test"), settings.Object, log.Object);

            testObject = new InsureShipmentResponse(request.Object, log.Object);
        }

        [TestMethod]        
        public void Process_UsesRawResponse_FromRequest_Test()
        {
            request.Setup(r => r.StatusCode).Returns((int)HttpStatusCode.NoContent);

            testObject.Process();

            request.Verify(r => r.StatusCode, Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(InsureShipResponseException))]
        public void Process_ThrowsInsureShipResponseException_WhenStatusCodeIsNotRecognized_Test()
        {
            request.Setup(r => r.StatusCode).Returns(900);

            testObject.Process();
        }

        [TestMethod]        
        public void Process_LogsMessage_WhenStatusCodeIsNotRecognized_Test()
        {
            request.Setup(r => r.StatusCode).Returns(900);

            try
            {
                testObject.Process();
            }
            catch (InsureShipResponseException)
            { }

            log.Verify(l => l.Error("An unknown response code was received from the InsureShip API for shipment 100031: 900"));
        }

        [TestMethod]
        [ExpectedException(typeof(InsureShipResponseException))]
        public void Process_ThrowsInsureShipResponseException_WhenStatusCodeIsRecongized_ButNotSuccessful_Test()
        {
            request.Setup(r => r.StatusCode).Returns(419);

            testObject.Process();
        }

        [TestMethod]
        public void Process_LogsMessage_WhenStatusCodeIsRecongized_ButNotSuccessful_Test()
        {
            HttpResponse rawResponse = new HttpResponse(writer.Object) { StatusCode = 419 };
            request.Setup(r => r.StatusCode).Returns(419);

            try
            {
                testObject.Process();
            }
            catch (InsureShipResponseException)
            { }

            log.Verify(l => l.Error("An error occurred trying to insure shipment 100031 with the InsureShip API: 419"));
        }

        [TestMethod]
        public void Process_SuccessfulResponse_Test()
        {
            // Response code of 204 is success
            HttpResponse rawResponse = new HttpResponse(writer.Object) { StatusCode = 204 };
            request.Setup(r => r.StatusCode).Returns((int)HttpStatusCode.NoContent);

            testObject.Process();
        }
    }
}
