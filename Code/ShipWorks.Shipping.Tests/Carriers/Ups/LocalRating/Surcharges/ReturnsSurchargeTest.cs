using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
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
    public class ReturnsSurchargeTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly ReturnsSurcharge testObject;

        private static readonly Dictionary<UpsSurchargeType, double> surcharges =
            new Dictionary<UpsSurchargeType, double>
            {
                [UpsSurchargeType.UpsReturnsElectronicReturnLabel] = 2,
                [UpsSurchargeType.UpsReturnsPrintReturnLabel] = 4,
                [UpsSurchargeType.UpsReturnsPrintandMail] = 6,
                [UpsSurchargeType.UpsReturnsReturnsPlusOneAttempt] = 8,
                [UpsSurchargeType.UpsReturnsReturnsPlusThreeAttempts] = 10
            };
        
        public ReturnsSurchargeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = new ReturnsSurcharge(surcharges);
        }

        [Theory]
        [InlineData(UpsSurchargeType.UpsReturnsElectronicReturnLabel, UpsReturnServiceType.ElectronicReturnLabel)]
        [InlineData(UpsSurchargeType.UpsReturnsPrintReturnLabel, UpsReturnServiceType.PrintReturnLabel)]
        [InlineData(UpsSurchargeType.UpsReturnsPrintandMail, UpsReturnServiceType.PrintAndMail)]
        [InlineData(UpsSurchargeType.UpsReturnsReturnsPlusOneAttempt, UpsReturnServiceType.ReturnPlus1)]
        [InlineData(UpsSurchargeType.UpsReturnsReturnsPlusThreeAttempts, UpsReturnServiceType.ReturnPlus3)]
        public void Apply_AddsReturnSurchargeAmount_WhenShipmentIsReturn(UpsSurchargeType expectedSurcharge, UpsReturnServiceType upsReturnServiceType)
        {
            Mock<IUpsLocalServiceRate> rate = mock.Mock<IUpsLocalServiceRate>();
            UpsShipmentEntity shipment = new UpsShipmentEntity
            {
                Shipment = new ShipmentEntity {ReturnShipment = true},
                ReturnService = (int) upsReturnServiceType,
                Packages = {new UpsPackageEntity()}
            };

            testObject.Apply(shipment, rate.Object);
            rate.Verify(r => r.AddAmount((decimal) surcharges[expectedSurcharge],
                EnumHelper.GetDescription(expectedSurcharge)));
        }

        [Fact]
        public void Apply_DoesNotAddReturnSurchargeAmount_WhenShipmentIsNotReturn()
        {
            Mock<IUpsLocalServiceRate> rate = mock.Mock<IUpsLocalServiceRate>();
            UpsShipmentEntity shipment = new UpsShipmentEntity
            {
                Shipment = new ShipmentEntity { ReturnShipment = false },
                ReturnService = (int)UpsReturnServiceType.ElectronicReturnLabel,
                Packages = { new UpsPackageEntity() }
            };

            testObject.Apply(shipment, rate.Object);
            rate.Verify(r => r.AddAmount(It.IsAny<decimal>(), It.IsAny<string>()), Times.Never);
        }

        public void Dispose()
        {
            mock.Dispose();
        }

    }
}