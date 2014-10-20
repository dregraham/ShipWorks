﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance.InsureShip;
using ShipWorks.Shipping.Insurance.InsureShip.Net;
using ShipWorks.Shipping.Insurance.InsureShip.Net.Claim;
using log4net;

namespace ShipWorks.Tests.Shipping.Insurance.InsureShip.Net.Claim
{
    [TestClass]
    public class InsureShipClaimStatusResponseTest
    {
        private InsureShipClaimStatusResponse testObject;

        private Mock<IInsureShipSettings> settings;
        private Mock<InsureShipRequestBase> request;

        private Mock<IInsureShipResponseFactory> responseFactory;

        private Mock<ILog> log;

        private ShipmentEntity shipment;

        [TestInitialize]
        public void Initialize()
        {
            shipment = new ShipmentEntity(100031)
            {
                InsurancePolicy = new InsurancePolicyEntity()
            };

            settings = new Mock<IInsureShipSettings>();
            settings.Setup(s => s.UseTestServer).Returns(true);
            settings.Setup(s => s.DistributorID).Returns("D00002");
            settings.Setup(s => s.Username).Returns("test2");
            settings.Setup(s => s.Password).Returns("password");
            settings.Setup(s => s.ApiUrl).Returns(new Uri("https://int.insureship.com/api/"));

            log = new Mock<ILog>();
            log.Setup(l => l.Error(It.IsAny<object>()));

            responseFactory = new Mock<IInsureShipResponseFactory>();

            // Setup the simulated response from the request
            string simulatedResponseBody = "{\"status\":\"Created\"}";

            request = new Mock<InsureShipRequestBase>(responseFactory.Object, shipment, new InsureShipAffiliate("test", "test"), settings.Object, log.Object, "Sample");
            request.Setup(r => r.ResponseContent).Returns(simulatedResponseBody);
            request.Setup(r => r.ResponseStatusCode).Returns(HttpStatusCode.OK);

            testObject = new InsureShipClaimStatusResponse(request.Object, log.Object);
        }

        [TestMethod]
        public void Process_UsesRawResponse_FromRequest_Test()
        {
            testObject.Process();

            request.Verify(r => r.ResponseContent, Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(InsureShipResponseException))]
        public void Process_ThrowsInsureShipResponseException_WhenStatusCodeIsNotExpected_Test()
        {
            request.Setup(r => r.ResponseStatusCode).Returns(HttpStatusCode.Found);

            testObject.Process();
        }

        [TestMethod]
        public void Process_LogsMessage_WhenStatusCodeIsNotRecognized_Test()
        {
            request.Setup(r => r.ResponseStatusCode).Returns(HttpStatusCode.Found);

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
            request.Setup(r => r.ResponseStatusCode).Returns(HttpStatusCode.Conflict);

            testObject.Process();
        }

        [TestMethod]
        [ExpectedException(typeof(InsureShipResponseException))]
        public void Process_ThrowsInsureShipResponseException_WhenStatusCodeIs419_ButNotSuccessful_Test()
        {
            // Called out specifically since there is not an HttpStatusCode entry for 419
            request.Setup(r => r.ResponseStatusCode).Returns((HttpStatusCode)419);

            testObject.Process();
        }

        [TestMethod]
        public void Process_LogsMessage_WhenStatusCodeIs419_ButNotSuccessful_Test()
        {
            // Called out specifically since there is not an HttpStatusCode entry for 419
            request.Setup(r => r.ResponseStatusCode).Returns((HttpStatusCode)419);

            try
            {
                testObject.Process();
            }
            catch (InsureShipResponseException)
            { }

            log.Verify(l => l.Error("An error occurred trying to check the claim status for shipment 100031 with the InsureShip API: 419"));
        }

        [TestMethod]
        public void Process_LogsMessage_WhenStatusCodeIsRecongized_ButNotSuccessful_Test()
        {
            request.Setup(r => r.ResponseStatusCode).Returns(HttpStatusCode.Conflict);

            try
            {
                testObject.Process();
            }
            catch (InsureShipResponseException)
            { }

            log.Verify(l => l.Error("An error occurred trying to check the claim status for shipment 100031 with the InsureShip API: 409"));
        }

        [TestMethod]
        public void Process_SuccessfulResponse_Test()
        {
            // Response code of 200 is success
            request.Setup(r => r.ResponseStatusCode).Returns(HttpStatusCode.OK);

            testObject.Process();
        }

        [TestMethod]
        public void Process_SetsClaimStatus_WhenResponseIsSuccesful_Test()
        {
            testObject.Process();

            Assert.AreEqual("Created", shipment.InsurancePolicy.ClaimStatus);
        }

        [TestMethod]
        public void Process_ReturnsNoStatusInformation_WhenStatusIsMissing_Test()
        {
            // Setup the simulated response from the request
            const string simulatedResponseBody = "{\"NotTheStatus\":\"\"}";
            request.Setup(r => r.ResponseContent).Returns(simulatedResponseBody);

            testObject.Process();

            Assert.AreEqual("No status information available.", shipment.InsurancePolicy.ClaimStatus);
        }


        [TestMethod]
        public void Process_LogsMessage_WhenStatusIsMissing_Test()
        {
            // Setup the simulated response from the request
            const string simulatedResponseBody = "{\"NotTheStatus\":\"\"}";
            request.Setup(r => r.ResponseContent).Returns(simulatedResponseBody);

            testObject.Process();

            log.Verify(l => l.Error("The claim status was not found in the InsureShip response. Check the request/response logs for more details."), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(InsureShipException))]
        public void Process_ThrowsInsureShipException_WhenResponseContentIsEmpty_Test()
        {
            // Setup the simulated response from the request
            request.Setup(r => r.ResponseContent).Returns(string.Empty);

            testObject.Process();
        }

        [TestMethod]
        [ExpectedException(typeof(InsureShipException))]
        public void Process_ThrowsInsureShipException_WhenResponseStreamIsNull_Test()
        {
            request.Setup(r => r.ResponseContent).Returns((string)null);

            testObject.Process();
        }
    }
}
