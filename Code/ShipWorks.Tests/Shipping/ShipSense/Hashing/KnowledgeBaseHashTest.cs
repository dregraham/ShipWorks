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
        public void ComputeHash_ReturnsInvalidResult_WhenUniquenessKeyIsNotFound_Test()
        {
            order.OrderItems.Add(new OrderItemEntity { Code = "", SKU = "", Quantity = 3 });

            KnowledgebaseHashResult result = testObject.ComputeHash(order, shipSenseUniquenessSettings);

            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void ComputeHash_IsTheSame_WhenCodeAndSkuInDifferentOrder_Test()
        {
            order.OrderItems.Add(new OrderItemEntity { Code = "a", SKU = "1", Quantity = 3 });
            order.OrderItems.Add(new OrderItemEntity { Code = "b", SKU = "2", Quantity = 3 });
            KnowledgebaseHashResult result1 = testObject.ComputeHash(order, shipSenseUniquenessSettings);

            order.OrderItems.Clear();
            order.OrderItems.Add(new OrderItemEntity { Code = "b", SKU = "2", Quantity = 3 });
            order.OrderItems.Add(new OrderItemEntity { Code = "a", SKU = "1", Quantity = 3 });
            KnowledgebaseHashResult result2 = testObject.ComputeHash(order, shipSenseUniquenessSettings);

            Assert.AreEqual(result1.HashValue, result2.HashValue);
        }

        [TestMethod]
        public void ComputeHash_IsTheSame_WhenCodeSameAndSkuInDifferentOrder_Test()
        {
            order.OrderItems.Add(new OrderItemEntity { Code = "a", SKU = "1", Quantity = 3 });
            order.OrderItems.Add(new OrderItemEntity { Code = "a", SKU = "2", Quantity = 3 });
            KnowledgebaseHashResult result1 = testObject.ComputeHash(order, shipSenseUniquenessSettings);

            order.OrderItems.Clear();
            order.OrderItems.Add(new OrderItemEntity { Code = "a", SKU = "2", Quantity = 3 });
            order.OrderItems.Add(new OrderItemEntity { Code = "a", SKU = "1", Quantity = 3 });
            KnowledgebaseHashResult result2 = testObject.ComputeHash(order, shipSenseUniquenessSettings);

            Assert.AreEqual(result1.HashValue, result2.HashValue);
        }

        [TestMethod]
        public void ComputeHash_IsTheSame_WhenSkuSameAndCodeInDifferentOrder_Test()
        {
            order.OrderItems.Add(new OrderItemEntity { Code = "a", SKU = "1", Quantity = 3 });
            order.OrderItems.Add(new OrderItemEntity { Code = "b", SKU = "1", Quantity = 3 });
            KnowledgebaseHashResult result1 = testObject.ComputeHash(order, shipSenseUniquenessSettings);

            order.OrderItems.Clear();
            order.OrderItems.Add(new OrderItemEntity { Code = "b", SKU = "1", Quantity = 3 });
            order.OrderItems.Add(new OrderItemEntity { Code = "a", SKU = "1", Quantity = 3 });
            KnowledgebaseHashResult result2 = testObject.ComputeHash(order, shipSenseUniquenessSettings);

            Assert.AreEqual(result1.HashValue, result2.HashValue);
        }

        [TestMethod]
        public void ComputeHash_WithSingleItem_Test()
        {
            order.OrderItems.Add(new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 3 });

            KnowledgebaseHashResult result = testObject.ComputeHash(order, shipSenseUniquenessSettings);

            Assert.AreEqual("+OaVh83n59aFH0JaS7TZLS3Qf2/Cd+s4qa6eCZ9z/5Q=", result.HashValue);
        }

        [TestMethod]
        public void ComputeHash_WithItemsHavingDuplicateItemCodeSkuValues_Test()
        {
            order.OrderItems.Add(new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 3 });
            order.OrderItems.Add(new OrderItemEntity { Code = "Code-9", SKU = "XYZ789", Quantity = 4 });
            order.OrderItems.Add(new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 5 });
            order.OrderItems.Add(new OrderItemEntity { Code = "Code-9", SKU = "XYZ789", Quantity = 6 });

            KnowledgebaseHashResult result = testObject.ComputeHash(order, shipSenseUniquenessSettings);

            Assert.AreEqual("J0XVHDoL/FTcQTHZgATYyME+obHWlTuTPpM5eErG4Ts=", result.HashValue);
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

            KnowledgebaseHashResult result = testObject.ComputeHash(order, shipSenseUniquenessSettings);

            Assert.AreEqual("oP1MPXqDMND8WO6362GBaH6tB1zXTcRpA4tTzFa/9sU=", result.HashValue);
        }

        [TestMethod]
        public void ComputeHash_CreatesSameHash_WhenSingleItemOrdersHaveSameItemCodeSkuQuantityValues_ForDifferentStores_Test()
        {
            order.OrderItems.Add(new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 3 });

            OrderEntity secondOrder = new OrderEntity();
            secondOrder.OrderItems.Add(new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 3 });
            secondOrder.StoreID = 2006;

            KnowledgebaseHashResult firstOrderResult = testObject.ComputeHash(order, shipSenseUniquenessSettings);
            KnowledgebaseHashResult secondOrderResult = testObject.ComputeHash(secondOrder, shipSenseUniquenessSettings);

            Assert.AreEqual(firstOrderResult.HashValue, secondOrderResult.HashValue);
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

            KnowledgebaseHashResult firstOrderResult = testObject.ComputeHash(order, shipSenseUniquenessSettings);
            KnowledgebaseHashResult secondOrderResult = testObject.ComputeHash(secondOrder, shipSenseUniquenessSettings);

            Assert.AreEqual(firstOrderResult.HashValue, secondOrderResult.HashValue);
        }

        [TestMethod]
        public void ComputeHash_CreatesDifferentHash_WhenItemCodeDiffers_Test()
        {
            order.OrderItems.Add(new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 3 });
            KnowledgebaseHashResult firstHashResult = testObject.ComputeHash(order, shipSenseUniquenessSettings);

            order.OrderItems[0].Code = "Code-2";
            KnowledgebaseHashResult secondHashResult = testObject.ComputeHash(order, shipSenseUniquenessSettings);

            Assert.AreNotEqual(firstHashResult.HashValue, secondHashResult.HashValue);
        }

        [TestMethod]
        public void ComputeHash_CreatesDifferentHash_WhenItemSKUDiffers_Test()
        {
            order.OrderItems.Add(new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 3 });
            KnowledgebaseHashResult firstHashResult = testObject.ComputeHash(order, shipSenseUniquenessSettings);

            order.OrderItems[0].SKU = "ABC789";
            KnowledgebaseHashResult secondHashResult = testObject.ComputeHash(order, shipSenseUniquenessSettings);

            Assert.AreNotEqual(firstHashResult.HashValue, secondHashResult.HashValue);
        }

        [TestMethod]
        public void ComputeHash_CreatesDifferentHash_WhenItemQuantitiesDiffers_Test()
        {
            order.OrderItems.Add(new OrderItemEntity { Code = "Code-1", SKU = "ABC123", Quantity = 3 });
            KnowledgebaseHashResult firstHashResult = testObject.ComputeHash(order, shipSenseUniquenessSettings);

            order.OrderItems[0].Quantity = 1;
            KnowledgebaseHashResult secondHashResult = testObject.ComputeHash(order, shipSenseUniquenessSettings);

            Assert.AreNotEqual(firstHashResult.HashValue, secondHashResult.HashValue);
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
            KnowledgebaseHashResult result = testObject.ComputeHash(order, shipSenseUniquenessSettings);

            Assert.AreEqual("8Sr4hHA5iztqc6A5Zibt7Lldwdl1XrJy8pTRUeQok7o=", result.HashValue);
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
            KnowledgebaseHashResult result = testObject.ComputeHash(order, shipSenseUniquenessSettings);

            Assert.AreEqual("rcheWRycCz2gqMhlWXYMx5uX6VxpzxwKnFDWWyByfw0=", result.HashValue);
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


            KnowledgebaseHashResult result = testObject.ComputeHash(order, shipSenseUniquenessSettings);

            Assert.AreEqual("4CdIyb7+1EFk8//gXKnMQmxII6xZKkCjStw0Ge2uKjc=", result.HashValue);
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

            KnowledgebaseHashResult result = testObject.ComputeHash(order, shipSenseUniquenessSettings);

            Assert.AreEqual("852hYIxO00Xi8W4sxnYBkiZAEbfqLVHpodIjUrVUGSs=", result.HashValue);
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

            KnowledgebaseHashResult result = testObject.ComputeHash(order, shipSenseUniquenessSettings);

            Assert.AreEqual("q0GoWIsPhdIH8bWRlpIYSoMwrcQ2DegUZAXhK27sEHo=", result.HashValue);
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

            KnowledgebaseHashResult result = testObject.ComputeHash(order, shipSenseUniquenessSettings);

            Assert.AreEqual("aZ5n9v+QX+iqB4Q/0rILNCU4pO0cKUCr1N/36Y5mCws=", result.HashValue);
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

            KnowledgebaseHashResult result1 = testObject.ComputeHash(order, shipSenseUniquenessSettings);


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


            KnowledgebaseHashResult result2 = testObject.ComputeHash(order, shipSenseUniquenessSettings);

            Assert.AreEqual(result1.HashValue, result2.HashValue);
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
