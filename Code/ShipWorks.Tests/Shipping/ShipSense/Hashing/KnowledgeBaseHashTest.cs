using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.ShipSense.Hashing;

namespace ShipWorks.Tests.Shipping.ShipSense.Hashing
{
    [TestClass]
    public class KnowledgebaseHashTest
    {
        private KnowledgebaseHash testObject;
        private OrderEntity order;
        
        [TestInitialize]
        public void Initialize()
        {
            order = new OrderEntity();
            order.StoreID = 1006;

            testObject = new KnowledgebaseHash();
        }

        [TestMethod]
        public void ComputeHash_WithSingleItem_Test()
        {
            order.OrderItems.Add(new OrderItemEntity { SKU = "ABC123", Quantity = 3 });

            string hash = testObject.ComputeHash(order);

            Assert.AreEqual("4oIJWSGB8ATPIxzGyq9crfUS2UMEOgG6WRAPBLvZ2IE=", hash);
        }

        [TestMethod]
        public void ComputeHash_WithItemsHavingDuplicateSkuValues_Test()
        {
            order.OrderItems.Add(new OrderItemEntity { SKU = "ABC123", Quantity = 3});
            order.OrderItems.Add(new OrderItemEntity { SKU = "XYZ789", Quantity = 4 });
            order.OrderItems.Add(new OrderItemEntity { SKU = "ABC123", Quantity = 5 });
            order.OrderItems.Add(new OrderItemEntity { SKU = "XYZ789", Quantity = 6 });

            string hash = testObject.ComputeHash(order);

            Assert.AreEqual("+XnRw6TCZQ8fJOhD9WiJoqHpMbYUEFQmMkJz8U7QVc8=", hash);
        }

        [TestMethod]
        public void ComputeHash_WithItemsHavingMixtureOfUniqueAndDuplicateSkuValues_Test()
        {
            order.OrderItems.Add(new OrderItemEntity { SKU = "ABC123", Quantity = 3 });
            order.OrderItems.Add(new OrderItemEntity { SKU = "XYZ789", Quantity = 6 });
            order.OrderItems.Add(new OrderItemEntity { SKU = "XYZ789", Quantity = 4 });
            order.OrderItems.Add(new OrderItemEntity { SKU = "UniqueItem-1", Quantity = 37 });
            order.OrderItems.Add(new OrderItemEntity { SKU = "UniqueItem-2", Quantity = 1 });
            order.OrderItems.Add(new OrderItemEntity { SKU = "ABC123", Quantity = 2 });
            order.OrderItems.Add(new OrderItemEntity { SKU = "XYZ789", Quantity = 9 });
            order.OrderItems.Add(new OrderItemEntity { SKU = "ABC123", Quantity = 13 });

            string hash = testObject.ComputeHash(order);

            Assert.AreEqual("U2+S7ANPLl5Wrhxwaj8am4r4lsslYLDRh6KwLF1zxKo=", hash);
        }

        [TestMethod]
        public void ComputeHash_CreatesUniqueHash_WhenSingleItemOrdersHaveSameSkuQuantityValues_ForDifferentStores_Test()
        {
            order.OrderItems.Add(new OrderItemEntity { SKU = "ABC123", Quantity = 3 });

            OrderEntity secondOrder = new OrderEntity();
            secondOrder.StoreID = 2006;
            secondOrder.OrderItems.Add(new OrderItemEntity { SKU = "ABC123", Quantity = 3 });
            
            
            string firstOrderHash = testObject.ComputeHash(order);
            string secondOrderHash = testObject.ComputeHash(secondOrder);

            Assert.AreNotEqual(firstOrderHash, secondOrderHash);
        }

        [TestMethod]
        public void ComputeHash_CreatesUniqueHash_WhenMultipleItemOrdersHaveSameSkuQuantityValues_ForDifferentStores_Test()
        {
            order.OrderItems.Add(new OrderItemEntity { SKU = "ABC123", Quantity = 3 });
            order.OrderItems.Add(new OrderItemEntity { SKU = "XYZ789", Quantity = 6 });
            order.OrderItems.Add(new OrderItemEntity { SKU = "XYZ789", Quantity = 4 });
            order.OrderItems.Add(new OrderItemEntity { SKU = "UniqueItem-1", Quantity = 37 });
            order.OrderItems.Add(new OrderItemEntity { SKU = "UniqueItem-2", Quantity = 1 });
            order.OrderItems.Add(new OrderItemEntity { SKU = "ABC123", Quantity = 2 });
            order.OrderItems.Add(new OrderItemEntity { SKU = "XYZ789", Quantity = 9 });
            order.OrderItems.Add(new OrderItemEntity { SKU = "ABC123", Quantity = 13 });

            OrderEntity secondOrder = new OrderEntity();
            secondOrder.StoreID = 2006;
            secondOrder.OrderItems.Add(new OrderItemEntity { SKU = "ABC123", Quantity = 3 });
            secondOrder.OrderItems.Add(new OrderItemEntity { SKU = "XYZ789", Quantity = 6 });
            secondOrder.OrderItems.Add(new OrderItemEntity { SKU = "XYZ789", Quantity = 4 });
            secondOrder.OrderItems.Add(new OrderItemEntity { SKU = "UniqueItem-1", Quantity = 37 });
            secondOrder.OrderItems.Add(new OrderItemEntity { SKU = "UniqueItem-2", Quantity = 1 });
            secondOrder.OrderItems.Add(new OrderItemEntity { SKU = "ABC123", Quantity = 2 });
            secondOrder.OrderItems.Add(new OrderItemEntity { SKU = "XYZ789", Quantity = 9 });
            secondOrder.OrderItems.Add(new OrderItemEntity { SKU = "ABC123", Quantity = 13 });


            string firstOrderHash = testObject.ComputeHash(order);
            string secondOrderHash = testObject.ComputeHash(secondOrder);

            Assert.AreNotEqual(firstOrderHash, secondOrderHash);
        }
    }
}
