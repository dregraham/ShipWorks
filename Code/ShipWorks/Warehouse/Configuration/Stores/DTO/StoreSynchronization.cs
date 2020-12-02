using System.Collections.Generic;
using System.Reflection;
using ShipWorks.Stores;

namespace ShipWorks.Warehouse.Configuration.Stores.DTO
{
    /// <summary>
    /// The Store Synchronization request DTO
    /// </summary>
    [Obfuscation()]
    public class StoreSynchronizationRequest
    {
        public List<StoreSynchronization> StoreSynchronizations;
    }

    /// <summary>
    /// A Store Synchronization
    /// </summary>
    [Obfuscation()]
    public class StoreSynchronization
    {
        public string Name { get; set; }

        public string UniqueIdentifier { get; set; }

        public StoreTypeCode StoreType { get; set; }

        public string SyncPayload { get; set; }

        public string ActionsPayload { get; set; }
    }
}
