using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Warehouse.Configuration.Stores.DTO;

namespace ShipWorks.Warehouse.Configuration.Stores
{
    /// <summary>
    /// Class to configure stores downloaded from the Hub
    /// </summary>
    public interface IHubStoreConfigurator
    {
        /// <summary>
        /// Perform the configuration
        /// </summary>
        Task Configure(IEnumerable<StoreConfiguration> storeConfigurations);
    }
}
