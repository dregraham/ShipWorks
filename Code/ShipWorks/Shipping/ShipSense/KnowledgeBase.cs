using System.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.ShipSense.Hashing;

namespace ShipWorks.Shipping.ShipSense
{
    /// <summary>
    /// Acts as the data source for ShipSense: knowledge base entries can be saved and fetched.
    /// </summary>
    public class Knowledgebase
    {
        private readonly IKnowledgebaseHash hashingStrategy;

        /// <summary>
        /// Initializes a new instance of the <see cref="Knowledgebase"/> class.
        /// </summary>
        public Knowledgebase()
            : this(new KnowledgebaseHash())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Knowledgebase"/> class.
        /// </summary>
        /// <param name="hashingStrategy">The hashing strategy that will be used to identify knowledge base entries.</param>
        public Knowledgebase(IKnowledgebaseHash hashingStrategy)
        {
            this.hashingStrategy = hashingStrategy;
        }

        /// <summary>
        /// Saves the knowledge base entry to the knowledge base using the order data as the key.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <param name="order">The order.</param>
        public void Save(KnowledgebaseEntry entry, OrderEntity order)
        {
            // Fetch the entity because if it exists, we need the IsNew property to
            // be set to false otherwise a PK violation will be thrown if it already exists
            // Note: in a later story we should probably look into caching this data to 
            // reduce the number of database calls 
            string hash = hashingStrategy.ComputeHash(order);
            ShipSenseKnowledgebaseEntity entity = FetchEntity(hash);

            if (entity == null)
            {
                // Doesn't exist in the db, so create a new one and set the hash
                entity = new ShipSenseKnowledgebaseEntity();
                entity.Hash = hash;
            }

            if (!string.IsNullOrWhiteSpace(entity.Entry))
            {
                // We don't want to overwrite any customs information that may already be on the
                // entry returned from the database if the current entry doesn't have any customs info
                KnowledgebaseEntry previousEntry = new KnowledgebaseEntry(entity.Entry);

                if (previousEntry.CustomsItems.Any() && !entry.CustomsItems.Any())
                {
                    // Make sure this entry also reflects the previous entry's customs items,
                    // so the customs info gets carried forward
                    entry.CustomsItems = previousEntry.CustomsItems;
                }
            }

            // Update the JSON of the entity to reflect the latest KB entry
            entity.Entry = entry.ToJson();
            
            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.SaveEntity(entity);
                adapter.Commit();
            }
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
                return new KnowledgebaseEntry(entity.Entry);
            }
            else
            {
                // There wasn't an entry in the data source for the items in this order,
                // so we'll just return a new, empty entry
                return new KnowledgebaseEntry(order.StoreID);
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
                    Hash = hashingStrategy.ComputeHash(order)
                };

            return FetchEntity(lookupEntity);
        }

        /// <summary>
        /// Fetches a ShipSenseKnowledgebaseEntity from the database based on the items in the given order.
        /// </summary>
        /// <returns>null if the ShipSenseKnowledgebaseEntity does not exist.  Otherwise, the ShipSenseKnowledgebaseEntity is returned.</returns>
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
    }
}
