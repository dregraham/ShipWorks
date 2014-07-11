using System.Collections.Generic;

namespace ShipWorks.Data.Administration.Indexing
{
    /// <summary>
    /// An interface for hooking into the data source for inspecting 
    /// and rebuilding database table indexes.
    /// </summary>
    public interface IIndexMonitor
    {
        /// <summary>
        /// Intended to obtain a collection of indexes that need to be
        /// rebuilt based on performance history.
        /// </summary>
        /// <returns>A collection of TableIndex objects.</returns>
        IEnumerable<TableIndex> GetIndexesToRebuild();

        /// <summary>
        /// Rebuilds the given table index.
        /// </summary>
        /// <param name="index">The TableIndex to be rebuilt.</param>
        void RebuildIndex(TableIndex index);
    }
}
