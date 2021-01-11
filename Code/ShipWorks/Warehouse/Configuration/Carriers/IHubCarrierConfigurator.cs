using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Warehouse.Configuration.Carriers.DTO;

namespace ShipWorks.Warehouse.Configuration.Carriers
{
    /// <summary>
    /// Configures carriers downloaded from the Hub
    /// </summary>
    public interface IHubCarrierConfigurator
    {
        /// <summary>
        /// Configure carriers
        /// </summary>
        Task Configure(IEnumerable<CarrierConfiguration> carrierConfigurations);
    }
}
