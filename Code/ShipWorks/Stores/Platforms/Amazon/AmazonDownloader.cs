using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Stores.Communication;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.HelperClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using log4net;
using Interapptive.Shared.Utility;
using System.Xml.XPath;
using System.Xml;
using Interapptive.Shared;
using ShipWorks.Stores.Content;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;

namespace ShipWorks.Stores.Platforms.Amazon
{
    /// <summary>
    /// Retrieves orders from Amazon.com
    /// </summary>
    public class AmazonDownloader : StoreDownloader
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(AmazonDownloader));

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonDownloader(StoreEntity store)
            : base(store)
        {

        }

        /// <summary>
        /// Start the download from Amazon.com
        /// </summary>
        [NDependIgnoreLongMethod]
        protected override void Download()
        {
            try
            {
                Progress.Detail = "Connecting to Amazon...";

                AmazonEnsClient client = new AmazonEnsClient((AmazonStoreEntity) Store);

                while (true)
                {
                    // check for cancellation
                    if (Progress.IsCancelRequested)
                    {
                        return;
                    }

                    DateTime? maxOrderDate = GetOrderDateStartingPoint();

                    string[] orders = client.DownloadOrders(maxOrderDate);

                    if (orders.Length == 0)
                    {
                        if (QuantitySaved == 0)
                        {
                            Progress.Detail = "No orders to download.";
                            Progress.PercentComplete = 100;
                        }
                        else
                        {
                            Progress.Detail = "Done";
                        }

                        return;
                    }

                    foreach (string order in orders)
                    {
                        // check for cancellation
                        if (Progress.IsCancelRequested)
                        {
                            return;
                        }

                        // set the progress detail
                        Progress.Detail = String.Format("Processing order {0}...", QuantitySaved + 1);

                        // import the order
                        LoadOrder(order);

                        // move the progress bar along
                        Progress.PercentComplete = Math.Min(100 * QuantitySaved / orders.Length, 100);
                    }

                    // the cookie may be updated, so save if it is needed
                    if (Store.IsDirty)
                    {
                        using (SqlAdapter adapter = new SqlAdapter())
                        {
                            adapter.SaveAndRefetch(Store);
                        }
                    }

                    Progress.Detail = "Done";
                    Progress.PercentComplete = 100;
                }
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
        /// Import the Order 
        /// </summary>
        private void LoadOrder(string orderXml)
        {
            // load the document
            XmlDocument document = new XmlDocument();
            document.LoadXml(orderXml);

            // create navigator
            XPathNavigator xpath = document.DocumentElement.CreateNavigator();

            // Now extract the Order#
            string amazonOrder = XPathUtility.Evaluate(xpath, "amazonOrderID", "");

            // get the order instance
            AmazonOrderEntity order = (AmazonOrderEntity)InstantiateOrder(new AmazonOrderIdentifier(amazonOrder));

            // Nothing to do if its not new - they don't change
            if (!order.IsNew)
            {
                return;
            }

            // basic properties
            order.OrderNumber = GetNextOrderNumber();
            order.OrderDate = DateTime.Parse(XPathUtility.Evaluate(xpath, "orderDate", "")).ToUniversalTime();

            // Online customerid
            long onlineCustomerID = XPathUtility.Evaluate(xpath, "orderingCustomerId", 0L);
            if (onlineCustomerID > 0)
            {
                order.OnlineCustomerID = onlineCustomerID;
            }

            // requested shipping
            order.RequestedShipping = XPathUtility.Evaluate(xpath, "fulfillmentData/fulfillmentServiceLevelCategory", "");

            // Load Address Info
            LoadAddresses(order, xpath);

            // Amazon doesn't automatically fill in the ShipTo email
            UpdateShipToEmail(order);

            // items
            LoadItems(order, xpath);

            // Charges
            LoadCharges(order, xpath);

            // Payments
            LoadPaymentDetails(order, xpath);

            // update the total
            order.OrderTotal = OrderUtility.CalculateTotal(order);

            // save the order
            SqlAdapterRetry<SqlException> retryAdapter = new SqlAdapterRetry<SqlException>(5, -5, "AmazonDownloader.LoadOrder");
            retryAdapter.ExecuteWithRetry(() => SaveDownloadedOrder(order));
        }

        /// <summary>
        /// Load the payment method details from the order
        /// </summary>
        private void LoadPaymentDetails(AmazonOrderEntity order, XPathNavigator xpath)
        {
            // CC stuff
            string cctype = XPathUtility.Evaluate(xpath, "billingData/creditCard/issuer", "");
            if (cctype.Length > 0)
            {
                LoadPaymentDetail(order, "Card Expires", XPathUtility.Evaluate(xpath, "billingData/creditCard/expirationDate", ""));
                LoadPaymentDetail(order, "Card Number", "************" + XPathUtility.Evaluate(xpath, "billingData/creditCard/tail", ""));
                LoadPaymentDetail(order, "Card Type", cctype);
            }
        }

        /// <summary>
        /// Creates a Payment Detail entity and populates it
        /// </summary>
        private void LoadPaymentDetail(OrderEntity order, string label, string value)
        {
            OrderPaymentDetailEntity detail = InstantiateOrderPaymentDetail(order);

            detail.Label = label;
            detail.Value = value;
        }

        /// <summary>
        /// Gets charges out of the Amazon order
        /// </summary>
        private void LoadCharges(AmazonOrderEntity order, XPathNavigator xpath)
        {
            decimal fees = XPathUtility.Evaluate(xpath, "string(sum(//itemFees/fee/amount/value))", 0m);
            decimal shippingHoldback = XPathUtility.Evaluate(xpath, "string(sum(//itemPrice/shippingHoldbackComponent/amount/value))", 0m);

            // set the commission
            order.AmazonCommission = Math.Abs(fees) + Math.Abs(shippingHoldback);

            // Promotion Discount
            LoadCharge(order, "Promotion Discount", "Promotion Discount", -XPathUtility.Evaluate(xpath, "string(sum(//promotion/component[type=\"Principal\"]/amount/value))", 0.0m), true);

            // Shipping Discount
            LoadCharge(order, "Shipping Discount", "Shipping Discount", -XPathUtility.Evaluate(xpath, "string(sum(//promotion/component[type=\"Shipping\"]/amount/value))", 0.0m), true);
            
            // Shipping
            LoadCharge(order, "Shipping", "Shipping", XPathUtility.Evaluate(xpath, "string(sum(//itemPrice/shippingComponent/amount/value))", 0.0m), false);
            
            // Tax
            decimal taxAmount = XPathUtility.Evaluate(xpath, "string(sum(//itemPrice/taxComponent/amount/value))", 0.0m);
            taxAmount += XPathUtility.Evaluate(xpath, "string(sum(//itemPrice/giftWrapTaxComponent/amount/value))", 0.0m);
            taxAmount += XPathUtility.Evaluate(xpath, "string(sum(//itemPrice/shippingTaxComponent/amount/value))", 0.0m);

            LoadCharge(order, "Tax", "Tax", taxAmount, false);
        }

        /// <summary>
        /// Creates an Order Charge in ShipWorks
        /// </summary>
        private void LoadCharge(AmazonOrderEntity order, string type, string name, decimal amount, bool ignoreZeroAmount)
        {
            if (amount == 0.0m && ignoreZeroAmount)
            {
                return;
            }

            if (name.Length == 0)
            {
                name = type;
            }

            OrderChargeEntity charge = InstantiateOrderCharge(order);

            charge.Type = type.ToUpper();
            charge.Description = name;
            charge.Amount = amount;
        }

        /// <summary>
        /// Imports order items from the Amazon order into ShipWorks
        /// </summary>
        private void LoadItems(AmazonOrderEntity order, XPathNavigator orderXPath)
        {
            XPathNodeIterator items = orderXPath.Select("item");
            while (items.MoveNext())
            {
                XPathNavigator xpath = items.Current;
                AmazonOrderItemEntity item = (AmazonOrderItemEntity)InstantiateOrderItem(order);

                // populate the basic fields
                item.Name = XPathUtility.Evaluate(xpath, "title", "");
                item.Quantity = XPathUtility.Evaluate(xpath, "quantity", 0);
                item.UnitPrice = XPathUtility.Evaluate(xpath, "itemPrice/principalComponent/amount/value", (decimal)0.0) / (decimal)item.Quantity;
                item.Code = XPathUtility.Evaluate(xpath, "sku", "");
                item.SKU = item.Code;

                // amazon-specific fields
                item.AmazonOrderItemCode = XPathUtility.Evaluate(xpath, "amazonOrderItemCode", string.Empty);
                item.ConditionNote = XPathUtility.Evaluate(xpath, "conditionNote", "");

                AmazonItemDetail amazonItemDetail = AmazonAsin.GetAmazonItemDetail((AmazonStoreEntity)Store, item.SKU);
                item.ASIN = amazonItemDetail.Asin;
                item.Weight = amazonItemDetail.Weight;
                item.Image = amazonItemDetail.ItemUrl;
                item.Thumbnail = item.Image;

                // check for gift wrapping
                string wrappingLevel = XPathUtility.Evaluate(xpath, "giftWrapLevel", "");
                string giftwrapMessage = XPathUtility.Evaluate(xpath, "giftMessageText", "");

                if (wrappingLevel.Length > 0 || giftwrapMessage.Length > 0)
                {
                    if (wrappingLevel.Length == 0)
                    {
                        wrappingLevel = "Gift Message";
                    }

                    decimal wrapCost = XPathUtility.Evaluate(xpath, "itemPrice/giftWrapComponent/amount/value", 0.0M);

                    OrderItemAttributeEntity attribute = InstantiateOrderItemAttribute(item);
                    attribute.Name = wrappingLevel;
                    attribute.Description = giftwrapMessage;
                    attribute.UnitPrice = wrapCost;
                }
            }
        }

        /// <summary>
        /// Update shipment emails
        /// </summary>
        private void UpdateShipToEmail(AmazonOrderEntity order)
        {
            // If its the same ShipTo\BillTo, fill in the ShipTo email address
            if (order.ShipFirstName == order.BillFirstName &&
                order.ShipLastName == order.BillLastName &&
                order.ShipStreet1 == order.BillStreet1 &&
                order.ShipPostalCode == order.BillPostalCode)
            {
                order.ShipEmail = order.BillEmail;
            }
        }

        /// <summary>
        /// Load address data
        /// </summary>
        [NDependIgnoreLongMethod]
        private void LoadAddresses(AmazonOrderEntity order, XPathNavigator xpath)
        {
            List<string> addressLines = new List<string>();

            // Billing information
            PersonName billFullName = PersonName.Parse(XPathUtility.Evaluate(xpath, "billingData/address/name", ""));

            order.BillFirstName = billFullName.First;
            order.BillMiddleName = billFullName.Middle;
            order.BillLastName = billFullName.LastWithSuffix;
            order.BillNameParseStatus = (int)billFullName.ParseStatus;
            order.BillUnparsedName = billFullName.UnparsedName;
            order.BillCompany = "";
            order.BillPhone = XPathUtility.Evaluate(xpath, "billingData/address/phoneNumber", "");

            // Billing Street Lines
            addressLines.Add(XPathUtility.Evaluate(xpath, "billingData/address/addressFieldOne", ""));
            addressLines.Add(XPathUtility.Evaluate(xpath, "billingData/address/addressFieldTwo", ""));
            addressLines.Add(XPathUtility.Evaluate(xpath, "billingData/address/addressFieldThree", ""));
            SetStreetAddress(new PersonAdapter(order, "Bill"), addressLines);

            order.BillCity = XPathUtility.Evaluate(xpath, "billingData/address/city", "");
            order.BillStateProvCode = Geography.GetStateProvCode(XPathUtility.Evaluate(xpath, "billingData/address/stateOrRegion", ""));
            order.BillPostalCode = XPathUtility.Evaluate(xpath, "billingData/address/postalCode", "");
            order.BillCountryCode = Geography.GetCountryCode(XPathUtility.Evaluate(xpath, "billingData/address/country", ""));

            // Bill only properties
            order.BillEmail = XPathUtility.Evaluate(xpath, "billingData/buyerEmailAddress", "");

            // Shipping information
            PersonName shipFullName = PersonName.Parse(XPathUtility.Evaluate(xpath, "fulfillmentData/address/name", ""));
            order.ShipFirstName = shipFullName.First;
            order.ShipMiddleName = shipFullName.Middle;
            order.ShipLastName = shipFullName.LastWithSuffix;
            order.ShipNameParseStatus = (int)shipFullName.ParseStatus;
            order.ShipUnparsedName = shipFullName.UnparsedName;
            order.ShipCompany = "";

            // Shipping Street Lines
            addressLines.Clear();
            addressLines.Add(XPathUtility.Evaluate(xpath, "fulfillmentData/address/addressFieldOne", ""));
            addressLines.Add(XPathUtility.Evaluate(xpath, "fulfillmentData/address/addressFieldTwo", ""));
            addressLines.Add(XPathUtility.Evaluate(xpath, "fulfillmentData/address/addressFieldThree", ""));
            SetStreetAddress(new PersonAdapter(order, "Ship"), addressLines);

            order.ShipPhone = XPathUtility.Evaluate(xpath, "fulfillmentData/address/phoneNumber", "");
            order.ShipCity = XPathUtility.Evaluate(xpath, "fulfillmentData/address/city", "");
            order.ShipStateProvCode = Geography.GetStateProvCode(XPathUtility.Evaluate(xpath, "fulfillmentData/address/stateOrRegion", ""));
            order.ShipPostalCode = XPathUtility.Evaluate(xpath, "fulfillmentData/address/postalCode", "");
            order.ShipCountryCode = Geography.GetCountryCode(XPathUtility.Evaluate(xpath, "fulfillmentData/address/country", ""));
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
    }
}
