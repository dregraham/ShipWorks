using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericFile.Warehouse;

namespace ShipWorks.Stores.Platforms.GenericFile
{
    /// <summary>
    /// GenericFileStoreWarehouseRepository
    /// </summary>
    public interface IGenericFileStoreWarehouseRepository
    {
        /// <summary>
        /// Get store from hub
        /// </summary>
        GenericFileStore GetStore(GenericFileStoreEntity store);
    }
}