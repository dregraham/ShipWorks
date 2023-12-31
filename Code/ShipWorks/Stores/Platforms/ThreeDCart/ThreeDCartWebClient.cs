﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Net;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication.Throttling;
using ShipWorks.Stores.Platforms.ThreeDCart.AdvancedApi;
using ShipWorks.Stores.Platforms.ThreeDCart.Enums;
using ShipWorks.Stores.Platforms.ThreeDCart.WebServices.Cart;
using ShipWorks.Stores.Platforms.ThreeDCart.WebServices.CartAdvanced;
using ShipWorks.Stores.Platforms.ThreeDCart.OnlineUpdating;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Stores.Platforms.ThreeDCart
{
    /// <summary>
    /// Interface for connecting to ThreeDCart
    /// </summary>
    [Component]
    public class ThreeDCartWebClient : IThreeDCartSoapWebClient
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ThreeDCartWebClient));
        IApiQueryProvider apiQueryProvider;
        readonly ThreeDCartStoreEntity store;
        readonly List<ThreeDCartOrderStatus> orderStatuses;
        static readonly LruCache<string, ThreeDCartProductDTO> productCache = new LruCache<string, ThreeDCartProductDTO>(1000);
        static readonly LruCache<string, XmlNode> getProductsCache = new LruCache<string, XmlNode>(500);
        static readonly List<string> productNotFoundCache = new List<string>();
        static readonly ThreeDCartWebClientRequestThrottle throttler = new ThreeDCartWebClientRequestThrottle();
        readonly IProgressReporter progressReporter;

        /// <summary>
        /// Create an instance of the web client for connecting to the specified store
        /// </summary>
        public ThreeDCartWebClient(ThreeDCartStoreEntity store, IProgressReporter progressReporter)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            // StoreDomain is based on StoreUrl, so check both.  But the user only knows about StoreUrl, so only mention
            // that in the exception.
            if (string.IsNullOrWhiteSpace(store.StoreUrl) || string.IsNullOrWhiteSpace(store.StoreDomain))
            {
                throw new ThreeDCartException("The 3dcart Store Url is missing or invalid.");
            }

            if (string.IsNullOrWhiteSpace(store.ApiUserKey))
            {
                throw new ThreeDCartException(
                    "The 3dcart Store API User Key is missing. Please enter your API User Key by going to " +
                    $"Manage > Stores > {store.StoreName} > Edit > Store Connection.  You will find instructions" +
                    " on how to find the API User Key there. ");
            }

            this.store = store;
            this.progressReporter = progressReporter;

            orderStatuses = new List<ThreeDCartOrderStatus>();
        }

        /// <summary>
        /// Create a 3dcart advanced api web service object with ApiLogEntry
        /// </summary>
        private static cartAPIAdvanced CreateAdvancedApiWebService(string logName)
        {
            // Strip out characters that aren't allowed in a file system file name
            logName = Regex.Replace(logName, "[^.0-9A-Za-z(),]", "");

            return new cartAPIAdvanced(new ApiLogEntry(ApiLogSource.ThreeDCart, logName));
        }

        /// <summary>
        /// Create a 3dcart api web service object with ApiLogEntry
        /// </summary>
        private static cartAPI CreateApiWebService(string logName)
        {
            // Strip out characters that aren't allowed in a file system file name
            logName = Regex.Replace(logName, "[^.0-9A-Za-z(),]", "");

            return new cartAPI(new ApiLogEntry(ApiLogSource.ThreeDCart, logName));
        }

        /// <summary>
        /// Makes calls to 3dcart to determine the type of database the site is running against.
        /// The sql statements should be ones that will not result in an error for it's database type.
        /// </summary>
        /// <returns>
        /// If an SQL Server call succeeds, an SqlServerQueryProvider object is returned
        /// If an SQL Server call fails, and an MS Access call succeeds, an MSAccessQueryProvider object is returned
        /// IF neither succeed, a ThreeDCartException is thrown.
        /// </returns>
        private IApiQueryProvider ApiQueryProvider
        {
            get
            {
                if (apiQueryProvider == null)
                {
                    IApiQueryProvider dBQuery = new SqlServerQueryProvider();
                    string determineDbVersionSql = dBQuery.ValidSqlStatementForThisDatabaseTypeOnly;
                    RequestThrottleParameters requestThrottleArgs = new RequestThrottleParameters(ThreeDCartWebClientApiCall.GetOrderCount, null, progressReporter);

                    XmlNode determineDbVersionResultXml = null;
                    using (cartAPIAdvanced advancedCartApiWebService = CreateAdvancedApiWebService("Determine Database Version - SQL Server"))
                    {
                        // Not using the MakeAdvancedCartApiQuery method here because it will throw if there's an error xml node.  So instead of
                        // trapping, we'll check for the error node.
                        // See if the database is sql server
                        try
                        {
                            throttler.ExecuteRequest(requestThrottleArgs, () =>
                            {
                                determineDbVersionResultXml = advancedCartApiWebService.runQuery(store.StoreDomain, store.ApiUserKey, determineDbVersionSql, string.Empty);
                            });
                        }
                        catch (Exception ex)
                        {
                            throw WebHelper.TranslateWebException(ex, typeof(ThreeDCartException));
                        }
                    }

                    XmlNode errorNode = GetErrorFromResponse(determineDbVersionResultXml);

                    // If errorNode is not null, there was an error, so try MS Access
                    if (errorNode != null)
                    {
                        // It wasn't, check MS Access
                        dBQuery = new MSAccessQueryProvider();
                        determineDbVersionSql = dBQuery.ValidSqlStatementForThisDatabaseTypeOnly;

                        using (cartAPIAdvanced advancedCartApiWebService = CreateAdvancedApiWebService("Determine Database Version - MS Access"))
                        {
                            try
                            {
                                throttler.ExecuteRequest(requestThrottleArgs, () =>
                                {
                                    determineDbVersionResultXml = advancedCartApiWebService.runQuery(store.StoreDomain, store.ApiUserKey, determineDbVersionSql, string.Empty);
                                });
                            }
                            catch (Exception ex)
                            {
                                throw WebHelper.TranslateWebException(ex, typeof(ThreeDCartException));
                            }
                        }

                        errorNode = GetErrorFromResponse(determineDbVersionResultXml);
                        if (errorNode != null)
                        {
                            // We couldn't connect via SQL server or MS Access...log and throw
                            log.Error("ShipWorks was unable to determine database type for 3dcart website.  See log for response.");
                            throw new ThreeDCartException("ShipWorks was unable to contact your 3dcart store.", errorNode);
                        }
                    }

                    apiQueryProvider = dBQuery;
                }

                return apiQueryProvider;
            }
        }

        /// <summary>
        /// Update the online status and details of the given shipment
        /// </summary>
        public IResult UploadOrderShipmentDetails(ThreeDCartOnlineUpdatingOrderDetail orderDetail, long threeDCartShipmentID, string trackingNumber)
        {
            if (orderDetail.IsManual)
            {
                log.InfoFormat("Not uploading shipment details for OrderID {0} since it is manual.", orderDetail.OrderNumberComplete);
                return Result.FromSuccess();
            }

            try
            {
                // Create a 3dcart fulfillment
                CreateFulfillment(orderDetail.OrderNumber, orderDetail.OrderNumberComplete, trackingNumber, threeDCartShipmentID);
            }
            catch (Exception ex)
            {
                return Result.FromError(WebHelper.TranslateWebException(ex, typeof(ThreeDCartException)));
            }

            return Result.FromSuccess();
        }

        /// <summary>
        /// Add a fulfillment, aka Shipment, to the ThreeDCart online order
        /// </summary>
        private void CreateFulfillment(long orderNumber, string orderNumberComplete, string trackingNumber, long threeDCartShipmentID)
        {
            RequestThrottleParameters requestThrottleArgs = new RequestThrottleParameters(ThreeDCartWebClientApiCall.CreateFulfillment, null, progressReporter);
            XmlNode response = null;

            using (cartAPI apiWebService = CreateApiWebService("CreateFulfillment"))
            {
                // Fix the invoice number to send for the fulfillment
                string fixedInvoiceNum = FixInvoiceNumberForFulfillment(orderNumber, orderNumberComplete);

                // Make the api call to update the order shipment
                throttler.ExecuteRequest(requestThrottleArgs, () =>
                {
                    response = apiWebService.updateOrderShipment(store.StoreDomain, store.ApiUserKey, fixedInvoiceNum,
                        threeDCartShipmentID.ToString(CultureInfo.InvariantCulture), trackingNumber,
                        store.ConvertToStoreDateTime(DateTime.Now, TimeZoneInfo.Local.Id).ToString(CultureInfo.InvariantCulture),
                        string.Empty);
                });

                // Check to see if there was an error in the response
                XmlNode errorNode = GetErrorFromResponse(response);
                if (errorNode != null)
                {
                    log.ErrorFormat("ShipWorks was unable to update the order shipment details.  Error: {0}", errorNode.OuterXml);
                    throw new ThreeDCartException("ShipWorks was unable to update the order shipment details.");
                }
            }
        }

        /// <summary>
        /// Remove the ending post fix for split orders
        /// </summary>
        /// <param name="invoiceNumber">The numeric invoice number</param>
        /// <param name="orderNumberComplete">The order number complete with 3dcart's prefix, if applicable, and SW's post fix for multiple orders, if applicable.</param>
        private static string FixInvoiceNumberForFulfillment(long invoiceNumber, string orderNumberComplete)
        {
            string invoiceNum = invoiceNumber.ToString();
            string fixedInvoiceNum = orderNumberComplete;
            if (fixedInvoiceNum.Contains("-"))
            {
                int endOfInvoiceNumIndex = fixedInvoiceNum.LastIndexOf(invoiceNum) + invoiceNum.Length - 1;
                int indexOfLastDash = fixedInvoiceNum.LastIndexOf("-");

                if (indexOfLastDash > endOfInvoiceNumIndex)
                {
                    fixedInvoiceNum = fixedInvoiceNum.Substring(0, indexOfLastDash);
                }
            }

            return fixedInvoiceNum;
        }

        /// <summary>
        /// Helper method to check an xml response from the api to see if there are any Error nodes.
        /// </summary>
        /// <param name="responseToCheck">The api xml response to check</param>
        /// <returns>
        /// If there is an error node, the error node is returned.  Otherwise, null.
        /// If responseToCheck is null, a new error node is returned nothing that an error communicating with 3dcart occurred. </returns>
        private static XmlNode GetErrorFromResponse(XmlNode responseToCheck)
        {
            XmlDocument doc;
            XmlNode errorNode = null;

            if (responseToCheck?.OwnerDocument == null)
            {
                doc = new XmlDocument();
                errorNode = doc.CreateElement("Error");
                errorNode.InnerText = "ShipWorks received an error communicating with 3dcart.";
                doc.AppendChild(errorNode);
            }
            else
            {
                if (responseToCheck.OwnerDocument.DocumentElement == null)
                {
                    // It seems that the response xml we get back does not always have a root node
                    // So check to see if the owner doc's document element is null.  If it is, create a new
                    // xml doc, and load the response xml into it.  Otherwise SelectNodes will fail.
                    doc = new XmlDocument();
                    doc.LoadXml(responseToCheck.OuterXml);
                    responseToCheck = doc.FirstChild;
                }

                // See if there are any error nodes
                XmlNodeList errorNodes = responseToCheck.SelectNodes("//Error");
                if (errorNodes != null && errorNodes.Count > 0)
                {
                    errorNode = errorNodes[0];
                }
            }

            return errorNode;
        }

        /// <summary>
        /// Updates the online status of orders
        /// </summary>
        public IResult UpdateOrderStatus(long orderNumber, string orderNumberComplete, int statusCode)
        {
            XmlNode response = null;
            RequestThrottleParameters requestThrottleArgs = new RequestThrottleParameters(ThreeDCartWebClientApiCall.UpdateOrderStatus, null, progressReporter);

            try
            {
                // Fix the invoice number to send for the fulfillment
                string fixedInvoiceNum = FixInvoiceNumberForFulfillment(orderNumber, orderNumberComplete);

                using (cartAPI api = CreateApiWebService("UpdateOrderStatus"))
                {
                    // Get the status text for the status code
                    ThreeDCartStatusCodeProvider statusProvider = new ThreeDCartStatusCodeProvider(store);
                    string statusText = statusProvider.GetCodeName(statusCode);

                    // Update the order status online
                    throttler.ExecuteRequest(requestThrottleArgs, () =>
                    {
                        response = api.updateOrderStatus(store.StoreDomain, store.ApiUserKey, fixedInvoiceNum, statusText, string.Empty);
                    });
                }
            }
            catch (Exception ex)
            {
                return Result.FromError(WebHelper.TranslateWebException(ex, typeof(ThreeDCartException)));
            }

            // If there was an error, throw
            XmlNode errorNode = GetErrorFromResponse(response);
            if (errorNode != null)
            {
                log.ErrorFormat("ShipWorks was unable to update the order status.  Error Message: {0}", errorNode.OuterXml);
                ThreeDCartException ex = new ThreeDCartException(
                    $"ShipWorks was unable to update the order status for order number { orderNumber }.", errorNode);
                return Result.FromError(ex);
            }

            return Result.FromSuccess();
        }

        /// <summary>
        /// Make a call to ThreeDCart requesting a count of orders matching criteria.
        /// </summary>
        /// <param name="orderSearchCriteria">Filter by ThreeDCartWebClientOrderSearchCriteria.</param>
        /// <returns>Number of orders matching criteria</returns>
        public int GetOrderCount(ThreeDCartWebClientOrderSearchCriteria orderSearchCriteria)
        {
            int orderCount = 0;
            RequestThrottleParameters requestThrottleArgs = new RequestThrottleParameters(ThreeDCartWebClientApiCall.GetOrderCount, null, progressReporter);
            XmlNode orderCountResultXml = null;

            using (cartAPIAdvanced advancedCartApiWebService = CreateAdvancedApiWebService("GetOrderCount"))
            {
                string orderCountSqlQuery = ApiQueryProvider.QueryOrderCount(orderSearchCriteria);

                // Make the call and get the response.
                throttler.ExecuteRequest(requestThrottleArgs, () =>
                {
                    orderCountResultXml = MakeAdvancedCartApiQuery(advancedCartApiWebService, orderCountSqlQuery, "Get order count");
                });
            }

            if (!int.TryParse(orderCountResultXml.InnerText, out orderCount))
            {
                log.Error("ShipWorks received an invalid order count from 3dcart.  See the log response for more detail.");
                throw new ThreeDCartException("ShipWorks received an invalid order count from 3dcart.");
            }

            return orderCount;
        }

        /// <summary>
        /// Make the call to ThreeDCart to get a list of orders matching criteria
        /// </summary>
        /// <param name="orderSearchCriteria">Filter by ThreeDCartWebClientOrderSearchCriteria.</param>
        /// <returns>List of orders matching criteria, sorted by LastUpdate ascending </returns>
        public List<XmlNode> GetOrders(ThreeDCartWebClientOrderSearchCriteria orderSearchCriteria)
        {
            List<XmlNode> ordersToReturn = new List<XmlNode>();
            RequestThrottleParameters requestThrottleArgs = new RequestThrottleParameters(ThreeDCartWebClientApiCall.GetOrders, null, progressReporter);

            // 3dcart's regular api doesn't let you query by modified date, but the advanced api does.
            // So we'll ask the advanced api for modified order numbers, then download them one by one
            XmlNode ordersResultXml = null;
            using (cartAPIAdvanced apiAdvancedWebService = CreateAdvancedApiWebService("GetOrders"))
            {
                string sqlQuery;
                if (orderSearchCriteria.OrderDateSearchType == ThreeDCartWebClientOrderDateSearchType.CreatedDate)
                {
                    sqlQuery = ApiQueryProvider.QueryByOrderCreateDate(orderSearchCriteria);
                }
                else
                {
                    sqlQuery = ApiQueryProvider.QueryByOrderModifiedDate(orderSearchCriteria);
                }

                throttler.ExecuteRequest(requestThrottleArgs, () =>
                {
                    ordersResultXml = MakeAdvancedCartApiQuery(apiAdvancedWebService, sqlQuery, "QueryByLastOrderDateFormat");
                });
            }

            XmlNodeList ordersToGet = ordersResultXml.SelectNodes("//runQueryRecord");

            FetchOrders(ordersToGet, ordersToReturn);

            return SortOrders(ordersToReturn, orderSearchCriteria).ToList();
        }

        /// <summary>
        /// Get all of the order info from 3dcart
        /// </summary>
        private void FetchOrders(XmlNodeList ordersToGet, List<XmlNode> ordersToReturn)
        {
            // Track if we should send the invoice number prefix
            bool sendPrefix = false;

            foreach (XmlNode orderNode in ordersToGet)
            {
                // Get the invoice number (without prefix).  We'll be using this for the async user state unique id
                string invoiceNumber = orderNode["invoicenum"].InnerText;
                string invoiceNumberPrefix = orderNode["invoicenum_prefix"].InnerText;
                XmlNode orderResultXml = null;
                RequestThrottleParameters requestThrottleArgs = new RequestThrottleParameters(ThreeDCartWebClientApiCall.GetOrder, null, progressReporter);

                // Sometimes 3dcart wants the invoice number prefix included in the request
                string invoiceNumberToSend = sendPrefix ? $"{invoiceNumberPrefix}{invoiceNumber}" : invoiceNumber;

                using (cartAPI api = CreateApiWebService($"GetOrder ({invoiceNumber})"))
                {
                    throttler.ExecuteRequest(requestThrottleArgs, () =>
                    {
                        orderResultXml = api.getOrder(store.StoreDomain, store.ApiUserKey, 1, 1, false, invoiceNumberToSend, string.Empty, string.Empty, string.Empty, string.Empty);
                    });

                    // Check to see if orders were returned, if they returned an error
                    // Call getOrders again with the prefix and then set sendPrefix to true
                    // see FD#603416
                    if (orderResultXml.Name == "Error")
                    {
                        throttler.ExecuteRequest(requestThrottleArgs, () =>
                        {
                            orderResultXml = api.getOrder(store.StoreDomain, store.ApiUserKey, 1, 1, false, $"{invoiceNumberPrefix}{invoiceNumber}", string.Empty, string.Empty, string.Empty, string.Empty);
                        });
                        sendPrefix = true;
                    }
                }

                ValidateCartApiQueryResponse(orderResultXml, "GetOrder");

                orderResultXml = orderResultXml.FirstChild;

                // 3dcart orders can have an invoice number prefix.
                // So we'll get the InvoiceNumber that doesn't have the prefix from the advanced api query,
                // then add the InvoiceNumberPrefix node so that the downloader can work with both values
                XmlNode invoiceNumberPrefixNode = orderResultXml.OwnerDocument.CreateNode(XmlNodeType.Element, "InvoiceNumberPrefix", orderResultXml.NamespaceURI);
                invoiceNumberPrefixNode.InnerText = invoiceNumberPrefix;
                orderResultXml.AppendChild(invoiceNumberPrefixNode);

                ordersToReturn.Add(orderResultXml);
            }
        }

        /// <summary>
        /// Returns a list of sorted orders
        /// </summary>
        private IEnumerable<XmlNode> SortOrders(List<XmlNode> ordersToReturn, ThreeDCartWebClientOrderSearchCriteria orderSearchCriteria)
        {
            // Now sort the list so that the oldest orders are first
            IOrderedEnumerable<XmlNode> sortedOrders;
            if (orderSearchCriteria.OrderDateSearchType == ThreeDCartWebClientOrderDateSearchType.ModifiedDate)
            {
                sortedOrders = from XmlNode node in ordersToReturn
                               orderby DateTime.Parse(node["LastUpdate"].InnerText)
                               select node;
            }
            else
            {
                sortedOrders = from XmlNode node in ordersToReturn
                               orderby DateTime.Parse(node["Date"].InnerText).Date +
                                       (node["Time"] != null ? DateTime.Parse(node["Time"].InnerText).TimeOfDay : DateTime.MinValue.TimeOfDay)
                               select node;
            }
            return sortedOrders.Select(o => o).ToList();
        }

        /// <summary>
        /// Get the list of 3dcart online order statuses
        /// </summary>
        public List<ThreeDCartOrderStatus> OrderStatuses
        {
            get
            {
                // If we don't have any order statuses, retrieve them
                if (orderStatuses.Count == 0)
                {
                    XmlNode queryOrderStatusesResultXml = null;
                    RequestThrottleParameters requestThrottleArgs = new RequestThrottleParameters(ThreeDCartWebClientApiCall.GetOrderStatuses, null, progressReporter);

                    try
                    {
                        // Make the call to 3dcart to get the statuses
                        using (cartAPIAdvanced apiAdvancedWebService = CreateAdvancedApiWebService("OrderStatuses"))
                        {
                            throttler.ExecuteRequest(requestThrottleArgs, () =>
                            {
                                queryOrderStatusesResultXml = MakeAdvancedCartApiQuery(apiAdvancedWebService, ApiQueryProvider.QueryOrderStatuses, "Order Statuses");
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        throw WebHelper.TranslateWebException(ex, typeof(ThreeDCartException));
                    }

                    // Iterate through each result and add to the list
                    foreach (XmlNode orderStatusNode in queryOrderStatusesResultXml.SelectNodes("//runQueryRecord"))
                    {
                        if (orderStatusNode["StatusText"] != null && orderStatusNode["id"] != null)
                        {
                            int orderStatusID;
                            string orderStatusText = orderStatusNode["StatusText"].InnerText;
                            if (!int.TryParse(orderStatusNode["id"].InnerText, out orderStatusID))
                            {
                                log.Error("ShipWorks was unable to retrieve order statuses from 3dcart.");
                                throw new ThreeDCartException("ShipWorks was unable to retrieve order statuses from 3dcart.");
                            }

                            ThreeDCartOrderStatus threeDCartOrderStatus = new ThreeDCartOrderStatus(orderStatusID, orderStatusText);
                            orderStatuses.Add(threeDCartOrderStatus);
                        }
                        else
                        {
                            log.Error("ShipWorks was unable to retrieve order statuses from 3dcart.  StatusText and/or StatusID was null.");
                            throw new ThreeDCartException("ShipWorks was unable to retrieve order statuses from 3dcart.");
                        }
                    }
                }

                return orderStatuses;
            }
        }

        /// <summary>
        /// Gets a ThreeDCartProductDTO entry from the product cache.  If it doesn't exist, it tries to find the product
        /// online, and store it in the cache if found.  If it can't be found online, null is returned.
        /// </summary>
        /// <param name="invoiceNumber">The invoice number for the order </param>
        /// <param name="threeDCartOrderItemProductId">The 3dcart order item product ID</param>
        /// <param name="threeDCartOrderItemName">The 3dcart order item option value text</param>
        /// <returns>ThreeDCartProductDTO representing the cart product.  Null if an online product can't be found.</returns>
        public ThreeDCartProductDTO GetProduct(long invoiceNumber, string threeDCartOrderItemProductId,
            string threeDCartOrderItemName)
        {
            // If we got a blank product name, just return
            if (string.IsNullOrWhiteSpace(threeDCartOrderItemProductId))
            {
                return null;
            }

            // Build the cache key
            string cacheOrderItemName = threeDCartOrderItemName.ToUpperInvariant().Replace(" ", string.Empty);
            string productCacheKey = $"{store.StoreID}-{threeDCartOrderItemProductId}-{cacheOrderItemName}";

            // See if we've already tried to find this product, but were unable to.
            if (productNotFoundCache.Contains(productCacheKey))
            {
                // We couldn't find it before, so we probably won't now...return null.
                return null;
            }

            // Check to see if the product is in the cache and return it if it is
            ThreeDCartProductDTO foundProductDto = productCache[productCacheKey];
            if (foundProductDto != null)
            {
                return foundProductDto;
            }

            // Get a list of products matching the order item id
            XmlNodeList products = QueryOrderProducts(invoiceNumber, threeDCartOrderItemProductId);


            ThreeDCartProductDTO productDto = LoadProduct(threeDCartOrderItemProductId, threeDCartOrderItemName,
                products, cacheOrderItemName);

            if (productDto != null)
            {
                productCache[productCacheKey] = productDto;
            }
            else
            {
                // Somehow we couldn't find the product at all...maybe it has been deleted
                // Add it to the product NOT FOUND cache so we don't try to find it again until ShipWorks is re-launched
                productNotFoundCache.Add(productCacheKey);
            }

            return productDto;
        }

        /// <summary>
        /// Loads the product.
        /// </summary>
        private ThreeDCartProductDTO LoadProduct(string threeDCartOrderItemProductId, string threeDCartOrderItemName,
            XmlNodeList products, string cacheOrderItemName)
        {
            ThreeDCartProductDTO productDto = null;
            ThreeDCartProductLoader productLoader = new ThreeDCartProductLoader(store);

            // Iterate through each returned product so we can find the one with the appropriate attributes
            foreach (XmlNode sqlQueryProduct in products)
            {
                // Get the REAL product id
                string productId = sqlQueryProduct["id"].InnerText;

                // Now make a call to the regular api to get the full product info
                XmlNode getProductResponseXml = GetProducts(ThreeDCartConstants.MaxResultsToReturn, 1, productId);

                CheckResponseForErrors(getProductResponseXml, productId);

                // Load product details, will return null if this product does not contain the appropriate attributes
                productDto = productLoader.LoadProduct(threeDCartOrderItemProductId,
                    threeDCartOrderItemName,
                    cacheOrderItemName, getProductResponseXml);

                if (productDto != null)
                {
                    break;
                }
            }

            return productDto;
        }

        /// <summary>
        /// Find the real product id based on the order item id.
        /// </summary>
        /// <remarks>
        /// The regular cart api does not return the actual product id; it returns the item id, but calls it ProductID.
        /// A call to getProduct using the item id yields no results
        /// So, we'll do an advanced sql query to find the real product id based on the order item id.
        /// </remarks>
        private XmlNodeList QueryOrderProducts(long invoiceNumber, string threeDCartOrderItemProductId)
        {
            XmlNode productQueryResultXml = null;
            RequestThrottleParameters requestThrottleArgs = new RequestThrottleParameters(
                ThreeDCartWebClientApiCall.GetProduct, null, progressReporter);

            using (cartAPIAdvanced advancedApiWebService = CreateAdvancedApiWebService(
                $"GetProduct ({threeDCartOrderItemProductId})"))
            {
                string queryProductSql = ApiQueryProvider.QueryProductsByItemID(invoiceNumber, threeDCartOrderItemProductId);

                throttler.ExecuteRequest(requestThrottleArgs,
                    () =>
                    {
                        productQueryResultXml = MakeAdvancedCartApiQuery(advancedApiWebService, queryProductSql, "QueryProduct");
                    });
            }

            return productQueryResultXml.SelectNodes("//runQueryRecord");
        }

        /// <summary>
        /// Get a batch of products for the 3dcart store
        /// </summary>
        /// <param name="batchSize">Number of products to return</param>
        /// <param name="startNumber">Index of page in total number of products from which to start</param>
        /// <param name="productId">Product Id for which to specifically search</param>
        /// <returns>Xml Node containing product nodes in the batch, matching criteria</returns>
        private XmlNode GetProducts(int batchSize, int startNumber, string productId)
        {
            XmlNode productsXml = null;
            RequestThrottleParameters requestThrottleArgs = new RequestThrottleParameters(ThreeDCartWebClientApiCall.GetProducts, null, progressReporter);

            string cacheProductId = productId.ToUpperInvariant().Replace(" ", string.Empty);
            string productCacheKey = $"GetProducts:{store.StoreID}-{productId}-{cacheProductId}";

            // See if we've already tried to find this product, but were unable to.
            if (productNotFoundCache.Contains(productCacheKey))
            {
                // We couldn't find it before, so we probably won't now...return null.
                return null;
            }

            // Check to see if the product is in the cache and return it if it is
            XmlNode foundProductDto = getProductsCache[productCacheKey];
            if (foundProductDto != null)
            {
                return foundProductDto;
            }

            try
            {
                // Make the call to get the products by criteria
                using (cartAPI api = CreateApiWebService($"GetProducts ({batchSize},{startNumber},{productId})"))
                {
                    throttler.ExecuteRequest(requestThrottleArgs, () =>
                    {
                        productsXml = api.getProduct(store.StoreDomain, store.ApiUserKey, batchSize, startNumber, productId, string.Empty);
                    });

                    // 3dcart seems to be double decoding the xml requests, so if we get back a not well-formed error, try the request again
                    // but double encode the product id.  We end up sending &lt;![CDATA[foo&amp;bar]]&gt; instead of foo&amp;bar, which is what it should be
                    if (ResponseIsNotWellFormedError(productsXml))
                    {
                        throttler.ExecuteRequest(requestThrottleArgs, () =>
                        {
                            productsXml = api.getProduct(store.StoreDomain, store.ApiUserKey, batchSize, startNumber,
                                $"<![CDATA[{productId}]]>", string.Empty);
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(ThreeDCartException));
            }

            // Check to see if we got a null response, and throw if we did
            if (productsXml == null)
            {
                productNotFoundCache.Add(productCacheKey);
                log.Error("A null response was received from 3dcart when calling getProduce from the api.");
                throw new ThreeDCartException("ShipWorks encountered an error requesting products from your 3dcart.");
            }

            getProductsCache[productCacheKey] = productsXml;

            return productsXml;
        }

        /// <summary>
        /// Returns whether the xml contains a not well formed error
        /// </summary>
        private static bool ResponseIsNotWellFormedError(XmlNode productsXml)
        {
            XElement errorElement = productsXml.ToXElement().XPathSelectElement("//Error/Description");
            return errorElement != null && errorElement.Value.ToUpperInvariant().Contains("not well-formed");
        }

        /// <summary>
        /// Wrapper method to make calls to the advanced cart api.  It handles checking for error node
        /// </summary>
        /// <param name="advancedCartApi">The cartAPIAdvanced instance to use</param>
        /// <param name="sqlQuery">The sql query to run</param>
        /// <param name="queryType">The type of query to run "GetOrderCount", "GetOrder", etc...</param>
        /// <returns></returns>
        private XmlNode MakeAdvancedCartApiQuery(cartAPIAdvanced advancedCartApi, string sqlQuery, string queryType)
        {
            XmlNode apiRunQueryResultXml;
            try
            {
                // Make the call to the advanced api
                apiRunQueryResultXml = advancedCartApi.runQuery(store.StoreDomain, store.ApiUserKey, sqlQuery, string.Empty);
                ValidateAdvancedCartApiQueryResponse(apiRunQueryResultXml, queryType);
            }
            catch (InvalidOperationException ex)
            {
                // FogBugz crash 262171.  The api returned invalid xml, which was converted into an InvalidOperationException with
                // an inner exception of XmlException.  Check for these and don't crash ShipWorks, but give the user a fairly nice
                // error message.
                if (ex.InnerException is XmlException)
                {
                    throw new ThreeDCartException("An invalid response was returned from the 3dcart store. Please try again later.", ex);
                }

                // If it's not an inner exception of XmlException type, translate and throw.
                throw WebHelper.TranslateWebException(ex, typeof(ThreeDCartException));
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(ThreeDCartException));
            }

            // Return the result xml
            return apiRunQueryResultXml;
        }

        /// <summary>
        /// Checks apiRunQueryResultXml for validity
        /// </summary>
        /// <param name="apiRunQueryResultXml">The response from an advanced cart api call</param>
        /// <param name="queryType">The type of query that was run "GetOrderCount", "GetOrder", etc...</param>
        /// <exception cref="ThreeDCartException" />
        private static void ValidateAdvancedCartApiQueryResponse(XmlNode apiRunQueryResultXml, string queryType)
        {
            // Check to see if their api returned a null object
            if (apiRunQueryResultXml == null)
            {
                log.Error("ShipWorks did not receive an xml response from 3dcart advanced api call.  See ApiLog Request for more detail.");
                throw new ThreeDCartException("ShipWorks encountered an error while attempting to contact 3dcart.");
            }

            // Check to see if there is an error node
            XmlNode errorNode = GetErrorFromResponse(apiRunQueryResultXml);
            if (errorNode != null)
            {
                // If no results were returned, let the caller decide what to do
                // "<Error><Id>99</Id><Description>SELECT statement returned no records</Description></Error>"
                bool noRecordsFound = errorNode.SelectSingleNode("//Id") != null && errorNode.SelectSingleNode("//Id")?.InnerText == "99";

                if (!noRecordsFound)
                {
                    // Create an inner exception from the response xml
                    ThreeDCartException threeDCartException = new ThreeDCartException(apiRunQueryResultXml.OuterXml);

                    // Create an error message for the log file
                    string logErrorMessage =
                        $"ShipWorks encountered an error while making an advanced api query of type '{queryType}'.";
                    log.Error(logErrorMessage, threeDCartException);

                    // Throw a new 3dcart exception for the user
                    throw new ThreeDCartException("ShipWorks encountered an error while attempting to contact 3dcart.", errorNode);
                }
            }
        }

        /// <summary>
        /// Checks xml from a cart call for validity
        /// </summary>
        /// <param name="apiResponse">The response from an advanced cart api call</param>
        /// <param name="queryType">The type of query that was run "GetOrderCount", "GetOrder", etc...</param>
        /// <exception cref="ThreeDCartException" />
        private static void ValidateCartApiQueryResponse(XmlNode apiResponse, string queryType)
        {
            // Check to see if their api returned a null object
            if (apiResponse == null)
            {
                log.Error("ShipWorks did not receive an xml response from 3dcart api call.  See ApiLog Request for more detail.");
                throw new ThreeDCartException("ShipWorks encountered an error while attempting to contact 3dcart.");
            }

            // Check to see if there is an error node
            XmlNode errorNode = GetErrorFromResponse(apiResponse);
            if (errorNode != null)
            {
                // If no results were returned, let the caller decide what to do
                // "<Error><Id>49</Id><Description>No Data Found</Description></Error>"
                bool noRecordsFound = errorNode.SelectSingleNode("//Id") != null && errorNode.SelectSingleNode("//Id")?.InnerText == "49";

                if (!noRecordsFound)
                {
                    // Create an inner exception from the response xml
                    ThreeDCartException threeDCartException = new ThreeDCartException(apiResponse.OuterXml);

                    // Create an error message for the log file
                    string logErrorMessage =
                        $"ShipWorks encountered an error while making an api query of type '{queryType}'.";
                    log.Error(logErrorMessage, threeDCartException);

                    // Throw a new 3dcart exception for the user
                    throw new ThreeDCartException("ShipWorks encountered an error while attempting to contact 3dcart.", errorNode);
                }
            }
        }

        /// <summary>
        /// Attempt to get an order count to test connecting to ThreeDCart.  If any error, assume connection failed.
        /// </summary>
        public void TestConnection()
        {
            XmlNode orderCountResultNode = null;
            RequestThrottleParameters requestThrottleArgs = new RequestThrottleParameters(ThreeDCartWebClientApiCall.TestConnection, null, progressReporter);

            // See if we can successfully call getOrderCount, if we throw, we can't connect or login
            try
            {
                // Using the standard cart api so we don't have to determine the version of sql server being used
                using (cartAPI api = CreateApiWebService("TestConnection"))
                {
                    // Ask for invoice number 1, so we only get a max of 1 order coming back.  If there is no invoice number 1, it's still a successful call.
                    throttler.ExecuteRequest(requestThrottleArgs, () =>
                    {
                        orderCountResultNode = api.getOrderCount(store.StoreDomain, store.ApiUserKey, false, "1", string.Empty, string.Empty, string.Empty, string.Empty);
                    });

                    // If there was an error, an Error node will be returned.
                    XmlNode errorNode = GetErrorFromResponse(orderCountResultNode);
                    if (errorNode != null)
                    {
                        throw new ThreeDCartException("ShipWorks was unable to connect to 3dcart with the Store Url and API User Key provided.", errorNode);
                    }
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(ThreeDCartException));
            }
        }

        /// <summary>
        /// Checks the response for errors.
        /// </summary>
        private void CheckResponseForErrors(XmlNode getProductResponseXml, string productId)
        {
            XmlNode errorNode = GetErrorFromResponse(getProductResponseXml);
            if (errorNode != null)
            {
                log.ErrorFormat(
                    $"ShipWorks was unable to get product information from 3dcart. Error Message: {errorNode.OuterXml}.");
                string errorMessage =
                    $"ShipWorks was unable to get product information from 3dcart for product ID/Sku '{productId}'";
                throw new ThreeDCartException(errorMessage, errorNode);
            }
        }
    }
}
