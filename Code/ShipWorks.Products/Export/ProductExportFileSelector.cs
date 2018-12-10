using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Products.Export
{
    [Component]
    public class ProductExportFileSelector
    {
        private readonly IProductExporter productExporter;

        public ProductExportFileSelector(IProductExporter productExporter)
        {
            this.productExporter = productExporter;
        }
    }
}
