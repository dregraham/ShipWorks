using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.IO.Zip;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Warehouse;
using ShipWorks.Stores.Warehouse.StoreData;

namespace ShipWorks.Stores.Platforms.Odbc.Warehouse
{
    /// <summary>
    /// Factory for creating StoreDtos from StoreEntities
    /// </summary>
    [KeyedComponent(typeof(IStoreDtoFactory), StoreTypeCode.Odbc)]
    public class OdbcStoreDtoFactory : IStoreDtoFactory
    {
        private readonly IStoreDtoHelpers helpers;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="helpers"></param>
        public OdbcStoreDtoFactory(IStoreDtoHelpers helpers)
        {
            this.helpers = helpers;
        }

        /// <summary>
        /// Create a StoreDto from the given store entity
        /// </summary>
        public Task<Store> Create(StoreEntity baseStoreEntity)
        {
            OdbcStore store = helpers.PopulateCommonData(baseStoreEntity, new OdbcStore());
            OdbcStoreEntity storeEntity = baseStoreEntity as OdbcStoreEntity;

            store.ImportMap = GZipUtility.Compress(storeEntity.ImportMap);
            store.UploadMap = GZipUtility.Compress(storeEntity.UploadMap);
            store.ImportStrategy = storeEntity.ImportStrategy;
            store.ImportColumnSourceType = storeEntity.ImportColumnSourceType;
            store.ImportColumnSource = storeEntity.ImportColumnSource;
            store.ImportOrderItemStrategy = storeEntity.ImportOrderItemStrategy;
            store.UploadStrategy = storeEntity.UploadStrategy;
            store.UploadColumnSourceType = storeEntity.UploadColumnSourceType;
            store.UploadColumnSource = storeEntity.UploadColumnSource;

            return Task.FromResult<Store>(store);
        }
    }
}
