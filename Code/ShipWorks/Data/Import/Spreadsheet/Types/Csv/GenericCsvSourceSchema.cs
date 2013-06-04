using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Import.Spreadsheet;
using System.Xml.Linq;
using System.Diagnostics;

namespace ShipWorks.Data.Import.Spreadsheet.Types.Csv
{
    /// <summary>
    /// The schema and formatting of a source CSV\Text file
    /// </summary>
    public class GenericCsvSourceSchema : GenericSpreadsheetSourceSchema
    {
        char delimiter = ',';
        char quotes = '"';
        char quotesEscape = '"';

        string encoding = "";

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericCsvSourceSchema()
            : base(new GenericSpreadsheetSourceColumn[0])
        {
            
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericCsvSourceSchema(IEnumerable<GenericSpreadsheetSourceColumn> columns, char delimiter, char quotes, char quotesEscape, string encoding)
            : base(columns)
        {
            this.delimiter = delimiter;
            this.quotes = quotes;
            this.quotesEscape = quotesEscape;
            this.encoding = encoding;
        }

        /// <summary>
        /// Unique type of the schema
        /// </summary>
        public override string SchemaType
        {
            get { return "GenericFile-CSV"; }
        }

        /// <summary>
        /// Version of the schema settings
        /// </summary>
        public override string SchemaVersion
        {
            get { return "1.0"; }
        }

        /// <summary>
        /// The delimiter that separates fields
        /// </summary>
        public char Delimiter
        {
            get { return delimiter; }
            set { delimiter = value; }
        }

        /// <summary>
        /// The text qualifer for quoting
        /// </summary>
        public char Quotes
        {
            get { return quotes; }
            set { quotes = value; }
        }

        /// <summary>
        /// The escape character for literal quotes within the document
        /// </summary>
        public char QuotesEscape
        {
            get { return quotesEscape; }
            set { quotesEscape = value; }
        }

        /// <summary>
        /// The encoding to use when reading the input file
        /// </summary>
        public string Encoding
        {
            get { return encoding; }
            set { encoding = value; }
        }

        /// <summary>
        /// Save the current content to the given XElement container
        /// </summary>
        public override void SaveTo(XElement xSource)
        {
            xSource.Add(
                new XElement("Delimiter", (byte) Delimiter ),
                new XElement("Quotes", (byte) Quotes ),
                new XElement("QuotesEscape", (byte) QuotesEscape ),
                new XElement("Encoding", encoding));

            base.SaveTo(xSource);
        }

        /// <summary>
        /// Load the properties from the given XElement
        /// </summary>
        public override void LoadFrom(XElement xSource)
        {
            // Properties
            delimiter = ',';
            quotes = '"';
            quotesEscape = '"';
            encoding = "";

            try
            {
                delimiter = (char) (int) xSource.Element("Delimiter");
                quotes = (char) (int) xSource.Element("Quotes");
                quotesEscape = (char) (int) xSource.Element("QuotesEscape");
                encoding = (string) xSource.Element("Encoding");
            }
            catch (FormatException ex)
            {
                // Changed how these were serialized - this can be removed for public release.
                Debug.Fail(ex.Message);
            }

            base.LoadFrom(xSource);
        }
    }
}
