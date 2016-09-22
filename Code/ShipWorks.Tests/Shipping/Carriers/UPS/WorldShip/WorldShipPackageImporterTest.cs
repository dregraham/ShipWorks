using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.ServiceManager;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using ShipWorks.Tests.Shared;
using System;
using System.Linq;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.WorldShip
{
    public class WorldShipPackageImporterTest : IDisposable
    {
        private readonly AutoMock mock;

        public WorldShipPackageImporterTest()
        {
            mock = AutoMock.GetLoose();
        }

        [Fact]
        public void ImportPackageToShipment_PublishedChargeIsSet()
        {
            var publishedCharge = 3.50d;
            ShipmentEntity shipment = new ShipmentEntity
            {
                Ups = new UpsShipmentEntity()
            };

            WorldShipProcessedEntity worldShipProcessed = new WorldShipProcessedEntity
            {
                PublishedCharges = publishedCharge
            };

            var testObject = mock.Create<WorldShipPackageImporter>();
            testObject.ImportPackageToShipment(shipment, worldShipProcessed);
            Assert.Equal((decimal) publishedCharge, shipment.Ups.PublishedCharges);
        }

        [Fact]
        public void ImportPackageToShipment_NegotiatedRateIsSet_WhenNegotiatedRate()
        {
            ShipmentEntity shipment = new ShipmentEntity
            {
                Ups = new UpsShipmentEntity()
            };

            WorldShipProcessedEntity worldShipProcessed = new WorldShipProcessedEntity
            {
                NegotiatedCharges = 5
            };

            var testObject = mock.Create<WorldShipPackageImporter>();
            testObject.ImportPackageToShipment(shipment, worldShipProcessed);

            Assert.True(shipment.Ups.NegotiatedRate);
            Assert.Equal(5, shipment.ShipmentCost);
        }

        [Fact]
        public void ImportPackageToShipment_ShippingCostIsPublishedRate_WhenNegotiatedRateIsZero()
        {
            var publishedCharge = 3.50d;
            ShipmentEntity shipment = new ShipmentEntity
            {
                Ups = new UpsShipmentEntity()
            };

            WorldShipProcessedEntity worldShipProcessed = new WorldShipProcessedEntity
            {
                NegotiatedCharges = 0,
                PublishedCharges = publishedCharge
            };

            var testObject = mock.Create<WorldShipPackageImporter>();
            testObject.ImportPackageToShipment(shipment, worldShipProcessed);

            Assert.False(shipment.Ups.NegotiatedRate);
            Assert.Equal((decimal) publishedCharge, shipment.ShipmentCost);
        }

        [Fact]
        public void ImportPackageToShipment_VerifyEnsureShipmentLoadedIsCalled()
        {
            ShipmentEntity shipment = new ShipmentEntity
            {
                Ups = new UpsShipmentEntity()
            };

            WorldShipProcessedEntity worldShipProcessed = new WorldShipProcessedEntity();

            Mock<IShippingManager> shippingManagerMock = mock.Mock<IShippingManager>();

            var testObject = mock.Create<WorldShipPackageImporter>();
            testObject.ImportPackageToShipment(shipment, worldShipProcessed);

            shippingManagerMock.Verify(s=>s.EnsureShipmentLoaded(shipment), Times.Once);
        }

        [Fact]
        public void ImportPackageToShipment_PackageTypeMatchesImportPackageType()
        {
            var publishedCharge = 3.50d;
            ShipmentEntity shipment = new ShipmentEntity
            {
                Ups = new UpsShipmentEntity()
            };

            WorldShipProcessedEntity worldShipProcessed = new WorldShipProcessedEntity
            {
                PublishedCharges = publishedCharge, PackageType = "PARCELS"
            };

            var testObject = mock.Create<WorldShipPackageImporter>();
            testObject.ImportPackageToShipment(shipment, worldShipProcessed);
            Assert.Equal((int) UpsPackagingType.Parcels, shipment.Ups.Packages.Single().PackagingType);
        }

        [Fact]
        public void ImportPackageToShipment_ServiceTypeMatchesImportServiceType()
        {
            var publishedCharge = 3.50d;
            ShipmentEntity shipment = new ShipmentEntity
            {
                Ups = new UpsShipmentEntity(),
                ShipCountryCode = "CA"
            };

            WorldShipProcessedEntity worldShipProcessed = new WorldShipProcessedEntity
            {
                PublishedCharges = publishedCharge,
                ServiceType = " testServicetype "
            };

            var upsServiceMappingMock = mock.MockRepository.Create<IUpsServiceMapping>();
            upsServiceMappingMock.SetupGet(m => m.UpsServiceType).Returns(UpsServiceType.UpsCaWorldWideExpress);


            Mock<IUpsServiceManager> upsServiceManagerMock = mock.MockRepository.Create<IUpsServiceManager>();
            upsServiceManagerMock.Setup(s => s.GetServicesByWorldShipDescription("TESTSERVICETYPE", "CA"))
                .Returns(upsServiceMappingMock.Object);

            var upsServiceManagerFactoryMock = mock.MockRepository.Create<IUpsServiceManagerFactory>();
            mock.MockFunc<ShipmentEntity, IUpsServiceManagerFactory>(upsServiceManagerFactoryMock);
            upsServiceManagerFactoryMock.Setup(f => f.Create(shipment)).Returns(upsServiceManagerMock.Object);

            var testObject = mock.Create<WorldShipPackageImporter>();
            testObject.ImportPackageToShipment(shipment, worldShipProcessed);
            Assert.Equal((int) UpsServiceType.UpsCaWorldWideExpress, shipment.Ups.Service);
        }

        [Fact]
        public void ImportPackageToShipment_DeclaredValueAmountSet()
        {
            ShipmentEntity shipment = new ShipmentEntity
            {
                Ups = new UpsShipmentEntity()
            };

            WorldShipProcessedEntity worldShipProcessed = new WorldShipProcessedEntity
            {
                DeclaredValueOption = "Y",
                DeclaredValueAmount = 42
            };

            var testObject = mock.Create<WorldShipPackageImporter>();
            testObject.ImportPackageToShipment(shipment, worldShipProcessed);
            Assert.Equal(42, shipment.Ups.Packages.Single().DeclaredValue);
        }

        [Fact]
        public void ImportPackageToShipment_TrackingNumbersSet_WhenMI()
        {
            string uspsTrackingNumber = "blah usps";
            ShipmentEntity shipment = new ShipmentEntity
            {
                Ups = new UpsShipmentEntity()
                {
                    Service = (int) UpsServiceType.UpsMailInnovationsFirstClass
                }
            };

            WorldShipProcessedEntity worldShipProcessed = new WorldShipProcessedEntity
            {
                UspsTrackingNumber=uspsTrackingNumber
            };

            var testObject = mock.Create<WorldShipPackageImporter>();
            testObject.ImportPackageToShipment(shipment, worldShipProcessed);
            Assert.Equal(uspsTrackingNumber, shipment.Ups.Packages.Single().UspsTrackingNumber);
            Assert.Equal(uspsTrackingNumber, shipment.Ups.UspsTrackingNumber);
            Assert.Empty(shipment.Ups.Packages.Single().TrackingNumber);
            Assert.Equal(uspsTrackingNumber, shipment.TrackingNumber);
        }

        [Fact]
        public void ImportPackageToShipment_TrackingNumbersSet_WhenSurePost()
        {
            string uspsTrackingNumber = "usps tracking number";
            string upsTrackingNumber = "ups tracking number";
            string leadTrackingNumber = "lead tracking number";

            ShipmentEntity shipment = new ShipmentEntity
            {
                Ups = new UpsShipmentEntity()
                {
                    Service = (int)UpsServiceType.UpsSurePost1LbOrGreater
                }
            };

            WorldShipProcessedEntity worldShipProcessed = new WorldShipProcessedEntity
            {
                UspsTrackingNumber = uspsTrackingNumber,
                TrackingNumber = upsTrackingNumber,
                LeadTrackingNumber = leadTrackingNumber
            };

            var testObject = mock.Create<WorldShipPackageImporter>();
            testObject.ImportPackageToShipment(shipment, worldShipProcessed);
            Assert.Equal(uspsTrackingNumber, shipment.Ups.Packages.Single().UspsTrackingNumber);
            Assert.Equal(uspsTrackingNumber, shipment.Ups.UspsTrackingNumber);
            Assert.Equal(upsTrackingNumber, shipment.Ups.Packages.Single().TrackingNumber);
            Assert.Equal(leadTrackingNumber, shipment.TrackingNumber);
        }

        [Fact]
        public void ImportPackageToShipment_TrackingNumbersSet_WhenNotHybridService()
        {
            string upsTrackingNumber = "ups tracking number";
            string leadTrackingNumber = "lead tracking number";

            ShipmentEntity shipment = new ShipmentEntity
            {
                Ups = new UpsShipmentEntity()
                {
                    Service = (int)UpsServiceType.UpsGround
                }
            };

            WorldShipProcessedEntity worldShipProcessed = new WorldShipProcessedEntity
            {
                TrackingNumber = upsTrackingNumber,
                LeadTrackingNumber = leadTrackingNumber
            };

            var testObject = mock.Create<WorldShipPackageImporter>();
            testObject.ImportPackageToShipment(shipment, worldShipProcessed);
            Assert.Empty(shipment.Ups.Packages.Single().UspsTrackingNumber);
            Assert.Empty(shipment.Ups.UspsTrackingNumber);
            Assert.Equal(upsTrackingNumber, shipment.Ups.Packages.Single().TrackingNumber);
            Assert.Equal(leadTrackingNumber, shipment.TrackingNumber);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
