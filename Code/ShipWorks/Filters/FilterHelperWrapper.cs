using System;

namespace ShipWorks.Filters
{
    /// <summary>
    /// Implementation of IFilterHelper to be able to DI and Mock classes that use FilterHelper
    /// </summary>
    public class FilterHelperWrapper : IFilterHelper
    {
        /// <summary>
        /// Ensure filters are up to date
        /// </summary>
        public bool EnsureFiltersUpToDate(TimeSpan timeout) =>
            FilterHelper.EnsureFiltersUpToDate(timeout);

        /// <summary>
        /// Get the FilterNodeContentID for the given node
        /// </summary>
        public long? GetFilterNodeContentID(long filterNodeID) =>
            FilterHelper.GetFilterNodeContentID(filterNodeID);

        /// <summary>
        /// Indicates if the given object is in the filter contents of the specified filter content id
        /// </summary>
        public bool IsObjectInFilterContent(long orderID, long value) =>
            FilterHelper.IsObjectInFilterContent(orderID, value);
    }
}
