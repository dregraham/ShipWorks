using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpreadsheetGear;

namespace ShipWorks.Data.Import.Spreadsheet.Types.Excel
{
    /// <summary>
    /// Utility class for working with Excel
    /// </summary>
    public static class GenericExcelUtility
    {
        /// <summary>
        /// Determine the headers of the worksheet starting at the given address
        /// </summary>
        public static Dictionary<string, int> DetermineHeaders(IWorksheet worksheet, string firstAddress)
        {
            IRange startCell = worksheet.Cells[firstAddress];

            int row = startCell.Row;
            int column = startCell.Column;

            Dictionary<string, int> headers = new Dictionary<string, int>();

            while (true)
            {
                string cellText = (worksheet.Cells[row, column].Value ?? (object) "").ToString();

                if (string.IsNullOrWhiteSpace(cellText))
                {
                    break;
                }
                else
                {
                    headers[cellText] = column;
                }

                column++;
            }

            return headers;
        }
    }
}
