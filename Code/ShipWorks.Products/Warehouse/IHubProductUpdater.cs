using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Products.Warehouse.DTO;

namespace ShipWorks.Products.Warehouse
{
    /// <summary>
    /// Update product a product variant with data from the Hub
    /// </summary>
    public interface IHubProductUpdater
    {
        /// <summary>
        /// Product data from the Hub
        /// </summary>
        WarehouseProduct ProductData { get; }

        /// <summary>
        /// Product variant in ShipWorks
        /// </summary>
        ProductVariantEntity ProductVariant { get; set; }

        /// <summary>
        /// Update the product variant with the data from the hub
        /// </summary>
        void UpdateProductVariant();
    }
}