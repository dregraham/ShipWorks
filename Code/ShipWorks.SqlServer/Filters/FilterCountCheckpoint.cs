using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.SqlServer.Filters
{
    /// <summary>
    /// Data about a filter count checkpoint
    /// </summary>
    class FilterCountCheckpoint
    {
        /// <summary>
        /// The max FilterNodeContentyDirtyID that the update checkpoint is for.  Only dirty records of this ID 
        /// and below will be considered.
        /// </summary>
        public long MaxDirtyID { get; set; }

        /// <summary>
        /// The total number or dirty objects in the FilterNodeContentDirty table (at the time of this checkpoint)
        /// </summary>
        public int DirtyCount { get; set; }

        /// <summary>
        /// The current state of the checkpoint
        /// </summary>
        public FilterCountCheckpointState State { get; set; }

        /// <summary>
        /// The total number of milliseconds spent actively working on this checkpoint.  Does not include waiting for locks.
        /// </summary>
        public int Duration { get; set; }
    }
}
