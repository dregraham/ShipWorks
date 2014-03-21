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
            ShipSenseKnowledgebaseEntity entity = FetchEntity(order);
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

            if (entity.Entry != null)
            {
                // The entry data is JSON, so deserialize the JSON into 
                // an instance of a KnowledgebaseEntry
                return new KnowledgebaseEntry(entity.Entry);
            }
            else
            {
                // There wasn't an entry in the data source for the items in this order,
                // so we'll just return a new, empty entry
                return new KnowledgebaseEntry();
            }
        }

        /// <summary>
        /// Fetches a ShipSenseKnowledgebaseEntity from the database based on the items in the given order.
        /// </summary>
        private ShipSenseKnowledgebaseEntity FetchEntity(OrderEntity order)
        {
            ShipSenseKnowledgebaseEntity lookupEntity = new ShipSenseKnowledgebaseEntity();
            lookupEntity.Hash = hashingStrategy.ComputeHash(order);

            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.FetchEntity(lookupEntity);
            }

            return lookupEntity;
        }
    }
}
