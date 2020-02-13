using System;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Warehouse;
using ShipWorks.Stores.Warehouse.StoreData;

namespace ShipWorks.Stores.Platforms.Volusion.Warehouse
{
    /// <summary>
    /// Factory for creating StoreDtos from StoreEntities
    /// </summary>
    [KeyedComponent(typeof(IStoreDtoFactory), StoreTypeCode.Volusion)]
    public class VolusionStoreDtoFactory : IStoreDtoFactory
    {
        private readonly IStoreDtoHelpers helpers;

        /// <summary>
        /// Constructor
        /// </summary>
        public VolusionStoreDtoFactory(IStoreDtoHelpers helpers)
        {
            this.helpers = helpers;
        }

        /// <summary>
        /// Create a StoreDto from the given store entity
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown when the given store's store type is not supported in
        /// ShipWorks warehouse mode.</exception>
        public async Task<Store> Create(StoreEntity baseStoreEntity)
        {
            var storeEntity = baseStoreEntity as VolusionStoreEntity;
            var store = helpers.PopulateCommonData(storeEntity, new VolusionStore());

            store.Username = storeEntity.WebUserName;
            store.EncryptedPassword = storeEntity.ApiPassword;
            store.BaseUrl = storeEntity.StoreUrl;

            return await Task.FromResult(store);
        }
    }
}
