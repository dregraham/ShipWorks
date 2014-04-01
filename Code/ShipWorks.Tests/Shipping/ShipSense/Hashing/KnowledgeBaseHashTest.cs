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

            Assert.AreEqual("8i+RruvmToi1PBp0bq21EQIK7XLi1EJqr6OMjqF3bnE=", hash);
        }

        [TestMethod]
        public void ComputeHash_WithItemsHavingDuplicateSkuValues_Test()
        {
            order.OrderItems.Add(new OrderItemEntity { SKU = "ABC123", Quantity = 3});
            order.OrderItems.Add(new OrderItemEntity { SKU = "XYZ789", Quantity = 4 });
            order.OrderItems.Add(new OrderItemEntity { SKU = "ABC123", Quantity = 5 });
            order.OrderItems.Add(new OrderItemEntity { SKU = "XYZ789", Quantity = 6 });

            string hash = testObject.ComputeHash(order);

            Assert.AreEqual("NPYGn6XkJwEJwUb+rsNSD9+MTcMa2QNLg/+zs2LzylI=", hash);
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

            Assert.AreEqual("sRNevMwnLmwXP4Rq87EouT0s20c7Y5LToCd/ZRqkamM=", hash);
        }

        [TestMethod]
        public void ComputeHash_CreatesSameHash_WhenSingleItemOrdersHaveSameSkuQuantityValues_ForDifferentStores_Test()
        {
            order.OrderItems.Add(new OrderItemEntity { SKU = "ABC123", Quantity = 3 });

            OrderEntity secondOrder = new OrderEntity();
            secondOrder.StoreID = 2006;
            secondOrder.OrderItems.Add(new OrderItemEntity { SKU = "ABC123", Quantity = 3 });
            
            
            string firstOrderHash = testObject.ComputeHash(order);
            string secondOrderHash = testObject.ComputeHash(secondOrder);

            Assert.AreEqual(firstOrderHash, secondOrderHash);
        }

        [TestMethod]
        public void ComputeHash_CreatesSameHash_WhenMultipleItemOrdersHaveSameSkuQuantityValues_ForDifferentStores_Test()
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

            Assert.AreEqual(firstOrderHash, secondOrderHash);
        }
    }
}
