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
            UpsLocalServiceRate serviceRate = new UpsLocalServiceRate(UpsServiceType.UpsGround, 100, true, null);
            testObject.Apply(null, serviceRate);

            Assert.Equal(105.25M, serviceRate.Amount);
        }

        [Fact]
        public void Apply_FuelGroundSurchargeIsNotAppliedToServiceRate_WhenRateIsNotGround()
        {
            UpsLocalServiceRate serviceRate = new UpsLocalServiceRate(UpsServiceType.UpsNextDayAir, 100, true, null);
            testObject.Apply(null, serviceRate);

            Assert.Equal(100M, serviceRate.Amount);
        }

        [Fact]
        public void Apply_CallsAddAmountWithGroundFuelSurchargeText()
        {
            var serviceRate = mock.Mock<IUpsLocalServiceRate>();
            serviceRate.Setup(r => r.Service).Returns(UpsServiceType.UpsGround);
            serviceRate.Setup(r => r.Amount).Returns(100);

            testObject.Apply(null, serviceRate.Object);

            serviceRate.Verify(r => r.AddAmount(5.25M, "Ground Fuel Surcharge of 0.0525"), Times.Once);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}