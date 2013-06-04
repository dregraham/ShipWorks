using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Import.Spreadsheet.Types.Csv;

namespace ShipWorks.Data.Import.Spreadsheet
{
    /// <summary>
    /// Exception thrown when a CSV import issue is encountered
    /// </summary>
    public class GenericSpreadsheetException : Exception
    {
        public GenericSpreadsheetException()
        {

        }

        public GenericSpreadsheetException(string message)
            : base(message)
        {

        }

        public GenericSpreadsheetException(string message, Exception inner)
            : base(message, inner)
        {

        }

        /// <summary>
        /// Message to display to the user
        /// </summary>
        public override string Message
        {
            get
            {
                return GenericCsvUtility.StripRawData(base.Message);
            }
        }
    }
}
