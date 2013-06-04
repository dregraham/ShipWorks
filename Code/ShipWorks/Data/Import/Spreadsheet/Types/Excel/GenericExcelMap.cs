using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Import.Spreadsheet.OrderSchema;

namespace ShipWorks.Data.Import.Spreadsheet.Types.Excel
{
    /// <summary>
    /// Order specific Excel mapping schema
    /// </summary>
    public class GenericExcelMap : GenericSpreadsheetMap
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GenericExcelMap(GenericSpreadsheetTargetSchema targetSchema)
            : base(targetSchema)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericExcelMap(GenericSpreadsheetTargetSchema targetSchema, string mapXml)
            : base(targetSchema, mapXml)
        {

        }

        /// <summary>
        /// Copy constructor for cloning
        /// </summary>
        public GenericExcelMap(GenericExcelMap copy)
            : base(copy)
        {

        }

        /// <summary>
        /// Clone this map
        /// </summary>
        public override GenericSpreadsheetMap Clone()
        {
            return new GenericExcelMap(this);
        }

        /// <summary>
        /// Create a new instance of the a source schema
        /// </summary>
        protected override GenericSpreadsheetSourceSchema CreateSourceSchema()
        {
            return new GenericExcelSourceSchema();
        }

        /// <summary>
        /// Easier access to most derived source schema type
        /// </summary>
        public new GenericExcelSourceSchema SourceSchema
        {
            get { return (GenericExcelSourceSchema) base.SourceSchema; }
            set { base.SourceSchema = value; }
        }
    }
}
