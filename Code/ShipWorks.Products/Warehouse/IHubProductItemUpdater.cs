using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Products.Warehouse.DTO;

namespace ShipWorks.Products.Warehouse
{
    /// <summary>
    /// Update some details about a product
    /// </summary>
    [Service]
    public interface IHubProductItemUpdater
    {
        /// <summary>
        /// Update the product variant with the data from the hub
        /// </summary>
        void UpdateProductVariant(ProductVariantEntity productVariant, WarehouseProduct warehouseProduct);
    }
}
