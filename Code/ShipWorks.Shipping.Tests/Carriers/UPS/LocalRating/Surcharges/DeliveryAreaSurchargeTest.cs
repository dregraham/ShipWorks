using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Shipping.Carriers.Ups.LocalRating.Surcharges;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating.Surcharges
{
    public class DeliveryAreaSurchargeTest : IDisposable
    {
        private const int StandardZip = 12345;
        private const int DeliveryAreaSurchargeZip = 01007;
        private const int DeliveryAreaExtendedSurchargeZip = 58001;
        private const int RemoteAlaskaZip = 96703;
        private const int RemoteHawaiiZip = 99950;
        private readonly UpsLocalServiceRate serviceRate;
        private readonly DeliveryAreaSurcharge testObject;
        private readonly AutoMock mock;
        private readonly Dictionary<UpsSurchargeType, decimal> surcharges = new Dictionary<UpsSurchargeType, decimal>
        {
            {UpsSurchargeType.LargePackage, 67.50m},
            {UpsSurchargeType.DeliveryAreaCommercialAir, 2.45m},
            {UpsSurchargeType.DeliveryAreaResidentialAir, 3.70m},
            {UpsSurchargeType.DeliveryAreaCommercialExtendedAir, 2.45m},
            {UpsSurchargeType.DeliveryAreaResidentialExtendedAir, 4.00m},
            {UpsSurchargeType.DeliveryAreaCommercialGround, 2.30m},
            {UpsSurchargeType.DeliveryAreaResidentialGround, 3.15m},
            {UpsSurchargeType.DeliveryAreaCommercialExtendedGround, 2.35m},
            {UpsSurchargeType.DeliveryAreaResidentialExtendedGround, 4.00m},
            {UpsSurchargeType.RemoteAreaAlaska, 22.50m},
            {UpsSurchargeType.RemoteAreaHawaii, 6.75m},
            {UpsSurchargeType.ResidentialAir, 3.65m},
            {UpsSurchargeType.ResidentialGround, 3.25m}
        };

        public DeliveryAreaSurchargeTest()
        {
            var zoneFile = new UpsLocalRatingZoneFileEntity
            {
                UpsLocalRatingDeliveryAreaSurcharge =
                {
                    new UpsLocalRatingDeliveryAreaSurchargeEntity
                    {
                        DeliveryAreaType = (int) UpsDeliveryAreaSurchargeType.Us48Das,
                        DestinationZip = DeliveryAreaSurchargeZip
                    },
                    new UpsLocalRatingDeliveryAreaSurchargeEntity
                    {
                        DeliveryAreaType = (int) UpsDeliveryAreaSurchargeType.Us48DasExtended,
                        DestinationZip = DeliveryAreaExtendedSurchargeZip
                    },
                    new UpsLocalRatingDeliveryAreaSurchargeEntity
                    {
                        DeliveryAreaType = (int) UpsDeliveryAreaSurchargeType.UsRemoteAk,
                        DestinationZip = RemoteAlaskaZip
                    },
                    new UpsLocalRatingDeliveryAreaSurchargeEntity
                    {
                        DeliveryAreaType = (int) UpsDeliveryAreaSurchargeType.UsRemoteHi,
                        DestinationZip = RemoteHawaiiZip
                    }
                }
            };

            serviceRate = new UpsLocalServiceRate(UpsServiceType.UpsGround, 0, false, 0);

            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            mock.Mock<IUpsLocalRateTableRepository>()
                .Setup(repo => repo.GetLatestZoneFile())
                .Returns(zoneFile);

            testObject = mock.Create<DeliveryAreaSurcharge>(new TypedParameter(typeof(Dictionary<UpsSurchargeType, decimal>), surcharges));
        }

        [Fact]
        public void Apply_AppliesNoSurcharge_WhenShipToZipIsNotDASZipAndNotResidential()
        {
            UpsShipmentEntity shipment = new UpsShipmentEntity
            {
                Service = (int) UpsServiceType.UpsGround,
                Shipment = new ShipmentEntity
                {
                    ShipPostalCode = StandardZip.ToString(),
                    ResidentialResult = false
                }
            };

            testObject.Apply(shipment, serviceRate);

            Assert.Equal(0, serviceRate.Amount);
        }

        [Theory]
        [InlineData(UpsSurchargeType.ResidentialGround, UpsServiceType.UpsGround, StandardZip, true)]
        [InlineData(UpsSurchargeType.ResidentialAir, UpsServiceType.Ups2DayAir, StandardZip, true)]
        [InlineData(UpsSurchargeType.DeliveryAreaResidentialGround, UpsServiceType.UpsGround, DeliveryAreaSurchargeZip, true)]
        [InlineData(UpsSurchargeType.DeliveryAreaResidentialAir, UpsServiceType.Ups2DayAir, DeliveryAreaSurchargeZip, true)]
        [InlineData(UpsSurchargeType.DeliveryAreaCommercialGround, UpsServiceType.UpsGround, DeliveryAreaSurchargeZip, false)]
        [InlineData(UpsSurchargeType.DeliveryAreaCommercialAir, UpsServiceType.Ups2DayAir, DeliveryAreaSurchargeZip, false)]
        [InlineData(UpsSurchargeType.DeliveryAreaResidentialExtendedGround, UpsServiceType.UpsGround, DeliveryAreaExtendedSurchargeZip, true)]
        [InlineData(UpsSurchargeType.DeliveryAreaResidentialExtendedAir, UpsServiceType.Ups2DayAir, DeliveryAreaExtendedSurchargeZip, true)]
        [InlineData(UpsSurchargeType.DeliveryAreaCommercialExtendedGround, UpsServiceType.UpsGround, DeliveryAreaExtendedSurchargeZip, false)]
        [InlineData(UpsSurchargeType.DeliveryAreaCommercialExtendedAir, UpsServiceType.Ups2DayAir, DeliveryAreaExtendedSurchargeZip, false)]
        [InlineData(UpsSurchargeType.RemoteAreaAlaska, UpsServiceType.UpsGround, RemoteAlaskaZip, true)]
        [InlineData(UpsSurchargeType.RemoteAreaHawaii, UpsServiceType.UpsGround, RemoteHawaiiZip, true)]
        public void Apply_AppliesSurchargesCorrectly(UpsSurchargeType surchargeType, UpsServiceType serviceType, string destZip, bool isResidential)
        {
            UpsShipmentEntity shipment = new UpsShipmentEntity
            {
                Service = (int) serviceType,
                Shipment = new ShipmentEntity
                {
                    ShipPostalCode = destZip,
                    ResidentialResult = isResidential
                }
            };

            testObject.Apply(shipment, serviceRate);

            Assert.Equal(surcharges[surchargeType], serviceRate.Amount);
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}