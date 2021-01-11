using System.Threading.Tasks;
using ShipWorks.Warehouse.Configuration.DTO;

namespace ShipWorks.Warehouse.Configuration.Customer
{
    /// <summary>
    /// Configures customer information downloaded from the Hub
    /// </summary>
    public interface IHubCustomerConfigurator
    {
        /// <summary>
        /// Configure customer
        /// </summary>
        void Configure(HubConfiguration hubConfiguration);
    }
}