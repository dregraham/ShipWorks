using System;
using System.Data.Common;
using Interapptive.Shared.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Filters
{
    /// <summary>
    /// Implementation of IFilterHelper to be able to DI and Mock classes that use FilterHelper
    /// </summary>
    public class FilterHelperWrapper : IFilterHelper
    {
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public FilterHelperWrapper(ISqlAdapterFactory sqlAdapterFactory)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
        }

        /// <summary>
        /// Ensure filters are up to date
        /// </summary>
        public bool EnsureFiltersUpToDate(TimeSpan timeout) =>
            FilterHelper.EnsureFiltersUpToDate(timeout);

        /// <summary>
        /// Indicates if the given object is in the filter contents of the specified filter content id
        /// </summary>
        public bool IsObjectInFilterContent(long orderID, IRuleEntity rule)
        {
            if (rule == null)
            {
                return false;
            }

            long? filterContentID = FilterHelper.GetFilterNodeContentID(rule.FilterNodeID);

            return filterContentID != null &&
                FilterHelper.IsObjectInFilterContent(orderID, filterContentID.Value);
        }

        /// <summary>
        /// Regenerate all the filters
        /// </summary>
        public void RegenerateFilters(DbConnection con)
        {
            try
            {
                // We need to push a new scope for the layout context
                FilterLayoutContext.PushScope();

                // Regenerate the filters
                using (var sqlAdapter = sqlAdapterFactory.Create(con))
                {
                    FilterLayoutContext.Current.RegenerateAllFilters(sqlAdapter);
                }

                // We can wipe any dirties and any current checkpoint - they don't matter since we have regenerated all filters anyway
                SqlUtility.TruncateTable("FilterNodeContentDirty", con);
                SqlUtility.TruncateTable("FilterNodeUpdateCheckpoint", con);
            }
            finally
            {
                FilterLayoutContext.PopScope();
            }
        }
    }
}
