using System.Threading.Tasks;

namespace ShipWorks.Stores.Platforms.Platform
{
    /// <summary>
    /// Client to communicate with SP (Via Platform)
    /// </summary>
    public interface IHubMonoauthClient
    {
        /// <summary>
        /// Get the monoauth URL to initiate an order source creation
        /// </summary>
        /// <remarks>
        /// Note that the orderSourceName will be used in both the URL used to communicate with the hub and the
        /// redirectUrl the hub will send on to monoauth
        /// </remarks>
        Task<string> GetCreateOrderSourceInitiateUrl(string orderSourceName);

        /// <summary>
        /// Get the Monoauth URL to initiate an order source credential change
        /// </summary>
        /// <remarks>
        /// Note that the orderSourceName will be used in both the URL used to communicate with the hub and the
        /// redirectUrl the hub will send on to monoauth
        /// </remarks>
        Task<string> GetUpdateOrderSourceInitiateUrl(string orderSourceName, string orderSourceId);
    }
}