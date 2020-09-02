using System.Threading.Tasks;
using ShipWorks.Warehouse.DTO.Configuration;

namespace ShipWorks.Warehouse
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
    }
}
