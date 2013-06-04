using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Filters.Grid
{
    /// <summary>
    /// How to sort the grid when a filter is initially selected
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum FilterInitialSortType
    {
        [Description("Whatever sort I'm already using")]
        CurrentSort = 0,

        [Description("The sort that was last used for the filter")]
        LastActiveSort = 1,

        [Description("The default sort of the filter")]
        DefaultSort = 2
    }
}
