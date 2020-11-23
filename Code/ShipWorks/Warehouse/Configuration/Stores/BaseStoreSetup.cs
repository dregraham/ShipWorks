using Newtonsoft.Json;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Warehouse.Configuration.Stores.DTO;

namespace ShipWorks.Warehouse.Configuration.Stores
{
    /// <summary>
    /// Base class for setting up a store from a getConfig call
    /// </summary>
    public class BaseStoreSetup : IStoreSetup
    {
        /// <summary>
        /// Setup the specified store type
        /// </summary>
        public StoreEntity Setup<T>(StoreConfiguration config) where T : StoreEntity =>
            JsonConvert.DeserializeObject<T>(config.SyncPayload);
    }
}
