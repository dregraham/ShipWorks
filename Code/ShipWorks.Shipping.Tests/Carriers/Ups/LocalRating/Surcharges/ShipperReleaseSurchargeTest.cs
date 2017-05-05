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
    public class ShipperReleaseSurchargeTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly ShipperReleaseSurcharge testObject;

        public ShipperReleaseSurchargeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = new ShipperReleaseSurcharge(
                new Dictionary<UpsSurchargeType, double> {{UpsSurchargeType.ShipperRelease, 123}});
        }

        [Fact]
        public void Apply_AddsShipperReleaseAmountToServiceRate_WhenShipmentHasShipperRelease()
        {
            UpsShipmentEntity shipment = new UpsShipmentEntity(){ShipperRelease = true, Packages = { new UpsPackageEntity()}};
            Mock<IUpsLocalServiceRate> serviceRate = mock.Mock<IUpsLocalServiceRate>();

            testObject.Apply(shipment, serviceRate.Object);

            serviceRate.Verify(r => r.AddAmount(123, EnumHelper.GetDescription(UpsSurchargeType.ShipperRelease)));
        }

        [Fact]
        public void Apply_AddsShipperReleaseAmountToServiceRate_WhenShipmentHasShipperReleaseAndMultiPackage()
        {
            UpsShipmentEntity shipment = new UpsShipmentEntity() { ShipperRelease = true, Packages = { new UpsPackageEntity(), new UpsPackageEntity() } };
            Mock<IUpsLocalServiceRate> serviceRate = mock.Mock<IUpsLocalServiceRate>();

            testObject.Apply(shipment, serviceRate.Object);

            serviceRate.Verify(r => r.AddAmount(123 * 2, EnumHelper.GetDescription(UpsSurchargeType.ShipperRelease)));
        }

        [Fact]
        public void Apply_DoesNotAddShipperReleaseAmountToServiceRate_WhenShipmentDoesNotHaveShipperRelease()
        {
            UpsShipmentEntity shipment = new UpsShipmentEntity() { ShipperRelease = false };
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