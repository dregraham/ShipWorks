using System.Data;
using Interapptive.Shared.Utility;

namespace ShipWorks.Products.Export
{
    /// <summary>
    /// Interface for writing product excel spreadsheets.
    /// </summary>
    public interface IProductExcelWriter
    {
        /// <summary>
        /// Write the data to the spreadsheet
        /// </summary>
        GenericResult<string> WriteDataToFile(DataTable data, string filename);
    }
}