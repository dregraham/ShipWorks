using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Shipping.Carriers.Ups.LocalRating.Surcharges;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating.Surcharges
{
    public class LargePackageUpsSurchargeTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly IDictionary<UpsSurchargeType, double> surcharges;
        private readonly LargePackageUpsSurcharge testObject;

        public LargePackageUpsSurchargeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            surcharges = new Dictionary<UpsSurchargeType, double>()
            {
                {UpsSurchargeType.LargePackage, 42}
            };

            testObject = new LargePackageUpsSurcharge(surcharges);
        }

        [Fact]
        public void Apply_AddsLargePackageSurcharge_WhenLargePackage()
        {
            var upsLocalServiceRate = new UpsLocalServiceRate(UpsServiceType.Ups2DayAir, 0, false, null);
            var shipment =
                Create.Shipment().AsUps(u => u.WithPackage(builder => builder.Set(p => p.DimsLength = 200))).Build();

            testObject.Apply(shipment.Ups, upsLocalServiceRate);
            Assert.Equal((decimal) surcharges[UpsSurchargeType.LargePackage], upsLocalServiceRate.Amount);
        }

        [Fact]
        public void Apply_DoesNotAddLargePackageSurcharge_WhenNotLargePackage()
        {
            var upsLocalServiceRate = new UpsLocalServiceRate(UpsServiceType.Ups2DayAir, 10, false, null);
            var shipment =
                Create.Shipment().AsUps(u => u.WithPackage(builder => builder.Set(p => p.DimsLength = 5))).Build();

            testObject.Apply(shipment.Ups, upsLocalServiceRate);

            Assert.Equal(10, upsLocalServiceRate.Amount);
        }

        public void Dispose()
        {
            mock.Dispose();
        }

    }
}