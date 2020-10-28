using System.Threading.Tasks;
using ShipWorks.Installer.Api.DTO;

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
    }
}