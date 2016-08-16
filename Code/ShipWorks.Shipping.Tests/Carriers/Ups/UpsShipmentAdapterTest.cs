using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Stores;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Ups
{
    public class UpsShipmentAdapterTest : IDisposable
    {
        readonly AutoMock mock;
        readonly ShipmentEntity shipment;

        public UpsShipmentAdapterTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
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
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExcpetion_WhenShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new UpsShipmentAdapter(null, mock.Create<IShipmentTypeManager>(),
                    mock.Create<ICustomsManager>(), mock.Create<IStoreManager>()));
            Assert.Throws<ArgumentNullException>(() =>
                new UpsShipmentAdapter(new ShipmentEntity(), mock.Create<IShipmentTypeManager>(),
                    mock.Create<ICustomsManager>(), mock.Create<IStoreManager>()));
            Assert.Throws<ArgumentNullException>(() =>
                new UpsShipmentAdapter(shipment, null,
                    mock.Create<ICustomsManager>(), mock.Create<IStoreManager>()));
            Assert.Throws<ArgumentNullException>(() =>
                new UpsShipmentAdapter(shipment, mock.Create
                    <IShipmentTypeManager>(), null, mock.Create<IStoreManager>()));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExcpetion_WhenPostalShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new UpsShipmentAdapter(new ShipmentEntity(), mock.Create<IShipmentTypeManager>(),
                    mock.Create<ICustomsManager>(), mock.Create<IStoreManager>()));
        }

        [Fact]
        public void AccountId_ReturnsShipmentValue()
        {
            shipment.Ups.UpsAccountID = 12;
            var testObject = mock.Create<UpsShipmentAdapter>(TypedParameter.From(shipment));
            Assert.Equal(12, testObject.AccountId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10009238)]
        public void AccountId_StoresSpecifiedValue_WhenValueIsValid(long value)
        {
            var testObject = mock.Create<UpsShipmentAdapter>(TypedParameter.From(shipment));
            testObject.AccountId = value;
            Assert.Equal(value, shipment.Ups.UpsAccountID);
        }

        [Fact]
        public void AccountId_StoresZero_WhenValueIsNull()
        {
            var testObject = mock.Create<UpsShipmentAdapter>(TypedParameter.From(shipment));
            testObject.AccountId = null;
            Assert.Equal(0, shipment.Ups.UpsAccountID);
        }

        [Fact]
        public void Shipment_IsNotNull()
        {
            var testObject = mock.Create<UpsShipmentAdapter>(TypedParameter.From(shipment));
            Assert.NotNull(testObject.Shipment);
        }

        [Fact]
        public void ShipmentTypeCode_IsUps()
        {
            var testObject = mock.Create<UpsShipmentAdapter>(TypedParameter.From(shipment));
            Assert.Equal(ShipmentTypeCode.UpsOnLineTools, testObject.ShipmentTypeCode);
        }

        [Fact]
        public void SupportsAccounts_IsTrue()
        {
            var testObject = mock.Create<UpsShipmentAdapter>(TypedParameter.From(shipment));

            Assert.True(testObject.SupportsAccounts);
        }

        [Fact]
        public void SupportsMultiplePackages_IsTrue()
        {
            mock.WithShipmentTypeFromShipmentManager(x => x.Setup(b => b.SupportsMultiplePackages).Returns(true));

            var testObject = mock.Create<UpsShipmentAdapter>(TypedParameter.From(shipment));

            Assert.True(testObject.SupportsMultiplePackages);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, true)]
        public void IsDomestic_DomesticIsTrue_WhenShipCountryIsUs(bool isDomestic, bool expected)
        {
            mock.WithShipmentTypeFromShipmentManager(x => x.Setup(b => b.IsDomestic(shipment)).Returns(isDomestic));

            var testObject = mock.Create<UpsShipmentAdapter>(TypedParameter.From(shipment));

            Assert.Equal(expected, testObject.IsDomestic);
        }

        [Fact]
        public void UpdateDynamicData_DelegatesToShipmentTypeAndCustomsManager()
        {
            var shipmentType = mock.WithShipmentTypeFromShipmentManager();

            var testObject = mock.Create<UpsShipmentAdapter>(TypedParameter.From(shipment));
            testObject.UpdateDynamicData();

            shipmentType.Verify(b => b.UpdateDynamicShipmentData(It.IsAny<ShipmentEntity>()));
            shipmentType.Verify(b => b.UpdateTotalWeight(It.IsAny<ShipmentEntity>()));

            mock.Mock<ICustomsManager>()
                .Verify(b => b.EnsureCustomsLoaded(It.IsAny<IEnumerable<ShipmentEntity>>()));
        }

        [Fact]
        public void UpdateDynamicData_ErrorsReturned_AreCorrect()
        {
            Dictionary<ShipmentEntity, Exception> errors = new Dictionary<ShipmentEntity, Exception>();
            errors.Add(shipment, new Exception("test"));

            mock.Mock<ICustomsManager>()
                .Setup(c => c.EnsureCustomsLoaded(It.IsAny<IEnumerable<ShipmentEntity>>())).Returns(errors);

            var testObject = mock.Create<UpsShipmentAdapter>(TypedParameter.From(shipment));

            Assert.NotNull(testObject.UpdateDynamicData());
            Assert.Equal(1, testObject.UpdateDynamicData().Count);
        }

        [Fact]
        public void SupportsPackageTypes_IsTrue()
        {
            var testObject = mock.Create<UpsShipmentAdapter>(TypedParameter.From(shipment));

            Assert.True(testObject.SupportsPackageTypes);
        }

        [Fact]
        public void ShipDate_ReturnsShipmentValue()
        {
            var testObject = mock.Create<UpsShipmentAdapter>(TypedParameter.From(shipment));
            Assert.Equal(shipment.ShipDate, testObject.ShipDate);
        }

        [Fact]
        public void ShipDate_IsUpdated()
        {
            var testObject = mock.Create<UpsShipmentAdapter>(TypedParameter.From(shipment));

            testObject.ShipDate = testObject.ShipDate.AddDays(1);

            Assert.Equal(shipment.ShipDate, testObject.ShipDate);
        }

        [Fact]
        public void ServiceType_ReturnsShipmentValue()
        {
            var testObject = mock.Create<UpsShipmentAdapter>(TypedParameter.From(shipment));
            Assert.Equal(shipment.Ups.Service, testObject.ServiceType);
        }

        [Fact]
        public void ServiceType_IsUpdated()
        {
            var testObject = mock.Create<UpsShipmentAdapter>(TypedParameter.From(shipment));

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
            mock.WithShipmentTypeFromShipmentManager(x =>
            {
                x.Setup(b => b.SupportsGetRates).Returns(true);
                x.Setup(b => b.ShipmentTypeCode).Returns(ShipmentTypeCode.UpsOnLineTools);
            });
            var testObject = mock.Create<UpsShipmentAdapter>(TypedParameter.From(shipment));
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
            mock.WithShipmentTypeFromShipmentManager(x =>
            {
                x.Setup(b => b.SupportsGetRates).Returns(true);
                x.Setup(b => b.ShipmentTypeCode).Returns(ShipmentTypeCode.UpsOnLineTools);
            });
            var testObject = mock.Create<UpsShipmentAdapter>(TypedParameter.From(shipment));
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
            mock.WithShipmentTypeFromShipmentManager(x =>
            {
                x.Setup(b => b.SupportsGetRates).Returns(true);
                x.Setup(b => b.ShipmentTypeCode).Returns(ShipmentTypeCode.UpsOnLineTools);
            });
            shipment.Ups.Service = (int) UpsServiceType.UpsSurePost1LbOrGreater;
            var testObject = mock.Create<UpsShipmentAdapter>(TypedParameter.From(shipment));
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
            var testObject = mock.Create<UpsShipmentAdapter>(TypedParameter.From(shipment));
            testObject.AddPackage();

            Assert.Equal(1, shipment.Ups.Packages.Count);
        }

        [Fact]
        public void AddPackage_ReturnsPackageAdapter_ForNewPackage()
        {
            var testObject = mock.Create<UpsShipmentAdapter>(TypedParameter.From(shipment));
            var newPackage = testObject.AddPackage();

            Assert.NotNull(newPackage);
        }

        [Fact]
        public void AddPackage_DelegatesToShipmentType_WhenNewPackageIsAdded()
        {
            var shipmentType = mock.WithShipmentTypeFromShipmentManager(x =>
                x.Setup(b => b.SupportsMultiplePackages).Returns(true));

            var testObject = mock.Create<UpsShipmentAdapter>(TypedParameter.From(shipment));
            testObject.AddPackage();

            shipmentType.Verify(x => x.UpdateDynamicShipmentData(shipment));
            shipmentType.Verify(x => x.UpdateTotalWeight(shipment));
        }

        [Fact]
        public void AddPackage_DelegatesToCustomsManager_WhenNewPackageIsAdded()
        {
            var shipmentType = mock.WithShipmentTypeFromShipmentManager(x =>
                x.Setup(b => b.SupportsMultiplePackages).Returns(true));

            var testObject = mock.Create<UpsShipmentAdapter>(TypedParameter.From(shipment));
            testObject.AddPackage();

            mock.Mock<ICustomsManager>().Verify(x => x.EnsureCustomsLoaded(new[] { shipment }));
        }

        [Fact]
        public void DeletePackage_RemovesPackageAssociatedWithAdapter()
        {
            var package = new UpsPackageEntity(2);

            shipment.Ups.Packages.Add(new UpsPackageEntity(1));
            shipment.Ups.Packages.Add(package);
            shipment.Ups.Packages.Add(new UpsPackageEntity(3));

            var testObject = mock.Create<UpsShipmentAdapter>(TypedParameter.From(shipment));
            testObject.DeletePackage(new UpsPackageAdapter(shipment, package, 1));

            Assert.DoesNotContain(package, shipment.Ups.Packages);
        }

        [Fact]
        public void DeletePackage_DoesNotRemovePackage_WhenShipmentHasSinglePackage()
        {
            var package = new UpsPackageEntity(2);

            shipment.Ups.Packages.Add(package);

            var testObject = mock.Create<UpsShipmentAdapter>(TypedParameter.From(shipment));
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

            var testObject = mock.Create<UpsShipmentAdapter>(TypedParameter.From(shipment));
            testObject.DeletePackage(new UpsPackageAdapter(shipment, new UpsPackageEntity(12), 1));

            Assert.Contains(package, shipment.Ups.Packages);
            Assert.Contains(package2, shipment.Ups.Packages);
        }

        [Fact]
        public void DeletePackage_DelegatesToShipmentType_WhenPackageIsRemoved()
        {
            var shipmentType = mock.WithShipmentTypeFromShipmentManager(x =>
                x.Setup(b => b.SupportsMultiplePackages).Returns(true));
            var package = new UpsPackageEntity(2);

            shipment.Ups.Packages.Add(package);
            shipment.Ups.Packages.Add(new UpsPackageEntity(3));

            var testObject = mock.Create<UpsShipmentAdapter>(TypedParameter.From(shipment));
            testObject.DeletePackage(new UpsPackageAdapter(shipment, package, 1));

            shipmentType.Verify(x => x.UpdateDynamicShipmentData(shipment));
            shipmentType.Verify(x => x.UpdateTotalWeight(shipment));
        }

        [Fact]
        public void DeletePackage_DelegatesToCustomsManager_WhenPackageIsRemoved()
        {
            var package = new UpsPackageEntity(2);

            shipment.Ups.Packages.Add(package);
            shipment.Ups.Packages.Add(new UpsPackageEntity(3));

            var testObject = mock.Create<UpsShipmentAdapter>(TypedParameter.From(shipment));
            testObject.DeletePackage(new UpsPackageAdapter(shipment, package, 1));

            mock.Mock<ICustomsManager>().Verify(x => x.EnsureCustomsLoaded(new[] { shipment }));
        }

        [Fact]
        public void DeletePackage_AddsPackageToRemovedEntityCollection_WhenPackageIsRemoved()
        {
            var package = new UpsPackageEntity(2);

            shipment.Ups.Packages.Add(package);
            shipment.Ups.Packages.Add(new UpsPackageEntity(3));

            var testObject = mock.Create<UpsShipmentAdapter>(TypedParameter.From(shipment));
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
