﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.ShipSense.Hashing;

namespace ShipWorks.Tests.Shipping.ShipSense.Hashing
{
    [TestClass]
    public class ShipSenseOrderItemKeyFactoryTest
    {
        private ShipSenseOrderItemKeyFactory testObject;
        
        private List<OrderItemEntity> orderItems;

        [TestInitialize]
        public void Initialize()
        {
            testObject = new ShipSenseOrderItemKeyFactory();

            orderItems = CreateOrderItems();
            foreach (OrderItemEntity item in orderItems)
            {
                AddAttributes(item);
            }
        }

        [TestMethod]
        public void GetKeys_ReturnsKeyForEachOrderItem_WithAllPropertiesAdded_Test()
        {
            List<ShipSenseOrderItemKey> keys = testObject.GetKeys(orderItems, GetPropertyNames(), GetAttributeNames()).ToList();

            Assert.AreEqual(3, keys.Count);
        }

        [TestMethod]
        public void GetKeys_ReturnsKeys_WithAllPropertiesInSpecifiedPropertyListAdded_Test()
        {
            List<ShipSenseOrderItemKey> keys = testObject.GetKeys(orderItems, GetPropertyNames(), GetAttributeNames()).ToList();

            Assert.AreEqual(3, keys.Count(k => k.KeyValue.Contains("SKU")));
            Assert.AreEqual(3, keys.Count(k => k.KeyValue.Contains("Code")));
            Assert.AreEqual(3, keys.Count(k => k.KeyValue.Contains("Name")));
        }

        [TestMethod]
        public void GetKeys_ReturnsKeys_WithProperties_OrderedByPropertyName_Test()
        {
            List<ShipSenseOrderItemKey> keys = testObject.GetKeys(orderItems, GetPropertyNames(), GetAttributeNames()).ToList();

            // Properties should be in the order of Code, Name, SKU
            int codeIndex = keys[0].KeyValue.IndexOf("Code");
            int nameIndex = keys[0].KeyValue.IndexOf("Name");
            int skuIndex = keys[0].KeyValue.IndexOf("SKU");

            Assert.IsTrue(codeIndex < nameIndex && nameIndex < skuIndex);
        }

        [TestMethod]
        public void GetKeys_ReturnsKeys_WithPropertyValues_Test()
        {
            List<ShipSenseOrderItemKey> keys = testObject.GetKeys(orderItems, GetPropertyNames(), GetAttributeNames()).ToList();

            List<string> values = keys.Select(k => k.KeyValue).ToList();

            // Keys were sorted by SKU in alpha
            Assert.IsTrue(values[0].Contains("ABC"));
            Assert.IsTrue(values[1].Contains("JKL"));
            Assert.IsTrue(values[2].Contains("XYZ"));
        }

        [TestMethod]
        public void GetKeys_ReturnsKeys_WithOnlyAttributesSpecifiedInAttributeListAdded_Test()
        {
            List<ShipSenseOrderItemKey> keys = testObject.GetKeys(orderItems, GetPropertyNames(), GetAttributeNames()).ToList();

            Assert.AreEqual(3, keys.Count(k => k.KeyValue.Contains("COLOR")));
            Assert.AreEqual(3, keys.Count(k => k.KeyValue.Contains("SIZE")));
            Assert.AreEqual(0, keys.Count(k => k.KeyValue.Contains("EDITION")));
        }

        [TestMethod]
        public void GetKeys_ReturnsKeys_WithAttributeValues_Test()
        {
            List<ShipSenseOrderItemKey> keys = testObject.GetKeys(orderItems, GetPropertyNames(), GetAttributeNames()).ToList();

            List<string> values = keys.Select(k => k.KeyValue).ToList();

            // Keys were sorted by SKU in alpha
            Assert.AreEqual(3, values.Count(v => v.Contains("MEDIUM")));
            Assert.AreEqual(3, values.Count(v => v.Contains("RED")));
        }

        [TestMethod]
        public void GetKeys_ReturnsKeys_WithAttributes_OrderedByPropertyName_Test()
        {
            List<ShipSenseOrderItemKey> keys = testObject.GetKeys(orderItems, GetPropertyNames(), GetAttributeNames()).ToList();

            // Properties should be in the order of Code, Name, SKU
            int sizeIndex = keys[0].KeyValue.IndexOf("SIZE");
            int colorIndex = keys[0].KeyValue.IndexOf("COLOR");

            Assert.IsTrue(colorIndex < sizeIndex);
        }



        [TestMethod]
        public void GetKeys_ConsolidatesItems_HavingDuplicateKeyValues_Test()
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

            Assert.AreEqual(1, itemsHavingSkuABC.Count);
            Assert.AreEqual(1, itemsHavingSkuXYZ.Count);
        }

        [TestMethod]
        public void GetKeys_SumsQuantities_WhenConsolidatingItems_HavingDuplicateKeyValues_Test()
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

            Assert.AreEqual(8, itemsHavingSkuABC.First().Quantity);
            Assert.AreEqual(10, itemsHavingSkuXYZ.First().Quantity);
        }

        [TestMethod]
        public void GetKeys_ConsolidatesItems_WhenHavingMixtureOfDuplicateAndUniqueKeyValues_Test()
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

            Assert.AreEqual(1, itemsHavingSkuABC.Count);
            Assert.AreEqual(1, itemsHavingSkuXYZ.Count);
            Assert.AreEqual(1, uniqueOneKey.Count);
            Assert.AreEqual(1, uniqueTwoKey.Count);
        }

        [TestMethod]
        public void GetKeys_SumsQuantities_WhenConsolidatingItems_HavingMixtureOfDuplicateAndUniqueKeyValues_Test()
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

            Assert.AreEqual(18, itemsHavingSkuABC.First().Quantity);
            Assert.AreEqual(19, itemsHavingSkuXYZ.First().Quantity);
            Assert.AreEqual(37, uniqueOneKey.First().Quantity);
            Assert.AreEqual(1, uniqueTwoKey.First().Quantity);
        }


        [TestMethod]
        public void GetKeys_CreatesSeparateKey_ForEachUniuqeKeyValue_Test()
        {
            orderItems = new List<OrderItemEntity>
            {
                new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 3 },
                new OrderItemEntity { Code = "Code-2", SKU = "ABC123", Quantity = 3 }
            };

            List<ShipSenseOrderItemKey> keys = testObject.GetKeys(orderItems, GetPropertyNames(), GetAttributeNames()).ToList();
            
            Assert.AreEqual(2, keys.Count);
        }

        [TestMethod]
        public void GetKeys_ConsolidatesItems_HavingSameAttributeValues_ButAttributesAreInDifferentOrder_Test()
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
            
            Assert.AreEqual(1, keys.Count);
        }

        [TestMethod]
        public void GetKeys_SumsQuantity_WhenConsolidatingItems_HavingSameAttributeValues_ButAttributesAreInDifferentOrder_Test()
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

            Assert.AreEqual(10, keys.First().Quantity);
        }

        [TestMethod]
        public void GetKeys_IgnoresAttributes_HavingEmptyStringInDescription_Test()
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
            Assert.IsFalse(keys.Any(k => k.KeyValue.Contains("COLOR")));
            Assert.IsFalse(keys.Any(k => k.KeyValue.Contains("SIZE")));
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
