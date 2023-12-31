﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Stores.Platforms.Shopify
{
    /// <summary>
    /// Class with API URL definitions and helper methods/properties for populating URLs
    /// </summary>
    public class ShopifyEndpoints
    {
        private InterapptiveOnlyWrapper interapptiveOnly;

        private const string ApiAuthorizeUrlFormat = "{0}oauth/authorize?client_id={1}&scope={2}&redirect_uri={3}";
        private const string ApiAccessTokenUrlFormat = "{0}oauth/access_token?client_id={1}&client_secret={2}&code={3}";

        // Order URL formats
        private const string ApiGetOrdersUrlFormat = "{0}api/2021-04/orders.json";
        private const string ApiGetOrderUrlFormat = "{0}orders/{1}.json";
        private const string ApiGetOrderCountUrlFormat = "{0}orders/count.json";
        private const string ViewOrderUrlFormat = "{0}orders/{1}";

        // Product URL formats
        private const string ApiGetProductFormat = "{0}products/{1}.json";
        private const string ViewProductFormat = "{0}products/{1}";

        // Fraud URL formats
        private const string ApiFraudUrlFormat = "{0}orders/{1}/risks.json";

        // Shipment URL formats
        private const string ApiFulfillmentsUrlFormat = "{0}api/2021-04/orders/{1}/fulfillments.json";

        // Shop URL Format
        private const string ShopUrlFormat = "{0}shop.json";

        // InventoryLevel URL Format
        private const string InventoryLevelForItemsUrlFormat = "{0}api/2021-04/inventory_levels.json?inventory_item_ids={1}";
        private const string InventoryLevelForLocationsUrlFormat = "{0}api/2021-04/inventory_levels.json?location_ids={1}";
        private const string LocationsUrlFormat = "{0}api/2021-04/locations.json";

        // Scopes we need
        private const string Scopes = "write_customers,write_orders,write_products,write_shipping,read_inventory";

        // The shop name for which to generate endpoints for
        string shopUrlName;

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="shopUrlName">The name of the shopify shop, based on the shop's url.  It's the section after http:// and before myshopify.com</param>
        [SuppressMessage("ShipWorks", "SW0002:Identifier should not be obfuscated",
            Justification = "Identifier is not being used for data binding")]
        public ShopifyEndpoints(string shopUrlName)
        {
            if (string.IsNullOrWhiteSpace(shopUrlName))
            {
                throw new ArgumentException("shopName must be specified", nameof(shopUrlName));
            }

            this.shopUrlName = shopUrlName;

            interapptiveOnly = new InterapptiveOnlyWrapper();

            // See if we can make a valid URI out of it.  This will throw if it's not valid
            Uri uri = new Uri(ApiBaseUrl);
        }

        /// <summary>
        /// The base URL for calling the Shopify API for a specific shop name
        /// </summary>
        private string ApiBaseUrl
        {
            get
            {
                if (UseFakeApi)
                {
                    var endpointOverride = interapptiveOnly.Registry.GetValue("ShopifyEndpoint", string.Empty);
                    if (!string.IsNullOrWhiteSpace(endpointOverride))
                    {
                        return endpointOverride;
                    }
                }

                return string.Format("https://{0}.myshopify.com/admin/", shopUrlName);
            }
        }

        /// <summary>
        /// The URL to get shop information
        /// </summary>
        public string ShopUrl
        {
            get
            {
                return string.Format(ShopUrlFormat, ApiBaseUrl);
            }
        }

        /// <summary>
        /// The URL to get item inventory level info
        /// </summary>
        public string InventoryLevelForItemsUrl(IEnumerable<long> itemInventoryIDs) =>
            string.Format(InventoryLevelForItemsUrlFormat, ApiBaseUrl, String.Join(",", itemInventoryIDs));

        /// <summary>
        /// The URL to get location inventory level info
        /// </summary>
        public string InventoryLevelForLocationsUrl(IEnumerable<long> locationIDs) =>
            string.Format(InventoryLevelForLocationsUrlFormat, ApiBaseUrl, String.Join(",", locationIDs));

        /// <summary>
        /// Url of the Locations api
        /// </summary>
        public string LocationsUrl =>
            string.Format(LocationsUrlFormat, ApiBaseUrl);

        /// <summary>
        /// The URL to which the user is sent for granting ShipWorks access
        /// </summary>
        public Uri GetApiAuthorizeUrl()
        {
            return new Uri(string.Format(ApiAuthorizeUrlFormat, ApiBaseUrl, ShopifyConstants.InterapptiveAppApiKey, Scopes,
                ShopifyConstants.CallbackUrl));
        }

        /// <summary>
        /// The URL used to ask Shopify for an AccessToken
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
        /// The base URL used to request orders
        /// </summary>
        public string ApiGetOrderUrl(long shopifyOrderID) =>
            string.Format(ApiGetOrderUrlFormat, ApiBaseUrl, shopifyOrderID);

        /// <summary>
        /// The base URL used to request orders
        /// </summary>
        public string ApiGetOrdersUrl
        {
            get
            {
                return string.Format(ApiGetOrdersUrlFormat, ApiBaseUrl);
            }
        }

        /// <summary>
        /// The base URL used to request an order count
        /// </summary>
        public string ApiGetOrderCountUrl
        {
            get
            {
                return string.Format(ApiGetOrderCountUrlFormat, ApiBaseUrl);
            }
        }

        /// <summary>
        /// The URL used for displaying an order in a browser.  Used for linking from the grid/etc...
        /// </summary>
        /// <param name="shopifyOrderId"></param>
        /// <returns>URL to shopify order</returns>
        public string ViewOrderUrl(long shopifyOrderId)
        {
            return string.Format(ViewOrderUrlFormat, ApiBaseUrl, shopifyOrderId);
        }

        /// <summary>
        /// The URL used for displaying a product in a browser.  Used for linking from the grid/etc...
        /// </summary>
        /// <param name="shopifyProductId"></param>
        /// <returns></returns>
        public string ViewProductUrl(long shopifyProductId)
        {
            return string.Format(ViewProductFormat, ApiBaseUrl, shopifyProductId);
        }

        /// <summary>
        /// The URL to retrieve all shopify fraud risks for a shopify order
        /// </summary>
        /// <param name="shopifyOrderId">The shopify order id</param>
        /// <returns>The URL to retrieve all shopify fraud risks for a shopify order</returns>
        public string ApiFraudUrl(long shopifyOrderId) =>
            string.Format(ApiFraudUrlFormat, ApiBaseUrl, shopifyOrderId);

        /// <summary>
        /// The URL to retrieve all shopify fulfillments for a shopify order
        /// </summary>
        /// <param name="shopifyOrderId">The shopify order id</param>
        /// <returns>The URL to retrieve all shopify fulfillments for a shopify order</returns>
        public string ApiFulfillmentsUrl(long shopifyOrderId)
        {
            return string.Format(ApiFulfillmentsUrlFormat, ApiBaseUrl, shopifyOrderId);
        }

        /// <summary>
        /// The URL to a shopify Product Id.  Used to get product information such as image urls.
        /// </summary>
        /// <param name="shopifyProductId">The shopify product Id</param>
        /// <returns>The URL to a shopify Product</returns>
        public string ApiProductUrl(long shopifyProductId)
        {
            return string.Format(ApiGetProductFormat, ApiBaseUrl, shopifyProductId);
        }

        /// <summary>
        /// Should the client use the fake api
        /// </summary>
        private bool UseFakeApi =>
            interapptiveOnly.IsInterapptiveUser && !interapptiveOnly.Registry.GetValue("ShopifyLive", true);
    }
}
