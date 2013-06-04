using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Import.Spreadsheet
{
    /// <summary>
    /// The schema that a Generic CSV map maps on to
    /// </summary>
    public abstract class GenericSpreadsheetTargetSchema
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected GenericSpreadsheetTargetSchema()
        {

        }

        /// <summary>
        /// A unique identifier to uniquely identify this schema
        /// </summary>
        public abstract string SchemaType
        {
            get;
        }

        /// <summary>
        /// The version of the schema
        /// </summary>
        public abstract Version SchemaVersion
        {
            get;
        }

        /// <summary>
        /// The field groups that are available in this schema
        /// </summary>
        public abstract IEnumerable<GenericSpreadsheetTargetFieldGroup> FieldGroups
        {
            get;
        }

        /// <summary>
        /// Create the schema specific settings object for custom schema settings
        /// </summary>
        public abstract GenericSpreadsheetTargetSchemaSettings CreateSettings();

        /// <summary>
        /// Gets the field with the given identifier, or null if it does not exist
        /// </summary>
        public GenericSpreadsheetTargetField GetField(string identifier, GenericSpreadsheetTargetSchemaSettings settings)
        {
            return FieldGroups.SelectMany(g => g.GetFields(settings)).FirstOrDefault(f => f.Identifier == identifier);
        }
    }
}
