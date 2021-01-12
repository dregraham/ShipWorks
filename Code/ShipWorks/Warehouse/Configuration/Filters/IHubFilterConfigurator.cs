using System.Collections.Generic;
using Interapptive.Shared.Threading;
using ShipWorks.Warehouse.Configuration.Filters.DTO;

namespace ShipWorks.Warehouse.Configuration.Filters
{
    /// <summary>
    /// Class to configure filters downloaded from the Hub
    /// </summary>
    public interface IHubFilterConfigurator
    {
        /// <summary>
        /// Configure the filters downloaded from Hub
        /// </summary>
        void Configure(IEnumerable<GetFiltersResponse> filtersResponse, IProgressReporter filterProgress);
    }
}
