using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Autofac;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.iParcel;
using ShipWorks.Shipping.Carriers.iParcel.Enums;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Stores;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.iParcel
{
    [SuppressMessage("SonarLint", "S101:Class names should comply with a naming convention",
        Justification = "Class is names to match iParcel's naming convention")]
    public class iParcelShipmentAdapterTest : IDisposable
    {
        readonly ShipmentEntity shipment;
        private readonly AutoMock mock;

        public iParcelShipmentAdapterTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            shipment = new ShipmentEntity
            {
                ShipmentTypeCode = ShipmentTypeCode.iParcel,
                ShipDate = new DateTime(2015, 11, 24, 10, 10, 10),
                ContentWeight = 1,
                TotalWeight = 1,
                Insurance = true,
                IParcel = new IParcelShipmentEntity()
                {
                    Service = (int) iParcelServiceType.Immediate
                }
            };
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExcpetion_WhenShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new iParcelShipmentAdapter(null, mock.Create<IShipmentTypeManager>(),
                    mock.Create<ICustomsManager>(), mock.Create<IStoreManager>()));
            Assert.Throws<ArgumentNullException>(() =>
                new iParcelShipmentAdapter(new ShipmentEntity(), mock.Create<IShipmentTypeManager>(),
                    mock.Create<ICustomsManager>(), mock.Create<IStoreManager>()));
            Assert.Throws<ArgumentNullException>(() =>
                new iParcelShipmentAdapter(shipment, null,
                    mock.Create<ICustomsManager>(), mock.Create<IStoreManager>()));
            Assert.Throws<ArgumentNullException>(() =>
                new iParcelShipmentAdapter(shipment, mock.Create
                    <IShipmentTypeManager>(), null, mock.Create<IStoreManager>()));
        }

        [Fact]
        public void AccountId_ReturnsShipmentValue()
        {
            shipment.IParcel.IParcelAccountID = 12;
            var testObject = mock.Create<iParcelShipmentAdapter>(TypedParameter.From(shipment));
            Assert.Equal(12, testObject.AccountId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10009238)]
        public void AccountId_StoresSpecifiedValue_WhenValueIsValid(long value)
        {
            var testObject = mock.Create<iParcelShipmentAdapter>(TypedParameter.From(shipment));
            testObject.AccountId = value;
            Assert.Equal(value, shipment.IParcel.IParcelAccountID);
        }

        [Fact]
        public void AccountId_StoresZero_WhenValueIsNull()
        {
            var testObject = mock.Create<iParcelShipmentAdapter>(TypedParameter.From(shipment));
            testObject.AccountId = null;
            Assert.Equal(0, shipment.IParcel.IParcelAccountID);
        }

        [Fact]
        public void Shipment_IsNotNull()
        {
            var testObject = mock.Create<iParcelShipmentAdapter>(TypedParameter.From(shipment));
            Assert.NotNull(testObject.Shipment);
        }

        [Fact]
        public void ShipmentTypeCode_IsIParcel()
        {
            var testObject = mock.Create<iParcelShipmentAdapter>(TypedParameter.From(shipment));
            Assert.Equal(ShipmentTypeCode.iParcel, testObject.ShipmentTypeCode);
        }

        [Fact]
        public void SupportsAccounts_IsTrue()
        {
            var testObject = mock.Create<iParcelShipmentAdapter>(TypedParameter.From(shipment));

            Assert.True(testObject.SupportsAccounts);
        }

        [Fact]
        public void SupportsMultiplePackages_IsTrue()
        {
            mock.WithShipmentTypeFromShipmentManager(x => x.Setup(b => b.SupportsMultiplePackages).Returns(true));

            var testObject = mock.Create<iParcelShipmentAdapter>(TypedParameter.From(shipment));

            Assert.True(testObject.SupportsMultiplePackages);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void SupportsMultiplePackages_DomesticIsTrue_WhenShipCountryIsUs(bool isDomestic, bool expected)
        {
            mock.WithShipmentTypeFromShipmentManager(x =>
                x.Setup(b => b.IsDomestic(It.IsAny<ShipmentEntity>())).Returns(isDomestic));

            var testObject = mock.Create<iParcelShipmentAdapter>(TypedParameter.From(shipment));

            Assert.Equal(expected, testObject.IsDomestic);
        }

        [Fact]
        public void SupportsMultiplePackages_DomesticIsFalse_WhenShipCountryIsCa()
        {
            shipment.OriginCountryCode = "US";
            shipment.ShipCountryCode = "CA";

            var testObject = mock.Create<iParcelShipmentAdapter>(TypedParameter.From(shipment));
            Assert.False(testObject.IsDomestic);
        }

        [Fact]
        public void UpdateDynamicData_DelegatesToShipmentTypeAndCustomsManager()
        {
            var shipmentType = mock.WithShipmentTypeFromShipmentManager();

            var testObject = mock.Create<iParcelShipmentAdapter>(TypedParameter.From(shipment));
            testObject.UpdateDynamicData();

            shipmentType.Verify(b => b.UpdateDynamicShipmentData(shipment));
            shipmentType.Verify(b => b.UpdateTotalWeight(shipment));
            mock.Mock<ICustomsManager>().Verify(x => x.EnsureCustomsLoaded(new[] { shipment }));
        }

        [Fact]
        public void UpdateDynamicData_ErrorsReturned_AreCorrect()
        {
            Dictionary<ShipmentEntity, Exception> errors = new Dictionary<ShipmentEntity, Exception>();
            errors.Add(shipment, new Exception("test"));

            mock.Mock<ICustomsManager>()
                .Setup(c => c.EnsureCustomsLoaded(It.IsAny<IEnumerable<ShipmentEntity>>())).Returns(errors);

            var testObject = mock.Create<iParcelShipmentAdapter>(TypedParameter.From(shipment));

            Assert.NotNull(testObject.UpdateDynamicData());
            Assert.Equal(1, testObject.UpdateDynamicData().Count);
        }

        [Fact]
        public void SupportsPackageTypes_IsFalse()
        {
            var testObject = mock.Create<iParcelShipmentAdapter>(TypedParameter.From(shipment));

            Assert.False(testObject.SupportsPackageTypes);
        }

        [Fact]
        public void ShipDate_ReturnsShipmentValue()
        {
            var testObject = mock.Create<iParcelShipmentAdapter>(TypedParameter.From(shipment));
            Assert.Equal(shipment.ShipDate, testObject.ShipDate);
        }

        [Fact]
        public void ShipDate_IsUpdated()
        {
            var testObject = mock.Create<iParcelShipmentAdapter>(TypedParameter.From(shipment));

            testObject.ShipDate = testObject.ShipDate.AddDays(1);

            Assert.Equal(shipment.ShipDate, testObject.ShipDate);
        }

        [Fact]
        public void ServiceType_ReturnsShipmentValue()
        {
            var testObject = mock.Create<iParcelShipmentAdapter>(TypedParameter.From(shipment));
            Assert.Equal(shipment.IParcel.Service, testObject.ServiceType);
        }

        [Fact]
        public void ServiceType_IsUpdated()
        {
            var testObject = mock.Create<iParcelShipmentAdapter>(TypedParameter.From(shipment));

            shipment.IParcel.Service = (int) iParcelServiceType.Immediate;
            testObject.ServiceType = (int) iParcelServiceType.Preferred;

            Assert.Equal(shipment.IParcel.Service, testObject.ServiceType);
        }

        [Theory]
        [InlineData(iParcelServiceType.Immediate)]
        [InlineData(iParcelServiceType.Preferred)]
        [InlineData(iParcelServiceType.SaverDeferred)]
        public void UpdateServiceFromRate_SetsService_WhenTagIsValid(iParcelServiceType serviceType)
        {
            mock.WithShipmentTypeFromShipmentManager(x =>
            {
                x.Setup(b => b.SupportsGetRates).Returns(true);
                x.Setup(b => b.ShipmentTypeCode).Returns(ShipmentTypeCode.iParcel);
            });

            var testObject = mock.Create<iParcelShipmentAdapter>(TypedParameter.From(shipment));

            testObject.SelectServiceFromRate(new RateResult("Foo", "1", 1M, (int) serviceType)
            {
                Selectable = true,
                ShipmentType = ShipmentTypeCode.iParcel
            });

            Assert.Equal((int) serviceType, shipment.IParcel.Service);
        }

        [Theory]
        [InlineData(iParcelServiceType.Immediate)]
        [InlineData(iParcelServiceType.Preferred)]
        [InlineData(iParcelServiceType.SaverDeferred)]
        public void UpdateServiceFromRate_SetsService_WhenTagIsValidServiceType(iParcelServiceType serviceType)
        {
            mock.WithShipmentTypeFromShipmentManager(x =>
            {
                x.Setup(b => b.SupportsGetRates).Returns(true);
                x.Setup(b => b.ShipmentTypeCode).Returns(ShipmentTypeCode.iParcel);
            });
            var testObject = mock.Create<iParcelShipmentAdapter>(TypedParameter.From(shipment));

            testObject.SelectServiceFromRate(new RateResult("Foo", "1", 1M, serviceType)
            {
                Selectable = true,
                ShipmentType = ShipmentTypeCode.iParcel
            });

            Assert.Equal((int) serviceType, shipment.IParcel.Service);
        }

        [Theory]
        [InlineData(iParcelServiceType.Immediate)]
        [InlineData(iParcelServiceType.Preferred)]
        [InlineData(iParcelServiceType.SaverDeferred)]
        public void UpdateServiceFromRate_SetsService_WhenTagIsValidRateSelection(iParcelServiceType serviceType)
        {
            mock.WithShipmentTypeFromShipmentManager(x =>
            {
                x.Setup(b => b.SupportsGetRates).Returns(true);
                x.Setup(b => b.ShipmentTypeCode).Returns(ShipmentTypeCode.iParcel);
            });
            var testObject = mock.Create<iParcelShipmentAdapter>(TypedParameter.From(shipment));

            testObject.SelectServiceFromRate(new RateResult("Foo", "1", 1M, new iParcelRateSelection(serviceType))
            {
                Selectable = true,
                ShipmentType = ShipmentTypeCode.iParcel
            });

            Assert.Equal((int) serviceType, shipment.IParcel.Service);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("Foo")]
        public void UpdateServiceFromRate_DoesNotSetService_WhenTagIsNotValid(string value)
        {
            mock.WithShipmentTypeFromShipmentManager(x =>
            {
                x.Setup(b => b.SupportsGetRates).Returns(true);
                x.Setup(b => b.ShipmentTypeCode).Returns(ShipmentTypeCode.iParcel);
            });
            shipment.IParcel.Service = (int) iParcelServiceType.SaverDeferred;
            var testObject = mock.Create<iParcelShipmentAdapter>(TypedParameter.From(shipment));

            testObject.SelectServiceFromRate(new RateResult("Foo", "1", 1M, value)
            {
                Selectable = true,
                ShipmentType = ShipmentTypeCode.iParcel
            });

            Assert.Equal((int) iParcelServiceType.SaverDeferred, shipment.IParcel.Service);
        }

        [Fact]
        public void AddPackage_AddsNewPackageToList()
        {
            var testObject = mock.Create<iParcelShipmentAdapter>(TypedParameter.From(shipment));
            testObject.AddPackage();

            Assert.Equal(1, shipment.IParcel.Packages.Count);
        }

        [Fact]
        public void AddPackage_ReturnsPackageAdapter_ForNewPackage()
        {
            var testObject = mock.Create<iParcelShipmentAdapter>(TypedParameter.From(shipment));
            var newPackage = testObject.AddPackage();

            Assert.NotNull(newPackage);
        }

        [Fact]
        public void AddPackage_DelegatesToShipmentType_WhenNewPackageIsAdded()
        {
            Mock<ShipmentType> shipmentType = mock.WithShipmentTypeFromShipmentManager();

            var testObject = mock.Create<iParcelShipmentAdapter>(TypedParameter.From(shipment));
            testObject.AddPackage();

            shipmentType.Verify(b => b.UpdateDynamicShipmentData(shipment));
            shipmentType.Verify(b => b.UpdateTotalWeight(shipment));
        }

        [Fact]
        public void AddPackage_DelegatesToCustomsManager_WhenNewPackageIsAdded()
        {
            var testObject = mock.Create<iParcelShipmentAdapter>(TypedParameter.From(shipment));
            testObject.AddPackage();

            mock.Mock<ICustomsManager>().Verify(x => x.EnsureCustomsLoaded(new[] { shipment }));
        }

        [Fact]
        public void DeletePackage_RemovesPackageAssociatedWithAdapter()
        {
            var package = new IParcelPackageEntity(2);

            shipment.IParcel.Packages.Add(new IParcelPackageEntity(1));
            shipment.IParcel.Packages.Add(package);
            shipment.IParcel.Packages.Add(new IParcelPackageEntity(3));

            var testObject = mock.Create<iParcelShipmentAdapter>(TypedParameter.From(shipment));
            testObject.DeletePackage(new iParcelPackageAdapter(shipment, package, 1));

            Assert.DoesNotContain(package, shipment.IParcel.Packages);
        }

        [Fact]
        public void DeletePackage_DoesNotRemovePackage_WhenShipmentHasSinglePackage()
        {
            var package = new IParcelPackageEntity(2);

            shipment.IParcel.Packages.Add(package);

            var testObject = mock.Create<iParcelShipmentAdapter>(TypedParameter.From(shipment));
            testObject.DeletePackage(new iParcelPackageAdapter(shipment, package, 1));

            Assert.Contains(package, shipment.IParcel.Packages);
        }

        [Fact]
        public void DeletePackage_DoesNotRemovePackage_WhenPackageDoesNotExist()
        {
            var package = new IParcelPackageEntity(2);
            var package2 = new IParcelPackageEntity(2);

            shipment.IParcel.Packages.Add(package);
            shipment.IParcel.Packages.Add(package2);

            var testObject = mock.Create<iParcelShipmentAdapter>(TypedParameter.From(shipment));
            testObject.DeletePackage(new iParcelPackageAdapter(shipment, new IParcelPackageEntity(12), 1));

            Assert.Contains(package, shipment.IParcel.Packages);
            Assert.Contains(package2, shipment.IParcel.Packages);
        }

        [Fact]
        public void DeletePackage_DelegatesToShipmentType_WhenPackageIsRemoved()
        {
            var package = new IParcelPackageEntity(2);

            shipment.IParcel.Packages.Add(package);
            shipment.IParcel.Packages.Add(new IParcelPackageEntity(3));

            var shipmentType = mock.WithShipmentTypeFromShipmentManager(x =>
            {
                x.SetupGet(b => b.SupportsMultiplePackages).Returns(true);
            });

            var testObject = mock.Create<iParcelShipmentAdapter>(TypedParameter.From(shipment));
            testObject.DeletePackage(new iParcelPackageAdapter(shipment, package, 1));

            shipmentType.Verify(b => b.UpdateDynamicShipmentData(shipment));
            shipmentType.Verify(b => b.UpdateTotalWeight(shipment));
        }

        [Fact]
        public void DeletePackage_DelegatesToCustomsManager_WhenPackageIsRemoved()
        {
            var package = new IParcelPackageEntity(2);

            shipment.IParcel.Packages.Add(package);
            shipment.IParcel.Packages.Add(new IParcelPackageEntity(3));

            var testObject = mock.Create<iParcelShipmentAdapter>(TypedParameter.From(shipment));
            testObject.DeletePackage(new iParcelPackageAdapter(shipment, package, 1));

            mock.Mock<ICustomsManager>().Verify(x => x.EnsureCustomsLoaded(new[] { shipment }));
        }

        [Fact]
        public void DeletePackage_AddsPackageToRemovedEntityCollection_WhenPackageIsRemoved()
        {
            var package = new IParcelPackageEntity(2);

            shipment.IParcel.Packages.Add(package);
            shipment.IParcel.Packages.Add(new IParcelPackageEntity(3));

            var testObject = mock.Create<iParcelShipmentAdapter>(TypedParameter.From(shipment));
            testObject.DeletePackage(new iParcelPackageAdapter(shipment, package, 1));

            Assert.Contains(package, shipment.IParcel.Packages.RemovedEntitiesTracker.OfType<IParcelPackageEntity>());
        }

        [Fact]
        public void DoesRateMatchSelectedService_ReturnsFalse_WhenRateShipmentTypeDoesNotMatch()
        {
            var testObject = mock.Create<iParcelShipmentAdapter>(new TypedParameter(typeof(ShipmentEntity), shipment));
            var rate = new RateResult("Foo", "1", 0, 1) { ShipmentType = ShipmentTypeCode.None };

            var result = testObject.DoesRateMatchSelectedService(rate);

            Assert.False(result);
        }

        [Fact]
        public void DoesRateMatchSelectedService_ReturnsFalse_WhenRateServiceAsIntDoesNotMatch()
        {
            var testObject = mock.Create<iParcelShipmentAdapter>(new TypedParameter(typeof(ShipmentEntity), shipment));
            var rate = new RateResult("Foo", "1", 0, (int) iParcelServiceType.Saver)
            {
                ShipmentType = ShipmentTypeCode.iParcel
            };

            var result = testObject.DoesRateMatchSelectedService(rate);

            Assert.False(result);
        }

        [Fact]
        public void DoesRateMatchSelectedService_ReturnsFalse_WhenRateServiceAsServiceTypeDoesNotMatch()
        {
            var testObject = mock.Create<iParcelShipmentAdapter>(new TypedParameter(typeof(ShipmentEntity), shipment));
            var rate = new RateResult("Foo", "1", 0, iParcelServiceType.Saver)
            {
                ShipmentType = ShipmentTypeCode.iParcel
            };

            var result = testObject.DoesRateMatchSelectedService(rate);

            Assert.False(result);
        }

        [Fact]
        public void DoesRateMatchSelectedService_ReturnsFalse_WhenRateServiceAsRateSelectionDoesNotMatch()
        {
            var testObject = mock.Create<iParcelShipmentAdapter>(new TypedParameter(typeof(ShipmentEntity), shipment));
            var rate = new RateResult("Foo", "1", 0, new iParcelRateSelection(iParcelServiceType.Saver))
            {
                ShipmentType = ShipmentTypeCode.iParcel
            };

            var result = testObject.DoesRateMatchSelectedService(rate);

            Assert.False(result);
        }

        [Fact]
        public void DoesRateMatchSelectedService_ReturnsFalse_WhenRateTagIsOtherObject()
        {
            var testObject = mock.Create<iParcelShipmentAdapter>(new TypedParameter(typeof(ShipmentEntity), shipment));
            var rate = new RateResult("Foo", "1", 0, "NOT A RATE")
            {
                ShipmentType = ShipmentTypeCode.iParcel
            };

            var result = testObject.DoesRateMatchSelectedService(rate);

            Assert.False(result);
        }

        [Fact]
        public void DoesRateMatchSelectedService_ReturnsTrue_WhenRateServiceAsIntMatches()
        {
            var testObject = mock.Create<iParcelShipmentAdapter>(new TypedParameter(typeof(ShipmentEntity), shipment));
            var rate = new RateResult("Foo", "1", 0, (int) iParcelServiceType.Immediate)
            {
                ShipmentType = ShipmentTypeCode.iParcel
            };

            var result = testObject.DoesRateMatchSelectedService(rate);

            Assert.True(result);
        }

        [Fact]
        public void DoesRateMatchSelectedService_ReturnsTrue_WhenRateServiceAsServiceTypeMatches()
        {
            var testObject = mock.Create<iParcelShipmentAdapter>(new TypedParameter(typeof(ShipmentEntity), shipment));
            var rate = new RateResult("Foo", "1", 0, iParcelServiceType.Immediate)
            {
                ShipmentType = ShipmentTypeCode.iParcel
            };

            var result = testObject.DoesRateMatchSelectedService(rate);

            Assert.True(result);
        }

        [Fact]
        public void DoesRateMatchSelectedService_ReturnsTrue_WhenRateServiceAsRateSelectionMatches()
        {
            var testObject = mock.Create<iParcelShipmentAdapter>(new TypedParameter(typeof(ShipmentEntity), shipment));
            var rate = new RateResult("Foo", "1", 0, new iParcelRateSelection(iParcelServiceType.Immediate))
            {
                ShipmentType = ShipmentTypeCode.iParcel
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
