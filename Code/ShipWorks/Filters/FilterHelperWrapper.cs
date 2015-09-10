using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public bool EnsureFiltersUpToDate(TimeSpan timeout)
        {
            return FilterHelper.EnsureFiltersUpToDate(timeout);
        }
    }
}
