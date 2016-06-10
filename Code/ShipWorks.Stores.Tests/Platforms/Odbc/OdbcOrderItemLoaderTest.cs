﻿using Autofac.Extras.Moq;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System.Collections.Generic;
using ShipWorks.Stores.Platforms.Odbc.Loaders;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc
{
    public class OdbcOrderItemLoaderTest
    {
        [Fact]
        public void Load_MapCloned()
        {
            using (var mock1 = AutoMock.GetLoose())
            using (var mock2 = AutoMock.GetLoose())
            {
                var originalMap = mock1.Mock<IOdbcFieldMap>();
                var clonedMap = mock2.Mock<IOdbcFieldMap>();
                originalMap.Setup(m => m.Clone()).Returns(clonedMap.Object);

                var testObject = mock1.Create<OdbcOrderItemLoader>();

                testObject.Load(originalMap.Object, new OrderEntity(), new List<OdbcRecord>());

                originalMap.Verify(m=>m.Clone(), Times.Once);
            }
        }

        [Fact]
        public void Load_EnsureApplyValuesNotCalledOnOriginalMap()
        {
            using (var mock1 = AutoMock.GetLoose())
            using (var mock2 = AutoMock.GetLoose())
            {
                var originalMap = mock1.Mock<IOdbcFieldMap>();
                var clonedMap = mock2.Mock<IOdbcFieldMap>();
                originalMap.Setup(m => m.Clone()).Returns(clonedMap.Object);

                var testObject = mock1.Create<OdbcOrderItemLoader>();

                testObject.Load(originalMap.Object, new OrderEntity(), new List<OdbcRecord> { new OdbcRecord() });

                originalMap.Verify(m => m.ApplyValues(It.IsAny<OdbcRecord>()), Times.Never);
            }
        }

        [Fact]
        public void Load_EnsureCopyToEntityNotCalledOnOriginalMap()
        {
            using (var mock1 = AutoMock.GetLoose())
            using (var mock2 = AutoMock.GetLoose())
            {
                var originalMap = mock1.Mock<IOdbcFieldMap>();
                var clonedMap = mock2.Mock<IOdbcFieldMap>();
                originalMap.Setup(m => m.Clone()).Returns(clonedMap.Object);

                var testObject = mock1.Create<OdbcOrderItemLoader>();

                testObject.Load(originalMap.Object, new OrderEntity(), new List<OdbcRecord> { new OdbcRecord() });

                originalMap.Verify(m => m.CopyToEntity(It.IsAny<IEntity2>()), Times.Never);
            }
        }

        [Fact]
        public void Load_ApplyValuesCalledForEachOdbcRecord()
        {
            using (var mock1 = AutoMock.GetLoose())
            using (var mock2 = AutoMock.GetLoose())
            {
                var originalMap = mock1.Mock<IOdbcFieldMap>();
                var clonedMap = mock2.Mock<IOdbcFieldMap>();
                originalMap.Setup(m => m.Clone()).Returns(clonedMap.Object);

                var testObject = mock1.Create<OdbcOrderItemLoader>();

                var record1 = new OdbcRecord();
                var record2 = new OdbcRecord();
                testObject.Load(originalMap.Object, new OrderEntity(), new List<OdbcRecord> { record1, record2 });

                clonedMap.Verify(m => m.ApplyValues(record1), Times.Once);
                clonedMap.Verify(m => m.ApplyValues(record2), Times.Once);
                clonedMap.Verify(m => m.ApplyValues(It.IsAny<OdbcRecord>()), Times.Exactly(2));
            }
        }

        [Fact]
        public void Load_OrderItemCountOfOrderMatchesNumberOfOdbcRecords()
        {
            using (var mock1 = AutoMock.GetLoose())
            using (var mock2 = AutoMock.GetLoose())
            {
                var originalMap = mock1.Mock<IOdbcFieldMap>();
                var clonedMap = mock2.Mock<IOdbcFieldMap>();
                originalMap.Setup(m => m.Clone()).Returns(clonedMap.Object);

                var testObject = mock1.Create<OdbcOrderItemLoader>();

                var record1 = new OdbcRecord();
                var record2 = new OdbcRecord();
                var order = new OrderEntity();
                testObject.Load(originalMap.Object, order, new List<OdbcRecord> { record1, record2 });

                Assert.Equal(2, order.OrderItems.Count);
            }
        }

        [Fact]
        public void Load_CopyToEntityCalledForEachOdbcRecord()
        {
            using (var mock1 = AutoMock.GetLoose())
            using (var mock2 = AutoMock.GetLoose())
            {
                var originalMap = mock1.Mock<IOdbcFieldMap>();
                var clonedMap = mock2.Mock<IOdbcFieldMap>();
                originalMap.Setup(m => m.Clone()).Returns(clonedMap.Object);

                var testObject = mock1.Create<OdbcOrderItemLoader>();

                var record1 = new OdbcRecord();
                var record2 = new OdbcRecord();
                var order = new OrderEntity();
                testObject.Load(originalMap.Object, order, new List<OdbcRecord> { record1, record2 });

                clonedMap.Verify(m => m.CopyToEntity(order.OrderItems[0]), Times.Once);
                clonedMap.Verify(m => m.CopyToEntity(order.OrderItems[1]), Times.Once);
                clonedMap.Verify(m => m.CopyToEntity(It.IsAny<OrderItemEntity>()), Times.Exactly(2));
            }
        }

        [Fact]
        public void Load_AttributeLoaderLoadCalledForEachOdbcRecord()
        {
            using (var mock1 = AutoMock.GetLoose())
            using (var mock2 = AutoMock.GetLoose())
            {
                var attributeLoader = mock1.Mock<IOdbcItemAttributeLoader>();

                var originalMap = mock1.Mock<IOdbcFieldMap>();
                var clonedMap = mock2.Mock<IOdbcFieldMap>();
                originalMap.Setup(m => m.Clone()).Returns(clonedMap.Object);

                var testObject = mock1.Create<OdbcOrderItemLoader>();

                var record1 = new OdbcRecord();
                var record2 = new OdbcRecord();
                var order = new OrderEntity();
                testObject.Load(originalMap.Object, order, new List<OdbcRecord> { record1, record2 });

                attributeLoader.Verify(m => m.Load(clonedMap.Object, order.OrderItems[0]), Times.Once);
                attributeLoader.Verify(m => m.Load(clonedMap.Object, order.OrderItems[1]), Times.Once);
                attributeLoader.Verify(m => m.Load(It.IsAny<IOdbcFieldMap>(), It.IsAny<OrderItemEntity>()), Times.Exactly(2));
            }
        }

        #region "Unit Price Tests"
        [Fact]
        public void Load_UnitPriceSetToMappedUnitPrice_WhenUnitPriceExistsInMap()
        {
            using (var mock1 = AutoMock.GetLoose())
            using (var mock2 = AutoMock.GetLoose())
            {
                var clonedMap = mock2.Mock<IOdbcFieldMap>();
                clonedMap.Setup(m => m.FindEntriesBy(It.Is<EntityField2>(f => f.Name == "UnitPrice"), false))
                    .Returns(new[]
                    {
                        GetOdbcFieldMapEntry(mock1, ShipWorksOdbcMappableField.UnitPriceDisplayName, 42M)
                    });

                var originalMap = mock1.Mock<IOdbcFieldMap>();
                originalMap.Setup(m => m.Clone()).Returns(clonedMap.Object);

                var testObject = mock1.Create<OdbcOrderItemLoader>();

                var order = new OrderEntity();
                testObject.Load(originalMap.Object, order, new List<OdbcRecord> { new OdbcRecord()});

                Assert.Equal(42M, order.OrderItems[0].UnitPrice);
            }
        }

        [Fact]
        public void Load_UnitPriceCalculatedCorrectly_WhenUnitPriceNotInMap_AndTotalPriceInMap_AndItemQuantityGreaterThanZero()
        {
            using (var mock1 = AutoMock.GetLoose())
            using (var mock2 = AutoMock.GetLoose())
            {
                var clonedMap = mock2.Mock<IOdbcFieldMap>();
                clonedMap.Setup(m => m.FindEntriesBy(It.Is<EntityField2>(f => f.Name == "UnitPrice"), false))
                    .Returns(new[]
                    {
                        GetOdbcFieldMapEntry(mock1, ShipWorksOdbcMappableField.TotalPriceDisplayName, 42M)
                    });

                SetOrderItemQuantity(clonedMap, 2D);

                var originalMap = mock1.Mock<IOdbcFieldMap>();
                originalMap.Setup(m => m.Clone()).Returns(clonedMap.Object);

                var testObject = mock1.Create<OdbcOrderItemLoader>();

                var order = new OrderEntity();
                testObject.Load(originalMap.Object, order, new List<OdbcRecord> { new OdbcRecord() });

                Assert.Equal(21M, order.OrderItems[0].UnitPrice);
            }
        }

        [Fact]
        public void Load_UnitPriceZero_WhenUnitPriceNotInMap_AndTotalPriceInMap_AndItemQuantityZero()
        {
            using (var mock1 = AutoMock.GetLoose())
            using (var mock2 = AutoMock.GetLoose())
            {
                var clonedMap = mock2.Mock<IOdbcFieldMap>();
                clonedMap.Setup(m => m.FindEntriesBy(It.Is<EntityField2>(f => f.Name == "UnitPrice"), false))
                    .Returns(new[]
                    {
                        GetOdbcFieldMapEntry(mock1, ShipWorksOdbcMappableField.TotalPriceDisplayName, 0M)
                    });

                SetOrderItemQuantity(clonedMap, 2D);

                var originalMap = mock1.Mock<IOdbcFieldMap>();
                originalMap.Setup(m => m.Clone()).Returns(clonedMap.Object);

                var testObject = mock1.Create<OdbcOrderItemLoader>();

                var order = new OrderEntity();
                testObject.Load(originalMap.Object, order, new List<OdbcRecord> { new OdbcRecord() });

                Assert.Equal(0M, order.OrderItems[0].UnitPrice);
            }
        }

        [Fact]
        public void Load_UnitPriceZero_WhenUnitPriceAndTotalPriceNotInMap()
        {
            using (var mock1 = AutoMock.GetLoose())
            using (var mock2 = AutoMock.GetLoose())
            {
                var clonedMap = mock2.Mock<IOdbcFieldMap>();
                SetOrderItemQuantity(clonedMap, 2D);

                var originalMap = mock1.Mock<IOdbcFieldMap>();
                originalMap.Setup(m => m.Clone()).Returns(clonedMap.Object);

                var testObject = mock1.Create<OdbcOrderItemLoader>();

                var order = new OrderEntity();
                testObject.Load(originalMap.Object, order, new List<OdbcRecord> {new OdbcRecord()});

                Assert.Equal(0M, order.OrderItems[0].UnitPrice);
            }
        }
        #endregion "Unit Price Tests"

        #region "Unit Cost Tests"
        [Fact]
        public void Load_UnitCostSetToMappedUnitCost_WhenUnitCostExistsInMap()
        {
            using (var mock1 = AutoMock.GetLoose())
            using (var mock2 = AutoMock.GetLoose())
            {
                var clonedMap = mock2.Mock<IOdbcFieldMap>();
                clonedMap.Setup(m => m.FindEntriesBy(It.Is<EntityField2>(f => f.Name == "UnitCost"), false))
                    .Returns(new[]
                    {
                        GetOdbcFieldMapEntry(mock1, ShipWorksOdbcMappableField.UnitCostDisplayName, 42M)
                    });

                var originalMap = mock1.Mock<IOdbcFieldMap>();
                originalMap.Setup(m => m.Clone()).Returns(clonedMap.Object);

                var testObject = mock1.Create<OdbcOrderItemLoader>();

                var order = new OrderEntity();
                testObject.Load(originalMap.Object, order, new List<OdbcRecord> { new OdbcRecord() });

                Assert.Equal(42M, order.OrderItems[0].UnitCost);
            }
        }

        [Fact]
        public void Load_UnitCostCalculatedCorrectly_WhenUnitCostNotInMap_AndTotalCostInMap_AndItemQuantityGreaterThanZero()
        {
            using (var mock1 = AutoMock.GetLoose())
            using (var mock2 = AutoMock.GetLoose())
            {
                var clonedMap = mock2.Mock<IOdbcFieldMap>();
                clonedMap.Setup(m => m.FindEntriesBy(It.Is<EntityField2>(f => f.Name == "UnitCost"), false))
                    .Returns(new[]
                    {
                        GetOdbcFieldMapEntry(mock1, ShipWorksOdbcMappableField.TotalCostDisplayName, 42M)
                    });

                SetOrderItemQuantity(clonedMap, 2D);

                var originalMap = mock1.Mock<IOdbcFieldMap>();
                originalMap.Setup(m => m.Clone()).Returns(clonedMap.Object);

                var testObject = mock1.Create<OdbcOrderItemLoader>();

                var order = new OrderEntity();
                testObject.Load(originalMap.Object, order, new List<OdbcRecord> { new OdbcRecord() });

                Assert.Equal(21M, order.OrderItems[0].UnitCost);
            }
        }

        [Fact]
        public void Load_UnitCostZero_WhenUnitCostNotInMap_AndTotalCostInMap_AndItemQuantityZero()
        {
            using (var mock1 = AutoMock.GetLoose())
            using (var mock2 = AutoMock.GetLoose())
            {
                var clonedMap = mock2.Mock<IOdbcFieldMap>();
                clonedMap.Setup(m => m.FindEntriesBy(It.Is<EntityField2>(f => f.Name == "UnitCost"), false))
                    .Returns(new[]
                    {
                        GetOdbcFieldMapEntry(mock1, ShipWorksOdbcMappableField.TotalCostDisplayName, 0M)
                    });

                SetOrderItemQuantity(clonedMap, 2D);

                var originalMap = mock1.Mock<IOdbcFieldMap>();
                originalMap.Setup(m => m.Clone()).Returns(clonedMap.Object);

                var testObject = mock1.Create<OdbcOrderItemLoader>();

                var order = new OrderEntity();
                testObject.Load(originalMap.Object, order, new List<OdbcRecord> { new OdbcRecord() });

                Assert.Equal(0M, order.OrderItems[0].UnitCost);
            }
        }

        [Fact]
        public void Load_UnitCostZero_WhenUnitCostAndTotalCostNotInMap()
        {
            using (var mock1 = AutoMock.GetLoose())
            using (var mock2 = AutoMock.GetLoose())
            {
                var clonedMap = mock2.Mock<IOdbcFieldMap>();
                SetOrderItemQuantity(clonedMap, 2D);

                var originalMap = mock1.Mock<IOdbcFieldMap>();
                originalMap.Setup(m => m.Clone()).Returns(clonedMap.Object);

                var testObject = mock1.Create<OdbcOrderItemLoader>();

                var order = new OrderEntity();
                testObject.Load(originalMap.Object, order, new List<OdbcRecord> { new OdbcRecord() });

                Assert.Equal(0M, order.OrderItems[0].UnitCost);
            }
        }
        #endregion "Unit Cost Tests"

        #region "Unit Weight Tests"
        [Fact]
        public void Load_UnitWeightSetToMappedUnitWeight_WhenUnitWeightExistsInMap()
        {
            using (var mock1 = AutoMock.GetLoose())
            using (var mock2 = AutoMock.GetLoose())
            {
                var clonedMap = mock2.Mock<IOdbcFieldMap>();
                clonedMap.Setup(m => m.FindEntriesBy(It.Is<EntityField2>(f => f.Name == "Weight"), false))
                    .Returns(new[]
                    {
                        GetOdbcFieldMapEntry(mock1, ShipWorksOdbcMappableField.UnitWeightDisplayName, 42D)
                    });

                var originalMap = mock1.Mock<IOdbcFieldMap>();
                originalMap.Setup(m => m.Clone()).Returns(clonedMap.Object);

                var testObject = mock1.Create<OdbcOrderItemLoader>();

                var order = new OrderEntity();
                testObject.Load(originalMap.Object, order, new List<OdbcRecord> { new OdbcRecord() });

                Assert.Equal(42D, order.OrderItems[0].Weight);
            }
        }

        [Fact]
        public void Load_UnitWeightCalculatedCorrectly_WhenUnitWeightNotInMap_AndTotalWeightInMap_AndItemQuantityGreaterThanZero()
        {
            using (var mock1 = AutoMock.GetLoose())
            using (var mock2 = AutoMock.GetLoose())
            {
                var clonedMap = mock2.Mock<IOdbcFieldMap>();
                clonedMap.Setup(m => m.FindEntriesBy(It.Is<EntityField2>(f => f.Name == "Weight"), false))
                    .Returns(new[]
                    {
                        GetOdbcFieldMapEntry(mock1, ShipWorksOdbcMappableField.TotalWeightDisplayName, 42D)
                    });

                SetOrderItemQuantity(clonedMap, 2D);

                var originalMap = mock1.Mock<IOdbcFieldMap>();
                originalMap.Setup(m => m.Clone()).Returns(clonedMap.Object);

                var testObject = mock1.Create<OdbcOrderItemLoader>();

                var order = new OrderEntity();
                testObject.Load(originalMap.Object, order, new List<OdbcRecord> { new OdbcRecord() });

                Assert.Equal(21D, order.OrderItems[0].Weight);
            }
        }

        [Fact]
        public void Load_UnitWeightZero_WhenUnitWeightNotInMap_AndTotalWeightInMap_AndItemQuantityZero()
        {
            using (var mock1 = AutoMock.GetLoose())
            using (var mock2 = AutoMock.GetLoose())
            {
                var clonedMap = mock2.Mock<IOdbcFieldMap>();
                clonedMap.Setup(m => m.FindEntriesBy(It.Is<EntityField2>(f => f.Name == "Weight"), false))
                    .Returns(new[]
                    {
                        GetOdbcFieldMapEntry(mock1, ShipWorksOdbcMappableField.TotalWeightDisplayName, 0D)
                    });

                SetOrderItemQuantity(clonedMap, 2D);

                var originalMap = mock1.Mock<IOdbcFieldMap>();
                originalMap.Setup(m => m.Clone()).Returns(clonedMap.Object);

                var testObject = mock1.Create<OdbcOrderItemLoader>();

                var order = new OrderEntity();
                testObject.Load(originalMap.Object, order, new List<OdbcRecord> { new OdbcRecord() });

                Assert.Equal(0D, order.OrderItems[0].Weight);
            }
        }

        [Fact]
        public void Load_UnitWeightZero_WhenUnitWeightAndTotalWeightNotInMap()
        {
            using (var mock1 = AutoMock.GetLoose())
            using (var mock2 = AutoMock.GetLoose())
            {
                var clonedMap = mock2.Mock<IOdbcFieldMap>();
                SetOrderItemQuantity(clonedMap, 2D);

                var originalMap = mock1.Mock<IOdbcFieldMap>();
                originalMap.Setup(m => m.Clone()).Returns(clonedMap.Object);

                var testObject = mock1.Create<OdbcOrderItemLoader>();

                var order = new OrderEntity();
                testObject.Load(originalMap.Object, order, new List<OdbcRecord> { new OdbcRecord() });

                Assert.Equal(0, order.OrderItems[0].Weight);
            }
        }
        #endregion "Unit Weight Tests"

        /// <summary>
        /// Sets the order item quantity - Sets up CopyToEntity
        /// </summary>
        private static void SetOrderItemQuantity(Mock<IOdbcFieldMap> clonedMap, double quantity)
        {
            clonedMap.Setup(m => m.CopyToEntity(It.IsAny<OrderItemEntity>()))
#pragma warning disable S3215 // "interface" instances should not be cast to concrete types
                    .Callback<IEntity2>(item => ((OrderItemEntity)item).Quantity = quantity);
#pragma warning restore S3215 // "interface" instances should not be cast to concrete types
        }

        private IOdbcFieldMapEntry GetOdbcFieldMapEntry(AutoMock mock, string displayName, object value)
        {
            var shipworksField = mock.Mock<IShipWorksOdbcMappableField>();
            shipworksField.SetupGet(f => f.DisplayName).Returns(displayName);
            shipworksField.SetupGet(f => f.Value).Returns(value);

            var odbcFieldMapEntry = mock.Mock<IOdbcFieldMapEntry>();
            odbcFieldMapEntry.SetupGet(e => e.ShipWorksField).Returns(shipworksField.Object);

            return odbcFieldMapEntry.Object;
        }
    }
}
