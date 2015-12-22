using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using System.Xml.XPath;
using System.Globalization;
using Interapptive.Shared;
using log4net;

namespace ShipWorks.Stores.Platforms.Amazon.Mws
{
    /// <summary>
    /// A repository to obtain product details (weight, images, etc.) for order items. 
    /// </summary>
    public class AmazonProductDetailRepository
    {
        const int MaxItemsInCache = 1000;
        AmazonMwsClient webClient;

        // logger
        static readonly ILog log = LogManager.GetLogger(typeof(AmazonProductDetailRepository));

        // An in-memory cache for product information obtained from Amazon
        static LruCache<string, XPathNamespaceNavigator> productDetailCache = new LruCache<string, XPathNamespaceNavigator>(MaxItemsInCache);

        // List of storeID's that don't support the products API (due to being a Webstore)
        static HashSet<long> unsupportedStores = new HashSet<long>();
        
        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonProductDetailRepository"/> class.
        /// </summary>
        public AmazonProductDetailRepository(AmazonMwsClient webClient)
        {
            this.webClient = webClient;
        }

        /// <summary>
        /// Gets the product details.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns>XPathNamspaceNavigator objects containing the product details.</returns>
        public IEnumerable<XPathNamespaceNavigator> GetProductDetails(List<AmazonOrderItemEntity> items)
        {
            List<XPathNamespaceNavigator> products = new List<XPathNamespaceNavigator>();

            // Extract the unique items from the item list; these will be used to compare against our cache
            // and any not found in the cache will be used to pare down the list of product data we retreive
            // from Amazon
            List<AmazonOrderItemEntity> distinctItems = items.Distinct(new AmazonOrderItemASINComparer()).ToList();
            List<AmazonOrderItemEntity> itemsFoundInCache = new List<AmazonOrderItemEntity>();

            // Check with the cache to see if we've already retrieved data for any of the ASINs from Amazon
            foreach (AmazonOrderItemEntity item in distinctItems)
            {
                lock (productDetailCache)
                {
                    if (productDetailCache.Contains(item.ASIN))
                    {
                        // Fetch the product info from the cache and note that the info for the 
                        // current item was found in the cache
                        products.Add(productDetailCache[item.ASIN]);
                        itemsFoundInCache.Add(item);
                    }
                }
            }

            // Remove those items that we found in the cache so that we are left only with those items 
            // whose data needs to be fetched from Amazon
            distinctItems = distinctItems.Except(itemsFoundInCache, new AmazonOrderItemASINComparer()).ToList();
            products.AddRange(FetchProductDetailsFromAmazon(distinctItems));

            return products;
        }

        /// <summary>
        /// Fetches the product details from Amazon. This will also add the items to local cache.
        /// </summary>
        /// <param name="items">The items we need to get product details for.</param>
        /// <returns>XPathNamspaceNavigator objects containing the product details.</returns>
        [NDependIgnoreLongMethod]
        private IEnumerable<XPathNamespaceNavigator> FetchProductDetailsFromAmazon(List<AmazonOrderItemEntity> items)
        {
            // We are going to get the product details for the list of items provided, but we are restricted
            // on the number of items we can get in one request. Each item in the request to Amazon must also 
            // be unique. We're going to get a list of the unique order items in the list provided, and get 
            // the details for each of those taken X number at a time based on our maximum allowed batch size. 
            // We're then going to add this product data to our local cache to cut down on the number of calls 
            // to Amazon by batching items together and by updating the items in the local cache we are not 
            // making calls to get data we've previously downloaded.

            List<XPathNamespaceNavigator> products = new List<XPathNamespaceNavigator>();

            // Extract the unique items from the item list; these will be used download product data from Amazon
            List<AmazonOrderItemEntity> distinctItems = items.Distinct(new AmazonOrderItemASINComparer()).ToList();

            while (distinctItems.Count > 0)
            {
                // Create a batch of items based on the maximum items allowed in a request; this batch is taken from the first
                // X number of items in the distinct items list, so it will be important to remove these from the distinct list
                // when we're done with this batch.
                List<AmazonOrderItemEntity> batchOfItems = distinctItems.Take(AmazonMwsClient.MaxItemsPerProductDetailsRequest).ToList();
                List<string> foundProductASINs = new List<string>();

                // Fetch the product details for our list of items and iterate over each product that is retrieved
                try
                {
                    // Don't try to get product details if we already know it won't work
                    if (!unsupportedStores.Contains(webClient.Store.StoreID))
                    {
                        XPathNamespaceNavigator navigator = webClient.GetProductDetails(batchOfItems);
                        XPathNodeIterator productIterator = navigator.Select("//amz:Product");

                        lock (productDetailCache)
                        {
                            foreach (XPathNavigator product in productIterator)
                            {
                                // Create the navigator to query over the product XML
                                XPathNamespaceNavigator productNavigator = new XPathNamespaceNavigator(product, navigator.Namespaces);

                                // Pull out the ASIN and add it to our list of products that we found
                                string productASIN = XPathUtility.Evaluate(productNavigator, "amz:Identifiers/amz:MarketplaceASIN/amz:ASIN", string.Empty);
                                foundProductASINs.Add(productASIN);

                                // Add the product to our cache and also add to the local list of products
                                productDetailCache[productASIN] = productNavigator;
                                products.Add(productNavigator);
                            }
                        }

                        // If we were not able to find all the products in the batch, so make a log entry denoting any products that were not found
                        if (foundProductASINs.Count != batchOfItems.Count)
                        {
                            IEnumerable<string> missingProducts = batchOfItems.Select(i => i.ASIN).Except(foundProductASINs, StringComparer.Create(CultureInfo.InvariantCulture, true));
                            log.InfoFormat("Amazon did not return any data for products with the following ASINs: {0}", string.Join(", ", missingProducts));
                        }
                    }
                }
                catch (AmazonException ex)
                {
                    // This happens if the Marketplace doesn't support the Products API - which is true (currently 9/5/2012) of Webstores
                    if (ex.Code == "InvalidParameterValue" && ex.Message.StartsWith("The given marketplace"))
                    {
                        log.WarnFormat("Adding store [{0}] to list of stores not supported by the Product API.", webClient.Store.StoreID);
                        unsupportedStores.Add(webClient.Store.StoreID);
                    }
                    else if (ex.Code == "InvalidParameterValue" && ex.Message.StartsWith("Invalid ASIN identifier"))
                    {
                        // Don't hold up the download if data for product data that could not be downloaded.
                        log.WarnFormat("ShipWorks could not download product details (item weight and thumbnail image) for an ASIN. {0}", ex.Message);
                    }
                    else if (ex.Code == "InternalError" && ex.Message.Contains("Please contact the MWS team if this problem persists"))
                    {
                        // A few stores reported this error with Amazon - log the error and continue, so downloads aren't stuck
                        log.WarnFormat("Amazon's server/API encountered an internal error: {0}. ShipWorks could not download product details (item weight and thumbnail image) for an ASIN.", ex.Message);
                    }
                    else
                    {
                        throw;
                    }
                }

                // We are finished with this group of items, so remove it from our distinct items list 
                // in preparation for the next iteration
                distinctItems.RemoveRange(0, batchOfItems.Count);
            }

            return products;
        }
    }
}
