using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Products.Export;

namespace ShipWorks.ApplicationCore.Settings.Warehouse
{
    /// <summary>
    /// Api for managing interaction with the warehouse for settings
    /// </summary>
    [Component]
    public class WarehouseSettingsApi : IWarehouseSettingsApi
    {
        private readonly IWarehouseList warehouseListRequest;
        private readonly IWarehouseLink warehouseLink;
        private readonly IWarehouseProductUploader uploader;

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseSettingsApi(IWarehouseList warehouseListRequest, IWarehouseLink warehouseLink, IWarehouseProductUploader uploader)
        {
            this.warehouseLink = warehouseLink;
            this.uploader = uploader;
            this.warehouseListRequest = warehouseListRequest;
        }

        /// <summary>
        /// Ge all warehouses
        /// </summary>
        public Task<GenericResult<WarehouseListDto>> GetAllWarehouses() => warehouseListRequest.GetList();

        /// <summary>
        /// Link the warehouse with this instance of ShipWorks
        /// </summary>
        public Task<Result> Link(string id) => warehouseLink.Link(id);

        /// <summary>
        /// Upload products to the associated warehouse
        /// </summary>
        public Task UploadProducts(ISingleItemProgressDialog progressItem) => uploader.Upload(progressItem);

        /// <summary>
        /// Get a count of products that need to be uploaded
        /// </summary>
        public Task<int> GetCountOfProductsThatNeedUpload() => uploader.GetCountOfProductsThatNeedUpload();
    }
}
