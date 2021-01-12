using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Warehouse.Configuration.DTO;
using ShipWorks.Warehouse.Configuration.Filters.DTO;

namespace ShipWorks.Warehouse.Configuration
{
    /// <summary>
    /// Web client for downloading configuration settings from the Hub
    /// </summary>
    public interface IHubConfigurationWebClient
    {
        /// <summary>
        /// Get the configuration from Hub
        /// </summary>
        Task<HubConfiguration> GetConfig(string warehouseID);

        /// <summary>
        /// Get list of filters from Hub
        /// </summary>
        Task<List<GetFiltersResponse>> GetFilters();
    }
}
