using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Interapptive.Shared.Utility;
using Interapptive.Shared.ComponentRegistration;
using Syncfusion.XlsIO;

namespace ShipWorks.Products.Import
{
    /// <summary>
    /// Reads a spreadsheet and returns lists of skus and bundle skus
    /// </summary>
    [Component]
    public class ProductExcelReader : IProductExcelReader
    {
        private readonly List<ProductToImportDto> skuRows = new List<ProductToImportDto>();
        private readonly List<ProductToImportDto> bundleRows = new List<ProductToImportDto>();

        /// <summary>
        /// Import the products
        /// </summary>
        public (List<ProductToImportDto> SkuRows, List<ProductToImportDto> BundleRows) LoadImportFile(string filename)
        {
            try
            {
                using (Stream fileStream = File.OpenRead(filename))
                {
                    using (ExcelEngine excelEngine = new ExcelEngine())
                    {
                        IWorkbook workbook = excelEngine.Excel.Workbooks.Open(fileStream);

                        Read(workbook.Worksheets[0]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ProductImportException($"An error occurred while reading product spreadsheet '{filename}'", ex);
            }

            return (skuRows, bundleRows);
        }

        /// <summary>
        /// Read the sheet and load it into lists of skus and bundle skus
        /// </summary>
        private void Read(IWorksheet sheet)
        {
            if(sheet.Rows.Length == 0)
            {
                throw new ProductImportException($"Sheet '{sheet.Name}' has no rows.");
            }

            List<ProductToImportDto> allRows = sheet.ExportData<ProductToImportDto>(1, 1, sheet.Rows.Length, sheet.Columns.Length)
                .Skip(1) // Skip the second header row
                .ToList();

            skuRows.AddRange(allRows.Where(r => r.BundleSkus.IsNullOrWhiteSpace()));
            bundleRows.AddRange(allRows.Where(r => !r.BundleSkus.IsNullOrWhiteSpace()));
        }
    }
}
