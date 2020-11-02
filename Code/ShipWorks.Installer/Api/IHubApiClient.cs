using System.Threading.Tasks;
using ShipWorks.Installer.Api.DTO;
using ShipWorks.Installer.Models;

namespace ShipWorks.Installer.Api
{
    /// <summary>
    /// Client for communicating with the Hub API
    /// </summary>
    public interface IHubApiClient
    {
        /// <summary>
        /// Login to Hub with a username and password
        /// </summary>
        Task<TokenResponse> Login(string username, string password);

        /// <summary>
        /// Get list of warehouses
        /// </summary>
        Task<WarehouseList> GetWarehouseList(HubToken token);
    }
}