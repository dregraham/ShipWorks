using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Warehouse;

namespace ShipWorks.Stores.Platforms.Odbc.Warehouse
{
    /// <summary>
    /// Factory for creating StoreDtos from StoreEntities
    /// </summary>
    [KeyedComponent(typeof(IStoreDtoFactory), StoreTypeCode.Odbc)]
    public class OdbcStoreDtoFactory : IStoreDtoFactory
    {
        /// <summary>
        /// Create a StoreDto from the given store entity
        /// </summary>
        public Task<Store> Create(StoreEntity baseStoreEntity)
        {
            var storeEntity = baseStoreEntity as OdbcStoreEntity;
            var store = new Store()
            {
                Name = storeEntity.StoreName,
                StoreType = (int) storeEntity.StoreTypeCode
            };
            
            return Task.FromResult(store);
        }
    }
}
