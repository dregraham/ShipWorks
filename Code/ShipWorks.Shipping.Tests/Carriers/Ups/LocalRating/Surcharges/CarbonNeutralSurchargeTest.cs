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
    public class CarbonNeutralSurchargeTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly CarbonNeutralSurcharge testObject;

        public CarbonNeutralSurchargeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = new CarbonNeutralSurcharge(
                new Dictionary<UpsSurchargeType, double>
                {
                    {UpsSurchargeType.CarbonNeutralAir, 123},
                    {UpsSurchargeType.CarbonNeutralGround, 999}
                });
        }

        [Fact]
        public void Apply_AddsCarbonNeutralAirAmountToServiceRate_WhenShipmentHasCarbonNeutralAndServiceIsAir()
        {
            UpsShipmentEntity shipment =
                new UpsShipmentEntity() {CarbonNeutral = true, Service = (int) UpsServiceType.UpsNextDayAir};
            Mock<IUpsLocalServiceRate> serviceRate = mock.Mock<IUpsLocalServiceRate>();

            testObject.Apply(shipment, serviceRate.Object);

            serviceRate.Verify(r => r.AddAmount(123, EnumHelper.GetDescription(UpsSurchargeType.CarbonNeutralAir)));
        }

        [Fact]
        public void Apply_AddsCarbonNeutralGroundAmountToServiceRate_WhenShipmentHasCarbonNeutralAndServiceIsGround()
        {
            UpsShipmentEntity shipment =
                new UpsShipmentEntity() { CarbonNeutral = true, Service = (int)UpsServiceType.UpsGround };
            Mock<IUpsLocalServiceRate> serviceRate = mock.Mock<IUpsLocalServiceRate>();

            testObject.Apply(shipment, serviceRate.Object);

            serviceRate.Verify(r => r.AddAmount(999, EnumHelper.GetDescription(UpsSurchargeType.CarbonNeutralGround)));
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}