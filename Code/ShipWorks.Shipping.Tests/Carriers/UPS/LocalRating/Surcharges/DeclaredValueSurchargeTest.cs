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
    public class DeclaredValueSurchargeTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly DeclaredValueSurcharge testObject;
        private readonly Mock<IUpsLocalServiceRate> serviceRate;

        public DeclaredValueSurchargeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            var surcharges = new Dictionary<UpsSurchargeType, double>
            {
                {UpsSurchargeType.DeclaredValueMinimumCharge, 2.70},
                {UpsSurchargeType.DeclaredValuePricePerHundred, .9}
            };
            mock.Provide<IDictionary<UpsSurchargeType, double>>(surcharges);

            serviceRate = mock.CreateMock<IUpsLocalServiceRate>();

            testObject = mock.Create<DeclaredValueSurcharge>();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(100)]
        public void Apply_DoesNotAddDeclaredValue_WhenValueIsUnder100(decimal declaredValue)
        {
            var shipment = new UpsShipmentEntity()
            {
                Packages =
                {
                    new UpsPackageEntity()
                    {
                        DeclaredValue = declaredValue
                    }
                }
            };

            testObject.Apply(shipment, serviceRate.Object);

            serviceRate.Verify(s => s.AddAmount(It.IsAny<decimal>(), It.IsAny<string>()), Times.Never);
        }

        [Theory]
        [InlineData(101, 2.70, UpsSurchargeType.DeclaredValueMinimumCharge)]
        [InlineData(300, 2.70, UpsSurchargeType.DeclaredValueMinimumCharge)]
        [InlineData(301, 3.60, UpsSurchargeType.DeclaredValuePricePerHundred)]
        [InlineData(400, 3.60, UpsSurchargeType.DeclaredValuePricePerHundred)]
        [InlineData(401, 4.50, UpsSurchargeType.DeclaredValuePricePerHundred)]
        public void Apply_AppliesCorrectAmount_WhenValueIsOver100(decimal declaredValue, decimal expectedValue, UpsSurchargeType expectedSurchargeType)
        {
            var shipment = new UpsShipmentEntity { Packages = { new UpsPackageEntity() { DeclaredValue = declaredValue } } };

            testObject.Apply(shipment, serviceRate.Object);

            string surchargeName = $"Package 1 - {EnumHelper.GetDescription(expectedSurchargeType)}";

            serviceRate.Verify(s => s.AddAmount(It.IsAny<decimal>(), surchargeName), Times.Once);
            serviceRate.Verify(s => s.AddAmount(expectedValue, It.IsAny<string>()), Times.Once);

            serviceRate.Verify(s => s.AddAmount(expectedValue, surchargeName), Times.Once);
        }

        [Fact]
        public void Apply_AppliesCorrectAmount_ForMultiplePackages()
        {
            var shipment = new UpsShipmentEntity
            {
                Packages =
                {
                    new UpsPackageEntity() {DeclaredValue = 300}, // 2.70
                    new UpsPackageEntity() {DeclaredValue = 100}, // 0
                    new UpsPackageEntity() {DeclaredValue = 400}, // 3.60
                    new UpsPackageEntity() {DeclaredValue = 301} // 3.60
                }
            };

            testObject.Apply(shipment, serviceRate.Object);

            serviceRate.Verify(s => s.AddAmount(2.7M, "Package 1 - Declared Value - Minimum Charge"), Times.Once);
            serviceRate.Verify(s => s.AddAmount(3.6M, "Package 3 - Declared Value - Price Per Hundred"), Times.Once);
            serviceRate.Verify(s => s.AddAmount(3.6M, "Package 4 - Declared Value - Price Per Hundred"), Times.Once);
            serviceRate.Verify(s => s.AddAmount(It.IsAny<decimal>(), It.IsAny<string>()), Times.Exactly(3));
        }

        public void Dispose()
        {
            mock.Dispose();
        }

    }
}