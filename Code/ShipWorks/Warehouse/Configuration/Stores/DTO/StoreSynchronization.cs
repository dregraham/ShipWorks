using System.Collections.Generic;
using ShipWorks.Stores;

namespace ShipWorks.Warehouse.Configuration.Stores.DTO
{
    /// <summary>
    /// The Store Synchronization request DTO
    /// </summary>
    public class StoreSynchronizationRequest
    {
        public List<StoreSynchronization> StoreSynchronizations;
    }

    /// <summary>
    /// A Store Synchronization
    /// </summary>
    public class StoreSynchronization
    {
        public string Name { get; set; }

        public string UniqueIdentifier { get; set; }

        public StoreTypeCode StoreType { get; set; }

        public string SyncPayload { get; set; }

        public string ActionsPayload { get; set; }
    }
}
