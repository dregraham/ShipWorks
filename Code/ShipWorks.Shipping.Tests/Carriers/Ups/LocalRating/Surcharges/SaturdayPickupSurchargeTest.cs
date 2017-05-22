using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Shipping.Carriers.Ups.LocalRating.Surcharges;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating.Surcharges
{
    public class SaturdayPickupSurchargeTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly SaturdayPickupSurcharge testObject;

        public SaturdayPickupSurchargeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = new SaturdayPickupSurcharge(
                new Dictionary<UpsSurchargeType, double> {{UpsSurchargeType.SaturdayPickup, 123}});
        }

        [Fact]
        public void Apply_AddsSaturdayPickupAmountToServiceRate_WhenShipmentHasSaturdayPickup()
        {
            UpsShipmentEntity shipment = new UpsShipmentEntity
            {
                Shipment = new ShipmentEntity() {ShipDate = new DateTime(2017, 5, 13)},
                Packages = {new UpsPackageEntity()}
            };
            Mock<IUpsLocalServiceRate> serviceRate = mock.Mock<IUpsLocalServiceRate>();

            testObject.Apply(shipment, serviceRate.Object);

            serviceRate.Verify(r => r.AddAmount(123, EnumHelper.GetDescription(UpsSurchargeType.SaturdayPickup)));
        }

        [Fact]
        public void Apply_AddsSaturdayPickupAmountToServiceRate_WhenShipmentHasSaturdayPickupAndMultiPackage()
        {
            UpsShipmentEntity shipment = new UpsShipmentEntity
            {
                Shipment = new ShipmentEntity() {ShipDate = new DateTime(2017, 5, 13)},
                Packages = {new UpsPackageEntity(), new UpsPackageEntity()}
            };
            Mock<IUpsLocalServiceRate> serviceRate = mock.Mock<IUpsLocalServiceRate>();

            testObject.Apply(shipment, serviceRate.Object);

            serviceRate.Verify(r => r.AddAmount(123 * 2, EnumHelper.GetDescription(UpsSurchargeType.SaturdayPickup)));
        }

        [Fact]
        public void Apply_DoesNotAddSaturdayPickupAmountToServiceRate_WhenShipmentDoesNotHaveSaturdayPickup()
        {
            UpsShipmentEntity shipment =
                new UpsShipmentEntity() {Shipment = new ShipmentEntity() {ShipDate = new DateTime(2017, 5, 12)}};
            Mock<IUpsLocalServiceRate> serviceRate = mock.Mock<IUpsLocalServiceRate>();

            testObject.Apply(shipment, serviceRate.Object);

            serviceRate.Verify(r => r.AddAmount(It.IsAny<decimal>(), It.IsAny<string>()), Times.Never);
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}