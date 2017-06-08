using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Shipping.Carriers.Ups.LocalRating.Surcharges;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating.Surcharges
{
    public class FuelGroundSurchargeTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly FuelGroundSurcharge testObject;
        
        public FuelGroundSurchargeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = new FuelGroundSurcharge(new Dictionary<UpsSurchargeType, double> { [UpsSurchargeType.FuelGround] = .0525 });
        }

        [Fact]
        public void Apply_FuelGroundSurchargeIsAppliedToServiceRate_WhenRateIsGround()
        {
            var serviceRate = mock.Mock<IUpsLocalServiceRate>();
            serviceRate.Setup(r => r.Service).Returns(UpsServiceType.UpsGround);
            serviceRate.Setup(r => r.Amount).Returns(200);

            testObject.Apply(null, serviceRate.Object);

            serviceRate.Verify(s => s.AddAmount(10.5M, "Ground Fuel Surcharge of 5.25%"));
        }

        [Fact]
        public void Apply_FuelGroundSurchargeIsNotAppliedToServiceRate_WhenRateIsNotGround()
        {
            var serviceRate = mock.Mock<IUpsLocalServiceRate>();
            serviceRate.SetupGet(s => s.Service).Returns(UpsServiceType.UpsNextDayAir);

            testObject.Apply(null, serviceRate.Object);

            serviceRate.Verify(s => s.AddAmount(It.IsAny<decimal>(), It.IsAny<string>()), Times.Never);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}