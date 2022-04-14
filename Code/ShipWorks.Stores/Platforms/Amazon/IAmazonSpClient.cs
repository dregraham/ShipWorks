using System.Threading.Tasks;

namespace ShipWorks.Stores.Platforms.Amazon
{
    /// <summary>
    /// Client to communicate with SP (Via Platform)
    /// </summary>
    public interface IAmazonSpClient
    {
        /// <summary>
        /// Get the URL to initiate Monoauth
        /// </summary>
        Task<string> GetMonauthInitiateUrl();
    }
}