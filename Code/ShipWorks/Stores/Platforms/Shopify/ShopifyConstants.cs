using System.ComponentModel.DataAnnotations;
using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Stores.Platforms.Shopify
{
    /// <summary>
    /// Constants for accessing Shopify API
    /// </summary>
    public static class ShopifyConstants
    {
        private const string ShopifyOrdersPerPageKeyName = "ShopifyOrdersPerPage";

        // Token param names
        public const string AccessTokenParamName = "access_token";
        public const string RequestTokenParamName = "code";
        public const string CallbackUrl = "http://www.shipworks.com/shopify/?r=sfy";

        // 250 is the max that Shopify allows
        [Range(1, 250, ErrorMessage = "Value for OrdersPageSize must be between 1 and 250.")]
        private const int OrdersPageSize = 250;

        // This is the error code that is returned in the request if the last api call exceeds the allowed amount.
        public const int OverApiLimitStatusCode = 429;

        /// <summary>
        /// The ShipWorks Shopify API key
        /// </summary>
        internal static string InterapptiveAppApiKey => 
            SecureText.Decrypt("5Oe7I3euZxdQEOwkhApKynbcSMijT8bGaAR1jcMwJki4Qwf7efgAwQ==", "interapptive");

        /// <summary>
        /// The ShipWorks Shopify API Password
        /// </summary>
        internal static string InterapptiveAppApiPassword => 
            SecureText.Decrypt("RanPBr3P6fG/vm/Gqn14Q3/KMcx3UAnnn9jhzqyQ9yHmZrdPBh4aXQ==", "interapptive");

        /// <summary>
        /// Gets the number of orders per page to return
        /// </summary>
        public static int ShopifyOrdersPerPage => 
            InterapptiveOnly.Registry.GetValue(ShopifyOrdersPerPageKeyName, OrdersPageSize);
    }
}
