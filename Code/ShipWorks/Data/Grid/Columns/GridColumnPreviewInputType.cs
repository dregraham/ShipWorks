using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Grid.Columns
{
    /// <summary>
    /// How a DisplayType's preview should be handled
    /// </summary>
    public enum GridColumnPreviewInputType
    {
        /// <summary>
        /// The example data is an Entity that FormatValue function is called on
        /// </summary>
        Entity,

        /// <summary>
        /// The example data is the actual entity value to use and be passed to formatting
        /// </summary>
        Value,

        /// <summary>
        /// The example value is assumed to be a literal string that should be displayed as-is.
        /// </summary>
        LiteralString
    }
}
