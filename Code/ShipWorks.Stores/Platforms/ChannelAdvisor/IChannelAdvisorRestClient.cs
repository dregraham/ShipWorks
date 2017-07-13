﻿namespace ShipWorks.Stores.Platforms.ChannelAdvisor
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
    }
}