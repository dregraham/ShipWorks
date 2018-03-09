using System;
using System.Data.Common;
using ShipWorks.Data.Model.EntityClasses;

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
        /// Indicates if the given object is in the filter contents of the specified filter content id
        /// </summary>
        bool IsObjectInFilterContent(long orderID, IRuleEntity rule);

        /// <summary>
        /// Regenerate all the filters
        /// </summary>
        void RegenerateFilters(DbConnection con);
    }
}
