using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Stores.Communication;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI;
using System.Xml;
using System.Xml.XPath;
using ShipWorks.Stores.Content;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Net;
using ShipWorks.Data.Connection;
using Interapptive.Shared.Business;

namespace ShipWorks.Stores.Platforms.ShopSite
{
    /// <summary>
    /// Downloader for ShopSiteStores
    /// </summary>
    public class ShopSiteDownloader : StoreDownloader
    {
        int totalCount = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopSiteDownloader(ShopSiteStoreEntity store)
            : base(store)
        {

        }

        /// <summary>
        /// Download data for the ShopSite store
        /// </summary>
        protected override void Download()
        {
            Progress.Detail = "Checking for orders...";

            try
            {
                // Create the web client used for downloading
                ShopSiteWebClient webClient = new ShopSiteWebClient(((ShopSiteStoreEntity) Store));

                while (true)
                {
                    // Check for cancel
                    if (Progress.IsCancelRequested)
                    {
                        return;
                    }

                    if (!DownloadNextOrdersPage(webClient))
                    {
                        return;
                    }
                }
            }
            catch (ShopSiteException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
            catch (SqlForeignKeyException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Download the next page of orders.
        /// </summary>
        private bool DownloadNextOrdersPage(ShopSiteWebClient webClient)
        {
            long lastOrderNumber = GetOrderNumberStartingPoint();

            XmlDocument xmlDocument = webClient.GetOrders(lastOrderNumber + 1);
            XPathNavigator xpath = xmlDocument.CreateNavigator();

            // Determine how many orders there are
            XPathNodeIterator orderNodes = xpath.Select("//Order");

            // If any orders were downloaded we have to import them
            if (orderNodes.Count > 0)
            {
                totalCount += orderNodes.Count;

                // Add this to the XML to be loaded into the database
                LoadOrders(orderNodes);

                return true;
            }
            else
            {
                Progress.Detail = "Done";

                return false;
            }
        }

        /// <summary>
        /// Load all the orders contained in the iterator
        /// </summary>
        private void LoadOrders(XPathNodeIterator orderNodes)
        {
            // Go through each order in the XML Document
            while (orderNodes.MoveNext())
            {
                // Check for user cancel
                if (Progress.IsCancelRequested)
                {
                    return;
                }

                // Update the status
                Progress.Detail = string.Format("Processing order {0}...", (QuantitySaved + 1));

                XPathNavigator order = orderNodes.Current.Clone();
                LoadOrder(order);

                // Update the status
                Progress.PercentComplete = 100 * QuantitySaved / totalCount;
            }
        }

        /// <summary>
        /// Extract and save the order from the XML
        /// </summary>
        private void LoadOrder(XPathNavigator xpath)
        {
            // Now extract the Order#
            int orderNumber = XPathUtility.Evaluate(xpath, "OrderNumber", 0);

            // Get the order instance
            OrderEntity order = InstantiateOrder(new OrderNumberIdentifier(orderNumber));

            // Set the total.  It will be calculated and verified later.
            order.OrderTotal = XPathUtility.Evaluate(xpath, "Totals/GrandTotal", 0.0m);

            // Dates
            order.OrderDate = DateTime.Parse(XPathUtility.Evaluate(xpath, "OrderDate", "")).ToUniversalTime();

            // Customer
            int onlineCustomerID = XPathUtility.Evaluate(xpath, "Other/CustomerID", -1);
            order.OnlineCustomerID = (onlineCustomerID == -1) ? (int?) null : onlineCustomerID;

            // Requested shipping
            order.RequestedShipping = XPathUtility.Evaluate(xpath, "Totals/ShippingTotal/Description", "");

            // Load address info
            LoadAddressInfo(order, xpath);

            // ShopSite doesnt automatically fill in the ShipTo email even if ShipTo\BillTo are the same
            UpdateShipToEmail(order);

            // Only update the rest for brand new orders
            if (order.IsNew)
            {
                LoadCustomerComments(order, xpath);

                // Items
                XPathNodeIterator itemNodes = xpath.Select("Shipping//Product");
                while (itemNodes.MoveNext())
                {
                    LoadItem(order, itemNodes.Current);
                }

                // Load all the charges
                LoadOrderCharges(order, xpath);

                // Load all payment details
                LoadPaymentDetails(order, xpath);
            }

            // Save the downloaded order
            SqlAdapterRetry<SqlException> retryAdapter = new SqlAdapterRetry<SqlException>(5, -5, "ShopSiteDownloader.LoadOrder");
            retryAdapter.ExecuteWithRetry(() => SaveDownloadedOrder(order));
        }

        /// <summary>
        /// Load the item information into the order
        /// </summary>
        private void LoadItem(OrderEntity order, XPathNavigator xpath)
        {
            OrderItemEntity item = InstantiateOrderItem(order);

            item.Name = XPathUtility.Evaluate(xpath, "Name", "");
            item.Code = XPathUtility.Evaluate(xpath, "SKU", "");
            item.SKU = item.Code;
            item.Quantity = XPathUtility.Evaluate(xpath, "Quantity", 0);
            item.UnitPrice = XPathUtility.Evaluate(xpath, "ItemPrice", (decimal) 0.0);
            item.Weight = XPathUtility.Evaluate(xpath, "Weight", (double) 0.0);

            // Now load all the item options
            XPathNodeIterator options = xpath.Select(".//OrderOption");
            while (options.MoveNext())
            {
                // Shopsite sends an empty options element when there are no options
                if (!options.Current.IsEmptyElement)
                {
                    LoadOption(item, options.Current);
                }
            }

            // If there is a customer entry, add it as an option too
            string customerText = XPathUtility.Evaluate(xpath, "CustomerText", "");
            if (customerText.Length > 0)
            {
                OrderItemAttributeEntity option = InstantiateOrderItemAttribute(item);

                option.Name = "Customer Text";
                option.Description = customerText;
                option.UnitPrice = 0;
            }
        }

        /// <summary>
        /// Load the option of the given item
        /// </summary>
        private void LoadOption(OrderItemEntity item, XPathNavigator xpath)
        {
            OrderItemAttributeEntity option = InstantiateOrderItemAttribute(item);

            option.Name = "Option";
            option.Description = XPathUtility.Evaluate(xpath, "SelectedOption", "");

            try
            {
                option.UnitPrice = (decimal) XPathUtility.Evaluate(xpath, "OptionPrice", 0.0);
            }
            catch (FormatException)
            {
                option.UnitPrice = 0;
            }

            // Shopsite incluedes the option price in the item price
            item.UnitPrice -= option.UnitPrice;
        }

        /// <summary>
        /// Load all the charges for the order
        /// </summary>
        private void LoadOrderCharges(OrderEntity order, XPathNavigator xpath)
        {
            // Charge - Discount
            XPathNodeIterator discounts = xpath.Select("Totals/Discount");
            while (discounts.MoveNext())
            {
                XPathNavigator discount = discounts.Current.Clone();

                // Shopsite sends empty elements
                if (!discount.IsEmptyElement)
                {
                    LoadCharge(order, "Discount", "Discount", XPathUtility.Evaluate(xpath, "Totals/Discount/Amount", 0.0));
                }
            }

            // Charges - Generic
            XPathNodeIterator surcharges = xpath.Select("Totals/Surcharge");
            while (surcharges.MoveNext())
            {
                XPathNavigator surcharge = surcharges.Current.Clone();

                // Shopsite sends empty elements
                if (!surcharge.IsEmptyElement)
                {
                    double amount = XPathUtility.Evaluate(surcharge, "Total", 0.0);

                    if (amount > 0)
                    {
                        LoadCharge(order, "Surcharge",
                            XPathUtility.Evaluate(surcharge, "Description", ""),
                            XPathUtility.Evaluate(surcharge, "Total", amount));
                    }
                }
            }

            // Charges - Coupons
            XPathNodeIterator coupons = xpath.Select("Coupon");
            while (coupons.MoveNext())
            {
                XPathNavigator coupon = coupons.Current.Clone();

                // Shopsite sends empty elements
                if (!coupon.IsEmptyElement)
                {
                    LoadCharge(order, "Coupon",
                        XPathUtility.Evaluate(coupon, "Name", "Coupon"),
                        XPathUtility.Evaluate(coupon, "Total", 0.0));
                }
            }

            LoadCharge(order, "Tax",
                XPathUtility.Evaluate(xpath, "Totals/Tax/TaxName", "Tax"),
                XPathUtility.Evaluate(xpath, "Totals/Tax/TaxAmount", 0.0));

            LoadCharge(order, "Shipping",
                XPathUtility.Evaluate(xpath, "Totals/ShippingTotal/Description", "Shipping"),
                XPathUtility.Evaluate(xpath, "Totals/ShippingTotal/Total", 0.0));
        }

        /// <summary>
        /// Load an order charge for the given values for the order
        /// </summary>
        private void LoadCharge(OrderEntity order, string type, string name, double amount)
        {
            OrderChargeEntity charge = InstantiateOrderCharge(order);

            if (name.Length == 0)
            {
                name = type;
            }

            charge.Type = type.ToUpperInvariant();
            charge.Description = name;
            charge.Amount = (decimal) amount;
        }

        /// <summary>
        /// Load the payment details for the order
        /// </summary>
        private void LoadPaymentDetails(OrderEntity order, XPathNavigator xpath)
        {
            // Payment details
            XPathNodeIterator paymentType = xpath.Select("Payment/node()");
            if (paymentType.MoveNext())
            {
                XPathNodeIterator paymentDetailNodes = paymentType.Current.Select("node()");
                while (paymentDetailNodes.MoveNext())
                {
                    XPathNavigator detailNode = paymentDetailNodes.Current.Clone();

                    LoadPaymentDetail(order, detailNode.LocalName, detailNode.Value);
                }

                string paymentTypeName = paymentType.Current.LocalName;
                LoadPaymentDetail(order, "Payment Type", paymentTypeName);
            }
        }

        /// <summary>
        /// Load the given payment detail into the ordr
        /// </summary>
        private void LoadPaymentDetail(OrderEntity order, string label, string value)
        {
            OrderPaymentDetailEntity detail = InstantiateOrderPaymentDetail(order);

            detail.Label = label;
            detail.Value = value;
        }

        /// <summary>
        /// Load any customer entered comments from the online order
        /// </summary>
        private void LoadCustomerComments(OrderEntity order, XPathNavigator xpath)
        {
            // Customer comments
            InstantiateNote(order, XPathUtility.Evaluate(xpath, "Other/Comments", ""), order.OrderDate, NoteVisibility.Public);

            // Odrder instructions
            InstantiateNote(order, XPathUtility.Evaluate(xpath, "Other/OrderInstructions", ""), order.OrderDate, NoteVisibility.Public);
        }

        /// <summary>
        /// Load the appropriate address info from the XPath
        /// </summary>
        private void LoadAddressInfo(OrderEntity order, XPathNavigator xpath)
        {
            LoadAddressInfo(order, xpath, "Shipping", "Ship");
            LoadAddressInfo(order, xpath, "Billing", "Bill");

            // Bill only properties
            order.BillEmail = XPathUtility.Evaluate(xpath, "Billing/Email", "");
        }

        /// <summary>
        /// Load the address info for the given address type prefix
        /// </summary>
        private void LoadAddressInfo(OrderEntity order, XPathNavigator xpath, string xpathPrefix, string dbPrefix)
        {
            PersonName fullName = PersonName.Parse(XPathUtility.Evaluate(xpath, xpathPrefix + "/FullName", ""));

            // See if the NameParts entries exist
            string first = XPathUtility.Evaluate(xpath, xpathPrefix + "/NameParts/FirstName", "");
            string middle = XPathUtility.Evaluate(xpath, xpathPrefix + "/NameParts/MiddleName", "");
            string last = XPathUtility.Evaluate(xpath, xpathPrefix + "/NameParts/LastName", "");

            // Use name parts if we can, otherwise revert to what we parsed.
            order.SetNewFieldValue(dbPrefix + "UnparsedName", fullName.FullName);
            order.SetNewFieldValue(dbPrefix + "NameParseStatus", (int)PersonNameParseStatus.Simple);
            order.SetNewFieldValue(dbPrefix + "FirstName", first != "" ? first : fullName.First);
            order.SetNewFieldValue(dbPrefix + "MiddleName", middle != "" ? middle : fullName.Middle);
            order.SetNewFieldValue(dbPrefix + "LastName", last != "" ? last : fullName.Last);

            order.SetNewFieldValue(dbPrefix + "Company", XPathUtility.Evaluate(xpath, xpathPrefix + "/Company", ""));
            order.SetNewFieldValue(dbPrefix + "Phone", XPathUtility.Evaluate(xpath, xpathPrefix + "/Phone", ""));

            order.SetNewFieldValue(dbPrefix + "Street1", XPathUtility.Evaluate(xpath, xpathPrefix + "/Address/Street1", ""));
            order.SetNewFieldValue(dbPrefix + "Street2", XPathUtility.Evaluate(xpath, xpathPrefix + "/Address/Street2", ""));

            order.SetNewFieldValue(dbPrefix + "City", XPathUtility.Evaluate(xpath, xpathPrefix + "/Address/City", ""));
            order.SetNewFieldValue(dbPrefix + "StateProvCode", Geography.GetStateProvCode(XPathUtility.Evaluate(xpath, xpathPrefix + "/Address/State", "")));
            order.SetNewFieldValue(dbPrefix + "PostalCode", XPathUtility.Evaluate(xpath, xpathPrefix + "/Address/Code", ""));
            order.SetNewFieldValue(dbPrefix + "CountryCode", Geography.GetCountryCode(XPathUtility.Evaluate(xpath, xpathPrefix + "/Address/Country", "")));
        }

        /// <summary>
        /// Since ShopSite does not automatically fill in the ShipTo email when the ShipTo\BillTo
        /// are the same, we manually do it here.
        /// </summary>
        private void UpdateShipToEmail(OrderEntity order)
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
    }
}
