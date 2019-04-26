using System.Threading.Tasks;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;

namespace ShipWorks.ApplicationCore.Settings.Warehouse
{
    /// <summary>
    /// Api for managing interaction with the warehouse for settings
    /// </summary>
    public interface IWarehouseSettingsApi
    {
        /// <summary>
        /// Ge all warehouses
        /// </summary>
        Task<GenericResult<WarehouseListDto>> GetAllWarehouses();

        /// <summary>
        /// Link the warehouse with this instance of ShipWorks
        /// </summary>
        Task<Result> Link(string id);

        /// <summary>
        /// Upload products to the associated warehouse
        /// </summary>
        Task UploadProducts(ISingleItemProgressDialog progressItem);

        /// <summary>
        /// Get a count of products that need to be uploaded
        /// </summary>
        Task<int> GetCountOfProductsThatNeedUpload();
    }
}