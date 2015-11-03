using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using log4net;
using System.Xml.XPath;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Business;
using ShipWorks.Stores.Content;
using System.Diagnostics;
using ShipWorks.Data.Model.HelperClasses;
using System.Globalization;
using Interapptive.Shared;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Collections;

namespace ShipWorks.Stores.Platforms.Amazon
{
    /// <summary>
    /// Order downloader for Amazon MWS
    /// </summary>
    public class AmazonMwsDownloader : StoreDownloader
    {
        static readonly ILog log = LogManager.GetLogger(typeof(AmazonMwsDownloader));
        
        int quantitySeen = 0;

        /// <summary>
        /// Gets the Amazon store entity
        /// </summary>
        private AmazonStoreEntity AmazonStore
        {
            get { return (AmazonStoreEntity)Store; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonMwsDownloader(StoreEntity store)
            : base(store)
        {
        }

        /// <summary>
        /// Start the download from Amazon.com using the Marketplace Web Service (MWS)
        /// </summary>
        protected override void Download()
        {
            try
            {
                Progress.Detail = "Connecting to Amazon...";

                // declare upfront which api calls we are going to be using so they will be throttled
                using (AmazonMwsClient client = new AmazonMwsClient((AmazonStoreEntity)Store, Progress))
                {
                    // test the local system clock
                    if (!client.ClockInSyncWithMWS())
                    {
                        throw new AmazonException("Your system time is out of sync with the Amazon servers.  Ensure your clock is accurate, including the time zone.", null);
                    }

                    // BN: There was some customer confusion over downloads just refusing to happen - this will make it more clear.
                    /*if (client.QuotaExceeded())
                    {
                        log.InfoFormat("Skipping download because the account has exceeded it's qouta.");

                        // silently return if we have no api calls remaining.  We don't want to block other downloaders right now.
                        return;
                    }*/

                    // try to detect if the system is up
                    client.TestServiceStatus();

                    // determine where to start downloading from
                    DateTime? startDate = GetOnlineLastModifiedStartingPoint();

                    // Amazon's APIs are LastUpdateTime-inclusive, so skip over the order we already have
                    if (startDate.HasValue)
                    {
                        startDate = startDate.Value.AddSeconds(1);
                    }

                    Progress.Detail = String.Format("Checking for new orders since {0}...", startDate);

                    foreach (XPathNamespaceNavigator xpath in client.GetOrders(startDate))
                    {
                        if (Progress.IsCancelRequested)
                        {
                            return;
                        }

                        // progress has to be indicated on each pass since we have 0 idea how many orders exists
                        Progress.PercentComplete = 0;

                        // load each order in this result page
                        LoadOrders(client, xpath);
                    }

                    Progress.PercentComplete = 100;
                    Progress.Detail = "Done.";
                }
            }
            catch (AmazonMwsThrottleWaitCancelException)
            {
                // Just a cancel - nothing to do
            }
            catch (AmazonException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
            catch (SqlForeignKeyException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Load orders from a page of results
        /// </summary>
        private void LoadOrders(AmazonMwsClient client, XPathNamespaceNavigator xpath)
        {
            int totalCount = quantitySeen + XPathUtility.Evaluate(xpath, "count(//amz:Order)", 0);

            foreach (XPathNavigator tempXPath in xpath.Select("//amz:Order"))
            {
                quantitySeen++;

                // We only know the true total count if we see less than 100 on the first download
                if (totalCount < 100)
                {
                    Progress.Detail = String.Format("Processing order {0} of {1}...", quantitySeen, totalCount);
                }
                else
                {
                    Progress.Detail = string.Format("Processing order {0}...", quantitySeen);
                }

                XPathNamespaceNavigator orderXPath = new XPathNamespaceNavigator(tempXPath, xpath.Namespaces);
                LoadOrder(client, orderXPath);

                // update progress
                Progress.PercentComplete = Math.Min(100 * quantitySeen / totalCount, 100);
            }
        }

        /// <summary>
        /// Loads a single order from the correctly positioned xpathnavigator
        /// </summary>
        [NDependIgnoreLongMethod]
        private void LoadOrder(AmazonMwsClient client, XPathNamespaceNavigator xpath)
        {
            string amazonOrderID = XPathUtility.Evaluate(xpath, "amz:AmazonOrderId", "");

            // get the order instance
            AmazonOrderEntity order = (AmazonOrderEntity)InstantiateOrder(new AmazonOrderIdentifier(amazonOrderID));

            string orderStatus = XPathUtility.Evaluate(xpath, "amz:OrderStatus", "");

            if (String.Compare(orderStatus, "Canceled", StringComparison.OrdinalIgnoreCase) == 0 && order.IsNew)
            {
                log.InfoFormat("Skipping order '{0}' due to canceled and not yet seen by ShipWorks.", amazonOrderID);
                return;
            }

            // basic properties
            order.OrderDate = DateTime.Parse(XPathUtility.Evaluate(xpath, "amz:PurchaseDate", "")).ToUniversalTime();
            order.OnlineLastModified = DateTime.Parse(XPathUtility.Evaluate(xpath, "amz:LastUpdateDate", "")).ToUniversalTime();

            order.EarliestExpectedDeliveryDate = ParseDeliveryDate(XPathUtility.Evaluate(xpath, "amz:EarliestDeliveryDate", ""));
            order.LatestExpectedDeliveryDate = ParseDeliveryDate(XPathUtility.Evaluate(xpath, "amz:LatestDeliveryDate", ""));
            
            // set the status
            order.OnlineStatus = orderStatus;
            order.OnlineStatusCode = orderStatus;

            // Fulfilled by
            string fulfillmentChannel = XPathUtility.Evaluate(xpath, "amz:FulfillmentChannel", "");
            order.FulfillmentChannel = (int) TranslateFulfillmentChannel(fulfillmentChannel);
            
            // IsPrime
            string isPrime = XPathUtility.Evaluate(xpath, "amz:IsPrime", "");
            order.IsPrime = (int)TranslateIsPrime(isPrime);

            // no customer ID in this Api
            order.OnlineCustomerID = null;

            // requested shipping
            string shipCategory = XPathUtility.Evaluate(xpath, "amz:ShipmentServiceLevelCategory", "");
            if (shipCategory.Length > 0)
            {
                shipCategory += ": ";
            }
            order.RequestedShipping = shipCategory + XPathUtility.Evaluate(xpath, "amz:ShipServiceLevel", "");

            // Address
            LoadAddresses(order, xpath);

            // only load order items on new orders
            if (order.IsNew)
            {
                order.OrderNumber = GetNextOrderNumber();

                LoadOrderItems(client, order);
                
                // Load details about the item (weight, image, etc.) Amazon throttles usage, so to conserve on 
                // calls to Amazon, we load the item details here since we can send more than one item in 
                // a request.
                LoadOrderItemDetails(order.OrderItems.Cast<AmazonOrderItemEntity>().ToList(), client);

                // update the total
                order.OrderTotal = OrderUtility.CalculateTotal(order);

                // get the amount so we can fudge order totals
                decimal orderAmount = XPathUtility.Evaluate(xpath, "amz:OrderTotal/amz:Amount", 0M);

                if (order.OrderTotal != orderAmount)
                {
                    string warning = string.Format("Order '{0} total should have been {1}, but was calculated as {2}", order.AmazonOrderID, orderAmount, order.OrderTotal);
                    log.WarnFormat(warning);

                    Debug.Fail(warning);
                }
            }
            
            // save
            SqlAdapterRetry<SqlException> retryAdapter = new SqlAdapterRetry<SqlException>(5, -5, "AmazonMwsDownloader.LoadOrder");
            retryAdapter.ExecuteWithRetry(() => SaveDownloadedOrder(order));
        }

        /// <summary>
        /// Map the fullfillment channel string provided by Amazon to our internal representation
        /// </summary>
        private static AmazonMwsFulfillmentChannel TranslateFulfillmentChannel(string fulfillmentChannel)
        {
            switch (fulfillmentChannel)
            {
                case "AFN": return AmazonMwsFulfillmentChannel.AFN;
                case "MFN": return AmazonMwsFulfillmentChannel.MFN;
            }

            return AmazonMwsFulfillmentChannel.Unknown;
        }

        /// <summary>
        /// Map the IsPrime string provided by Amazon to our internal representation
        /// </summary>
        private static AmazonMwsIsPrime TranslateIsPrime(string IsPrime)
        {
            switch (IsPrime.ToUpperInvariant())
            {
                case "TRUE": return AmazonMwsIsPrime.Yes;
                case "FALSE": return AmazonMwsIsPrime.No;
            }

            return AmazonMwsIsPrime.Unknown;
        }

        /// <summary>
        /// Loads the order items of an amazon order
        /// </summary>
        [NDependIgnoreLongMethod]
        private void LoadOrderItems(AmazonMwsClient client, AmazonOrderEntity order)
        {
            foreach (XPathNamespaceNavigator navigator in client.GetOrderItems(order.AmazonOrderID))
            {
                foreach (XPathNavigator tempXpath in navigator.Select("//amz:OrderItem"))
                {
                    XPathNamespaceNavigator xpath = new XPathNamespaceNavigator(tempXpath, navigator.Namespaces);

                    AmazonOrderItemEntity item = (AmazonOrderItemEntity)InstantiateOrderItem(order);

                    // populate the basics
                    item.Name = XPathUtility.Evaluate(xpath, "amz:Title", "");
                    item.Quantity = XPathUtility.Evaluate(xpath, "amz:QuantityOrdered", 0d);
                    item.UnitPrice = XPathUtility.Evaluate(xpath, "amz:ItemPrice/amz:Amount", 0M) / (item.Quantity == 0 ? 1 : Convert.ToDecimal(item.Quantity));
                    item.Code = XPathUtility.Evaluate(xpath, "amz:SellerSKU", "");
                    item.SKU = item.Code;

                    // amazon-specific fields
                    item.AmazonOrderItemCode = XPathUtility.Evaluate(xpath, "amz:OrderItemId", string.Empty);
                    item.ConditionNote = XPathUtility.Evaluate(xpath, "amz:ConditionNote", "");
                    item.ASIN = XPathUtility.Evaluate(xpath, "amz:ASIN", "");

                    // Amazon doesn't have a new solution for weights or images

                    // see if we need to add any attributes
                    string giftMessage = XPathUtility.Evaluate(xpath, "amz:GiftMessageText", "");
                    decimal giftWrapPrice = XPathUtility.Evaluate(xpath, "amz:GiftWrapPrice/amz:Amount", 0M);

                    if (giftMessage.Length > 0 || giftWrapPrice > 0)
                    {
                        OrderItemAttributeEntity attribute = InstantiateOrderItemAttribute(item);
                        attribute.Name = "Gift Message";
                        attribute.Description = giftMessage;
                        attribute.UnitPrice = giftWrapPrice;
                    }

                    string giftwrapLevel = XPathUtility.Evaluate(xpath, "amz:GiftWrapLevel", "");
                    if (giftwrapLevel.Length > 0)
                    {
                        OrderItemAttributeEntity attribute = InstantiateOrderItemAttribute(item);
                        attribute.Name = "Gift Wrap Level";
                        attribute.Description = giftwrapLevel;
                        attribute.UnitPrice = 0;
                    }

                    // add an attribute for each promotion
                    foreach (XPathNavigator promoXPath in xpath.Select("amz:PromotionIds/amz:PromotionId"))
                    {
                        string promoId = XPathUtility.Evaluate(promoXPath, "text()", "");
                        if (promoId.Length > 0)
                        {
                            OrderItemAttributeEntity attribute = InstantiateOrderItemAttribute(item);
                            attribute.Name = "Promotion ID";
                            attribute.Description = promoId;
                            attribute.UnitPrice = 0;
                        }
                    }

                    // Charges
                    decimal itemTax = XPathUtility.Evaluate(xpath, "amz:ItemTax/amz:Amount", 0M);
                    AddToCharge(order, "Tax", "Tax", itemTax);

                    decimal giftWrapTax = XPathUtility.Evaluate(xpath, "amz:GiftWrapTax/amz:Amount", 0M);
                    AddToCharge(order, "Tax", "Tax", giftWrapTax);

                    decimal shippingTax = XPathUtility.Evaluate(xpath, "amz:ShippingTax/amz:Amount", 0M);
                    AddToCharge(order, "Tax", "Tax", shippingTax);

                    decimal shipDiscount = XPathUtility.Evaluate(xpath, "amz:ShippingDiscount/amz:Amount", 0M);
                    AddToCharge(order, "Shipping Discount", "Shipping Discount", -shipDiscount);

                    decimal promoDiscount = XPathUtility.Evaluate(xpath, "amz:PromotionDiscount/amz:Amount", 0M);
                    AddToCharge(order, "Promotion Discount", "Promotion Discount", -promoDiscount);

                    decimal shippingPrice = XPathUtility.Evaluate(xpath, "amz:ShippingPrice/amz:Amount", 0M);
                    AddToCharge(order, "Shipping", "Shipping", shippingPrice);
                }
            }
        }

        /// <summary>
        /// Loads additional details (weight, thumbnail, etc.) about the items provided.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="webClient">The web client.</param>
        private void LoadOrderItemDetails(List<AmazonOrderItemEntity> items, AmazonMwsClient webClient)
        {
            AmazonProductDetailRepository repository = new AmazonProductDetailRepository(webClient);
            IEnumerable<XPathNamespaceNavigator> products = repository.GetProductDetails(items);

            foreach (XPathNavigator product in products)
            {
                // Use the ASIN to find all of the order items in the original list and update the image and 
                // weight properties based on the data retrieved from Amazon
                string productASIN = XPathUtility.Evaluate(product, "amz:Identifiers/amz:MarketplaceASIN/amz:ASIN", string.Empty);
                List<AmazonOrderItemEntity> matchedItems = items.FindAll(i => i.ASIN == productASIN);

                foreach (AmazonOrderItemEntity item in matchedItems)
                {
                    // There is only one image URL provided, so populate the both image properties with it
                    item.Thumbnail = XPathUtility.Evaluate(product, "amz:AttributeSets/details:ItemAttributes/details:SmallImage/details:URL", string.Empty);
                    item.Image = item.Thumbnail;

                    // There are two ways to obtain the weight of the order item: from the package dimensions and from the 
                    // item dimensions. Our preferences is to use the weight of the package dimension, and if that is not
                    // available, we'll fall back to the item's weight
                    double weight = 0.0;
                    string weightPath = "amz:AttributeSets/details:ItemAttributes/details:PackageDimensions/details:Weight";

                    if (!double.TryParse(XPathUtility.Evaluate(product, weightPath, null), out weight))
                    {
                        // We didn't find the weight of the package of the item, so we'll try to extract the weight
                        // of the item instead. If a value is not found for this, the weight will just be 0.0 - the
                        // default value of weight
                        weightPath = "amz:AttributeSets/details:ItemAttributes/details:ItemDimensions/details:Weight";
                        double.TryParse(XPathUtility.Evaluate(product, weightPath, null), out weight);
                    }

                    item.Weight = weight;
                }
            }
        }

        /// <summary>
        /// Locates an order's charge (or creates it) and adds the value
        /// </summary>
        private void AddToCharge(OrderEntity order, string chargeType, string name, decimal amount)
        {
            // dont' need to create 0-value charges
            if (amount == 0)
            {
                return;
            }

            OrderChargeEntity charge = order.OrderCharges.FirstOrDefault(c => String.Compare(c.Type, chargeType.ToUpper(), StringComparison.OrdinalIgnoreCase) == 0);
            if (charge == null)
            {
                // first one, create it
                charge = InstantiateOrderCharge(order);
                charge.Type = chargeType.ToUpper();
                charge.Description = name;
                charge.Amount = 0;
            }

            charge.Amount += amount;
        }

        /// <summary>
        /// Populates the Shipping Address
        /// </summary>
        [NDependIgnoreLongMethod]
        private static void LoadAddresses(AmazonOrderEntity order, XPathNamespaceNavigator xpath)
        {
            bool addressExists = xpath.SelectSingleNode("amz:ShippingAddress") != null;

            if (addressExists || order.IsNew)
            {
                PersonName shipFullName = PersonName.Parse(XPathUtility.Evaluate(xpath, "amz:ShippingAddress/amz:Name", ""));
                order.ShipFirstName = shipFullName.First;
                order.ShipMiddleName = shipFullName.Middle;
                order.ShipLastName = shipFullName.LastWithSuffix;
                order.ShipNameParseStatus = (int)shipFullName.ParseStatus;
                order.ShipUnparsedName = shipFullName.UnparsedName;
                order.ShipCompany = "";
                order.ShipPhone = XPathUtility.Evaluate(xpath, "amz:ShippingAddress/amz:Phone", "");

                List<string> addressLines = new List<string>();
                addressLines.Add(XPathUtility.Evaluate(xpath, "amz:ShippingAddress/amz:AddressLine1", ""));
                addressLines.Add(XPathUtility.Evaluate(xpath, "amz:ShippingAddress/amz:AddressLine2", ""));
                addressLines.Add(XPathUtility.Evaluate(xpath, "amz:ShippingAddress/amz:AddressLine3", ""));
                SetStreetAddress(new PersonAdapter(order, "Ship"), addressLines);

                order.ShipCity = XPathUtility.Evaluate(xpath, "amz:ShippingAddress/amz:City", "");
                order.ShipPostalCode = XPathUtility.Evaluate(xpath, "amz:ShippingAddress/amz:PostalCode", "");
                order.ShipCountryCode = Geography.GetCountryCode(XPathUtility.Evaluate(xpath, "amz:ShippingAddress/amz:CountryCode", ""));
                order.ShipStateProvCode = Geography.GetStateProvCode(XPathUtility.Evaluate(xpath, "amz:ShippingAddress/amz:StateOrRegion", ""), order.ShipCountryCode);

                // 10/18/2011, Amazon just added BuyerName and BuyerEmail.  Use it here to ovewrite
                string buyerFullName = XPathUtility.Evaluate(xpath, "amz:BuyerName", "");
                if (!String.IsNullOrEmpty(buyerFullName))
                {
                    // parse the name
                    PersonName buyerName = PersonName.Parse(buyerFullName);
                    order.BillFirstName = buyerName.First;
                    order.BillMiddleName = buyerName.Middle;
                    order.BillLastName = buyerName.LastWithSuffix;
                    order.BillNameParseStatus = (int)buyerName.ParseStatus;
                    order.BillUnparsedName = buyerName.UnparsedName;

                    // If first and ladt name on the buyer are the same as the shipping name, copy the rest of hte address too
                    if ((String.Compare(order.BillFirstName, order.ShipFirstName, StringComparison.OrdinalIgnoreCase) == 0) &&
                        (String.Compare(order.BillLastName, order.ShipLastName, StringComparison.OrdinalIgnoreCase) == 0))
                    {
                        // until Amazon provides some billing information, copy everything to billing from shipping
                        PersonAdapter.Copy(new PersonAdapter(order, "Ship"), new PersonAdapter(order, "Bill"));
                    }
                }
                else
                {
                    // until Amazon provides some billing information, copy everything to billing from shipping
                    PersonAdapter.Copy(new PersonAdapter(order, "Ship"), new PersonAdapter(order, "Bill"));

                }

                // Amazon sends buyer email now, use it for billing and shipping
                order.BillEmail = XPathUtility.Evaluate(xpath, "amz:BuyerEmail", "");
                order.ShipEmail = order.BillEmail;
            }
        }

        /// <summary>
        /// Sets the XXXStreet1 - XXXStreet3 address lines
        /// </summary>
        private static void SetStreetAddress(PersonAdapter address, List<string> addressLines)
        {
            // first get rid of blanks
            addressLines.RemoveAll(s => s.Length == 0);

            int targetLine = 0;
            foreach (string addressLine in addressLines)
            {
                targetLine++;

                switch (targetLine)
                {
                    case 1:
                        address.Street1 = addressLine;
                        break;
                    case 2:
                        address.Street2 = addressLine;
                        break;
                    case 3:
                        address.Street3 = addressLine;
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Sets delivery date from string, returns null if parse failed
        /// </summary>
        private static DateTime? ParseDeliveryDate(string date)
        {
            DateTime parsedDate;
            if (DateTime.TryParse(date, out parsedDate))
            {
                return parsedDate.ToUniversalTime();
            }

            return null;
        } 
    }
}
