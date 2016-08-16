using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.XPath;
using Interapptive.Shared;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using Quartz.Util;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Stores.Platforms.Yahoo.EmailIntegration
{
    /// <summary>
    /// Manages yahoo products and their associated weights
    /// </summary>
    public static class YahooEmailProductManager
    {
        static Dictionary<string, double> productWeightMap = new Dictionary<string, double>();

        /// <summary>
        /// Get the weight of the given product from the important yahoo product catalog. Returns zero if not found.
        /// </summary>
        public static double GetItemWeight(YahooStoreEntity yahooStoreEntity, string yahooProductID)
        {
            if (string.IsNullOrEmpty(yahooProductID))
            {
                return 0;
            }

            double weight;
            string lookupKey = GetLookupKey(yahooStoreEntity.StoreID, yahooProductID);

            lock (productWeightMap)
            {
                if (productWeightMap.TryGetValue(lookupKey, out weight))
                {
                    return weight;
                }
            }

            YahooProductEntity product = new YahooProductEntity(yahooStoreEntity.StoreID, yahooProductID);
            SqlAdapter.Default.FetchEntity(product);

            lock (productWeightMap)
            {
                weight = (product.Fields.State == EntityState.Fetched) ? product.Weight : 0;
                productWeightMap[lookupKey] = weight;
            }

            return weight;
        }

        /// <summary>
        /// Get the key to use in a dictionary to lookup the given product for the given store
        /// </summary>
        private static string GetLookupKey(long storeID, string yahooProductID)
        {
            return string.Format("Store{0}_{1}", storeID, yahooProductID);
        }

        /// <summary>
        /// Perform the inventory import
        /// </summary>
        [NDependIgnoreLongMethod]
        public static void ImportProductCatalog(ProgressItem progressItem, long yahooStoreID, string catalogUrl)
        {
            catalogUrl = catalogUrl.Trim();

            if (catalogUrl.IsNullOrWhiteSpace())
            {
                throw new YahooException("Enter your product catalog URL.");
            }

            if (!catalogUrl.StartsWith("http"))
            {
                catalogUrl = "http://" + catalogUrl;
            }

            Uri uri;

            try
            {
                uri = new Uri(catalogUrl);
            }
            catch (UriFormatException)
            {
                throw new YahooException("The URL you entered is not a valid URL format.");
            }

            // Have to clear out the previous cache so new lookups get the newly imported values
            lock (productWeightMap)
            {
                productWeightMap.Clear();
            }

            // Starting the work
            progressItem.Starting();
            progressItem.Detail = "Downloading catalog...";

            try
            {
                HttpWebRequest webRequest = (HttpWebRequest) WebRequest.Create(uri);

                using (HttpWebResponse response = (HttpWebResponse) webRequest.GetResponse())
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new YahooException(response.StatusDescription);
                    }

                    if (!response.ContentType.ToUpper().EndsWith("XML"))
                    {
                        throw new YahooException("The specified URL does not appear to be a Yahoo! Product Catalog.");
                    }


                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        XPathDocument document = new XPathDocument(reader);
                        XPathNavigator xpath = document.CreateNavigator();

                        // this is the catalog.xml format
                        XPathNodeIterator iterator = xpath.Select("/Catalog/Item");

                        int total = iterator.Count;
                        int counter = 0;

                        // Standard format
                        foreach (XPathNavigator xpathItem in iterator)
                        {
                            counter++;
                            progressItem.Detail = string.Format("Importing item {0} of {1}...", counter, total);

                            ImportItem(xpathItem, yahooStoreID);
                        }

                        // Legacy format
                        iterator = xpath.Select("/StoreExport/Products/Product");

                        total = iterator.Count;
                        counter = 0;

                        foreach (XPathNavigator xpathItem in iterator)
                        {
                            counter++;
                            progressItem.Detail = string.Format("Importing item {0} of {1}...", counter, total);

                            ImportLegacyItem(xpathItem, yahooStoreID);
                        }
                    }
                }

                // done with the work
                progressItem.Detail = "Done";
                progressItem.Completed();
            }
            catch (Exception ex)
            {
                progressItem.Failed(ex);

                throw WebHelper.TranslateWebException(ex, typeof(YahooException));
            }
        }

        /// <summary>
        /// Imports an item from the catalog.xml
        /// </summary>
        private static void ImportItem(XPathNavigator xpath, long yahooStoreID)
        {
            // product id
            string id = XPathUtility.Evaluate(xpath, "@ID", "");

            // get the shipweight
            double shipWeight = XPathUtility.Evaluate(xpath, "ItemField[@TableFieldID='ship-weight']/@Value", 0.0);

            // add to or update the database for this item
            AddToDatabase(yahooStoreID, id, shipWeight);
        }

        /// <summary>
        /// Imports an item from the objinfo.xml (legacy catalog format)
        /// </summary>
        private static void ImportLegacyItem(XPathNavigator xpath, long yahooStoreID)
        {
            // product Id
            string id = XPathUtility.Evaluate(xpath, "@Id", "");

            // get the weight
            double weight = XPathUtility.Evaluate(xpath, "Weight", 0.0);

            // add to or update the database for this item
            AddToDatabase(yahooStoreID, id, weight);
        }

        /// <summary>
        /// Adds an inventory record to the database
        /// </summary>
        private static void AddToDatabase(long yahooStoreID, string productID, double weight)
        {
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                YahooProductCollection collection = YahooProductCollection.Fetch(adapter,
                    YahooProductFields.StoreID == yahooStoreID &
                    YahooProductFields.YahooProductID == productID);

                YahooProductEntity inventoryItem = collection.FirstOrDefault<YahooProductEntity>();
                if (inventoryItem == null)
                {
                    inventoryItem = new YahooProductEntity();
                }

                // update the information
                inventoryItem.StoreID = yahooStoreID;
                inventoryItem.YahooProductID = productID;
                inventoryItem.Weight = weight;

                // insert or update
                adapter.SaveEntity(inventoryItem);

                adapter.Commit();
            }
        }
    }
}
