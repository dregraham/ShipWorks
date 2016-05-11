using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Services.Builders;
using ShipWorks.Shipping.UI.ShippingPanel.ShipmentControl;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.ShippingPanel.ShipmentControl
{
    public class BestRateShipmentViewModelTest : IDisposable
    {
        private readonly ShipmentEntity shipment;
        private Mock<ICarrierShipmentAdapter> shipmentAdapter;
        private readonly Mock<IDimensionsManager> dimensionsManager;
        private readonly List<IPackageAdapter> packageAdapters = new List<IPackageAdapter>();
        private readonly AutoMock mock;
        TestMessenger messenger;

        public BestRateShipmentViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

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

            dimensionsManager = mock.CreateMock<IDimensionsManager>();
            dimensionsManager.Setup(dm => dm.Profiles(It.IsAny<IPackageAdapter>())).Returns(dimensionsProfileEntities);

            shipment = new ShipmentEntity()
            {
                BestRate = new BestRateShipmentEntity()
            };
        }

        [Fact]
        public void ShipDate_MatchesShipmentAdapterValue()
        {
            CreateDefaultShipmentAdapter(mock);

            BestRateShipmentViewModel testObject = mock.Create<BestRateShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);

            Assert.Equal(shipmentAdapter.Object.ShipDate, testObject.ShipDate);
        }

        [Fact]
        public void ContentWeight_MatchesShipmentAdapterValue()
        {
            CreateDefaultShipmentAdapter(mock);

            BestRateShipmentViewModel testObject = mock.Create<BestRateShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);

            Assert.Equal(shipmentAdapter.Object.ContentWeight, testObject.ContentWeight);
        }

        [Fact]
        public void ServiceType_MatchesShipmentAdapterValue()
        {
            CreateDefaultShipmentAdapter(mock);

            BestRateShipmentViewModel testObject = mock.Create<BestRateShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);

            Assert.Equal(shipmentAdapter.Object.ServiceType, testObject.ServiceLevel);
        }

        [Fact]
        public void SupportsMultiplePackages_MatchesShipmentAdapterValue()
        {
            CreateDefaultShipmentAdapter(mock);

            BestRateShipmentViewModel testObject = mock.Create<BestRateShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);

            Assert.Equal(shipmentAdapter.Object.SupportsMultiplePackages, testObject.SupportsMultiplePackages);
        }

        [Fact]
        public void SupportsPackageTypes_MatchesShipmentAdapterValue()
        {
            CreateDefaultShipmentAdapter(mock);

            BestRateShipmentViewModel testObject = mock.Create<BestRateShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);

            Assert.Equal(shipmentAdapter.Object.SupportsPackageTypes, testObject.SupportsPackageTypes);
        }

        [Fact]
        public void PackageAdapters_HasOneEntry()
        {
            CreateDefaultShipmentAdapter(mock);

            BestRateShipmentViewModel testObject = mock.Create<BestRateShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);

            Assert.Equal(1, testObject.PackageAdapters.Count());
        }

        [Fact]
        public void SelectedPackageAdapter_DefaultsToFirstInList()
        {
            CreateDefaultShipmentAdapter(mock);

            BestRateShipmentViewModel testObject = mock.Create<BestRateShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);

            Assert.Equal(packageAdapters[0].Index, testObject.SelectedPackageAdapter.Index);
        }

        [Fact]
        public void DimensionsProfiles_MatchDimensionsProfilesManagerValues()
        {
            CreateDefaultShipmentAdapter(mock);
            CreateDimensionsProfilesManager(mock);

            BestRateShipmentViewModel testObject = mock.Create<BestRateShipmentViewModel>();

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

        [Fact]
        public void UpdateSelectedDimensionsProfile_MatchDimensionsProfilesManagerValues()
        {
            CreateDefaultShipmentAdapter(mock);
            CreateDimensionsProfilesManager(mock);

            BestRateShipmentViewModel testObject = mock.Create<BestRateShipmentViewModel>();

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

        [Fact]
        public void UpdateSelectedDimensionsProfile_ReturnsDefaultProfile_WhenSelectedDimensionsProfileIDIsNotInDimsManager()
        {
            CreateDefaultShipmentAdapter(mock);
            CreateDimensionsProfilesManager(mock);

            BestRateShipmentViewModel testObject = mock.Create<BestRateShipmentViewModel>();

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

            CreateDefaultShipmentAdapter(mock);
            CreateDimensionsProfilesManager(mock);

            IDimensionsManager dimsMgr = mock.Create<IDimensionsManager>();
            DimensionsProfileEntity dimsProfileEntity = new DimensionsProfileEntity(10);
            List<DimensionsProfileEntity> profiles = dimsMgr.Profiles(It.IsAny<IPackageAdapter>()).ToList();

            Mock<IDimensionsManager> mockedDimsMgr = mock.Mock<IDimensionsManager>();
            mockedDimsMgr.Setup(d => d.Profiles(It.IsAny<IPackageAdapter>())).Returns(profiles);

            BestRateShipmentViewModel testObject = mock.Create<BestRateShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);
            profiles.Add(dimsProfileEntity);
            mockedDimsMgr.Setup(d => d.Profiles(It.IsAny<IPackageAdapter>())).Returns(profiles);

            DimensionsProfilesChangedMessage message = new DimensionsProfilesChangedMessage(this);
            testMessenger.Send(message);

            Assert.Equal(profiles.Count, testObject.DimensionsProfiles.Count);

            testMessenger.Dispose();
        }

        [Fact]
        public void ManageDimensionsProfiles_UpdatesDimensionsProfiles_WhenSelectedDimsProfileIsNotDefaultAndDimensionsProfilesChangedMessageReceived()
        {
            TestMessenger testMessenger = new TestMessenger();
            mock.Provide<IMessenger>(testMessenger);

            CreateDefaultShipmentAdapter(mock);
            CreateDimensionsProfilesManager(mock);

            IDimensionsManager dimsMgr = mock.Create<IDimensionsManager>();
            DimensionsProfileEntity dimsProfileEntity = new DimensionsProfileEntity(10);
            List<DimensionsProfileEntity> profiles = dimsMgr.Profiles(It.IsAny<IPackageAdapter>()).ToList();

            Mock<IDimensionsManager> mockedDimsMgr = mock.Mock<IDimensionsManager>();
            mockedDimsMgr.Setup(d => d.Profiles(It.IsAny<IPackageAdapter>())).Returns(profiles);

            BestRateShipmentViewModel testObject = mock.Create<BestRateShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);
            testObject.SelectedDimensionsProfile = new DimensionsProfileEntity(999);

            profiles.Add(dimsProfileEntity);
            mockedDimsMgr.Setup(d => d.Profiles(It.IsAny<IPackageAdapter>())).Returns(profiles);

            DimensionsProfilesChangedMessage message = new DimensionsProfilesChangedMessage(this);
            testMessenger.Send(message);

            Assert.Equal(profiles.Count, testObject.DimensionsProfiles.Count);

            testMessenger.Dispose();
        }

        [Fact]
        public void SelectedDimensionsProfile_UpdatesSelectedPackageAdapter_WhenProfileExists()
        {
            CreateDefaultShipmentAdapter(mock);
            CreateDimensionsProfilesManager(mock);

            IDimensionsManager dimsMgr = mock.Create<IDimensionsManager>();
            DimensionsProfileEntity dimsProfileEntity = dimsMgr.Profiles(It.IsAny<IPackageAdapter>()).Skip(1).First();
            BestRateShipmentViewModel testObject = mock.Create<BestRateShipmentViewModel>();

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
            CreateDefaultShipmentAdapter(mock);
            CreateDimensionsProfilesManager(mock);

            shipmentAdapter.Setup(sa => sa.ShipmentTypeCode).Returns(ShipmentTypeCode.Other);

            BestRateShipmentViewModel testObject = mock.Create<BestRateShipmentViewModel>();

            testObject.Load(shipmentAdapter.Object);

            Assert.False(testObject.SupportsDimensions);
        }

        [Fact]
        public void SupportsDimensions_IsTrue_WhenShipmentTypeIsNotOther()
        {
            CreateDefaultShipmentAdapter(mock);
            CreateDimensionsProfilesManager(mock);

            shipmentAdapter.Setup(sa => sa.ShipmentTypeCode).Returns(ShipmentTypeCode.UpsOnLineTools);

            BestRateShipmentViewModel testObject = mock.Create<BestRateShipmentViewModel>();

            testObject.Load(shipmentAdapter.Object);

            Assert.True(testObject.SupportsDimensions);
        }

        [Fact]
        public void SelectedCustomsItem_UpdatesWithNewValue()
        {
            CreateDefaultShipmentAdapter(mock);

            ShipmentCustomsItemEntity shipmentCustomsItemEntity = new ShipmentCustomsItemEntity()
            {
                ShipmentCustomsItemID = 3,
                Shipment = shipmentAdapter.Object.Shipment,
                ShipmentID = shipmentAdapter.Object.Shipment.ShipmentID,
                Weight = 6.6
            };

            BestRateShipmentViewModel testObject = mock.Create<BestRateShipmentViewModel>();

            testObject.Load(shipmentAdapter.Object);
            testObject.SelectedCustomsItem = new ShipmentCustomsItemAdapter(new ShipmentCustomsItemEntity());
            testObject.SelectedCustomsItem = new ShipmentCustomsItemAdapter(shipmentCustomsItemEntity);

            Assert.Equal(shipmentCustomsItemEntity.ShipmentCustomsItemID, testObject.SelectedCustomsItem.ShipmentCustomsItemID);
            Assert.Equal(shipmentCustomsItemEntity.Weight, testObject.SelectedCustomsItem.Weight);
        }

        [Fact]
        public void OnSelectedCustomsItemPropertyChanged_UpdatesTotalCustomsValue_WhenUnitValueAndOrQuantityChanges()
        {
            CreateDefaultShipmentAdapter(mock);
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

            BestRateShipmentViewModel testObject = mock.Create<BestRateShipmentViewModel>();

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
            CreateDefaultShipmentAdapter(mock);
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

            BestRateShipmentViewModel testObject = mock.Create<BestRateShipmentViewModel>();

            testObject.Load(shipmentAdapter.Object);
            testObject.SelectedCustomsItem = new ShipmentCustomsItemAdapter(new ShipmentCustomsItemEntity());
            testObject.SelectedCustomsItem = new ShipmentCustomsItemAdapter(shipmentCustomsItemEntity);

            testObject.SelectedCustomsItem.Weight = 100;
            testObject.SelectedCustomsItem.Quantity = 2.5;

            double expectedValue = testObject.SelectedCustomsItem.Weight * testObject.SelectedCustomsItem.Quantity;

            Assert.Equal(expectedValue, testObject.ContentWeight);
        }

        [Fact]
        public void RedistributeContentWeight_UpdatesPackageAdapterWeights_WhenWeightAndOrQuantityChanges()
        {
            CreateDefaultShipmentAdapter(mock);
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

            BestRateShipmentViewModel testObject = mock.Create<BestRateShipmentViewModel>();

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
        public void Save_UpdatesShipmentAdapter_WithViewModelValue()
        {
            CreateDefaultShipmentAdapter(mock);

            BestRateShipmentViewModel testObject = mock.Create<BestRateShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);

            testObject.ShipDate = testObject.ShipDate.AddDays(1);
            testObject.ServiceLevel = (int) ServiceLevelType.Anytime;

            testObject.SelectedDimensionsProfile = dimensionsManager.Object.Profiles(testObject.PackageAdapters.First()).FirstOrDefault();

            testObject.Save();

            shipmentAdapter.VerifySet(sa => sa.ShipDate = testObject.ShipDate, Times.Once());
            Assert.Equal((int)ServiceLevelType.Anytime, testObject.ServiceLevel);
        }

        [Fact]
        public void Save_SetsContentWeightCorrectly_WhenSelectedPackageWeightHasChanged()
        {
            var package1 = mock.CreateMock<IPackageAdapter>(p => p.SetupProperty(x => x.Weight));
            package1.Object.Weight = 1;

            shipmentAdapter = mock.CreateMock<ICarrierShipmentAdapter>();
            shipmentAdapter.SetupProperty(x => x.ContentWeight);
            shipmentAdapter.Setup(x => x.GetPackageAdapters()).Returns(new[] { package1.Object });

            var testObject = mock.Create<BestRateShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);

            testObject.SelectedPackageAdapter = testObject.PackageAdapters.First();
            testObject.ContentWeight = 2;

            testObject.Save();

            Assert.Equal(2, shipmentAdapter.Object.ContentWeight);
            Assert.Equal(2, package1.Object.Weight);
        }

        [Fact]
        public void AddCustomsItem_DelegatesToShipmentAdapter_WithViewModelValue()
        {
            CreateDefaultShipmentAdapter(mock);

            shipmentAdapter.Setup(sa => sa.CustomsAllowed).Returns(true);

            BestRateShipmentViewModel testObject = mock.Create<BestRateShipmentViewModel>();

            testObject.CustomsAllowed = true;
            testObject.Load(shipmentAdapter.Object);

            testObject.ShipDate = testObject.ShipDate.AddDays(1);
            testObject.ServiceLevel = testObject.ServiceLevel++;
            testObject.SelectedDimensionsProfile = dimensionsManager.Object.Profiles(testObject.PackageAdapters.First()).FirstOrDefault();

            testObject.AddCustomsItemCommand.Execute(null);
            shipmentAdapter.Verify(sa => sa.AddCustomsItem(), Times.Once());
        }

        [Fact]
        public void DeleteCustomsItemCommand_CanNotExecute_WhenSelectedCustomsItemIsNull()
        {
            CreateDefaultShipmentAdapter(mock);

            shipmentAdapter.Setup(sa => sa.CustomsAllowed).Returns(true);

            BestRateShipmentViewModel testObject = mock.Create<BestRateShipmentViewModel>();

            testObject.CustomsAllowed = true;
            testObject.Load(shipmentAdapter.Object);
            testObject.SelectedCustomsItem = null;

            Assert.False(testObject.DeleteCustomsItemCommand.CanExecute(mock));
        }

        [Fact]
        public void DeleteCustomsItemCommand_CanNotExecute_WhenCustomsItemsDoesNotContainSelectedCustomsItem()
        {
            CreateDefaultShipmentAdapter(mock);

            shipmentAdapter.Setup(sa => sa.CustomsAllowed).Returns(true);

            BestRateShipmentViewModel testObject = mock.Create<BestRateShipmentViewModel>();

            testObject.CustomsAllowed = true;
            testObject.Load(shipmentAdapter.Object);
            testObject.SelectedCustomsItem = new ShipmentCustomsItemAdapter(new ShipmentCustomsItemEntity(999));

            Assert.False(testObject.DeleteCustomsItemCommand.CanExecute(mock));
        }

        [Fact]
        public void AddCustomsItemCommand_CanExecute()
        {
            CreateDefaultShipmentAdapter(mock);

            shipmentAdapter.Setup(sa => sa.CustomsAllowed).Returns(true);

            BestRateShipmentViewModel testObject = mock.Create<BestRateShipmentViewModel>();

            testObject.CustomsAllowed = true;
            testObject.Load(shipmentAdapter.Object);
            testObject.SelectedCustomsItem = new ShipmentCustomsItemAdapter(new ShipmentCustomsItemEntity(999));

            Assert.True(testObject.AddCustomsItemCommand.CanExecute(mock));
        }

        [Fact]
        public void AddCustomsItemCommand_AddsCustomsItem()
        {
            CreateDefaultShipmentAdapter(mock);

            shipmentAdapter.Setup(sa => sa.CustomsAllowed).Returns(true);

            BestRateShipmentViewModel testObject = mock.Create<BestRateShipmentViewModel>();

            testObject.CustomsAllowed = true;
            testObject.Load(shipmentAdapter.Object);
            testObject.AddCustomsItemCommand.Execute(null);

            Assert.Equal(1, testObject.CustomsItems.Count);
        }

        [Fact]
        public void DeleteCustomsItemCommand_DeletesCustomsItem()
        {
            CreateDefaultShipmentAdapter(mock);

            shipmentAdapter.Setup(sa => sa.CustomsAllowed).Returns(true);

            shipmentAdapter.Setup(sa => sa.GetCustomsItemAdapters())
                .Returns(new[] {
                    new ShipmentCustomsItemAdapter(new ShipmentCustomsItemEntity(0)),
                    new ShipmentCustomsItemAdapter(new ShipmentCustomsItemEntity(1)),
                    new ShipmentCustomsItemAdapter(new ShipmentCustomsItemEntity(2))
                });

            BestRateShipmentViewModel testObject = mock.Create<BestRateShipmentViewModel>();

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
                CreateDefaultShipmentAdapter(mock);
                shipmentAdapter.Setup(sa => sa.Shipment).Returns(shipment);

                BestRateShipmentViewModel testObject = mock.Create<BestRateShipmentViewModel>();

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
            CreateDefaultShipmentAdapter(mock);
            shipmentAdapter.Setup(sa => sa.Shipment).Returns(shipment);

            List<Mock<IPackageAdapter>> mockPackageAdapters = new List<Mock<IPackageAdapter>>()
                {
                    new Mock<IPackageAdapter>(),
                    new Mock<IPackageAdapter>()
                };

            shipmentAdapter.Setup(sa => sa.GetPackageAdapters()).Returns(mockPackageAdapters.Select(mpa => mpa.Object));

            BestRateShipmentViewModel testObject = mock.Create<BestRateShipmentViewModel>();

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
            CreateDefaultShipmentAdapter(mock);
            shipmentAdapter.Setup(sa => sa.Shipment).Returns(shipment);

            BestRateShipmentViewModel testObject = mock.Create<BestRateShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);

            Assert.Equal(string.Empty, testObject["TotalCustomsValue"]);
        }

        [Fact]
        public void Indexer_ReturnsError_WhenTotalCustomsValueIsInvalid()
        {
            messenger = new TestMessenger();
            mock.Provide<IMessenger>(messenger);

            shipment.Processed = false;
            CreateDefaultShipmentAdapter(mock);
            shipmentAdapter.Setup(sa => sa.Shipment).Returns(shipment);

            BestRateShipmentViewModel testObject = mock.Create<BestRateShipmentViewModel>();
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
            CreateDefaultShipmentAdapter(mock);
            shipmentAdapter.Setup(sa => sa.Shipment).Returns(shipment);

            BestRateShipmentViewModel testObject = mock.Create<BestRateShipmentViewModel>();
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
            CreateDefaultShipmentAdapter(mock);
            shipmentAdapter.Setup(sa => sa.Shipment).Returns(shipment);

            BestRateShipmentViewModel testObject = mock.Create<BestRateShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);

            Assert.Null(testObject.Error);
        }

        [Fact]
        public void AddPackage_DoesNotDelegateToShipmentAdapter_WhenMultiplePackagesAreNotSupported()
        {
            shipmentAdapter = mock.CreateMock<ICarrierShipmentAdapter>();
            shipmentAdapter.Setup(x => x.SupportsMultiplePackages).Returns(false);
            BestRateShipmentViewModel testObject = mock.Create<BestRateShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);

            testObject.AddPackageCommand?.Execute(null);

            shipmentAdapter.Verify(x => x.AddPackage(), Times.Never);
        }

        [Fact]
        public void DeletePackage_DoesNotDelegateToShipmentAdapter_WhenMultiplePackagesAreNotSupported()
        {
            shipmentAdapter = mock.CreateMock<ICarrierShipmentAdapter>();
            shipmentAdapter.Setup(x => x.SupportsMultiplePackages).Returns(false);
            BestRateShipmentViewModel testObject = mock.Create<BestRateShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);

            testObject.DeletePackageCommand?.Execute(null);

            shipmentAdapter.Verify(x => x.DeletePackage(It.IsAny<PackageAdapterWrapper>()), Times.Never);
        }

        [Fact]
        public void ServiceLevelTypes_Returns_SameValuesAsEnum()
        {
            shipmentAdapter = mock.CreateMock<ICarrierShipmentAdapter>();
            shipmentAdapter.Setup(x => x.SupportsMultiplePackages).Returns(false);
            BestRateShipmentViewModel testObject = mock.Create<BestRateShipmentViewModel>();
            testObject.Load(shipmentAdapter.Object);

            EnumList<ServiceLevelType> expectedEnumList = EnumHelper.GetEnumList<ServiceLevelType>();

            expectedEnumList.TrueForAll(slt => testObject.ServiceLevelTypes.ContainsKey((int) slt.Value));

            Assert.Equal(expectedEnumList.Count, testObject.ServiceLevelTypes.Count);
        }

        [Fact]
        public void RatesLoaded_IsFalse_WhenRatingHashDoesntMatch()
        {
            using (TestMessenger messenger = new TestMessenger())
            {
                mock.Provide<IMessenger>(messenger);

                shipment.Processed = false;
                CreateDefaultShipmentAdapter(mock);
                shipmentAdapter.Setup(sa => sa.Shipment).Returns(shipment);

                BestRateShipmentViewModel testObject = mock.Create<BestRateShipmentViewModel>();

                testObject.Load(shipmentAdapter.Object);

                messenger.Send(new RatesRetrievingMessage(this, "ignore_me_rating_hash"));
                messenger.Send(new RatesRetrievedMessage(this, "retrieved_rating_hash", new GenericResult<RateGroup>(), shipmentAdapter.Object));

                Assert.False(testObject.RatesLoaded);
            }
        }

        [Fact]
        public void RatesLoaded_IsTrue_WhenRatingHashDoesMatch()
        {
            using (TestMessenger messenger = new TestMessenger())
            {
                mock.Provide<IMessenger>(messenger);

                shipment.Processed = false;
                CreateDefaultShipmentAdapter(mock);
                shipmentAdapter.Setup(sa => sa.Shipment).Returns(shipment);

                BestRateShipmentViewModel testObject = mock.Create<BestRateShipmentViewModel>();

                testObject.Load(shipmentAdapter.Object);

                messenger.Send(new RatesRetrievingMessage(this, "honor_me_rating_hash"));
                messenger.Send(new RatesRetrievedMessage(this, "honor_me_rating_hash", new GenericResult<RateGroup>(), shipmentAdapter.Object));

                Assert.True(testObject.RatesLoaded);
            }
        }

        private void CreateDefaultShipmentAdapter(AutoMock autoMock)
        {
            CreatePackageAdapters();

            shipmentAdapter = autoMock.Mock<ICarrierShipmentAdapter>();
            shipmentAdapter.Setup(sa => sa.ShipmentTypeCode).Returns(ShipmentTypeCode.BestRate);
            shipmentAdapter.Setup(sa => sa.ServiceType).Returns((int)UpsServiceType.UpsGround);
            shipmentAdapter.Setup(sa => sa.ShipDate).Returns(new DateTime(2015, 1, 1, 1, 1, 1));
            shipmentAdapter.Setup(sa => sa.ContentWeight).Returns(0.5);
            shipmentAdapter.Setup(sa => sa.SupportsPackageTypes).Returns(false);
            shipmentAdapter.Setup(sa => sa.SupportsAccounts).Returns(false);
            shipmentAdapter.Setup(sa => sa.SupportsMultiplePackages).Returns(false);
            shipmentAdapter.Setup(sa => sa.GetPackageAdapters()).Returns(packageAdapters);

            shipmentAdapter.Setup(sa => sa.CustomsAllowed).Returns(false);

            shipmentAdapter.Setup(sa => sa.Shipment).Returns(shipment);
        }

        private void CreatePackageAdapters()
        {
            packageAdapters.Clear();

            TestPackageAdapter packageAdapter = new TestPackageAdapter();
            packageAdapter.PackagingType = (int) UpsPackagingType.Custom;
            packageAdapter.AdditionalWeight = 0.1;
            packageAdapter.ApplyAdditionalWeight = false;
            packageAdapter.Index = 1;
            packageAdapter.DimsHeight = 2;
            packageAdapter.DimsLength = 2;
            packageAdapter.DimsWidth = 1;
            packageAdapter.Weight = 0.5;
            packageAdapter.InsuranceChoice = mock.Create<IInsuranceChoice>();

            packageAdapters.Add(packageAdapter);
        }

        private void CreateDimensionsProfilesManager(AutoMock autoMock)
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

            Mock<IDimensionsManager> dimsMgr = autoMock.Mock<IDimensionsManager>();
            dimsMgr.Setup(d => d.Profiles(It.IsAny<IPackageAdapter>())).Returns(dims);
        }

        public void Dispose()
        {
            messenger?.Dispose();
            mock?.Dispose();
        }
    }
}
