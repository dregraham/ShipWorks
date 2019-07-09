using System;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Warehouse.Encryption;

namespace ShipWorks.Stores.Warehouse
{
    /// <summary>
    /// Helper methods for all StoreDtoFactories
    /// </summary>
    [Component]
    public class StoreDtoHelpers : IStoreDtoHelpers
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private readonly IStoreTypeManager storeTypeManager;
        private readonly IWarehouseEncryptionService encryptionService;

        /// <summary>
        /// Constructor
        /// </summary>
        public StoreDtoHelpers(IStoreTypeManager storeTypeManager,
                               IWarehouseEncryptionService encryptionService)
        {
            this.encryptionService = encryptionService;
            this.storeTypeManager = storeTypeManager;
        }

        /// <summary>
        /// Populate the common data for all stores
        /// </summary>
        public T PopulateCommonData<T>(StoreEntity storeEntity, T store) where T : Store
        {
            store.Name = storeEntity.StoreName;
            store.StoreType = storeEntity.TypeCode;
            store.UniqueIdentifier = storeTypeManager.GetType(storeEntity).LicenseIdentifier;

            return store;
        }

        /// <summary>
        /// Encrypt a secret using the AWS encryption service
        /// </summary>
        public Task<string> EncryptSecret(string secret) => encryptionService.Encrypt(secret);

        /// <summary>
        /// get unix timestamp from the given date time
        /// </summary>
        public ulong GetUnixTimestampMillis(DateTime? dateTime) =>
            Convert.ToUInt64((dateTime - UnixEpoch)?.TotalMilliseconds ?? 0);
    }
}