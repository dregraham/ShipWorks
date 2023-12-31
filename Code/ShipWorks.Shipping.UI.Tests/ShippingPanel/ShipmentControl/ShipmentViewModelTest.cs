﻿using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Interapptive.Shared.IO.Hardware.Scales;
using Moq;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Services.Builders;
using ShipWorks.Shipping.UI.ShippingPanel.ShipmentControl;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.ShippingPanel.ShipmentControl
{
    public class ShipmentViewModelTest : IDisposable
    {
        private readonly ShipmentEntity shipment = new ShipmentEntity();
        private Mock<ICarrierShipmentAdapter> shipmentAdapter;
        private Mock<IShipmentServicesBuilder> shipmentServicesBuilder;
        private Mock<IShipmentServicesBuilderFactory> shipmentServicesBuilderFactory;
        private Mock<IShipmentPackageTypesBuilder> shipmentPackageTypesBuilder;
        private Mock<IShipmentPackageTypesBuilderFactory> shipmentPackageTypesBuilderFactory;
        private readonly Mock<IDimensionsManager> dimensionsManager;
        private Dictionary<int, string> expectedServices = new Dictionary<int, string>();
        private Dictionary<int, string> expectedPackageTypes = new Dictionary<int, string>();
        private readonly List<IPackageAdapter> packageAdapters = new List<IPackageAdapter>();
        private readonly AutoMock mock;
        private TestMessenger messenger;
        private readonly List<DimensionsProfileEntity> dimsProfiles;

        public ShipmentViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            dimsProfiles = new List<DimensionsProfileEntity>()
            {
                new DimensionsProfileEntity(0)
                {
                    Name = "Profile 1",
                    Length = 6,
                    Width = 4,
                    Height = 1,
                    Weight = 0.5
                }
            };

            dimensionsManager = mock.Mock<IDimensionsManager>();
            dimensionsManager.Setup(dm => dm.ProfilesReadOnly(It.IsAny<IPackageAdapter>())).Returns(dimsProfiles);

            expectedPackageTypes = new Dictionary<int, string>();
            expectedPackageTypes.Add(0, "Package Type 0");
            expectedPackageTypes.Add(1, "Package Type 1");

            expectedServices = new Dictionary<int, string>();
            expectedServices.Add(0, "Service 0");
            expectedServices.Add(1, "Service 1");

            shipmentServicesBuilder = mock.Mock<IShipmentServicesBuilder>();
            shipmentServicesBuilder.Setup(sb => sb.BuildServiceTypeDictionary(It.IsAny<IEnumerable<ShipmentEntity>>())).Returns(expectedServices);

            shipmentServicesBuilderFactory = mock.Mock<IShipmentServicesBuilderFactory>();
            shipmentServicesBuilderFactory.Setup(sbf => sbf.Get(It.IsAny<ShipmentTypeCode>())).Returns(shipmentServicesBuilder.Object);

            shipmentPackageTypesBuilder = mock.Mock<IShipmentPackageTypesBuilder>();
            shipmentPackageTypesBuilder.Setup(sb => sb.BuildPackageTypeDictionary(It.IsAny<IEnumerable<ShipmentEntity>>())).Returns(expectedPackageTypes);

            shipmentPackageTypesBuilderFactory = mock.Mock<IShipmentPackageTypesBuilderFactory>();
            shipmentPackageTypesBuilderFactory.Setup(sbf => sbf.Get(It.IsAny<ShipmentTypeCode>())).Returns(shipmentPackageTypesBuilder.Object);
        }

        [Fact]
        public void ShipDate_MatchesShipmentAdapterValue()
        {
            CreateDefaultShipmentAdapter(mock, 2);

            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);

            Assert.Equal(shipmentAdapter.Object.ShipDate, testObject.ShipDate.ToUniversalTime());
        }

        [Fact]
        public void ContentWeight_MatchesShipmentAdapterValue()
        {
            CreateDefaultShipmentAdapter(mock, 2);

            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);

            Assert.Equal(shipmentAdapter.Object.ContentWeight, testObject.ContentWeight);
        }

        [Fact]
        public void ServiceType_MatchesShipmentAdapterValue()
        {
            CreateDefaultShipmentAdapter(mock, 2);

            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);

            Assert.Equal(shipmentAdapter.Object.ServiceType, testObject.ServiceType);
        }

        [Fact]
        public void SelectRate_UpdatesServiceTypeOnlyOnce_WhenServiceTypeIsTheSameButUpdatedMultipleTimes()
        {
            CreateDefaultShipmentAdapter(mock, 2);

            int serviceType = 3;
            shipmentAdapter.Setup(sa => sa.ServiceType).Returns(serviceType);

            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);

            testObject.SelectRate(new RateResult("desc", "3 days"));

            shipmentAdapter.ResetCalls();
            shipmentAdapter.VerifySet(sa => sa.ServiceType = serviceType, Times.Never());

            testObject.SelectRate(new RateResult("desc", "3 days"));
            testObject.SelectRate(new RateResult("desc", "3 days"));
            testObject.SelectRate(new RateResult("desc", "3 days"));

            shipmentAdapter.ResetCalls();
            shipmentAdapter.VerifySet(sa => sa.ServiceType = serviceType, Times.Never());
        }

        [Fact]
        public void PackagingType_DefaultsToFirstInPackageTypesList_WhenNewPackagingTypeIsNotInTheList()
        {
            CreateDefaultShipmentAdapter(mock, 2);

            expectedPackageTypes.Clear();
            expectedPackageTypes.Add(0, "First");
            expectedPackageTypes.Add(1, "Second");

            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);

            testObject.PackagingType = 99999;

            Assert.Equal(expectedPackageTypes.First().Key, testObject.PackagingType);
        }

        [Fact]
        public void SupportsMultiplePackages_MatchesShipmentAdapterValue()
        {
            CreateDefaultShipmentAdapter(mock, 2);

            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);

            Assert.Equal(shipmentAdapter.Object.SupportsMultiplePackages, testObject.SupportsMultiplePackages);
        }

        [Fact]
        public void SupportsPackageTypes_MatchesShipmentAdapterValue()
        {
            CreateDefaultShipmentAdapter(mock, 2);

            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);

            Assert.Equal(shipmentAdapter.Object.SupportsPackageTypes, testObject.SupportsPackageTypes);
        }

        [Fact]
        public void PackageCountList_HasTwentyFiveEntries()
        {
            CreateDefaultShipmentAdapter(mock, 2);

            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);

            Assert.Equal(25, testObject.PackageCountList.Count());
        }

        [Fact]
        public void SelectedPackageAdapter_DefaultsToFirstInList()
        {
            CreateDefaultShipmentAdapter(mock, 2);

            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);

            Assert.Equal(packageAdapters[0].Index, testObject.SelectedPackageAdapter.Index);
        }

        [Fact]
        public void Load_GetsServices()
        {
            CreateDefaultShipmentAdapter(mock, 2);

            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);
            Assert.Equal(expectedServices.Count, testObject.Services.Count());
        }

        [Fact]
        public void RefreshServiceTypes_UpdatesServices()
        {
            CreateDefaultShipmentAdapter(mock, 2);

            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);
            Assert.Equal(expectedServices.Count, testObject.Services.Count());

            expectedServices.Add(3, "Service 3");
            testObject.RefreshServiceTypes();
            Assert.Equal(expectedServices.Count, testObject.Services.Count());
        }

        [Fact]
        public void RefreshServiceTypes_ReturnsErrorService_WhenInvalidRateGroupShippingException()
        {
            CreateDefaultShipmentAdapter(mock, 2);
            shipmentServicesBuilder.Setup(sb => sb.BuildServiceTypeDictionary(It.IsAny<IEnumerable<ShipmentEntity>>()))
                .Throws(new InvalidRateGroupShippingException(new RateGroup(Enumerable.Empty<RateResult>())));

            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);

            Assert.Equal(1, testObject.Services.Count());
            Assert.Contains("error", testObject.Services.First().Value, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void RefreshServiceTypes_SetsServiceType()
        {
            CreateDefaultShipmentAdapter(mock, 2);

            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);
            testObject.ServiceType = -1;

            testObject.RefreshServiceTypes();

            Assert.Equal(shipmentAdapter.Object.ServiceType, testObject.ServiceType);
        }

        [Fact]
        public void Load_GetsPackageTypes()
        {
            CreateDefaultShipmentAdapter(mock, 2);

            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);

            Assert.Equal(expectedPackageTypes.Count, testObject.PackageTypes.Count());
        }

        [Fact]
        public void DimensionsProfiles_MatchDimensionsProfilesManagerValues()
        {
            CreateDefaultShipmentAdapter(mock, 2);
            CreateDimensionsProfilesManager(mock);

            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();

            testObject.Load(shipmentAdapter.Object);

            IDimensionsManager dimsMgr = mock.Mock<IDimensionsManager>().Object;

            Assert.Equal(dimsMgr.ProfilesReadOnly(It.IsAny<IPackageAdapter>()).Count(), testObject.DimensionsProfiles.Count());

            foreach (var expectedDim in dimsMgr.ProfilesReadOnly(It.IsAny<IPackageAdapter>()))
            {
                var actualDim = testObject.DimensionsProfiles.First(d => d.DimensionsProfileID == expectedDim.DimensionsProfileID);

                Assert.NotNull(actualDim);
                Assert.Equal(expectedDim.Height, actualDim.Height);
                Assert.Equal(expectedDim.Width, actualDim.Width);
                Assert.Equal(expectedDim.Length, actualDim.Length);
                Assert.Equal(expectedDim.Weight, actualDim.Weight);
            }
        }

        [Fact]
        public void UpdateSelectedDimensionsProfile_MatchDimensionsProfilesManagerValues()
        {
            CreateDefaultShipmentAdapter(mock, 2);
            CreateDimensionsProfilesManager(mock);

            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();

            testObject.Load(shipmentAdapter.Object);

            IDimensionsManager dimsMgr = mock.Mock<IDimensionsManager>().Object;

            Assert.Equal(dimsMgr.ProfilesReadOnly(It.IsAny<IPackageAdapter>()).Count(), testObject.DimensionsProfiles.Count());

            foreach (var expectedDim in dimsMgr.ProfilesReadOnly(It.IsAny<IPackageAdapter>()))
            {
                var actualDim = testObject.DimensionsProfiles.First(d => d.DimensionsProfileID == expectedDim.DimensionsProfileID);

                Assert.NotNull(actualDim);
                Assert.Equal(expectedDim.Height, actualDim.Height);
                Assert.Equal(expectedDim.Width, actualDim.Width);
                Assert.Equal(expectedDim.Length, actualDim.Length);
                Assert.Equal(expectedDim.Weight, actualDim.Weight);
            }
        }

        [Fact]
        public void UpdateSelectedDimensionsProfile_ReturnsDefaultProfile_WhenSelectedDimensionsProfileIDIsNotInDimsManager()
        {
            CreateDefaultShipmentAdapter(mock, 2);
            CreateDimensionsProfilesManager(mock);

            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();

            testObject.Load(shipmentAdapter.Object);
            IPackageAdapter nonExistentPackageAdapter = new TestPackageAdapter() { DimsProfileID = 999 };
            testObject.SelectedPackageAdapter = new PackageAdapterWrapper(nonExistentPackageAdapter);

            Assert.Equal(0, testObject.DimsProfileID);
        }

        [Fact]
        public void ManageDimensionsProfiles_UpdatesDimensionsProfiles_WhenDimensionsProfilesChangedMessageReceived()
        {
            TestMessenger testMessenger = new TestMessenger();
            mock.Provide<IMessenger>(testMessenger);

            CreateDefaultShipmentAdapter(mock, 2);
            CreateDimensionsProfilesManager(mock);

            DimensionsProfileEntity dimsProfileEntity = new DimensionsProfileEntity(10);
            
            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);
            dimsProfiles.Add(dimsProfileEntity);
            
            DimensionsProfilesChangedMessage message = new DimensionsProfilesChangedMessage(this);
            testMessenger.Send(message);

            Assert.Equal(dimsProfiles.Count, testObject.DimensionsProfiles.Count);

            testMessenger.Dispose();
        }

        [Fact]
        public void DimensionsAreUpdated_WhenChangeDimensionsMessageReceived()
        {
            TestMessenger testMessenger = new TestMessenger();
            mock.Provide<IMessenger>(testMessenger);

            CreateDefaultShipmentAdapter(mock, 1);
            CreateDimensionsProfilesManager(mock);

            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);

            var result = ScaleReadResult.Success(1D, 2D, 3D, 4D, ScaleType.None);
            var changeDimensionMessage = new ChangeDimensionsMessage(this, result);
            testMessenger.Send(changeDimensionMessage);

            testObject.Save();
            
            Assert.Equal(packageAdapters[0].DimsLength, result.Length);
            Assert.Equal(packageAdapters[0].DimsWidth, result.Width);
            Assert.Equal(packageAdapters[0].DimsHeight, result.Height);
            Assert.Equal(packageAdapters[0].DimsProfileID, 0);

            testMessenger.Dispose();
        }

        [Fact]
        public void ManageDimensionsProfiles_UpdatesDimensionsProfiles_WhenSelectedDimsProfileIsNotDefaultAndDimensionsProfilesChangedMessageReceived()
        {
            TestMessenger testMessenger = new TestMessenger();
            mock.Provide<IMessenger>(testMessenger);

            CreateDefaultShipmentAdapter(mock, 2);
            CreateDimensionsProfilesManager(mock);

            DimensionsProfileEntity dimsProfileEntity = new DimensionsProfileEntity(10);
            
            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);
            testObject.SelectedDimensionsProfile = new DimensionsProfileEntity(999);

            dimsProfiles.Add(dimsProfileEntity);
            
            DimensionsProfilesChangedMessage message = new DimensionsProfilesChangedMessage(this);
            testMessenger.Send(message);

            Assert.Equal(dimsProfiles.Count, testObject.DimensionsProfiles.Count);

            testMessenger.Dispose();
        }

        [Fact]
        public void SelectedDimensionsProfile_UpdatesSelectedPackageAdapter_WhenProfileExists()
        {
            CreateDefaultShipmentAdapter(mock, 2);
            CreateDimensionsProfilesManager(mock);

            IDimensionsManager dimsMgr = mock.Mock<IDimensionsManager>().Object;
            var dimsProfileEntity = dimsMgr.ProfilesReadOnly(It.IsAny<IPackageAdapter>()).Skip(1).First();
            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();

            testObject.Load(shipmentAdapter.Object);
            testObject.SelectedDimensionsProfile = dimsProfileEntity;

            Assert.Equal(dimsProfileEntity.DimensionsProfileID, testObject.DimsProfileID);
            Assert.Equal(dimsProfileEntity.Length, testObject.DimsLength);
            Assert.Equal(dimsProfileEntity.Width, testObject.DimsWidth);
            Assert.Equal(dimsProfileEntity.Height, testObject.DimsHeight);
        }

        [Fact]
        public void SupportsDimensions_IsFalse_WhenShipmentTypeIsOther()
        {
            CreateDefaultShipmentAdapter(mock, 2);
            CreateDimensionsProfilesManager(mock);

            shipmentAdapter.Setup(sa => sa.ShipmentTypeCode).Returns(ShipmentTypeCode.Other);

            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();

            testObject.Load(shipmentAdapter.Object);

            Assert.False(testObject.SupportsDimensions);
        }

        [Fact]
        public void SupportsDimensions_IsTrue_WhenShipmentTypeIsNotOther()
        {
            CreateDefaultShipmentAdapter(mock, 2);
            CreateDimensionsProfilesManager(mock);

            shipmentAdapter.Setup(sa => sa.ShipmentTypeCode).Returns(ShipmentTypeCode.UpsOnLineTools);

            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();

            testObject.Load(shipmentAdapter.Object);

            Assert.True(testObject.SupportsDimensions);
        }

        [Fact]
        public void SelectedCustomsItem_UpdatesWithNewValue()
        {
            CreateDefaultShipmentAdapter(mock, 2);

            ShipmentCustomsItemEntity shipmentCustomsItemEntity = new ShipmentCustomsItemEntity()
            {
                ShipmentCustomsItemID = 3,
                Shipment = shipmentAdapter.Object.Shipment,
                ShipmentID = shipmentAdapter.Object.Shipment.ShipmentID,
                Weight = 6.6
            };

            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();

            testObject.Load(shipmentAdapter.Object);
            testObject.SelectedCustomsItem = new ShipmentCustomsItemAdapter(new ShipmentCustomsItemEntity());
            testObject.SelectedCustomsItem = new ShipmentCustomsItemAdapter(shipmentCustomsItemEntity);

            Assert.Equal(shipmentCustomsItemEntity.ShipmentCustomsItemID, testObject.SelectedCustomsItem.ShipmentCustomsItemID);
            Assert.Equal(shipmentCustomsItemEntity.Weight, testObject.SelectedCustomsItem.Weight);
        }

        [Fact]
        public void OnSelectedCustomsItemPropertyChanged_UpdatesTotalCustomsValue_WhenUnitValueAndOrQuantityChanges()
        {
            CreateDefaultShipmentAdapter(mock, 2);
            shipmentAdapter.Setup(sa => sa.CustomsAllowed).Returns(true);

            ShipmentCustomsItemEntity shipmentCustomsItemEntity = new ShipmentCustomsItemEntity()
            {
                ShipmentCustomsItemID = 3,
                ShipmentID = shipmentAdapter.Object.Shipment.ShipmentID,
                Weight = 6.6,
                UnitValue = 1.5M,
                Quantity = 3
            };

            shipmentAdapter.Setup(sa => sa.GetCustomsItemAdapters())
                .Returns(new[] { new ShipmentCustomsItemAdapter(shipmentCustomsItemEntity) });

            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();

            testObject.Load(shipmentAdapter.Object);
            testObject.SelectedCustomsItem = new ShipmentCustomsItemAdapter(new ShipmentCustomsItemEntity());
            testObject.SelectedCustomsItem = new ShipmentCustomsItemAdapter(shipmentCustomsItemEntity);

            testObject.SelectedCustomsItem.UnitValue = 100;
            testObject.SelectedCustomsItem.Quantity = 2.5;

            decimal expectedValue = (testObject.SelectedCustomsItem.UnitValue * (decimal) testObject.SelectedCustomsItem.Quantity);

            Assert.Equal(expectedValue, testObject.TotalCustomsValue);
        }

        [Fact]
        public void OnSelectedCustomsItemPropertyChanged_UpdatesShipmentContentWeight_WhenWeightAndOrQuantityChanges()
        {
            int packageCount = 2;
            CreateDefaultShipmentAdapter(mock, packageCount);
            shipmentAdapter.Setup(sa => sa.CustomsAllowed).Returns(true);

            ShipmentCustomsItemEntity shipmentCustomsItemEntity = new ShipmentCustomsItemEntity()
            {
                ShipmentCustomsItemID = 3,
                ShipmentID = shipmentAdapter.Object.Shipment.ShipmentID,
                Weight = 6.6,
                UnitValue = 1.5M,
                Quantity = 3
            };

            shipmentAdapter.Setup(sa => sa.GetCustomsItemAdapters())
                .Returns(new[] { new ShipmentCustomsItemAdapter(shipmentCustomsItemEntity) });

            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();

            testObject.Load(shipmentAdapter.Object);
            testObject.SelectedCustomsItem = new ShipmentCustomsItemAdapter(new ShipmentCustomsItemEntity());
            testObject.SelectedCustomsItem = new ShipmentCustomsItemAdapter(shipmentCustomsItemEntity);

            testObject.SelectedCustomsItem.Weight = 100;
            testObject.SelectedCustomsItem.Quantity = 2.5;

            double expectedValue = (testObject.SelectedCustomsItem.Weight * testObject.SelectedCustomsItem.Quantity) / packageCount;

            Assert.Equal(expectedValue, testObject.ContentWeight);
        }

        [Fact]
        public void RedistributeContentWeight_UpdatesPackageAdapterWeights_WhenWeightAndOrQuantityChanges()
        {
            CreateDefaultShipmentAdapter(mock, 2);
            shipmentAdapter.Setup(sa => sa.CustomsAllowed).Returns(true);

            ShipmentCustomsItemEntity shipmentCustomsItemEntity = new ShipmentCustomsItemEntity()
            {
                ShipmentCustomsItemID = 3,
                ShipmentID = shipmentAdapter.Object.Shipment.ShipmentID,
                Weight = 6.6,
                UnitValue = 1.5M,
                Quantity = 3
            };

            shipmentAdapter.Setup(sa => sa.GetCustomsItemAdapters())
                .Returns(new[] { new ShipmentCustomsItemAdapter(shipmentCustomsItemEntity) });

            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();

            testObject.Load(shipmentAdapter.Object);
            testObject.SelectedCustomsItem = new ShipmentCustomsItemAdapter(new ShipmentCustomsItemEntity());
            testObject.SelectedCustomsItem = new ShipmentCustomsItemAdapter(shipmentCustomsItemEntity);

            testObject.SelectedCustomsItem.Weight = 100;
            testObject.SelectedCustomsItem.Quantity = 2.53;

            double expectedValue = testObject.SelectedCustomsItem.Weight * testObject.SelectedCustomsItem.Quantity;

            double actualValue = testObject.PackageAdapters.Sum(pa => pa.Weight);

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void RefreshPackageTypes_UpdatesPackageTypes()
        {
            CreateDefaultShipmentAdapter(mock, 2);

            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);
            Assert.Equal(expectedPackageTypes.Count, testObject.PackageTypes.Count());

            expectedServices.Add(3, "Package Type 3");
            testObject.RefreshServiceTypes();
            Assert.Equal(expectedPackageTypes.Count, testObject.PackageTypes.Count());
        }

        [Fact]
        public void RefreshServiceTypes_UpdatesPackageTypes_WhenSelectedServiceChanges()
        {
            CreateDefaultShipmentAdapter(mock, 2);

            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();

            shipmentPackageTypesBuilder.Setup(sb => sb.BuildPackageTypeDictionary(It.IsAny<IEnumerable<ShipmentEntity>>())).Returns(
            () =>
            {
                Dictionary<int, string> packageTypeSubSet = new Dictionary<int, string>();
                KeyValuePair<int, string> packageType = expectedPackageTypes.First(pt => pt.Key == testObject.ServiceType);

                packageTypeSubSet.Add(packageType.Key, packageType.Value);

                return packageTypeSubSet;
            });

            testObject.Load(shipmentAdapter.Object);

            KeyValuePair<int, string> expectedPackageType = expectedServices.Skip(1).Take(1).First();
            testObject.ServiceType = expectedPackageType.Key;

            Assert.Equal(expectedPackageType.Key, testObject.PackagingType);
        }

        [Fact]
        public void Save_UpdatesShipmentAdapter_WithViewModelValue()
        {
            CreateDefaultShipmentAdapter(mock, 2);

            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);

            testObject.ShipDate = testObject.ShipDate.AddDays(1).ToUniversalTime();
            testObject.ServiceType = testObject.ServiceType++;

            testObject.SelectedDimensionsProfile = dimensionsManager.Object.ProfilesReadOnly(testObject.PackageAdapters.First()).FirstOrDefault();

            testObject.Save();

            shipmentAdapter.VerifySet(sa => sa.ShipDate = testObject.ShipDate, Times.Once());
            shipmentAdapter.VerifySet(sa => sa.ServiceType = testObject.ServiceType);
        }

        [Fact]
        public void Save_SetsContentWeightCorrectly_WhenSelectedPackageWeightHasChanged()
        {
            var package1 = mock.CreateMock<IPackageAdapter>(p => p.SetupProperty(x => x.Weight));
            package1.Object.Weight = 1;
            var package2 = mock.CreateMock<IPackageAdapter>(p => p.SetupProperty(x => x.Weight));
            package2.Object.Weight = 1;

            shipmentAdapter = mock.CreateMock<ICarrierShipmentAdapter>();
            shipmentAdapter.SetupProperty(x => x.ContentWeight);
            shipmentAdapter.Setup(x => x.GetPackageAdaptersAndEnsureShipmentIsLoaded()).Returns(new[] { package1.Object, package2.Object });

            var testObject = mock.Create<ShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);

            testObject.SelectedPackageAdapter = testObject.PackageAdapters.First();
            testObject.ContentWeight = 2;

            testObject.Save();

            Assert.Equal(3, shipmentAdapter.Object.ContentWeight);
            Assert.Equal(2, package1.Object.Weight);
            Assert.Equal(1, package2.Object.Weight);
        }

        [Fact]
        public void AddCustomsItem_DelegatesToShipmentAdapter_WithViewModelValue()
        {
            CreateDefaultShipmentAdapter(mock, 2);

            shipmentAdapter.Setup(sa => sa.CustomsAllowed).Returns(true);

            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();

            testObject.CustomsAllowed = true;
            testObject.Load(shipmentAdapter.Object);

            testObject.ShipDate = testObject.ShipDate.AddDays(1);
            testObject.ServiceType = testObject.ServiceType++;
            testObject.SelectedDimensionsProfile = dimensionsManager.Object.ProfilesReadOnly(testObject.PackageAdapters.First()).FirstOrDefault();

            testObject.AddCustomsItemCommand.Execute(null);
            shipmentAdapter.Verify(sa => sa.AddCustomsItem(), Times.Once());
        }

        [Fact]
        public void DeleteCustomsItemCommand_CanNotExecute_WhenSelectedCustomsItemIsNull()
        {
            CreateDefaultShipmentAdapter(mock, 2);

            shipmentAdapter.Setup(sa => sa.CustomsAllowed).Returns(true);

            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();

            testObject.CustomsAllowed = true;
            testObject.Load(shipmentAdapter.Object);
            testObject.SelectedCustomsItem = null;

            Assert.False(testObject.DeleteCustomsItemCommand.CanExecute(mock));
        }

        [Fact]
        public void DeleteCustomsItemCommand_CanNotExecute_WhenCustomsItemsDoesNotContainSelectedCustomsItem()
        {
            CreateDefaultShipmentAdapter(mock, 2);

            shipmentAdapter.Setup(sa => sa.CustomsAllowed).Returns(true);

            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();

            testObject.CustomsAllowed = true;
            testObject.Load(shipmentAdapter.Object);
            testObject.SelectedCustomsItem = new ShipmentCustomsItemAdapter(new ShipmentCustomsItemEntity(999));

            Assert.False(testObject.DeleteCustomsItemCommand.CanExecute(mock));
        }

        [Fact]
        public void AddCustomsItemCommand_CanExecute()
        {
            CreateDefaultShipmentAdapter(mock, 2);

            shipmentAdapter.Setup(sa => sa.CustomsAllowed).Returns(true);

            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();

            testObject.CustomsAllowed = true;
            testObject.Load(shipmentAdapter.Object);
            testObject.SelectedCustomsItem = new ShipmentCustomsItemAdapter(new ShipmentCustomsItemEntity(999));

            Assert.True(testObject.AddCustomsItemCommand.CanExecute(mock));
        }

        [Fact]
        public void AddCustomsItemCommand_AddsCustomsItem()
        {
            CreateDefaultShipmentAdapter(mock, 2);

            shipmentAdapter.Setup(sa => sa.CustomsAllowed).Returns(true);

            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();

            testObject.CustomsAllowed = true;
            testObject.Load(shipmentAdapter.Object);
            testObject.AddCustomsItemCommand.Execute(null);

            Assert.Equal(1, testObject.CustomsItems.Count);
        }

        [Fact]
        public void DeleteCustomsItemCommand_DeletesCustomsItem()
        {
            CreateDefaultShipmentAdapter(mock, 2);

            shipmentAdapter.Setup(sa => sa.CustomsAllowed).Returns(true);

            shipmentAdapter.Setup(sa => sa.GetCustomsItemAdapters())
                .Returns(new[] {
                    new ShipmentCustomsItemAdapter(new ShipmentCustomsItemEntity(0)),
                    new ShipmentCustomsItemAdapter(new ShipmentCustomsItemEntity(1)),
                    new ShipmentCustomsItemAdapter(new ShipmentCustomsItemEntity(2))
                });

            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();

            testObject.CustomsAllowed = true;
            testObject.Load(shipmentAdapter.Object);

            testObject.SelectedCustomsItem = testObject.CustomsItems.Skip(1).First();

            testObject.DeleteCustomsItemCommand.Execute(null);

            Assert.Equal(2, testObject.CustomsItems.Count);
        }

        [Fact]
        public void HandleShippingSettingsChangedMessage_DoesNotUpdateInsuranceFields_WhenShipmentIsProcessed()
        {
            using (TestMessenger messenger = new TestMessenger())
            {
                mock.Provide<IMessenger>(messenger);

                shipment.Processed = true;
                CreateDefaultShipmentAdapter(mock, 2);
                shipmentAdapter.Setup(sa => sa.Shipment).Returns(shipment);

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();

                testObject.Load(shipmentAdapter.Object);

                messenger.Send(new ShippingSettingsChangedMessage(this, new ShippingSettingsEntity()));

                shipmentAdapter.Verify(sa => sa.UpdateInsuranceFields(It.IsAny<ShippingSettingsEntity>()),
                    Times.Never);
            }
        }

        [Fact]
        public void HandleShippingSettingsChangedMessage_UpdatesInsuranceFields_WhenShipmentIsNotProcessed()
        {
            messenger = new TestMessenger();
            mock.Provide<IMessenger>(messenger);

            shipment.Processed = false;
            CreateDefaultShipmentAdapter(mock, 2);
            shipmentAdapter.Setup(sa => sa.Shipment).Returns(shipment);

            List<Mock<IPackageAdapter>> mockPackageAdapters = new List<Mock<IPackageAdapter>>()
                {
                    new Mock<IPackageAdapter>(),
                    new Mock<IPackageAdapter>()
                };

            shipmentAdapter.Setup(sa => sa.GetPackageAdaptersAndEnsureShipmentIsLoaded()).Returns(mockPackageAdapters.Select(mpa => mpa.Object));

            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();

            testObject.Load(shipmentAdapter.Object);

            messenger.Send(new ShippingSettingsChangedMessage(this, new ShippingSettingsEntity()));

            shipmentAdapter.Verify(sa => sa.UpdateInsuranceFields(It.IsAny<ShippingSettingsEntity>()), Times.Once);

            foreach (Mock<IPackageAdapter> mpa in mockPackageAdapters)
            {
                mpa.Verify(pa => pa.UpdateInsuranceFields(It.IsAny<ShippingSettingsEntity>()), Times.Once());
            }
        }

        [Fact]
        public void Indexer_ReturnsEmptyString_WhenShipmentIsProcessed()
        {
            messenger = new TestMessenger();
            mock.Provide<IMessenger>(messenger);

            shipment.Processed = true;
            CreateDefaultShipmentAdapter(mock, 2);
            shipmentAdapter.Setup(sa => sa.Shipment).Returns(shipment);

            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);

            Assert.Equal(string.Empty, testObject["TotalCustomsValue"]);
        }

        [Fact]
        public void Indexer_ReturnsError_WhenTotalCustomsValueIsInvalid()
        {
            messenger = new TestMessenger();
            mock.Provide<IMessenger>(messenger);

            shipment.Processed = false;
            CreateDefaultShipmentAdapter(mock, 2);
            shipmentAdapter.Setup(sa => sa.Shipment).Returns(shipment);

            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);
            testObject.TotalCustomsValue = -1;

            Assert.NotEqual(string.Empty, testObject["TotalCustomsValue"]);
        }

        [Fact]
        public void AllErrors_ReturnsListOfErrors_WhenTotalCustomsValueIsInvalid()
        {
            messenger = new TestMessenger();
            mock.Provide<IMessenger>(messenger);

            shipment.Processed = false;
            CreateDefaultShipmentAdapter(mock, 2);
            shipmentAdapter.Setup(sa => sa.Shipment).Returns(shipment);

            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);
            testObject.TotalCustomsValue = -1;

            Assert.NotNull(testObject.AllErrors());
            Assert.InRange(testObject.AllErrors().Count, 1, Int32.MaxValue);
        }

        [Fact]
        public void Error_ReturnsNull()
        {
            messenger = new TestMessenger();
            mock.Provide<IMessenger>(messenger);

            shipment.Processed = false;
            CreateDefaultShipmentAdapter(mock, 2);
            shipmentAdapter.Setup(sa => sa.Shipment).Returns(shipment);

            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);

            Assert.Null(testObject.Error);
        }

        [Fact]
        public void AddPackage_DoesNotDelegateToShipmentAdapter_WhenMultiplePackagesAreNotSupported()
        {
            shipmentAdapter = mock.CreateMock<ICarrierShipmentAdapter>();
            shipmentAdapter.Setup(x => x.SupportsMultiplePackages).Returns(false);
            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);

            testObject.AddPackageCommand.Execute(null);

            shipmentAdapter.Verify(x => x.AddPackage(), Times.Never);
        }

        [Fact]
        public void AddPackage_DelegatesToShipmentAdapter_WhenMultiplePackagesAreSupported()
        {
            shipmentAdapter = mock.CreateMock<ICarrierShipmentAdapter>();
            shipmentAdapter.Setup(x => x.SupportsMultiplePackages).Returns(true);
            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);

            testObject.AddPackageCommand.Execute(null);

            shipmentAdapter.Verify(x => x.AddPackage(), Times.Once);
        }

        [Fact]
        public void AddPackage_AddsNewPackageToEndOfCollection_WhenMultiplePackagesAreSupported()
        {
            shipmentAdapter = mock.CreateMock<ICarrierShipmentAdapter>();
            IPackageAdapter packageAdapter = mock.Build<IPackageAdapter>();
            shipmentAdapter.Setup(x => x.SupportsMultiplePackages).Returns(true);
            shipmentAdapter.Setup(x => x.AddPackage()).Returns(packageAdapter);
            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);

            testObject.AddPackageCommand.Execute(null);

            Assert.Equal(packageAdapter, testObject.PackageAdapters.Last().WrappedAdapter);
        }

        [Fact]
        public void AddPackage_SelectsNewPackageAdapter_WhenMultiplePackagesAreSupported()
        {
            shipmentAdapter = mock.CreateMock<ICarrierShipmentAdapter>();
            IPackageAdapter packageAdapter = mock.Build<IPackageAdapter>();
            shipmentAdapter.Setup(x => x.SupportsMultiplePackages).Returns(true);
            shipmentAdapter.Setup(x => x.AddPackage()).Returns(packageAdapter);
            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);

            testObject.AddPackageCommand.Execute(null);

            Assert.Equal(packageAdapter, testObject.SelectedPackageAdapter.WrappedAdapter);
        }

        [Fact]
        public void AddPackage_DoesNotAddPackage_WhenShipmentAlreadyHas25Packages()
        {
            shipmentAdapter = mock.CreateMock<ICarrierShipmentAdapter>();
            shipmentAdapter.Setup(x => x.SupportsMultiplePackages).Returns(true);
            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);

            for (int i = 0; i < 25; i++)
            {
                testObject.PackageAdapters.Add(new PackageAdapterWrapper(mock.CreateMock<IPackageAdapter>().Object));
            }

            testObject.AddPackageCommand.Execute(null);

            Assert.Equal(25, testObject.PackageAdapters.Count);
        }

        [Fact]
        public void DeletePackage_DoesNotDelegateToShipmentAdapter_WhenMultiplePackagesAreNotSupported()
        {
            shipmentAdapter = mock.CreateMock<ICarrierShipmentAdapter>();
            shipmentAdapter.Setup(x => x.SupportsMultiplePackages).Returns(false);
            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);

            testObject.DeletePackageCommand.Execute(null);

            shipmentAdapter.Verify(x => x.DeletePackage(It.IsAny<PackageAdapterWrapper>()), Times.Never);
        }

        [Fact]
        public void DeletePackage_DelegatesToShipmentAdapter_WhenMultiplePackagesAreSupported()
        {
            IPackageAdapter packageAdapter = mock.CreateMock<IPackageAdapter>().Object;
            shipmentAdapter = mock.CreateMock<ICarrierShipmentAdapter>();
            shipmentAdapter.Setup(x => x.GetPackageAdaptersAndEnsureShipmentIsLoaded())
                .Returns(new[] { packageAdapter, mock.CreateMock<IPackageAdapter>().Object });
            shipmentAdapter.Setup(x => x.SupportsMultiplePackages).Returns(true);
            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);

            testObject.DeletePackageCommand.Execute(null);

            shipmentAdapter.Verify(x => x.DeletePackage(packageAdapter), Times.Once);
        }

        [Fact]
        public void DeletePackage_RemovesPackageFromList_WhenMultiplePackagesAreSupported()
        {
            IPackageAdapter packageAdapter = mock.CreateMock<IPackageAdapter>().Object;
            shipmentAdapter = mock.CreateMock<ICarrierShipmentAdapter>();
            shipmentAdapter.Setup(x => x.GetPackageAdaptersAndEnsureShipmentIsLoaded())
                .Returns(new[] { packageAdapter, mock.CreateMock<IPackageAdapter>().Object });
            shipmentAdapter.Setup(x => x.SupportsMultiplePackages).Returns(true);
            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);

            testObject.DeletePackageCommand.Execute(null);

            Assert.DoesNotContain(packageAdapter, testObject.PackageAdapters);
        }

        [Fact]
        public void DeletePackage_DoesNotDelegateToShipmentAdapter_WhenShipmentHasOnePackageAdapter()
        {
            shipmentAdapter = mock.CreateMock<ICarrierShipmentAdapter>();
            shipmentAdapter.Setup(x => x.SupportsMultiplePackages).Returns(true);
            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);

            testObject.DeletePackageCommand.Execute(null);

            shipmentAdapter.Verify(x => x.DeletePackage(It.IsAny<IPackageAdapter>()), Times.Never);
        }

        [Fact]
        public void DeletePackage_SelectsSubsequentPackageAdapter_WhenPackageIsDeleted()
        {
            IPackageAdapter packageAdapter = mock.CreateMock<IPackageAdapter>().Object;
            IPackageAdapter expectedPackageAdapter = mock.CreateMock<IPackageAdapter>().Object;
            shipmentAdapter = mock.CreateMock<ICarrierShipmentAdapter>();
            shipmentAdapter.Setup(x => x.GetPackageAdaptersAndEnsureShipmentIsLoaded())
                .Returns(new[] { packageAdapter, expectedPackageAdapter, mock.CreateMock<IPackageAdapter>().Object });
            shipmentAdapter.Setup(x => x.SupportsMultiplePackages).Returns(true);
            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);

            testObject.DeletePackageCommand.Execute(null);

            Assert.Equal(expectedPackageAdapter, testObject.SelectedPackageAdapter.WrappedAdapter);
        }

        [Fact]
        public void DeletePackage_SelectsPreviousPackageAdapter_WhenDeletedPackageIsAtTheEndOfTheList()
        {
            IPackageAdapter packageAdapter = mock.CreateMock<IPackageAdapter>().Object;
            IPackageAdapter expectedPackageAdapter = mock.CreateMock<IPackageAdapter>().Object;
            shipmentAdapter = mock.CreateMock<ICarrierShipmentAdapter>();
            shipmentAdapter.Setup(x => x.GetPackageAdaptersAndEnsureShipmentIsLoaded())
                .Returns(new[] { mock.CreateMock<IPackageAdapter>().Object, expectedPackageAdapter, packageAdapter });
            shipmentAdapter.Setup(x => x.SupportsMultiplePackages).Returns(true);
            ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);
            testObject.SelectedPackageAdapter = testObject.PackageAdapters.Last();

            testObject.DeletePackageCommand.Execute(null);

            Assert.Equal(expectedPackageAdapter, testObject.SelectedPackageAdapter.WrappedAdapter);
        }

        private Dictionary<int, string> CreateDefaultShipmentAdapter(AutoMock autoMock, int numberOfPackages)
        {
            CreatePackageAdapters(numberOfPackages);

            shipmentAdapter = autoMock.Mock<ICarrierShipmentAdapter>();
            shipmentAdapter.Setup(sa => sa.ShipmentTypeCode).Returns(ShipmentTypeCode.UpsOnLineTools);
            shipmentAdapter.Setup(sa => sa.ServiceType).Returns((int) UpsServiceType.UpsGround);
            shipmentAdapter.Setup(sa => sa.ShipDate).Returns(new DateTime(2015, 1, 1, 1, 1, 1));
            shipmentAdapter.Setup(sa => sa.ContentWeight).Returns(0.5);
            shipmentAdapter.Setup(sa => sa.SupportsPackageTypes).Returns(true);
            shipmentAdapter.Setup(sa => sa.SupportsAccounts).Returns(true);
            shipmentAdapter.Setup(sa => sa.SupportsMultiplePackages).Returns(true);
            shipmentAdapter.Setup(sa => sa.GetPackageAdaptersAndEnsureShipmentIsLoaded()).Returns(packageAdapters);

            shipmentAdapter.Setup(sa => sa.CustomsAllowed).Returns(false);

            return expectedServices;
        }

        private void CreatePackageAdapters(int numberOfPackages)
        {
            packageAdapters.Clear();

            for (int i = 1; i <= numberOfPackages; i++)
            {
                TestPackageAdapter packageAdapter = new TestPackageAdapter();
                packageAdapter.PackagingType = (int) UpsPackagingType.Custom;
                packageAdapter.AdditionalWeight = 0.1 * i;
                packageAdapter.ApplyAdditionalWeight = false;
                packageAdapter.Index = 1 * i;
                packageAdapter.DimsHeight = 2 * i;
                packageAdapter.DimsLength = 2 * i;
                packageAdapter.DimsWidth = 1 * i;
                packageAdapter.Weight = 0.5 * i;
                packageAdapter.InsuranceChoice = mock.Build<IInsuranceChoice>();

                packageAdapters.Add(packageAdapter);
            }
        }

        private void CreateDimensionsProfilesManager(AutoMock autoMock)
        {
            dimsProfiles.Clear();
            var newDims = new List<DimensionsProfileEntity>()
            {
                new DimensionsProfileEntity(0)
                {
                    Weight = 1.5,
                    Width = 1,
                    Length = 4,
                    Height = 6,
                    Name = "Select a profile"
                },
                new DimensionsProfileEntity(1)
                {
                    Weight = 1.5,
                    Width = 1,
                    Length = 4,
                    Height = 6,
                    Name = "Dims profile 1"
                },
                new DimensionsProfileEntity(2)
                {
                    Weight = 2.5,
                    Width = 2.2,
                    Length = 4.2,
                    Height = 6.2,
                    Name = "Dims profile 2"
                }
            };
            
            dimsProfiles.AddRange(newDims);
        }

        public void Dispose()
        {
            messenger?.Dispose();
            mock?.Dispose();
        }
    }
}
