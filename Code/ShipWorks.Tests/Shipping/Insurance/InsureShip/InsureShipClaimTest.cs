using System;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using log4net;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance.InsureShip;
using ShipWorks.Shipping.Insurance.InsureShip.Net.Claim;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Tests.Shipping.Insurance.InsureShip
{
    public class InsureShipClaimTest
    {
        private readonly AutoMock mock;
        private readonly ShipmentEntity shipment;

        public InsureShipClaimTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            mock.Mock<IInsureShipClaimValidator>()
                .Setup(x => x.IsShipmentEligibleToSubmitClaim(It.IsAny<InsureShipClaimType>(), AnyIShipment))
                .Returns(Result.FromSuccess());
            shipment = Create.Shipment()
                .Set(x => x.InsurancePolicy = Create.Entity<InsurancePolicyEntity>().Build())
                .Build();
        }

        [Fact]
        public void Submit_DelegatesToValidator()
        {
            var testObject = mock.Create<InsureShipClaim>();

            testObject.Submit(InsureShipClaimType.Damage, shipment, _ => { });

            mock.Mock<IInsureShipClaimValidator>()
                .Verify(x => x.IsShipmentEligibleToSubmitClaim(InsureShipClaimType.Damage, shipment));
        }

        [Fact]
        public void Submit_LogsError_WhenShipmentIsNotValidForClaim()
        {
            mock.Mock<IInsureShipClaimValidator>()
                .Setup(x => x.IsShipmentEligibleToSubmitClaim(It.IsAny<InsureShipClaimType>(), AnyIShipment))
                .Returns(Result.FromError("Foo"));
            var testObject = mock.Create<InsureShipClaim>();

            testObject.Submit(InsureShipClaimType.Damage, Create.Shipment().Build(), _ => { });

            mock.Mock<ILog>()
                .Verify(x => x.Error("An error occurred trying to submit a claim to InsureShip on shipment 0.", It.IsAny<Exception>()));
        }

        [Fact]
        public void Submit_ReturnsFailure_WhenShipmentIsNotValidForClaim()
        {
            mock.Mock<IInsureShipClaimValidator>()
                .Setup(x => x.IsShipmentEligibleToSubmitClaim(It.IsAny<InsureShipClaimType>(), AnyIShipment))
                .Returns(Result.FromError("Foo"));
            var testObject = mock.Create<InsureShipClaim>();

            var result = testObject.Submit(InsureShipClaimType.Damage, Create.Shipment().Build(), _ => { });

            Assert.False(result.Success);
        }

        [Fact]
        public void Submit_CallsUpdateMethod_WithClaimTypeSet()
        {
            var testObject = mock.Create<InsureShipClaim>();
            var called = false;

            var result = testObject.Submit(InsureShipClaimType.Damage, shipment,
                x =>
                {
                    Assert.Equal((int) InsureShipClaimType.Damage, x.ClaimType);
                    called = true;
                });

            Assert.True(called);
        }

        [Fact]
        public void Submit_LogsMessage_WhenShipmentIsValidForClaim()
        {
            var testObject = mock.Create<InsureShipClaim>();

            testObject.Submit(InsureShipClaimType.Damage, shipment, _ => { });

            mock.Mock<ILog>()
                .Verify(x => x.InfoFormat("Submitting claim to InsureShip for shipment {0}.", shipment.ShipmentID));
        }

        [Fact]
        public void Submit_DelegatesToClaimRequest_WhenShipmentIsValidForClaim()
        {
            var testObject = mock.Create<InsureShipClaim>();

            testObject.Submit(InsureShipClaimType.Damage, shipment, _ => { });

            mock.Mock<IInsureShipSubmitClaimRequest>()
                .Verify(x => x.CreateInsuranceClaim(shipment));
        }

        [Fact]
        public void Submit_LogsError_WhenClaimRequestFails()
        {
            mock.Mock<IInsureShipSubmitClaimRequest>()
                .Setup(x => x.CreateInsuranceClaim(AnyShipment))
                .Returns(Result.FromError("Foo"));
            var testObject = mock.Create<InsureShipClaim>();

            testObject.Submit(InsureShipClaimType.Damage, shipment, _ => { });

            mock.Mock<ILog>()
                .Verify(x => x.Error("An error occurred trying to submit a claim to InsureShip on shipment 0.", It.IsAny<Exception>()));
        }

        [Fact]
        public void Submit_ReturnsFailure_WhenClaimRequestFails()
        {
            mock.Mock<IInsureShipSubmitClaimRequest>()
                .Setup(x => x.CreateInsuranceClaim(AnyShipment))
                .Returns(Result.FromError("Foo"));
            var testObject = mock.Create<InsureShipClaim>();

            var result = testObject.Submit(InsureShipClaimType.Damage, shipment, _ => { });

            Assert.True(result.Failure);
        }

        [Fact]
        public void Submit_LogsInfo_WhenClaimRequestSucceeds()
        {
            mock.Mock<IInsureShipSubmitClaimRequest>()
                .Setup(x => x.CreateInsuranceClaim(AnyShipment))
                .Returns(Result.FromSuccess());
            var testObject = mock.Create<InsureShipClaim>();

            testObject.Submit(InsureShipClaimType.Damage, shipment, _ => { });

            mock.Mock<ILog>()
                .Verify(x => x.InfoFormat("Response code from InsureShip for claim submission on shipment {0} was successful.", shipment.ShipmentID));
        }

        [Fact]
        public void Submit_ReturnsSuccess_WhenClaimRequestSucceeds()
        {
            mock.Mock<IInsureShipSubmitClaimRequest>()
                .Setup(x => x.CreateInsuranceClaim(AnyShipment))
                .Returns(Result.FromSuccess());
            var testObject = mock.Create<InsureShipClaim>();

            var result = testObject.Submit(InsureShipClaimType.Damage, shipment, _ => { });

            Assert.True(result.Success);
        }
    }
}
