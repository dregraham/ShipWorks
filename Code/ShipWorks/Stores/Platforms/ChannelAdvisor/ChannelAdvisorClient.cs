using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using caOrderService = ShipWorks.Stores.Platforms.ChannelAdvisor.WebServices.Order;
using caShippingService = ShipWorks.Stores.Platforms.ChannelAdvisor.WebServices.Shipping;
using caInventoryService = ShipWorks.Stores.Platforms.ChannelAdvisor.WebServices.Inventory;
using caAdminService = ShipWorks.Stores.Platforms.ChannelAdvisor.WebServices.Admin;
using ShipWorks.ApplicationCore.Logging;
using System.Net;
using Interapptive.Shared.Utility;
using log4net;
using System.Web.Services.Protocols;
using Interapptive.Shared.Net;
using System.Xml;
using Interapptive.Shared;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.ChannelAdvisor.Constants;
using Interapptive.Shared.Collections;
using ShipWorks.Stores.Platforms.ChannelAdvisor.WebServices.Inventory;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    /// <summary>
    /// Thin wrapper around the ChannelAdvisor web clients
    /// </summary>
    public class ChannelAdvisorClient
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(ChannelAdvisorClient));

        // Cache of inventory items we've already looked up
        static LruCache<string, caInventoryService.InventoryItemResponse> inventoryItemCache = new LruCache<string, caInventoryService.InventoryItemResponse>(1000);
        static LruCache<string, caInventoryService.ImageInfoResponse[]> inventoryImageCache = new LruCache<string, caInventoryService.ImageInfoResponse[]>(1000);
        static LruCache<string, List<AttributeInfo>> inventoryItemAttributeCache = new LruCache<string, List<AttributeInfo>>(1000);
						  
        // store this client is interacting on behalf of 
        ChannelAdvisorStoreEntity store = null;

        /// <summary>
        /// Enrypted credentials for using their api
        /// </summary>
        static string apiKey = "kHEXBLDJfWgNzWtKwQfmGgvqYUpr7MY+KHKhrg343I16NZiHJpfNqg==";
        static string apiPassword = "FXH5tUy8pYE6aLR/k1eR8w==";

        // Headers of orders still to download, sorted in ascending order
        List<caOrderService.OrderResponseItem> orderHeaders = null;

        // Number of days back to look if not specified
        const int defaultDaysBack = 1000;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelAdvisorClient(ChannelAdvisorStoreEntity store)
        {
            this.store = store;
        }

        /// <summary>
        /// Must be called prior to each download
        /// </summary>
        public int InitializeDownload(DateTime? startDate)
        {
            DownloadSortedOrderHeaders(startDate);

            return orderHeaders.Count;
        }

        /// <summary>
        /// Tests connectivity by downloading the carrier list for the account provided
        /// </summary>
        public static bool TestConnection(string accountKey)
        {
            try
            {
                using (caShippingService.ShippingService service = CreateShippingService("TestConnection"))
                {
                    service.APICredentialsValue = GetShippingCredentials();

                    caShippingService.APIResultOfArrayOfShippingCarrier result = service.GetShippingCarrierList(accountKey);
                    if (result.Status == caShippingService.ResultStatus.Success)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("An exception occurred while trying to test connectivity with ChannelAdvisor: {0}", ex.Message);

                if (WebHelper.IsWebException(ex))
                {
                    return false;
                }

                throw;
            }
        }

        /// <summary>
        /// Creates and returns an instance of the CA Shipping Service
        /// </summary>
        private static caShippingService.ShippingService CreateShippingService(string logName)
        {
            return new caShippingService.ShippingService(new ApiLogEntry(ApiLogSource.ChannelAdvisor, logName));
        }

        /// <summary>
        /// Creates and returns an instance of the CA Order Service
        /// </summary>
        private static caOrderService.OrderService CreateOrderService(string logName)
        {
            return new caOrderService.OrderService(new ApiLogEntry(ApiLogSource.ChannelAdvisor, logName));
        }

        /// <summary>
        /// Creates an instance of the CA Inventory service configured for logging
        /// </summary>
        private static caInventoryService.InventoryService CreateInventoryService(string logName)
        {
            return new caInventoryService.InventoryService(new ApiLogEntry(ApiLogSource.ChannelAdvisor, logName));
        }

        /// <summary>
        /// Creates an instance of the CA Admin service configured for logging
        /// </summary>
        private static caAdminService.AdminService CreateAdminService(string logName)
        {
            return new caAdminService.AdminService(new ApiLogEntry(ApiLogSource.ChannelAdvisor, logName));
        }

        /// <summary>
        /// Constructs the api credientails for communication with the CA Admin service
        /// </summary>
        private static caAdminService.APICredentials GetAdminCredentials()
        {
            caAdminService.APICredentials credentials = new caAdminService.APICredentials();
            credentials.DeveloperKey = SecureText.Decrypt(apiKey, "interapptive");
            credentials.Password = SecureText.Decrypt(apiPassword, "interapptive");

            return credentials;
        }

        /// <summary>
        /// Constructs the api credentials for communications
        /// </summary>
        private static caShippingService.APICredentials GetShippingCredentials()
        {
            ShipWorks.Stores.Platforms.ChannelAdvisor.WebServices.Shipping.APICredentials credentials = new caShippingService.APICredentials();
            credentials.DeveloperKey = SecureText.Decrypt(apiKey, "interapptive");
            credentials.Password = SecureText.Decrypt(apiPassword, "interapptive");

            return credentials;
        }

        /// <summary>
        /// Gets the credentials for the CA Order service
        /// </summary>
        /// <returns></returns>
        private static caOrderService.APICredentials GetOrderCredentials()
        {
            caOrderService.APICredentials credentials = new caOrderService.APICredentials();
            credentials.DeveloperKey = SecureText.Decrypt(apiKey, "interapptive");
            credentials.Password = SecureText.Decrypt(apiPassword, "interapptive");

            return credentials;
        }

        /// <summary>
        /// Gets the credentialsf or the CA Inventory Service
        /// </summary>
        private static caInventoryService.APICredentials GetInventoryCredentials()
        {
            caInventoryService.APICredentials credentials = new caInventoryService.APICredentials();
            credentials.DeveloperKey = SecureText.Decrypt(apiKey, "interapptive");
            credentials.Password = SecureText.Decrypt(apiPassword, "interapptive");

            return credentials;
        }

        /// <summary>
        /// Downloads the next page of orders from CA
        /// </summary>
        [NDependIgnoreLongMethod]
        public List<caOrderService.OrderResponseDetailComplete> GetNextOrders()
        {
            if (orderHeaders == null)
            {
                throw new InvalidOperationException("IntiializeDownload must be called before GetNextOrders()");
            }

            // Return an empty list early
            if (orderHeaders.Count == 0)
            {
                return new List<caOrderService.OrderResponseDetailComplete>();
            }

            // prepare for the api calls
            using (caOrderService.OrderService service = CreateOrderService("GetOrderList"))
            {
                service.APICredentialsValue = GetOrderCredentials();

                // select which orders to retrieve
                caOrderService.OrderCriteria criteria = new caOrderService.OrderCriteria();
                criteria.DetailLevel = OrderCriteriaDetailLevels.Complete;
                
                // CA docs say you can only get 20 per when using DetailLevelType.Complete
                criteria.PageSize = 20;  
                criteria.PageNumberFilter = 1;
                criteria.StatusUpdateFilterEndTimeGMT = DateTime.UtcNow;

                // Get the next page worth of order ID's
                var nextPage = orderHeaders.Take(criteria.PageSize.Value).ToList();
                orderHeaders.RemoveRange(0, nextPage.Count);

                // Specify the orders we want to download
                criteria.OrderIDList = nextPage.Select(o => o.OrderID).ToArray();

                try
                {
                    // execute the web service call for orders 
                    caOrderService.APIResultOfArrayOfOrderResponseItem result = service.GetOrderList(store.AccountKey, criteria);

                    // check for success
                    if (result.Status == caOrderService.ResultStatus.Success)
                    {
                        // Has to be ordered by last modified so that if we cancel, we start off in the right place
                        List<caOrderService.OrderResponseDetailComplete> orders = result.ResultData.Cast<caOrderService.OrderResponseDetailComplete>().OrderBy(o => o.LastUpdateDate ?? o.OrderTimeGMT).ToList();

                        // If any of the order's LastModTimes have changed since we grabbed the headers then skip them for now (we'll get them in a later call, b\c the date will now be later).  If
                        // we processed it now, we'd be processing a later date befor earlier dates, and if the user then cancelled, we miss everyting in between
                        foreach (var order in orders.ToList())
                        {
                            if (!nextPage.Any(o => o.OrderID == order.OrderID && o.LastUpdateDate == order.LastUpdateDate))
                            {
                                orders.Remove(order);
                            }
                        }

                        return orders;
                    }
                    else
                    {
                        // Fail.
                        throw new ChannelAdvisorException(result.MessageCode, result.Message);
                    }
                }
                catch (Exception ex)
                {
                    throw WebHelper.TranslateWebException(ex, typeof(ChannelAdvisorException));
                }
            }
        }

        /// <summary>
        /// Configures the order criteria for which orders to download based on the user-configurable store setting
        /// </summary>
        private void ConfigureOrderFilter(caOrderService.OrderCriteria criteria)
        {
            // There were many bugs related to users choosing the other critiera. If payment hadn't cleared, order details would be incomplete.
            // If we filtered the shipping status to unshipped, we'd never pickup historic shipped orders, or changes to shipped orders online status
            criteria.PaymentStatusFilter = PaymentStatusCodes.Cleared;
        }

        /// <summary>
        /// Retrieves image information for a particular SKU
        /// </summary>
        public caInventoryService.ImageInfoResponse[] GetItemImages(string sku)
        {
            // do nothing with a blank sku
            if (String.IsNullOrEmpty(sku))
            {
                return null;
            }

            // See if its cached
            var imageResponse = inventoryImageCache[GetInventoryCacheKey(sku)];

            // Only look it up if its not
            if (imageResponse == null)
            {
                using (caInventoryService.InventoryService service = CreateInventoryService("GetItemImages"))
                {
                    try
                    {
                        service.APICredentialsValue = GetInventoryCredentials();

                        caInventoryService.APIResultOfArrayOfImageInfoResponse response = service.GetInventoryItemImageList(store.AccountKey, sku);

                        imageResponse = response.ResultData;
                        inventoryImageCache[GetInventoryCacheKey(sku)] = imageResponse;
                    }
                    catch (Exception ex)
                    {
                        throw WebHelper.TranslateWebException(ex, typeof(ChannelAdvisorException));
                    }
                }
            }

            return imageResponse;
        }

        /// <summary>
        /// Retrieves inventory information for a given SKU
        /// </summary>
        public caInventoryService.InventoryItemResponse[] GetInventoryItems(List<string> skus)
        {
            if (skus == null)
            {
                throw new ArgumentNullException("skus");
            }

            List<caInventoryService.InventoryItemResponse> foundItems = new List<caInventoryService.InventoryItemResponse>();
            List<string> missingSkus = new List<string>();

            // First figure out which ones are already cached
            foreach (string sku in skus)
            {
                string lookupKey = GetInventoryCacheKey(sku);

                var foundItem = inventoryItemCache[lookupKey];

                // Already cached
                if (foundItem != null)
                {
                    foundItems.Add(foundItem);
                }
                // Need to look it up
                else
                {
                    missingSkus.Add(sku);
                }
            }

            // If there are any left to get, get them from CA
            if (missingSkus.Count > 0)
            {
                using (caInventoryService.InventoryService service = CreateInventoryService("GetInventoryItem"))
                {
                    service.APICredentialsValue = GetInventoryCredentials();

                    try
                    {
                        caInventoryService.APIResultOfArrayOfInventoryItemResponse response = service.GetInventoryItemList(store.AccountKey, missingSkus.ToArray());

                        // See if we got the item
                        if (response.ResultData != null)
                        {
                            foreach (var result in response.ResultData)
                            {
                                inventoryItemCache[GetInventoryCacheKey(result.Sku)] = result;
                                foundItems.Add(result);
                            }
                        }
                        
                        if (response.Status == caInventoryService.ResultStatus.Failure || !string.IsNullOrEmpty(response.Message) || response.ResultData == null)
                        {
                            log.WarnFormat("Issue getting inventory results. '{0}', '{1}', '{2}'", response.Status, response.Message, (response.ResultData == null) ? "null" : response.ResultData.Length.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        throw WebHelper.TranslateWebException(ex, typeof(ChannelAdvisorException));
                    }
                }
            }

            return foundItems.ToArray();
        }

        /// <summary>
        /// Gets the item attributes for a given SKU
        /// </summary>
        /// <param name="sku">The SKU of the order item.</param>
        /// <returns>A collection of ChannelAdvisor AttributeInfo objects.</returns>
        /// <exception cref="System.ArgumentNullException">sku</exception>
        public IEnumerable<AttributeInfo> GetInventoryItemAttributes(string sku)
        {
            if (sku == null)
            {
                throw new ArgumentNullException("sku");
            }

            string lookupKey = GetInventoryCacheKey(sku);

            if (!inventoryItemAttributeCache.Contains(lookupKey))
            {
                // We don't have this SKU/key in the cache; grab the item attributes from the 
                // ChannelAdvisor API and add them to the cache (even if the list is empty) 
                // for any future requests for the same SKU
                List<AttributeInfo> attributes = FetchItemAttributes(sku);
                inventoryItemAttributeCache[lookupKey] = attributes;
            }

            // The attributes should be in the cache at this point
            return inventoryItemAttributeCache[lookupKey];
        }

        /// <summary>
        /// Fetches the item attributes for the SKU from the ChannelAdvisor API.
        /// </summary>
        /// <param name="sku">The SKU.</param>
        /// <returns>A List of ChannelAdvisor AttributeInfo objects.</returns>
        private List<AttributeInfo> FetchItemAttributes(string sku)
        {
            try
            {
                List<AttributeInfo> attributes = new List<AttributeInfo>();
                using (InventoryService service = CreateInventoryService("GetInventoryItemItemAttributeList"))
                {
                    service.APICredentialsValue = GetInventoryCredentials();
                    APIResultOfArrayOfAttributeInfo response = service.GetInventoryItemAttributeList(store.AccountKey, sku);

                    if (response.ResultData != null)
                    {
                        // We have data, so add each attribute to our list
                        foreach (AttributeInfo result in response.ResultData)
                        {
                            attributes.Add(result);
                        }
                    }

                    if (response.Status == ResultStatus.Failure || !string.IsNullOrEmpty(response.Message) || response.ResultData == null)
                    {
                        log.WarnFormat("Problem getting item attribute results. '{0}', '{1}', '{2}'", response.Status,
                                       response.Message, (response.ResultData == null) ? "null" : response.ResultData.Length.ToString(CultureInfo.InvariantCulture));
                    }
                }

                return attributes;
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(ChannelAdvisorException));
            }
        }

        /// <summary>
        /// Get the inventory cache key to use for the lookup into the inventory cache
        /// </summary>
        private string GetInventoryCacheKey(string sku)
        {
            return string.Format("{0}_{1}", store.StoreID, sku).ToLowerInvariant();
        }

        /// <summary>
        /// Returns the number of orders available for download
        /// </summary>
        private void DownloadSortedOrderHeaders(DateTime? startDate)
        {
            orderHeaders = new List<caOrderService.OrderResponseItem>();

            using (caOrderService.OrderService service = CreateOrderService("GetOrderList"))
            {
                service.APICredentialsValue = GetOrderCredentials();

                // select which orders to retrieve
                caOrderService.OrderCriteria criteria = new caOrderService.OrderCriteria();
                criteria.DetailLevel = OrderCriteriaDetailLevels.Low;
                criteria.PageSize = 200;
                criteria.PageNumberFilter = 1;
                criteria.StatusUpdateFilterBeginTimeGMT = startDate == null ? DateTime.UtcNow.AddDays(-1 * defaultDaysBack) : startDate.Value;
                criteria.StatusUpdateFilterEndTimeGMT = DateTime.UtcNow;
                ConfigureOrderFilter(criteria);

                // execute the first request
                try
                {
                    caOrderService.APIResultOfArrayOfOrderResponseItem result = service.GetOrderList(store.AccountKey, criteria);

                    // go until we get 0 results back, or a failure
                    while (result.Status == caOrderService.ResultStatus.Success && result.ResultData.Length > 0)
                    {
                        orderHeaders.AddRange(result.ResultData);

                        // safeguard against their api messing up paging
                        if (criteria.PageNumberFilter == 1000)
                        {
                            break;
                        }

                        // results didn't fill the page, so this is the last page
                        if (result.ResultData.Length < criteria.PageSize)
                        {
                            break;
                        }

                        // next page of results
                        criteria.PageNumberFilter++;

                        // execute the next request
                        result = service.GetOrderList(store.AccountKey, criteria);
                    }

                    // raise an error if our last call failed
                    if (result.Status == caOrderService.ResultStatus.Failure)
                    {
                        throw new ChannelAdvisorException(result.MessageCode, result.Message);
                    }
                }
                catch (Exception ex)
                {
                    throw WebHelper.TranslateWebException(ex, typeof(ChannelAdvisorException));
                }
            }

            // Now we have to ensure the are sorted in ascending order
            orderHeaders = orderHeaders.OrderBy(o => o.LastUpdateDate ?? o.OrderTimeGMT).ToList();

            // If the first one exists exactly as is - remove it from the list.  Since we start off with the exact date we just ended on, we would probably have downloaded it again.
            if (orderHeaders.Count > 0 && IsOrderAlreadyUpToDate(orderHeaders[0]))
            {
                orderHeaders.RemoveAt(0);
            }
        }

        /// <summary>
        /// Determines if the given CA order is already completely up-to-date in our local database
        /// </summary>
        private bool IsOrderAlreadyUpToDate(caOrderService.OrderResponseItem order)
        {
            return 1 == OrderCollection.GetCount(SqlAdapter.Default,
                                        OrderFields.StoreID == store.StoreID &
                                        OrderFields.OrderNumber == order.OrderID &
                                        OrderFields.OnlineLastModified == (order.LastUpdateDate ?? order.OrderTimeGMT) &
                                        OrderFields.IsManual == false);
        }
        
        /// <summary>
        /// Sets the orders as exported on Channel Advisor.
        /// </summary>
        public void SetOrdersExportStatus(IEnumerable<string> clientOrderIdentifiers)
        {
            using (caOrderService.OrderService service = CreateOrderService("OrderExportStatus"))
            {
                service.APICredentialsValue = GetOrderCredentials();

                try
                {
                    service.SetOrdersExportStatus(store.AccountKey, clientOrderIdentifiers.ToArray(), true);
                }
                catch (Exception ex)
                {
                    throw WebHelper.TranslateWebException(ex, typeof(ChannelAdvisorException));
                }
            }
        }

        /// <summary>
        /// Uploads the shipment details for the specified order
        /// </summary>
        public void UploadShipmentDetails(int caOrderID, DateTime dateShipped, string carrierCode, string classCode, string trackingNumber)
        {
            using (caShippingService.ShippingService service = CreateShippingService("OrderShipped"))
            {
                service.APICredentialsValue = GetShippingCredentials();

                try
                {
                    caShippingService.OrderShipment[] shipments = new caShippingService.OrderShipment[] 
                    {
                        new caShippingService.OrderShipment 
                        { 
                            OrderId = caOrderID, 
                            ShipmentType = "Full", 
                            FullShipment = new caShippingService.FullShipmentContents 
                            { 
                                carrierCode = carrierCode, 
                                classCode = classCode, 
                                trackingNumber = trackingNumber, 
                                dateShippedGMT = dateShipped 
                            } 
                        }
                    };

                    caShippingService.APIResultOfArrayOfShipmentResponse results = service.SubmitOrderShipmentList(store.AccountKey, new caShippingService.OrderShipmentList { ShipmentList = shipments });
                    if (results.Status == caShippingService.ResultStatus.Failure)
                    {
                        throw new ChannelAdvisorException(results.MessageCode, results.Message);
                    }
                    
                }
                catch (Exception ex)
                {
                    throw WebHelper.TranslateWebException(ex, typeof(ChannelAdvisorException));
                }
            }
        }

        /// <summary>
        /// Initiate the process for requesting access to ChannelAdvisor
        /// </summary>
        public static void RequestShipWorksAccess(int profileIdNumber)
        {
            using (caAdminService.AdminService service = CreateAdminService("RequestAccess"))
            {
                service.APICredentialsValue = GetAdminCredentials();

                try
                {
                    caAdminService.APIResultOfBoolean result = service.RequestAccess(profileIdNumber);
                    if (result.Status == caAdminService.ResultStatus.Failure)
                    {
                        // an authorization already exists, just pretend the request goes through because we'll
                        // pickup the Account Key when we poll for it
                        if (result.MessageCode == 12)
                        {
                            // the same MessageCode 12 is used for completely invalid profileIDs
                            if (result.Message.StartsWith("No ChannelAdvisor Account was found for the specified ID", StringComparison.OrdinalIgnoreCase))
                            {
                                throw new ChannelAdvisorException(result.MessageCode, result.Message);
                            }
                        }
                        else
                        {
                            throw new ChannelAdvisorException(result.MessageCode, result.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw WebHelper.TranslateWebException(ex, typeof(ChannelAdvisorException));
                }
            }
        }

        /// <summary>
        /// Makes an api call to ChannelAdvisor to lookup the user's AccountKey
        /// </summary>
        public static string GetAccountKey(int profileId)
        {
            using (caAdminService.AdminService service = CreateAdminService("GetAccountKey"))
            {
                service.APICredentialsValue = GetAdminCredentials();

                try
                {
                    caAdminService.APIResultOfArrayOfAuthorizationResponse result = service.GetAuthorizationList(profileId.ToString());
                    if (result.Status == caAdminService.ResultStatus.Failure)
                    {
                            throw new ChannelAdvisorException(result.MessageCode, result.Message);
                    }

                    if (result.ResultData != null && result.ResultData.Length > 0)
                    {
                        return result.ResultData[0].AccountID;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    throw WebHelper.TranslateWebException(ex, typeof(ChannelAdvisorException));
                }
            }
        }
    }
}
