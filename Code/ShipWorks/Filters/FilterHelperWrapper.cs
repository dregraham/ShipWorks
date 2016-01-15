using System;
using ShipWorks.Data.Model.EntityClasses;

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
        /// Indicates if the given object is in the filter contents of the specified filter content id
        /// </summary>
        public bool IsObjectInFilterContent(long orderID, ShippingProviderRuleEntity rule)
        {
            if (rule == null)
            {
                return false;
            }

            long? filterContentID = FilterHelper.GetFilterNodeContentID(rule.FilterNodeID);

            return filterContentID != null &&
                FilterHelper.IsObjectInFilterContent(orderID, filterContentID.Value);
        }
    }
}
