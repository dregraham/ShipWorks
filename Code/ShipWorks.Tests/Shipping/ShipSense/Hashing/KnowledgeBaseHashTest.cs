using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.ShipSense.Hashing;

namespace ShipWorks.Tests.Shipping.ShipSense.Hashing
{
    [TestClass]
    public class KnowledgebaseHashTest
    {
        private KnowledgebaseHash testObject;
        private OrderEntity order;
        private string shipSenseUniquenessSettings;
        
        [TestInitialize]
        public void Initialize()
        {
            order = new OrderEntity();
            order.StoreID = 1006;
            shipSenseUniquenessSettings = @"
                <ShipSenseUniqueness>
                    <ItemAttributeNames>
                        <Name>
                        </Name>
                    </ItemAttributeNames>
                </ShipSenseUniqueness>
                ";

            testObject = new KnowledgebaseHash();
        }

        [TestMethod]
        public void ComputeHash_WithSingleItem_Test()
        {
            order.OrderItems.Add(new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 3 });

            string hash = testObject.ComputeHash(order, shipSenseUniquenessSettings);

            Assert.AreEqual("+OaVh83n59aFH0JaS7TZLS3Qf2/Cd+s4qa6eCZ9z/5Q=", hash);
        }

        [TestMethod]
        public void ComputeHash_WithItemsHavingDuplicateItemCodeSkuValues_Test()
        {
            order.OrderItems.Add(new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 3 });
            order.OrderItems.Add(new OrderItemEntity { Code = "Code-9", SKU = "XYZ789", Quantity = 4 });
            order.OrderItems.Add(new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 5 });
            order.OrderItems.Add(new OrderItemEntity { Code = "Code-9", SKU = "XYZ789", Quantity = 6 });

            string hash = testObject.ComputeHash(order, shipSenseUniquenessSettings);

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

            string hash = testObject.ComputeHash(order, shipSenseUniquenessSettings);

            Assert.AreEqual("oP1MPXqDMND8WO6362GBaH6tB1zXTcRpA4tTzFa/9sU=", hash);
        }

        [TestMethod]
        public void ComputeHash_CreatesSameHash_WhenSingleItemOrdersHaveSameItemCodeSkuQuantityValues_ForDifferentStores_Test()
        {
            order.OrderItems.Add(new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 3 });

            OrderEntity secondOrder = new OrderEntity();
            secondOrder.OrderItems.Add(new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 3 });
            secondOrder.StoreID = 2006;

            string firstOrderHash = testObject.ComputeHash(order, shipSenseUniquenessSettings);
            string secondOrderHash = testObject.ComputeHash(secondOrder, shipSenseUniquenessSettings);

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

            string firstOrderHash = testObject.ComputeHash(order, shipSenseUniquenessSettings);
            string secondOrderHash = testObject.ComputeHash(secondOrder, shipSenseUniquenessSettings);

            Assert.AreEqual(firstOrderHash, secondOrderHash);
        }

        [TestMethod]
        public void ComputeHash_CreatesDifferentHash_WhenItemCodeDiffers_Test()
        {
            order.OrderItems.Add(new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 3 });
            string firstHash = testObject.ComputeHash(order, shipSenseUniquenessSettings);

            order.OrderItems[0].Code = "Code-2";
            string secondHash = testObject.ComputeHash(order, shipSenseUniquenessSettings);

            Assert.AreNotEqual(firstHash, secondHash);
        }

        [TestMethod]
        public void ComputeHash_CreatesDifferentHash_WhenItemSKUDiffers_Test()
        {
            order.OrderItems.Add(new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 3 });
            string firstHash = testObject.ComputeHash(order, shipSenseUniquenessSettings);

            order.OrderItems[0].SKU = "ABC789";
            string secondHash = testObject.ComputeHash(order, shipSenseUniquenessSettings);

            Assert.AreNotEqual(firstHash, secondHash);
        }

        [TestMethod]
        public void ComputeHash_CreatesDifferentHash_WhenItemQuantitiesDiffers_Test()
        {
            order.OrderItems.Add(new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 3 });
            string firstHash = testObject.ComputeHash(order, shipSenseUniquenessSettings);

            order.OrderItems[0].Quantity = 1;
            string secondHash = testObject.ComputeHash(order, shipSenseUniquenessSettings);

            Assert.AreNotEqual(firstHash, secondHash);
        }

        [TestMethod]
        public void ComputeHash_WithSingleItemAndSingleAttribute_AllMatchingAttributes_Test()
        {
            shipSenseUniquenessSettings = @"
                <ShipSenseUniqueness>
                    <ItemAttributeNames>
                        <Name>Color</Name>
                    </ItemAttributeNames>
                </ShipSenseUniqueness>
                ";

            OrderItemEntity orderItemEntity = CreateOrderItemEntity("Code-1", "Sku-1", 3);
            orderItemEntity.OrderItemAttributes.Add(CreateOrderItemAttributeEntity("Color", "Blue"));

            order.OrderItems.Add(orderItemEntity);
            string hash = testObject.ComputeHash(order, shipSenseUniquenessSettings);

            Assert.AreEqual("8Sr4hHA5iztqc6A5Zibt7Lldwdl1XrJy8pTRUeQok7o=", hash);
        }
        [TestMethod]
        public void ComputeHash_WithSingleItemAndMultipleAttributes_AllMatchingAttributes_Test()
        {
            shipSenseUniquenessSettings = @"
                <ShipSenseUniqueness>
                    <ItemAttributeNames>
                        <Name>Color</Name>
                        <Name>Size</Name>
                    </ItemAttributeNames>
                </ShipSenseUniqueness>
                ";

            OrderItemEntity orderItemEntity = CreateOrderItemEntity("Code-1", "Sku-1", 3);
            orderItemEntity.OrderItemAttributes.Add(CreateOrderItemAttributeEntity("Color", "Blue"));
            orderItemEntity.OrderItemAttributes.Add(CreateOrderItemAttributeEntity("Size", "10.5"));

            order.OrderItems.Add(orderItemEntity);
            string hash = testObject.ComputeHash(order, shipSenseUniquenessSettings);

            Assert.AreEqual("rcheWRycCz2gqMhlWXYMx5uX6VxpzxwKnFDWWyByfw0=", hash);
        }

        [TestMethod]
        public void ComputeHash_WithMultipleItemsAndSingleAttribute_AllMatchingAttributes_Test()
        {
            shipSenseUniquenessSettings = @"
                <ShipSenseUniqueness>
                    <ItemAttributeNames>
                        <Name>Color</Name>
                    </ItemAttributeNames>
                </ShipSenseUniqueness>
                ";

            OrderItemEntity orderItemEntity = CreateOrderItemEntity("Code-1", "Sku-1", 3);
            orderItemEntity.OrderItemAttributes.Add(CreateOrderItemAttributeEntity("Color", "Blue"));
            order.OrderItems.Add(orderItemEntity);

            OrderItemEntity orderItemEntity2 = CreateOrderItemEntity("Code-2", "Sku-2", 2);
            order.OrderItems.Add(orderItemEntity2);


            string hash = testObject.ComputeHash(order, shipSenseUniquenessSettings);

            Assert.AreEqual("4CdIyb7+1EFk8//gXKnMQmxII6xZKkCjStw0Ge2uKjc=", hash);
        }

        [TestMethod]
        public void ComputeHash_WithMultipleItemsndMultipleAttributes_AllMatchingAttributes_Test()
        {
            shipSenseUniquenessSettings = @"
                <ShipSenseUniqueness>
                    <ItemAttributeNames>
                        <Name>Color</Name>
                        <Name>Size</Name>
                    </ItemAttributeNames>
                </ShipSenseUniqueness>
                ";

            OrderItemEntity orderItemEntity = CreateOrderItemEntity("Code-1", "Sku-1", 3);
            orderItemEntity.OrderItemAttributes.Add(CreateOrderItemAttributeEntity("Color", "Blue"));
            orderItemEntity.OrderItemAttributes.Add(CreateOrderItemAttributeEntity("Size", "10.5"));
            order.OrderItems.Add(orderItemEntity);

            OrderItemEntity orderItemEntity2 = CreateOrderItemEntity("Code-2", "Sku-2", 2);
            orderItemEntity2.OrderItemAttributes.Add(CreateOrderItemAttributeEntity("Color", "Yellow"));
            orderItemEntity2.OrderItemAttributes.Add(CreateOrderItemAttributeEntity("Size", "9"));
            order.OrderItems.Add(orderItemEntity2);

            string hash = testObject.ComputeHash(order, shipSenseUniquenessSettings);

            Assert.AreEqual("852hYIxO00Xi8W4sxnYBkiZAEbfqLVHpodIjUrVUGSs=", hash);
        }

        [TestMethod]
        public void ComputeHash_WithMultipleItemsndMultipleAttributes_NoMatchingAttributes_Test()
        {
            shipSenseUniquenessSettings = @"
                <ShipSenseUniqueness>
                    <ItemAttributeNames>
                        <Name>Colorxxxxx</Name>
                        <Name>Sizexxxxx</Name>
                    </ItemAttributeNames>
                </ShipSenseUniqueness>
                ";

            OrderItemEntity orderItemEntity = CreateOrderItemEntity("Code-1", "Sku-1", 3);
            orderItemEntity.OrderItemAttributes.Add(CreateOrderItemAttributeEntity("Color", "Blue"));
            orderItemEntity.OrderItemAttributes.Add(CreateOrderItemAttributeEntity("Size", "10.5"));
            order.OrderItems.Add(orderItemEntity);

            OrderItemEntity orderItemEntity2 = CreateOrderItemEntity("Code-2", "Sku-2", 2);
            orderItemEntity2.OrderItemAttributes.Add(CreateOrderItemAttributeEntity("Color", "Yellow"));
            orderItemEntity2.OrderItemAttributes.Add(CreateOrderItemAttributeEntity("Size", "9"));
            order.OrderItems.Add(orderItemEntity2);

            string hash = testObject.ComputeHash(order, shipSenseUniquenessSettings);

            Assert.AreEqual("q0GoWIsPhdIH8bWRlpIYSoMwrcQ2DegUZAXhK27sEHo=", hash);
        }

        [TestMethod]
        public void ComputeHash_WithMultipleItemsndMultipleAttributes_OneMatchingAttributes_Test()
        {
            shipSenseUniquenessSettings = @"
                <ShipSenseUniqueness>
                    <ItemAttributeNames>
                        <Name>Colorxxxxx</Name>
                        <Name>Size</Name>
                    </ItemAttributeNames>
                </ShipSenseUniqueness>
                ";

            OrderItemEntity orderItemEntity = CreateOrderItemEntity("Code-1", "Sku-1", 3);
            orderItemEntity.OrderItemAttributes.Add(CreateOrderItemAttributeEntity("Color", "Blue"));
            orderItemEntity.OrderItemAttributes.Add(CreateOrderItemAttributeEntity("Size", "10.5"));
            order.OrderItems.Add(orderItemEntity);

            OrderItemEntity orderItemEntity2 = CreateOrderItemEntity("Code-2", "Sku-2", 2);
            orderItemEntity2.OrderItemAttributes.Add(CreateOrderItemAttributeEntity("Color", "Yellow"));
            orderItemEntity2.OrderItemAttributes.Add(CreateOrderItemAttributeEntity("Size", "9"));
            order.OrderItems.Add(orderItemEntity2);

            string hash = testObject.ComputeHash(order, shipSenseUniquenessSettings);

            Assert.AreEqual("aZ5n9v+QX+iqB4Q/0rILNCU4pO0cKUCr1N/36Y5mCws=", hash);
        }


        [TestMethod]
        public void ComputeHash_WithMultipleItemsndMultipleAttributes_HashesMatchWhenItemsAndAttributesInDifferentOrder_Test()
        {
            shipSenseUniquenessSettings = @"
                <ShipSenseUniqueness>
                    <ItemAttributeNames>
                        <Name>Color</Name>
                        <Name>Size</Name>
                    </ItemAttributeNames>
                </ShipSenseUniqueness>
                ";

            OrderItemEntity orderItemEntity = CreateOrderItemEntity("Code-1", "Sku-1", 3);
            orderItemEntity.OrderItemAttributes.Add(CreateOrderItemAttributeEntity("Color", "Blue"));
            orderItemEntity.OrderItemAttributes.Add(CreateOrderItemAttributeEntity("Size", "10.5"));
            order.OrderItems.Add(orderItemEntity);

            OrderItemEntity orderItemEntity2 = CreateOrderItemEntity("Code-2", "Sku-2", 2);
            orderItemEntity2.OrderItemAttributes.Add(CreateOrderItemAttributeEntity("Color", "Yellow"));
            orderItemEntity2.OrderItemAttributes.Add(CreateOrderItemAttributeEntity("Size", "9"));
            order.OrderItems.Add(orderItemEntity2);

            string hash1 = testObject.ComputeHash(order, shipSenseUniquenessSettings);


            shipSenseUniquenessSettings = @"
                <ShipSenseUniqueness>
                    <ItemAttributeNames>
                        <Name>Size</Name>
                        <Name>Color</Name>
                    </ItemAttributeNames>
                </ShipSenseUniqueness>
                ";

            order.OrderItems.Clear();

            OrderItemEntity orderItemEntityReversed2 = CreateOrderItemEntity("Code-2", "Sku-2", 2);
            orderItemEntityReversed2.OrderItemAttributes.Add(CreateOrderItemAttributeEntity("Size", "9"));
            orderItemEntityReversed2.OrderItemAttributes.Add(CreateOrderItemAttributeEntity("Color", "Yellow"));
            order.OrderItems.Add(orderItemEntityReversed2);

            OrderItemEntity orderItemEntityReversed = CreateOrderItemEntity("Code-1", "Sku-1", 3);
            orderItemEntityReversed.OrderItemAttributes.Add(CreateOrderItemAttributeEntity("Size", "10.5"));
            orderItemEntityReversed.OrderItemAttributes.Add(CreateOrderItemAttributeEntity("Color", "Blue"));
            order.OrderItems.Add(orderItemEntityReversed);


            string hash2 = testObject.ComputeHash(order, shipSenseUniquenessSettings);

            Assert.AreEqual(hash1, hash2);
        }

        private OrderItemEntity CreateOrderItemEntity(string code, string sku, double quantity)
        {
            OrderItemEntity orderItemEntity = new OrderItemEntity
            {
                Code = code,
                SKU = sku,
                Quantity = quantity
            };

            return orderItemEntity;
        }

        private OrderItemAttributeEntity CreateOrderItemAttributeEntity(string name, string description)
        {
            OrderItemAttributeEntity orderItemAttributeEntity = new OrderItemAttributeEntity
            {
                Name = name,
                Description = description
            };

            return orderItemAttributeEntity;
        }


    }
}
