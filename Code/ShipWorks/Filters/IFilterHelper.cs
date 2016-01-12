using System;

namespace ShipWorks.Filters
{
    /// <summary>
    /// Interface for interacting with FilterHelper
    /// </summary>
    public interface IFilterHelper
    {
        /// <summary>
        /// Ensure filters are up to date
        /// </summary>
        bool EnsureFiltersUpToDate(TimeSpan timeout);

        /// <summary>
        /// Get the FilterNodeContentID for the given node
        /// </summary>
        long? GetFilterNodeContentID(long filterNodeID);

        /// <summary>
        /// Indicates if the given object is in the filter contents of the specified filter content id
        /// </summary>
        bool IsObjectInFilterContent(long orderID, long value);
    }
}
