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
        public GenericResult<(List<ProductToImportDto> SkuRows, List<ProductToImportDto> BundleRows)> LoadImportFile(string filename)
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
            catch (ProductImportException ex)
            {
                return GenericResult.FromError<(List<ProductToImportDto> SkuRows, List<ProductToImportDto> BundleRows)>(ex);
            }
            catch (Exception ex)
            {
                return GenericResult.FromError<(List<ProductToImportDto> SkuRows, List<ProductToImportDto> BundleRows)>(
                    new ProductImportException($"An error occurred while reading product spreadsheet '{filename}'", ex));
            }

            return GenericResult.FromSuccess((skuRows, bundleRows));
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

            if (sheet.Rows.Length < 2)
            {
                throw new ProductImportException("The spreadsheet does not contain the correct header rows.");
            }

            IRange row = sheet.Rows.FirstOrDefault();

            // Remove any leading/trailing spaces from the header row.
            foreach (var column in row.Columns)
            {
                if (!column.Value.IsNullOrWhiteSpace())
                {
                    column.Value = column.Value.Trim();
                }
            }

            List<string> columnNames = ProductToImportDto.PropertyNames;
            string missingOnes = String.Join(",", columnNames
                .Except(row.Columns.Select(c => c.Value.ToString())));

            if (missingOnes.Length > 0)
            {
                throw new ProductImportException($"The spreadsheet has invalid columns: {missingOnes}. Make sure there are no extra spaces in the column text.");
            }

            List<ProductToImportDto> allRows = sheet.ExportData<ProductToImportDto>(1, 1, sheet.Rows.Length, sheet.Columns.Length)
                .Skip(1) // Skip the second header row
                .ToList();

            skuRows.AddRange(allRows.Where(r => r.BundleSkus.IsNullOrWhiteSpace()));
            bundleRows.AddRange(allRows.Where(r => !r.BundleSkus.IsNullOrWhiteSpace()));
        }
    }
}
