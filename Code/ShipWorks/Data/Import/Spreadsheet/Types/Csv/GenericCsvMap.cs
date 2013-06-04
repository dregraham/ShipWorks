using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Import.Spreadsheet.OrderSchema;

namespace ShipWorks.Data.Import.Spreadsheet.Types.Csv
{
    /// <summary>
    /// Order specific CSV mapping schema
    /// </summary>
    public class GenericCsvMap : GenericSpreadsheetMap
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GenericCsvMap(GenericSpreadsheetTargetSchema targetSchema)
            : base(targetSchema)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericCsvMap(GenericSpreadsheetTargetSchema targetSchema, string mapXml)
            : base(targetSchema, mapXml)
        {

        }

        /// <summary>
        /// Copy constructor for cloning
        /// </summary>
        public GenericCsvMap(GenericCsvMap copy)
            : base(copy)
        {

        }

        /// <summary>
        /// Clone this map
        /// </summary>
        public override GenericSpreadsheetMap Clone()
        {
            return new GenericCsvMap(this);
        }

        /// <summary>
        /// Create a new instance of the a source schema
        /// </summary>
        protected override GenericSpreadsheetSourceSchema CreateSourceSchema()
        {
            return new GenericCsvSourceSchema();
        }

        /// <summary>
        /// Easier access to most derived source schema type
        /// </summary>
        public new GenericCsvSourceSchema SourceSchema
        {
            get { return (GenericCsvSourceSchema) base.SourceSchema; }
            set { base.SourceSchema = value; }
        }
    }
}
