using System;
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
        bool IsObjectInFilterContent(long orderID, ShippingProviderRuleEntity rule);
    }
}
