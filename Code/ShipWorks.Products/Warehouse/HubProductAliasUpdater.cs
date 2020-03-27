using System.Linq;
using Interapptive.Shared.Collections;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Products.Warehouse.DTO;

namespace ShipWorks.Products.Warehouse
{
    /// <summary>
    /// Update product aliases
    /// </summary>
    public class HubProductAliasUpdater : IHubProductItemUpdater
    {
        /// <summary>
        /// Update the product variant with the data from the hub
        /// </summary>
        public void UpdateProductVariant(ProductVariantEntity productVariant, WarehouseProduct warehouseProduct)
        {
            productVariant.Aliases.RemovedEntitiesTracker = new ProductVariantAliasCollection();
            var aliasesToRemove = productVariant.Aliases
                .Where(x => !x.IsDefault)
                .Where(x => warehouseProduct.Aliases.Keys.None(sku => sku == x.Sku))
                .ToList();

            foreach (var alias in aliasesToRemove)
            {
                productVariant.Aliases.Remove(alias);
            }

            foreach (var hubAlias in warehouseProduct.Aliases.Values)
            {
                var alias = productVariant.Aliases.FirstOrDefault(x => x.Sku == hubAlias.Sku);
                if (alias == null)
                {
                    alias = productVariant.Aliases.AddNew();
                    alias.Sku = hubAlias.Sku;
                }
                alias.AliasName = hubAlias.Name;
            }
        }
    }
}
