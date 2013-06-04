using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Grid.Columns.DisplayTypes
{
    /// <summary>
    /// Controls how an enum column is sorted in the grid
    /// </summary>
    public enum EnumSortMethod
    {
        /// <summary>
        /// The column is sorted by the Description value applied to the enum value.  This is slightly slower due to the CASE statement for sorting
        /// that is generated.
        /// </summary>
        Description,

        /// <summary>
        /// The column is sorted by the raw enum value.  This is also the fastest method.
        /// </summary>
        Value
    }
}
