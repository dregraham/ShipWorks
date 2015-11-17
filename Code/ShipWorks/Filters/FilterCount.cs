using System;
using System.Collections.Generic;
using System.Text;
using Interapptive.Shared;

namespace ShipWorks.Filters
{
    /// <summary>
    /// Represents the calculated count of a filter node
    /// </summary>
    public class FilterCount
    {
        long nodeID;
        long countID;
        FilterNodePurpose purpose;
        FilterCountStatus status;
        int count;
        long countVersion;
        long rowVersion;
        int cost;

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreTooManyParams]
        public FilterCount(long nodeID, long countID, FilterNodePurpose purpose, FilterCountStatus status, int count, long countVersion, long rowVersion, int cost)
        {
            this.nodeID = nodeID;
            this.countID = countID;
            this.purpose = purpose;
            this.status = status;
            this.count = count;
            this.countVersion = countVersion;
            this.rowVersion = rowVersion;
            this.cost = cost;
        }

        /// <summary>
        /// ToString
        /// </summary>
        public override string ToString()
        {
            return string.Format("NodeID{0},CountID{1}:Count{2}", nodeID, countID, count);
        }

        /// <summary>
        /// The FilterNodeID that this count is associated with
        /// </summary>
        public long FilterNodeID
        {
            get { return nodeID; }
        }

        /// <summary>
        /// The FilterNodeContentID of the row that contains the count
        /// </summary>
        public long FilterNodeContentID
        {
            get { return countID; }
        }

        /// <summary>
        /// The purpose of the FilterNode as specified when it was created.
        /// </summary>
        public FilterNodePurpose Purpose
        {
            get { return purpose; }
        }

        /// <summary>
        /// The status of the count
        /// </summary>
        public FilterCountStatus Status
        {
            get { return status; }
        }

        /// <summary>
        /// The count.  Only valid if Status is "Ready"
        /// </summary>
        public int Count
        {
            get { return count; }
        }

        /// <summary>
        /// The version of the count.  Even though RowVersion will change for any update, even if the update has no effective change,
        /// CountVersion changes if and only if the count, or any contents of the filter data, changes.
        /// </summary>
        public long CountVersion
        {
            get { return countVersion; }
        }

        /// <summary>
        /// The database timestamp of this version of the count
        /// </summary>
        public long RowVersion
        {
            get { return rowVersion; }
        }

        /// <summary>
        /// The cost (in milliseconds) it took to calculate the filter count.
        /// </summary>
        public int CostInMilliseconds
        {
            get { return cost; }
        }
    }
}
