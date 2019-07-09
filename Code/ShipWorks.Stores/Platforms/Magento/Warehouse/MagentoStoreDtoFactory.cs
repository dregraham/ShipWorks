using System;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Warehouse.StoreData;

namespace ShipWorks.Stores.Warehouse
{
    /// <summary>
    /// Factory for creating StoreDtos from StoreEntities
    /// </summary>
    [KeyedComponent(typeof(IStoreDtoFactory), StoreTypeCode.Magento)]
    public class MagentoStoreDtoFactory : IStoreDtoFactory
    {
        private readonly GenericModuleStoreDtoFactory genericModuleStoreDtoFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public MagentoStoreDtoFactory(GenericModuleStoreDtoFactory genericModuleStoreDtoFactory)
        {
            this.genericModuleStoreDtoFactory = genericModuleStoreDtoFactory;
        }

        /// <summary>
        /// Create a StoreDto from the given store entity
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown when the given store's store type is not supported in
        /// ShipWorks warehouse mode.</exception>
        public async Task<Store> Create(StoreEntity baseStoreEntity)
        {
            var storeEntity = baseStoreEntity as MagentoStoreEntity;
            var store = await genericModuleStoreDtoFactory.Create(storeEntity).ConfigureAwait(false);

            var magentoStore = store as MagentoStore;
            magentoStore.MagentoVersion = (uint) storeEntity.MagentoVersion;

            return magentoStore;
        }
    }
}