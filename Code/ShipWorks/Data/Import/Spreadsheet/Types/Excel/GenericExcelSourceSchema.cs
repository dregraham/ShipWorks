using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Import.Spreadsheet;
using System.Xml.Linq;
using System.Diagnostics;

namespace ShipWorks.Data.Import.Spreadsheet.Types.Excel
{
    /// <summary>
    /// The schema and formatting of a source Excel file
    /// </summary>
    public class GenericExcelSourceSchema : GenericSpreadsheetSourceSchema
    {
        string sheetName;
        string startAddress;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericExcelSourceSchema()
            : base(new GenericSpreadsheetSourceColumn[0])
        {
            
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericExcelSourceSchema(IEnumerable<GenericSpreadsheetSourceColumn> columns, string sheetName, string startAddress)
            : base(columns)
        {
            this.sheetName = sheetName;
            this.startAddress = startAddress;
        }

        /// <summary>
        /// Unique type of the schema
        /// </summary>
        public override string SchemaType
        {
            get { return "GenericFile-Excel"; }
        }

        /// <summary>
        /// Version of the schema settings
        /// </summary>
        public override string SchemaVersion
        {
            get { return "1.0"; }
        }

        /// <summary>
        /// The name of the Excel sheet to read from
        /// </summary>
        public string SheetName
        {
            get { return sheetName; }
            set { sheetName = value; }
        }

        /// <summary>
        /// The starting address of the headers area
        /// </summary>
        public string StartAddress
        {
            get { return startAddress; }
            set { startAddress = value; }
        }

        /// <summary>
        /// Save the current content to the given XElement container
        /// </summary>
        public override void SaveTo(XElement xSource)
        {
            xSource.Add(
                new XElement("Sheet", sheetName ),
                new XElement("Address", startAddress));

            base.SaveTo(xSource);
        }

        /// <summary>
        /// Load the properties from the given XElement
        /// </summary>
        public override void LoadFrom(XElement xSource)
        {
            sheetName = (string) xSource.Element("Sheet");
            startAddress = (string) xSource.Element("Address");

            base.LoadFrom(xSource);
        }
    }
}
