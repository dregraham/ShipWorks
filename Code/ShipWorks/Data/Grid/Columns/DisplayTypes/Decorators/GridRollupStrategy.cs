using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Grid.Columns.DisplayTypes.Decorators
{
    /// <summary>
    /// Specifies what strategy is used to rollup the value of a particular column
    /// </summary>
    public enum GridRollupStrategy
    {
        /// <summary>
        /// If there is a single child row, its value is used.  If there are multiple child rows, null is used.
        /// </summary>
        SingleChildOrNull,

        /// <summary>
        /// If all child rows have the same value that value is uses, otherwise null is used.
        /// </summary>
        SameValueOrNull
    }
}
