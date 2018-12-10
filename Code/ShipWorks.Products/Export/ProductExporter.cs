using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Products.Import;

namespace ShipWorks.Products.Export
{
    /// <summary>
    /// Class for exporting products
    /// </summary>
    [Component]
    public class ProductExporter : IProductExporter
    {
        private readonly IProductCatalog productCatalog;
        private readonly IProductExcelWriter productExcelWriter;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductExporter(IProductCatalog productCatalog, IProductExcelWriter productExcelWriter
            , Func<Type, ILog> logFactory)
        {
            this.productCatalog = productCatalog;
            this.productExcelWriter = productExcelWriter;
            log = logFactory(GetType());
        }

        /// <summary>
        /// Export products to the given file
        /// </summary>
        public async Task Export(string filePath, IProgressReporter progressReporter)
        {
            progressReporter.PercentComplete = 0;
            DataTable data;

            try
            {
                data = await productCatalog.GetProductDataForExport().ConfigureAwait(false);
            }
            catch (ProductImportException ex)
            {
                log.Error(ex);
                throw;
            }

            progressReporter.PercentComplete = 50;

            if (progressReporter.IsCancelRequested)
            {
                return;
            }

            GenericResult<string> result = productExcelWriter.WriteDataToFile(data, filePath);

            if (result.Failure)
            {
                log.Error(result.Exception);
            }

            progressReporter.PercentComplete = 100;
        }
    }
}
