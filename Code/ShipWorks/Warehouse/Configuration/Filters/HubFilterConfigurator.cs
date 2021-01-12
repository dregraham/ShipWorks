using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Threading;
using ShipWorks.Filters;
using ShipWorks.Warehouse.Configuration.Filters.DTO;

namespace ShipWorks.Warehouse.Configuration.Filters
{
    /// <summary>
    /// Class to configure filters downloaded from the Hub
    /// </summary>
    public class HubFilterConfigurator : IHubFilterConfigurator
    {
        private readonly IFilterHelper filterHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        public HubFilterConfigurator(IFilterHelper filterHelper)
        {
            this.filterHelper = filterHelper;
        }

        /// <summary>
        /// Configure the filters downloaded from Hub
        /// </summary>
        public void Configure(IEnumerable<GetFiltersResponse> filtersResponse, IProgressReporter filterProgress)
        {
            foreach (var response in filtersResponse)
            {
                var newFilter = filterHelper.CreateFilterEntity(response.Name, response.Definition);

                if (response.Children.Any())
                {
                    newFilter.IsFolder = true;
                }

                // FilterLayoutContext.Current.AddFilter(newFilter);
            }
        }
    }
}
