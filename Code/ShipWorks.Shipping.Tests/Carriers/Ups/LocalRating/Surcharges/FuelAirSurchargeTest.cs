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
            UpsLocalServiceRate serviceRate = new UpsLocalServiceRate(UpsServiceType.UpsNextDayAir, 100, true, null);
            testObject.Apply(null, serviceRate);

            Assert.Equal(105.25M, serviceRate.Amount);
        }

        [Fact]
        public void Apply_FuelAirSurchargeIsNotAppliedToServiceRate_WhenRateIsNotAir()
        {
            UpsLocalServiceRate serviceRate = new UpsLocalServiceRate(UpsServiceType.UpsGround, 100, true, null);
            testObject.Apply(null, serviceRate);

            Assert.Equal(100M, serviceRate.Amount);
        }

        [Fact]
        public void Apply_CallsAddAmountWithAirFuelSurchargeText()
        {
            var serviceRate = mock.Mock<IUpsLocalServiceRate>();
            serviceRate.Setup(r => r.Service).Returns(UpsServiceType.Ups2DayAir);
            serviceRate.Setup(r => r.Amount).Returns(100);

            testObject.Apply(null, serviceRate.Object);

            serviceRate.Verify(r => r.AddAmount(5.25M, "Air Fuel Surcharge of 0.0525"), Times.Once);
        }

        public void Dispose()
        {
            mock.Dispose();
        }

    }
}