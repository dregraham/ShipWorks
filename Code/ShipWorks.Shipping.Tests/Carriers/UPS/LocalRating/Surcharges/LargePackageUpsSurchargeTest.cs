using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Shipping.Carriers.Ups.LocalRating.Surcharges;
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
            var upsLocalServiceRate = mock.CreateMock<IUpsLocalServiceRate>();
            var shipment =
                Create.Shipment().AsUps(u => u.WithPackage(builder => builder.Set(p => p.DimsLength = 200))).Build();

            testObject.Apply(shipment.Ups, upsLocalServiceRate.Object);

            upsLocalServiceRate.Verify(
                r => r.AddAmount((decimal) surcharges[UpsSurchargeType.LargePackage], "1 Large Package(s)"), Times.Once);
        }

        [Fact]
        public void Apply_DoesNotAddLargePackageSurcharge_WhenNotLargePackage()
        {
            var upsLocalServiceRate = mock.CreateMock<IUpsLocalServiceRate>();
            var shipment =
                Create.Shipment().AsUps(u => u.WithPackage(builder => builder.Set(p => p.DimsLength = 5))).Build();

            testObject.Apply(shipment.Ups, upsLocalServiceRate.Object);

            upsLocalServiceRate.Verify(r => r.AddAmount(It.IsAny<decimal>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void Apply_AddsMultipleLargePackages_WhenMultipleLargePackages()
        {
            var upsLocalServiceRate = mock.CreateMock<IUpsLocalServiceRate>();
            var shipment =
                Create.Shipment().AsUps(u => u
                .WithPackage(builder => builder.Set(p => p.DimsLength = 200))
                .WithPackage(builder => builder.Set(p => p.DimsLength = 200))).Build();

            testObject.Apply(shipment.Ups, upsLocalServiceRate.Object);

            var expectedSurcharge = ((decimal) surcharges[UpsSurchargeType.LargePackage]) * 2;

            upsLocalServiceRate.Verify(
                r => r.AddAmount(expectedSurcharge, "2 Large Package(s)"), Times.Once);
        }

        [Fact]
        public void Apply_AddsLargePackageSurcharge_WhenMultiplePackages_AndOneIsLarge()
        {
            var upsLocalServiceRate = mock.CreateMock<IUpsLocalServiceRate>();
            var shipment =
                Create.Shipment().AsUps(u => u
                .WithPackage(builder => builder.Set(p => p.DimsLength = 10))
                .WithPackage(builder => builder.Set(p => p.DimsLength = 200))).Build();

            testObject.Apply(shipment.Ups, upsLocalServiceRate.Object);

            upsLocalServiceRate.Verify(
                r => r.AddAmount((decimal) surcharges[UpsSurchargeType.LargePackage], "1 Large Package(s)"), Times.Once);
        }


        [Fact]
        public void Apply_DoesNotAddLargePackageSurcharge_WhenMultipleSmallPackages()
        {
            var upsLocalServiceRate = mock.CreateMock<IUpsLocalServiceRate>();
            var shipment =
                Create.Shipment().AsUps(u => u
                .WithPackage(builder => builder.Set(p => p.DimsLength = 10))
                .WithPackage(builder => builder.Set(p => p.DimsLength = 10))).Build();

            testObject.Apply(shipment.Ups, upsLocalServiceRate.Object);

            upsLocalServiceRate.Verify(r => r.AddAmount(It.IsAny<decimal>(), It.IsAny<string>()), Times.Never);
        }

        public void Dispose()
        {
            mock.Dispose();
        }

    }
}