using System.Threading.Tasks;
using ShipWorks.Warehouse.Configuration.DTO;

namespace ShipWorks.Warehouse.Configuration
{
    /// <summary>
    /// Class to configure data downloaded from the Hub
    /// </summary>
    public interface IHubConfigurator
    {
        /// <summary>
        /// Perform the given configuration
        /// </summary>
        Task Configure(HubConfiguration config);
    }
}