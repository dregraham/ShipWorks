using System;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance.InsureShip;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.Shipping.Insurance.InsureShip
{
    public class InsureShipClaimValidatorTest
    {
        private readonly AutoMock mock;

        public InsureShipClaimValidatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void IsShipmentEligibleToSubmitClaim_ReturnsFailure_WhenClaimHasAlreadyBeenMade()
        {
            var shipment = CreateShipment();
            shipment.InsurancePolicy.ClaimID = 1;
            var testObject = mock.Create<InsureShipClaimValidator>();

            var result = testObject.IsShipmentEligibleToSubmitClaim(InsureShipClaimType.Damage, shipment);

            result.Do(() => Assert.True(false))
                .OnFailure(ex => Assert.Equal("A claim has already been made for this shipment.", ex.Message));
        }

        [Fact]
        public void IsShipmentEligibleToSubmitClaim_ReturnsFailure_WhenShipmentIsNotProcessed()
        {
            var shipment = CreateShipment();
            shipment.Processed = false;
            var testObject = mock.Create<InsureShipClaimValidator>();

            var result = testObject.IsShipmentEligibleToSubmitClaim(InsureShipClaimType.Damage, shipment);

            Assert.False(result.Success);
        }

        [Fact]
        public void IsShipmentEligibleToSubmitClaim_LogsMessage_WhenShipmentIsNotProcessed()
        {
            var shipment = CreateShipment();
            shipment.Processed = false;
            var testObject = mock.Create<InsureShipClaimValidator>();

            var result = testObject.IsShipmentEligibleToSubmitClaim(InsureShipClaimType.Damage, shipment);

            mock.Mock<ILog>()
                .Verify(l => l.InfoFormat(
                    "Shipment {0} has not been processed. A claim cannot be submitted for an unprocessed shipment.", shipment.ShipmentID));
        }

        [Fact]
        public void IsShipmentEligibleToSubmitClaim_LogsMessage_WhenClaimTypeIsDamage()
        {
            var shipment = CreateShipment();
            var testObject = mock.Create<InsureShipClaimValidator>();

            var result = testObject.IsShipmentEligibleToSubmitClaim(InsureShipClaimType.Damage, shipment);

            mock.Mock<ILog>()
                .Verify(l => l.InfoFormat(
                    "Shipment {0} is eligible for submitting a claim to InsureShip.", shipment.ShipmentID));
        }

        [Fact]
        public void IsShipmentEligibleToSubmitClaim_ReturnsSuccess_WhenClaimTypeIsDamage()
        {
            var shipment = CreateShipment();
            var testObject = mock.Create<InsureShipClaimValidator>();

            var result = testObject.IsShipmentEligibleToSubmitClaim(InsureShipClaimType.Damage, shipment);

            Assert.True(result.Success);
        }

        [Theory]
        [InlineData(InsureShipClaimType.Lost)]
        [InlineData(InsureShipClaimType.Missing)]
        public void IsShipmentEligibleToSubmitClaim_LogsMessage_WhenClaimTypeIsNotDamageAndDateIsPastWaitingPeriod(InsureShipClaimType type)
        {
            mock.Mock<IInsureShipSettings>().Setup(x => x.ClaimSubmissionWaitingPeriod).Returns(TimeSpan.FromDays(1));
            mock.Mock<IDateTimeProvider>().Setup(x => x.Now).Returns(new DateTime(2018, 12, 20, 12, 0, 0));
            var shipment = CreateShipment();
            shipment.ShipDate = new DateTime(2018, 12, 18, 11, 30, 0);
            var testObject = mock.Create<InsureShipClaimValidator>();

            var result = testObject.IsShipmentEligibleToSubmitClaim(type, shipment);

            mock.Mock<ILog>()
                .Verify(l => l.InfoFormat(
                    "Shipment {0} is eligible for submitting a claim to InsureShip.", shipment.ShipmentID));
        }

        [Theory]
        [InlineData(InsureShipClaimType.Lost)]
        [InlineData(InsureShipClaimType.Missing)]
        public void IsShipmentEligibleToSubmitClaim_ReturnsSuccess_WhenClaimTypeIsNotDamageAndDateIsPastWaitingPeriod(InsureShipClaimType type)
        {
            mock.Mock<IInsureShipSettings>().Setup(x => x.ClaimSubmissionWaitingPeriod).Returns(TimeSpan.FromDays(1));
            mock.Mock<IDateTimeProvider>().Setup(x => x.Now).Returns(new DateTime(2018, 12, 20, 12, 0, 0));
            var shipment = CreateShipment();
            shipment.ShipDate = new DateTime(2018, 12, 18, 11, 30, 0);
            var testObject = mock.Create<InsureShipClaimValidator>();

            var result = testObject.IsShipmentEligibleToSubmitClaim(type, shipment);

            Assert.True(result.Success);
        }

        [Theory]
        [InlineData(InsureShipClaimType.Lost)]
        [InlineData(InsureShipClaimType.Missing)]
        public void IsShipmentEligibleToSubmitClaim_LogsMessage_WhenClaimTypeIsNotDamageAndDateIsNotPastWaitingPeriod(InsureShipClaimType type)
        {
            mock.Mock<IInsureShipSettings>().Setup(x => x.ClaimSubmissionWaitingPeriod).Returns(TimeSpan.FromDays(5));
            mock.Mock<IDateTimeProvider>().Setup(x => x.Now).Returns(new DateTime(2018, 12, 20, 12, 0, 0));
            var shipment = CreateShipment();
            shipment.ShipDate = new DateTime(2018, 12, 18, 11, 30, 0);
            var testObject = mock.Create<InsureShipClaimValidator>();

            var result = testObject.IsShipmentEligibleToSubmitClaim(type, shipment);

            mock.Mock<ILog>()
                .Verify(l => l.InfoFormat(
                    "A claim cannot be submitted for shipment {0}. It hasn't been {1} days since the ship date.", shipment.ShipmentID, (double) 5));
        }

        [Theory]
        [InlineData(InsureShipClaimType.Lost)]
        [InlineData(InsureShipClaimType.Missing)]
        public void IsShipmentEligibleToSubmitClaim_ReturnsFailure_WhenClaimTypeIsNotDamageAndDateIsNotPastWaitingPeriod(InsureShipClaimType type)
        {
            mock.Mock<IInsureShipSettings>().Setup(x => x.ClaimSubmissionWaitingPeriod).Returns(TimeSpan.FromDays(5));
            mock.Mock<IDateTimeProvider>().Setup(x => x.Now).Returns(new DateTime(2018, 12, 20, 12, 0, 0));
            var shipment = CreateShipment();
            shipment.ShipDate = new DateTime(2018, 12, 18, 11, 30, 0);
            var testObject = mock.Create<InsureShipClaimValidator>();

            var result = testObject.IsShipmentEligibleToSubmitClaim(type, shipment);

            Assert.True(result.Failure);
        }

        private static ShipmentEntity CreateShipment() =>
            new ShipmentEntity(100031)
            {
                Processed = true,
                ShipDate = DateTime.UtcNow.Subtract(TimeSpan.FromDays(8)),
                InsurancePolicy = new InsurancePolicyEntity()
                {
                    InsureShipPolicyID = 1234,
                    CreatedWithApi = true
                }
            };
    }
}
