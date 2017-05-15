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
    public class NdaEarlyOver150LbsSurchargeTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly NdaEarlyOver150LbsSurcharge testObject;

        public NdaEarlyOver150LbsSurchargeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = new NdaEarlyOver150LbsSurcharge(new Dictionary<UpsSurchargeType, double>
            {
                {UpsSurchargeType.NdaEarlyOver150Lbs, 30.00}
            });
        }

        [Theory]
        [InlineData(1, 55, 20, 19, UpsServiceType.UpsNextDayAirAM, 30)]
        [InlineData(2, 55, 20, 19, UpsServiceType.UpsNextDayAirAM, 60)]
        public void Apply_AddsSurchargeAmount(int numberOfPacakges,
            int length,
            int width,
            int height,
            UpsServiceType serviceType,
            decimal amountToAdd)
        {
            var package = new UpsPackageEntity()
            {
                DimsLength = length,
                DimsWidth = width,
                DimsHeight = height
            };

            var shipment = new UpsShipmentEntity();
            shipment.Packages.AddRange(Enumerable.Repeat(package, numberOfPacakges));

            var localServiceRate = mock.CreateMock<IUpsLocalServiceRate>();
            localServiceRate.Setup(x => x.Service).Returns(serviceType);

            testObject.Apply(shipment, localServiceRate.Object);

            localServiceRate.Verify(
                r => r.AddAmount(amountToAdd, $"{numberOfPacakges} NDA Early package(s) over 150LBS"), Times.Once);
        }


        [Theory]
        [InlineData(52, 20, 20, UpsServiceType.UpsNextDayAirAM)]
        [InlineData(55, 20, 19, UpsServiceType.UpsNextDayAir)]
        public void Apply_DoesNotAddSurchargeAmount(int length, int width, int height, UpsServiceType serviceType)
        {
            var package = new UpsPackageEntity()
            {
                DimsLength = length,
                DimsWidth = width,
                DimsHeight = height
            };
            var shipment = new UpsShipmentEntity();
            shipment.Packages.Add(package);

            var localServiceRate = mock.CreateMock<IUpsLocalServiceRate>();
            localServiceRate.Setup(x => x.Service).Returns(serviceType);

            testObject.Apply(shipment, localServiceRate.Object);

            localServiceRate.Verify(r => r.AddAmount(It.IsAny<decimal>(), It.IsAny<string>()), Times.Never());
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
