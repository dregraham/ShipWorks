using System;
using System.Data.Common;
using Interapptive.Shared.Threading;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Filters
{
    /// <summary>
    /// Interface for interacting with FilterHelper
    /// </summary>
    public interface IFilterHelper
    {
        /// <summary>
        /// Indicates if the given object is in the filter contents of the specified filter content id
        /// </summary>
        bool IsObjectInFilterContent(long orderID, IRuleEntity rule);

        /// <summary>
        /// Regenerate all the filters
        /// </summary>
        void RegenerateFilters(DbConnection con);

        /// <summary>
        /// Calculate initial filter counts
        /// </summary>
        void CalculateInitialFilterCounts(DbConnection connection, IProgressReporter progressFilterCounts, int initialPercentComplete);
    }
}