using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Import.Spreadsheet
{
    /// <summary>
    /// Represents the mapping between a source column and a target field
    /// </summary>
    public class GenericSpreadsheetFieldMapping
    {
        GenericSpreadsheetTargetField targetField;
        string sourceColumnName;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericSpreadsheetFieldMapping(GenericSpreadsheetTargetField targetField, string sourceColumnName)
        {
            this.targetField = targetField;
            this.sourceColumnName = sourceColumnName;
        }

        /// <summary>
        /// The target field the data will be put into
        /// </summary>
        public GenericSpreadsheetTargetField TargetField
        {
            get { return targetField; }
        }

        /// <summary>
        /// The source column
        /// </summary>
        public string SourceColumnName
        {
            get { return sourceColumnName; }
        }
    }
}
