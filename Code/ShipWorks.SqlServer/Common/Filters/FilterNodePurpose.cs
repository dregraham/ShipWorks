using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Filters
{
    /// <summary>
    /// The reason a filter node exists
    /// </summary>
    public enum FilterNodePurpose
    {
        /// <summary>
        /// A standard filter created by a user and displayed in the filter tree
        /// </summary>
        Standard = 0,

        /// <summary>
        /// A filter created internally by ShipWorks to materialize search results
        /// </summary>
        Search = 1,

        /// <summary>
        /// A filter created that is not apart of the standard filter tree
        /// </summary>
        Quick = 2
    }
}
