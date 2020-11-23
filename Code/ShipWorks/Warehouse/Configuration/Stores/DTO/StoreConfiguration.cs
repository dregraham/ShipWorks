using ShipWorks.Stores;

namespace ShipWorks.Warehouse.Configuration.Stores.DTO
{
    /// <summary>
    /// A store configuration downloaded from the Hub
    /// </summary>
    public class StoreConfiguration
    {
        /// <summary>
        /// The type code of this store
        /// </summary>
        public StoreTypeCode StoreType { get; set; }

        /// <summary>
        /// The name of this store
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The unique identifier of this store
        /// </summary>
        public string UniqueIdentifier { get; set; }

        /// <summary>
        /// The JSON serialized store entity
        /// </summary>
        public string SyncPayload { get; set; }
    }
}
