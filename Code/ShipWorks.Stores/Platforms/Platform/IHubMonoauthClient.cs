using System.Threading.Tasks;

namespace ShipWorks.Stores.Platforms.Platform
{
    /// <summary>
    /// Client to communicate with SP (Via Platform)
    /// </summary>
    public interface IHubMonoauthClient
    {
        /// <summary>
        /// Get the URL to initiate Monoauth
        /// </summary>
        Task<string> GetMonauthInitiateUrl(string orderSourceName);
    }
}