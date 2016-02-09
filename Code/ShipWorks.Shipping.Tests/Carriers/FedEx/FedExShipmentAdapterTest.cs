using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx
{
    public class FedExShipmentAdapterTest : IDisposable
    {
        readonly AutoMock mock;
        readonly ShipmentEntity shipment;
        private readonly Mock<IShipmentTypeManager> shipmentTypeManager;
        private readonly Mock<ICustomsManager> customsManager;
        private readonly Mock<FedExShipmentType> shipmentTypeMock;
        private readonly ShipmentType shipmentType;

        public FedExShipmentAdapterTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            shipmentType = new FedExShipmentType();
            shipment = new ShipmentEntity
            {
                ShipmentTypeCode = ShipmentTypeCode.FedEx,
                ShipDate = new DateTime(2015, 11, 24, 10, 10, 10),
                ContentWeight = 1,
                TotalWeight = 1,
                Insurance = true,
                FedEx = new FedExShipmentEntity()
                {
                    Service = (int) FedExServiceType.FedEx2DayAM
                }
            };

            customsManager = new Mock<ICustomsManager>();
            customsManager.Setup(c => c.EnsureCustomsLoaded(It.IsAny<IEnumerable<ShipmentEntity>>())).Returns(new Dictionary<ShipmentEntity, Exception>());

            shipmentTypeMock = new Mock<FedExShipmentType>(MockBehavior.Strict);
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
            Assert.Throws<ArgumentNullException>(() => new FedExShipmentAdapter(null, shipmentTypeManager.Object, customsManager.Object));
            Assert.Throws<ArgumentNullException>(() => new FedExShipmentAdapter(new ShipmentEntity(), shipmentTypeManager.Object, customsManager.Object));
            Assert.Throws<ArgumentNullException>(() => new FedExShipmentAdapter(shipment, null, customsManager.Object));
            Assert.Throws<ArgumentNullException>(() => new FedExShipmentAdapter(shipment, shipmentTypeManager.Object, null));
        }

        [Fact]
        public void AccountId_ReturnsShipmentValue()
        {
            shipment.FedEx.FedExAccountID = 12;
            var testObject = new FedExShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            Assert.Equal(12, testObject.AccountId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10009238)]
        public void AccountId_StoresSpecifiedValue_WhenValueIsValid(long value)
        {
            var testObject = new FedExShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            testObject.AccountId = value;
            Assert.Equal(value, shipment.FedEx.FedExAccountID);
        }

        [Fact]
        public void AccountId_StoresZero_WhenValueIsNull()
        {
            var testObject = new FedExShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            testObject.AccountId = null;
            Assert.Equal(0, shipment.FedEx.FedExAccountID);
        }

        [Fact]
        public void Shipment_IsNotNull()
        {
            var testObject = new FedExShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            Assert.NotNull(testObject.Shipment);
        }

        [Fact]
        public void ShipmentTypeCode_IsFedEx()
        {
            var testObject = new FedExShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            Assert.Equal(ShipmentTypeCode.FedEx, testObject.ShipmentTypeCode);
        }

        [Fact]
        public void SupportsAccounts_IsTrue()
        {
            FedExShipmentAdapter testObject = new FedExShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);

            Assert.True(testObject.SupportsAccounts);
        }

        [Fact]
        public void SupportsMultiplePackages_IsTrue()
        {
            FedExShipmentAdapter testObject = new FedExShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            Assert.True(testObject.SupportsMultiplePackages);
        }

        [Fact]
        public void SupportsMultiplePackages_DomesticIsTrue_WhenShipCountryIsUs()
        {
            shipment.OriginCountryCode = "US";
            shipment.ShipCountryCode = "US";

            FedExShipmentAdapter testObject = new FedExShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            Assert.True(testObject.IsDomestic);
        }

        [Fact]
        public void SupportsMultiplePackages_DomesticIsFalse_WhenShipCountryIsCa()
        {
            shipment.OriginCountryCode = "US";
            shipment.ShipCountryCode = "CA";

            FedExShipmentAdapter testObject = new FedExShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            Assert.False(testObject.IsDomestic);
        }

        [Fact]
        public void UpdateDynamicData_DelegatesToShipmentTypeAndCustomsManager()
        {
            FedExShipmentAdapter testObject = new FedExShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
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

            FedExShipmentAdapter testObject = new FedExShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);

            Assert.NotNull(testObject.UpdateDynamicData());
            Assert.Equal(1, testObject.UpdateDynamicData().Count);
        }

        [Fact]
        public void SupportsPackageTypes_IsTrue()
        {
            ICarrierShipmentAdapter testObject = new FedExShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);

            Assert.True(testObject.SupportsPackageTypes);
        }

        [Fact]
        public void ShipDate_ReturnsShipmentValue()
        {
            ICarrierShipmentAdapter testObject = new FedExShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            Assert.Equal(shipment.ShipDate, testObject.ShipDate);
        }

        [Fact]
        public void ShipDate_IsUpdated()
        {
            ICarrierShipmentAdapter testObject = new FedExShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);

            testObject.ShipDate = testObject.ShipDate.AddDays(1);

            Assert.Equal(shipment.ShipDate, testObject.ShipDate);
        }

        [Fact]
        public void ServiceType_ReturnsShipmentValue()
        {
            ICarrierShipmentAdapter testObject = new FedExShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            Assert.Equal(shipment.FedEx.Service, testObject.ServiceType);
        }

        [Fact]
        public void ServiceType_IsUpdated()
        {
            ICarrierShipmentAdapter testObject = new FedExShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);

            shipment.FedEx.Service = (int) FedExServiceType.FedEx2DayAM;
            testObject.ServiceType = (int) FedExServiceType.FedEx2Day;

            Assert.Equal(shipment.FedEx.Service, testObject.ServiceType);
        }

        [Theory]
        [InlineData(FedExServiceType.FedEx2DayAM)]
        [InlineData(FedExServiceType.FirstFreight)]
        [InlineData(FedExServiceType.SmartPost)]
        public void UpdateServiceFromRate_SetsService_WhenTagIsValid(FedExServiceType serviceType)
        {
            shipmentTypeManager.Setup(x => x.Get(shipment)).Returns(shipmentType);
            var testObject = new FedExShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            testObject.SelectServiceFromRate(new RateResult("Foo", "1", 1M, (int) serviceType)
            {
                Selectable = true,
                ShipmentType = ShipmentTypeCode.FedEx
            });
            Assert.Equal((int) serviceType, shipment.FedEx.Service);
        }

        [Theory]
        [InlineData(FedExServiceType.FedEx2DayAM)]
        [InlineData(FedExServiceType.FirstFreight)]
        [InlineData(FedExServiceType.SmartPost)]
        public void UpdateServiceFromRate_SetsService_WhenTagIsValidServiceType(FedExServiceType serviceType)
        {
            shipmentTypeManager.Setup(x => x.Get(shipment)).Returns(shipmentType);
            var testObject = new FedExShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            testObject.SelectServiceFromRate(new RateResult("Foo", "1", 1M, serviceType)
            {
                Selectable = true,
                ShipmentType = ShipmentTypeCode.FedEx
            });
            Assert.Equal((int) serviceType, shipment.FedEx.Service);
        }

        [Theory]
        [InlineData(FedExServiceType.FedEx2DayAM)]
        [InlineData(FedExServiceType.FirstFreight)]
        [InlineData(FedExServiceType.SmartPost)]
        public void UpdateServiceFromRate_SetsService_WhenTagIsValidRateSelection(FedExServiceType serviceType)
        {
            shipmentTypeManager.Setup(x => x.Get(shipment)).Returns(shipmentType);
            var testObject = new FedExShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            testObject.SelectServiceFromRate(new RateResult("Foo", "1", 1M, new FedExRateSelection(serviceType))
            {
                Selectable = true,
                ShipmentType = ShipmentTypeCode.FedEx
            });
            Assert.Equal((int) serviceType, shipment.FedEx.Service);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("Foo")]
        public void UpdateServiceFromRate_DoesNotSetService_WhenTagIsNotValid(string value)
        {
            shipmentTypeManager.Setup(x => x.Get(shipment)).Returns(shipmentType);
            shipment.FedEx.Service = (int) FedExServiceType.GroundHomeDelivery;
            var testObject = new FedExShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            testObject.SelectServiceFromRate(new RateResult("Foo", "1", 1M, value)
            {
                Selectable = true,
                ShipmentType = ShipmentTypeCode.FedEx
            });
            Assert.Equal((int) FedExServiceType.GroundHomeDelivery, shipment.FedEx.Service);
        }

        [Fact]
        public void AddPackage_AddsNewPackageToList()
        {
            var testObject = new FedExShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            testObject.AddPackage();

            Assert.Equal(1, shipment.FedEx.Packages.Count);
        }

        [Fact]
        public void AddPackage_ReturnsPackageAdapter_ForNewPackage()
        {
            var testObject = new FedExShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            var newPackage = testObject.AddPackage();

            Assert.NotNull(newPackage);
        }

        [Fact]
        public void AddPackage_DelegatesToShipmentType_WhenNewPackageIsAdded()
        {
            var testObject = new FedExShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            testObject.AddPackage();

            shipmentTypeMock.Verify(x => x.UpdateDynamicShipmentData(shipment));
            shipmentTypeMock.Verify(x => x.UpdateTotalWeight(shipment));
        }

        [Fact]
        public void AddPackage_DelegatesToCustomsManager_WhenNewPackageIsAdded()
        {
            var testObject = new FedExShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            testObject.AddPackage();

            customsManager.Verify(x => x.EnsureCustomsLoaded(new[] { shipment }));
        }

        [Fact]
        public void DeletePackage_RemovesPackageAssociatedWithAdapter()
        {
            var package = new FedExPackageEntity(2);

            shipment.FedEx.Packages.Add(new FedExPackageEntity(1));
            shipment.FedEx.Packages.Add(package);
            shipment.FedEx.Packages.Add(new FedExPackageEntity(3));

            var testObject = new FedExShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            testObject.DeletePackage(new FedExPackageAdapter(shipment, package, 1));

            Assert.DoesNotContain(package, shipment.FedEx.Packages);
        }

        [Fact]
        public void DeletePackage_DoesNotRemovePackage_WhenShipmentHasSinglePackage()
        {
            var package = new FedExPackageEntity(2);

            shipment.FedEx.Packages.Add(package);

            var testObject = new FedExShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            testObject.DeletePackage(new FedExPackageAdapter(shipment, package, 1));

            Assert.Contains(package, shipment.FedEx.Packages);
        }

        [Fact]
        public void DeletePackage_DoesNotRemovePackage_WhenPackageDoesNotExist()
        {
            var package = new FedExPackageEntity(2);
            var package2 = new FedExPackageEntity(2);

            shipment.FedEx.Packages.Add(package);
            shipment.FedEx.Packages.Add(package2);

            var testObject = new FedExShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            testObject.DeletePackage(new FedExPackageAdapter(shipment, new FedExPackageEntity(12), 1));

            Assert.Contains(package, shipment.FedEx.Packages);
            Assert.Contains(package2, shipment.FedEx.Packages);
        }

        [Fact]
        public void DeletePackage_DelegatesToShipmentType_WhenPackageIsRemoved()
        {
            var package = new FedExPackageEntity(2);

            shipment.FedEx.Packages.Add(package);
            shipment.FedEx.Packages.Add(new FedExPackageEntity(3));

            var testObject = new FedExShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            testObject.DeletePackage(new FedExPackageAdapter(shipment, package, 1));

            shipmentTypeMock.Verify(x => x.UpdateDynamicShipmentData(shipment));
            shipmentTypeMock.Verify(x => x.UpdateTotalWeight(shipment));
        }

        [Fact]
        public void DeletePackage_DelegatesToCustomsManager_WhenPackageIsRemoved()
        {
            var package = new FedExPackageEntity(2);

            shipment.FedEx.Packages.Add(package);
            shipment.FedEx.Packages.Add(new FedExPackageEntity(3));

            var testObject = new FedExShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            testObject.DeletePackage(new FedExPackageAdapter(shipment, package, 1));

            customsManager.Verify(x => x.EnsureCustomsLoaded(new[] { shipment }));
        }

        [Fact]
        public void DeletePackage_AddsPackageToRemovedEntityCollection_WhenPackageIsRemoved()
        {
            var package = new FedExPackageEntity(2);

            shipment.FedEx.Packages.Add(package);
            shipment.FedEx.Packages.Add(new FedExPackageEntity(3));

            var testObject = new FedExShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            testObject.DeletePackage(new FedExPackageAdapter(shipment, package, 1));

            Assert.Contains(package, shipment.FedEx.Packages.RemovedEntitiesTracker.OfType<FedExPackageEntity>());
        }

        [Fact]
        public void DoesRateMatchSelectedService_ReturnsFalse_WhenRateShipmentTypeDoesNotMatch()
        {
            var testObject = mock.Create<FedExShipmentAdapter>(new TypedParameter(typeof(ShipmentEntity), shipment));
            var rate = new RateResult("Foo", "1", 0, 1) { ShipmentType = ShipmentTypeCode.None };

            var result = testObject.DoesRateMatchSelectedService(rate);

            Assert.False(result);
        }

        [Fact]
        public void DoesRateMatchSelectedService_ReturnsFalse_WhenRateServiceAsIntDoesNotMatch()
        {
            var testObject = mock.Create<FedExShipmentAdapter>(new TypedParameter(typeof(ShipmentEntity), shipment));
            var rate = new RateResult("Foo", "1", 0, (int) FedExServiceType.FedExEconomyCanada)
            {
                ShipmentType = ShipmentTypeCode.FedEx
            };

            var result = testObject.DoesRateMatchSelectedService(rate);

            Assert.False(result);
        }

        [Fact]
        public void DoesRateMatchSelectedService_ReturnsFalse_WhenRateServiceAsServiceTypeDoesNotMatch()
        {
            var testObject = mock.Create<FedExShipmentAdapter>(new TypedParameter(typeof(ShipmentEntity), shipment));
            var rate = new RateResult("Foo", "1", 0, FedExServiceType.FedExEconomyCanada)
            {
                ShipmentType = ShipmentTypeCode.FedEx
            };

            var result = testObject.DoesRateMatchSelectedService(rate);

            Assert.False(result);
        }

        [Fact]
        public void DoesRateMatchSelectedService_ReturnsFalse_WhenRateServiceAsRateSelectionDoesNotMatch()
        {
            var testObject = mock.Create<FedExShipmentAdapter>(new TypedParameter(typeof(ShipmentEntity), shipment));
            var rate = new RateResult("Foo", "1", 0, new FedExRateSelection(FedExServiceType.FedExEconomyCanada))
            {
                ShipmentType = ShipmentTypeCode.FedEx
            };

            var result = testObject.DoesRateMatchSelectedService(rate);

            Assert.False(result);
        }

        [Fact]
        public void DoesRateMatchSelectedService_ReturnsFalse_WhenRateTagIsOtherObject()
        {
            var testObject = mock.Create<FedExShipmentAdapter>(new TypedParameter(typeof(ShipmentEntity), shipment));
            var rate = new RateResult("Foo", "1", 0, "NOT A RATE")
            {
                ShipmentType = ShipmentTypeCode.FedEx
            };

            var result = testObject.DoesRateMatchSelectedService(rate);

            Assert.False(result);
        }

        [Fact]
        public void DoesRateMatchSelectedService_ReturnsTrue_WhenRateServiceAsIntMatches()
        {
            var testObject = mock.Create<FedExShipmentAdapter>(new TypedParameter(typeof(ShipmentEntity), shipment));
            var rate = new RateResult("Foo", "1", 0, (int) FedExServiceType.FedEx2DayAM)
            {
                ShipmentType = ShipmentTypeCode.FedEx
            };

            var result = testObject.DoesRateMatchSelectedService(rate);

            Assert.True(result);
        }

        [Fact]
        public void DoesRateMatchSelectedService_ReturnsTrue_WhenRateServiceAsServiceTypeMatches()
        {
            var testObject = mock.Create<FedExShipmentAdapter>(new TypedParameter(typeof(ShipmentEntity), shipment));
            var rate = new RateResult("Foo", "1", 0, FedExServiceType.FedEx2DayAM)
            {
                ShipmentType = ShipmentTypeCode.FedEx
            };

            var result = testObject.DoesRateMatchSelectedService(rate);

            Assert.True(result);
        }

        [Fact]
        public void DoesRateMatchSelectedService_ReturnsTrue_WhenRateServiceAsRateSelectionMatches()
        {
            var testObject = mock.Create<FedExShipmentAdapter>(new TypedParameter(typeof(ShipmentEntity), shipment));
            var rate = new RateResult("Foo", "1", 0, new FedExRateSelection(FedExServiceType.FedEx2DayAM))
            {
                ShipmentType = ShipmentTypeCode.FedEx
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
