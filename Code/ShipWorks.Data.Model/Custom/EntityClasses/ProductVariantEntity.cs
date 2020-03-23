using System.Linq;

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
        public static ProductVariantEntity Create(string sku)
        {
            ProductVariantEntity productVariant = new ProductVariantEntity()
            {
                Product = new ProductEntity()
                {
                    IsActive = true,
                    IsBundle = false
                }
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