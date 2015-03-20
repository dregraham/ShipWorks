using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using ShipWorks.Data.Import.Spreadsheet;

namespace ShipWorks.Data.Import.Spreadsheet.OrderSchema
{
    /// <summary>
    /// Custom settings for generic file import
    /// </summary>
    public class GenericSpreadsheetOrderMapSettings : GenericSpreadsheetTargetSchemaSettings
    {
        GenericSpreadsheetOrderMultipleItemStrategy multiItemStrategy = GenericSpreadsheetOrderMultipleItemStrategy.SingleLine;

        // If all items on a single line, how many are there
        int singleLineCount = 5;

        // If items on multiple lines, what fields define the "uniqueness" of the repeating lines
        List<string> multiLineKeyColumns = new List<string>();
        
        /// <summary>
        /// Strategy for reading in multiple line items
        /// </summary>
        public GenericSpreadsheetOrderMultipleItemStrategy MultiItemStrategy
        {
            get { return multiItemStrategy; }
            set { multiItemStrategy = value; }
        }

        /// <summary>
        /// If items are all on a single line, this controls how many items there are columns for
        /// </summary>
        public int SingleLineCount
        {
            get { return singleLineCount; }
            set { singleLineCount = value; }
        }

        /// <summary>
        /// If items are accross multiple lines, this controls the source fiels that are the "primary key" for detecting repeat lines
        /// </summary>
        public List<string> MultiLineKeyColumns
        {
            get { return multiLineKeyColumns; }
        }

        /// <summary>
        /// Gets or sets how many attributes appear on a line.
        /// </summary>
        public int AttributeCountPerLine { get; set; }

        /// <summary>
        /// Save the settings to the given element as the parent
        /// </summary>
        public override void SaveTo(XElement xElement)
        {
            xElement.Add(
                new XElement("Strategy", (int) multiItemStrategy),
                new XElement("Single",
                    new XElement("Count", singleLineCount)),                
                new XElement("Multiple",
                    new XElement("KeyColumns",
                        multiLineKeyColumns.Select(column =>
                            new XElement("Column", column)))),
                new XElement("AttributeCount", AttributeCountPerLine)
            );
        }

        /// <summary>
        /// Load the settings from the given element
        /// </summary>
        public override void LoadFrom(XElement xElement)
        {
            if (xElement == null)
            {
                return;
            }

            multiItemStrategy = (GenericSpreadsheetOrderMultipleItemStrategy) (int) xElement.Element("Strategy");
            singleLineCount = (int) xElement.XPathSelectElement("Single/Count");
            multiLineKeyColumns = xElement.XPathSelectElements("Multiple/KeyColumns/Column").Select(x => x.Value).ToList();

            // the attribute node may not be there if the map was created on a version that did not support loading attributes
            XElement attributeNode = xElement.Element("AttributeCount");
            AttributeCountPerLine = attributeNode == null ? 0 : int.Parse(attributeNode.Value);
        }
    }
}
