using System.Collections.Generic;

namespace ShipWorks.Products.Import
{
    /// <summary>
    /// Interface for reading product excel spreadsheets.
    /// </summary>
    public interface IProductExcelReader
    {
        /// <summary>
        /// Load the spreadsheet
        /// </summary>
        (List<ProductToImportDto> SkuRows, List<ProductToImportDto> BundleRows) LoadImportFile(string filename);
    }
}