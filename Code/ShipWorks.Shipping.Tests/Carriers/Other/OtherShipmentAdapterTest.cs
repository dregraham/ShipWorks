using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Other;
using ShipWorks.Shipping.Services;
using ShipWorks.Stores;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Other
{
    public class OtherShipmentAdapterTest : IDisposable
    {
        readonly AutoMock mock;
        readonly ShipmentEntity shipment;

        public OtherShipmentAdapterTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            shipment = new ShipmentEntity
            {
                ShipmentTypeCode = ShipmentTypeCode.Other,
                ShipDate = new DateTime(2015, 11, 24, 10, 10, 10),
                ContentWeight = 1,
                TotalWeight = 1,
                Insurance = true,
            };
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExcpetion_WhenShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new OtherShipmentAdapter(null, mock.Create<IShipmentTypeManager>(), mock.Create<ICustomsManager>(),
                    mock.Create<IStoreManager>()));
            Assert.Throws<ArgumentNullException>(() =>
                new OtherShipmentAdapter(shipment, null, mock.Create<ICustomsManager>(),
                    mock.Create<IStoreManager>()));
            Assert.Throws<ArgumentNullException>(() =>
                new OtherShipmentAdapter(shipment, mock.Create<IShipmentTypeManager>(), null,
                    mock.Create<IStoreManager>()));
        }

        [Fact]
        public void AccountId_ReturnsNull()
        {
            var testObject = mock.Create<OtherShipmentAdapter>(TypedParameter.From(shipment));
            Assert.Null(testObject.AccountId);
        }

        [Fact]
        public void AccountId_DoesNotThrow_WhenValueIsValid()
        {
            var testObject = mock.Create<OtherShipmentAdapter>(TypedParameter.From(shipment));
            testObject.AccountId = 6;
        }

        [Fact]
        public void AccountId_DoesNotThrow_WhenValueIsNull()
        {
            var testObject = mock.Create<OtherShipmentAdapter>(TypedParameter.From(shipment));
            testObject.AccountId = null;
        }

        [Fact]
        public void Shipment_IsNotNull()
        {
            var testObject = mock.Create<OtherShipmentAdapter>(TypedParameter.From(shipment));
            Assert.NotNull(testObject.Shipment);
        }

        [Fact]
        public void ShipmentTypeCode_IsOther()
        {
            var testObject = mock.Create<OtherShipmentAdapter>(TypedParameter.From(shipment));
            Assert.Equal(ShipmentTypeCode.Other, testObject.ShipmentTypeCode);
        }

        [Fact]
        public void SupportsAccounts_IsFalse()
        {
            var testObject = mock.Create<OtherShipmentAdapter>(TypedParameter.From(shipment));

            Assert.False(testObject.SupportsAccounts);
        }

        [Fact]
        public void SupportsMultiplePackages_IsFalse()
        {
            var testObject = mock.Create<OtherShipmentAdapter>(TypedParameter.From(shipment));
            Assert.False(testObject.SupportsMultiplePackages);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, true)]
        public void IsDomestic_DelegatesToIsDomestic_OnShipmentType(bool isDomestic, bool expected)
        {
            mock.WithShipmentTypeFromShipmentManager(x => x.Setup(b => b.IsDomestic(shipment)).Returns(isDomestic));

            var testObject = mock.Create<OtherShipmentAdapter>(TypedParameter.From(shipment));

            Assert.Equal(expected, testObject.IsDomestic);
        }

        [Fact]
        public void UpdateDynamicData_DelegatesToShipmentTypeAndCustomsManager()
        {
            var shipmentType = mock.WithShipmentTypeFromShipmentManager();

            var testObject = mock.Create<OtherShipmentAdapter>(TypedParameter.From(shipment));
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

            var testObject = mock.Create<OtherShipmentAdapter>(TypedParameter.From(shipment));

            Assert.NotNull(testObject.UpdateDynamicData());
            Assert.Equal(1, testObject.UpdateDynamicData().Count);
        }

        [Fact]
        public void SupportsPackageTypes_IsFalse()
        {
            var testObject = mock.Create<OtherShipmentAdapter>(TypedParameter.From(shipment));

            Assert.False(testObject.SupportsPackageTypes);
        }

        [Fact]
        public void ShipDate_ReturnsShipmentValue()
        {
            var testObject = mock.Create<OtherShipmentAdapter>(TypedParameter.From(shipment));
            Assert.Equal(shipment.ShipDate, testObject.ShipDate);
        }

        [Fact]
        public void ShipDate_IsUpdated()
        {
            var testObject = mock.Create<OtherShipmentAdapter>(TypedParameter.From(shipment));

            testObject.ShipDate = testObject.ShipDate.AddDays(1);

            Assert.Equal(shipment.ShipDate, testObject.ShipDate);
        }

        [Fact]
        public void AddCustomsItem_AddsNewCustomsItemToList()
        {
            var testObject = mock.Create<OtherShipmentAdapter>(TypedParameter.From(shipment));
            testObject.AddCustomsItem();

            Assert.Equal(1, shipment.CustomsItems.Count);
        }

        [Fact]
        public void AddCustomsItem_ReturnsCustomsItemAdapter_ForNewCustomsItem()
        {
            var testObject = mock.Create<OtherShipmentAdapter>(TypedParameter.From(shipment));
            var newCustomsItem = testObject.AddCustomsItem();

            Assert.NotNull(newCustomsItem);
        }

        [Fact]
        public void AddCustomsItem_DelegatesToShipmentType_WhenNewCustomsItemIsAdded()
        {
            var shipmentTypeMock = mock.WithShipmentTypeFromShipmentManager(x => { });

            var testObject = mock.Create<OtherShipmentAdapter>(TypedParameter.From(shipment));
            testObject.AddCustomsItem();

            shipmentTypeMock.Verify(x => x.UpdateDynamicShipmentData(shipment));
            shipmentTypeMock.Verify(x => x.UpdateTotalWeight(shipment));
        }

        [Fact]
        public void AddCustomsItem_DelegatesToCustomsManager_WhenNewCustomsItemIsAdded()
        {
            var testObject = mock.Create<OtherShipmentAdapter>(TypedParameter.From(shipment));
            testObject.AddCustomsItem();

            mock.Mock<ICustomsManager>().Verify(x => x.EnsureCustomsLoaded(new[] { shipment }));
        }

        [Fact]
        public void DeleteCustomsItem_RemovesCustomsItemAssociatedWithAdapter()
        {
            var customsItem = new ShipmentCustomsItemEntity(2);

            shipment.CustomsItems.Add(new ShipmentCustomsItemEntity(1));
            shipment.CustomsItems.Add(customsItem);
            shipment.CustomsItems.Add(new ShipmentCustomsItemEntity(3));

            var testObject = mock.Create<OtherShipmentAdapter>(TypedParameter.From(shipment));
            testObject.DeleteCustomsItem(new ShipmentCustomsItemAdapter(customsItem));

            Assert.DoesNotContain(customsItem, shipment.CustomsItems);
        }

        [Fact]
        public void DeleteCustomsItem_DoesNotRemoveCustomsItem_WhenCustomsItemDoesNotExist()
        {
            var CustomsItem = new ShipmentCustomsItemEntity(2);
            var CustomsItem2 = new ShipmentCustomsItemEntity(2);

            shipment.CustomsItems.Add(CustomsItem);
            shipment.CustomsItems.Add(CustomsItem2);

            var testObject = mock.Create<OtherShipmentAdapter>(TypedParameter.From(shipment));
            testObject.DeleteCustomsItem(new ShipmentCustomsItemAdapter(new ShipmentCustomsItemEntity(12)));

            Assert.Contains(CustomsItem, shipment.CustomsItems);
            Assert.Contains(CustomsItem2, shipment.CustomsItems);
        }

        [Fact]
        public void DeleteCustomsItem_DelegatesToShipmentType_WhenCustomsItemIsRemoved()
        {
            var shipmentType = mock.WithShipmentTypeFromShipmentManager(x => { });

            var CustomsItem = new ShipmentCustomsItemEntity(2);

            shipment.CustomsItems.Add(CustomsItem);
            shipment.CustomsItems.Add(new ShipmentCustomsItemEntity(3));

            var testObject = mock.Create<OtherShipmentAdapter>(TypedParameter.From(shipment));
            testObject.DeleteCustomsItem(new ShipmentCustomsItemAdapter(CustomsItem));

            shipmentType.Verify(x => x.UpdateDynamicShipmentData(shipment));
            shipmentType.Verify(x => x.UpdateTotalWeight(shipment));
        }

        [Fact]
        public void DeleteCustomsItem_DelegatesToCustomsManager_WhenCustomsItemIsRemoved()
        {
            var CustomsItem = new ShipmentCustomsItemEntity(2);

            shipment.CustomsItems.Add(CustomsItem);
            shipment.CustomsItems.Add(new ShipmentCustomsItemEntity(3));

            var testObject = mock.Create<OtherShipmentAdapter>(TypedParameter.From(shipment));
            testObject.DeleteCustomsItem(new ShipmentCustomsItemAdapter(CustomsItem));

            mock.Mock<ICustomsManager>().Verify(x => x.EnsureCustomsLoaded(new[] { shipment }));
        }

        [Fact]
        public void DeleteCustomsItem_AddsCustomsItemToRemovedEntityCollection_WhenCustomsItemIsRemoved()
        {
            var CustomsItem = new ShipmentCustomsItemEntity(2);

            shipment.CustomsItems.Add(CustomsItem);
            shipment.CustomsItems.Add(new ShipmentCustomsItemEntity(3));

            var testObject = mock.Create<OtherShipmentAdapter>(TypedParameter.From(shipment));
            testObject.DeleteCustomsItem(new ShipmentCustomsItemAdapter(CustomsItem));

            Assert.Contains(CustomsItem, shipment.CustomsItems.RemovedEntitiesTracker.OfType<ShipmentCustomsItemEntity>());
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
