using System;
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
        private readonly ShipSenseOrderItemKeyFactory testObject;
        
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

            Assert.AreEqual(3, keys.Count(k => k.Value.Contains("SKU")));
            Assert.AreEqual(3, keys.Count(k => k.Value.Contains("Code")));
            Assert.AreEqual(3, keys.Count(k => k.Value.Contains("Name")));
        }

        [TestMethod]
        public void GetKeys_ReturnsKeys_WithProperties_OrderedByPropertyName_Test()
        {
            List<ShipSenseOrderItemKey> keys = testObject.GetKeys(orderItems, GetPropertyNames(), GetAttributeNames()).ToList();

            // Properties should be in the order of Code, Name, SKU
            int codeIndex = keys[0].Value.IndexOf("Code");
            int nameIndex = keys[0].Value.IndexOf("Name");
            int skuIndex = keys[0].Value.IndexOf("SKU");

            Assert.IsTrue(codeIndex < nameIndex && nameIndex < skuIndex);
        }

        [TestMethod]
        public void GetKeys_ReturnsKeys_WithPropertyValues_Test()
        {
            List<ShipSenseOrderItemKey> keys = testObject.GetKeys(orderItems, GetPropertyNames(), GetAttributeNames()).ToList();

            List<string> values = keys.Select(k => k.Value).ToList();

            // Keys were sorted by SKU in alpha
            Assert.IsTrue(values[0].Contains("ABC"));
            Assert.IsTrue(values[1].Contains("JKL"));
            Assert.IsTrue(values[2].Contains("XYZ"));
        }

        [TestMethod]
        public void GetKeys_ReturnsKeys_WithOnlyAttributesSpecifiedInAttributeListAdded_Test()
        {
            List<ShipSenseOrderItemKey> keys = testObject.GetKeys(orderItems, GetPropertyNames(), GetAttributeNames()).ToList();

            Assert.AreEqual(3, keys.Count(k => k.Value.Contains("COLOR")));
            Assert.AreEqual(3, keys.Count(k => k.Value.Contains("SIZE")));
            Assert.AreEqual(0, keys.Count(k => k.Value.Contains("EDITION")));
        }

        [TestMethod]
        public void GetKeys_ReturnsKeys_WithAttributeValues_Test()
        {
            List<ShipSenseOrderItemKey> keys = testObject.GetKeys(orderItems, GetPropertyNames(), GetAttributeNames()).ToList();

            List<string> values = keys.Select(k => k.Value).ToList();

            // Keys were sorted by SKU in alpha
            Assert.AreEqual(3, values.Count(v => v.Contains("MEDIUM")));
            Assert.AreEqual(3, values.Count(v => v.Contains("RED")));
        }

        [TestMethod]
        public void GetKeys_ReturnsKeys_WithAttributes_OrderedByPropertyName_Test()
        {
            List<ShipSenseOrderItemKey> keys = testObject.GetKeys(orderItems, GetPropertyNames(), GetAttributeNames()).ToList();

            // Properties should be in the order of Code, Name, SKU
            int sizeIndex = keys[0].Value.IndexOf("SIZE");
            int colorIndex = keys[0].Value.IndexOf("COLOR");

            Assert.IsTrue(colorIndex < sizeIndex);
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
