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
    public class FuelAirSurchargeTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly FuelAirSurcharge testObject;
        
        public FuelAirSurchargeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = new FuelAirSurcharge(new Dictionary<UpsSurchargeType, double> { [UpsSurchargeType.FuelAir] = .0525 });
        }

        [Fact]
        public void Apply_FuelAirSurchargeIsAppliedToServiceRate_WhenRateIsAir()
        {
            var serviceRate = mock.Mock<IUpsLocalServiceRate>();
            serviceRate.Setup(r => r.Service).Returns(UpsServiceType.Ups2DayAir);
            serviceRate.Setup(r => r.Amount).Returns(100);

            testObject.Apply(null, serviceRate.Object);

            serviceRate.Verify(s => s.AddAmount(5.25M, "Air Fuel Surcharge of 0.0525"));
        }

        [Fact]
        public void Apply_FuelAirSurchargeIsNotAppliedToServiceRate_WhenRateIsNotAir()
        {
            var serviceRate = mock.Mock<IUpsLocalServiceRate>();
            serviceRate.SetupGet(s => s.Service).Returns(UpsServiceType.UpsGround);

            testObject.Apply(null, serviceRate.Object);

            serviceRate.Verify(s => s.AddAmount(It.IsAny<decimal>(), It.IsAny<string>()), Times.Never);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}