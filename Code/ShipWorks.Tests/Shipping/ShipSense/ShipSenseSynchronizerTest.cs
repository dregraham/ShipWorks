using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.ShipSense;
using ShipWorks.Shipping.ShipSense.Hashing;

namespace ShipWorks.Tests.Shipping.ShipSense
{
    [TestClass]
    public class ShipSenseSynchronizerTest
    {
        private ShipSenseSynchronizer testObject;

        private Mock<IKnowledgebase> knowledgebase;
        private Mock<IKnowledgebaseHash> hashingStrategy;
        private Mock<IShipSenseOrderItemKeyFactory> keyFactory;
        
        private ShippingSettingsEntity shippingSettings;
        private List<ShipmentEntity> shipments;

        [TestInitialize]
        public void Initialize()
        {
            // Setup the hash to just return a pipe-delimited list of the keys
            hashingStrategy = new Mock<IKnowledgebaseHash>();
            hashingStrategy.Setup(h => h.ComputeHash(It.IsAny<IEnumerable<ShipSenseOrderItemKey>>()))
                           .Returns((IEnumerable<ShipSenseOrderItemKey> keys) => new KnowledgebaseHashResult(true, string.Join("|", keys.Select(k => k.KeyValue))));

            // Configure the key factory to just create key values based on SKU, Code, and Quantity
            keyFactory = new Mock<IShipSenseOrderItemKeyFactory>();
            keyFactory.Setup(f => f.GetKeys(It.IsAny<IEnumerable<OrderItemEntity>>(), It.IsAny<List<string>>(), It.IsAny<List<string>>()))
                      .Returns((IEnumerable<OrderItemEntity> items, List<string> properties, List<string> attributes) =>
                      {
                          List<ShipSenseOrderItemKey> keys = new List<ShipSenseOrderItemKey>();
                          foreach (OrderItemEntity item in items)
                          {
                              ShipSenseOrderItemKey key = new ShipSenseOrderItemKey();
                              key.Add("SKU", item.SKU);
                              key.Add("Code", item.Code);
                              key.Quantity = item.Quantity;

                              keys.Add(key);
                          }

                          return keys;
                      });

            // Setup the knowledge base to use the mock hash, mock key factory, and to return 
            // a KnowledgebaseEntry based on the order ID
            knowledgebase = new Mock<IKnowledgebase>();
            knowledgebase.SetupGet(k => k.HashingStrategy).Returns(hashingStrategy.Object);
            knowledgebase.SetupGet(k => k.KeyFactory).Returns(keyFactory.Object);
            knowledgebase.Setup(k => k.GetEntry(It.IsAny<OrderEntity>()))
                         .Returns((OrderEntity order) =>
                         {
                             // By default return an entry with "10" as the weight and dimension values
                             KnowledgebaseEntry entry = new KnowledgebaseEntry("{\"Packages\":[{\"Hash\":\"hlSh5NM0o5rQy3iA9AAS1D6fbTjHgRPTwWVQblq+yJk=\",\"Length\":10.0,\"Width\":10.0,\"Height\":10.0,\"Weight\":10.0,\"ApplyAdditionalWeight\":false,\"AdditionalWeight\":10.0}],\"CustomsItems\":[{\"Hash\":\"FPvuFwoAMW/h1HvR8vi6HTiWP0O6VjXqdk9SRY4MpV8=\",\"Description\":\"Apple iPhone Bluetooth Headset\",\"Quantity\":1.0,\"Weight\":1.0,\"UnitValue\":99.0000,\"CountryOfOrigin\":\"US\",\"HarmonizedCode\":\"\",\"NumberOfPieces\":0,\"UnitPriceAmount\":99.0000}]}");

                             if (order.OrderID == 1)
                             {
                                 // Return a entry with "5" as the weight and dimension values for order ID 1
                                 entry = new KnowledgebaseEntry("{\"Packages\":[{\"Hash\":\"A1XuGqpzO6Ej++h2L8Re0U/bBS32jkemNhjfWLaU8NE=\",\"Length\":5.0,\"Width\":5.0,\"Height\":5.0,\"Weight\":5.0,\"ApplyAdditionalWeight\":false,\"AdditionalWeight\":5.0}],\"CustomsItems\":[{\"Hash\":\"wX6QbOdK0DoJQP8ZlIvgIMwtbMYet2yAlouOBvhfh4E=\",\"Description\":\"Apple iPhone Bluetooth Headset\",\"Quantity\":1.0,\"Weight\":1.0,\"UnitValue\":99.0,\"CountryOfOrigin\":\"US\",\"HarmonizedCode\":\"\",\"NumberOfPieces\":0,\"UnitPriceAmount\":99.0}]}");
                             }

                             return entry;
                         });

            shippingSettings = new ShippingSettingsEntity
            {
                ShipSenseEnabled = true,
                ShipSenseUniquenessXml = "<ShipSenseUniqueness><ItemProperty><Name>SKU</Name><Name>Code</Name></ItemProperty><ItemAttribute /></ShipSenseUniqueness>"
            };

            shipments = new List<ShipmentEntity>
            {
                GetShipmentForOrder1(),
                GetShipmentForOrder2(),
                GetShipmentForOrder1(),
                GetShipmentForOrder2(),
                GetShipmentForOrder1(),
                GetShipmentForOrder1()
            };

            testObject = new ShipSenseSynchronizer(shipments, shippingSettings, knowledgebase.Object);
        }

        [TestMethod]
        public void Constructor_AddsShipmentsToDictionary_Test()
        {
            // Already constructed the testObject in the initializer, so just 
            // inspect it
            Assert.AreEqual(6, testObject.MonitoredShipments.Count());
        }

        [TestMethod]
        public void Constructor_AddsKnowledgebaseEntriesToDictionary_Test()
        {
            // Already constructed the testObject in the initializer, so just 
            // inspect it. Based on the shipments and mocks, there should be 
            // two entries
            Assert.AreEqual(2, testObject.KnowledgebaseEntries.Count());
        }

        [TestMethod]
        public void Add_MonitorsShipment_WhenOneShipmentIsAdded_Test()
        {
            ShipmentEntity shipment = GetShipmentForOrder2();
            int originalCount = testObject.MonitoredShipments.Count();

            testObject.Add(shipment);

            Assert.AreEqual(originalCount + 1, testObject.MonitoredShipments.Count());
        }

        [TestMethod]
        public void Add_MonitorsShipments_WhenMultipleShipmentsAreAdded_Test()
        {
            List<ShipmentEntity> addedShipments = new List<ShipmentEntity>
            {
                GetShipmentForOrder2(),
                GetShipmentForOrder1()
            };

            int originalCount = testObject.MonitoredShipments.Count();

            testObject.Add(addedShipments);

            Assert.AreEqual(originalCount + 2, testObject.MonitoredShipments.Count());
        }

        [TestMethod]
        public void Add_UpdatesKnowledgebaseEntries_WhenShipmentIsAdded_AndKonwledgebaseEntryDoesNotAlreadyExist_Test()
        {
            // Create a shipment and change the order item SKU, so it gets 
            // recognized as a new entry that needs to be added (based on our mocks)
            ShipmentEntity shipment = GetShipmentForOrder2();
            shipment.Order.OrderItems[0].SKU = "ABC123";

            int originalCount = testObject.KnowledgebaseEntries.Count();

            testObject.Add(shipment);

            Assert.AreEqual(originalCount + 1, testObject.KnowledgebaseEntries.Count());
        }

        [TestMethod]
        public void Add_DoesNotUpdateKnowledgebaseEntries_WhenShipmentIsAdded_AndKnowledgebaseReturnsNullKnowledgebaseEntry_Test()
        {
            knowledgebase.Setup(k => k.GetEntry(It.IsAny<OrderEntity>())).Returns<KnowledgebaseEntry>(null);

            ShipmentEntity shipment = GetShipmentForOrder2();

            int originalCount = testObject.KnowledgebaseEntries.Count();

            testObject.Add(shipment);

            Assert.AreEqual(originalCount, testObject.KnowledgebaseEntries.Count());
        }

        [TestMethod]
        public void Add_DoesNotUpdateKnowledgebaseEntries_WhenShipmentIsAdded_AndKnowledgebaseReturnsKnowledgebaseEntry_WithoutPackages_Test()
        {
            knowledgebase.Setup(k => k.GetEntry(It.IsAny<OrderEntity>())).Returns(new KnowledgebaseEntry());

            ShipmentEntity shipment = GetShipmentForOrder2();

            int originalCount = testObject.KnowledgebaseEntries.Count();

            testObject.Add(shipment);

            Assert.AreEqual(originalCount, testObject.KnowledgebaseEntries.Count());
        }

        [TestMethod]
        public void Add_IgnoresShipments_WhenResultingHashIsInvalid_Test()
        {
            // Setup the hash to return an invalid result
            hashingStrategy.Setup(h => h.ComputeHash(It.IsAny<IEnumerable<ShipSenseOrderItemKey>>())).Returns(new KnowledgebaseHashResult(false, string.Empty));

            int originalCount = testObject.MonitoredShipments.Count();

            List<ShipmentEntity> addedShipments = new List<ShipmentEntity>
            {
                GetShipmentForOrder2(),
                GetShipmentForOrder1()
            };

            testObject.Add(addedShipments);

            Assert.AreEqual(originalCount, testObject.MonitoredShipments.Count());
        }

        [TestMethod]
        public void Add_IgnoresNullShipments_Test()
        {
            // Setup the hash to return an invalid result
            hashingStrategy.Setup(h => h.ComputeHash(It.IsAny<IEnumerable<ShipSenseOrderItemKey>>())).Returns(new KnowledgebaseHashResult(false, string.Empty));

            int originalCount = testObject.MonitoredShipments.Count();

            testObject.Add(null as ShipmentEntity);

            Assert.AreEqual(originalCount, testObject.MonitoredShipments.Count());
        }
        
        [TestMethod]
        public void Remove_DecrementsShipmentsBeingMonitored_Test()
        {
            int originalCount = testObject.MonitoredShipments.Count();

            // Remove the third shipment in our original list
            ShipmentEntity shipment = shipments[2];
            testObject.Remove(shipment);

            Assert.AreEqual(originalCount - 1, testObject.MonitoredShipments.Count());
        }


        private ShipmentEntity GetShipmentForOrder1()
        {
            OrderEntity order = new OrderEntity { OrderID = 1 };
            order.OrderItems.Add(new OrderItemEntity { SKU = "123", Code = "ABC", Quantity = 1 });

            ShipmentEntity shipment = new ShipmentEntity
            {
                Order = order
            };

            return shipment;
        }

        private ShipmentEntity GetShipmentForOrder2()
        {
            OrderEntity order = new OrderEntity { OrderID = 2 };
            order.OrderItems.Add(new OrderItemEntity { SKU = "789", Code = "XYZ", Quantity = 2 });

            ShipmentEntity shipment = new ShipmentEntity
            {
                Order = order
            };

            return shipment;
        }

    }
}
