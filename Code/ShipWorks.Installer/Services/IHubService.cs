using System.Threading.Tasks;
using ShipWorks.Installer.Models;

namespace ShipWorks.Installer.Services
{
    /// <summary>
    /// Service for interacting with the Hub
    /// </summary>
    public interface IHubService
    {
        /// <summary>
        /// Login to the Hub and save the token
        /// </summary>
        Task Login(InstallSettings settings, string username, string password);
    }
}