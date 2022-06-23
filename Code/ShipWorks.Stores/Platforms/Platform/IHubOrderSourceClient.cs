﻿using System.Threading.Tasks;

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
        Task<string> GetCreateOrderSourceInitiateUrl(string orderSourceName, string apiRegion, int? daysBack);

        /// <summary>
        /// Get the Monoauth URL to initiate an order source credential change
        /// </summary>
        /// <remarks>
        /// Note that the orderSourceName will be used in both the URL used to communicate with the hub and the
        /// redirectUrl the hub will send on to monoauth
        /// </remarks>
        Task<string> GetUpdateOrderSourceInitiateUrl(string orderSourceName, string orderSourceId, string apiRegion, string sellerId);

        /// <summary>
        /// Call Hub to get a Platform Amazon carrier Id for Buy Shipping
        /// </summary>
        /// <returns></returns>
        Task<string> GetPlatformAmazonCarrierId(string uniqueIdentifier);

        /// <summary>
        /// Get the monoauth url to initiate creating a Platform Carrier
        /// </summary>
        Task<string> GetCreateCarrierInitiateUrl(string orderSourceName, string apiRegion);

        /// <summary>
        /// Get the monoauth url to update a platform carrier
        /// </summary>
        Task<string> GetUpdateCarrierInitiateUrl(string orderSourceName, string carrierId, string apiRegion, string sellerId);
    }
}