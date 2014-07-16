using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        private Mock<InsureShipRequestBase> request;
        private Mock<IInsureShipResponseFactory> responseFactory;

        private Mock<ILog> log;
        private Mock<TextWriter> writer;

        private ShipmentEntity shipment;

        [TestInitialize]
        public void Initialize()
        {
            shipment = new ShipmentEntity(100031);

            log = new Mock<ILog>();
            log.Setup(l => l.Error(It.IsAny<object>()));

            writer = new Mock<TextWriter>();
            responseFactory = new Mock<IInsureShipResponseFactory>();

            request = new Mock<InsureShipRequestBase>(responseFactory.Object, shipment, new InsureShipAffiliate("test", "test"), log.Object);

            testObject = new InsureShipmentResponse(request.Object, log.Object);
        }

        [TestMethod]        
        public void Process_UsesRawResponse_FromRequest_Test()
        {
            HttpResponse rawResponse = new HttpResponse(writer.Object) { StatusCode = 204 };
            request.Setup(r => r.RawResponse).Returns(rawResponse);

            testObject.Process();

            request.Verify(r => r.RawResponse, Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(InsureShipResponseException))]
        public void Process_ThrowsInsureShipResponseException_WhenStatusCodeIsNotRecognized_Test()
        {
            HttpResponse rawResponse = new HttpResponse(writer.Object) { StatusCode = 900 };
            request.Setup(r => r.RawResponse).Returns(rawResponse);

            testObject.Process();
        }

        [TestMethod]        
        public void Process_LogsMessage_WhenStatusCodeIsNotRecognized_Test()
        {
            HttpResponse rawResponse = new HttpResponse(writer.Object) { StatusCode = 900 };
            request.Setup(r => r.RawResponse).Returns(rawResponse);

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
            HttpResponse rawResponse = new HttpResponse(writer.Object) { StatusCode = 419 };
            request.Setup(r => r.RawResponse).Returns(rawResponse);

            testObject.Process();
        }

        [TestMethod]
        public void Process_LogsMessage_WhenStatusCodeIsRecongized_ButNotSuccessful_Test()
        {
            HttpResponse rawResponse = new HttpResponse(writer.Object) { StatusCode = 419 };
            request.Setup(r => r.RawResponse).Returns(rawResponse);

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
            request.Setup(r => r.RawResponse).Returns(rawResponse);

            testObject.Process();
        }
    }
}
