using System.Linq;

namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Read only entity class which represents the entity 'ProductVariant'.
    /// </summary>
    public partial class ReadOnlyProductVariantEntity
    {
        /// <summary>
        /// The default sku for the product variant
        /// </summary>
        public string DefaultSku => Aliases.FirstOrDefault(a => a.IsDefault)?.Sku;
    }
}