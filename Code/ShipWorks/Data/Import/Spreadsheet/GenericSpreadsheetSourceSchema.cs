using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;

namespace ShipWorks.Data.Import.Spreadsheet
{
    /// <summary>
    /// Base class for source spreadhseet schemas
    /// </summary>
    public abstract class GenericSpreadsheetSourceSchema
    {
        List<GenericSpreadsheetSourceColumn> sourceColumns;

        /// <summary>
        /// Constructor
        /// </summary>
        protected GenericSpreadsheetSourceSchema(IEnumerable<GenericSpreadsheetSourceColumn> columns)
        {
            this.sourceColumns = columns.ToList();
        }

        /// <summary>
        /// Must be provided by derived classes
        /// </summary>
        public abstract string SchemaType { get; }

        /// <summary>
        /// Must be provided by derived classes
        /// </summary>
        public abstract string SchemaVersion { get; }

        /// <summary>
        /// The list of column (headers)
        /// </summary>
        public List<GenericSpreadsheetSourceColumn> Columns
        {
            get { return sourceColumns; }
        }

        /// <summary>
        /// Save the current content to the given source
        /// </summary>
        public virtual void SaveTo(XElement xSource)
        {
            xSource.Add(
                new XAttribute("type", SchemaType),
                new XAttribute("version", SchemaVersion));

            xSource.Add(
                new XElement("Columns",
                    sourceColumns.Select(column =>
                        new XElement("Column",
                            new XElement("Name", column.Name),
                            new XElement("Samples",
                                column.Samples.Select(sample =>
                                    new XElement("Sample", sample)))))));
        }

        /// <summary>
        /// Load the schema content from the given XElement
        /// </summary>
        public virtual void LoadFrom(XElement xSource)
        {
            sourceColumns.Clear();

            string type = (string) xSource.Attribute("type");
            if (type != SchemaType)
            {
                throw new GenericSpreadsheetException("The selected map is a different type than was expected.");
            }

            // Source columns
            foreach (XElement xColumn in xSource.XPathSelectElements("Columns/Column"))
            {
                GenericSpreadsheetSourceColumn column = new GenericSpreadsheetSourceColumn((string) xColumn.Element("Name"));
                column.Samples.AddRange(xColumn.XPathSelectElements("Samples/Sample").Select(sample => sample.Value));

                sourceColumns.Add(column);
            }
        }
    }
}
