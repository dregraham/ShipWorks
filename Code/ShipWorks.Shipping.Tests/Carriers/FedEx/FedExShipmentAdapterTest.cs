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
using ShipWorks.Stores;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx
{
    public class FedExShipmentAdapterTest : IDisposable
    {
        readonly AutoMock mock;
        readonly ShipmentEntity shipment;
        readonly Mock<ShipmentType> shipmentType;

        public FedExShipmentAdapterTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
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
            shipmentType = mock.WithShipmentTypeFromShipmentManager(x =>
            {
                x.Setup(b => b.SupportsGetRates).Returns(true);
                x.Setup(b => b.SupportsMultiplePackages).Returns(true);
                x.Setup(b => b.ShipmentTypeCode).Returns(ShipmentTypeCode.FedEx);
            });
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExcpetion_WhenShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new FedExShipmentAdapter(null, mock.Create<IShipmentTypeManager>(),
                    mock.Create<ICustomsManager>(), mock.Create<IStoreManager>()));
            Assert.Throws<ArgumentNullException>(() =>
                new FedExShipmentAdapter(new ShipmentEntity(), mock.Create<IShipmentTypeManager>(),
                    mock.Create<ICustomsManager>(), mock.Create<IStoreManager>()));
            Assert.Throws<ArgumentNullException>(() =>
                new FedExShipmentAdapter(shipment, null,
                    mock.Create<ICustomsManager>(), mock.Create<IStoreManager>()));
            Assert.Throws<ArgumentNullException>(() =>
                new FedExShipmentAdapter(shipment, mock.
                    Create<IShipmentTypeManager>(), null, mock.Create<IStoreManager>()));
        }

        [Fact]
        public void AccountId_ReturnsShipmentValue()
        {
            shipment.FedEx.FedExAccountID = 12;
            var testObject = mock.Create<FedExShipmentAdapter>(TypedParameter.From(shipment));
            Assert.Equal(12, testObject.AccountId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10009238)]
        public void AccountId_StoresSpecifiedValue_WhenValueIsValid(long value)
        {
            var testObject = mock.Create<FedExShipmentAdapter>(TypedParameter.From(shipment));
            testObject.AccountId = value;
            Assert.Equal(value, shipment.FedEx.FedExAccountID);
        }

        [Fact]
        public void AccountId_StoresZero_WhenValueIsNull()
        {
            var testObject = mock.Create<FedExShipmentAdapter>(TypedParameter.From(shipment));
            testObject.AccountId = null;
            Assert.Equal(0, shipment.FedEx.FedExAccountID);
        }

        [Fact]
        public void Shipment_IsNotNull()
        {
            var testObject = mock.Create<FedExShipmentAdapter>(TypedParameter.From(shipment));
            Assert.NotNull(testObject.Shipment);
        }

        [Fact]
        public void ShipmentTypeCode_IsFedEx()
        {
            var testObject = mock.Create<FedExShipmentAdapter>(TypedParameter.From(shipment));
            Assert.Equal(ShipmentTypeCode.FedEx, testObject.ShipmentTypeCode);
        }

        [Fact]
        public void SupportsAccounts_IsTrue()
        {
            var testObject = mock.Create<FedExShipmentAdapter>(TypedParameter.From(shipment));

            Assert.True(testObject.SupportsAccounts);
        }

        [Fact]
        public void SupportsMultiplePackages_IsTrue()
        {
            var testObject = mock.Create<FedExShipmentAdapter>(TypedParameter.From(shipment));
            Assert.True(testObject.SupportsMultiplePackages);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, true)]
        public void IsDomestic_DelegatesToIsDomestic_OnShipmentType(bool isDomestic, bool expected)
        {
            shipmentType.Setup(b => b.IsDomestic(shipment)).Returns(isDomestic);

            var testObject = mock.Create<FedExShipmentAdapter>(TypedParameter.From(shipment));

            Assert.Equal(expected, testObject.IsDomestic);
        }

        [Fact]
        public void UpdateDynamicData_DelegatesToShipmentTypeAndCustomsManager()
        {
            var testObject = mock.Create<FedExShipmentAdapter>(TypedParameter.From(shipment));
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

            var testObject = mock.Create<FedExShipmentAdapter>(TypedParameter.From(shipment));

            Assert.NotNull(testObject.UpdateDynamicData());
            Assert.Equal(1, testObject.UpdateDynamicData().Count);
        }

        [Fact]
        public void SupportsPackageTypes_IsTrue()
        {
            var testObject = mock.Create<FedExShipmentAdapter>(TypedParameter.From(shipment));

            Assert.True(testObject.SupportsPackageTypes);
        }

        [Fact]
        public void ShipDate_ReturnsShipmentValue()
        {
            var testObject = mock.Create<FedExShipmentAdapter>(TypedParameter.From(shipment));
            Assert.Equal(shipment.ShipDate, testObject.ShipDate);
        }

        [Fact]
        public void ShipDate_IsUpdated()
        {
            var testObject = mock.Create<FedExShipmentAdapter>(TypedParameter.From(shipment));

            testObject.ShipDate = testObject.ShipDate.AddDays(1);

            Assert.Equal(shipment.ShipDate, testObject.ShipDate);
        }

        [Fact]
        public void ServiceType_ReturnsShipmentValue()
        {
            var testObject = mock.Create<FedExShipmentAdapter>(TypedParameter.From(shipment));
            Assert.Equal(shipment.FedEx.Service, testObject.ServiceType);
        }

        [Fact]
        public void ServiceType_IsUpdated()
        {
            var testObject = mock.Create<FedExShipmentAdapter>(TypedParameter.From(shipment));

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
            var testObject = mock.Create<FedExShipmentAdapter>(TypedParameter.From(shipment));
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
            var testObject = mock.Create<FedExShipmentAdapter>(TypedParameter.From(shipment));
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
            var testObject = mock.Create<FedExShipmentAdapter>(TypedParameter.From(shipment));
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
            shipment.FedEx.Service = (int) FedExServiceType.GroundHomeDelivery;
            var testObject = mock.Create<FedExShipmentAdapter>(TypedParameter.From(shipment));
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
            var testObject = mock.Create<FedExShipmentAdapter>(TypedParameter.From(shipment));
            testObject.AddPackage();

            Assert.Equal(1, shipment.FedEx.Packages.Count);
        }

        [Fact]
        public void AddPackage_ReturnsPackageAdapter_ForNewPackage()
        {
            var testObject = mock.Create<FedExShipmentAdapter>(TypedParameter.From(shipment));
            var newPackage = testObject.AddPackage();

            Assert.NotNull(newPackage);
        }

        [Fact]
        public void AddPackage_DelegatesToShipmentType_WhenNewPackageIsAdded()
        {
            var testObject = mock.Create<FedExShipmentAdapter>(TypedParameter.From(shipment));
            testObject.AddPackage();

            shipmentType.Verify(x => x.UpdateDynamicShipmentData(shipment));
            shipmentType.Verify(x => x.UpdateTotalWeight(shipment));
        }

        [Fact]
        public void AddPackage_DelegatesToCustomsManager_WhenNewPackageIsAdded()
        {
            var testObject = mock.Create<FedExShipmentAdapter>(TypedParameter.From(shipment));
            testObject.AddPackage();

            mock.Mock<ICustomsManager>().Verify(x => x.EnsureCustomsLoaded(new[] { shipment }));
        }

        [Fact]
        public void DeletePackage_RemovesPackageAssociatedWithAdapter()
        {
            var package = new FedExPackageEntity(2);

            shipment.FedEx.Packages.Add(new FedExPackageEntity(1));
            shipment.FedEx.Packages.Add(package);
            shipment.FedEx.Packages.Add(new FedExPackageEntity(3));

            var testObject = mock.Create<FedExShipmentAdapter>(TypedParameter.From(shipment));
            testObject.DeletePackage(new FedExPackageAdapter(shipment, package, 1));

            Assert.DoesNotContain(package, shipment.FedEx.Packages);
        }

        [Fact]
        public void DeletePackage_DoesNotRemovePackage_WhenShipmentHasSinglePackage()
        {
            var package = new FedExPackageEntity(2);

            shipment.FedEx.Packages.Add(package);

            var testObject = mock.Create<FedExShipmentAdapter>(TypedParameter.From(shipment));
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

            var testObject = mock.Create<FedExShipmentAdapter>(TypedParameter.From(shipment));
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

            var testObject = mock.Create<FedExShipmentAdapter>(TypedParameter.From(shipment));
            testObject.DeletePackage(new FedExPackageAdapter(shipment, package, 1));

            shipmentType.Verify(x => x.UpdateDynamicShipmentData(shipment));
            shipmentType.Verify(x => x.UpdateTotalWeight(shipment));
        }

        [Fact]
        public void DeletePackage_DelegatesToCustomsManager_WhenPackageIsRemoved()
        {
            var package = new FedExPackageEntity(2);

            shipment.FedEx.Packages.Add(package);
            shipment.FedEx.Packages.Add(new FedExPackageEntity(3));

            var testObject = mock.Create<FedExShipmentAdapter>(TypedParameter.From(shipment));
            testObject.DeletePackage(new FedExPackageAdapter(shipment, package, 1));

            mock.Mock<ICustomsManager>().Verify(x => x.EnsureCustomsLoaded(new[] { shipment }));
        }

        [Fact]
        public void DeletePackage_AddsPackageToRemovedEntityCollection_WhenPackageIsRemoved()
        {
            var package = new FedExPackageEntity(2);

            shipment.FedEx.Packages.Add(package);
            shipment.FedEx.Packages.Add(new FedExPackageEntity(3));

            var testObject = mock.Create<FedExShipmentAdapter>(TypedParameter.From(shipment));
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
