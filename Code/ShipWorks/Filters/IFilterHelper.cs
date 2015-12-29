using System;

namespace ShipWorks.Filters
{
    /// <summary>
    /// Interface for interacting with FilterHelper
    /// </summary>
    public interface IFilterHelper
    {
        bool EnsureFiltersUpToDate(TimeSpan timeout);
        long? GetFilterNodeContentID(long filterNodeID);
        bool IsObjectInFilterContent(long orderID, long value);
    }
}
