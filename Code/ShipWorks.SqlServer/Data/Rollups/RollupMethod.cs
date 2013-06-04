using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.SqlServer.Data.Rollups
{
    /// <summary>
    /// Defines how a column should be rolled up
    /// </summary>
    public enum RollupMethod
    {
        /// <summary>
        /// If there is a single child, that value of that child is used. Otherwise null.
        /// </summary>
        SingleOrNull,

        /// <summary>
        /// If all child values are the same, that value is used.  Otherwise null.
        /// </summary>
        SameOrNull,

        /// <summary>
        /// The sum of all child values is used.
        /// </summary>
        Sum,
    }
}
