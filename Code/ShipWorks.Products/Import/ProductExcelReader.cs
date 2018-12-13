using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
        public static readonly string SkuSeparatorRegex = @"(?<!($|[^\\])(\\\\)*?\\)\|";
        public static readonly string SkuQuantitySeparatorRegex = @"(?<!($|[^\\])(\\\\)*?\\):";
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
            catch (IOException ex) when(ex.HResult == -2147024864)
            {
                return GenericResult.FromError<(List<ProductToImportDto> SkuRows, List<ProductToImportDto> BundleRows)>(
                    new ProductImportException($"An error occurred while reading product spreadsheet '{filename}'." +
                                               $"{Environment.NewLine}{Environment.NewLine}Verify that it is not already open by another application.", ex));
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

            allRows.ForEach(r =>
            {
                if (!r.AliasSkus.IsNullOrWhiteSpace())
                {
                    var aliasSkus = Regex.Split(r.AliasSkus, SkuSeparatorRegex, RegexOptions.IgnoreCase)
                        .Where(s => !s.IsNullOrWhiteSpace() && !s.Equals(r.Sku.Trim(), StringComparison.InvariantCultureIgnoreCase))
                        .Distinct()
                        .Select(s => Regex.Unescape(s).Trim());

                    r.AliasSkuList = aliasSkus;
                }
                else
                {
                    r.AliasSkuList = Enumerable.Empty<string>();
                }

                r.BundleSkuList = GetBundleSkuList(r.Sku, r.BundleSkus, r.AliasSkuList);
            });

            skuRows.AddRange(allRows.Where(r => r.BundleSkus.IsNullOrWhiteSpace()));
            bundleRows.AddRange(allRows.Where(r => !r.BundleSkus.IsNullOrWhiteSpace()));
        }

        /// <summary>
        /// Build the list of product bundle sku and quantity
        /// </summary>
        private IEnumerable<(string Sku, int Quantity)> GetBundleSkuList(string sku, string bundleSkus, IEnumerable<string> aliasSkuList)
        {
            if (bundleSkus.IsNullOrWhiteSpace())
            {
                return Enumerable.Empty<(string, int)>();
            }

            return Regex.Split(bundleSkus, SkuSeparatorRegex, RegexOptions.IgnoreCase)
                .Select(skuAndQty => Regex.Split(skuAndQty, SkuQuantitySeparatorRegex, RegexOptions.IgnoreCase))
                .Where(values => !(values.Length == 1 && values[0].IsNullOrWhiteSpace())) // Ignore extra beginning/ending delimiters
                .Select(values =>
                {
                    if (values.Length != 2)
                    {
                        throw new ProductImportException($"Quantity is required, but wasn't supplied for bundled item SKU {values[0]}");
                    }

                    string testSku = Regex.Unescape(values[0]);
                    string testQty = Regex.Unescape(values[1]);
                    if (testSku.Equals(sku, StringComparison.InvariantCultureIgnoreCase) ||
                        aliasSkuList.Any(a => a.Equals(testSku, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        throw new ProductImportException($"Bundles may not be composed of its SKU or any of its alias SKUs.  Problem SKU: {testSku}");
                    }

                    int quantity = GetValue(testQty, "Bundle Quantity", 0);
                    if (quantity <= 0)
                    {
                        throw new ProductImportException($"Quantity must be greater than 0 for bundled item SKU {values[0]}");
                    }

                    return (testSku, quantity);
                });
        }

        /// <summary>
        /// Get T value of the given string.
        /// </summary>
        public static T GetValue<T>(string text, string propertyName, T defaultValue)
        {
            if (!text.IsNullOrWhiteSpace())
            {
                try
                {
                    var converter = TypeDescriptor.GetConverter(typeof(T));
                    if (converter != null)
                    {
                        return (T) converter.ConvertFromString(text);
                    }
                }
                catch
                {
                    // throwing below
                }

                throw new ProductImportException($"Unable to convert '{propertyName}' with value {text.Trim()} to a number.");
            }

            return defaultValue;
        }
    }
}
