using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Grid.Paging;

namespace ShipWorks.Data.Grid
{
    /// <summary>
    /// Defines the interface for the object the grid exposes its selection through
    /// </summary>
    public interface IGridSelection
    {
        /// <summary>
        /// Counter that increaseses anytime the selection or its ordering changes
        /// </summary>
        int Version { get; } 

        /// <summary>
        /// The selected element count
        /// </summary>
        int Count { get; }

        /// <summary>
        /// The selected entity keys.  This does not necessarily have to be in sort order
        /// </summary>
        IEnumerable<long> Keys { get; }

        /// <summary>
        /// The selected entity keys in grid sort order
        /// </summary>
        IEnumerable<long> OrderedKeys { get; }
    }
}
