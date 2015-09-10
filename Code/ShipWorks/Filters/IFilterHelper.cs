using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Filters
{
    /// <summary>
    /// Interface for interacting with FilterHelper
    /// </summary>
    public interface IFilterHelper
    {
        bool EnsureFiltersUpToDate(TimeSpan timeout);
    }
}
