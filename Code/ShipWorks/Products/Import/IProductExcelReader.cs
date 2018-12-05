using System.Collections.Generic;
using Interapptive.Shared.Utility;

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
        GenericResult<(List<ProductToImportDto> SkuRows, List<ProductToImportDto> BundleRows)> LoadImportFile(string filename);
    }
}