﻿using System.Security;

namespace ShipWorks.Stores.Platforms.ShopSite.AccountSettings
{
    /// <summary>
    /// Basic interface for the ShopSite account settings view model
    /// </summary>
    public interface IShopSiteAccountSettingsViewModel
    {
        /// <summary>
        /// Username for legacy API access
        /// </summary>
        string LegacyMerchantID { get; set; }

        /// <summary>
        /// Password for legacy API access
        /// </summary>
        SecureString LegacyPassword { get; set; }

        /// <summary>
        /// Username for OAuth access
        /// </summary>
        string OAuthClientID { get; set; }

        /// <summary>
        /// Password for OAuth access
        /// </summary>
        string OAuthSecretKey { get; set; }

        /// <summary>
        /// Authorization Code for OAuth access
        /// </summary>
        string OAuthAuthorizationCode { get; set; }

        /// <summary>
        /// Api URL for OAuth access
        /// </summary>
        string ApiUrl { get; set; }

        /// <summary>
        /// Using unsecure connections for legacy accounts
        /// </summary>
        bool LegacyUseUnsecureHttp { get; set; }
    }
}
