using System;
using System.Collections.Generic;
using System.Text;

namespace ShipWorks.Data.Grid.Paging
{
    /// <summary>
    /// The states that a grid row in ShipWorks can be in
    /// </summary>
    public enum PagedDataState
    {
        /// <summary>
        /// Initial state. Nothing has happened yet.
        /// </summary>
        None,

        /// <summary>
        /// Row had been loaded, but then was reset and cleared
        /// </summary>
        Reset,

        /// <summary>
        /// All data has been loaded into the grid row.
        /// </summary>
        Loaded,

        /// <summary>
        /// The grid is actively trying to load data for the row
        /// </summary>
        Loading,

        /// <summary>
        /// The grid knows the row needs data, but its put it off for the time being due to volume.
        /// </summary>
        LoadDeferred,

        /// <summary>
        /// The object representing the grid row has been deleted or removed from the filter, but the row
        /// has not yet gone away.
        /// </summary>
        Removing
    }
}
