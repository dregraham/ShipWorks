using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Warehouse.Configuration.Stores.DTO;

namespace ShipWorks.Warehouse.Configuration.Stores
{
    /// <summary>
    /// Creates an action task for stores managed in the hub
    /// </summary>
    public interface IActionTaskConfigurator
    {
        /// <summary>
        /// Configure the Action Task if required
        /// </summary>
        void Configure(StoreConfiguration configuration, StoreEntity store, bool isNew);
    }
}