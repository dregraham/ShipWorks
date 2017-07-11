using System;

namespace ShipWorks.Stores.Communication
{
    /// <summary>
    /// ChannelAdvisor Rest Web Client
    /// </summary>
    public interface IChannelAdvisorRestClient
    {
        /// <summary>
        /// Given a Channel Advisor Authorization Code, request and return the refresh token
        /// </summary>
        string GetRefreshToken(string code);

        /// <summary>
        /// Gets the authorize URL to send to a browser window for the user to Authorize ShipWorks
        /// </summary>
        Uri AuthorizeUrl { get; }
    }
}