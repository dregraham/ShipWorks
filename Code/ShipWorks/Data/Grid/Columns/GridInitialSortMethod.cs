using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Grid.Columns
{
    /// <summary>
    /// Controls how a grid will be sorted each time it is first loaded.
    /// </summary>
    public enum GridInitialSortMethod
    {
        /// <summary>
        /// Sort using the configured default sort.
        /// </summary>
        DefaultSort = 0,

        /// <summary>
        /// Sort however the user had it last.
        /// </summary>
        LastSort = 1
    }
}
