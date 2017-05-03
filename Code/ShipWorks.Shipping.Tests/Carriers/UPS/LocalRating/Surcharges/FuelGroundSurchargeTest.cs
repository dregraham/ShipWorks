

using System;
using System.Collections.Generic;
using System.Text;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
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
        readonly AutoMock mock;
        private readonly IDictionary<UpsSurchargeType, double> surcharges;
        private readonly FuelGroundSurcharge testObject;
        
        public FuelGroundSurchargeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            surcharges = new Dictionary<UpsSurchargeType, double> {[UpsSurchargeType.FuelGround] = .0525};
            testObject = new FuelGroundSurcharge(surcharges);
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
            UpsLocalServiceRate serviceRate = new UpsLocalServiceRate(UpsServiceType.UpsGround, 100, true, null);
            testObject.Apply(null, serviceRate);

            var stringBuilder = new StringBuilder();
            serviceRate.Log(stringBuilder);
            Assert.Contains("Ground Fuel Surcharge", stringBuilder.ToString());
        }

        public void Dispose()
        {
            mock.Dispose();
        }

    }
}