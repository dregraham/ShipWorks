using System;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Warehouse;

namespace ShipWorks.Stores.Platforms.GenericFile.Warehouse
{
    /// <summary>
    /// Factory for creating StoreDtos from StoreEntities
    /// </summary>
    [KeyedComponent(typeof(IStoreDtoFactory), StoreTypeCode.GenericFile)]
    public class GenericFileStoreDtoFactory : IStoreDtoFactory
    {
        private readonly IStoreDtoHelpers helpers;
        readonly Lazy<string> warehouseID;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="helpers"></param>
        public GenericFileStoreDtoFactory(IStoreDtoHelpers helpers, IConfigurationData configurationData)
        {
            this.helpers = helpers;
            warehouseID = new Lazy<string>(() => configurationData.FetchReadOnly().WarehouseID);
        }

        /// <summary>
        /// Create a StoreDto from the given store entity
        /// </summary>
        public Task<Store> Create(StoreEntity baseStoreEntity)
        {
            GenericFileStore store = helpers.PopulateCommonData(baseStoreEntity, new GenericFileStore());
            store.UniqueIdentifier = Guid.NewGuid().ToString();

            GenericFileStoreEntity storeEntity = baseStoreEntity as GenericFileStoreEntity;

            store.FileFormat = storeEntity.FileFormat;

            return Task.FromResult<Store>(store);
        }
    }
}
