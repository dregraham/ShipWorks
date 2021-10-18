using System;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.IO.Zip;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Data;
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
        readonly Lazy<string> warehouseID;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="helpers"></param>
        public OdbcStoreDtoFactory(IStoreDtoHelpers helpers, IConfigurationData configurationData)
        {
            this.helpers = helpers;
            warehouseID = new Lazy<string>(() => configurationData.FetchReadOnly().WarehouseID);
        }

        /// <summary>
        /// Create a StoreDto from the given store entity
        /// </summary>
        public Task<Store> Create(StoreEntity baseStoreEntity)
        {
            OdbcStore store = helpers.PopulateCommonData(baseStoreEntity, new OdbcStore());
            // At this point, unique identifier is the license identifier. This could lead us to 
            // pick an existing store for warehouse and not the store the user selected.
            store.UniqueIdentifier = Guid.NewGuid().ToString();

            OdbcStoreEntity storeEntity = baseStoreEntity as OdbcStoreEntity;

            if (storeEntity != null)
            {
                store.ImportMap = storeEntity.ImportMap;
                store.UploadMap = storeEntity.UploadMap;
                store.ImportStrategy = storeEntity.ImportStrategy;
                store.ImportColumnSourceType = storeEntity.ImportColumnSourceType;
                store.ImportColumnSource = storeEntity.ImportColumnSource;
                store.ImportOrderItemStrategy = storeEntity.ImportOrderItemStrategy;
                store.UploadStrategy = storeEntity.UploadStrategy;
                store.UploadColumnSourceType = storeEntity.UploadColumnSourceType;
                store.UploadColumnSource = storeEntity.UploadColumnSource;
                if (storeEntity.ShouldUploadWarehouseOrders)
                {
                    store.OrderImportingWarehouseId = warehouseID.Value;
                }
            }

            return Task.FromResult<Store>(store);
        }
    }
}
