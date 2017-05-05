using System;
using System.Collections.Generic;
using System.Text;
using Autofac.Extras.Moq;
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
        readonly AutoMock mock;
        private readonly IDictionary<UpsSurchargeType, double> surcharges;
        private readonly FuelAirSurcharge testObject;
        
        public FuelAirSurchargeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            surcharges = new Dictionary<UpsSurchargeType, double> {[UpsSurchargeType.FuelAir] = .0525};
            testObject = new FuelAirSurcharge(surcharges);
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
            UpsLocalServiceRate serviceRate = new UpsLocalServiceRate(UpsServiceType.UpsNextDayAir, 100, true, null);
            testObject.Apply(null, serviceRate);

            var stringBuilder = new StringBuilder();
            serviceRate.Log(stringBuilder);
            Assert.Contains("Air Fuel Surcharge", stringBuilder.ToString());
        }

        public void Dispose()
        {
            mock.Dispose();
        }

    }
}