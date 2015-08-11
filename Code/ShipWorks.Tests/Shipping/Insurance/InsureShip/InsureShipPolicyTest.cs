using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Insurance.InsureShip.Net;
using log4net;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance.InsureShip;

namespace ShipWorks.Tests.Shipping.Insurance.InsureShip
{
    public class InsureShipPolicyTest
    {
        private InsureShipPolicy testObject;

        private Mock<IInsureShipSettings> settings;
        private Mock<IInsureShipRequestFactory> requestFactory;
        private Mock<IInsureShipResponseFactory> responseFactory;

        private Mock<InsureShipRequestBase> request;
        private Mock<IInsureShipResponse> response;
        
        private Mock<ILog> log;

        private ShipmentEntity shipment;
        private InsureShipAffiliate affiliate;
        private ShipmentEntity shipmentForVoiding;
       
        [TestInitialize]
        public void Initialize()
        {
            settings = new Mock<IInsureShipSettings>();
            settings.Setup(s => s.DistributorID).Returns("D00002");
            settings.Setup(s => s.Username).Returns("test2");
            settings.Setup(s => s.Password).Returns("password");
            settings.Setup(s => s.ApiUrl).Returns(new Uri("https://int.insureship.com/api/"));
            settings.Setup(s => s.VoidPolicyMaximumAge).Returns(new TimeSpan(0, 24, 0, 0));

            shipment = new ShipmentEntity(100031);
            affiliate = new InsureShipAffiliate("test", "test");

            log = new Mock<ILog>();
            log.Setup(l => l.Error(It.IsAny<object>(), It.IsAny<InsureShipResponseException>()));
            log.Setup(l => l.Error(It.IsAny<string>()));
            log.Setup(l => l.InfoFormat(It.IsAny<string>(), It.IsAny<int>()));
            log.Setup(l => l.InfoFormat(It.IsAny<string>(), It.IsAny<string>()));
            log.Setup(l => l.InfoFormat(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<double>()));
            
            responseFactory = new Mock<IInsureShipResponseFactory>();
            response = new Mock<IInsureShipResponse>();

            request = new Mock<InsureShipRequestBase>(responseFactory.Object, shipment, affiliate, settings.Object, log.Object, "InsureShipment");
            request.Setup(r => r.Submit()).Returns(response.Object);

            requestFactory = new Mock<IInsureShipRequestFactory>();
            requestFactory.Setup(f => f.CreateInsureShipmentRequest(It.IsAny<ShipmentEntity>(), It.IsAny<InsureShipAffiliate>())).Returns(request.Object);
            requestFactory.Setup(f => f.CreateVoidPolicyRequest(It.IsAny<ShipmentEntity>(), It.IsAny<InsureShipAffiliate>())).Returns(request.Object);

            response.Setup(r => r.Process()).Returns(InsureShipResponseCode.Success);

            shipmentForVoiding = new ShipmentEntity(1031)
            {
                Processed = true,
                ProcessedDate = DateTime.UtcNow,
                ShipDate = DateTime.UtcNow,
                InsurancePolicy = new InsurancePolicyEntity
                {
                    CreatedWithApi = true
                }
            };
            
            testObject = new InsureShipPolicy(affiliate, requestFactory.Object, log.Object, settings.Object);
        }

        [Fact]
        public void Insure_LogsMessge_WhenSubmittingShipmentInformation_Test()
        {
            testObject.Insure(shipment);

            log.Verify(l => l.InfoFormat("Submitting shipment information to InsureShip for shipment {0}.", shipment.ShipmentID), Times.Once());
        }

        [Fact]
        public void Insure_DelegatesToRequestFactory_Test()
        {
            testObject.Insure(shipment);

            requestFactory.Verify(f => f.CreateInsureShipmentRequest(shipment, affiliate), Times.Once());
        }

        [Fact]
        public void Insure_DelegatesToRequest_Test()
        {
            testObject.Insure(shipment);

            request.Verify(r => r.Submit(), Times.Once());
        }

        [Fact]
        public void Insure_LogsMessage_WhenProcessingResponse_Test()
        {
            testObject.Insure(shipment);

            log.Verify(l => l.InfoFormat("Processing response from InsureShip for shipment {0}.", shipment.ShipmentID), Times.Once());
        }

        [Fact]
        public void Insure_DelegatesToResponse_Test()
        {
            testObject.Insure(shipment);

            response.Verify(r => r.Process(), Times.Once());
        }

        [Fact]
        public void Insure_LogsMessage_WhenPolicyIsInsuredSuccessfully_Test()
        {
            testObject.Insure(shipment);

            // Response was setup to simulate a successful response from InsureShip
            log.Verify(l => l.InfoFormat("Response code from InsureShip was {0} successful (response code {1}).", string.Empty, InsureShipResponseCode.Success), Times.Once());
        }

        [Fact]
        public void Insure_LogsMessage_WhenPolicyIsNotInsuredSuccessfully_Test()
        {
            response.Setup(r => r.Process()).Returns(InsureShipResponseCode.Conflict);

            testObject.Insure(shipment);

            log.Verify(l => l.InfoFormat("Response code from InsureShip was {0} successful (response code {1}).", "not ", InsureShipResponseCode.Conflict), Times.Once());
        }

        [Fact]
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

        
        [Fact]
        public void Void_DoesNotMakeRequest_WhenShipmentIsNotProcessed_Test()
        {
            shipmentForVoiding.Processed = false;

            testObject.Void(shipmentForVoiding);

            requestFactory.Verify(f => f.CreateVoidPolicyRequest(It.IsAny<ShipmentEntity>(), It.IsAny<InsureShipAffiliate>()), Times.Never());
            request.Verify(r => r.Submit(), Times.Never());
        }

        [Fact]
        public void Void_LogsMessage_WhenShipmentIsNotProcessed_Test()
        {
            shipmentForVoiding.Processed = false;

            testObject.Void(shipmentForVoiding);

            log.Verify(l => l.InfoFormat("Shipment {0} was not insured with the API.", shipmentForVoiding.ShipmentID), Times.Once());
        }

        [Fact]
        public void Void_DoesNotMakeRequest_WhenShipmentFailedToInsureWithApi_Test()
        {
            shipmentForVoiding.InsurancePolicy.CreatedWithApi = false;

            testObject.Void(shipmentForVoiding);

            requestFactory.Verify(f => f.CreateVoidPolicyRequest(It.IsAny<ShipmentEntity>(), It.IsAny<InsureShipAffiliate>()), Times.Never());
            request.Verify(r => r.Submit(), Times.Never());
        }

        [Fact]
        public void Void_LogsMessage_WhenShipmentFailedToInsureWithApi_Test()
        {
            shipmentForVoiding.InsurancePolicy.CreatedWithApi = false;

            testObject.Void(shipmentForVoiding);

            log.Verify(l => l.InfoFormat("Shipment {0} was not insured with the API.", shipmentForVoiding.ShipmentID), Times.Once());
        }

        [Fact]
        public void Void_DoesNotMakeRequest_WhenShipmentWasNotInsuredWithApi_Test()
        {
            shipmentForVoiding.InsurancePolicy = null;

            testObject.Void(shipmentForVoiding);

            requestFactory.Verify(f => f.CreateVoidPolicyRequest(It.IsAny<ShipmentEntity>(), It.IsAny<InsureShipAffiliate>()), Times.Never());
            request.Verify(r => r.Submit(), Times.Never());
        }

        [Fact]
        public void Void_LogsMessage_WhenShipmentWasNotInsuredWithApi_Test()
        {
            shipmentForVoiding.InsurancePolicy = null;

            testObject.Void(shipmentForVoiding);

            log.Verify(l => l.InfoFormat("Shipment {0} was not insured with the API.", shipmentForVoiding.ShipmentID), Times.Once());
        }

        [Fact]
        [ExpectedException(typeof(InsureShipException))]
        public void Void_DoesNotMakeRequest_WhenPolicyAgeExceedsGracePeriodForVoiding_Test()
        {
            // Grace period set to 24 hours in the initialize method above
            shipmentForVoiding.ShipDate = DateTime.UtcNow.Subtract(new TimeSpan(0, 24, 1, 0));

            testObject.Void(shipmentForVoiding);

            requestFactory.Verify(f => f.CreateVoidPolicyRequest(It.IsAny<ShipmentEntity>(), It.IsAny<InsureShipAffiliate>()), Times.Never());
            request.Verify(r => r.Submit(), Times.Never());
        }

        [Fact]
        [ExpectedException(typeof(InsureShipException))]
        public void Void_LogsMessage_WhenPolicyAgeExceedsGracePeriodForVoiding_Test()
        {
            // Grace period set to 24 hours in the initialize method above
            shipmentForVoiding.ShipDate = DateTime.UtcNow.Subtract(new TimeSpan(0, 24, 1, 0));

            testObject.Void(shipmentForVoiding);

            log.Verify(l => l.InfoFormat("The policy for shipment {0} cannot be voided with the InsureShip API. The policy was created more than {1} hours ago.", shipmentForVoiding.ShipmentID, It.IsAny<double>()), Times.Once());
        }

        [Fact]
        [ExpectedException(typeof(InsureShipException))]
        public void Void_UsesInsureShipSettings_ToDetermineEligibility_Test()
        {
            // Grace period set to 24 hours in the initialize method above
            shipmentForVoiding.ShipDate = DateTime.UtcNow.Subtract(new TimeSpan(0, 24, 1, 0));

            testObject.Void(shipmentForVoiding);

            settings.Verify(s => s.VoidPolicyMaximumAge, Times.Once());
        }

        [Fact]
        public void Void_LogsMessage_WhenShipmentIsEligibleForVoiding_Test()
        {
            testObject.Void(shipmentForVoiding);

            log.Verify(l => l.InfoFormat("The policy for shipment {0} is eligible for voiding with the InsureShip API.", shipmentForVoiding.ShipmentID), Times.Once());
        }

        [Fact]
        public void Void_DelegatesToRequestFactory_WhenShipmentIsEligibleForVoiding_Test()
        {
            testObject.Void(shipmentForVoiding);

            requestFactory.Verify(f => f.CreateVoidPolicyRequest(It.IsAny<ShipmentEntity>(), It.IsAny<InsureShipAffiliate>()), Times.Once());
        }

        [Fact]
        public void Void_DelegatesToRequest_WhenShipmentIsEligibleForVoiding_Test()
        {
            testObject.Void(shipmentForVoiding);

            request.Verify(r => r.Submit(), Times.Once());
        }

        [Fact]
        public void Void_LogsMessage_WhenSubmittingRequest_Test()
        {
            testObject.Void(shipmentForVoiding);

            log.Verify(l => l.InfoFormat("Submitting void request to InsureShip for shipment {0}.", shipmentForVoiding.ShipmentID), Times.Once());
        }

        [Fact]
        public void Void_LogsMessage_WhenProcessingResponse_Test()
        {
            testObject.Void(shipmentForVoiding);

            log.Verify(l => l.InfoFormat("Processing void response for shipment {0}.", shipmentForVoiding.ShipmentID), Times.Once());
        }

        [Fact]
        public void Void_DelegatesToResponse_WhenShipmentIsEligibleForVoiding_Test()
        {
            testObject.Void(shipmentForVoiding);

            response.Verify(r => r.Process(), Times.Once());
        }
        
        [Fact]
        public void Void_LogsMessage_WhenPolicyIsVoidedSuccessfully_Test()
        {
            testObject.Void(shipmentForVoiding);

            // Response was setup to simulate a successful response from InsureShip
            log.Verify(l => l.InfoFormat("Response code from InsureShip was {0} successful (response code {1}).", string.Empty, InsureShipResponseCode.Success), Times.Once());
        }

        [Fact]
        public void Void_LogsMessage_WhenPolicyIsNotVoidedSuccessfully_Test()
        {
            response.Setup(r => r.Process()).Returns(InsureShipResponseCode.Conflict);

            testObject.Void(shipmentForVoiding);

            log.Verify(l => l.InfoFormat("Response code from InsureShip was {0} successful (response code {1}).", "not ", InsureShipResponseCode.Conflict), Times.Once());
        }

        [Fact]
        [ExpectedException(typeof(InsureShipException))]
        public void Void_ThrowsInsureShipException_WhenInsureShipResponseExceptionIsCaught_Test()
        {
            response.Setup(r => r.Process()).Throws(new InsureShipResponseException(InsureShipResponseCode.MissingRequiredParameter));

            testObject.Void(shipmentForVoiding);
        }

        [Fact]
        public void Void_LogsException_WhenInsureShipResponseExceptionIsCaught_Test()
        {
            response.Setup(r => r.Process()).Throws(new InsureShipResponseException(InsureShipResponseCode.MissingRequiredParameter));

            try
            {
                testObject.Void(shipmentForVoiding);
            }
            catch (InsureShipException)
            { }

            log.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<InsureShipResponseException>()));
        }
    }
}
