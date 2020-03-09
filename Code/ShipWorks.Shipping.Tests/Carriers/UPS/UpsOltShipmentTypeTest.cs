using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Tests.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.UPS.ShipEngine;
using ShipWorks.Shipping.Settings;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Ups
{
    public class UpsOltShipmentTypeTest : IDisposable
    {
        private readonly UpsOltShipmentType testObject;
        readonly AutoMock mock;

        public UpsOltShipmentTypeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<UpsOltShipmentType>();
        }

        [Fact]
        public void SupportsMultiplePackages_ReturnsTrue()
        {
            Assert.True(testObject.SupportsMultiplePackages);
        }

        [Fact]
        public void ShipmentTypeCode_ReturnsUps()
        {
            Assert.Equal(ShipmentTypeCode.UpsOnLineTools, testObject.ShipmentTypeCode);
        }

        [Fact]
        public void GetPackageAdapters_ReturnsPackageAdapterPerPackage()
        {
            var shipment = new ShipmentEntity();
            var UpsShipment = new UpsShipmentEntity();
            UpsShipment.Packages.Add(new UpsPackageEntity());
            UpsShipment.Packages.Add(new UpsPackageEntity());
            UpsShipment.Packages.Add(new UpsPackageEntity());

            shipment.Ups = UpsShipment;
            
            Assert.Equal(3, testObject.GetPackageAdapters(shipment).Count());
        }
        
        [Fact]
        public void RedistributeContentWeight_SetsPackageWeightToShipmentWeightDividedByPackageCount()
        {
            var shipment = new ShipmentEntity();
            shipment.ContentWeight = 123;
            var UpsShipment = new UpsShipmentEntity();
            UpsShipment.Packages.Add(new UpsPackageEntity());
            UpsShipment.Packages.Add(new UpsPackageEntity());
            UpsShipment.Packages.Add(new UpsPackageEntity());

            shipment.Ups = UpsShipment;

            Assert.True(UpsOltShipmentType.RedistributeContentWeight(shipment));
            Assert.Equal(41, shipment.Ups.Packages[0].Weight);
        }


        [Fact]
        public void RedistributeContentWeight_DoesNothingWhenPackageWeightAndShipmentWeightMatch()
        {
            var shipment = new ShipmentEntity();
            shipment.ContentWeight = 123;
            var UpsShipment = new UpsShipmentEntity();
            UpsShipment.Packages.Add(new UpsPackageEntity() { Weight = 41 });
            UpsShipment.Packages.Add(new UpsPackageEntity() { Weight = 41 });
            UpsShipment.Packages.Add(new UpsPackageEntity() { Weight = 41 });

            shipment.Ups = UpsShipment;

            Assert.False(UpsOltShipmentType.RedistributeContentWeight(shipment));
        }

        [Fact]
        public void UpdateTotalWeight_SetsShipmentContentWeight()
        {
            var shipment = new ShipmentEntity();
            shipment.ContentWeight = 123;
            var UpsShipment = new UpsShipmentEntity();
            UpsShipment.Packages.Add(new UpsPackageEntity() { Weight = 2 });
            UpsShipment.Packages.Add(new UpsPackageEntity() { Weight = 3 });
            UpsShipment.Packages.Add(new UpsPackageEntity() { Weight = 4 });

            shipment.Ups = UpsShipment;

            testObject.UpdateTotalWeight(shipment);

            Assert.Equal(9, shipment.ContentWeight);
        }

        [Fact]
        public void UpdateTotalWeight_SetsShipmentTotalWeight()
        {
            var shipment = new ShipmentEntity();
            shipment.ContentWeight = 123;
            var UpsShipment = new UpsShipmentEntity();
            UpsShipment.Packages.Add(new UpsPackageEntity() { Weight = 2 });
            UpsShipment.Packages.Add(new UpsPackageEntity() { Weight = 3 });
            UpsShipment.Packages.Add(new UpsPackageEntity() { Weight = 4, DimsAddWeight = true, DimsWeight = 3 });

            shipment.Ups = UpsShipment;

            testObject.UpdateTotalWeight(shipment);

            Assert.Equal(12, shipment.TotalWeight);
        }

        [Fact]
        public void GetParcelCount_ReturnsParcelCount()
        {
            var shipment = new ShipmentEntity();
            shipment.ContentWeight = 123;
            var UpsShipment = new UpsShipmentEntity();
            UpsShipment.Packages.Add(new UpsPackageEntity() { Weight = 2 });
            UpsShipment.Packages.Add(new UpsPackageEntity() { Weight = 3 });
            UpsShipment.Packages.Add(new UpsPackageEntity() { Weight = 4, DimsAddWeight = true, DimsWeight = 3 });

            shipment.Ups = UpsShipment;
            
            Assert.Equal(3,testObject.GetParcelCount(shipment));
        }
        
        [Theory]
        [InlineData(true, 1.1, 1.2, 2.3)]
        [InlineData(false, 1.1, 1.2, 1.2)]
        [InlineData(true, 0, 1.2, 1.2)]
        [InlineData(false, 0, 1.2, 1.2)]
        public void GetParcelDetail_HasCorrectTotalWeight(bool dimsAddWeight, double dimsWeight, double weight, double expectedTotalWeight)
        {
            var testObject = mock.Create<UpsOltShipmentType>();

            var shipment = new ShipmentEntity()
            {
                TrackingNumber = "test",
                Ups = new UpsShipmentEntity()
            };
            shipment.Ups.Packages.Add(new UpsPackageEntity()
            {
                DimsAddWeight = dimsAddWeight,
                DimsWeight = dimsWeight,
                Weight = weight
            });

            var parcelDetail = testObject.GetParcelDetail(shipment, 0);

            Assert.Equal(expectedTotalWeight, parcelDetail.TotalWeight);
        }

        [Fact]
        public void GetAvailableServiceTypes_ReturnsAllServices_WhenUserHasNonShipEngineAccountAndNoExcludedServices()
        {
            IEnumerable<IUpsAccountEntity> accounts = new[]
            {
                new UpsAccountEntity(),
                new UpsAccountEntity {ShipEngineCarrierId = "foo"}
            };

            var availableServices = ExecuteGetAvailableServiceTypes(accounts, new List<ExcludedServiceTypeEntity>())
                .ToArray();

            var expectedServices = Enum.GetValues(typeof(UpsServiceType)).Cast<int>().ToArray();

            Assert.Equal(expectedServices, availableServices);
        }

        [Fact]
        public void GetAvailableServiceTypes_ReturnsServicesThatAreNotExcluded_WhenUserHasNonShipEngineAccountAndThereAreExcludedServiceTypes()
        {
            IEnumerable<IUpsAccountEntity> accounts = new[]
            {
                new UpsAccountEntity(),
                new UpsAccountEntity {ShipEngineCarrierId = "foo"}
            };

            var excludedServiceTypes = new List<ExcludedServiceTypeEntity>()
            {
                new ExcludedServiceTypeEntity((int) ShipmentTypeCode.UpsOnLineTools, (int) UpsServiceType.UpsGround)
            };

            var availableServices = ExecuteGetAvailableServiceTypes(accounts, excludedServiceTypes).ToArray();

            var expectedServices = Enum.GetValues(typeof(UpsServiceType)).Cast<int>()
                .Except(new[] {excludedServiceTypes[0].ServiceType}).ToArray();

            Assert.Equal(expectedServices, availableServices);
        }

        [Fact]
        public void GetAvailableServiceTypes_ReturnsServicesThatAreSupportedByShipEngine_WhenUserOnlyHasShipEngineUpsAccounts()
        {
            IEnumerable<IUpsAccountEntity> accounts = new[]
            {
                new UpsAccountEntity {ShipEngineCarrierId = "foo"}
            };

            var availableServices = ExecuteGetAvailableServiceTypes(accounts, new List<ExcludedServiceTypeEntity>())
                .OrderBy(x => x)
                .ToArray();

            var expectedServices = UpsShipEngineServiceTypeUtility.GetSupportedServices().Cast<int>()
                .OrderBy(x => x)
                .ToArray();

            Assert.Equal(expectedServices, availableServices);
        }

        [Fact]
        public void GetAvailableServiceTypes_ReturnsNonExcludedServicesSupportedByShipEngine_WhenAllShipEngineAccountsAndThereAreExcludedServiceTypes()
        {
            IEnumerable<IUpsAccountEntity> accounts = new[]
            {
                new UpsAccountEntity {ShipEngineCarrierId = "foo"}
            };

            var excludedServiceTypes = new List<ExcludedServiceTypeEntity>
            {
                new ExcludedServiceTypeEntity((int) ShipmentTypeCode.UpsOnLineTools, (int) UpsServiceType.UpsGround)
            };

            var availableServices = ExecuteGetAvailableServiceTypes(accounts, excludedServiceTypes)
                .OrderBy(x => x)
                .ToArray();

            var expectedServices = UpsShipEngineServiceTypeUtility.GetSupportedServices().Cast<int>()
                .Except(new[] {excludedServiceTypes[0].ServiceType})
                .OrderBy(x => x)
                .ToArray();

            Assert.Equal(expectedServices, availableServices);
        }

        private IEnumerable<int> ExecuteGetAvailableServiceTypes(IEnumerable<IUpsAccountEntity> accounts,
                                                                 List<ExcludedServiceTypeEntity> excludedServices)
        {
            var accountRepo = mock.Mock<ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity>>();
            accountRepo.Setup(r => r.AccountsReadOnly).Returns(accounts);

            testObject.AccountRepository = accountRepo.Object;

            var excludedServiceTypeRepo = mock.Mock<IExcludedServiceTypeRepository>();
            excludedServiceTypeRepo.Setup(x => x.GetExcludedServiceTypes(It.IsAny<ShipmentType>()))
                .Returns(excludedServices);

            return testObject.GetAvailableServiceTypes(excludedServiceTypeRepo.Object);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
