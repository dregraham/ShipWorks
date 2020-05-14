using System;
using System.Linq;
using Interapptive.Shared.Utility;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Entity class which represents the entity 'ProductVariant'.
    /// </summary>
    public partial class ProductVariantEntity
    {
        /// <summary>
        /// Create a new default ProductVariant
        /// </summary>
        public static ProductVariantEntity Create(string sku, DateTime createDateTime)
        {
            ProductVariantEntity productVariant = new ProductVariantEntity()
            {
                Product = ProductEntity.Create(),
                CreatedDate = createDateTime.ToSqlSafeDateTime()
            };

            productVariant.Aliases.Add(new ProductVariantAliasEntity()
            {
                AliasName = string.Empty,
                IsDefault = true,
                Sku = sku,
            });

            return productVariant;
        }

        /// <summary>
        /// The default sku for the product variant
        /// </summary>
        public string DefaultSku => Aliases?.FirstOrDefault(a => a.IsDefault)?.Sku;
    }
}