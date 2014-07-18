using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Insurance.InsureShip.Net;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance.InsureShip;

namespace ShipWorks.Tests.Shipping.Insurance.InsureShip
{
    [TestClass]
    public class InsureShipPolicyTest
    {
        private InsureShipPolicy testObject;

        private Mock<IInsureShipSettings> settings = new Mock<IInsureShipSettings>();
        private Mock<IInsureShipRequestFactory> requestFactory;
        private Mock<IInsureShipResponseFactory> responseFactory;

        private Mock<InsureShipRequestBase> request;
        private Mock<IInsureShipResponse> response;
        
        private Mock<ILog> log;

        private ShipmentEntity shipment;
        private InsureShipAffiliate affiliate;

        [TestInitialize]
        public void Initialize()
        {
            settings.Setup(s => s.DistributorID).Returns("D00002");
            settings.Setup(s => s.Username).Returns("test2");
            settings.Setup(s => s.Password).Returns("password");
            settings.Setup(s => s.ApiUrl).Returns(new Uri("https://int.insureship.com/api/"));

            shipment = new ShipmentEntity(100031);
            affiliate = new InsureShipAffiliate("test", "test");

            log = new Mock<ILog>();
            log.Setup(l => l.Error(It.IsAny<object>(), It.IsAny<InsureShipResponseException>()));
            log.Setup(l => l.Error(It.IsAny<string>()));
            log.Setup(l => l.InfoFormat(It.IsAny<string>(), It.IsAny<int>()));
            log.Setup(l => l.InfoFormat(It.IsAny<string>(), It.IsAny<string>()));
            
            responseFactory = new Mock<IInsureShipResponseFactory>();
            response = new Mock<IInsureShipResponse>();

            request = new Mock<InsureShipRequestBase>(responseFactory.Object, shipment, affiliate, settings.Object, log.Object);
            request.Setup(r => r.Submit()).Returns(response.Object);

            requestFactory = new Mock<IInsureShipRequestFactory>();
            requestFactory.Setup(f => f.CreateInsureShipmentRequest(It.IsAny<ShipmentEntity>(), It.IsAny<InsureShipAffiliate>())).Returns(request.Object);

            response.Setup(r => r.Process()).Returns(InsureShipResponseCode.Success);


            testObject = new InsureShipPolicy(affiliate, requestFactory.Object, log.Object);
        }

        [TestMethod]
        public void Insure_LogsMessge_WhenSubmittingShipmentInformation_Test()
        {
            testObject.Insure(shipment);

            log.Verify(l => l.InfoFormat("Submitting shipment information to InsureShip for shipment {0}.", shipment.ShipmentID), Times.Once());
        }

        [TestMethod]
        public void Insure_DelegatesToRequestFactory_Test()
        {
            testObject.Insure(shipment);

            requestFactory.Verify(f => f.CreateInsureShipmentRequest(shipment, affiliate), Times.Once());
        }

        [TestMethod]
        public void Insure_DelegatesToRequest_Test()
        {
            testObject.Insure(shipment);

            request.Verify(r => r.Submit(), Times.Once());
        }

        [TestMethod]
        public void Insure_LogsMessage_WhenProcessingResponse_Test()
        {
            testObject.Insure(shipment);

            log.Verify(l => l.InfoFormat("Processing response from InsureShip for shipment {0}.", shipment.ShipmentID), Times.Once());
        }

        [TestMethod]
        public void Insure_DelegatesToResponse_Test()
        {
            testObject.Insure(shipment);

            response.Verify(r => r.Process(), Times.Once());
        }

        [TestMethod]
        public void Insure_LogsMessage_WhenPolicyIsInsuredSuccessfully_Test()
        {
            testObject.Insure(shipment);

            // Response was setup to simulate a successful response from InsureShip
            log.Verify(l => l.InfoFormat("Response code from InsureShip was {0}successful (response code {1}).", string.Empty, InsureShipResponseCode.Success), Times.Once());
        }

        [TestMethod]
        public void Insure_LogsMessage_WhenPolicyIsNotInsuredSuccessfully_Test()
        {
            response.Setup(r => r.Process()).Returns(InsureShipResponseCode.Conflict);

            testObject.Insure(shipment);

            log.Verify(l => l.InfoFormat("Response code from InsureShip was {0}successful (response code {1}).", "not ", InsureShipResponseCode.Conflict), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(InsureShipException))]
        public void Insure_ThrowsInsureShipException_WhenInsureShipResponseExceptionIsCaught_Test()
        {
            response.Setup(r => r.Process()).Throws(new InsureShipResponseException(InsureShipResponseCode.UnknownFailure));

            testObject.Insure(shipment);
        }

        [TestMethod]
        public void Insure_LogsMessage_WhenInsureShipResponseExceptionIsCaught_Test()
        {
            InsureShipResponseException responseException = new InsureShipResponseException(InsureShipResponseCode.UnknownFailure);
            response.Setup(r => r.Process()).Throws(responseException);

            try
            {
                testObject.Insure(shipment);
            }
            catch(InsureShipException)
            { }

            log.Verify(l => l.Error("An error occurred trying to insure shipment 100031 with InsureShip. A(n) UnknownFailure response code was received from InsureShip.", responseException), Times.Once());
        }
    }
}
