using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Import.Spreadsheet
{
    /// <summary>
    /// Represents a ShipWorks target field that can be mapped to
    /// </summary>
    public class GenericSpreadsheetTargetField
    {
        string identifier;
        string displayName;
        Type dataType;
        bool isRequired;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericSpreadsheetTargetField(string identifier, string displayName, Type dataType, bool isRequired = false)
        {
            this.identifier = identifier;
            this.displayName = displayName;
            this.dataType = dataType;
            this.isRequired = isRequired;
        }

        /// <summary>
        /// Unique ever-unchanging identifier for the field
        /// </summary>
        public string Identifier
        {
            get { return identifier; }
        }

        /// <summary>
        /// User visible name of the field
        /// </summary>
        public string DisplayName
        {
            get { return displayName; }
        }

        /// <summary>
        /// DataType of the field
        /// </summary>
        public Type DataType
        {
            get { return dataType; }
        }

        /// <summary>
        /// Indicates if this is a required field that must be mapped
        /// </summary>
        public bool IsRequired
        {
            get { return isRequired; }
        }
    }
}
