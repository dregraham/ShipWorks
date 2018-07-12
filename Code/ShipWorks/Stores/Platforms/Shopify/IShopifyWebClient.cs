using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using ShipWorks.Stores.Platforms.Shopify.DTOs;

namespace ShipWorks.Stores.Platforms.Shopify
{
    /// <summary>
    /// Interface to connecting to Shopify
    /// </summary>
    public interface IShopifyWebClient
    {
        /// <summary>
        /// Endpoints object for getting api urls
        /// </summary>
        ShopifyEndpoints Endpoints { get; }

        /// <summary>
        /// Gets shop information for a Shopify store
        /// </summary>
        ShopifyShop GetShop();

        /// <summary>
        /// Update store information from Shopify
        /// </summary>
        void RetrieveShopInformation();

        /// <summary>
        /// Makes a call to the web server and get's it's current date and time
        /// </summary>
        DateTime GetServerCurrentDateTime();

        /// <summary>
        /// Make a call to Shopify requesting a count of orders matching criteria.
        /// </summary>
        /// <param name="startDate">Filter by shopify order modified date after this date</param>
        /// <param name="endDate">Filter by shopify order modified date before this date</param>
        /// <returns>Number of orders matching criteria</returns>
        int GetOrderCount(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Make the call to Shopify to get a list of orders in the date range
        /// </summary>
        /// <returns>List of JToken orders, sorted by updated_at ascending</returns>
        List<JToken> GetOrders(DateTime startDate, DateTime endDate, int page = 1);

        /// <summary>
        /// Get a shopify product by shopify Product Id
        /// This method will first check the local product cache and return that product object if found,
        /// otherwise, it will make a call to Shopify to get the product, then store it in the cache.
        /// </summary>
        /// <param name="shopifyProductId">Shopify Product Id</param>
        /// <returns></returns>
        JToken GetProduct(long shopifyProductId);

        /// <summary>
        /// Get fraud risks for an order
        /// </summary>
        /// <param name="shopifyOrderId">Shopify Order Id</param>
        /// <returns>Collection of fraud risks</returns>
        IEnumerable<JToken> GetFraudRisks(long shopifyOrderId);

        /// <summary>
        /// Update the online status of the given orders
        /// </summary>
        void UploadOrderShipmentDetails(long shopifyOrderID, string trackingNumber, string carrier, string carrierTrackingUrl, IEnumerable<long> itemIDs);

        /// <summary>
        /// Get inventory levels for a list of inventory ids
        /// </summary>
        IEnumerable<ShopifyInventoryLevel> GetInventoryLevels(IEnumerable<long> itemInventoryIdList);
    }
}
