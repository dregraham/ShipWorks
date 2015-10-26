using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.ThreeDCart.Enums;
using log4net;
using ShipWorks.ApplicationCore.Logging;

namespace ShipWorks.Stores.Platforms.ThreeDCart
{
    /// <summary>
    /// Downloader for 3d Cart stores
    /// </summary>
    public class ThreeDCartDownloader : StoreDownloader
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ThreeDCartDownloader));
        int totalCount;
        ThreeDCartWebClient webClient;
        readonly ThreeDCartStoreEntity threeDCartStore;
        const int MissingCustomerID = 0;
        DateTime lastModifiedOrderDateProcessed;

        // provider for status codes
        ThreeDCartStatusCodeProvider statusProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="store">Store for which this downloader will operate</param>
        public ThreeDCartDownloader(ThreeDCartStoreEntity store)
            : base(store)
        {
            threeDCartStore = store;
            totalCount = 0;
        }

        /// <summary>
        /// The web client used to download
        /// </summary>
        private ThreeDCartWebClient WebClient
        {
            get
            {
                if (webClient == null)
                {
                    //Create the web client used for downloading
                    webClient = new ThreeDCartWebClient((ThreeDCartStoreEntity)Store, Progress);
                }

                return webClient;
            }
        }

        /// <summary>
        /// Download orders and statuses for the 3D Cart store
        /// </summary>
        protected override void Download()
        {
            try
            {
                // Update the orders statuses from online
                UpdateOrderStatuses();

                Progress.Detail = "Checking for new orders...";

                // Get the number of orders for the range
                ThreeDCartWebClientOrderSearchCriteria orderSearchCriteria =
                    GetOrderSearchCriteria(ThreeDCartWebClientOrderDateSearchType.CreatedDate);

                totalCount = WebClient.GetOrderCount(orderSearchCriteria);

                if (totalCount != 0)
                {
                    // Download any newly created orders (thier modified dates could be older than the last last modified date SW has)
                    DownloadOrders(orderSearchCriteria);
                }

                if (threeDCartStore.DownloadModifiedNumberOfDaysBack > 0)
                {
                    // Now Download modified orders
                    Progress.Detail = "Checking for modified orders...";
                    lastModifiedOrderDateProcessed = DateTime.UtcNow.AddDays(-7);
                    orderSearchCriteria =
                        GetOrderSearchCriteria(ThreeDCartWebClientOrderDateSearchType.ModifiedDate);

                    int modifiedCount = WebClient.GetOrderCount(orderSearchCriteria);
                    if (modifiedCount == 0)
                    {
                        Progress.Detail = "No orders to download.";
                        Progress.PercentComplete = 100;
                        return;
                    }

                    // Update the total count to included the modified orders
                    totalCount += modifiedCount;

                    // Update the progress bar for the new count
                    Progress.PercentComplete = QuantitySaved/totalCount;

                    // Download the modified orders
                    DownloadOrders(orderSearchCriteria);
                }
            }
            catch (ThreeDCartException ex)
            {
                log.Error(ex);
                throw new DownloadException(ex.Message, ex);
            }
            catch (SqlForeignKeyException ex)
            {
                log.Error(ex);
                throw new DownloadException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw WebHelper.TranslateWebException(ex, typeof(DownloadException));
            }
        }

        /// <summary>
        /// Update the local order status provider
        /// </summary>
        private void UpdateOrderStatuses()
        {
            Progress.Detail = "Updating status codes...";

            // refresh the status codes from 3D Cart
            statusProvider = new ThreeDCartStatusCodeProvider((ThreeDCartStoreEntity)Store);
            statusProvider.UpdateFromOnlineStore();
        }

        /// <summary>
        /// Download orders for specified order search criteria
        /// </summary>
        /// <param name="orderSearchCriteria"></param>
        private void DownloadOrders(ThreeDCartWebClientOrderSearchCriteria orderSearchCriteria)
        {
            while (true)
            {
                if (!DownloadOrderRange(orderSearchCriteria))
                {
                    return;
                }

                // Check for cancel
                if (Progress.IsCancelRequested)
                {
                    return;
                }

                // Download the next range of orders
                orderSearchCriteria = GetOrderSearchCriteria(orderSearchCriteria.OrderDateSearchType);
            }
        }

        /// <summary>
        /// Download the next range of orders. 
        /// </summary>
        /// <param name="orderSearchCriteria">The ThreeDCartWebClientOrderSearchCriteria for which to download orders.</param>
        /// <returns>True if orders were loaded, false if the user clicks cancel or if no orders were left to download</returns>
        private bool DownloadOrderRange(ThreeDCartWebClientOrderSearchCriteria orderSearchCriteria)
        {
            // Check for cancel
            if (Progress.IsCancelRequested)
            {
                return false;
            }

            // Update progress reporter that we are now downloading orders
            Progress.Detail = string.Format("Downloading {0} orders...", 
                orderSearchCriteria.OrderDateSearchType == ThreeDCartWebClientOrderDateSearchType.CreatedDate ? "new" : "modified");

            // Download the orders to process
            // The web client will use SQL to get the correct number of orders in this "page" to process
            List<XmlNode> orders = WebClient.GetOrders(orderSearchCriteria); 

            // Check to see that we received some orders to process (just in case another ShipWorks instance downloaded them before we got to them)
            if (orders.Count == 0)
            {
                Progress.Detail = "No additional orders to download.";
                Progress.PercentComplete = 100;
                return false;
            }

            // Return the result of loading the orders.
            return LoadOrders(orders);
        }

        /// <summary>
        /// Gets the order search criteria 
        /// </summary>
        /// <returns>ThreeDCartWebClientOrderSearchCriteria </returns>
        private ThreeDCartWebClientOrderSearchCriteria GetOrderSearchCriteria(ThreeDCartWebClientOrderDateSearchType orderDateSearchType)
        {
            // Getting last online modified starting point
            DateTime? modifiedDateStartingPoint;
            if (lastModifiedOrderDateProcessed == DateTime.MinValue)
            {
                modifiedDateStartingPoint = GetOnlineLastModifiedStartingPoint();
                if (!modifiedDateStartingPoint.HasValue)
                {
                    modifiedDateStartingPoint = DateTime.UtcNow.AddDays(-7);
                }
            }
            else
            {
                modifiedDateStartingPoint = lastModifiedOrderDateProcessed;
            }

            // Add 1 second
            modifiedDateStartingPoint = modifiedDateStartingPoint.Value.AddSeconds(1);

            // Set end date to now
            DateTime modifiedDateEndPoint = DateTime.UtcNow;

            DateTime? createdDateStartingPoint = GetOrderDateStartingPoint();
            
            // If the date has a value, add 1 second
            createdDateStartingPoint = createdDateStartingPoint.HasValue ? createdDateStartingPoint.Value.AddSeconds(1) : DateTime.UtcNow.AddYears(-10);
            
            // Set end date to now
            DateTime createdDateEndPoint = DateTime.UtcNow;

            ThreeDCartWebClientOrderSearchCriteria orderSearchCriteria = new ThreeDCartWebClientOrderSearchCriteria(orderDateSearchType,
                modifiedDateStartingPoint.Value, modifiedDateEndPoint, 
                createdDateStartingPoint.Value, createdDateEndPoint,
                threeDCartStore, ThreeDCartConstants.MaxResultsToReturn, 1);

            // Create the tuple to return
            return orderSearchCriteria;
        }

        /// <summary>
        /// Load all the orders contained in the list
        /// </summary>
        /// <returns>True, if all orders were loaded.  False if the user pressed cancel</returns>
        private bool LoadOrders(List<XmlNode> orderNodes)
        {
            // Go through each order in the XML Document
            foreach (XmlNode order in orderNodes)
            {
                // Check for user cancel
                if (Progress.IsCancelRequested)
                {
                    return false;
                }

                // Update the status
                Progress.Detail = string.Format("Processing order {0}...", (QuantitySaved + 1));

                XPathNavigator orderXPathNavigator = order.CreateNavigator();

                // Default to 1, so that when creating orders with sub orders, they will have the format
                // prefixOrder-ShipmentIndex
                int shipmentIndex = 1;

                // Get the list of shipments on this order, sorted so that we always process the orders in the same order
                XPathExpression getSortedShipmentsExpression = orderXPathNavigator.Compile("./ShippingInformation/Shipment");
                getSortedShipmentsExpression.AddSort("ShipmentID", XmlSortOrder.Ascending, XmlCaseOrder.None, "", XmlDataType.Number);
                XPathNodeIterator shipmentNodes = orderXPathNavigator.Select(getSortedShipmentsExpression);

                // The first order will always NOT be a sub order
                bool isSubOrder = false;
                bool hasSubOrders = shipmentNodes.Count > 1;
                while (shipmentNodes.MoveNext())
                {
                    XPathNavigator shipmentNode = shipmentNodes.Current.Clone();

                    string invoiceNumberPostFix = hasSubOrders ? string.Format("-{0}", shipmentIndex) : string.Empty;

                    LoadOrder(orderXPathNavigator, shipmentNode, invoiceNumberPostFix, isSubOrder, hasSubOrders);

                    shipmentIndex++;

                    isSubOrder = true;
                }

                //Update the PercentComplete
                Progress.PercentComplete = 100 * QuantitySaved / totalCount;
            }

            return true;
        }

        /// <summary>
        /// Extract and save the order from the XML
        /// </summary>
        private void LoadOrder(XPathNavigator xmlOrderXPath, XPathNavigator shipmentNode, string invoiceNumberPostFix, bool isSubOrder, bool hasSubOrders)
        {
            // Create a ThreeDCartOrderIdentifier
            ThreeDCartOrderIdentifier threeDCartOrderIdentifier = CreateOrderIdentifier(xmlOrderXPath, invoiceNumberPostFix);

            // Get the order instance.
            OrderEntity order = InstantiateOrder(threeDCartOrderIdentifier);

            // Get the order total from the xml
            decimal xmlOrderTotal = XPathUtility.Evaluate(xmlOrderXPath, "./Total", 0.0m);

            // If this order does not have sub orders, set the order total to that which we received from 3D Cart
            // If it does have sub orders, we'll calculate the order total after we add items for this shipment/charges/payment
            if (order.IsNew && !hasSubOrders)
            {
                // Set the total.  It will be calculated and verified later.
                order.OrderTotal = xmlOrderTotal;
            }

            // Get the customer
            int onlineCustomerID = XPathUtility.Evaluate(xmlOrderXPath, "./CustomerID", MissingCustomerID);
            if (onlineCustomerID <= MissingCustomerID)
            {
                order.OnlineCustomerID = null;
            }
            else
            {
                order.OnlineCustomerID = onlineCustomerID;
            }

            // Requested shipping
            order.RequestedShipping = XPathUtility.Evaluate(shipmentNode, "./Method", "");

            // Get order date.  The date and time is split between two nodes, so combine them.
            // Then convert to UTC based on the store's time zone.
            DateTime orderDay = DateTime.Parse(XPathUtility.Evaluate(xmlOrderXPath, "./Date", ""));
            DateTime orderTime = DateTime.Parse(XPathUtility.Evaluate(xmlOrderXPath, "./Time", ""));
            TimeSpan orderTimeTimeSpan = new TimeSpan(orderTime.Hour, orderTime.Minute, orderTime.Second);
            DateTime storeDateTimeConversion = orderDay.Add(orderTimeTimeSpan);
            order.OrderDate = DateTimeUtility.ConvertTimeToUtcForTimeZone(storeDateTimeConversion,
                                                                          threeDCartStore.StoreTimeZone);

            // Set OnlineLastModified to their last modified date
            // Convert to UTC based on the store's time zone.
            storeDateTimeConversion = DateTime.Parse(XPathUtility.Evaluate(xmlOrderXPath, "./LastUpdate", ""));
            order.OnlineLastModified = DateTimeUtility.ConvertTimeToUtcForTimeZone(storeDateTimeConversion,
                                                                          threeDCartStore.StoreTimeZone);

            //Load Online Status
            LoadStatus(order, xmlOrderXPath);

            //Load address info
            LoadAddressInfo(order, xmlOrderXPath, shipmentNode);

            // Load any order notes
            LoadOrderNotes(order, xmlOrderXPath);

            // Only update the rest for brand new orders
            if (order.IsNew)
            {
                int shipmentId = XPathUtility.Evaluate(shipmentNode, "./ShipmentID", 0);

                // Iterate through each item
                // Added the .ToList() to avoid multiple enumeration of IEnumerable
                var orderItemsForThisShipment =
                    (from XPathNavigator item in xmlOrderXPath.Select("./ShippingInformation/OrderItems/Item")
                    where XPathUtility.Evaluate(item, "./ShipmentID", 0) == shipmentId
                    select item).ToList();

                foreach (var orderItem in orderItemsForThisShipment)
                {
                    //LoadItem for line item
                    LoadItem(order, orderItem);
                }

                // If this order is not a sub order, add the charges and payment details
                if (!isSubOrder)
                {
                    //Load all the charges
                    LoadOrderCharges(order, xmlOrderXPath);

                    //Load all payment details
                    LoadPaymentDetails(order, xmlOrderXPath);
                }

                // Calculate the order total
                decimal total = OrderUtility.CalculateTotal(order);

                // Find any kit items
                var kitProductNames =
                    from XPathNavigator item in orderItemsForThisShipment
                    where XPathUtility.Evaluate(item, "./ProductName", string.Empty).StartsWith("KIT ITEM:",StringComparison.OrdinalIgnoreCase)
                    select item;

                // If the order contains kit items, check to see if the order total doesn't match the xml order total
                // If they don't match, add a "Kit Adjustment" to make the order total correct
                if (kitProductNames.Any() && xmlOrderTotal != total)
                {
                    AddKitAdjustment(order, xmlOrderTotal - total);
                }
             
                // If this is a sub order, or it's the first order that has sub orders, calculate the total 
                if (isSubOrder || hasSubOrders)
                {
                    order.OrderTotal = total;
                }
            }

            // If this is a sub order, increment the total count
            if (isSubOrder)
            {
                totalCount++;
            }

            //Save the downloaded order
            SqlAdapterRetry<SqlException> retryAdapter = new SqlAdapterRetry<SqlException>(5, -5, "ThreeDCartDownloader.LoadOrder");
            retryAdapter.ExecuteWithRetry(() => SaveDownloadedOrder(order));

            lastModifiedOrderDateProcessed = order.OnlineLastModified;
        }

        /// <summary>
        /// Creates a 3D Cart order identifier for the current order
        /// </summary>
        /// <param name="xpath">Order XPathNavigator</param>
        /// <param name="invoiceNumberPostFix">For multi shipment orders, add this as the order number post fix</param>
        /// <returns>3D Cart order identifier for the current order</returns>
        private ThreeDCartOrderIdentifier CreateOrderIdentifier(XPathNavigator xpath, string invoiceNumberPostFix)
        {
            // Now extract the Invoice number and ThreeDCart Order Id
            long orderId = XPathUtility.Evaluate(xpath, "./OrderID", 0L);

            // Invoice number is defined as an integer in the 3D Cart schema
            // So we can safely remove the prefix to get to a long
            long invoiceNum = 0;
            string invoiceNumber = XPathUtility.Evaluate(xpath, "./InvoiceNumber", string.Empty);
            string invoiceNumberPrefix = XPathUtility.Evaluate(xpath, "./InvoiceNumberPrefix", string.Empty);

            // I've seen invoice number as blank in one of the 3D Cart test stores...  so instead of blank, we'll put the 3D Cart Order ID
            if (string.IsNullOrWhiteSpace(invoiceNumber))
            {
                invoiceNumber = orderId.ToString();
            }
            else if (!string.IsNullOrWhiteSpace(invoiceNumberPrefix))
            {
                // 3d Cart allows you to add a prefix to the invoice number.
                // The legacy order importer stripped the prefix, so we'll do that here too.
                invoiceNumber = invoiceNumber.Replace(invoiceNumberPrefix, string.Empty);
            }

            if (!long.TryParse(invoiceNumber, out invoiceNum))
            {
                log.ErrorFormat("3D Cart returned an invalid invoice number: {0}.", invoiceNumber);
                throw new ThreeDCartException("3D Cart returned an invalid response while downloading orders");
            }

            // Create an order identifier without a prefix.  If we find an order, it must have been downloaded prior to
            // the upgrade.  If an order is found, we will not use the prefix.  If we don't find an order, we'll use the prefix.
            ThreeDCartOrderIdentifier threeDCartOrderIdentifier = new ThreeDCartOrderIdentifier(invoiceNum, string.Empty, invoiceNumberPostFix);
            OrderEntity order = FindOrder(threeDCartOrderIdentifier);
            if (order == null)
            {
                threeDCartOrderIdentifier = new ThreeDCartOrderIdentifier(invoiceNum, invoiceNumberPrefix, invoiceNumberPostFix);
            }

            return threeDCartOrderIdentifier;
        }

        /// <summary>
        /// Extract and set the status from the order
        /// </summary>
        private void LoadStatus(OrderEntity order, XPathNavigator xpath)
        {
            // Assume the onlineStatus is open.  We'll change it if necessary below.
            string onlineStatus = XPathUtility.Evaluate(xpath, "./OrderStatus", "");

            //Add Online Status
            order.OnlineStatusCode = statusProvider.GetCodeValue(onlineStatus);
            order.OnlineStatus = onlineStatus;
        }

        /// <summary>
        /// Load the item information into the order
        /// </summary>
        private void LoadItem(OrderEntity order, XPathNavigator xpath)
        {
            ThreeDCartOrderItemEntity item = (ThreeDCartOrderItemEntity) InstantiateOrderItem(order);

            // Set item properties
            // The item name will be determined in the LoadProductAndRelatedObjects as we 
            // have to parse the product name for options and the name itself
            item.Code = XPathUtility.Evaluate(xpath, "./ProductID", "");
            item.SKU = item.Code; 
            item.Quantity = XPathUtility.Evaluate(xpath, "./Quantity", 0d);
            item.UnitCost = XPathUtility.Evaluate(xpath, "./UnitCost", 0.0m);
            item.Weight = XPathUtility.Evaluate(xpath, "./Weight", 0.0d);

            // Save the 3D Cart ShipmentID with which this item is associated
            item.ThreeDCartShipmentID = XPathUtility.Evaluate(xpath, "./ShipmentID", 0);
                
            //Now load all the item options
            LoadProductAndRelatedObjects(item, xpath);

            // Sometimes 3D Cart doesn't send a unit price, but does send option price.
            // So, we'll get the unitPrice, optionPrice, and sum the amount of order item attributes loaded and
            // set the optionPrice equal to the amount we received from 3D Cart less order item attributes sum.
            // From this we'll set the item's unit price equal to the new calculated option price.
            decimal unitPrice = XPathUtility.Evaluate(xpath, "./UnitPrice", 0.0m);
            decimal orderItemAttributeTotalUnitPrice = item.OrderItemAttributes.Sum(a => a.UnitPrice);
            decimal optionPrice = XPathUtility.Evaluate(xpath, "./OptionPrice", 0.0m) - orderItemAttributeTotalUnitPrice;

            item.UnitPrice = unitPrice + optionPrice;
        }

        /// <summary>
        /// Load the option(s) of the given item
        /// </summary>
        private void LoadProductAndRelatedObjects(OrderItemEntity item, XPathNavigator xpath)
        {
            ThreeDCartProductDTO product = null;
            const RegexOptions regexOptions = RegexOptions.IgnoreCase | RegexOptions.Multiline;
            const string newLine = @"\n";
            
            string productName = XPathUtility.Evaluate(xpath, "./ProductName", "");
            string productId = XPathUtility.Evaluate(xpath, "./ProductID", "");

            /*
             * The item's product name text contains multiple lines, with the first line being the
             * actual product name, then the next lines are the attributes, one per line.
             * 
				<ProductName>
					<![CDATA[Custom Cap
                             Size: Small - $1.00]]>
				</ProductName>
             */
            string[] splitProductName = Regex.Split(productName, newLine, regexOptions);
            if (splitProductName.Count() > 1)
            {
                // The first line has the name of the product without option info
                item.Name = splitProductName[0];

                // Process the product name lines, skipping the first line since it is the product name, not an attribute
                for (int i = 1; i <= splitProductName.Count() - 1; i++)
                {
                    string optionLine = splitProductName[i];
                    decimal optionPrice = 0.0m;
                    string optionName = string.Empty;
                    string optionDescription = string.Empty;
                    bool addOption = true;

                    // To parse, we need 1 colon and 1 dash
                    int numberOfDashes = optionLine.Count(f => f == '-');
                    int numberOfColons = optionLine.Count(f => f == ':');

                    // Sometimes the name will have "- $x.xx" pricing...  so if there is only 1 dash, 
                    // take the left of the dash as the name of the option.  Otherwise use the full option line
                    product = WebClient.GetProduct(item.Order.OrderNumber, productId, numberOfDashes == 1 ? optionLine.Split('-')[0] : optionLine);
                    if (product != null)
                    {
                        // See if there were any options loaded.  If so, set them on option variables
                        if (!string.IsNullOrWhiteSpace(product.OptionName))
                        {
                            optionName = product.OptionName;
                            optionDescription = product.OptionDescription;
                            optionPrice = product.OptionPrice;
                        }
                        else
                        {
                            // No options were loaded, so it's just the product name
                            addOption = false;
                            item.Name = productName;
                            item.Thumbnail = product.ImageThumbnail;
                            item.Image = product.ImageUrl;
                        }
                    }
                    else
                    {
                        if (numberOfDashes > 1 || numberOfColons > 1)
                        {
                            // The cart admin must have put dashes in the option and/or value name
                            // This makes it very difficult to try and determine the value, so just
                            // set the name to the full line.
                            optionName = optionLine;
                            optionDescription = string.Empty;
                        }
                        else
                        {
                            // Split on the : and if there is more than two parts, set the name and description,
                            // Otherwise just set the name to the whole thing
                            string[] nameSplit = optionLine.Split(':');
                            if (nameSplit.Count() > 1)
                            {
                                optionName = nameSplit[0];
                                optionDescription = nameSplit[1].Split('-')[0];
                            }
                            else
                            {
                                optionName = nameSplit[0];
                                optionDescription = string.Empty;
                            }
                        }
                    }

                    // The item price could be different from the product xml option price since the item is a snapshot in time.
                    // So check to see if we have a single dash in the option line, and if so, try to parse out the item price.
                    if (numberOfDashes == 1)
                    {
                        string[] priceSplit = optionLine.Split('-');
                        string optionPriceString = priceSplit[1];

                        // Leaving in &# in case the user put html entities...and this really isn't a price
                        optionPriceString = Regex.Replace(optionPriceString, "[^.0-9A-Za-z&#/@%^&*()-_~|]", "");

                        // Try to get the item's price.  If it fails, we want it to stay at either it's default value
                        // or the value that the Products xml option price
                        if (!decimal.TryParse(optionPriceString, out optionPrice))
                        {
                            // We weren't able to get a real decimal from the optionPriceString, so there
                            // must have been a dash in the string that wasn't a price.
                            // So set the option description to the text to the right of the :
                            optionDescription = numberOfColons > 0 ? optionLine.Split(':')[1] : optionLine;
                        }
                    }

                    if (addOption)
                    {
                        //Instantiate the order item attribute
                        OrderItemAttributeEntity optionToAdd = InstantiateOrderItemAttribute(item);
                        optionToAdd.Name = optionName.Trim();
                        optionToAdd.Description = optionDescription.Trim();
                        optionToAdd.UnitPrice = optionPrice;
                    }
                }
            }
            else
            {
                // There weren't multiple lines in the productName, so this must be the regular product name
                // without options.
                item.Name = productName;
                product = WebClient.GetProduct(item.Order.OrderNumber, productId, string.Empty);

                if (product != null)
                {
                    item.Thumbnail = product.ImageThumbnail;
                    item.Image = product.ImageUrl;
                }
            }

			// Set the item location if we found a product
            if (product != null)
            {
                item.Location = product.WarehouseBin;
            }

            decimal orderItemAttributeTotalUnitPrice = item.OrderItemAttributes.Sum(a => a.UnitPrice);
            decimal xmlItemOptionPrice = XPathUtility.Evaluate(xpath, "./OptionPrice", 0.0m);
            if (orderItemAttributeTotalUnitPrice != xmlItemOptionPrice)
            {
                // The calculated order item attribute unit prices don't match what the XML says the total option price should be
                log.Debug("The calculated order item attribute unit prices don't match what the XML says the total option price should be");
            }
        }

        /// <summary>
        /// Add a kit adjustment to the order
        /// </summary>
        private void AddKitAdjustment(OrderEntity order, decimal kitAdjustmentAmount)
        {
            // Charge - kit adjustment
            if (kitAdjustmentAmount > 0.0m)
            {
                //Load charge for the kit adjustment
                LoadCharge(order, "Kit Adjustment", "Kit Adjustment", kitAdjustmentAmount);
            }
        }

        /// <summary>
        /// Load all the charges for the order
        /// </summary>
        private void LoadOrderCharges(OrderEntity order, XPathNavigator orderXPath)
        {
            // Charge - Promotions and Discounts
            LoadOrderPromotionAndDiscountCharges(order, orderXPath);

            //Iterate through each gift cert
            XPathNodeIterator giftCertificateNodes = orderXPath.Select("./GiftCertificateUsed/Gift");
            while (giftCertificateNodes.MoveNext())
            {
                XPathNavigator giftCertificate = giftCertificateNodes.Current.Clone();

                decimal giftCertificateAmount = -XPathUtility.Evaluate(giftCertificate, "./Amount", 0.0m);

                //Load charge for each gift certificate
                LoadCharge(order, "Gift Certificate", 
                    XPathUtility.Evaluate(giftCertificate, "./Code", "Unknown Gift Certificate Code"),
                    giftCertificateAmount);

                if (order.OrderTotal > 0)
                {
                    order.OrderTotal += giftCertificateAmount;
                }
            }

            // Load Tax, even if it's $0.00, we still want to show there was no tax
            decimal rate = XPathUtility.Evaluate(orderXPath, "Tax", 0.0m);
            LoadCharge(order, "Tax", "Tax", rate);

            // Load Tax2, but don't save if it's $0
            rate = XPathUtility.Evaluate(orderXPath, "Tax2", 0.0m);
            if (rate > 0.0m)
            {
                LoadCharge(order, "Tax", "Tax2", rate);
            }

            // Load Tax3, but don't save if it's $0
            rate = XPathUtility.Evaluate(orderXPath, "Tax3", 0.0m);
            if (rate > 0.0m)
            {
                LoadCharge(order, "Tax", "Tax3", rate);
            }

            //Find the first shippling_lines
            decimal shippingAmount = XPathUtility.Evaluate(orderXPath, "Shipping", 0.0m);
            if (shippingAmount > 0.0m)
            {
                // Create a list of each shipping method, comma delimited
                XPathNodeIterator shipmentMethods = orderXPath.Select("./ShippingInformation/Shipment/Method");
                string shippingMethod = string.Empty;
                while (shipmentMethods.MoveNext())
                {
                    shippingMethod += string.Format("{0}{1}", 
                        shipmentMethods.Current.Value, 
                        shipmentMethods.CurrentPosition < shipmentMethods.Count ? ", " : string.Empty);
                }

                //LoadCharge for shipping line
                LoadCharge(order, "Shipping", shippingMethod, shippingAmount);
            }
        }

        /// <summary>
        /// Load promotions and discounts
        /// </summary>
        private void LoadOrderPromotionAndDiscountCharges(OrderEntity order, XPathNavigator orderXPath)
        {
            // Charge - Discount
            decimal discountAmount = -XPathUtility.Evaluate(orderXPath, "./Discount", 0.0m);

            // 3D Cart sends us a Discount field which is the amount subtracted from the Order Total
            // This Discount is the original sum of any Promotions, however an admin can make this amount anything
            // they want.  So we'll decrement each promotion from the discount total, and if anything is left,
            // add a Discount charge with the remainder.
            var promotionTotal = orderXPath.Evaluate("sum(./Promotions/Promotion/Amount)");
            decimal promoTotal = 0.0m;
            if (promotionTotal != null)
            {
                decimal.TryParse(promotionTotal.ToString(), out promoTotal);

                // Make the amount negative
                promoTotal = -promoTotal;
            }

            // Determine if the discount and promo sum matches.  
            bool promoTotalMatchesDiscount = (discountAmount == 0 || promoTotal == discountAmount);

            XPathNodeIterator promotionNodes = orderXPath.Select("./Promotions/Promotion");
            while (promotionNodes.MoveNext())
            {
                XPathNavigator promotion = promotionNodes.Current.Clone();

                // Load charge for each promo code
                // If the promoTotal matches the discount, we want to add the actual promo amount.  Otherwise, we'll just
                // set it to zero so so the order will total up.
                decimal promoAmount = -XPathUtility.Evaluate(promotion, "./Amount", 0.0m);
                string promoCode = XPathUtility.Evaluate(promotion, "./Code", "Unknown Promo Code");

                // If the promoTotal matches the discount, we want to to use the promo description.  If they do not match,
                // we will add the amount to the end of the description.
                
                if(!promoTotalMatchesDiscount)
                {
                    // If the promoTotal matches the discount, we want to to use the promo description.  If they do not match,
                    // we will add the amount to the end of the description.
                    promoAmount = 0.0m;
                    promoCode=string.Format("{0} : {1:C}", promoCode, XPathUtility.Evaluate(promotion, "./Amount", 0.0m));
                }

                //promoCode = promoTotalMatchesDiscount
                //                ? promoCode
                //                : string.Format("{0} : {1:C}", promoCode,
                //                                XPathUtility.Evaluate(promotion, "./Amount", 0.0m));

                LoadCharge(order, "Promotion", promoCode, promoAmount);

                // Subtract the amount of the promo from the total discount amount
                discountAmount -= promoAmount;
            }

            // If we end up with a left over discount amount, add that amount as a Discount.
            if (discountAmount < 0.0m)
            {
                //Load charge for each discount code
                LoadCharge(order, "Discount", "Discount", discountAmount);
            }
        }

        /// <summary>
        /// Load an order charge for the given values for the order
        /// </summary>
        private void LoadCharge(OrderEntity order, string type, string name, decimal amount)
        {
            //InstantiateOrderCharge
            OrderChargeEntity charge = InstantiateOrderCharge(order);

            //Set charge properties
            if (string.IsNullOrWhiteSpace(name))
            {
                name = type;
            }

            charge.Type = type.ToUpperInvariant();
            charge.Description = name;
            charge.Amount = amount;
        }

        /// <summary>
        /// Load the payment details for the order
        /// </summary>
        private void LoadPaymentDetails(OrderEntity order, XPathNavigator xpath)
        {
            // Payment details
            string paymentMethod = XPathUtility.Evaluate(xpath, "./PaymentMethod", "Unknown");
            if (!string.IsNullOrWhiteSpace(paymentMethod))
            {
                //Load payment details
                LoadPaymentDetail(order, "Payment Type", paymentMethod);
            }

            // Add the card type used, if provided
            string cardType = XPathUtility.Evaluate(xpath, "./CardType", "");
            if (!string.IsNullOrWhiteSpace(cardType))
            {
                // Load card type
                LoadPaymentDetail(order, "Card Type", cardType);
            }
        }

        /// <summary>
        /// Load the given payment detail into the ordr
        /// </summary>
        private void LoadPaymentDetail(OrderEntity order, string label, string value)
        {
            //Instantiate OrderPaymentDetail
            OrderPaymentDetailEntity detail = InstantiateOrderPaymentDetail(order);

            //Set orderPaymentDetail properties
            detail.Label = label;
            detail.Value = value;
        }

        /// <summary>
        /// If the order has any notes, save them
        /// </summary>
        private void LoadOrderNotes(OrderEntity order, XPathNavigator xpath)
        {
            string orderComment = XPathUtility.Evaluate(xpath, "./Comments/OrderComment", string.Empty);
            InstantiateNote(order, orderComment, DateTime.Now, NoteVisibility.Public, true);
            
            string orderInternalComment = XPathUtility.Evaluate(xpath, "./Comments/OrderInternalComment", string.Empty);
            InstantiateNote(order, orderInternalComment, DateTime.Now, NoteVisibility.Internal, true);
            
            string orderExternalComment = XPathUtility.Evaluate(xpath, "./Comments/OrderExternalComment", string.Empty);
            InstantiateNote(order, orderExternalComment, DateTime.Now, NoteVisibility.Internal, true);

            //Iterate through each checkout question, and a note for each one 
            XPathNodeIterator orderCheckoutQuestions = xpath.Select("./CheckoutQuestions/Question");
            if (orderCheckoutQuestions != null)
            {
                while (orderCheckoutQuestions.MoveNext())
                {
                    string question = XPathUtility.Evaluate(orderCheckoutQuestions.Current, "./Question", string.Empty);
                    string answer = XPathUtility.Evaluate(orderCheckoutQuestions.Current, "./Answer", string.Empty);
                    if (!string.IsNullOrWhiteSpace(answer))
                    {
                        answer = string.Format(" : {0}", answer);
                    }

                    InstantiateNote(order, string.Format("{0}{1}", question, answer), DateTime.Now,
                                    NoteVisibility.Internal, true);
                }
            }
        }

        /// <summary>
        /// Load the appropriate address info from the XPath
        /// </summary>
        private static void LoadAddressInfo(OrderEntity order, XPathNavigator xpath, XPathNavigator shipmentNode)
        {
            //Load shipping address info
            LoadAddressInfo(order, shipmentNode, ".", "Ship");

            //Load billing address info
            LoadAddressInfo(order, xpath, "./BillingAddress", "Bill");

            //Bill only properties
            order.BillEmail = XPathUtility.Evaluate(xpath, "./BillingAddress/Email", "");

            // ThreeDCart doesnt have a ShipTo email, so set it to the bill email
            order.ShipEmail = order.BillEmail;
        }

        /// <summary>
        /// Load the address info for the given address type prefix
        /// </summary>
        private static void LoadAddressInfo(OrderEntity order, XPathNavigator xpath, string addressType, string dbPrefix)
        {
            //See if the NameParts entries exist
            string first = XPathUtility.Evaluate(xpath, addressType + "/FirstName", "");
            string middle = string.Empty;
            string last = XPathUtility.Evaluate(xpath, addressType + "/LastName", "");

            //Parse person's name
            PersonName fullName = PersonName.Parse(string.Format("{0} {1}", first, last));
            PersonAdapter personAdapter = new PersonAdapter(order, dbPrefix);

            personAdapter.UnparsedName = fullName.FullName;
            personAdapter.NameParseStatus = PersonNameParseStatus.Simple;
            personAdapter.FirstName = first;
            personAdapter.MiddleName = middle;
            personAdapter.LastName = last;

            personAdapter.Company = XPathUtility.Evaluate(xpath, addressType + "/Company", "");
            personAdapter.Phone = XPathUtility.Evaluate(xpath, addressType + "/Phone", "");

            personAdapter.Street1 = XPathUtility.Evaluate(xpath, addressType + "/Address", "");
            personAdapter.Street2 = XPathUtility.Evaluate(xpath, addressType + "/Address2", "");

            personAdapter.City = XPathUtility.Evaluate(xpath, addressType + "/City", "");
            personAdapter.StateProvCode = Geography.GetStateProvCode(XPathUtility.Evaluate(xpath, addressType + "/StateCode", ""));
            personAdapter.PostalCode = XPathUtility.Evaluate(xpath, addressType + "/ZipCode", "");
            personAdapter.CountryCode = Geography.GetCountryCode(XPathUtility.Evaluate(xpath, addressType + "/CountryCode", ""));
        }
    }
}
