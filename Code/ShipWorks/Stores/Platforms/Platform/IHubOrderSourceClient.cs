using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShipWorks.Stores.Platforms.Platform
{
    /// <summary>
    /// Client to communicate with SP (Via Platform)
    /// </summary>
    public interface IHubOrderSourceClient
    {
        /// <summary>
        /// Get the monoauth URL to initiate an order source creation
        /// </summary>
        /// <remarks>
        /// Note that the orderSourceName will be used in both the URL used to communicate with the hub and the
        /// redirectUrl the hub will send on to monoauth
        /// </remarks>
        Task<string> GetCreateOrderSourceInitiateUrl(string orderSourceName, int? daysBack, Dictionary<string, string> otherParameters = null);

		/// <summary>
		/// Get the Monoauth URL to initiate an order source credential change for non Amazon
		/// </summary>
		/// <remarks>
		/// Note that the orderSourceName will be used in both the URL used to communicate with the hub and the
		/// redirectUrl the hub will send on to monoauth
		/// </remarks>
		Task<string> GetUpdateOrderSourceInitiateUrl(string orderSourceName, string orderSourceId, Dictionary<string, string> otherParameters = null);

		/// <summary>
		/// Call Hub to get a Platform Amazon carrier Id for Buy Shipping
		/// </summary>
		/// <returns></returns>
		Task<string> GetPlatformAmazonCarrierId(string uniqueIdentifier);

        /// <summary>
        /// Call Hub to get a Platform Amazon carrier Id for Buy Shipping from a MWS store
        /// </summary>
        /// <returns></returns>
        Task<string> CreateAmazonCarrierFromMws(string sellingPartnerId, string mwsAuthToken, string countryCode);

        /// <summary>
        /// Get the monoauth url to initiate creating a Platform Carrier
        /// </summary>
        Task<string> GetCreateCarrierInitiateUrl(string orderSourceName, string apiRegion);

        /// <summary>
        /// Get the monoauth url to update a platform carrier
        /// </summary>
        Task<string> GetUpdateCarrierInitiateUrl(string orderSourceName, string carrierId, string apiRegion, string sellerId);

        /// <summary>
        /// Call hub to update the amazon sp FBA criteria
        /// </summary>
        Task UpdateAmazonFbaCriteria(string orderSourceId, bool downloadFba, string apiRegion);
    }
}