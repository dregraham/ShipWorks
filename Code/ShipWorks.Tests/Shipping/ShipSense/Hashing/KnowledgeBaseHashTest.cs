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
            order.OrderItems.Add(new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 3 });

            string hash = testObject.ComputeHash(order);

            Assert.AreEqual("+OaVh83n59aFH0JaS7TZLS3Qf2/Cd+s4qa6eCZ9z/5Q=", hash);
        }

        [TestMethod]
        public void ComputeHash_WithItemsHavingDuplicateItemCodeSkuValues_Test()
        {
            order.OrderItems.Add(new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 3 });
            order.OrderItems.Add(new OrderItemEntity { Code = "Code-9", SKU = "XYZ789", Quantity = 4 });
            order.OrderItems.Add(new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 5 });
            order.OrderItems.Add(new OrderItemEntity { Code = "Code-9", SKU = "XYZ789", Quantity = 6 });

            string hash = testObject.ComputeHash(order);

            Assert.AreEqual("J0XVHDoL/FTcQTHZgATYyME+obHWlTuTPpM5eErG4Ts=", hash);
        }

        [TestMethod]
        public void ComputeHash_WithItemsHavingMixtureOfUniqueAndDuplicateItemCodeSkuValues_Test()
        {
            order.OrderItems.Add(new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 3 });
            order.OrderItems.Add(new OrderItemEntity { Code = "Code-9", SKU = "XYZ789", Quantity = 6 });
            order.OrderItems.Add(new OrderItemEntity { Code = "Code-9", SKU = "XYZ789", Quantity = 4 });
            order.OrderItems.Add(new OrderItemEntity { Code = "Unique One", SKU = "UniqueItem-1", Quantity = 37 });
            order.OrderItems.Add(new OrderItemEntity { Code = "Unique Two", SKU = "UniqueItem-2", Quantity = 1 });
            order.OrderItems.Add(new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 2 });
            order.OrderItems.Add(new OrderItemEntity { Code = "Code-9", SKU = "XYZ789", Quantity = 9 });
            order.OrderItems.Add(new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 13 });

            string hash = testObject.ComputeHash(order);

            Assert.AreEqual("oP1MPXqDMND8WO6362GBaH6tB1zXTcRpA4tTzFa/9sU=", hash);
        }

        [TestMethod]
        public void ComputeHash_CreatesSameHash_WhenSingleItemOrdersHaveSameItemCodeSkuQuantityValues_ForDifferentStores_Test()
        {
            order.OrderItems.Add(new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 3 });

            OrderEntity secondOrder = new OrderEntity();
            secondOrder.OrderItems.Add(new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 3 });
            secondOrder.StoreID = 2006;
            
            string firstOrderHash = testObject.ComputeHash(order);
            string secondOrderHash = testObject.ComputeHash(secondOrder);

            Assert.AreEqual(firstOrderHash, secondOrderHash);
        }

        [TestMethod]
        public void ComputeHash_CreatesSameHash_WhenMultipleItemOrdersHaveSameItemCodeSkuQuantityValues_ForDifferentStores_Test()
        {
            order.OrderItems.Add(new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 3 });
            order.OrderItems.Add(new OrderItemEntity { Code = "Code-9", SKU = "XYZ789", Quantity = 6 });
            order.OrderItems.Add(new OrderItemEntity { Code = "Code-9", SKU = "XYZ789", Quantity = 4 });
            order.OrderItems.Add(new OrderItemEntity { Code = "Unique One", SKU = "UniqueItem-1", Quantity = 37 });
            order.OrderItems.Add(new OrderItemEntity { Code = "Unique Two", SKU = "UniqueItem-2", Quantity = 1 });
            order.OrderItems.Add(new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 2 });
            order.OrderItems.Add(new OrderItemEntity { Code = "Code-9", SKU = "XYZ789", Quantity = 9 });
            order.OrderItems.Add(new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 13 });

            OrderEntity secondOrder = new OrderEntity();
            secondOrder.OrderItems.Add(new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 3 });
            secondOrder.OrderItems.Add(new OrderItemEntity { Code = "Code-9", SKU = "XYZ789", Quantity = 6 });
            secondOrder.OrderItems.Add(new OrderItemEntity { Code = "Code-9", SKU = "XYZ789", Quantity = 4 });
            secondOrder.OrderItems.Add(new OrderItemEntity { Code = "Unique One", SKU = "UniqueItem-1", Quantity = 37 });
            secondOrder.OrderItems.Add(new OrderItemEntity { Code = "Unique Two", SKU = "UniqueItem-2", Quantity = 1 });
            secondOrder.OrderItems.Add(new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 2 });
            secondOrder.OrderItems.Add(new OrderItemEntity { Code = "Code-9", SKU = "XYZ789", Quantity = 9 });
            secondOrder.OrderItems.Add(new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 13 });

            secondOrder.StoreID = 2006;

            string firstOrderHash = testObject.ComputeHash(order);
            string secondOrderHash = testObject.ComputeHash(secondOrder);

            Assert.AreEqual(firstOrderHash, secondOrderHash);
        }

        [TestMethod]
        public void ComputeHash_CreatesDifferentHash_WhenItemCodeDiffers_Test()
        {
            order.OrderItems.Add(new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 3 });
            string firstHash = testObject.ComputeHash(order);

            order.OrderItems[0].Code = "Code-2";
            string secondHash = testObject.ComputeHash(order);

            Assert.AreNotEqual(firstHash, secondHash);
        }

        [TestMethod]
        public void ComputeHash_CreatesDifferentHash_WhenItemSKUDiffers_Test()
        {
            order.OrderItems.Add(new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 3 });
            string firstHash = testObject.ComputeHash(order);

            order.OrderItems[0].SKU = "ABC789";
            string secondHash = testObject.ComputeHash(order);

            Assert.AreNotEqual(firstHash, secondHash);
        }

        [TestMethod]
        public void ComputeHash_CreatesDifferentHash_WhenItemQuantitiesDiffers_Test()
        {
            order.OrderItems.Add(new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 3 });
            string firstHash = testObject.ComputeHash(order);

            order.OrderItems[0].Quantity = 1;
            string secondHash = testObject.ComputeHash(order);

            Assert.AreNotEqual(firstHash, secondHash);
        }
    }
}
