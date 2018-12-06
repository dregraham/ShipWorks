using System.Linq;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Entity class which represents the entity 'ProductVariant'.
    /// </summary>
    public partial class ProductVariantEntity
    {
        /// <summary>
        /// The default sku for the product variant
        /// </summary>
        public string DefaultSku => Aliases?.FirstOrDefault(a => a.IsDefault)?.Sku;
    }
}