﻿using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Carriers.Other;
using ShipWorks.Shipping.Carriers.Postal.Other;
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
    public class OtherShipmentViewModelTest
    {
        private readonly ShipmentEntity shipment = new ShipmentEntity(1);
        private Mock<ICarrierShipmentAdapter> shipmentAdapter;
        private readonly List<OtherPackageAdapter> packageAdapters = new List<OtherPackageAdapter>();

        public OtherShipmentViewModelTest()
        {
            shipment.Other = new OtherShipmentEntity(shipment.ShipmentID);
        }

        [Fact]
        public void ShipDate_MatchesShipmentAdapterValue_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock);

                OtherShipmentViewModel testObject = mock.Create<OtherShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);

                Assert.Equal(shipmentAdapter.Object.ShipDate, testObject.ShipDate);
            }
        }

        [Fact]
        public void TotalWeight_MatchesShipmentAdapterValue_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock);

                OtherShipmentViewModel testObject = mock.Create<OtherShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);

                Assert.Equal(shipmentAdapter.Object.TotalWeight, testObject.TotalWeight);
            }
        }
        
        [Fact]
        public void Service_MatchesShipmentValue_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock);

                OtherShipmentViewModel testObject = mock.Create<OtherShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);
                testObject.Service = "a new service";
                testObject.Save();

                Assert.Equal(testObject.Service, shipment.Other.Service);
            }
        }

        [Fact]
        public void CarrierName_MatchesShipmentValue_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock);

                OtherShipmentViewModel testObject = mock.Create<OtherShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);
                testObject.CarrierName = "a new carrier";
                testObject.Save();

                Assert.Equal(testObject.CarrierName, shipment.Other.Carrier);
            }
        }

        [Fact]
        public void TrackingNumber_MatchesShipmentValue_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock);

                OtherShipmentViewModel testObject = mock.Create<OtherShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);
                testObject.TrackingNumber = "1zasdf";
                testObject.Save();

                Assert.Equal(testObject.TrackingNumber, shipment.TrackingNumber);
            }
        }

        [Fact]
        public void ShipmentCost_MatchesShipmentValue_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock);

                OtherShipmentViewModel testObject = mock.Create<OtherShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);
                testObject.Cost = 3.93M;
                testObject.Save();

                Assert.Equal(testObject.Cost, shipment.ShipmentCost);
            }
        }

        [Fact]
        public void CustomsItems_IsNotLoaded_WhenCustomsIsNotAllowed_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock);

                ShipmentCustomsItemEntity shipmentCustomsItemEntity = new ShipmentCustomsItemEntity()
                {
                    ShipmentCustomsItemID = 3,
                    ShipmentID = shipmentAdapter.Object.Shipment.ShipmentID,
                    Weight = 6.6
                };

                OtherShipmentViewModel testObject = mock.Create<OtherShipmentViewModel>();
                shipmentAdapter.Setup(sa => sa.IsDomestic).Returns(true);

                testObject.Load(shipmentAdapter.Object);
                testObject.SelectedCustomsItem = new ShipmentCustomsItemEntity();
                testObject.SelectedCustomsItem = shipmentCustomsItemEntity;

                Assert.Null(testObject.CustomsItems);
            }
        }

        [Fact]
        public void SelectedCustomsItem_UpdatesWithNewValue_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock);

                ShipmentCustomsItemEntity shipmentCustomsItemEntity = new ShipmentCustomsItemEntity()
                {
                    ShipmentCustomsItemID = 3,
                    ShipmentID = shipmentAdapter.Object.Shipment.ShipmentID,
                    Weight = 6.6
                };

                OtherShipmentViewModel testObject = mock.Create<OtherShipmentViewModel>();
                
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

                shipmentAdapter.Setup(sa => sa.CustomsItems).Returns(
                    new EntityCollection<ShipmentCustomsItemEntity>()
                    {
                        shipmentCustomsItemEntity
                    });

                OtherShipmentViewModel testObject = mock.Create<OtherShipmentViewModel>();

                testObject.Load(shipmentAdapter.Object);
                testObject.SelectedCustomsItem = new ShipmentCustomsItemEntity();
                testObject.SelectedCustomsItem = shipmentCustomsItemEntity;

                decimal originalTotalValue = testObject.TotalCustomsValue;
                testObject.SelectedCustomsItem.UnitValue = 100;
                testObject.SelectedCustomsItem.Quantity = 2.5;

                decimal expectedValue = testObject.SelectedCustomsItem.UnitValue * (decimal)testObject.SelectedCustomsItem.Quantity;

                Assert.Equal(expectedValue, testObject.TotalCustomsValue);
            }
        }

        [Fact]
        public void OnSelectedCustomsItemPropertyChanged_UpdatesShipmentContentWeight_WhenWeightAndOrQuantityChanges_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
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

                shipmentAdapter.Setup(sa => sa.CustomsItems).Returns(
                    new EntityCollection<ShipmentCustomsItemEntity>()
                    {
                        shipmentCustomsItemEntity
                    });

                OtherShipmentViewModel testObject = mock.Create<OtherShipmentViewModel>();

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
        public void DeleteCustomsItemCommand_CanNotExecute_WhenSelectedCustomsItemIsNull_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock);

                shipmentAdapter.Setup(sa => sa.CustomsAllowed).Returns(true);
                shipmentAdapter.Setup(sa => sa.CustomsItems).Returns(new EntityCollection<ShipmentCustomsItemEntity>());

                OtherShipmentViewModel testObject = mock.Create<OtherShipmentViewModel>();

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
                CreateDefaultShipmentAdapter(mock);

                shipmentAdapter.Setup(sa => sa.CustomsAllowed).Returns(true);
                shipmentAdapter.Setup(sa => sa.CustomsItems).Returns(new EntityCollection<ShipmentCustomsItemEntity>());

                OtherShipmentViewModel testObject = mock.Create<OtherShipmentViewModel>();

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
                CreateDefaultShipmentAdapter(mock);

                shipmentAdapter.Setup(sa => sa.CustomsAllowed).Returns(true);
                shipmentAdapter.Setup(sa => sa.CustomsItems).Returns(new EntityCollection<ShipmentCustomsItemEntity>());

                OtherShipmentViewModel testObject = mock.Create<OtherShipmentViewModel>();

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
                CreateDefaultShipmentAdapter(mock);

                shipmentAdapter.Setup(sa => sa.CustomsAllowed).Returns(true);
                shipmentAdapter.Setup(sa => sa.CustomsItems).Returns(new EntityCollection<ShipmentCustomsItemEntity>());

                OtherShipmentViewModel testObject = mock.Create<OtherShipmentViewModel>();

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
                CreateDefaultShipmentAdapter(mock);

                shipmentAdapter.Setup(sa => sa.CustomsAllowed).Returns(true);
                shipmentAdapter.Setup(sa => sa.CustomsItems).Returns(
                    new EntityCollection<ShipmentCustomsItemEntity>()
                    {
                        new ShipmentCustomsItemEntity(0),
                        new ShipmentCustomsItemEntity(1),
                        new ShipmentCustomsItemEntity(2)
                    });

                OtherShipmentViewModel testObject = mock.Create<OtherShipmentViewModel>();
                testObject.PropertyChanged += OnPropertyChanged;

                testObject.CustomsAllowed = true;
                testObject.Load(shipmentAdapter.Object);

                testObject.SelectedCustomsItem = testObject.CustomsItems.Skip(1).First();

                testObject.DeleteCustomsItemCommand.Execute(null);

                Assert.Equal(2, testObject.CustomsItems.Count);
            }
        }

        [Fact]
        public void RefreshServiceTypes_DoesNotThrow_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock);

                OtherShipmentViewModel testObject = mock.Create<OtherShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);
                testObject.RefreshServiceTypes();
            }
        }

        [Fact]
        public void RefreshPackageTypes_DoesNotThrow_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                CreateDefaultShipmentAdapter(mock);

                OtherShipmentViewModel testObject = mock.Create<OtherShipmentViewModel>();
                testObject.Load(shipmentAdapter.Object);
                testObject.RefreshPackageTypes();
            }
        }

        private void CreateDefaultShipmentAdapter(AutoMock mock)
        {
            CreatePackageAdapters(1);

            shipmentAdapter = mock.Mock<ICarrierShipmentAdapter>();
            shipmentAdapter.Setup(sa => sa.Shipment).Returns(shipment);
            shipmentAdapter.Setup(sa => sa.ShipmentTypeCode).Returns(ShipmentTypeCode.Other);
            shipmentAdapter.Setup(sa => sa.ShipDate).Returns(new DateTime(2015, 1, 1, 1, 1, 1));
            shipmentAdapter.Setup(sa => sa.TotalWeight).Returns(0.5);
            shipmentAdapter.Setup(sa => sa.SupportsPackageTypes).Returns(false);
            shipmentAdapter.Setup(sa => sa.SupportsAccounts).Returns(false);
            shipmentAdapter.Setup(sa => sa.SupportsMultiplePackages).Returns(false);
            shipmentAdapter.Setup(sa => sa.GetPackageAdapters()).Returns(packageAdapters);
            shipmentAdapter.Setup(sa => sa.GetPackageAdapters(It.IsAny<int>())).Returns((int x) =>
                {
                    CreatePackageAdapters(x);
                    return packageAdapters;
                });

            shipmentAdapter.Setup(sa => sa.CustomsAllowed).Returns(false);
            shipmentAdapter.Setup(sa => sa.CustomsItems).Returns(new EntityCollection<ShipmentCustomsItemEntity>());
        }

        private void CreatePackageAdapters(int numberOfPackages)
        {
            packageAdapters.Clear();

            for (int i = 1; i <= numberOfPackages; i++)
            {
                OtherPackageAdapter packageAdapter = new OtherPackageAdapter(shipment);
                packageAdapter.PackagingType = new PackageTypeBinding() {PackageTypeID = (int) UpsPackagingType.Custom, Name = "Your Packaging"};
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

        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
        }
    }
}
