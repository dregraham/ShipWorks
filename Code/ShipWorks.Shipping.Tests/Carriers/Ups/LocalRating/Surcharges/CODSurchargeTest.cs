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
    public class CODSurchargeTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly CODSurcharge testObject;

        public CODSurchargeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = new CODSurcharge(new Dictionary<UpsSurchargeType, double>
            {
                {UpsSurchargeType.CollectonDelivery, 1.11}
            });
        }

        [Fact]
        public void Apply_AddsSurchargeAmount_WhenShipmentHasCODEnabled()
        {
            UpsShipmentEntity shipment = new UpsShipmentEntity()
            {
                CodEnabled = true,
                Packages = { new UpsPackageEntity()}
            };

            Mock<IUpsLocalServiceRate> localServiceRate = mock.Mock<IUpsLocalServiceRate>();

            testObject.Apply(shipment, localServiceRate.Object);

            localServiceRate.Verify(s=>s.AddAmount(1.11m, EnumHelper.GetDescription(UpsSurchargeType.CollectonDelivery)));
        }

        [Fact]
        public void Apply_AddsSurchargeAmount_WhenShipmentHasCODEnabledAndMultiPackage()
        {
            UpsShipmentEntity shipment = new UpsShipmentEntity
            {
                CodEnabled = true,
                Packages = { new UpsPackageEntity(), new UpsPackageEntity() }
            };

            Mock<IUpsLocalServiceRate> localServiceRate = mock.Mock<IUpsLocalServiceRate>();

            testObject.Apply(shipment, localServiceRate.Object);

            localServiceRate.Verify(s => s.AddAmount(1.11m * 2, EnumHelper.GetDescription(UpsSurchargeType.CollectonDelivery)));
        }

        [Fact]
        public void Apply_DoesNotAddSurchargeAmount_WhenShipmentDoesNotHaveCODEnabled()
        {
            UpsShipmentEntity shipment = new UpsShipmentEntity()
            {
                CodEnabled = false,
                Packages = { new UpsPackageEntity() }
            };

            Mock<IUpsLocalServiceRate> localServiceRate = mock.Mock<IUpsLocalServiceRate>();

            testObject.Apply(shipment, localServiceRate.Object);

            localServiceRate.Verify(s => s.AddAmount(It.IsAny<decimal>(), It.IsAny<string>()), Times.Never);
        }

        public void Dispose()
        {
            mock.Dispose();
        }

    }
}