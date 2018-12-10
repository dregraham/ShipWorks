using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.IO;

namespace ShipWorks.Products.Export
{
    /// <summary>
    /// ViewModel for exporting products
    /// </summary>
    [Component]
    public class ProductExporterViewModel : IProductExporterViewModel
    {
        private readonly IFileSelector fileSelector;
        private readonly IProductExporter productExporter;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductExporterViewModel(IFileSelector fileSelector, IProductExporter productExporter)
        {
            this.productExporter = productExporter;
            this.fileSelector = fileSelector;
        }

        /// <summary>
        /// Export products
        /// </summary>
        public void ExportProducts()
        {

        }
    }
}
