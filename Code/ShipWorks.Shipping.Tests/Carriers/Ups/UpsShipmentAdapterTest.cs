using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Ups
{
    public class UpsShipmentAdapterTest : IDisposable
    {
        readonly AutoMock mock;
        readonly ShipmentEntity shipment;
        private readonly Mock<IShipmentTypeManager> shipmentTypeManager;
        private readonly Mock<ICustomsManager> customsManager;
        private readonly Mock<UpsShipmentType> shipmentTypeMock;
        private readonly ShipmentType shipmentType;

        public UpsShipmentAdapterTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            shipmentType = new UpsOltShipmentType();
            shipment = new ShipmentEntity
            {
                ShipmentTypeCode = ShipmentTypeCode.UpsOnLineTools,
                ShipDate = new DateTime(2015, 11, 24, 10, 10, 10),
                ContentWeight = 1,
                TotalWeight = 1,
                Insurance = true,
                Ups = new UpsShipmentEntity()
                {
                    Service = (int) UpsServiceType.Ups2DayAirAM
                }
            };

            customsManager = new Mock<ICustomsManager>();
            customsManager.Setup(c => c.EnsureCustomsLoaded(It.IsAny<IEnumerable<ShipmentEntity>>())).Returns(new Dictionary<ShipmentEntity, Exception>());

            shipmentTypeMock = new Mock<UpsShipmentType>(MockBehavior.Strict);
            shipmentTypeMock.Setup(b => b.UpdateDynamicShipmentData(shipment)).Verifiable();
            shipmentTypeMock.Setup(b => b.UpdateTotalWeight(shipment)).Verifiable();
            shipmentTypeMock.Setup(b => b.SupportsMultiplePackages).Returns(() => shipmentType.SupportsMultiplePackages);
            shipmentTypeMock.Setup(b => b.IsDomestic(It.IsAny<ShipmentEntity>())).Returns(() => shipmentType.IsDomestic(shipment));

            shipmentTypeManager = new Mock<IShipmentTypeManager>();
            shipmentTypeManager.Setup(x => x.Get(shipment)).Returns(shipmentTypeMock.Object);
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExcpetion_WhenShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new UpsShipmentAdapter(null, shipmentTypeManager.Object, customsManager.Object));
            Assert.Throws<ArgumentNullException>(() => new UpsShipmentAdapter(new ShipmentEntity(), shipmentTypeManager.Object, customsManager.Object));
            Assert.Throws<ArgumentNullException>(() => new UpsShipmentAdapter(shipment, null, customsManager.Object));
            Assert.Throws<ArgumentNullException>(() => new UpsShipmentAdapter(shipment, shipmentTypeManager.Object, null));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExcpetion_WhenPostalShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new UpsShipmentAdapter(new ShipmentEntity(), shipmentTypeManager.Object, customsManager.Object));
        }

        [Fact]
        public void AccountId_ReturnsShipmentValue()
        {
            shipment.Ups.UpsAccountID = 12;
            var testObject = new UpsShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            Assert.Equal(12, testObject.AccountId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10009238)]
        public void AccountId_StoresSpecifiedValue_WhenValueIsValid(long value)
        {
            var testObject = new UpsShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            testObject.AccountId = value;
            Assert.Equal(value, shipment.Ups.UpsAccountID);
        }

        [Fact]
        public void AccountId_StoresZero_WhenValueIsNull()
        {
            var testObject = new UpsShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            testObject.AccountId = null;
            Assert.Equal(0, shipment.Ups.UpsAccountID);
        }

        [Fact]
        public void Shipment_IsNotNull()
        {
            var testObject = new UpsShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            Assert.NotNull(testObject.Shipment);
        }

        [Fact]
        public void ShipmentTypeCode_IsUps()
        {
            var testObject = new UpsShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            Assert.Equal(ShipmentTypeCode.UpsOnLineTools, testObject.ShipmentTypeCode);
        }

        [Fact]
        public void SupportsAccounts_IsTrue()
        {
            UpsShipmentAdapter testObject = new UpsShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);

            Assert.True(testObject.SupportsAccounts);
        }

        [Fact]
        public void SupportsMultiplePackages_IsTrue()
        {
            UpsShipmentAdapter testObject = new UpsShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            Assert.True(testObject.SupportsMultiplePackages);
        }

        [Fact]
        public void SupportsMultiplePackages_DomesticIsTrue_WhenShipCountryIsUs()
        {
            shipment.OriginCountryCode = "US";
            shipment.ShipCountryCode = "US";

            UpsShipmentAdapter testObject = new UpsShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            Assert.True(testObject.IsDomestic);
        }

        [Fact]
        public void SupportsMultiplePackages_DomesticIsFalse_WhenShipCountryIsCa()
        {
            shipment.OriginCountryCode = "US";
            shipment.ShipCountryCode = "CA";

            UpsShipmentAdapter testObject = new UpsShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            Assert.False(testObject.IsDomestic);
        }

        [Fact]
        public void UpdateDynamicData_DelegatesToShipmentTypeAndCustomsManager()
        {
            UpsShipmentAdapter testObject = new UpsShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            testObject.UpdateDynamicData();

            shipmentTypeMock.Verify(b => b.UpdateDynamicShipmentData(It.IsAny<ShipmentEntity>()), Times.Once);
            shipmentTypeMock.Verify(b => b.UpdateTotalWeight(It.IsAny<ShipmentEntity>()), Times.Once);

            customsManager.Verify(b => b.EnsureCustomsLoaded(It.IsAny<IEnumerable<ShipmentEntity>>()), Times.Once);
        }

        [Fact]
        public void UpdateDynamicData_ErrorsReturned_AreCorrect()
        {
            Dictionary<ShipmentEntity, Exception> errors = new Dictionary<ShipmentEntity, Exception>();
            errors.Add(shipment, new Exception("test"));

            customsManager.Setup(c => c.EnsureCustomsLoaded(It.IsAny<IEnumerable<ShipmentEntity>>())).Returns(errors);

            UpsShipmentAdapter testObject = new UpsShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);

            Assert.NotNull(testObject.UpdateDynamicData());
            Assert.Equal(1, testObject.UpdateDynamicData().Count);
        }

        [Fact]
        public void SupportsPackageTypes_IsTrue()
        {
            ICarrierShipmentAdapter testObject = new UpsShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);

            Assert.True(testObject.SupportsPackageTypes);
        }

        [Fact]
        public void ShipDate_ReturnsShipmentValue()
        {
            ICarrierShipmentAdapter testObject = new UpsShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            Assert.Equal(shipment.ShipDate, testObject.ShipDate);
        }

        [Fact]
        public void ShipDate_IsUpdated()
        {
            ICarrierShipmentAdapter testObject = new UpsShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);

            testObject.ShipDate = testObject.ShipDate.AddDays(1);

            Assert.Equal(shipment.ShipDate, testObject.ShipDate);
        }

        [Fact]
        public void ServiceType_ReturnsShipmentValue()
        {
            ICarrierShipmentAdapter testObject = new UpsShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            Assert.Equal(shipment.Ups.Service, testObject.ServiceType);
        }

        [Fact]
        public void ServiceType_IsUpdated()
        {
            ICarrierShipmentAdapter testObject = new UpsShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);

            shipment.Ups.Service = (int) UpsServiceType.UpsGround;
            testObject.ServiceType = (int) UpsServiceType.Ups2DayAir;

            Assert.Equal(shipment.Ups.Service, testObject.ServiceType);
        }

        [Theory]
        [InlineData(UpsServiceType.Ups2DayAir)]
        [InlineData(UpsServiceType.UpsNextDayAir)]
        [InlineData(UpsServiceType.WorldwideExpressPlus)]
        public void UpdateServiceFromRate_SetsService_WhenTagIsValid(UpsServiceType serviceType)
        {
            shipmentTypeManager.Setup(x => x.Get(shipment)).Returns(shipmentType);
            var testObject = new UpsShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            testObject.SelectServiceFromRate(new RateResult("Foo", "1", 1M, (int) serviceType)
            {
                Selectable = true,
                ShipmentType = ShipmentTypeCode.UpsOnLineTools
            });
            Assert.Equal((int) serviceType, shipment.Ups.Service);
        }

        [Theory]
        [InlineData(UpsServiceType.Ups2DayAir)]
        [InlineData(UpsServiceType.UpsNextDayAir)]
        [InlineData(UpsServiceType.WorldwideExpressPlus)]
        public void UpdateServiceFromRate_SetsService_WhenTagIsValidServiceType(UpsServiceType serviceType)
        {
            shipmentTypeManager.Setup(x => x.Get(shipment)).Returns(shipmentType);
            var testObject = new UpsShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            testObject.SelectServiceFromRate(new RateResult("Foo", "1", 1M, serviceType)
            {
                Selectable = true,
                ShipmentType = ShipmentTypeCode.UpsOnLineTools
            });
            Assert.Equal((int) serviceType, shipment.Ups.Service);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("Foo")]
        public void UpdateServiceFromRate_DoesNotSetService_WhenTagIsNotValid(string value)
        {
            shipmentTypeManager.Setup(x => x.Get(shipment)).Returns(shipmentType);
            shipment.Ups.Service = (int) UpsServiceType.UpsSurePost1LbOrGreater;
            var testObject = new UpsShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            testObject.SelectServiceFromRate(new RateResult("Foo", "1", 1M, value)
            {
                Selectable = true,
                ShipmentType = ShipmentTypeCode.UpsOnLineTools
            });
            Assert.Equal((int) UpsServiceType.UpsSurePost1LbOrGreater, shipment.Ups.Service);
        }

        [Fact]
        public void AddPackage_AddsNewPackageToList()
        {
            var testObject = new UpsShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            testObject.AddPackage();

            Assert.Equal(1, shipment.Ups.Packages.Count);
        }

        [Fact]
        public void AddPackage_ReturnsPackageAdapter_ForNewPackage()
        {
            var testObject = new UpsShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            var newPackage = testObject.AddPackage();

            Assert.NotNull(newPackage);
        }

        [Fact]
        public void AddPackage_DelegatesToShipmentType_WhenNewPackageIsAdded()
        {
            var testObject = new UpsShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            testObject.AddPackage();

            shipmentTypeMock.Verify(x => x.UpdateDynamicShipmentData(shipment));
            shipmentTypeMock.Verify(x => x.UpdateTotalWeight(shipment));
        }

        [Fact]
        public void AddPackage_DelegatesToCustomsManager_WhenNewPackageIsAdded()
        {
            var testObject = new UpsShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            testObject.AddPackage();

            customsManager.Verify(x => x.EnsureCustomsLoaded(new[] { shipment }));
        }

        [Fact]
        public void DeletePackage_RemovesPackageAssociatedWithAdapter()
        {
            var package = new UpsPackageEntity(2);

            shipment.Ups.Packages.Add(new UpsPackageEntity(1));
            shipment.Ups.Packages.Add(package);
            shipment.Ups.Packages.Add(new UpsPackageEntity(3));

            var testObject = new UpsShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            testObject.DeletePackage(new UpsPackageAdapter(shipment, package, 1));

            Assert.DoesNotContain(package, shipment.Ups.Packages);
        }

        [Fact]
        public void DeletePackage_DoesNotRemovePackage_WhenShipmentHasSinglePackage()
        {
            var package = new UpsPackageEntity(2);

            shipment.Ups.Packages.Add(package);

            var testObject = new UpsShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            testObject.DeletePackage(new UpsPackageAdapter(shipment, package, 1));

            Assert.Contains(package, shipment.Ups.Packages);
        }

        [Fact]
        public void DeletePackage_DoesNotRemovePackage_WhenPackageDoesNotExist()
        {
            var package = new UpsPackageEntity(2);
            var package2 = new UpsPackageEntity(2);

            shipment.Ups.Packages.Add(package);
            shipment.Ups.Packages.Add(package2);

            var testObject = new UpsShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            testObject.DeletePackage(new UpsPackageAdapter(shipment, new UpsPackageEntity(12), 1));

            Assert.Contains(package, shipment.Ups.Packages);
            Assert.Contains(package2, shipment.Ups.Packages);
        }

        [Fact]
        public void DeletePackage_DelegatesToShipmentType_WhenPackageIsRemoved()
        {
            var package = new UpsPackageEntity(2);

            shipment.Ups.Packages.Add(package);
            shipment.Ups.Packages.Add(new UpsPackageEntity(3));

            var testObject = new UpsShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            testObject.DeletePackage(new UpsPackageAdapter(shipment, package, 1));

            shipmentTypeMock.Verify(x => x.UpdateDynamicShipmentData(shipment));
            shipmentTypeMock.Verify(x => x.UpdateTotalWeight(shipment));
        }

        [Fact]
        public void DeletePackage_DelegatesToCustomsManager_WhenPackageIsRemoved()
        {
            var package = new UpsPackageEntity(2);

            shipment.Ups.Packages.Add(package);
            shipment.Ups.Packages.Add(new UpsPackageEntity(3));

            var testObject = new UpsShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            testObject.DeletePackage(new UpsPackageAdapter(shipment, package, 1));

            customsManager.Verify(x => x.EnsureCustomsLoaded(new[] { shipment }));
        }

        [Fact]
        public void DeletePackage_AddsPackageToRemovedEntityCollection_WhenPackageIsRemoved()
        {
            var package = new UpsPackageEntity(2);

            shipment.Ups.Packages.Add(package);
            shipment.Ups.Packages.Add(new UpsPackageEntity(3));

            var testObject = new UpsShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            testObject.DeletePackage(new UpsPackageAdapter(shipment, package, 1));

            Assert.Contains(package, shipment.Ups.Packages.RemovedEntitiesTracker.OfType<UpsPackageEntity>());
        }

        [Fact]
        public void DoesRateMatchSelectedService_ReturnsFalse_WhenRateShipmentTypeDoesNotMatch()
        {
            var testObject = mock.Create<UpsShipmentAdapter>(new TypedParameter(typeof(ShipmentEntity), shipment));
            var rate = new RateResult("Foo", "1", 0, 1) { ShipmentType = ShipmentTypeCode.None };

            var result = testObject.DoesRateMatchSelectedService(rate);

            Assert.False(result);
        }

        [Fact]
        public void DoesRateMatchSelectedService_ReturnsFalse_WhenRateServiceAsIntDoesNotMatch()
        {
            var testObject = mock.Create<UpsShipmentAdapter>(new TypedParameter(typeof(ShipmentEntity), shipment));
            var rate = new RateResult("Foo", "1", 0, (int) UpsServiceType.UpsGround)
            {
                ShipmentType = ShipmentTypeCode.UpsOnLineTools
            };

            var result = testObject.DoesRateMatchSelectedService(rate);

            Assert.False(result);
        }

        [Fact]
        public void DoesRateMatchSelectedService_ReturnsFalse_WhenRateServiceAsServiceTypeDoesNotMatch()
        {
            var testObject = mock.Create<UpsShipmentAdapter>(new TypedParameter(typeof(ShipmentEntity), shipment));
            var rate = new RateResult("Foo", "1", 0, UpsServiceType.UpsGround)
            {
                ShipmentType = ShipmentTypeCode.UpsOnLineTools
            };

            var result = testObject.DoesRateMatchSelectedService(rate);

            Assert.False(result);
        }

        [Fact]
        public void DoesRateMatchSelectedService_ReturnsFalse_WhenRateTagIsOtherObject()
        {
            var testObject = mock.Create<UpsShipmentAdapter>(new TypedParameter(typeof(ShipmentEntity), shipment));
            var rate = new RateResult("Foo", "1", 0, "NOT A RATE")
            {
                ShipmentType = ShipmentTypeCode.UpsOnLineTools
            };

            var result = testObject.DoesRateMatchSelectedService(rate);

            Assert.False(result);
        }

        [Fact]
        public void DoesRateMatchSelectedService_ReturnsTrue_WhenRateServiceAsIntMatches()
        {
            var testObject = mock.Create<UpsShipmentAdapter>(new TypedParameter(typeof(ShipmentEntity), shipment));
            var rate = new RateResult("Foo", "1", 0, (int) UpsServiceType.Ups2DayAirAM)
            {
                ShipmentType = ShipmentTypeCode.UpsOnLineTools
            };

            var result = testObject.DoesRateMatchSelectedService(rate);

            Assert.True(result);
        }

        [Fact]
        public void DoesRateMatchSelectedService_ReturnsTrue_WhenRateServiceAsServiceTypeMatches()
        {
            var testObject = mock.Create<UpsShipmentAdapter>(new TypedParameter(typeof(ShipmentEntity), shipment));
            var rate = new RateResult("Foo", "1", 0, UpsServiceType.Ups2DayAirAM)
            {
                ShipmentType = ShipmentTypeCode.UpsOnLineTools
            };

            var result = testObject.DoesRateMatchSelectedService(rate);

            Assert.True(result);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
