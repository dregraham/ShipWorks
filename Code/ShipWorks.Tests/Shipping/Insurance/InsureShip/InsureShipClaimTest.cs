using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance.InsureShip;
using ShipWorks.Shipping.Insurance.InsureShip.Net;
using log4net;
using Interapptive.Shared.Utility;
using System.Threading;

namespace ShipWorks.Tests.Shipping.Insurance.InsureShip
{
    [TestClass]
    public class InsureShipClaimTest
    {
        private InsureShipClaim testObject;

        private Mock<IInsureShipSettings> settings;
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
            settings = new Mock<IInsureShipSettings>();
            settings.Setup(s => s.DistributorID).Returns("D00002");
            settings.Setup(s => s.Username).Returns("test2");
            settings.Setup(s => s.Password).Returns("password");
            settings.Setup(s => s.ApiUrl).Returns(new Uri("https://int.insureship.com/api/"));
            settings.Setup(s => s.ClaimSubmissionWaitingPeriod).Returns(TimeSpan.FromDays(7));

            // Create a shipment that is eligible for submitting a claim
            shipment = new ShipmentEntity(100031)
            {
                Processed = true,
                ShipDate = DateTime.UtcNow.Subtract(TimeSpan.FromDays(8)),
                InsurancePolicy = new InsurancePolicyEntity()
                {
                    CreatedWithApi = true
                }
            };

            affiliate = new InsureShipAffiliate("test", "test");

            log = new Mock<ILog>();
            log.Setup(l => l.Error(It.IsAny<object>(), It.IsAny<InsureShipResponseException>()));
            log.Setup(l => l.Error(It.IsAny<string>()));
            log.Setup(l => l.InfoFormat(It.IsAny<string>(), It.IsAny<int>()));
            log.Setup(l => l.InfoFormat(It.IsAny<string>(), It.IsAny<string>()));
            log.Setup(l => l.InfoFormat(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            responseFactory = new Mock<IInsureShipResponseFactory>();
            response = new Mock<IInsureShipResponse>();

            request = new Mock<InsureShipRequestBase>(responseFactory.Object, shipment, affiliate, settings.Object, log.Object, "Sample");
            request.Setup(r => r.Submit()).Returns(response.Object);

            requestFactory = new Mock<IInsureShipRequestFactory>();
            requestFactory.Setup(f => f.CreateSubmitClaimRequest(It.IsAny<ShipmentEntity>(), It.IsAny<InsureShipAffiliate>())).Returns(request.Object);
            requestFactory.Setup(f => f.CreateClaimStatusRequest(It.IsAny<ShipmentEntity>(), It.IsAny<InsureShipAffiliate>())).Returns(request.Object);

            response.Setup(r => r.Process()).Returns(InsureShipResponseCode.Success);

            testObject = new InsureShipClaim(shipment, affiliate, requestFactory.Object, settings.Object, log.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(InsureShipException))]
        public void Submit_ThrowsInsureShipException_WhenClaimHasAlreadyBeenMade_Test()
        {
            shipment.InsurancePolicy.ClaimID = 1;

            testObject.Submit(InsureShipClaimType.Damage, "item 1", "desc", 1.00M, "email@shipworks.com");
        }

        [TestMethod]
        public void Submit_LogsMessage_WhenClaimHasAlreadyBeenMade_Test()
        {
            shipment.InsurancePolicy.ClaimID = 1;

            try
            {
                testObject.Submit(InsureShipClaimType.Damage, "item 1", "desc", 1.00M, "email@shipworks.com");
            }
            catch(InsureShipException)
            { }

            log.Verify(l => l.ErrorFormat("A claim has already been submitted for shipment {0}", shipment.ShipmentID), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(InsureShipException))]
        public void Submit_ThrowsInsureShipException_WhenShipmentIsProcessed_Test()
        {
            shipment.Processed = false;

            testObject.Submit(InsureShipClaimType.Damage, "item 1", "desc", 1.00M, "email@shipworks.com");
        }

        [TestMethod]
        public void Submit_LogsMessage_WhenShipmentIsProcessed_Test()
        {
            shipment.Processed = false;

            try
            {
                testObject.Submit(InsureShipClaimType.Damage, "item 1", "desc", 1.00M, "email@shipworks.com");
            }
            catch(InsureShipException)
            { }

            log.Verify(l => l.InfoFormat("Shipment {0} has not been processed. A claim cannot be submitted for an unprocessed shipment.", shipment.ShipmentID), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(InsureShipException))]
        public void Submit_ThrowsInsureShipException_WhenShipDateDoesNotExceedWaitPeriod_Test()
        {
            // Set the ship date to be a day short of the wait period
            shipment.ShipDate = DateTime.UtcNow.Subtract(settings.Object.ClaimSubmissionWaitingPeriod).AddDays(1);

            testObject.Submit(InsureShipClaimType.Missing, "item 1", "desc", 1.00M, "email@shipworks.com");
        }

        [TestMethod]
        public void Submit_DoesNotThrowInsureShipException_WhenShipDateDoesNotExceedWaitPeriod_Test()
        {
            // Set the ship date to be a day short of the wait period
            shipment.ShipDate = DateTime.UtcNow.Subtract(settings.Object.ClaimSubmissionWaitingPeriod).AddDays(1);

            testObject.Submit(InsureShipClaimType.Damage, "item 1", "desc", 1.00M, "email@shipworks.com");
        }

        [TestMethod]
        public void Submit_LogsMessage_WhenShipDateDoesNotExceedWaitPeriod_Test()
        {
            // Set the ship date to be a day short of the wait period
            shipment.ShipDate = DateTime.UtcNow.Subtract(settings.Object.ClaimSubmissionWaitingPeriod).AddDays(1);

            try
            {
                testObject.Submit(InsureShipClaimType.Lost, "item 1", "desc", 1.00M, "email@shipworks.com");
            }
            catch (InsureShipException)
            { }

            log.Verify(l => l.InfoFormat("A claim cannot be submitted for shipment {0}. It hasn't been {1} days since the ship date.", shipment.ShipmentID, settings.Object.ClaimSubmissionWaitingPeriod.TotalDays), Times.Once());
        }

        [TestMethod]
        public void Submit_LogsSuccessMessage_WhenClaimTypeIsDamageAndShipDateDoesNotExceedWaitPeriod_Test()
        {
            // Set the ship date to be a day short of the wait period
            shipment.ShipDate = DateTime.UtcNow.Subtract(settings.Object.ClaimSubmissionWaitingPeriod).AddDays(1);

            try
            {
                testObject.Submit(InsureShipClaimType.Damage, "item 1", "desc", 1.00M, "email@shipworks.com");
            }
            catch (InsureShipException)
            { }

            log.Verify(l => l.InfoFormat("Response code from InsureShip for claim submission on shipment {0} was {1} successful (response code {2}).",
                                   shipment.ShipmentID, string.Empty, EnumHelper.GetApiValue(InsureShipResponseCode.Success)), Times.Once());
        }

        [TestMethod]
        public void Submit_LogsMessage_WhenShipmentIsEligible_Test()
        {
            testObject.Submit(InsureShipClaimType.Damage, "item 1", "desc", 1.00M, "email@shipworks.com");

            log.Verify(l => l.InfoFormat("Shipment {0} is eligible for submitting a claim to InsureShip.", shipment.ShipmentID), Times.Once());
        }

        [TestMethod]
        public void Submit_SetsClaimType_WhenShipmentIsEligible_Test()
        {
            testObject.Submit(InsureShipClaimType.Damage, "item 1", "desc", 1.00M, "email@shipworks.com");

            Assert.AreEqual((int)InsureShipClaimType.Damage, shipment.InsurancePolicy.ClaimType);
        }

        [TestMethod]
        public void Submit_SetsItemName_WhenShipmentIsEligible_Test()
        {
            testObject.Submit(InsureShipClaimType.Damage, "item 1", "desc", 1.00M, "email@shipworks.com");

            Assert.AreEqual("item 1", shipment.InsurancePolicy.ItemName);
        }

        [TestMethod]
        public void Submit_SetsDamageValue_WhenShipmentIsEligible_Test()
        {
            testObject.Submit(InsureShipClaimType.Damage, "item 1", "desc", 1.00M, "email@shipworks.com");

            Assert.AreEqual(1.00M, shipment.InsurancePolicy.DamageValue);
        }

        [TestMethod]
        public void Submit_SetsSubmissionDate_WhenShipmentIsEligible_Test()
        {
            DateTime testBegin = DateTime.UtcNow;

            testObject.Submit(InsureShipClaimType.Damage, "item 1", "desc", 1.00M, "email@shipworks.com");
            
            DateTime testEnd = DateTime.UtcNow;


            // Make sure the recorded time the test was began and the time of the submission is positive;
            // Same for time recorded after the submission and the time of the submission
            TimeSpan beginToSubmission = shipment.InsurancePolicy.SubmissionDate.Value.Subtract(testBegin);
            TimeSpan endFromSubmission = testEnd.Subtract(shipment.InsurancePolicy.SubmissionDate.Value);

            Assert.IsTrue(beginToSubmission.TotalMilliseconds >= 0 && endFromSubmission.TotalMilliseconds >= 0);
        }

        [TestMethod]
        public void Submit_SetsEmail_WhenShipmentIsEligible_Test()
        {
            testObject.Submit(InsureShipClaimType.Damage, "item 1", "desc", 1.00M, "email@shipworks.com");

            Assert.AreEqual("email@shipworks.com", shipment.InsurancePolicy.EmailAddress);
        }

        [TestMethod]
        public void Submit_LogsMessage_WhenSubmittingRequest_Test()
        {
            testObject.Submit(InsureShipClaimType.Damage, "item 1", "desc", 1.00M, "email@shipworks.com");

            log.Verify(l => l.InfoFormat("Submitting claim to InsureShip for shipment {0}.", shipment.ShipmentID), Times.Once());
        }

        [TestMethod]
        public void Submit_DelegatesToRequestFactory_WhenShipmentIsEligible_Test()
        {
            testObject.Submit(InsureShipClaimType.Damage, "item 1", "desc", 1.00M, "email@shipworks.com");

            requestFactory.Verify(f => f.CreateSubmitClaimRequest(shipment, affiliate), Times.Once());
        }

        [TestMethod]
        public void Submit_DelegatesToRequest_WhenShipmentIsEligible_Test()
        {
            testObject.Submit(InsureShipClaimType.Damage, "item 1", "desc", 1.00M, "email@shipworks.com");

            request.Verify(r => r.Submit(), Times.Once());
        }

        [TestMethod]
        public void Submit_LogsMessage_WhenProcessingResponse_Test()
        {
            testObject.Submit(InsureShipClaimType.Damage, "item 1", "desc", 1.00M, "email@shipworks.com");

            log.Verify(l => l.InfoFormat("Processing claim response from InsureShip for shipment {0}.", shipment.ShipmentID), Times.Once());
        }

        [TestMethod]
        public void Submit_DelegatesToResponse_WhenShipmentIsEligible_Test()
        {
            testObject.Submit(InsureShipClaimType.Damage, "item 1", "desc", 1.00M, "email@shipworks.com");

            response.Verify(r => r.Process(), Times.Once());
        }

        [TestMethod]
        public void Submit_LogsMessage_WhenProcessingIsSuccessful_Test()
        {
            testObject.Submit(InsureShipClaimType.Damage, "item 1", "desc", 1.00M, "email@shipworks.com");

            log.Verify(l => l.InfoFormat("Response code from InsureShip for claim submission on shipment {0} was {1} successful (response code {2}).",
                                   shipment.ShipmentID, string.Empty, EnumHelper.GetApiValue(InsureShipResponseCode.Success)), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof (InsureShipException))]
        public void Submit_CatchesInsureShipResponseException_AndThrowsInsureShipException_Test()
        {
            InsureShipResponseException responseException = new InsureShipResponseException(InsureShipResponseCode.MissingRequiredParameter);
            response.Setup(r => r.Process()).Throws(responseException);

            testObject.Submit(InsureShipClaimType.Damage, "item 1", "desc", 1.00M, "email@shipworks.com");
        }

        [TestMethod]
        public void Submit_LogsErrorMessage_WhenInsureShipResponseExceptionIsCaught_Test()
        {
            InsureShipResponseException responseException = new InsureShipResponseException(InsureShipResponseCode.MissingRequiredParameter);
            response.Setup(r => r.Process()).Throws(responseException);

            try
            {
                testObject.Submit(InsureShipClaimType.Damage, "item 1", "desc", 1.00M, "email@shipworks.com");
            }
            catch(InsureShipException)
            { }

            log.Verify(l => l.Error("An error occurred trying to submit a claim to InsureShip on shipment 100031. A(n) MissingRequiredParameter response code was received from InsureShip.", responseException), Times.Once());
        }        

        [TestMethod]
        [ExpectedException(typeof(InsureShipException))]
        public void CheckStatus_ThrowsException_WhenClaimIDIsNull_Test()
        {
            shipment.InsurancePolicy.ClaimID = null;
            
            testObject.CheckStatus();
        }

        [TestMethod]
        public void CheckStatus_LogsMessage_WhenClaimIDIsNull_Test()
        {
            shipment.InsurancePolicy.ClaimID = null;

            try
            {
                testObject.CheckStatus();
            }
            catch (InsureShipException)
            { }

            log.Verify(l => l.Error("Unable to check claim status for shipment 100031. A claim has not been submitted yet."), Times.Once());
        }

        [TestMethod]
        public void CheckStatus_LogsMessage_WhenMakingRequest_Test()
        {
            shipment.InsurancePolicy.ClaimID = 939;

            testObject.CheckStatus();

            log.Verify(l => l.InfoFormat("Checking claim status with InsureShip for shipment {0}.", shipment.ShipmentID), Times.Once());
        }

        [TestMethod]
        public void CheckStatus_DelegatesToRequestFactory_Test()
        {
            shipment.InsurancePolicy.ClaimID = 939;

            testObject.CheckStatus();

            requestFactory.Verify(r => r.CreateClaimStatusRequest(shipment, affiliate), Times.Once());
        }

        [TestMethod]
        public void CheckStatus_DelegatesToClaimStatusRequest_Test()
        {
            shipment.InsurancePolicy.ClaimID = 939;

            testObject.CheckStatus();

            request.Verify(r => r.Submit(), Times.Once());
        }

        [TestMethod]
        public void CheckStatus_LogsMessage_WhenProcessingResponse_Test()
        {
            shipment.InsurancePolicy.ClaimID = 939;

            testObject.CheckStatus();

            log.Verify(l => l.InfoFormat("Processing claim status response from InsureShip for shipment {0}.", shipment.ShipmentID), Times.Once());
        }

        [TestMethod]
        public void CheckStatus_DelegatesToResponse_Test()
        {
            shipment.InsurancePolicy.ClaimID = 939;

            testObject.CheckStatus();

            response.Verify(r => r.Process(), Times.Once());
        }

        [TestMethod]
        public void CheckStatus_LogsMessage_WhenProcessingIsSuccessful_Test()
        {
            shipment.InsurancePolicy.ClaimID = 939;

            testObject.CheckStatus();

            log.Verify(l => l.InfoFormat("Response code from InsureShip for claim status on shipment {0} was {1} successful (response code {2}).",
                                   shipment.ShipmentID, string.Empty, EnumHelper.GetApiValue(InsureShipResponseCode.Success)), Times.Once());
        }

        [TestMethod]
        public void CheckStatus_LogsErrorMessage_WhenInsureShipResponseExceptionIsCaught_Test()
        {
            shipment.InsurancePolicy.ClaimID = 939;

            InsureShipResponseException responseException = new InsureShipResponseException(InsureShipResponseCode.MissingRequiredParameter);
            response.Setup(r => r.Process()).Throws(responseException);

            try
            {
                testObject.CheckStatus();
            }
            catch (InsureShipException)
            { }

            log.Verify(l => l.Error("An error occurred trying to check the claim status with InsureShip on shipment 100031. A(n) MissingRequiredParameter response code was received from InsureShip.", responseException), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(InsureShipException))]
        public void CheckStatus_CatchesInsureShipResponseException_AndThrowsInsureShipException_Test()
        {
            shipment.InsurancePolicy.ClaimID = 939;

            InsureShipResponseException responseException = new InsureShipResponseException(InsureShipResponseCode.MissingRequiredParameter);
            response.Setup(r => r.Process()).Throws(responseException);

            testObject.CheckStatus();
        }

        [TestMethod]
        public void CheckStatus_ReturnsClaimStatusOnPolicy_Test()
        {
            shipment.InsurancePolicy.ClaimID = 939;
            shipment.InsurancePolicy.ClaimStatus = "Created";

            string status = testObject.CheckStatus();

            Assert.AreEqual(shipment.InsurancePolicy.ClaimStatus, status);
        }
    }
}
