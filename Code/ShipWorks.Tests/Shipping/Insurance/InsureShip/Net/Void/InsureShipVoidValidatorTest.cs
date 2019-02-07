using System;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance.InsureShip;
using ShipWorks.Shipping.Insurance.InsureShip.Net.Void;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.Shipping.Insurance.InsureShip.Net.Void
{
    public class InsureShipVoidValidatorTest : IDisposable
    {
        private readonly AutoMock mock;

        public InsureShipVoidValidatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void IsVoidable_ReturnsSuccessButFalse_WhenShipmentIsNotProcessed()
        {
            var shipment = new ShipmentEntity { Processed = false, InsurancePolicy = new InsurancePolicyEntity { CreatedWithApi = true } };
            var testObject = mock.Create<InsureShipVoidValidator>();

            var result = testObject.IsVoidable(shipment);

            result
                .Do(x => Assert.False(x))
                .ThrowOnFailure();
        }

        [Fact]
        public void VoidInsurancePolicy_ReturnsSuccessButFalse_WhenPolicyIsNull()
        {
            var shipment = new ShipmentEntity { Processed = true, InsurancePolicy = null };
            var testObject = mock.Create<InsureShipVoidValidator>();

            var result = testObject.IsVoidable(shipment);

            result
                .Do(x => Assert.False(x))
                .ThrowOnFailure();
        }

        [Fact]
        public void IsVoidable_ReturnsSuccessButFalse_WhenPolicyWasNotCreatedWithApi()
        {
            var shipment = new ShipmentEntity { Processed = true, InsurancePolicy = new InsurancePolicyEntity { CreatedWithApi = false } };
            var testObject = mock.Create<InsureShipVoidValidator>();

            var result = testObject.IsVoidable(shipment);

            result
                .Do(x => Assert.False(x))
                .ThrowOnFailure();
        }

        [Fact]
        public void IsVoidable_ReturnsFailure_WhenPolicyIsOutsideOfGracePeriod()
        {
            var now = new DateTime(2018, 12, 12, 11, 30, 0);
            mock.Mock<IDateTimeProvider>().Setup(x => x.UtcNow).Returns(now);

            var shipDate = now.Subtract(InsureShipSettings.VoidPolicyMaximumAge).Subtract(TimeSpan.FromHours(1));
            var shipment = new ShipmentEntity { Processed = true, ShipDate = shipDate, InsurancePolicy = new InsurancePolicyEntity { CreatedWithApi = true } };
            var testObject = mock.Create<InsureShipVoidValidator>();

            var result = testObject.IsVoidable(shipment);

            result.Do(_ => Assert.True(false))
                .OnFailure(ex => Assert.IsAssignableFrom<InsureShipException>(ex));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
