using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Connection;
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
        private readonly DeliveryAreaSurcharge testObject;
        private readonly AutoMock mock;
        private readonly Mock<IResidentialDeterminationService> residentialDeterminationService;

        private readonly Dictionary<UpsSurchargeType, double> surcharges = new Dictionary<UpsSurchargeType, double>
        {
            {UpsSurchargeType.LargePackage, 67.50},
            {UpsSurchargeType.DeliveryAreaCommercialAir, 2.45},
            {UpsSurchargeType.DeliveryAreaResidentialAir, 3.70},
            {UpsSurchargeType.DeliveryAreaCommercialExtendedAir, 2.45},
            {UpsSurchargeType.DeliveryAreaResidentialExtendedAir, 4.00},
            {UpsSurchargeType.DeliveryAreaCommercialGround, 2.30},
            {UpsSurchargeType.DeliveryAreaResidentialGround, 3.15},
            {UpsSurchargeType.DeliveryAreaCommercialExtendedGround, 2.35},
            {UpsSurchargeType.DeliveryAreaResidentialExtendedGround, 4.00},
            {UpsSurchargeType.RemoteAreaAlaska, 22.50},
            {UpsSurchargeType.RemoteAreaHawaii, 6.75},
            {UpsSurchargeType.ResidentialAir, 3.65},
            {UpsSurchargeType.ResidentialGround, 3.25}
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

            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            var sqlAdapter = mock.Mock<ISqlAdapterFactory>();

            residentialDeterminationService = mock.CreateMock<IResidentialDeterminationService>();
            testObject = new DeliveryAreaSurcharge(surcharges, zoneFile, residentialDeterminationService.Object, sqlAdapter.Object);
        }

        [Fact]
        public void Apply_AppliesNoSurcharge_WhenShipToZipIsNotDASZipAndNotResidential()
        {
            UpsShipmentEntity shipment = new UpsShipmentEntity
            {
                Packages = { new UpsPackageEntity() },
                Shipment = new ShipmentEntity
                {
                    ShipPostalCode = StandardZip.ToString()
                }
            };

            residentialDeterminationService
                .Setup(s => s.IsResidentialAddress(It.IsAny<ShipmentEntity>()))
                .Returns(false);

            var serviceRate = new UpsLocalServiceRate(UpsServiceType.UpsGround, 0, false, null);

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
                Packages = {new UpsPackageEntity()},
                Service = (int) serviceType,
                Shipment = new ShipmentEntity
                {
                    ShipPostalCode = destZip.PadLeft(5,'0')
                }
            };

            residentialDeterminationService
                .Setup(s => s.IsResidentialAddress(It.IsAny<ShipmentEntity>()))
                .Returns(isResidential);

            var serviceRate = new UpsLocalServiceRate(serviceType, 0, false, null);
            
            testObject.Apply(shipment, serviceRate);

            Assert.Equal((decimal) surcharges[surchargeType], serviceRate.Amount);
        }

        [Fact]
        public void Apply_AppliesSurchargesCorrectly_WhenMultiPackage()
        {
            UpsShipmentEntity shipment = new UpsShipmentEntity
            {
                Packages = { new UpsPackageEntity(), new UpsPackageEntity(), new UpsPackageEntity() },
                Service = (int) UpsServiceType.UpsGround,
                Shipment = new ShipmentEntity
                {
                    ShipPostalCode = StandardZip.ToString()
                }
            };

            residentialDeterminationService
                .Setup(s => s.IsResidentialAddress(It.IsAny<ShipmentEntity>()))
                .Returns(true);

            var serviceRate = new UpsLocalServiceRate(UpsServiceType.UpsGround, 0, false, null);

            testObject.Apply(shipment, serviceRate);

            Assert.Equal((decimal)surcharges[UpsSurchargeType.ResidentialGround] * shipment.Packages.Count, serviceRate.Amount);
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}