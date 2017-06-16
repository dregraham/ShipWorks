using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Shipping.Carriers.Ups.LocalRating.Surcharges;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating.Surcharges
{
    public class DryIceSurchargeTest : IDisposable
    {
        readonly AutoMock mock;

        public DryIceSurchargeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Theory]
        [InlineData(1, true, UpsDryIceRegulationSet.Iata, false, 0, 5)]
        [InlineData(1, true, UpsDryIceRegulationSet.Cfr, false, 5.6, 5)]
        [InlineData(2, true, UpsDryIceRegulationSet.Iata, true, 0, 10)]
        public void Apply_CorrectSurchargeApplied(int numberOfPackages,
            bool enabled,
            UpsDryIceRegulationSet regulationSet,
            bool forMedical,
            double weight,
            decimal amount)
        {
            var package = new UpsPackageEntity()
            {
                DryIceEnabled = enabled,
                DryIceWeight = weight,
                DryIceIsForMedicalUse = forMedical,
                DryIceRegulationSet = (int) regulationSet
            };

            var shipment = new UpsShipmentEntity();
            shipment.Packages.AddRange(Enumerable.Repeat(package, numberOfPackages));

            var surcharges = new Dictionary<UpsSurchargeType, double>(){ { UpsSurchargeType.DryIce, 5} };

            var localServiceRate = mock.CreateMock<IUpsLocalServiceRate>();
            var testObject = new DryIceSurcharge(surcharges);
            
            testObject.Apply(shipment, localServiceRate.Object);

            localServiceRate.Verify(r=>r.AddAmount(amount, $"{numberOfPackages} Package(s) with dry ice"));
        }

        [Theory]
        [InlineData(false, UpsDryIceRegulationSet.Iata, false, 10)]
        [InlineData(false, UpsDryIceRegulationSet.Cfr, false, 10)]
        [InlineData(true, UpsDryIceRegulationSet.Cfr, false, 5.5)]
        [InlineData(true, UpsDryIceRegulationSet.Cfr, true, 10)]
        public void Apply_DoesNotApplySurcharge(bool enabled,
            UpsDryIceRegulationSet regulationSet,
            bool forMedical,
            double weight)
        {
            var package = new UpsPackageEntity()
            {
                DryIceEnabled = enabled,
                DryIceWeight = weight,
                DryIceIsForMedicalUse = forMedical,
                DryIceRegulationSet = (int) regulationSet
            };

            var shipment = new UpsShipmentEntity();
            shipment.Packages.Add(package);

            var surcharges = new Dictionary<UpsSurchargeType, double>() { { UpsSurchargeType.DryIce, 5 } };

            var localServiceRate = mock.CreateMock<IUpsLocalServiceRate>();
            var testObject = new DryIceSurcharge(surcharges);

            testObject.Apply(shipment, localServiceRate.Object);

            localServiceRate.Verify(r => r.AddAmount(It.IsAny<decimal>(), It.IsAny<string>()), Times.Never);
        }

        public void Dispose()
        {
            mock.Dispose();
        }

    }
}