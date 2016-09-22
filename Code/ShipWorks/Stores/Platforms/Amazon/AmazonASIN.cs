using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Interapptive.Shared.IO.Text.Csv;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Stores.Platforms.Amazon
{
    /// <summary>
    /// Handles dealing with the Amazon inventory exports and weight/asin/sku lookups.
    /// </summary>
    public static class AmazonAsin
    {
        // cache of loaded ASINs, keyed on SKU and storeID
        static Dictionary<string, string> skuAsinMap = new Dictionary<string, string>();

        // cache of amazon item details, keyed on ASIN
        static Dictionary<string, AmazonItemDetail> asinDetailMap = new Dictionary<string, AmazonItemDetail>();

        #region Amazon Inventory Report importing

        /// <summary>
        /// Validates the specified file to make sure it's an Amazon inventory report.
        /// The number of found records is returned.
        /// </summary>
        public static int ValidateInventoryReport(string reportFile)
        {
            if (!File.Exists(reportFile))
            {
                throw new AmazonException("The specified report file could not be found.", null);
            }

            try
            {
                // open the file and cycle through it
                using (StreamReader reader = File.OpenText(reportFile))
                {
                    using (CsvReader csv = new CsvReader(reader, true, '\t'))
                    {
                        CheckHeaders(csv);

                        // cycle through to find any errors
                        try
                        {
                            int records = 0;
                            while (csv.ReadNextRecord())
                            {
                                records++;
                            }

                            return records;
                        }
                        catch (MalformedCsvException ex)
                        {
                            throw new AmazonException(ex.Message, ex);
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                throw new AmazonException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Simple file-format checking
        /// </summary>
        private static void CheckHeaders(CsvReader csv)
        {
            string[] headers = csv.GetFieldHeaders();

            if (Array.IndexOf<string>(headers, "asin") == -1 || Array.IndexOf<string>(headers, "sku") == -1)
            {
                throw new AmazonException("The inventory file does not contain headers for SKU or ASIN.", null);
            }
        }

        /// <summary>
        /// Perform the inventory import
        /// </summary>
        public static void ImportInventory(ProgressItem progressItem, long amazonStoreID, string reportFile)
        {
            // Starting the work
            progressItem.Starting();

            try
            {
                if (!File.Exists(reportFile))
                {
                    throw new AmazonException("Could not find the specified report file.", null);
                }

                int estimatedRowCount = ValidateInventoryReport(reportFile);

                try
                {
                    // The SKU -> ASIN map will change
                    skuAsinMap.Clear();

                    using (StreamReader reader = File.OpenText(reportFile))
                    {
                        using (CsvReader csv = new CsvReader(reader, true, '\t'))
                        {
                            CheckHeaders(csv);

                            try
                            {
                                int counter = 0;
                                while (csv.ReadNextRecord())
                                {
                                    // exit early if requested
                                    if (progressItem.IsCancelRequested)
                                    {
                                        return;
                                    }

                                    counter++;

                                    progressItem.PercentComplete = estimatedRowCount > 0 ? Math.Min((100 * counter / estimatedRowCount), 100) : 0;
                                    progressItem.Detail = String.Format("Loading item {0} of {1}...", counter, estimatedRowCount);

                                    string sku = csv["sku"];
                                    string asin = csv["asin"];

                                    AddToDatabase(amazonStoreID, sku, asin);
                                }
                            }
                            catch (MalformedCsvException ex)
                            {
                                throw new AmazonException(ex.Message, ex);
                            }
                        }
                    }
                }
                catch (IOException ex)
                {
                    throw new AmazonException(ex.Message, ex);
                }

                // done with the work
                progressItem.Detail = "Done";
                progressItem.Completed();
            }
            catch (Exception ex)
            {
                progressItem.Failed(ex);

                throw;
            }
        }

        /// <summary>
        /// Adds an inentory record to the database
        /// </summary>
        private static void AddToDatabase(long amazonStoreID, string sku, string asin)
        {
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                AmazonASINCollection collection = AmazonASINCollection.Fetch(adapter,
                    AmazonASINFields.StoreID == amazonStoreID &
                    AmazonASINFields.SKU == sku);

                AmazonASINEntity inventoryItem = collection.FirstOrDefault<AmazonASINEntity>();
                if (inventoryItem == null)
                {
                    inventoryItem = new AmazonASINEntity();
                }

                // update the information
                inventoryItem.StoreID = amazonStoreID;
                inventoryItem.SKU = sku;
                inventoryItem.AmazonASIN = asin;

                // insert or update
                adapter.SaveEntity(inventoryItem);

                adapter.Commit();
            }
        }

        #endregion

        /// <summary>
        /// Retrieves the detail about an amazon item
        /// </summary>
        public static AmazonItemDetail GetAmazonItemDetail(AmazonStoreEntity amazonStore, string sku)
        {
            if (amazonStore == null)
            {
                throw new ArgumentNullException("amazonStore");
            }

            if (string.IsNullOrEmpty(sku))
            {
                throw new ArgumentException("sku must be provided.", "sku");
            }

            // translate sku to asin
            string asin = SkuToAsin(amazonStore, sku);

            // see if we have details cached
            AmazonItemDetail itemDetail = null;

            // Only check if we have an Asin for this SKU)
            if (!string.IsNullOrEmpty(asin))
            {
                if (!asinDetailMap.TryGetValue(asin, out itemDetail))
                {
                    // Perform the item lookup against the Amazon Web Service
                    itemDetail = AmazonAssociatesWebClient.GetItemDetail(amazonStore, asin);
                    asinDetailMap[asin] = itemDetail;
                }
            }

            if (itemDetail != null)
            {
                return itemDetail;
            }
            else
            {
                // Create an empty detail item since one wasn't found
                return itemDetail = new AmazonItemDetail() { Asin = "", Weight = 0, ItemUrl = "" };
            }
        }

        /// <summary>
        /// Gets the ASIN that is mapped to the sku in the amazonStore.
        /// </summary>
        public static string SkuToAsin(AmazonStoreEntity amazonStore, string sku)
        {
            if (string.IsNullOrEmpty(sku))
            {
                return "";
            }

            // retrieve the sku -> asin mapping from the imported Inventory report, now in the AmazonAsin table
            string key = sku + "-store" + amazonStore.StoreID;

            // Check the cache first
            string asin;
            if (!skuAsinMap.TryGetValue(key, out asin))
            {
                // Lookup in the database
                using (SqlAdapter adapter = new SqlAdapter(false))
                {
                    object objAsin = adapter.GetScalar(AmazonASINFields.AmazonASIN, null, AggregateFunction.None, AmazonASINFields.StoreID == amazonStore.StoreID & AmazonASINFields.SKU == sku);
                    if (objAsin != null)
                    {
                        asin = (string) objAsin;
                    }
                    else
                    {
                        // asin not found, return empty
                        asin = "";
                    }

                    skuAsinMap[key] = asin;
                }
            }

            return asin;
        }
    }
}
