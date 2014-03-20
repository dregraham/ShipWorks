using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.ShipSense.Hashing;

namespace ShipWorks.Tests.Shipping.ShipSense.Hashing
{
    [TestClass]
    public class KnowledgebaseHashTest
    {
        private readonly KnowledgebaseHash testObject;

        public KnowledgebaseHashTest()
        {
            testObject = new KnowledgebaseHash();
        }

        [TestMethod]
        public void ComputeHash_WithSingleItem_Test()
        {
            OrderEntity order = new OrderEntity();
            order.OrderItems.Add(new OrderItemEntity { SKU = "ABC123", Quantity = 3 });

            string hash = testObject.ComputeHash(order);

            Assert.AreEqual("9usJ0brYx3PPACf5C21r7qXBKGFCFkth/XIwTseqtg0=", hash);
        }

        [TestMethod]
        public void ComputeHash_WithItemsHavingDuplicateSkuValues_Test()
        {
            OrderEntity order = new OrderEntity();
            order.OrderItems.Add(new OrderItemEntity { SKU = "ABC123", Quantity = 3});
            order.OrderItems.Add(new OrderItemEntity { SKU = "XYZ789", Quantity = 4 });
            order.OrderItems.Add(new OrderItemEntity { SKU = "ABC123", Quantity = 5 });
            order.OrderItems.Add(new OrderItemEntity { SKU = "XYZ789", Quantity = 6 });

            string hash = testObject.ComputeHash(order);
            
            Assert.AreEqual("nTcDX/Sru6qgwLvp3Glke3/KZ6jCjsveabK46crO8Vk=", hash);
        }

        [TestMethod]
        public void ComputeHash_WithItemsHavingMixtureOfUniqueAndDuplicateSkuValues_Test()
        {
            OrderEntity order = new OrderEntity();
            order.OrderItems.Add(new OrderItemEntity { SKU = "ABC123", Quantity = 3 });
            order.OrderItems.Add(new OrderItemEntity { SKU = "XYZ789", Quantity = 6 });
            order.OrderItems.Add(new OrderItemEntity { SKU = "XYZ789", Quantity = 4 });
            order.OrderItems.Add(new OrderItemEntity { SKU = "UniqueItem-1", Quantity = 37 });
            order.OrderItems.Add(new OrderItemEntity { SKU = "UniqueItem-2", Quantity = 1 });
            order.OrderItems.Add(new OrderItemEntity { SKU = "ABC123", Quantity = 2 });
            order.OrderItems.Add(new OrderItemEntity { SKU = "XYZ789", Quantity = 9 });
            order.OrderItems.Add(new OrderItemEntity { SKU = "ABC123", Quantity = 13 });

            string hash = testObject.ComputeHash(order);

            Assert.AreEqual("S18CbOgRikI2EFE0oZXzPjk7XfgjXn4So45exANHG0k=", hash);
        }
    }
}
