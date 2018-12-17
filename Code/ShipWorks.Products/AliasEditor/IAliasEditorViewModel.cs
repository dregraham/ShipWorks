using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Products.AliasEditor
{
    public interface IAliasEditorViewModel
    {
        /// <summary>
        /// Load the view model with the given product
        /// </summary>
        void Load(ProductVariantEntity productVariantEntity);

        /// <summary>
        /// Save the aliases to the product
        /// </summary>
        void Save();
    }
}