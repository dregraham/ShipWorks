using System.Collections.Generic;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Platforms.ChannelAdvisor.DTO;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    /// <summary>
    /// ChannelAdvisor Rest Web Client
    /// </summary>
    public interface IChannelAdvisorRestClient
    {
        /// <summary>
        /// Given a Channel Advisor Authorization Code and RedirectUrl, request and return the refresh token
        /// </summary>
        GenericResult<string> GetRefreshToken(string code, string redirectUrl);

        /// <summary>
        /// GetOrders from the given start time
        /// </summary>
        ChannelAdvisorOrderResult GetOrders(int daysBack, string refreshToken, bool includeExternallyManagedDistributionCenters);

        /// <summary>
        /// GetOrders from the given next token
        /// </summary>
        ChannelAdvisorOrderResult GetOrders(string nextToken, string refreshToken);

        /// <summary>
        /// Get the profile
        /// </summary>
        /// <remarks>
        /// Expanding sites throws errors for some customers. If setting expandSites to true, make sure to 
        /// catch any error that comes of it.
        /// </remarks>
        ChannelAdvisorProfilesResponse GetProfiles(string refreshToken, bool expandSites);

        /// <summary>
        /// Gets the distribution centers.
        /// </summary>
        ChannelAdvisorDistributionCenterResponse GetDistributionCenters(string refreshToken);

        /// <summary>
        /// Gets the next batch of distribution centers.
        /// </summary>
        ChannelAdvisorDistributionCenterResponse GetDistributionCenters(string nextToken, string refreshToken);

        /// <summary>
        /// Fetches the given products and adds them to the cache if they aren't already in it
        /// </summary>
        void AddProductsToCache(IEnumerable<int> productIds, string refreshToken);

        /// <summary>
        /// Gets the product.
        /// </summary>
        ChannelAdvisorProduct GetProduct(int productID, string refreshToken);

        /// <summary>
        /// Uploads the shipment details.
        /// </summary>
        void UploadShipmentDetails(ChannelAdvisorShipment channelAdvisorShipment, string refreshToken, string channelAdvisorOrderID);

        /// <summary>
        /// Gets next batch of items
        /// </summary>
        ChannelAdvisorOrderItemsResult GetOrderItems(string nextToken, string refreshToken);

        /// <summary>
        /// Get the base endpoint for ChannelAdvisor requests
        /// </summary>
        string GetEndpointBase();
    }
}