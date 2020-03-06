using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse
{
    /// <summary>
    /// Upload skus to a warehouse
    /// </summary>
    public interface IUploadSkusToWarehouse
    {
        /// <summary>
        /// Upload skus to a warehouse
        /// </summary>
        Task<Result> Upload(UploadProductsRequest uploadProductsRequest);
    }
}
