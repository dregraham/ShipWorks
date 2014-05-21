using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.IO.Zip;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Common.Threading;
using ShipWorks.Data;
using ShipWorks.Data.Adapter;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.ShipSense.Hashing;
using ShipWorks.Users.Audit;

namespace ShipWorks.Shipping.ShipSense
{
    /// <summary>
    /// Acts as the data source for ShipSense: knowledge base entries can be saved and fetched.
    /// </summary>
    public class Knowledgebase : IKnowledgebase
    {
        private readonly ILog log;
        private readonly IKnowledgebaseHash hashingStrategy;
        private readonly IShipSenseOrderItemKeyFactory keyFactory;
        private readonly ShipSenseUniquenessXmlParser shipSenseUniquenessXmlParser;

        /// <summary>
        /// Initializes a new instance of the <see cref="Knowledgebase"/> class.
        /// </summary>
        public Knowledgebase()
            : this(new KnowledgebaseHash(), new ShipSenseOrderItemKeyFactory(), LogManager.GetLogger(typeof(Knowledgebase)))
        {
            shipSenseUniquenessXmlParser = new ShipSenseUniquenessXmlParser();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Knowledgebase" /> class.
        /// </summary>
        /// <param name="hashingStrategy">The hashing strategy that will be used to identify knowledge base entries.</param>
        /// <param name="keyFactory">The key factory used to generate the ShipsenseOrderItemKey objects.</param>
        /// <param name="log">The log.</param>
        public Knowledgebase(IKnowledgebaseHash hashingStrategy, IShipSenseOrderItemKeyFactory keyFactory, ILog log)
        {
            shipSenseUniquenessXmlParser = new ShipSenseUniquenessXmlParser();
            this.hashingStrategy = hashingStrategy;
            this.keyFactory = keyFactory;
            this.log = log;
        }

        /// <summary>
        /// Gets the hashing strategy used by the knowledge base.
        /// </summary>
        public IKnowledgebaseHash HashingStrategy
        {
            get { return hashingStrategy; }
        }

        /// <summary>
        /// Gets the IShipSenseOrderItemKeyFactory being used by the knowledge base.
        /// </summary>
        public IShipSenseOrderItemKeyFactory KeyFactory
        {
            get { return keyFactory; }
        }

        /// <summary>
        /// Saves the knowledge base entry to the knowledge base using the order data as the key.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <param name="order">The order.</param>
        public void Save(KnowledgebaseEntry entry, OrderEntity order)
        {
            // Populate the order item attributes so we can compute the hash
            using (SqlAdapter adapter = new SqlAdapter())
            {
                foreach (OrderItemEntity orderItemEntity in order.OrderItems)
                {
                    adapter.FetchEntityCollection(orderItemEntity.OrderItemAttributes, new RelationPredicateBucket(OrderItemAttributeFields.OrderItemID == orderItemEntity.OrderItemID));
                }
            }

            KnowledgebaseHashResult hash = GetHashResult(order);
            if (hash.IsValid)
            {
                ShipSenseKnowledgebaseEntity entity = FetchEntity(hash.HashValue);

                if (entity == null)
                {
                    // Doesn't exist in the db, so create a new one and set the hash
                    entity = new ShipSenseKnowledgebaseEntity();
                    entity.Hash = hash.HashValue;
                }

                if (entity.Entry != null && entity.Entry.Any())
                {
                    // We don't want to overwrite any customs information that may already be on the
                    // entry returned from the database if the current entry doesn't have any customs info
                    KnowledgebaseEntry previousEntry = CreateKnowledgebaseEntry(entity.Entry);

                    if (previousEntry.CustomsItems.Any() && !entry.CustomsItems.Any())
                    {
                        // Make sure this entry also reflects the previous entry's customs items,
                        // so the customs info gets carried forward
                        entry.CustomsItems = previousEntry.CustomsItems;
                    }
                }

                // Update the compressed JSON of the entity to reflect the latest KB entry            
                entity.Entry = CompressEntry(entry);

                using (SqlAdapter adapter = new SqlAdapter())
                {
                    // Since we need the trigger to fire, we need to force an update even if the values haven't changed
                    entity.Fields[0].IsChanged = true;
                    entity.Fields[1].IsChanged = true;
                    entity.IsDirty = true;

                    adapter.SaveEntity(entity, false, false);
                    adapter.Commit();
                }
            }
            else
            {
                log.WarnFormat("A knowledge base entry was not created for order {0}. A unique value could not be determined based " +
                               "on the ShipSense uniqueness settings.", order.OrderID);
            }
        }

        /// <summary>
        /// Compresses the data in the knowledge base entry into a byte array.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <returns>A byte[] containing the compressed data of the knowledge base entry.</returns>
        public byte[] CompressEntry(KnowledgebaseEntry entry)
        {
            return GZipUtility.Compress(Encoding.UTF8.GetBytes(entry.ToJson()));
        }

        /// <summary>
        /// Gets the hash result for the given order.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns>A KnowledgebaseHashResult object.</returns>
        public KnowledgebaseHashResult GetHashResult(OrderEntity order)
        {
            // Fetch the entity because if it exists, we need the IsNew property to
            // be set to false otherwise a PK violation will be thrown if it already exists
            // Note: in a later story we should probably look into caching this data to 
            // reduce the number of database calls 
            string shipSenseXml = ShippingSettings.Fetch().ShipSenseUniquenessXml;
            IEnumerable<ShipSenseOrderItemKey> keys = keyFactory.GetKeys(order.OrderItems, shipSenseUniquenessXmlParser.GetItemProperties(shipSenseXml), shipSenseUniquenessXmlParser.GetItemAttributes(shipSenseXml));

            return hashingStrategy.ComputeHash(keys);
        }

        /// <summary>
        /// Gets the knowledge base entry for the items in the given order. If an entry does not already exist in the
        /// knowledge base, this will act as a factory method and create an empty entry.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns>An instance of a a KnowledgebaseEntr; the package data will be empty if there is not an entry in the knowledge base.</returns>
        public KnowledgebaseEntry GetEntry(OrderEntity order)
        {
            ShipSenseKnowledgebaseEntity entity = FetchEntity(order);

            if (entity != null)
            {
                // The entry data is JSON, so deserialize the JSON into 
                // an instance of a KnowledgebaseEntry
                return CreateKnowledgebaseEntry(entity.Entry);
            }
            
            // There wasn't an entry in the data source for the items in this order,
            // so we'll just return a new, empty entry
            return new KnowledgebaseEntry();
        }

        /// <summary>
        /// Determines whether the specified shipment has overwritten the data in the ShipSense knowledge base.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>A Boolean value indicating if the shipment has overwritten data provided by ShipSense.</returns>
        public bool IsOverwritten(ShipmentEntity shipment)
        {
            bool isOverwritten = false;
            
            // Make sure we have all of the order information
            OrderEntity order = (OrderEntity)DataProvider.GetEntity(shipment.OrderID);
            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.FetchEntityCollection(order.OrderItems, new RelationPredicateBucket(OrderItemFields.OrderID == order.OrderID));
            }

            // A null entity means there is not an ShipSense entry for the order, so 
            // there was nothing to overwrite
            ShipSenseKnowledgebaseEntity entity = FetchEntity(order);

            if (entity != null)
            {
                // Rehydrate the knowledge base entry and consider the shipment to have overwritten
                // the entry if the two do not match
                KnowledgebaseEntry entry = CreateKnowledgebaseEntry(entity.Entry);
                isOverwritten = !entry.Matches(shipment);
            }

            return isOverwritten;
        }

        /// <summary>
        /// Resets/truncates the underlying knowledge base data on a background thread causing
        /// the knowledge base to be reset as if it were new.
        /// </summary>
        /// <param name="initiatedBy">The initiated by.</param>
        /// <param name="progressReporter">The progress reporter.</param>
        /// <returns>The Task that is executing the operation.</returns>
        public Task ResetAsync(UserEntity initiatedBy, IProgressReporter progressReporter)
        {
            return new Task(() => Reset(initiatedBy, progressReporter));
        }

        /// <summary>
        /// Resets/truncates the underlying knowledge base data causing the knowledge base
        /// to be reset as if it were new.
        /// </summary>
        private void Reset(UserEntity initiatedBy, IProgressReporter progressReporter)
        {
            progressReporter.Starting();
            progressReporter.Detail = "Deleting ShipSense data...";

            using (AuditBehaviorScope scope = new AuditBehaviorScope(AuditState.Disabled))
            {
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    ActionProcedures.ResetShipSense(adapter);
                }
            }

            AuditUtility.Audit(AuditActionType.ResetShipSense);

            log.InfoFormat("The ShipSense knowledge base has been reset by user {0}.", initiatedBy.Username);

            progressReporter.Detail = "Done";
            progressReporter.PercentComplete = 100;
            progressReporter.Completed();
        }

        /// <summary>
        /// Refreshes the ShipSense status of unprocessed shipments whose order has a hash key matching
        /// the one provided. Shipments corresponding to the list of shipment IDs provided will not be
        /// impacted. This is useful for the case where the shipping window is open with a batch of 
        /// shipments and you don't want the underlying data to be refreshed when processing a shipment.
        /// Otherwise, you would get an error message indicating that the shipment has been updated when
        /// you go to process the next shipment.
        /// </summary>
        /// <param name="hashKey">The hash key.</param>
        /// <param name="excludedShipmentIDs">The excluded shipment IDs.</param>
        public void RefreshShipSenseStatus(string hashKey, IEnumerable<long> excludedShipmentIDs)
        {
            // Build the shipment XML for the excluded shipments
            StringBuilder shipmentXml = new StringBuilder();
            foreach (long shipmentID in excludedShipmentIDs)
            {
                shipmentXml.AppendFormat("<shipment><id>{0}</id></shipment>", shipmentID);
            }

            using (SqlAdapter adapter = new SqlAdapter())
            {
                ActionProcedures.ShipmentShipSenseProcedure(hashKey, shipmentXml.ToString(), adapter);
            }
        }

        /// <summary>
        /// Fetches a ShipSenseKnowledgebaseEntity from the database based on the items in the given order.
        /// </summary>
        /// <returns>null if the ShipSenseKnowledgebaseEntity does not exist.  Otherwise, the ShipSenseKnowledgebaseEntity is returned.</returns>
        private ShipSenseKnowledgebaseEntity FetchEntity(OrderEntity order)
        {
            ShipSenseKnowledgebaseEntity lookupEntity = new ShipSenseKnowledgebaseEntity
                {
                    Hash = GetHashResult(order).HashValue
                };

            return FetchEntity(lookupEntity);
        }

        /// <summary>
        /// Fetches a ShipSenseKnowledgebaseEntity from the database based on the items in the given order.
        /// </summary>
        /// <returns>Null if the ShipSenseKnowledgebaseEntity does not exist.  Otherwise, the ShipSenseKnowledgebaseEntity is returned.</returns>
        private ShipSenseKnowledgebaseEntity FetchEntity(string hash)
        {
            ShipSenseKnowledgebaseEntity lookupEntity = new ShipSenseKnowledgebaseEntity
            {
                Hash = hash
            };

            return FetchEntity(lookupEntity);
        }

        /// <summary>
        /// Fetches a ShipSenseKnowledgebaseEntity from the database based on the items in the given order.
        /// </summary>
        /// <returns>null if the ShipSenseKnowledgebaseEntity does not exist.  Otherwise, the ShipSenseKnowledgebaseEntity is returned.</returns>
        private ShipSenseKnowledgebaseEntity FetchEntity(ShipSenseKnowledgebaseEntity lookupEntity)
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.FetchEntity(lookupEntity);
            }

            // If the kb entity doesn't exist, the state will not be fetched.
            // Return null so callers don't have to know this.
            if (lookupEntity.Fields.State != EntityState.Fetched)
            {
                lookupEntity = null;
            }

            return lookupEntity;
        }

        /// <summary>
        /// Creates a knowledge base entry from JSON string that has been compressed.
        /// </summary>
        /// <param name="compressedJson">The compressed json.</param>
        /// <returns>The de-serialized KnowledgebaseEntry.</returns>
        private static KnowledgebaseEntry CreateKnowledgebaseEntry(byte[] compressedJson)
        {
            // Deserialize the compressed JSON into an instance of KnowledgebaseEntry
            string serializedJson = Encoding.UTF8.GetString(GZipUtility.Decompress(compressedJson));
            return new KnowledgebaseEntry(serializedJson);
        }        
    }
}
