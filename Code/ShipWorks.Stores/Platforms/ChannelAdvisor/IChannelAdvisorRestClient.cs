using System;
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
        ChannelAdvisorOrderResult GetOrders(DateTime start, string refreshToken);

        /// <summary>
        /// GetOrders from the given next token
        /// </summary>
        ChannelAdvisorOrderResult GetOrders(string nextToken, string refreshToken);

        /// <summary>
        /// Get the profile
        /// </summary>
        ChannelAdvisorProfilesResponse GetProfiles(string refreshToken);

        /// <summary>
        /// Gets the distribution centers.
        /// </summary>
        ChannelAdvisorDistributionCenterResponse GetDistributionCenters(string refreshToken);

        /// <summary>
        /// Gets the product.
        /// </summary>
        ChannelAdvisorProduct GetProduct(int productID, string refreshToken);

        /// <summary>
        /// Uploads the shipment details.
        /// </summary>
        void UploadShipmentDetails(ChannelAdvisorShipment channelAdvisorShipment, string refreshToken, string channelAdvisorOrderID);
    }
}