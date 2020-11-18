using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Stores;
using ShipWorks.Warehouse.Configuration.Stores.DTO;

namespace ShipWorks.Warehouse.Configuration.Stores
{
    /// <summary>
    /// Setup a Generic Module store
    /// </summary>
    [KeyedComponent(typeof(IStoreSetup), StoreTypeCode.GenericModule)]
    public class GenericModuleSetup : IStoreSetup
    {
        /// <summary>
        /// Setup the store
        /// </summary>
        public async Task Setup(StoreConfiguration config)
        {

        }
    }
}
