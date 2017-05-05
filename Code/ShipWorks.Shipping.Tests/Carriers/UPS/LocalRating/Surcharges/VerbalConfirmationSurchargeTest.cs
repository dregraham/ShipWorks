using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Shipping.Carriers.Ups.LocalRating.Surcharges;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating.Surcharges
{
    public class VerbalConfirmationSurchargeTest : IDisposable
    {
        readonly AutoMock mock;

        public VerbalConfirmationSurchargeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Theory]
        [InlineData(1, 10)]
        [InlineData(10, 100)]
        public void Apply_CorrectSurchargeApplied(int applicablePackages, decimal amount)
        {
            var packageWithConfirmation = new UpsPackageEntity()
            {
                VerbalConfirmationEnabled = true
            };

            var packageWithoutConfirmation = new UpsPackageEntity();

            var shipment = new UpsShipmentEntity();
            shipment.Packages.AddRange(Enumerable.Repeat(packageWithoutConfirmation, 5));
            shipment.Packages.AddRange(Enumerable.Repeat(packageWithConfirmation, applicablePackages));
            shipment.Packages.AddRange(Enumerable.Repeat(packageWithoutConfirmation, 5));

            var surcharges = new Dictionary<UpsSurchargeType, double>() {{UpsSurchargeType.VerbalConfirmation, 10}};

            var localServiceRate = mock.CreateMock<IUpsLocalServiceRate>();
            var testObject = new VerbalConfirmationSurcharge(surcharges);

            testObject.Apply(shipment, localServiceRate.Object);

            localServiceRate.Verify(r => r.AddAmount(amount, $"{applicablePackages} Package(s) with Verbal Confirmation"));
        }

        [Fact]
        public void Apply_SurchargeNotApplied()
        {
            var packageWithoutConfirmation = new UpsPackageEntity();

            var shipment = new UpsShipmentEntity();
            shipment.Packages.AddRange(Enumerable.Repeat(packageWithoutConfirmation, 5));

            var surcharges = new Dictionary<UpsSurchargeType, double>() { { UpsSurchargeType.VerbalConfirmation, 10 } };

            var localServiceRate = mock.CreateMock<IUpsLocalServiceRate>();
            var testObject = new VerbalConfirmationSurcharge(surcharges);

            testObject.Apply(shipment, localServiceRate.Object);

            localServiceRate.Verify(r => r.AddAmount(It.IsAny<decimal>(), It.IsAny<string>()), Times.Never);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}