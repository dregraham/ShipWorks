using System;
using ShipWorks.Data.Model.EntityClasses;
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
        string GetRefreshToken(string code, string redirectUrl);

        /// <summary>
        /// Get an access token from the refresh token
        /// </summary>
        string GetAccessToken(string refreshToken);

        /// <summary>
        /// GetOrders from the given start time
        /// </summary>
        ChannelAdvisorOrderResult GetOrders(DateTime start, string accessToken);

        /// <summary>
        /// Get the profile
        /// </summary>
        /// <param name="accessToken"></param>
        ChannelAdvisorProfilesResponse GetProfiles(string accessToken);

        /// <summary>
        /// Gets the product.
        /// </summary>
        ChannelAdvisorProduct GetProduct(int productID, string accessToken);
    }
}