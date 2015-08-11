using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.ShipSense;
using ShipWorks.Shipping.ShipSense.Hashing;
using ShipWorks.Stores.Platforms.Amazon.WebServices.Associates;

namespace ShipWorks.Tests.Shipping.ShipSense
{
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
                             KnowledgebaseEntry entry = new KnowledgebaseEntry("{\"Packages\":[{\"Hash\":\"2QFOGnmDmXBNQn/6vhAgRGyLQ1txl+/f9thbvbIl4N4=\",\"Length\":10.0,\"Width\":10.0,\"Height\":10.0,\"Weight\":10.0,\"ApplyAdditionalWeight\":false,\"AdditionalWeight\":10.0}],\"CustomsItems\":[{\"Hash\":\"FPvuFwoAMW/h1HvR8vi6HTiWP0O6VjXqdk9SRY4MpV8=\",\"Description\":\"Apple iPhone Bluetooth Headset\",\"Quantity\":1.0,\"Weight\":1.0,\"UnitValue\":99.0000,\"CountryOfOrigin\":\"US\",\"HarmonizedCode\":\"\",\"NumberOfPieces\":0,\"UnitPriceAmount\":99.0000}]}");

                             if (order.OrderID == 1)
                             {
                                 // Return a entry with "5" as the weight and dimension values for order ID 1
                                 entry = new KnowledgebaseEntry("{\"Packages\":[{\"Hash\":\"/FnBE8xclha9OT7LTyLjPQClF4y8bdBdO8T/CjOeDnE=\",\"Length\":5.0,\"Width\":5.0,\"Height\":5.0,\"Weight\":5.0,\"ApplyAdditionalWeight\":false,\"AdditionalWeight\":5.0}],\"CustomsItems\":[{\"Hash\":\"FPvuFwoAMW/h1HvR8vi6HTiWP0O6VjXqdk9SRY4MpV8=\",\"Description\":\"Apple iPhone Bluetooth Headset\",\"Quantity\":1.0,\"Weight\":1.0,\"UnitValue\":99.0000,\"CountryOfOrigin\":\"US\",\"HarmonizedCode\":\"\",\"NumberOfPieces\":0,\"UnitPriceAmount\":99.0000}]}");
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
                GetSinglePackageShipmentForOrder1(1),
                GetShipmentForOrder2(2),
                GetSinglePackageShipmentForOrder1(3),
                GetShipmentForOrder2(4),
                GetSinglePackageShipmentForOrder1(5),
                GetSinglePackageShipmentForOrder1(6)
            };

            testObject = new ShipSenseSynchronizer(shipments, shippingSettings, knowledgebase.Object);
        }

        [Fact]
        public void Constructor_AddsShipmentsToDictionary_Test()
        {
            // Already constructed the testObject in the initializer, so just 
            // inspect it
            Assert.AreEqual(6, testObject.MonitoredShipments.Count());
        }

        [Fact]
        public void Constructor_AddsKnowledgebaseEntriesToDictionary_Test()
        {
            // Already constructed the testObject in the initializer, so just 
            // inspect it. Based on the shipments and mocks, there should be 
            // two entries
            Assert.AreEqual(2, testObject.KnowledgebaseEntries.Count());
        }

        [Fact]
        public void Add_MonitorsShipment_WhenOneShipmentIsAdded_Test()
        {
            ShipmentEntity shipment = GetShipmentForOrder2(10);
            int originalCount = testObject.MonitoredShipments.Count();

            testObject.Add(shipment);

            Assert.AreEqual(originalCount + 1, testObject.MonitoredShipments.Count());
        }

        [Fact]
        public void Add_MonitorsShipments_WhenMultipleShipmentsAreAdded_Test()
        {
            List<ShipmentEntity> addedShipments = new List<ShipmentEntity>
            {
                GetShipmentForOrder2(10),
                GetSinglePackageShipmentForOrder1(11)
            };

            int originalCount = testObject.MonitoredShipments.Count();

            testObject.Add(addedShipments);

            Assert.AreEqual(originalCount + 2, testObject.MonitoredShipments.Count());
        }

        [Fact]
        public void Add_UpdatesKnowledgebaseEntries_WhenShipmentIsAdded_AndKnowledgebaseEntryDoesNotAlreadyExist_Test()
        {
            // Create a shipment and change the order item SKU, so it gets 
            // recognized as a new entry that needs to be added (based on our mocks)
            ShipmentEntity shipment = GetShipmentForOrder2(10);
            shipment.Order.OrderItems[0].SKU = "ABC123";

            int originalCount = testObject.KnowledgebaseEntries.Count();

            testObject.Add(shipment);

            Assert.AreEqual(originalCount + 1, testObject.KnowledgebaseEntries.Count());
        }

        [Fact]
        public void Add_DoesNotRemoveExistingShipments_WithSameShipmentID_WhenHashIsSame_Test()
        {
            // Create a new shipment with the same ID as one already in the synchronizer
            ShipmentEntity shipment = GetShipmentForOrder2(1);

            int originalCount = testObject.KnowledgebaseEntries.Count();

            testObject.Add(shipment);

            Assert.AreEqual(originalCount, testObject.KnowledgebaseEntries.Count());
        }

        [Fact]
        public void Add_DoesNotRemoveExistingShipments_WithSameShipmentID_WhenHashIsDifferent_Test()
        {
            // Create a shipment and change the order item SKU, so it gets 
            // recognized as a new entry that needs to be added (based on our mocks)
            ShipmentEntity shipment = GetShipmentForOrder2(1);
            shipment.Order.OrderItems[0].SKU = "ABC123";

            int originalCount = testObject.KnowledgebaseEntries.Count();

            testObject.Add(shipment);

            Assert.AreEqual(originalCount + 1, testObject.KnowledgebaseEntries.Count());
        }

        [Fact]
        public void Add_DoesNotUpdateKnowledgebaseEntries_WhenShipmentIsAdded_AndKnowledgebaseReturnsNullKnowledgebaseEntry_Test()
        {
            knowledgebase.Setup(k => k.GetEntry(It.IsAny<OrderEntity>())).Returns<KnowledgebaseEntry>(null);

            ShipmentEntity shipment = GetShipmentForOrder2(10);

            int originalCount = testObject.KnowledgebaseEntries.Count();

            testObject.Add(shipment);

            Assert.AreEqual(originalCount, testObject.KnowledgebaseEntries.Count());
        }

        [Fact]
        public void Add_DoesNotUpdateKnowledgebaseEntries_WhenShipmentIsAdded_AndKnowledgebaseReturnsKnowledgebaseEntry_WithoutPackages_Test()
        {
            knowledgebase.Setup(k => k.GetEntry(It.IsAny<OrderEntity>())).Returns(new KnowledgebaseEntry());

            ShipmentEntity shipment = GetShipmentForOrder2(10);

            int originalCount = testObject.KnowledgebaseEntries.Count();

            testObject.Add(shipment);

            Assert.AreEqual(originalCount, testObject.KnowledgebaseEntries.Count());
        }

        [Fact]
        public void Add_IgnoresShipments_WhenResultingHashIsInvalid_Test()
        {
            // Setup the hash to return an invalid result
            hashingStrategy.Setup(h => h.ComputeHash(It.IsAny<IEnumerable<ShipSenseOrderItemKey>>())).Returns(new KnowledgebaseHashResult(false, string.Empty));

            int originalCount = testObject.MonitoredShipments.Count();

            List<ShipmentEntity> addedShipments = new List<ShipmentEntity>
            {
                GetShipmentForOrder2(10),
                GetSinglePackageShipmentForOrder1(11)
            };

            testObject.Add(addedShipments);

            Assert.AreEqual(originalCount, testObject.MonitoredShipments.Count());
        }

        [Fact]
        public void Add_IgnoresNullShipments_Test()
        {
            // Setup the hash to return an invalid result
            hashingStrategy.Setup(h => h.ComputeHash(It.IsAny<IEnumerable<ShipSenseOrderItemKey>>())).Returns(new KnowledgebaseHashResult(false, string.Empty));

            int originalCount = testObject.MonitoredShipments.Count();

            testObject.Add(null as ShipmentEntity);

            Assert.AreEqual(originalCount, testObject.MonitoredShipments.Count());
        }
        
        [Fact]
        public void Remove_DecrementsShipmentsBeingMonitored_Test()
        {
            int originalCount = testObject.MonitoredShipments.Count();

            // Remove the third shipment in our original list
            ShipmentEntity shipment = shipments[2];
            testObject.Remove(shipment);

            Assert.AreEqual(originalCount - 1, testObject.MonitoredShipments.Count());
        }

        [Fact]
        public void SynchronizeWith_RemovesProcessedShipments_WhenShipSenseIsDisabled_Test()
        {
            shippingSettings.ShipSenseEnabled = false;
            shipments[2].Processed = true;

            int originalCount = testObject.MonitoredShipments.Count();

            testObject.SynchronizeWith(shipments[0]);

            Assert.AreEqual(originalCount - 1, testObject.MonitoredShipments.Count());
        }

        [Fact]
        public void SynchronizeWith_RemovesProcessedShipments_WhenShipSenseIsEnabled_Test()
        {
            shippingSettings.ShipSenseEnabled = true;
            shipments[2].Processed = true;

            int originalCount = testObject.MonitoredShipments.Count();

            testObject.SynchronizeWith(shipments[0]);

            Assert.AreEqual(originalCount - 1, testObject.MonitoredShipments.Count());
        }
        
        [Fact]
        public void SynchronizeWith_LeavesStatusAsNotApplied_WhenShipmentShipSenseStatusIsNotApplied_AndShipSenseIsDisabled_Test()
        {
            shippingSettings.ShipSenseEnabled = false;

            ShipmentEntity shipment = shipments[0];
            shipment.ShipSenseStatus = (int)ShipSenseStatus.NotApplied;
            
            testObject.SynchronizeWith(shipment);

            Assert.AreEqual((int)ShipSenseStatus.NotApplied, shipment.ShipSenseStatus);
        }

        [Fact]
        public void SynchronizeWith_LeavesStatusAsNotApplied_WhenShipmentShipSenseStatusIsNotApplied_AndShipSenseIsEnabled_Test()
        {
            shippingSettings.ShipSenseEnabled = true;

            ShipmentEntity shipment = shipments[0];
            shipment.ShipSenseStatus = (int)ShipSenseStatus.NotApplied;

            testObject.SynchronizeWith(shipment);

            Assert.AreEqual((int)ShipSenseStatus.NotApplied, shipment.ShipSenseStatus);
        }

        [Fact]
        public void SynchronizeWith_SetsStatusToOverwritten_WhenShipmentShipSenseStatusIsApplied_AndShipSenseIsEnabled_Test()
        {
            shippingSettings.ShipSenseEnabled = true;

            // Based on the mocked knowledge base, the shipment should be seen as overwritten
            ShipmentEntity shipment = shipments[0];
            shipment.ShipSenseStatus = (int)ShipSenseStatus.Applied;

            testObject.SynchronizeWith(shipment);

            Assert.AreEqual((int)ShipSenseStatus.Overwritten, shipment.ShipSenseStatus);
        }

        [Fact]
        public void SynchronizeWith_SetsStatusToOverwritten_WhenShipmentShipSenseStatusIsApplied_AndShipSenseIsDisabled_Test()
        {
            shippingSettings.ShipSenseEnabled = false;

            // Based on the mocked knowledge base, the shipment should be seen as overwritten
            ShipmentEntity shipment = shipments[0];
            shipment.ShipSenseStatus = (int)ShipSenseStatus.Applied;

            testObject.SynchronizeWith(shipment);

            Assert.AreEqual((int)ShipSenseStatus.Overwritten, shipment.ShipSenseStatus);
        }

        [Fact]
        public void SynchronizeWith_SetsStatusToApplied_WhenShipmentShipSenseStatusIsApplied_AndShipSenseIsEnabled_Test()
        {
            shippingSettings.ShipSenseEnabled = true;

            ShipmentEntity shipment = shipments[0];
            shipment.ShipSenseStatus = (int)ShipSenseStatus.Applied;

            // Setup the shipment to be the same its corresponding KB entry
            KnowledgebaseEntry entry = knowledgebase.Object.GetEntry(shipment.Order);
            ShipmentType shipmentType = ShipmentTypeManager.GetType(shipment);
            entry.ApplyTo(shipmentType.GetPackageAdapters(shipment), shipment.CustomsItems);

            testObject.SynchronizeWith(shipment);

            Assert.AreEqual((int)ShipSenseStatus.Applied, shipment.ShipSenseStatus);
        }

        [Fact]
        public void SynchronizeWith_SetsStatusToApplied_WhenShipmentShipSenseStatusIsApplied_AndShipSenseIsDisabled_Test()
        {
            shippingSettings.ShipSenseEnabled = false;

            ShipmentEntity shipment = shipments[0];
            shipment.ShipSenseStatus = (int)ShipSenseStatus.Applied;

            // Setup the shipment to be the same its corresponding KB entry
            KnowledgebaseEntry entry = knowledgebase.Object.GetEntry(shipment.Order);
            ShipmentType shipmentType = ShipmentTypeManager.GetType(shipment);
            entry.ApplyTo(shipmentType.GetPackageAdapters(shipment), shipment.CustomsItems);

            testObject.SynchronizeWith(shipment);

            Assert.AreEqual((int)ShipSenseStatus.Applied, shipment.ShipSenseStatus);
        }

        [Fact]
        public void SynchronizeWith_DoesNotChangeMatchingShipments_WhenShipSenseIsDisabled_Test()
        {
            shippingSettings.ShipSenseEnabled = false;

            // Shipment[0] is for order1 based on the initialization, so all elements 
            // should still have their original values
            ShipmentEntity shipment = shipments[0];
            shipment.Postal.DimsHeight = 1;
            shipment.Postal.DimsLength = 1;
            shipment.Postal.DimsWidth = 1;
            shipment.Postal.DimsWeight = 1;
            
            testObject.SynchronizeWith(shipment);

            KnowledgebaseEntry entry = knowledgebase.Object.GetEntry(shipment.Order);

            for (int i = 1; i < shipments.Count; i++)
            {
                ShipmentType shipmentType = ShipmentTypeManager.GetType(shipments[i]);
                Assert.IsFalse(entry.Matches(shipments[i]));
            }
        }

        [Fact]
        public void SynchronizeWith_SynchronizesMatchingShipments_WhenShipSenseIsEnabled_Test()
        {
            shippingSettings.ShipSenseEnabled = true;

            // Shipment[0] is for order1 based on the initialization, so all elements 
            // should still have their original values
            ShipmentEntity shipment = shipments[0];
            shipment.Postal.DimsHeight = 1;
            shipment.Postal.DimsLength = 1;
            shipment.Postal.DimsWidth = 1;
            shipment.Postal.DimsWeight = 1;

            ShipmentType shipmentType = ShipmentTypeManager.GetType(shipment);

            KnowledgebaseEntry entry = new KnowledgebaseEntry();
            entry.ApplyFrom(shipmentType.GetPackageAdapters(shipment), shipment.CustomsItems);

            testObject.SynchronizeWith(shipment);

            // Shipments 2, 4, and 5 should match based on the initialization
            Assert.IsTrue(entry.Matches(shipments[2]));
            Assert.IsTrue(entry.Matches(shipments[4]));
            Assert.IsTrue(entry.Matches(shipments[5]));
        }

        [Fact]
        public void SynchronizeWith_ShipSenseStatusOfMatchedShipmentsIsSameAsSourcedShipment_WhenShipSenseIsEnabled_AndMatchedShipmentsAndSourcedShipmentHaveShipSenseApplied_Test()
        {
            shippingSettings.ShipSenseEnabled = true;
            
            foreach (ShipmentEntity entity in shipments)
            {
                entity.ShipSenseStatus = (int)ShipSenseStatus.Applied;
            }

            // Shipment[0] is for order1 based on the initialization, so all elements 
            // should still have their original values
            ShipmentEntity shipment = shipments[0];
            shipment.Postal.DimsHeight = 1;
            shipment.Postal.DimsLength = 1;
            shipment.Postal.DimsWidth = 1;
            shipment.Postal.DimsWeight = 1;


            ShipmentType shipmentType = ShipmentTypeManager.GetType(shipment);

            KnowledgebaseEntry entry = new KnowledgebaseEntry();
            entry.ApplyFrom(shipmentType.GetPackageAdapters(shipment), shipment.CustomsItems);

            testObject.SynchronizeWith(shipment);

            // Shipments 2, 4, and 5 should match based on the initialization
            Assert.AreEqual(shipment.ShipSenseStatus, shipments[2].ShipSenseStatus);
            Assert.AreEqual(shipment.ShipSenseStatus, shipments[4].ShipSenseStatus);
            Assert.AreEqual(shipment.ShipSenseStatus, shipments[5].ShipSenseStatus);
        }

        [Fact]
        public void SynchronizeWith_ShipSenseStatusOfMatchedShipmentsIsApplied_WhenShipSenseIsEnabled_AndSourcedShipmentHasNotHadShipSenseApplied_AndSourcedShipmentMatchesEntry_Test()
        {
            shippingSettings.ShipSenseEnabled = true;

            foreach (ShipmentEntity entity in shipments)
            {
                entity.ShipSenseStatus = (int)ShipSenseStatus.Applied;
            }
            
            // Shipment[0] is for order1 based on the initialization, so all elements 
            // should still have their original values
            ShipmentEntity shipment = shipments[0];
            shipment.Postal.DimsHeight = 5.0;
            shipment.ContentWeight = 5.0;
            shipment.Postal.DimsLength = 5.0;
            shipment.Postal.DimsWidth = 5.0;
            shipment.Postal.DimsWeight = 5.0;
            shipment.ShipSenseStatus = (int)ShipSenseStatus.NotApplied;
            
            testObject.SynchronizeWith(shipment);

            // Shipments 2, 4, and 5 should match based on the initialization
            Assert.AreEqual((int)ShipSenseStatus.Applied, shipments[2].ShipSenseStatus);
            Assert.AreEqual((int)ShipSenseStatus.Applied, shipments[4].ShipSenseStatus);
            Assert.AreEqual((int)ShipSenseStatus.Applied, shipments[5].ShipSenseStatus);
        }

        [Fact]
        public void SynchronizeWith_ShipSenseStatusOfMatchedShipmentsIsOverwritten_WhenShipSenseIsEnabled_AndSourcedShipmentHasNotHadShipSenseApplied_AndSourcedShipmentDoesNotMatchEntry_Test()
        {
            shippingSettings.ShipSenseEnabled = true;

            foreach (ShipmentEntity entity in shipments)
            {
                entity.ShipSenseStatus = (int)ShipSenseStatus.Applied;
            }

            // Shipment[0] is for order1 based on the initialization, so all elements 
            // should still have their original values
            ShipmentEntity shipment = shipments[0];
            shipment.Postal.DimsHeight = 1;
            shipment.Postal.DimsLength = 1;
            shipment.Postal.DimsWidth = 1;
            shipment.Postal.DimsWeight = 1;
            shipment.ShipSenseStatus = (int)ShipSenseStatus.NotApplied;

            testObject.SynchronizeWith(shipment);

            // Shipments 2, 4, and 5 should match based on the initialization
            Assert.AreEqual((int)ShipSenseStatus.Overwritten, shipments[2].ShipSenseStatus);
            Assert.AreEqual((int)ShipSenseStatus.Overwritten, shipments[4].ShipSenseStatus);
            Assert.AreEqual((int)ShipSenseStatus.Overwritten, shipments[5].ShipSenseStatus);
        }

        [Fact]
        public void SynchronizeWith_ShipSenseStatusOfMatchedShipmentsIsNotApplied_WhenShipSenseIsEnabled_AndSourcedShipmentHasHadShipSenseApplied_AndSourcedShipmentDoesNotMatchEntry_Test()
        {
            shippingSettings.ShipSenseEnabled = true;

            foreach (ShipmentEntity entity in shipments)
            {
                entity.ShipSenseStatus = (int)ShipSenseStatus.NotApplied;
            }

            // Shipment[0] is for order1 based on the initialization, so all elements 
            // should still have their original values
            ShipmentEntity shipment = shipments[0];
            shipment.Postal.DimsHeight = 1;
            shipment.Postal.DimsLength = 1;
            shipment.Postal.DimsWidth = 1;
            shipment.Postal.DimsWeight = 1;
            shipment.ShipSenseStatus = (int)ShipSenseStatus.Applied;

            testObject.SynchronizeWith(shipment);

            // Shipments 2, 4, and 5 should match based on the initialization
            Assert.AreEqual((int)ShipSenseStatus.NotApplied, shipments[2].ShipSenseStatus);
            Assert.AreEqual((int)ShipSenseStatus.NotApplied, shipments[4].ShipSenseStatus);
            Assert.AreEqual((int)ShipSenseStatus.NotApplied, shipments[5].ShipSenseStatus);
        }

        [Fact]
        public void SynchronizeWith_IgnoresShipmentsForProvidersNotSupportingMultiplePackages_WhenSourcedShipmentContainsMultiplePackages_Test()
        {
            shipments = new List<ShipmentEntity>
            {
                GetSinglePackageShipmentForOrder1(1),
                GetMultiplePackageShipmentForOrder1(2),
                GetSinglePackageShipmentForOrder1(3),
                GetSinglePackageShipmentForOrder1(4),
                GetMultiplePackageShipmentForOrder1(5),
                GetSinglePackageShipmentForOrder1(6)
            };

            ShipmentEntity multiPackageShipment = shipments[1];
            ShipmentType shipmentType = ShipmentTypeManager.GetType(multiPackageShipment);

            // Create KB entry for the multi package shipment to use in our assertions
            KnowledgebaseEntry multiPackageEntry = new KnowledgebaseEntry();
            multiPackageEntry.ApplyFrom(shipmentType.GetPackageAdapters(multiPackageShipment), multiPackageShipment.CustomsItems);

            testObject.SynchronizeWith(multiPackageShipment);

            Assert.IsFalse(multiPackageEntry.Matches(shipments[0]));
            Assert.IsFalse(multiPackageEntry.Matches(shipments[2]));
            Assert.IsFalse(multiPackageEntry.Matches(shipments[3]));
            Assert.IsFalse(multiPackageEntry.Matches(shipments[5]));
        }

        [Fact]
        public void SynchronizeWith_SynchronizesMutliplePackageShipments_WhenSourcedShipmentAndTarget_PackageCountsAreEqual_Test()
        {
            shipments = new List<ShipmentEntity>
            {
                GetSinglePackageShipmentForOrder1(1),
                GetMultiplePackageShipmentForOrder1(2),
                GetSinglePackageShipmentForOrder1(3),
                GetSinglePackageShipmentForOrder1(4),
                GetMultiplePackageShipmentForOrder1(5),
                GetSinglePackageShipmentForOrder1(6)
            };

            ShipmentEntity multiPackageShipment = shipments[1];            
            ShipmentType shipmentType = ShipmentTypeManager.GetType(multiPackageShipment);

            // Create KB entry for the multi package shipment to use in our assertions
            KnowledgebaseEntry multiPackageEntry = new KnowledgebaseEntry();
            multiPackageEntry.ApplyFrom(shipmentType.GetPackageAdapters(multiPackageShipment), multiPackageShipment.CustomsItems);

            testObject.SynchronizeWith(multiPackageShipment);

            Assert.IsTrue(multiPackageEntry.Matches(shipments[4]));
        }

        [Fact]
        public void SynchronizeWith_IgnoresShipments_WhenSourcedShipmentAndTarget_PackageCountsAreDifferent_Test()
        {
            shipments = new List<ShipmentEntity>
            {
                GetSinglePackageShipmentForOrder1(1),
                GetMultiplePackageShipmentForOrder1(2),
                GetSinglePackageShipmentForOrder1(3),
                GetSinglePackageShipmentForOrder1(4),
                GetMultiplePackageShipmentForOrder1(5),
                GetSinglePackageShipmentForOrder1(6)
            };

            testObject = new ShipSenseSynchronizer(shipments, shippingSettings, knowledgebase.Object);

            ShipmentEntity multiPackageShipment = shipments[1];
            ShipmentType shipmentType = ShipmentTypeManager.GetType(multiPackageShipment);

            // Remove one package from the target
            shipments[4].FedEx.Packages.RemoveAt(0);

            // Create KB entry for the multi package shipment to use in our assertions
            KnowledgebaseEntry multiPackageEntry = new KnowledgebaseEntry();
            multiPackageEntry.ApplyFrom(shipmentType.GetPackageAdapters(multiPackageShipment), multiPackageShipment.CustomsItems);
            
            testObject.SynchronizeWith(multiPackageShipment);

            Assert.IsFalse(multiPackageEntry.Matches(shipments[4]));
        }

        [Fact]
        public void SynchronizeWith_DoesNotThrowKeyNotFoundException_WhenNoShipmentsExist()
        {
            testObject = new ShipSenseSynchronizer(new List<ShipmentEntity>(), shippingSettings, knowledgebase.Object);

            try
            {
                testObject.SynchronizeWith(GetSinglePackageShipmentForOrder1(1));
            }
            catch (KeyNotFoundException)
            {
                Assert.Fail("Should not have thrown KeyNotFoundException");
            }
        }
        
        private ShipmentEntity GetSinglePackageShipmentForOrder1(int shipmentID)
        {
            OrderEntity order = new OrderEntity { OrderID = 1 };
            order.OrderItems.Add(new OrderItemEntity { SKU = "123", Code = "ABC", Quantity = 1 });

            ShipmentEntity shipment = new ShipmentEntity
            {
                ShipmentID = shipmentID,
                Order = order,
                OriginCountryCode = "US",
                OriginPostalCode = "63102",
                ShipCountryCode = "US",
                ShipPostalCode = "63102",
                ShipmentType = (int)ShipmentTypeCode.Endicia,
                Postal = new PostalShipmentEntity
                {
                    Endicia = new EndiciaShipmentEntity(),
                    DimsHeight = 35,
                    DimsLength = 35,
                    DimsWidth = 35,
                    DimsWeight = 35
                }
            };

            return shipment;
        }

        private ShipmentEntity GetMultiplePackageShipmentForOrder1(int shipmentID)
        {
            OrderEntity order = new OrderEntity { OrderID = 1 };
            order.OrderItems.Add(new OrderItemEntity { SKU = "123", Code = "ABC", Quantity = 1 });

            ShipmentEntity shipment = new ShipmentEntity
            {
                ShipmentID = shipmentID,
                Order = order,
                OriginCountryCode = "US",
                OriginPostalCode = "63102",
                ShipCountryCode = "US",
                ShipPostalCode = "63102",
                ShipmentType = (int)ShipmentTypeCode.FedEx,
                FedEx = new FedExShipmentEntity()
            };

            shipment.FedEx.Packages.Add(new FedExPackageEntity { DimsWidth = 5, DimsHeight = 4, DimsLength = 3, DimsWeight = 2 });
            shipment.FedEx.Packages.Add(new FedExPackageEntity { DimsWidth = 6, DimsHeight = 7, DimsLength = 8, DimsWeight = 9 });

            return shipment;
        }

        private ShipmentEntity GetShipmentForOrder2(int shipmentID)
        {
            OrderEntity order = new OrderEntity { OrderID = 2 };
            order.OrderItems.Add(new OrderItemEntity { SKU = "789", Code = "XYZ", Quantity = 2 });

            ShipmentEntity shipment = new ShipmentEntity
            {
                ShipmentID = shipmentID,
                Order = order,
                OriginCountryCode = "US",
                OriginPostalCode = "63102",
                ShipCountryCode = "US",
                ShipPostalCode = "63102",
                ShipmentType = (int)ShipmentTypeCode.FedEx,
                FedEx = new FedExShipmentEntity()
            };

            shipment.FedEx.Packages.Add(new FedExPackageEntity { DimsWidth = 5, DimsHeight = 4, DimsLength = 3, DimsWeight = 2 });
            return shipment;
        }

    }
}
