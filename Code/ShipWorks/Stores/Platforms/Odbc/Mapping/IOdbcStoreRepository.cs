using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Warehouse.StoreData;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Repository for odbc stores
    /// </summary>
    public interface IOdbcStoreRepository
    {
        /// <summary>
        /// Get the odbc store data for the given store
        /// </summary>
        OdbcStore GetStore(OdbcStoreEntity store);

        /// <summary>
        /// Update the cached odbc store data for the given store
        /// </summary>
        Task UpdateStoreCache(OdbcStoreEntity store);
    }
}
