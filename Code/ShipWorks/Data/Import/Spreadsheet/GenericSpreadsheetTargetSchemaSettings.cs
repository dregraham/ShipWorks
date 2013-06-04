using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ShipWorks.Data.Import.Spreadsheet
{
    /// <summary>
    /// A property bag of settings for maps
    /// </summary>
    public abstract class GenericSpreadsheetTargetSchemaSettings
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected GenericSpreadsheetTargetSchemaSettings()
        {

        }

        /// <summary>
        /// Save the settings to the given element as the parent
        /// </summary>
        public abstract void SaveTo(XElement xElement);

        /// <summary>
        /// Load the settings from the given element
        /// </summary>
        public abstract void LoadFrom(XElement xElement);
    }
}
