using System.Threading;
using System.Threading.Tasks;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Products.Warehouse.DTO;

namespace ShipWorks.Products.Warehouse
{
    /// <summary>
    /// Interface for handling syncing product bundles 
    /// </summary>
    public interface IWarehouseProductBundleService
    {
        /// <summary>
        /// Handle syncing product bundles 
        /// </summary>
        Task UpdateProductBundleDetails(ISqlAdapter sqlAdapter, ProductVariantEntity productVariant, WarehouseProduct warehouseProductDto, CancellationToken cancellationToken);
    }
}
