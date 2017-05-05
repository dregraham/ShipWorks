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
    public class SaturdayDeliverySurchargeTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly SaturdayDeliverySurcharge testObject;

        public SaturdayDeliverySurchargeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = new SaturdayDeliverySurcharge(
                new Dictionary<UpsSurchargeType, double> {{UpsSurchargeType.SaturdayDelivery, 123}});
        }

        [Fact]
        public void Apply_AddsSaturdayDeliveryAmountToServiceRate_WhenShipmentHasSaturdayDelivery()
        {
            UpsShipmentEntity shipment = new UpsShipmentEntity(){SaturdayDelivery = true, Packages = { new UpsPackageEntity()}};
            Mock<IUpsLocalServiceRate> serviceRate = mock.Mock<IUpsLocalServiceRate>();

            testObject.Apply(shipment, serviceRate.Object);

            serviceRate.Verify(r => r.AddAmount(123, EnumHelper.GetDescription(UpsSurchargeType.SaturdayDelivery)));
        }

        [Fact]
        public void Apply_AddsSaturdayDeliveryAmountToServiceRate_WhenShipmentHasSaturdayDeliveryAndMultiPackage()
        {
            UpsShipmentEntity shipment = new UpsShipmentEntity() { SaturdayDelivery = true, Packages = { new UpsPackageEntity(), new UpsPackageEntity() } };
            Mock<IUpsLocalServiceRate> serviceRate = mock.Mock<IUpsLocalServiceRate>();

            testObject.Apply(shipment, serviceRate.Object);

            serviceRate.Verify(r => r.AddAmount(123 * 2, EnumHelper.GetDescription(UpsSurchargeType.SaturdayDelivery)));
        }

        [Fact]
        public void Apply_DoesNotAddSaturdayDeliveryAmountToServiceRate_WhenShipmentDoesNotHaveSaturdayDelivery()
        {
            UpsShipmentEntity shipment = new UpsShipmentEntity() { SaturdayDelivery = false };
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