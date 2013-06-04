﻿using System;
using System.Linq;
using Interapptive.Shared.Utility;
using System.ComponentModel.DataAnnotations;

namespace ShipWorks.Stores.Platforms.Shopify
{
    /// <summary>
    /// Constants for accessing Shopify API
    /// </summary>
    public static class ShopifyConstants
    {
        // Token param names
        public const string AccessTokenParamName = "access_token";
        public const string RequestTokenParamName = "code";

        // 250 is the max that Shopify allows 
        [Range(1, 250, ErrorMessage = "Value for OrdersPageSize must be between 1 and 250.")]
        public const int OrdersPageSize = 100;

        /// <summary>
        /// The ShipWorks Shopify API key
        /// </summary>
        internal static string InterapptiveAppApiKey
        {
            get
            {
                return SecureText.Decrypt("5Oe7I3euZxdQEOwkhApKynbcSMijT8bGaAR1jcMwJki4Qwf7efgAwQ==", "interapptive");
            }
        }

        /// <summary>
        /// The ShipWorks Shopify API Password
        /// </summary>
        internal static string InterapptiveAppApiPassword
        {
            get
            {
                return SecureText.Decrypt("RanPBr3P6fG/vm/Gqn14Q3/KMcx3UAnnn9jhzqyQ9yHmZrdPBh4aXQ==", "interapptive");
            }
        }
    }
}
