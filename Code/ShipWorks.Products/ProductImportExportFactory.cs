using System;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Products.Export;
using ShipWorks.Products.Import;

namespace ShipWorks.Products
{
    /// <summary>
    /// Factory for creating product view models
    /// </summary>
    [Component]
    public class ProductViewModelFactory : IProductViewModelFactory
    {
        private readonly Func<IProductExporterViewModel> createExportViewModel;
        private readonly Func<IProductImporterViewModel> createImportViewModel;
        private readonly Func<IProductEditorViewModel> createEditorViewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductViewModelFactory(
            Func<IProductExporterViewModel> createExportViewModel,
            Func<IProductImporterViewModel> createImportViewModel,
            Func<IProductEditorViewModel> createEditorViewModel)
        {
            this.createExportViewModel = createExportViewModel;
            this.createEditorViewModel = createEditorViewModel;
            this.createImportViewModel = createImportViewModel;
        }

        /// <summary>
        /// Create an Export view model
        /// </summary>
        public IProductExporterViewModel CreateExport() => createExportViewModel();

        /// <summary>
        /// Create an Import view model
        /// </summary>
        public IProductImporterViewModel CreateImport() => createImportViewModel();

        /// <summary>
        /// Create an editor view model
        /// </summary>
        public IProductEditorViewModel CreateEditor() => createEditorViewModel();
    }
}
