using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Carriers.Other;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Rating;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Services.Builders;
using ShipWorks.Shipping.UI.ShippingPanel.ShipmentControl;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.ShippingPanel.ShipmentControl
{
    public class ShipmentViewModelTest
    {
        private readonly ShipmentEntity shipment = new ShipmentEntity();
        private Mock<ICarrierShipmentAdapter> shipmentAdapter;
        private Mock<IShipmentServicesBuilder> shipmentServicesBuilder;
        private Mock<IShipmentServicesBuilderFactory> shipmentServicesBuilderFactory;
        private Mock<IShipmentPackageTypesBuilder> shipmentPackageTypesBuilder;
        private Mock<IShipmentPackageTypesBuilderFactory> shipmentPackageTypesBuilderFactory;
        private readonly Mock<IDimensionsManager> dimensionsManager;
        private Mock<IRateSelectionFactory> rateSelectionFactory;
        private Dictionary<int, string> expectedServices = new Dictionary<int, string>();
        private Dictionary<int, string> expectedPackageTypes = new Dictionary<int, string>();
        private readonly List<IPackageAdapter> packageAdapters = new List<IPackageAdapter>();

        public ShipmentViewModelTest()
        {
            List<DimensionsProfileEntity> dimensionsProfileEntities = new List<DimensionsProfileEntity>()
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

            dimensionsManager = new Mock<IDimensionsManager>();
            dimensionsManager.Setup(dm => dm.Profiles(It.IsAny<IPackageAdapter>())).Returns(dimensionsProfileEntities);
        }

        [Fact]
        public void ShipDate_MatchesShipmentAdapterValue_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock, 2);

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);

                Assert.Equal(shipmentAdapter.Object.ShipDate, testObject.ShipDate);
            }
        }

        [Fact]
        public void TotalWeight_MatchesShipmentAdapterValue_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock, 2);

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);

                Assert.Equal(shipmentAdapter.Object.TotalWeight, testObject.TotalWeight);
            }
        }
        
        [Fact]
        public void ServiceType_MatchesShipmentAdapterValue_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock, 2);

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);

                Assert.Equal(shipmentAdapter.Object.ServiceType, testObject.ServiceType);
            }
        }

        [Fact]
        public void SupportsMultiplePackages_MatchesShipmentAdapterValue_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock, 2);

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);

                Assert.Equal(shipmentAdapter.Object.SupportsMultiplePackages, testObject.SupportsMultiplePackages);
            }
        }

        [Fact]
        public void SupportsPackageTypes_MatchesShipmentAdapterValue_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock, 2);

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);

                Assert.Equal(shipmentAdapter.Object.SupportsPackageTypes, testObject.SupportsPackageTypes);
            }
        }

        [Fact]
        public void PackageCountList_HasTwentyFiveEntries_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock, 2);

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);

                Assert.Equal(25, testObject.PackageCountList.Count());
            }
        }

        [Fact]
        public void NumberOfPackages_AddsPackageAdapters_WhenNumberRequestedGreaterThanCurrentCount_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock, 2);

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);

                int numOfPackages = testObject.NumberOfPackages;
                numOfPackages++;
                testObject.NumberOfPackages = numOfPackages;

                Assert.Equal(numOfPackages, packageAdapters.Count);
            }
        }

        [Fact]
        public void NumberOfPackages_RemovesPackageAdapters_WhenNumberRequestedLessThanCurrentCount_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock, 2);

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);

                int numOfPackages = testObject.NumberOfPackages;
                numOfPackages--;
                testObject.NumberOfPackages = numOfPackages;

                Assert.Equal(numOfPackages, packageAdapters.Count);
            }
        }

        [Fact]
        public void SelectedPackageAdapter_DefaultsToFirstInList_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock, 2);

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);

                Assert.Equal(packageAdapters[0].Index, testObject.SelectedPackageAdapter.Index);
            }
        }

        [Fact]
        public void Load_GetsServices_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                expectedServices = new Dictionary<int, string>();
                expectedServices.Add(0, "Service 0");
                expectedServices.Add(1, "Service 1");
                
                CreateDefaultShipmentAdapter(mock, 2);

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);
                Assert.Equal(expectedServices.Count, testObject.Services.Count);
            }
        }

        [Fact]
        public void RefreshServiceTypes_UpdatesServices_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                expectedServices = new Dictionary<int, string>();
                expectedServices.Add(0, "Service 0");
                expectedServices.Add(1, "Service 1");

                CreateDefaultShipmentAdapter(mock, 2);

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);
                Assert.Equal(expectedServices.Count, testObject.Services.Count);

                expectedServices.Add(3, "Service 3");
                testObject.RefreshServiceTypes();
                Assert.Equal(expectedServices.Count, testObject.Services.Count);
            }
        }

        [Fact]
        public void RefreshServiceTypes_ReturnsErrorService_WhenInvalidRateGroupShippingException_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock, 2);
                shipmentServicesBuilder.Setup(sb => sb.BuildServiceTypeDictionary(It.IsAny<IEnumerable<ShipmentEntity>>()))
                    .Throws(new InvalidRateGroupShippingException(new RateGroup(Enumerable.Empty<RateResult>())));

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);

                Assert.Equal(1, testObject.Services.Count);
                Assert.Contains("error", testObject.Services.First().Value, StringComparison.OrdinalIgnoreCase);
            }
        }

        [Fact]
        public void RefreshServiceTypes_SetsServiceType_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock, 2);

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);
                testObject.ServiceType = -1;

                testObject.RefreshServiceTypes();

                Assert.Equal(shipmentAdapter.Object.ServiceType, testObject.ServiceType);
            }
        }

        [Fact]
        public void Load_GetsPackageTypes_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                expectedPackageTypes = new Dictionary<int, string>();
                expectedPackageTypes.Add(0, "Package Type 0");
                expectedPackageTypes.Add(1, "Package Type 1");

                CreateDefaultShipmentAdapter(mock, 2);

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);

                Assert.Equal(expectedPackageTypes.Count, testObject.PackageTypes.Count);
            }
        }

        [Fact]
        public void DimensionsProfiles_MatchDimensionsProfilesManagerValues_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock, 2);
                CreateDimensionsProfilesManager(mock);
                
                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
                
                testObject.Load(shipmentAdapter.Object);

                IDimensionsManager dimsMgr = mock.Create<IDimensionsManager>();

                Assert.Equal(dimsMgr.Profiles(It.IsAny<IPackageAdapter>()).Count(), testObject.DimensionsProfiles.Count());

                foreach (DimensionsProfileEntity expectedDim in dimsMgr.Profiles(It.IsAny<IPackageAdapter>()))
                {
                    DimensionsProfileEntity actualDim = testObject.DimensionsProfiles.First(d => d.DimensionsProfileID == expectedDim.DimensionsProfileID);

                    Assert.NotNull(actualDim);
                    Assert.Equal(expectedDim.Height, actualDim.Height);
                    Assert.Equal(expectedDim.Width, actualDim.Width);
                    Assert.Equal(expectedDim.Length, actualDim.Length);
                    Assert.Equal(expectedDim.Weight, actualDim.Weight);
                }
            }
        }

        [Fact]
        public void UpdateSelectedDimensionsProfile_MatchDimensionsProfilesManagerValues_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock, 2);
                CreateDimensionsProfilesManager(mock);

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();

                testObject.Load(shipmentAdapter.Object);

                IDimensionsManager dimsMgr = mock.Create<IDimensionsManager>();

                Assert.Equal(dimsMgr.Profiles(It.IsAny<IPackageAdapter>()).Count(), testObject.DimensionsProfiles.Count());

                foreach (DimensionsProfileEntity expectedDim in dimsMgr.Profiles(It.IsAny<IPackageAdapter>()))
                {
                    DimensionsProfileEntity actualDim = testObject.DimensionsProfiles.First(d => d.DimensionsProfileID == expectedDim.DimensionsProfileID);

                    Assert.NotNull(actualDim);
                    Assert.Equal(expectedDim.Height, actualDim.Height);
                    Assert.Equal(expectedDim.Width, actualDim.Width);
                    Assert.Equal(expectedDim.Length, actualDim.Length);
                    Assert.Equal(expectedDim.Weight, actualDim.Weight);
                }
            }
        }

        [Fact]
        public void UpdateSelectedDimensionsProfile_ReturnsDefaultProfile_WhenSelectedDimensionsProfileIDIsNotInDimsManager_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock, 2);
                CreateDimensionsProfilesManager(mock);

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();

                testObject.Load(shipmentAdapter.Object);
                IPackageAdapter nonExistentPackageAdapter = new TestPackageAdapter() {DimsProfileID = 999};
                testObject.SelectedPackageAdapter = nonExistentPackageAdapter;

                Assert.Equal(0, testObject.SelectedPackageAdapter.DimsProfileID);
            }
        }

        [Fact]
        public void ManageDimensionsProfiles_UpdatesDimensionsProfiles_WhenDimensionsProfilesChangedMessageReceived_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                TestMessenger testMessenger = new TestMessenger();
                mock.Provide<IMessenger>(testMessenger);

                CreateDefaultShipmentAdapter(mock, 2);
                CreateDimensionsProfilesManager(mock);

                IDimensionsManager dimsMgr = mock.Create<IDimensionsManager>();
                DimensionsProfileEntity dimsProfileEntity = new DimensionsProfileEntity(10);
                List<DimensionsProfileEntity> profiles = dimsMgr.Profiles(It.IsAny<IPackageAdapter>()).ToList();

                Mock<IDimensionsManager> mockedDimsMgr = mock.Mock<IDimensionsManager>();
                mockedDimsMgr.Setup(d => d.Profiles(It.IsAny<IPackageAdapter>())).Returns(profiles);

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);
                profiles.Add(dimsProfileEntity);
                mockedDimsMgr.Setup(d => d.Profiles(It.IsAny<IPackageAdapter>())).Returns(profiles);

                DimensionsProfilesChangedMessage message = new DimensionsProfilesChangedMessage(this);
                testMessenger.Send(message);
                
                Assert.Equal(profiles.Count, testObject.DimensionsProfiles.Count);

                testMessenger.Dispose();
            }
        }

        [Fact]
        public void ManageDimensionsProfiles_UpdatesDimensionsProfiles_WhenSelectedDimsProfileIsNotDefaultAndDimensionsProfilesChangedMessageReceived_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                TestMessenger testMessenger = new TestMessenger();
                mock.Provide<IMessenger>(testMessenger);

                CreateDefaultShipmentAdapter(mock, 2);
                CreateDimensionsProfilesManager(mock);

                IDimensionsManager dimsMgr = mock.Create<IDimensionsManager>();
                DimensionsProfileEntity dimsProfileEntity = new DimensionsProfileEntity(10);
                List<DimensionsProfileEntity> profiles = dimsMgr.Profiles(It.IsAny<IPackageAdapter>()).ToList();

                Mock<IDimensionsManager> mockedDimsMgr = mock.Mock<IDimensionsManager>();
                mockedDimsMgr.Setup(d => d.Profiles(It.IsAny<IPackageAdapter>())).Returns(profiles);

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);
                testObject.SelectedDimensionsProfile = new DimensionsProfileEntity(999);

                profiles.Add(dimsProfileEntity);
                mockedDimsMgr.Setup(d => d.Profiles(It.IsAny<IPackageAdapter>())).Returns(profiles);

                DimensionsProfilesChangedMessage message = new DimensionsProfilesChangedMessage(this);
                testMessenger.Send(message);

                Assert.Equal(profiles.Count, testObject.DimensionsProfiles.Count);

                testMessenger.Dispose();
            }
        }

        [Fact]
        public void SelectedDimensionsProfile_UpdatesSelectedPackageAdapter_WhenProfileExists_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock, 2);
                CreateDimensionsProfilesManager(mock);

                IDimensionsManager dimsMgr = mock.Create<IDimensionsManager>();
                DimensionsProfileEntity dimsProfileEntity = dimsMgr.Profiles(It.IsAny<IPackageAdapter>()).Skip(1).First();
                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();

                testObject.Load(shipmentAdapter.Object);
                testObject.SelectedDimensionsProfile = dimsProfileEntity;

                Assert.Equal(dimsProfileEntity.DimensionsProfileID, testObject.SelectedPackageAdapter.DimsProfileID);
                Assert.Equal(dimsProfileEntity.Length, testObject.SelectedPackageAdapter.DimsLength);
                Assert.Equal(dimsProfileEntity.Width, testObject.SelectedPackageAdapter.DimsWidth);
                Assert.Equal(dimsProfileEntity.Height, testObject.SelectedPackageAdapter.DimsHeight);
            }
        }

        [Fact]
        public void SupportsDimensions_IsFalse_WhenShipmentTypeIsOther_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock, 2);
                CreateDimensionsProfilesManager(mock);

                shipmentAdapter.Setup(sa => sa.ShipmentTypeCode).Returns(ShipmentTypeCode.Other);

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();

                testObject.Load(shipmentAdapter.Object);

                Assert.False(testObject.SupportsDimensions);
            }
        }

        [Fact]
        public void SupportsDimensions_IsTrue_WhenShipmentTypeIsNotOther_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock, 2);
                CreateDimensionsProfilesManager(mock);

                shipmentAdapter.Setup(sa => sa.ShipmentTypeCode).Returns(ShipmentTypeCode.UpsOnLineTools);

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();

                testObject.Load(shipmentAdapter.Object);

                Assert.True(testObject.SupportsDimensions);
            }
        }

        [Fact]
        public void SelectedCustomsItem_UpdatesWithNewValue_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock, 2);

                ShipmentCustomsItemEntity shipmentCustomsItemEntity = new ShipmentCustomsItemEntity()
                {
                    ShipmentCustomsItemID = 3,
                    ShipmentID = shipmentAdapter.Object.Shipment.ShipmentID,
                    Weight = 6.6
                };

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
                
                testObject.Load(shipmentAdapter.Object);
                testObject.SelectedCustomsItem = new ShipmentCustomsItemEntity();
                testObject.SelectedCustomsItem = shipmentCustomsItemEntity;

                Assert.Equal(shipmentCustomsItemEntity.ShipmentCustomsItemID, testObject.SelectedCustomsItem.ShipmentCustomsItemID);
                Assert.Equal(shipmentCustomsItemEntity.ShipmentID, testObject.SelectedCustomsItem.ShipmentID);
                Assert.Equal(shipmentCustomsItemEntity.Weight, testObject.SelectedCustomsItem.Weight);
            }
        }

        [Fact]
        public void OnSelectedCustomsItemPropertyChanged_UpdatesTotalCustomsValue_WhenUnitValueAndOrQuantityChanges_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
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

                shipmentAdapter.Setup(sa => sa.CustomsItems).Returns(
                    new EntityCollection<ShipmentCustomsItemEntity>()
                    {
                        shipmentCustomsItemEntity
                    });

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();

                testObject.Load(shipmentAdapter.Object);
                testObject.SelectedCustomsItem = new ShipmentCustomsItemEntity();
                testObject.SelectedCustomsItem = shipmentCustomsItemEntity;

                testObject.SelectedCustomsItem.UnitValue = 100;
                testObject.SelectedCustomsItem.Quantity = 2.5;

                double expectedValue = (double)(testObject.SelectedCustomsItem.UnitValue * (decimal)testObject.SelectedCustomsItem.Quantity);

                Assert.Equal(expectedValue, testObject.TotalCustomsValue);
            }
        }

        [Fact]
        public void OnSelectedCustomsItemPropertyChanged_UpdatesShipmentContentWeight_WhenWeightAndOrQuantityChanges_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
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

                shipmentAdapter.Setup(sa => sa.CustomsItems).Returns(
                    new EntityCollection<ShipmentCustomsItemEntity>()
                    {
                        shipmentCustomsItemEntity
                    });

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();

                testObject.Load(shipmentAdapter.Object);
                testObject.SelectedCustomsItem = new ShipmentCustomsItemEntity();
                testObject.SelectedCustomsItem = shipmentCustomsItemEntity;

                testObject.SelectedCustomsItem.Weight = 100;
                testObject.SelectedCustomsItem.Quantity = 2.5;

                double expectedValue = testObject.SelectedCustomsItem.Weight * testObject.SelectedCustomsItem.Quantity;

                Assert.Equal(expectedValue, testObject.ShipmentContentWeight);
            }
        }

        [Fact]
        public void RedistributeContentWeight_UpdatesPackageAdapterWeights_WhenWeightAndOrQuantityChanges_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
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

                shipmentAdapter.Setup(sa => sa.CustomsItems).Returns(
                    new EntityCollection<ShipmentCustomsItemEntity>()
                    {
                        shipmentCustomsItemEntity
                    });

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();

                testObject.Load(shipmentAdapter.Object);
                testObject.SelectedCustomsItem = new ShipmentCustomsItemEntity();
                testObject.SelectedCustomsItem = shipmentCustomsItemEntity;

                testObject.SelectedCustomsItem.Weight = 100;
                testObject.SelectedCustomsItem.Quantity = 2.53;

                double expectedValue = testObject.SelectedCustomsItem.Weight * testObject.SelectedCustomsItem.Quantity;

                double actualValue = testObject.PackageAdapters.Sum(pa => pa.Weight);

                Assert.Equal(expectedValue, actualValue);
            }
        }

        [Fact]
        public void RefreshPackageTypes_UpdatesPackageTypes_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                expectedPackageTypes = new Dictionary<int, string>();
                expectedPackageTypes.Add(0, "Package Type 0");
                expectedPackageTypes.Add(1, "Package Type 1");

                CreateDefaultShipmentAdapter(mock, 2);

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);
                Assert.Equal(expectedPackageTypes.Count, testObject.PackageTypes.Count);

                expectedServices.Add(3, "Package Type 3");
                testObject.RefreshServiceTypes();
                Assert.Equal(expectedPackageTypes.Count, testObject.PackageTypes.Count);
            }
        }

        [Fact]
        public void Save_UpdatesShipmentAdapter_WithViewModelValue_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock, 2);

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);

                testObject.ShipDate = testObject.ShipDate.AddDays(1);
                testObject.ServiceType = testObject.ServiceType++;

                testObject.SelectedDimensionsProfile = dimensionsManager.Object.Profiles(testObject.PackageAdapters.First()).FirstOrDefault();

                testObject.Save();

                shipmentAdapter.VerifySet(sa => sa.ShipDate = testObject.ShipDate, Times.Once());
                shipmentAdapter.VerifySet(sa => sa.ServiceType = testObject.ServiceType);
            }
        }

        [Fact]
        public void SelectedRateChangedMessage_DelegatesTo_HandleSelectedRateChangedMessageAndUpdatesServiceType_Test()
        {
            IMessenger messenger = new TestMessenger();

            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                mock.Provide(messenger);

                Mock<IRateSelection> rateSelection = mock.Mock<IRateSelection>();
                rateSelection.Setup(rs => rs.ServiceType).Returns(99);

                rateSelectionFactory = mock.Mock<IRateSelectionFactory>();
                rateSelectionFactory.Setup(r => r.CreateRateSelection(It.IsAny<RateResult>()))
                    .Returns(rateSelection.Object);

                CreateDefaultShipmentAdapter(mock, 2);

                shipmentAdapter.SetupSet(sa => sa.ServiceType = rateSelection.Object.ServiceType);

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);

                SelectedRateChangedMessage message = new SelectedRateChangedMessage(this, new RateResult("test message", "1", 1, "test"));
                messenger.Send(message);

                shipmentAdapter.Verify(sa => sa.ServiceType);
                Assert.Equal(rateSelection.Object.ServiceType, testObject.ServiceType);
            }
        }

        [Fact]
        public void Save_UpdatesShipmentAdapterCustomsItems_WithViewModelValue_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock, 2);

                shipmentAdapter.Setup(sa => sa.CustomsAllowed).Returns(true);
                shipmentAdapter.Setup(sa => sa.CustomsItems).Returns(new EntityCollection<ShipmentCustomsItemEntity>());

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();

                testObject.CustomsAllowed = true;
                testObject.Load(shipmentAdapter.Object);

                testObject.ShipDate = testObject.ShipDate.AddDays(1);
                testObject.ServiceType = testObject.ServiceType++;
                testObject.SelectedDimensionsProfile = dimensionsManager.Object.Profiles(testObject.PackageAdapters.First()).FirstOrDefault();

                testObject.Save();

                shipmentAdapter.VerifySet(sa => sa.ShipDate = testObject.ShipDate, Times.Once());
                shipmentAdapter.VerifySet(sa => sa.ServiceType = testObject.ServiceType);
            }
        }

        [Fact]
        public void DeleteCustomsItemCommand_CanNotExecute_WhenSelectedCustomsItemIsNull_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock, 2);

                shipmentAdapter.Setup(sa => sa.CustomsAllowed).Returns(true);
                shipmentAdapter.Setup(sa => sa.CustomsItems).Returns(new EntityCollection<ShipmentCustomsItemEntity>());

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();

                testObject.CustomsAllowed = true;
                testObject.Load(shipmentAdapter.Object);
                testObject.SelectedCustomsItem = null;

                Assert.False(testObject.DeleteCustomsItemCommand.CanExecute(mock));
            }
        }

        [Fact]
        public void DeleteCustomsItemCommand_CanNotExecute_WhenCustomsItemsDoesNotContainSelectedCustomsItem_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock, 2);

                shipmentAdapter.Setup(sa => sa.CustomsAllowed).Returns(true);
                shipmentAdapter.Setup(sa => sa.CustomsItems).Returns(new EntityCollection<ShipmentCustomsItemEntity>());

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();

                testObject.CustomsAllowed = true;
                testObject.Load(shipmentAdapter.Object);
                testObject.SelectedCustomsItem = new ShipmentCustomsItemEntity(999);

                Assert.False(testObject.DeleteCustomsItemCommand.CanExecute(mock));
            }
        }

        [Fact]
        public void AddCustomsItemCommand_CanExecute_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock, 2);

                shipmentAdapter.Setup(sa => sa.CustomsAllowed).Returns(true);
                shipmentAdapter.Setup(sa => sa.CustomsItems).Returns(new EntityCollection<ShipmentCustomsItemEntity>());

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();

                testObject.CustomsAllowed = true;
                testObject.Load(shipmentAdapter.Object);
                testObject.SelectedCustomsItem = new ShipmentCustomsItemEntity(999);

                Assert.True(testObject.AddCustomsItemCommand.CanExecute(mock));
            }
        }

        [Fact]
        public void AddCustomsItemCommand_AddsCustomsItem_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock, 2);

                shipmentAdapter.Setup(sa => sa.CustomsAllowed).Returns(true);
                shipmentAdapter.Setup(sa => sa.CustomsItems).Returns(new EntityCollection<ShipmentCustomsItemEntity>());

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();

                testObject.CustomsAllowed = true;
                testObject.Load(shipmentAdapter.Object);
                testObject.AddCustomsItemCommand.Execute(null);

                Assert.Equal(1, testObject.CustomsItems.Count);
            }
        }

        [Fact]
        public void DeleteCustomsItemCommand_DeletesCustomsItem_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock, 2);

                shipmentAdapter.Setup(sa => sa.CustomsAllowed).Returns(true);
                shipmentAdapter.Setup(sa => sa.CustomsItems).Returns(
                    new EntityCollection<ShipmentCustomsItemEntity>()
                    {
                        new ShipmentCustomsItemEntity(0),
                        new ShipmentCustomsItemEntity(1),
                        new ShipmentCustomsItemEntity(2)
                    });

                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
                testObject.PropertyChanged += OnPropertyChanged;

                testObject.CustomsAllowed = true;
                testObject.Load(shipmentAdapter.Object);

                testObject.SelectedCustomsItem = testObject.CustomsItems.Skip(1).First();

                testObject.DeleteCustomsItemCommand.Execute(null);

                Assert.Equal(2, testObject.CustomsItems.Count);
            }
        }

        private Dictionary<int, string> CreateDefaultShipmentAdapter(AutoMock mock, int numberOfPackages)
        {
            shipmentServicesBuilder = mock.Mock<IShipmentServicesBuilder>();
            shipmentServicesBuilder.Setup(sb => sb.BuildServiceTypeDictionary(It.IsAny<IEnumerable<ShipmentEntity>>())).Returns(expectedServices);

            shipmentServicesBuilderFactory = mock.Mock<IShipmentServicesBuilderFactory>();
            shipmentServicesBuilderFactory.Setup(sbf => sbf.Get(It.IsAny<ShipmentTypeCode>())).Returns(shipmentServicesBuilder.Object);

            shipmentPackageTypesBuilder = mock.Mock<IShipmentPackageTypesBuilder>();
            shipmentPackageTypesBuilder.Setup(sb => sb.BuildPackageTypeDictionary(It.IsAny<IEnumerable<ShipmentEntity>>())).Returns(expectedPackageTypes);

            shipmentPackageTypesBuilderFactory = mock.Mock<IShipmentPackageTypesBuilderFactory>();
            shipmentPackageTypesBuilderFactory.Setup(sbf => sbf.Get(It.IsAny<ShipmentTypeCode>())).Returns(shipmentPackageTypesBuilder.Object);
            
            CreatePackageAdapters(numberOfPackages);

            shipmentAdapter = mock.Mock<ICarrierShipmentAdapter>();
            shipmentAdapter.Setup(sa => sa.ShipmentTypeCode).Returns(ShipmentTypeCode.UpsOnLineTools);
            shipmentAdapter.Setup(sa => sa.ServiceType).Returns((int) UpsServiceType.UpsGround);
            shipmentAdapter.Setup(sa => sa.ShipDate).Returns(new DateTime(2015, 1, 1, 1, 1, 1));
            shipmentAdapter.Setup(sa => sa.TotalWeight).Returns(0.5);
            shipmentAdapter.Setup(sa => sa.SupportsPackageTypes).Returns(true);
            shipmentAdapter.Setup(sa => sa.SupportsAccounts).Returns(true);
            shipmentAdapter.Setup(sa => sa.SupportsMultiplePackages).Returns(true);
            shipmentAdapter.Setup(sa => sa.GetPackageAdapters()).Returns(packageAdapters);
            shipmentAdapter.Setup(sa => sa.GetPackageAdapters(It.IsAny<int>())).Returns((int x) =>
                {
                    CreatePackageAdapters(x);
                    return packageAdapters;
                });

            shipmentAdapter.Setup(sa => sa.CustomsAllowed).Returns(false);
            shipmentAdapter.Setup(sa => sa.CustomsItems).Returns(new EntityCollection<ShipmentCustomsItemEntity>());

            return expectedServices;
        }

        private void CreatePackageAdapters(int numberOfPackages)
        {
            packageAdapters.Clear();

            for (int i = 1; i <= numberOfPackages; i++)
            {
                TestPackageAdapter packageAdapter = new TestPackageAdapter();
                packageAdapter.PropertyChanged += OnPropertyChanged;
                packageAdapter.PackagingType = new PackageTypeBinding() {PackageTypeID = (int) UpsPackagingType.Custom, Name = "Your Pakaging"};
                packageAdapter.AdditionalWeight = 0.1 * i;
                packageAdapter.ApplyAdditionalWeight = false;
                packageAdapter.Index = 1 * i;
                packageAdapter.DimsHeight = 2 * i;
                packageAdapter.DimsLength = 2 * i;
                packageAdapter.DimsWidth = 1 * i;
                packageAdapter.Weight = 0.5 * i;

                packageAdapters.Add(packageAdapter);
            }
        }

        private void CreateDimensionsProfilesManager(AutoMock mock)
        {
            List<DimensionsProfileEntity> dims = new List<DimensionsProfileEntity>()
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

            Mock<IDimensionsManager> dimsMgr = mock.Mock<IDimensionsManager>();
            dimsMgr.Setup(d => d.Profiles(It.IsAny<IPackageAdapter>())).Returns(dims);
        }

        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // Only used for testing.
        }

    }
}
