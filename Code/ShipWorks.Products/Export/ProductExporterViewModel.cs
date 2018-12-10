using System;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.IO;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Core.Common.Threading;
using static Interapptive.Shared.Utility.Functional;

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
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IMessageHelper messageHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductExporterViewModel(IFileSelector fileSelector, IProductExporter productExporter, IDateTimeProvider dateTimeProvider, IMessageHelper messageHelper)
        {
            this.messageHelper = messageHelper;
            this.dateTimeProvider = dateTimeProvider;
            this.productExporter = productExporter;
            this.fileSelector = fileSelector;
        }

        /// <summary>
        /// Export products
        /// </summary>
        public void ExportProducts()
        {
            var timestamp = dateTimeProvider.Now.ToString("yyyy-MM-dd_HHmmss");
            fileSelector.GetFilePathToSave("Comma Separated|*.csv|All Files|*.*", $"ProductExport_{timestamp}.csv")
                .Do(path => ExportProducts(path).Forget());
        }

        /// <summary>
        /// Export products to the given path
        /// </summary>
        private Task<Unit> ExportProducts(string path) =>
            UsingAsync(
                    messageHelper.ShowProgressDialog("Exporting products", "Exporting products"),
                    dialog => productExporter.Export(path, dialog.ProgressItem).ToTyped<Unit>())
                .Do(_ => messageHelper.ShowInformation("Product export succeeded"))
                .Recover(HandleExportError);

        /// <summary>
        /// Handle export errors
        /// </summary>
        private Unit HandleExportError(Exception ex)
        {
            var actualException = ex is AggregateException aggregateException ?
                aggregateException.InnerExceptions.FirstOrDefault() :
                ex;
            messageHelper.ShowError(actualException.Message, actualException);
            return Unit.Default;
        }
    }
}
