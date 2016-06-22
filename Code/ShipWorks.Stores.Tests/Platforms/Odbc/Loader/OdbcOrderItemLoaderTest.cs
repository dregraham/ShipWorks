using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.Loaders;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System;
using System.Collections.Generic;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Loader
{
    public class OdbcOrderItemLoaderTest : IDisposable
    {
        private readonly Mock<IOdbcFieldMap> clonedMap;
        private readonly AutoMock mock;
        private Mock<IOdbcFieldMapEntry> odbcFieldMapEntry;

        private readonly OrderEntity order;

        private readonly Mock<IOdbcFieldMap> originalMap;
        private readonly OdbcOrderItemLoader testObject;
        private readonly Mock<IOdbcItemAttributeLoader> attributeLoader;

        public OdbcOrderItemLoaderTest()
        {
            mock = AutoMock.GetLoose();

            clonedMap = new Mock<IOdbcFieldMap>();
            SetOdbcFieldMapEntry("display", "value");

            clonedMap.SetupGet(m => m.Entries)
                .Returns(() => new List<IOdbcFieldMapEntry> {odbcFieldMapEntry.Object});
            clonedMap.Setup(m => m.FindEntriesBy(It.IsAny<EntityType[]>(), It.IsAny<int>(), false))
                .Returns(() => new List<IOdbcFieldMapEntry> {odbcFieldMapEntry.Object});
            clonedMap.Setup(m => m.FindEntriesBy(It.IsAny<EntityField2>(), false))
                .Returns(() => new List<IOdbcFieldMapEntry> {odbcFieldMapEntry.Object});

            originalMap = new Mock<IOdbcFieldMap>();
            originalMap.Setup(m => m.Clone()).Returns(clonedMap.Object);

            order = new OrderEntity();

            attributeLoader = mock.Mock<IOdbcItemAttributeLoader>();
            testObject = mock.Create<OdbcOrderItemLoader>();
        }

        [Fact]
        public void Load_MapCloned()
        {
            testObject.Load(originalMap.Object, new OrderEntity(), new List<OdbcRecord>());
            originalMap.Verify(m => m.Clone(), Times.Once);
        }

        [Fact]
        public void Load_EnsureApplyValuesNotCalledOnOriginalMap()
        {
            testObject.Load(originalMap.Object, new OrderEntity(), new List<OdbcRecord> {new OdbcRecord()});
            originalMap.Verify(m => m.ApplyValues(It.IsAny<OdbcRecord>()), Times.Never);
        }

        [Fact]
        public void Load_EnsureCopyToEntityNotCalledOnOriginalMap()
        {
            testObject.Load(originalMap.Object, new OrderEntity(), new List<OdbcRecord> {new OdbcRecord()});
            originalMap.Verify(m => m.CopyToEntity(It.IsAny<IEntity2>()), Times.Never);
        }

        [Fact]
        public void Load_ApplyValuesCalledForEachOdbcRecord()
        {
            var record1 = new OdbcRecord();
            var record2 = new OdbcRecord();
            testObject.Load(originalMap.Object, new OrderEntity(), new List<OdbcRecord> {record1, record2});

            clonedMap.Verify(m => m.ApplyValues(record1), Times.Once);
            clonedMap.Verify(m => m.ApplyValues(record2), Times.Once);
            clonedMap.Verify(m => m.ApplyValues(It.IsAny<OdbcRecord>()), Times.Exactly(2));
        }

        [Fact]
        public void Load_OrderItemCountOfOrderMatchesNumberOfOdbcRecords()
        {
            testObject.Load(originalMap.Object, order, new List<OdbcRecord> {new OdbcRecord(), new OdbcRecord()});

            Assert.Equal(2, order.OrderItems.Count);
        }

        [Fact]
        public void Load_CopyToEntityCalledForEachOdbcRecord()
        {
            testObject.Load(originalMap.Object, order, new List<OdbcRecord> {new OdbcRecord(), new OdbcRecord()});

            clonedMap.Verify(m => m.CopyToEntity(order.OrderItems[0], 0), Times.Once);
            clonedMap.Verify(m => m.CopyToEntity(order.OrderItems[1], 0), Times.Once);
            clonedMap.Verify(m => m.CopyToEntity(It.IsAny<OrderItemEntity>(), 0), Times.Exactly(2));
        }


        [Fact]
        public void Load_AttributeLoaderLoadCalledForEachOdbcRecord()
        {
            testObject.Load(originalMap.Object, order, new List<OdbcRecord> {new OdbcRecord(), new OdbcRecord()});

            attributeLoader.Verify(m => m.Load(clonedMap.Object, order.OrderItems[0], 0), Times.Once);
            attributeLoader.Verify(m => m.Load(clonedMap.Object, order.OrderItems[1], 0), Times.Once);
            attributeLoader.Verify(m => m.Load(It.IsAny<IOdbcFieldMap>(), It.IsAny<OrderItemEntity>(), 0),
                Times.Exactly(2));
        }

        [Fact]
        public void Load_OrderItemNotAdded_WhenNoValuesRetrieved()
        {
            clonedMap.Setup(m => m.FindEntriesBy(It.IsAny<EntityType[]>(), It.IsAny<int>(), false))
                .Returns(new List<IOdbcFieldMapEntry>());

            testObject.Load(originalMap.Object, order, new List<OdbcRecord> {new OdbcRecord()});

            Assert.Empty(order.OrderItems);
        }

        [Fact]
        public void Load_OneItemCreated_WhenMaxIndexIs0()
        {
            testObject.Load(originalMap.Object, order, new List<OdbcRecord> {new OdbcRecord()});

            Assert.Equal(1, order.OrderItems.Count);
        }

        [Fact]
        public void Load_TwoItemsCreated_WhenMaxIndexIs1()
        {
            SetOdbcFieldMapEntry("name", "value", 1);

            testObject.Load(originalMap.Object, order, new List<OdbcRecord> {new OdbcRecord()});

            Assert.Equal(2, order.OrderItems.Count);
        }

        [Fact]
        public void Load_AttributeLoaderCalledForEachIndex_WhenMaxIndexIs1()
        {
            SetOdbcFieldMapEntry("name", "value", 1);

            testObject.Load(originalMap.Object, order, new List<OdbcRecord> {new OdbcRecord()});

            attributeLoader.Verify(l => l.Load(clonedMap.Object, order.OrderItems[0], 0), Times.Once);
            attributeLoader.Verify(l => l.Load(clonedMap.Object, order.OrderItems[1], 1), Times.Once);
            attributeLoader.Verify(
                l => l.Load(It.IsAny<IOdbcFieldMap>(), It.IsAny<OrderItemEntity>(), It.IsAny<int>()),
                Times.Exactly(2));
        }

        #region "Unit Cost Tests"
        [Fact]
        public void Load_UnitCostSetToMappedUnitCost_WhenUnitCostExistsInMap()
        {
            SetOdbcFieldMapEntry(EnumHelper.GetDescription(OdbcOrderFieldDescription.ItemUnitCost), 42M);

            testObject.Load(originalMap.Object, order, new List<OdbcRecord> {new OdbcRecord()});

            Assert.Equal(42M, order.OrderItems[0].UnitCost);
        }


        [Fact]
        public void Load_UnitCostCalculatedCorrectly_WhenUnitCostNotInMap_AndTotalCostInMap_AndItemQuantityGreaterThanZero()
        {
            SetOdbcFieldMapEntry(EnumHelper.GetDescription(OdbcOrderFieldDescription.ItemTotalCost), 42M);
            SetOrderItemQuantity(clonedMap, 2D, 0);
            
            testObject.Load(originalMap.Object, order, new List<OdbcRecord> {new OdbcRecord()});

            Assert.Equal(21M, order.OrderItems[0].UnitCost);
        }

        [Fact]
        public void Load_UnitCostZero_WhenUnitCostNotInMap_AndTotalCostInMap_AndItemQuantityZero()
        {
            SetOdbcFieldMapEntry(EnumHelper.GetDescription(OdbcOrderFieldDescription.ItemTotalCost), 42M);
            SetOrderItemQuantity(clonedMap, 0D, 0);

            testObject.Load(originalMap.Object, order, new List<OdbcRecord> {new OdbcRecord()});

            Assert.Equal(0M, order.OrderItems[0].UnitCost);
        }

        [Fact]
        public void Load_UnitCostZero_WhenUnitCostAndTotalCostNotInMap()
        {
            SetOrderItemQuantity(clonedMap, 5D, 0);

            testObject.Load(originalMap.Object, order, new List<OdbcRecord> {new OdbcRecord()});

            Assert.Equal(0M, order.OrderItems[0].UnitCost);
        }
        #endregion "Unit Cost Tests"

        #region "Unit Price Tests"
        [Fact]
        public void Load_UnitPriceSetToMappedUnitPrice_WhenUnitPriceExistsInMap()
        {
            SetOdbcFieldMapEntry(EnumHelper.GetDescription(OdbcOrderFieldDescription.ItemUnitPrice), 42M);

            testObject.Load(originalMap.Object, order, new List<OdbcRecord> { new OdbcRecord() });

            Assert.Equal(42M, order.OrderItems[0].UnitPrice);
        }

        [Fact]
        public void Load_UnitPriceCalculatedCorrectly_WhenUnitPriceNotInMap_AndTotalPriceInMap_AndItemQuantityGreaterThanZero()
        {
            SetOdbcFieldMapEntry(EnumHelper.GetDescription(OdbcOrderFieldDescription.ItemTotalPrice), 42M);
            SetOrderItemQuantity(clonedMap, 2D, 0);

            testObject.Load(originalMap.Object, order, new List<OdbcRecord> { new OdbcRecord() });

            Assert.Equal(21M, order.OrderItems[0].UnitPrice);
        }

        [Fact]
        public void Load_UnitPriceZero_WhenUnitPriceNotInMap_AndTotalPriceInMap_AndItemQuantityZero()
        {
            SetOdbcFieldMapEntry(EnumHelper.GetDescription(OdbcOrderFieldDescription.ItemTotalPrice), 42M);
            SetOrderItemQuantity(clonedMap, 0D, 0);

            testObject.Load(originalMap.Object, order, new List<OdbcRecord> { new OdbcRecord() });

            Assert.Equal(0M, order.OrderItems[0].UnitPrice);
        }

        [Fact]
        public void Load_UnitPriceZero_WhenUnitPriceAndTotalPriceNotInMap()
        {
            SetOrderItemQuantity(clonedMap, 5D, 0);

            testObject.Load(originalMap.Object, order, new List<OdbcRecord> { new OdbcRecord() });

            Assert.Equal(0M, order.OrderItems[0].UnitPrice);
        }
        #endregion "Unit Price Tests"

        #region "Unit Weight Tests"
        [Fact]
        public void Load_WeightSetToMappedWeight_WhenUnitWeightExistsInMap()
        {
            SetOdbcFieldMapEntry(EnumHelper.GetDescription(OdbcOrderFieldDescription.ItemUnitWeight), 42);

            testObject.Load(originalMap.Object, order, new List<OdbcRecord> { new OdbcRecord() });

            Assert.Equal(42, order.OrderItems[0].Weight);
        }


        [Fact]
        public void Load_WeightCalculatedCorrectly_WhenUnitWeightNotInMap_AndTotalWeightInMap_AndItemQuantityGreaterThanZero()
        {
            SetOdbcFieldMapEntry(EnumHelper.GetDescription(OdbcOrderFieldDescription.ItemTotalWeight), 42);
            SetOrderItemQuantity(clonedMap, 2D, 0);

            testObject.Load(originalMap.Object, order, new List<OdbcRecord> { new OdbcRecord() });

            Assert.Equal(21, order.OrderItems[0].Weight);
        }

        [Fact]
        public void Load_WeightZero_WhenUnitWeightNotInMap_AndTotalWeightInMap_AndItemQuantityZero()
        {
            SetOdbcFieldMapEntry(EnumHelper.GetDescription(OdbcOrderFieldDescription.ItemTotalWeight), 42M);
            SetOrderItemQuantity(clonedMap, 0D, 0);

            testObject.Load(originalMap.Object, order, new List<OdbcRecord> { new OdbcRecord() });

            Assert.Equal(0, order.OrderItems[0].Weight);
        }

        [Fact]
        public void Load_UnitWeightZero_WhenUnitWeightAndTotalWeightNotInMap()
        {
            SetOrderItemQuantity(clonedMap, 5D, 0);

            testObject.Load(originalMap.Object, order, new List<OdbcRecord> { new OdbcRecord() });

            Assert.Equal(0, order.OrderItems[0].Weight);
        }
        #endregion "Unit Weight Tests"

        #region "Helper Methods"
        /// <summary>
        /// Sets the order item quantity - Sets up CopyToEntity
        /// </summary>
        private static void SetOrderItemQuantity(Mock<IOdbcFieldMap> clonedMap, double quantity, int index)
        {
#pragma warning disable S3215 // "interface" instances should not be cast to concrete types
            clonedMap.Setup(m => m.CopyToEntity(It.IsAny<OrderItemEntity>(), index))
                .Callback<IEntity2, int>((item, itemIndex) => ((OrderItemEntity)item).Quantity = quantity);
#pragma warning restore S3215 // "interface" instances should not be cast to concrete types
        }

        private void SetOdbcFieldMapEntry(string displayName, object value, int index = 0)
        {
            var shipworksField = new Mock<IShipWorksOdbcMappableField>();
            shipworksField.SetupGet(f => f.DisplayName).Returns(displayName);
            shipworksField.SetupGet(f => f.Value).Returns(value);

            odbcFieldMapEntry = new Mock<IOdbcFieldMapEntry>();
            odbcFieldMapEntry.SetupGet(e => e.ShipWorksField).Returns(shipworksField.Object);
            odbcFieldMapEntry.SetupGet(e => e.Index).Returns(index);
        }
        #endregion "Helper Methods"

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}