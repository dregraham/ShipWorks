using ShipWorks.Products.Export;
using ShipWorks.Products.Import;

namespace ShipWorks.Products
{
    /// <summary>
    /// Factory for creating product view models
    /// </summary>
    public interface IProductViewModelFactory
    {
        /// <summary>
        /// Create an editor view model
        /// </summary>
        IProductEditorViewModel CreateEditor();

        /// <summary>
        /// Create an Export view model
        /// </summary>
        IProductExporterViewModel CreateExport();

        /// <summary>
        /// Create an Import view model
        /// </summary>
        IProductImporterViewModel CreateImport();
    }
}