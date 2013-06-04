using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Import.Spreadsheet
{
    /// <summary>
    /// Represents a single column to be mapped within a generic CSV\text import file
    /// </summary>
    public class GenericSpreadsheetSourceColumn
    {
        // Special case for a target field that is not mapped on to any column
        static GenericSpreadsheetSourceColumn nullColumn = new GenericSpreadsheetSourceColumn("__sw_nullcolumn");

        string name;
        List<string> samples = new List<string>();

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericSpreadsheetSourceColumn(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Represents a null (unmapped) column
        /// </summary>
        public static GenericSpreadsheetSourceColumn Null
        {
            get { return nullColumn; }
        }

        /// <summary>
        /// Name of the source column header
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Sample values taken from the original source document
        /// </summary>
        public List<string> Samples
        {
            get { return samples; }
        }
    }
}
