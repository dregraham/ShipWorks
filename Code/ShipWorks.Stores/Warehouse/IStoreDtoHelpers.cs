using System;
using System.Threading.Tasks;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Warehouse
{
    /// <summary>
    /// Helper methods for all StoreDtoFactories
    /// </summary>
    public interface IStoreDtoHelpers
    {
        /// <summary>
        /// Populate the common data for all stores
        /// </summary>
        T PopulateCommonData<T>(StoreEntity storeEntity, T store) where T : Store;

        /// <summary>
        /// Encrypt a secret using the AWS encryption service
        /// </summary>
        Task<string> EncryptSecret(string authToken);

        /// <summary>
        /// get UNIX timestamp from the given date time
        /// </summary>
        ulong GetUnixTimestampMillis(DateTime? dateTime);
    }
}