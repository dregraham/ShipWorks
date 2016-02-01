using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.ShipSense.Hashing;

namespace ShipWorks.Tests.Shipping.ShipSense.Hashing
{
    public class ShipSenseOrderItemKeyFactoryTest
    {
        private ShipSenseOrderItemKeyFactory testObject;
        
        private List<OrderItemEntity> orderItems;

        public ShipSenseOrderItemKeyFactoryTest()
        {
            testObject = new ShipSenseOrderItemKeyFactory();

            orderItems = CreateOrderItems();
            foreach (OrderItemEntity item in orderItems)
            {
                AddAttributes(item);
            }
        }

        [Fact]
        public void GetKeys_ReturnsKeyForEachOrderItem_WithAllPropertiesAdded()
        {
            List<ShipSenseOrderItemKey> keys = testObject.GetKeys(orderItems, GetPropertyNames(), GetAttributeNames()).ToList();

            Assert.Equal(3, keys.Count);
        }

        [Fact]
        public void GetKeys_ReturnsKeys_WithAllPropertiesInSpecifiedPropertyListAdded()
        {
            List<ShipSenseOrderItemKey> keys = testObject.GetKeys(orderItems, GetPropertyNames(), GetAttributeNames()).ToList();

            Assert.Equal(3, keys.Count(k => k.KeyValue.Contains("SKU")));
            Assert.Equal(3, keys.Count(k => k.KeyValue.Contains("Code")));
            Assert.Equal(3, keys.Count(k => k.KeyValue.Contains("Name")));
        }

        [Fact]
        public void GetKeys_ReturnsKeys_WithProperties_OrderedByPropertyName()
        {
            List<ShipSenseOrderItemKey> keys = testObject.GetKeys(orderItems, GetPropertyNames(), GetAttributeNames()).ToList();

            // Properties should be in the order of Code, Name, SKU
            int codeIndex = keys[0].KeyValue.IndexOf("Code");
            int nameIndex = keys[0].KeyValue.IndexOf("Name");
            int skuIndex = keys[0].KeyValue.IndexOf("SKU");

            Assert.True(codeIndex < nameIndex && nameIndex < skuIndex);
        }

        [Fact]
        public void GetKeys_ReturnsKeys_WithPropertyValues()
        {
            List<ShipSenseOrderItemKey> keys = testObject.GetKeys(orderItems, GetPropertyNames(), GetAttributeNames()).ToList();

            List<string> values = keys.Select(k => k.KeyValue).ToList();

            // Keys were sorted by SKU in alpha
            Assert.True(values[0].Contains("ABC"));
            Assert.True(values[1].Contains("JKL"));
            Assert.True(values[2].Contains("XYZ"));
        }

        [Fact]
        public void GetKeys_ReturnsKeys_WithOnlyAttributesSpecifiedInAttributeListAdded()
        {
            List<ShipSenseOrderItemKey> keys = testObject.GetKeys(orderItems, GetPropertyNames(), GetAttributeNames()).ToList();

            Assert.Equal(3, keys.Count(k => k.KeyValue.Contains("COLOR")));
            Assert.Equal(3, keys.Count(k => k.KeyValue.Contains("SIZE")));
            Assert.Equal(0, keys.Count(k => k.KeyValue.Contains("EDITION")));
        }

        [Fact]
        public void GetKeys_ReturnsKeys_WithAttributeValues()
        {
            List<ShipSenseOrderItemKey> keys = testObject.GetKeys(orderItems, GetPropertyNames(), GetAttributeNames()).ToList();

            List<string> values = keys.Select(k => k.KeyValue).ToList();

            // Keys were sorted by SKU in alpha
            Assert.Equal(3, values.Count(v => v.Contains("MEDIUM")));
            Assert.Equal(3, values.Count(v => v.Contains("RED")));
        }

        [Fact]
        public void GetKeys_ReturnsKeys_WithAttributes_OrderedByPropertyName()
        {
            List<ShipSenseOrderItemKey> keys = testObject.GetKeys(orderItems, GetPropertyNames(), GetAttributeNames()).ToList();

            // Properties should be in the order of Code, Name, SKU
            int sizeIndex = keys[0].KeyValue.IndexOf("SIZE");
            int colorIndex = keys[0].KeyValue.IndexOf("COLOR");

            Assert.True(colorIndex < sizeIndex);
        }



        [Fact]
        public void GetKeys_ConsolidatesItems_HavingDuplicateKeyValues()
        {
            orderItems = new List<OrderItemEntity>
            {
                new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 3 },
                new OrderItemEntity { Code = "Code-9", SKU = "XYZ789", Quantity = 4 },
                new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 5 },
                new OrderItemEntity { Code = "Code-9", SKU = "XYZ789", Quantity = 6 }
            };

            List<ShipSenseOrderItemKey> keys = testObject.GetKeys(orderItems, GetPropertyNames(), GetAttributeNames()).ToList();

            List<ShipSenseOrderItemKey> itemsHavingSkuABC = keys.Where(k => k.KeyValue.Contains("ABC")).ToList();
            List<ShipSenseOrderItemKey> itemsHavingSkuXYZ = keys.Where(k => k.KeyValue.Contains("XYZ")).ToList();

            Assert.Equal(1, itemsHavingSkuABC.Count);
            Assert.Equal(1, itemsHavingSkuXYZ.Count);
        }

        [Fact]
        public void GetKeys_SumsQuantities_WhenConsolidatingItems_HavingDuplicateKeyValues()
        {
            orderItems = new List<OrderItemEntity>
            {
                new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 3 },
                new OrderItemEntity { Code = "Code-9", SKU = "XYZ789", Quantity = 4 },
                new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 5 },
                new OrderItemEntity { Code = "Code-9", SKU = "XYZ789", Quantity = 6 }
            };

            List<ShipSenseOrderItemKey> keys = testObject.GetKeys(orderItems, GetPropertyNames(), GetAttributeNames()).ToList();

            List<ShipSenseOrderItemKey> itemsHavingSkuABC = keys.Where(k => k.KeyValue.Contains("ABC")).ToList();
            List<ShipSenseOrderItemKey> itemsHavingSkuXYZ = keys.Where(k => k.KeyValue.Contains("XYZ")).ToList();

            Assert.Equal(8, itemsHavingSkuABC.First().Quantity);
            Assert.Equal(10, itemsHavingSkuXYZ.First().Quantity);
        }

        [Fact]
        public void GetKeys_ConsolidatesItems_WhenHavingMixtureOfDuplicateAndUniqueKeyValues()
        {
            orderItems = new List<OrderItemEntity>
            {
                new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 3 },
                new OrderItemEntity { Code = "Code-9", SKU = "XYZ789", Quantity = 6 },
                new OrderItemEntity { Code = "Code-9", SKU = "XYZ789", Quantity = 4 },
                new OrderItemEntity { Code = "Unique One", SKU = "UniqueItem-1", Quantity = 37 },
                new OrderItemEntity { Code = "Unique Two", SKU = "UniqueItem-2", Quantity = 1 },
                new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 2 },
                new OrderItemEntity { Code = "Code-9", SKU = "XYZ789", Quantity = 9 },
                new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 13 }
            };

            List<ShipSenseOrderItemKey> keys = testObject.GetKeys(orderItems, GetPropertyNames(), GetAttributeNames()).ToList();

            List<ShipSenseOrderItemKey> itemsHavingSkuABC = keys.Where(k => k.KeyValue.Contains("ABC")).ToList();
            List<ShipSenseOrderItemKey> itemsHavingSkuXYZ = keys.Where(k => k.KeyValue.Contains("XYZ")).ToList();
            List<ShipSenseOrderItemKey> uniqueOneKey = keys.Where(k => k.KeyValue.Contains("UniqueItem-1")).ToList();
            List<ShipSenseOrderItemKey> uniqueTwoKey = keys.Where(k => k.KeyValue.Contains("UniqueItem-2")).ToList();

            Assert.Equal(1, itemsHavingSkuABC.Count);
            Assert.Equal(1, itemsHavingSkuXYZ.Count);
            Assert.Equal(1, uniqueOneKey.Count);
            Assert.Equal(1, uniqueTwoKey.Count);
        }

        [Fact]
        public void GetKeys_SumsQuantities_WhenConsolidatingItems_HavingMixtureOfDuplicateAndUniqueKeyValues()
        {
            orderItems = new List<OrderItemEntity>
            {
                new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 3 },
                new OrderItemEntity { Code = "Code-9", SKU = "XYZ789", Quantity = 6 },
                new OrderItemEntity { Code = "Code-9", SKU = "XYZ789", Quantity = 4 },
                new OrderItemEntity { Code = "Unique One", SKU = "UniqueItem-1", Quantity = 37 },
                new OrderItemEntity { Code = "Unique Two", SKU = "UniqueItem-2", Quantity = 1 },
                new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 2 },
                new OrderItemEntity { Code = "Code-9", SKU = "XYZ789", Quantity = 9 },
                new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 13 }
            };

            List<ShipSenseOrderItemKey> keys = testObject.GetKeys(orderItems, GetPropertyNames(), GetAttributeNames()).ToList();

            List<ShipSenseOrderItemKey> itemsHavingSkuABC = keys.Where(k => k.KeyValue.Contains("ABC")).ToList();
            List<ShipSenseOrderItemKey> itemsHavingSkuXYZ = keys.Where(k => k.KeyValue.Contains("XYZ")).ToList();
            List<ShipSenseOrderItemKey> uniqueOneKey = keys.Where(k => k.KeyValue.Contains("UniqueItem-1")).ToList();
            List<ShipSenseOrderItemKey> uniqueTwoKey = keys.Where(k => k.KeyValue.Contains("UniqueItem-2")).ToList();

            Assert.Equal(18, itemsHavingSkuABC.First().Quantity);
            Assert.Equal(19, itemsHavingSkuXYZ.First().Quantity);
            Assert.Equal(37, uniqueOneKey.First().Quantity);
            Assert.Equal(1, uniqueTwoKey.First().Quantity);
        }


        [Fact]
        public void GetKeys_CreatesSeparateKey_ForEachUniuqeKeyValue()
        {
            orderItems = new List<OrderItemEntity>
            {
                new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 3 },
                new OrderItemEntity { Code = "Code-2", SKU = "ABC123", Quantity = 3 }
            };

            List<ShipSenseOrderItemKey> keys = testObject.GetKeys(orderItems, GetPropertyNames(), GetAttributeNames()).ToList();
            
            Assert.Equal(2, keys.Count);
        }

        [Fact]
        public void GetKeys_ConsolidatesItems_HavingSameAttributeValues_ButAttributesAreInDifferentOrder()
        {
            orderItems = new List<OrderItemEntity>
            {
                new OrderItemEntity { Code = "Code-9", SKU = "XYZ789", Quantity = 6 },
                new OrderItemEntity { Code = "Code-9", SKU = "XYZ789", Quantity = 4 },
            };

            // Add the attributes to both items
            orderItems.ForEach(AddAttributes);

            // Both items should have the same attributes now, so swap the order of the first and second attributes
            OrderItemAttributeEntity attributeEntity = orderItems[0].OrderItemAttributes[0];
            orderItems[0].OrderItemAttributes[0] = orderItems[0].OrderItemAttributes[1];
            orderItems[0].OrderItemAttributes[1] = attributeEntity;


            List<ShipSenseOrderItemKey> keys = testObject.GetKeys(orderItems, GetPropertyNames(), GetAttributeNames()).ToList();
            
            Assert.Equal(1, keys.Count);
        }

        [Fact]
        public void GetKeys_SumsQuantity_WhenConsolidatingItems_HavingSameAttributeValues_ButAttributesAreInDifferentOrder()
        {
            orderItems = new List<OrderItemEntity>
            {
                new OrderItemEntity { Code = "Code-9", SKU = "XYZ789", Quantity = 6 },
                new OrderItemEntity { Code = "Code-9", SKU = "XYZ789", Quantity = 4 },
            };

            // Add the attributes to both items
            orderItems.ForEach(AddAttributes);

            // Both items should have the same attributes now, so swap the order of the first and second attributes
            OrderItemAttributeEntity attributeEntity = orderItems[0].OrderItemAttributes[0];
            orderItems[0].OrderItemAttributes[0] = orderItems[0].OrderItemAttributes[1];
            orderItems[0].OrderItemAttributes[1] = attributeEntity;


            List<ShipSenseOrderItemKey> keys = testObject.GetKeys(orderItems, GetPropertyNames(), GetAttributeNames()).ToList();

            Assert.Equal(10, keys.First().Quantity);
        }

        [Fact]
        public void GetKeys_IgnoresAttributes_HavingEmptyStringInDescription()
        {
            orderItems = new List<OrderItemEntity>
            {
                new OrderItemEntity { Code = "Code-9", SKU = "XYZ789", Quantity = 6 },
                new OrderItemEntity { Code = "Code-9", SKU = "XYZ789", Quantity = 4 },
            };

            // Add the attributes to both items
            orderItems.ForEach(AddAttributes);

            // Now clear out the values/descriptions
            foreach (OrderItemEntity item in orderItems)
            {
                foreach (OrderItemAttributeEntity attribute in item.OrderItemAttributes)
                {
                    attribute.Description = string.Empty;
                }
            }


            List<ShipSenseOrderItemKey> keys = testObject.GetKeys(orderItems, GetPropertyNames(), GetAttributeNames()).ToList();

            // The key values should not contain anything for size and color
            Assert.False(keys.Any(k => k.KeyValue.Contains("COLOR")));
            Assert.False(keys.Any(k => k.KeyValue.Contains("SIZE")));
        }

        private List<string> GetPropertyNames()
        {
            return new List<string> { "SKU", "Code", "Name" };
        }

        private List<string> GetAttributeNames()
        {
            return new List<string> { "Size", "Color" };
        }

        private List<OrderItemEntity> CreateOrderItems()
        {
            return new List<OrderItemEntity>
            {
                new OrderItemEntity { SKU = "ABC", Code = "123", Name = "Item 1", Description = "The first item", ISBN = "110-123-324", UnitCost = 2.3M },
                new OrderItemEntity { SKU = "XYZ", Code = "QRS", Name = "Another item", Description = "The second item", ISBN = "", UnitCost = 12.3M },
                new OrderItemEntity { SKU = "JKL", Code = "TUV123", Name = "One more item", Description = "The third item", ISBN = "", UnitCost = 42.3M }
            };
        }

        private void AddAttributes(OrderItemEntity orderItem)
        {
            OrderItemAttributeEntity sizeAttribute = new OrderItemAttributeEntity { Name = "Size", Description = "Medium" };
            orderItem.OrderItemAttributes.Add(sizeAttribute);

            OrderItemAttributeEntity colorAttribute = new OrderItemAttributeEntity { Name = "Color", Description = "Red" };
            orderItem.OrderItemAttributes.Add(colorAttribute);

            OrderItemAttributeEntity editionAttribute = new OrderItemAttributeEntity { Name = "Edition", Description = "Enterprise" };
            orderItem.OrderItemAttributes.Add(editionAttribute);
        }
    }
}
