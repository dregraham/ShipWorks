using System;
using System.Linq;

namespace ShipWorks.Stores.Platforms.Shopify
{
    /// <summary>
    /// Class with api url definitions and helper methods/properties for populating urls
    /// </summary>
    public class ShopifyEndpoints
    {
        private const string ApiAuthorizeUrlFormat = "{0}oauth/authorize?client_id={1}&scope={2}&redirect_uri={3}";
        private const string ApiAccessTokenUrlFormat = "{0}oauth/access_token?client_id={1}&client_secret={2}&code={3}";

        // Order URL formats
        private const string ApiGetOrdersUrlFormat = "{0}orders.json";
        private const string ApiGetOrderCountUrlFormat = "{0}orders/count.json";
        private const string ViewOrderUrlFormat = "{0}orders/{1}";

        // Product URL formats
        private const string ApiGetProductFormat = "{0}products/{1}.json";
        private const string ViewProductFormat = "{0}products/{1}";

        // Shipment URL formats
        private const string ApiFulfillmentsUrlFormat = "{0}orders/{1}/fulfillments.json";

        // Shop Url Format
        private const string ShopUrlFormat = "{0}shop.json";

        // Scopes we need
        private const string Scopes = "write_customers,write_orders,write_products,write_shipping";

        // The shop name for which to generate endpoints for
        string shopUrlName;

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="shopifyShopName">The name of the shopify shop, based on the shop's url.  It's the section after http:// and before myshopify.com</param>
        public ShopifyEndpoints(string shopUrlName)
        {
            if (string.IsNullOrWhiteSpace(shopUrlName))
            {
                throw new ArgumentException("shopName must be specified", "shopUrlName");
            }

            this.shopUrlName = shopUrlName;

            // See if we can make a valid uri out of it.  This will throw if it's not valid
            Uri uri = new Uri(ApiBaseUrl);
        }

        /// <summary>
        /// The base url for calling the Shopify API for a specific shop name
        /// </summary>
        private string ApiBaseUrl
        {
            get
            {
                return string.Format("https://{0}.myshopify.com/admin/", shopUrlName);
            }
        }

        /// <summary>
        /// The url to get shop information
        /// </summary>
        public string ShopUrl
        {
            get
            {
                return string.Format(ShopUrlFormat, ApiBaseUrl);
            }
        }

        /// <summary>
        /// The url to which the user is sent for granting ShipWorks access
        /// </summary>
        public Uri GetApiAuthorizeUrl()
        {
            return new Uri(string.Format(ApiAuthorizeUrlFormat, ApiBaseUrl, ShopifyConstants.InterapptiveAppApiKey, Scopes,
                ShopifyConstants.CallbackUrl));
        }

        /// <summary>
        /// The url used to ask Shopify for an AccessToken
        /// </summary>
        public string GetApiAccessTokenUrl(string requestToken)
        {
            return string.Format(ApiAccessTokenUrlFormat, ApiBaseUrl, ShopifyConstants.InterapptiveAppApiKey, ShopifyConstants.InterapptiveAppApiPassword, requestToken);
        }

        /// <summary>
        /// URL used to force a user to be logged out if they have cookies saved to keep them logged in.
        /// </summary>
        public string ApiLogoutUrl
        {
            get { return string.Format(ApiBaseUrl + "auth/logout", shopUrlName); }
        }

        /// <summary>
        /// Gets the browser bypass URL.
        /// </summary>
        public string BrowserBypassUrl
        {
            get { return string.Format(ApiBaseUrl + "/unsupported_browser_bypass", shopUrlName); }
        }

        /// <summary>
        /// The base url used to request orders
        /// </summary>
        public string ApiGetOrdersUrl
        {
            get
            {
                return string.Format(ApiGetOrdersUrlFormat, ApiBaseUrl);
            }
        }

        /// <summary>
        /// The base url used to request an order count
        /// </summary>
        public string ApiGetOrderCountUrl
        {
            get
            {
                return string.Format(ApiGetOrderCountUrlFormat, ApiBaseUrl);
            }
        }

        /// <summary>
        /// The url used for displaying an order in a browser.  Used for linking from the grid/etc...
        /// </summary>
        /// <param name="shopifyOrderId"></param>
        /// <returns>Url to shopify order</returns>
        public string ViewOrderUrl(long shopifyOrderId)
        {
            return string.Format(ViewOrderUrlFormat, ApiBaseUrl, shopifyOrderId);
        }

        /// <summary>
        /// The url used for displaying a product in a browser.  Used for linking from the grid/etc...
        /// </summary>
        /// <param name="shopifyProductId"></param>
        /// <returns></returns>
        public string ViewProductUrl(long shopifyProductId)
        {
            return string.Format(ViewProductFormat, ApiBaseUrl, shopifyProductId);
        }

        /// <summary>
        /// The url to retrieve all shopify fulfillments for a shopify order
        /// </summary>
        /// <param name="shopifyOrderId">The shopify order id</param>
        /// <returns>The url to retrieve all shopify fulfillments for a shopify order</returns>
        public string ApiFulfillmentsUrl(long shopifyOrderId)
        {
            return string.Format(ApiFulfillmentsUrlFormat, ApiBaseUrl, shopifyOrderId);
        }

        /// <summary>
        /// The url to a shopify Product Id.  Used to get product information such as image urls.
        /// </summary>
        /// <param name="shopifyProductId">The shopify product Id</param>
        /// <returns>The url to a shopify Product</returns>
        public string ApiProductUrl(long shopifyProductId)
        {
            return string.Format(ApiGetProductFormat, ApiBaseUrl, shopifyProductId);
        }
    }
}
