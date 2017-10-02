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
                new UpsShipmentEntity() { CarbonNeutral = true, Packages = { new UpsPackageEntity() } };
            Mock<IUpsLocalServiceRate> serviceRate = mock.Mock<IUpsLocalServiceRate>();
            serviceRate.SetupGet(r => r.Service).Returns(UpsServiceType.UpsNextDayAir);

            testObject.Apply(shipment, serviceRate.Object);

            serviceRate.Verify(r => r.AddAmount(123, EnumHelper.GetDescription(UpsSurchargeType.CarbonNeutralAir)));
        }

        [Fact]
        public void Apply_AddsCarbonNeutralGroundAmountToServiceRate_WhenShipmentHasCarbonNeutralAndServiceIsGround()
        {
            UpsShipmentEntity shipment =
                new UpsShipmentEntity() { CarbonNeutral = true, Packages = { new UpsPackageEntity() } };

            Mock<IUpsLocalServiceRate> serviceRate = mock.Mock<IUpsLocalServiceRate>();
            serviceRate.SetupGet(r => r.Service).Returns(UpsServiceType.UpsGround);

            testObject.Apply(shipment, serviceRate.Object);

            serviceRate.Verify(r => r.AddAmount(999, EnumHelper.GetDescription(UpsSurchargeType.CarbonNeutralGround)));
        }

        [Fact]
        public void Apply_AddsCarbonNeutralGroundAmountToServiceRate_WhenShipmentHasCarbonNeutralAndServiceIsGroundAndMultiPackage()
        {
            UpsShipmentEntity shipment =
                new UpsShipmentEntity() { CarbonNeutral = true, Packages = { new UpsPackageEntity(), new UpsPackageEntity() } };

            Mock<IUpsLocalServiceRate> serviceRate = mock.Mock<IUpsLocalServiceRate>();
            serviceRate.SetupGet(r => r.Service).Returns(UpsServiceType.UpsGround);

            testObject.Apply(shipment, serviceRate.Object);

            serviceRate.Verify(r => r.AddAmount(999 * 2, EnumHelper.GetDescription(UpsSurchargeType.CarbonNeutralGround)));
        }

        [Fact]
        public void Apply_DoesNotAddsCarbonNeutralGroundAmountToServiceRate_WhenShipmentDoesNotHaveCarbonNeutral()
        {
            UpsShipmentEntity shipment =
                new UpsShipmentEntity() { CarbonNeutral = false, Packages = { new UpsPackageEntity(), new UpsPackageEntity() } };
            Mock<IUpsLocalServiceRate> serviceRate = mock.Mock<IUpsLocalServiceRate>();
            serviceRate.SetupGet(r => r.Service).Returns(UpsServiceType.UpsGround);


            testObject.Apply(shipment, serviceRate.Object);

            serviceRate.Verify(r => r.AddAmount(It.IsAny<decimal>(), It.IsAny<string>()), Times.Never);
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}