using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Stores.Communication;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;
using System.Xml.XPath;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Content;
using System.Globalization;
using Interapptive.Shared;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;

namespace ShipWorks.Stores.Platforms.Infopia
{
    /// <summary>
    /// Order downloader for retrieving Infopia orders.
    /// </summary>
    public class InfopiaDownloader : StoreDownloader
    {
        // count of orders to be downloaded
        int totalCount = 0;

        /// <summary>
        /// Convenience property for quick access to the specific store entity
        /// </summary>
        private InfopiaStoreEntity InfopiaStore
        {
            get { return (InfopiaStoreEntity)Store; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public InfopiaDownloader(InfopiaStoreEntity store)
            : base(store)
        {
        }

        /// <summary>
        /// Download the data
        /// </summary>
        protected override void Download()
        {
            Progress.Detail = "Checking for orders...";

            DateTime? lastModified = GetOnlineLastModifiedStartingPoint();
            if (!lastModified.HasValue)
            {
                // If they chose to go back forever, still limit to a year
                lastModified = DateTime.UtcNow.AddYears(-1);
            }

            // get the order ids that need to be downloaded
            InfopiaWebClient client = new InfopiaWebClient(InfopiaStore);
            try
            {
                // total number to be downloaded
                totalCount = client.GetOrderCount(lastModified);

                // exit early if there's nothing to download
                if (totalCount == 0)
                {
                    Progress.Detail = "No orders to download.";
                    Progress.PercentComplete = 100;
                    return;
                }

                Progress.Detail = string.Format("Downloading {0} orders...", totalCount);

                while (client.GetNextOrdersPage(lastModified))
                {
                    // check for cancellation
                    if (Progress.IsCancelRequested)
                    {
                        return;
                    }

                    LoadOrders(client);
                }

                Progress.Detail = "Done";
            }
            catch (InfopiaException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
            catch (SqlForeignKeyException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Loads the next page of returned orders
        /// </summary>
        [NDependIgnoreLongMethod]
        private void LoadOrders(InfopiaWebClient client)
        {
            // don't do anything
            if (client.OrdersXml == null)
            {
                return;
            }

            XPathNodeIterator orderNodes = client.OrdersXml.CreateNavigator().Select("//InfopiaOrder");
            while (orderNodes.MoveNext())
            {
                Progress.Detail = string.Format("Downloading {0} orders... ({1} of {0})", totalCount, QuantitySaved + 1);

                XPathNavigator xpath = orderNodes.Current;

                // extract the order number
                int orderNumber = XPathUtility.Evaluate(xpath, "Cell[@Name='*ORDER ID*']", 0);

                // get the order instance, creates one if neccessary
                OrderEntity order = InstantiateOrder(new OrderNumberIdentifier(orderNumber));

                // test the ORDER TM.  We have had problems in the past where all order information was blank due to some Infopia server problems.
                string orderDate = XPathUtility.Evaluate(xpath, "Cell[@Name='*ORDER TM*']", "");
                if (orderDate.Length == 0)
                {
                    // if this is happening, just notify the user and stop.
                    throw new InfopiaException("Infopia provided ShipWorks with incomplete or blank order information.  ShipWorks cannot continue to download.");
                }
                 
                // basic properties
                order.OrderDate = InfopiaUtility.ConvertFromInfopiaTimeZone(InfopiaUtility.ParseDate(orderDate));
                order.OnlineLastModified = InfopiaUtility.ConvertFromInfopiaTimeZone(InfopiaUtility.ParseDate(XPathUtility.Evaluate(xpath, "Cell[@Name='*ORDER STATUS LAST CHANGED TM*']", "")));
                order.OnlineCustomerID = XPathUtility.Evaluate(xpath, "Cell[@Name='*ORDER CUSTOMER ID*']", 0);

                // address information
                LoadAddresses(order, xpath);

                // shipping method
                order.RequestedShipping =
                    (XPathUtility.Evaluate(xpath, "Cell[@Name='*ORDER SHIPPER*']", "") + " " +
                     XPathUtility.Evaluate(xpath, "Cell[@Name='*ORDER SHIP METHOD*']", "")).Trim();

                order.OnlineStatus = XPathUtility.Evaluate(xpath, "Cell[@Name='*ORDER STATUS*']", "").Replace(" Order", "").Replace(" Transaction", "");

                if (order.IsNew)
                {
                    // Notes
                    LoadNotes(order, xpath);

                    CacheProducts(xpath);

                    // Order Items
                    XPathNodeIterator itemNodes = xpath.Select("Product");
                    while (itemNodes.MoveNext())
                    {
                        LoadItem(client, order, itemNodes.Current);
                    }

                    // Charges
                    LoadOrderCharges(order, xpath);

                    // Payments
                    LoadPaymentDetails(order, xpath);

                    // if there are any tracking lines, consider the status shipped
                    if (xpath.Select("Tracking").Count > 0)
                    {
                        order.LocalStatus = "Shipped";
                    }

                    order.OrderTotal = OrderUtility.CalculateTotal(order);
                }

                SqlAdapterRetry<SqlException> retryAdapter = new SqlAdapterRetry<SqlException>(5, -5, "InfopiaDownloader.LoadOrder");
                retryAdapter.ExecuteWithRetry(() => SaveDownloadedOrder(order));

                // update the status, 100 max
                Progress.PercentComplete = Math.Min(100 * QuantitySaved / totalCount, 100);
            }
        }

        /// <summary>
        /// Ensures all the products in the order get downloaded/cached in a single call to Infopia
        /// </summary>
        private void CacheProducts(XPathNavigator xpath)
        {
            List<string> productsToDownload = new List<string>();
            XPathNodeIterator products = xpath.Select("Product//Cell[@Name='*ORDER PRODUCT LINE PRODUCT CODE*']");
            while (products.MoveNext())
            {
                string code = products.Current.Value;

                if (code.Length > 0)
                {
                    productsToDownload.Add(code);
                }
            }

            // make sure these are allready cached
            InfopiaWebClient client = new InfopiaWebClient(InfopiaStore);
            client.EnsureProducts(productsToDownload);
            
        }

        /// <summary>
        /// Load order payments
        /// </summary>
        private void LoadPaymentDetails(OrderEntity order, XPathNavigator xpath)
        {
            // CC stuff
            string cardNumber = XPathUtility.Evaluate(xpath, "Cell[@Name='*ORDER MASKED CREDIT CARD NUM*']", "");
            if (cardNumber.Length > 0)
            {
                LoadPaymentDetail(order, "Card Owner", XPathUtility.Evaluate(xpath, "Cell[@Name='*ORDER CREDIT CARD NAME ON CARD*']", ""));
                LoadPaymentDetail(order, "Card Expires", XPathUtility.Evaluate(xpath, "Cell[@Name='*ORDER CREDIT CARD EXP MONTH*']", "") + "/" + XPathUtility.Evaluate(xpath, "Cell[@Name='*ORDER CREDIT CARD EXP YEAR*']", ""));
                LoadPaymentDetail(order, "Card Number", cardNumber);
            }

           // Payment details
           LoadPaymentDetail(order, "Payment Type", XPathUtility.Evaluate(xpath, "Cell[@Name='*ORDER PAY TERM*']", ""));
        }

        /// <summary>
        /// Creates a payment detail record
        /// </summary>
        private void LoadPaymentDetail(OrderEntity order, string name, string data)
        {
            OrderPaymentDetailEntity detail = InstantiateOrderPaymentDetail(order);

            detail.Label = name;
            detail.Value = data;
        }

        /// <summary>
        /// Load order charges
        /// </summary>
        private void LoadOrderCharges(OrderEntity order, XPathNavigator xpath)
        {
            double discount = 0;
            double shipping = 0;
            double handling = 0;
            double tax = 0;
            double surcharge = 0;

            // Some charges are the sum of all the lines in the products
            XPathNodeIterator itemNodes = xpath.Select("Product");
            while (itemNodes.MoveNext())
            {
                XPathNavigator itemNode = itemNodes.Current;

                discount += XPathUtility.Evaluate(itemNode, "Cell[@Name='*ORDER PRODUCT LINE COUPON DISCOUNT*']", 0.0);
                shipping += XPathUtility.Evaluate(itemNode, "Cell[@Name='*ORDER PRODUCT LINE SHIP PRICE*']", 0.0);
                handling += XPathUtility.Evaluate(itemNode, "Cell[@Name='*ORDER PRODUCT LINE HANDLING FEE*']", 0.0);
                tax += XPathUtility.Evaluate(itemNode, "Cell[@Name='*ORDER PRODUCT LINE TAX*']", 0.0);
                surcharge += XPathUtility.Evaluate(itemNode, "Cell[@Name='*ORDER PRODUCT LINE SURCHARGE*']", 0.0);
            }

            // Coupon name
            string couponName = XPathUtility.Evaluate(xpath, "Cell[@Name='*ORDER COUPON CODE*']", "");
            if (couponName.Length == 0 && discount != 0)
            {
                couponName = "discount";
            }

            // Always load tax
            LoadCharge(order, "Tax", "Tax", tax);

            // See if there is any insurance
            double insurance = XPathUtility.Evaluate(xpath, "Cell[@Name='*ORDER INSURANCE FEE*']", 0.0);
            if (insurance != 0)
            {
                LoadCharge(order, "Insurance", "Insurance", insurance);
            }

            // Add surchage if any
            if (surcharge != 0)
            {
                LoadCharge(order, "Surchage", "Surchage", surcharge);
            }

            // Load handling if its nonzero
            if (handling != 0)
            {
                LoadCharge(order, "Handling", "Handling", handling);
            }

            // Always load shipping
            LoadCharge(order, "Shipping", "Shipping", shipping);

            // If there is a coupon, add it as a charge
            if (couponName.Length > 0)
            {
                LoadCharge(order, "COUPON", "Coupon '" + couponName + "'", -discount);
            }
        }
        /// <summary>
        /// Load the charge for the order
        /// </summary>
        private void LoadCharge(OrderEntity order, string type, string name, double amount)
        {
            if (name.Length == 0)
            {
                name = type;
            }

            OrderChargeEntity charge = InstantiateOrderCharge(order);

            charge.Type = type.ToUpper(CultureInfo.InvariantCulture);
            charge.Description = name;
            charge.Amount = (decimal)amount;
        }

        /// <summary>
        /// Loads order items
        /// </summary>
        private void LoadItem(InfopiaWebClient client, OrderEntity order, XPathNavigator xpath)
        {
            InfopiaOrderItemEntity item = (InfopiaOrderItemEntity)InstantiateOrderItem(order);

            item.Name = XPathUtility.Evaluate(xpath, "Cell[@Name='*ORDER PRODUCT LINE TITLE*']", "");
            item.Code = XPathUtility.Evaluate(xpath, "Cell[@Name='*ORDER PRODUCT LINE PRODUCT CODE*']", "");
            item.SKU = item.Code;
            item.Quantity = XPathUtility.Evaluate(xpath, "Cell[@Name='*ORDER PRODUCT LINE QUANTITY*']", 0);
            item.UnitPrice = XPathUtility.Evaluate(xpath, "Cell[@Name='*ORDER PRODUCT LINE PRICE*']", 0.0M);
            item.UnitCost = XPathUtility.Evaluate(xpath, "Cell[@Name='*ORDER PRODUCT LINE COST*']", 0.0M);

            // Infopia-specific
            item.Marketplace = XPathUtility.Evaluate(xpath, "Cell[@Name='*ORDER PRODUCT LINE MARKETPLACE*']", "");
            item.MarketplaceItemID = XPathUtility.Evaluate(xpath, "Cell[@Name='*ORDER PRODUCT LINE MARKETPLACE ID*']", "");
            item.BuyerID = XPathUtility.Evaluate(xpath, "Cell[@Name='*ORDER PRODUCT LINE BUYER MARKETPLACE ID*']", "");

            // site, auction, buyer
            InfopiaProductInfo productInfo = client.GetProductInfo(item.Code);

            // product details
            item.Weight = productInfo.WeightLbs;
            item.Image = productInfo.ImageUrl;
            item.Thumbnail = productInfo.ImageUrl;
            item.ISBN = productInfo.Isbn;
            item.UPC = productInfo.Upc;
            item.Location = productInfo.Location;

            decimal optionTotal = 0;

            // now load the options
            XPathNodeIterator options = xpath.Select("Attribute");
            while (options.MoveNext())
            {
                optionTotal += LoadOption(item, options.Current);
            }

            item.UnitPrice -= optionTotal;
        }

        /// <summary>
        /// Loads item options
        /// </summary>
        private decimal LoadOption(InfopiaOrderItemEntity item, XPathNavigator xpath)
        {
            OrderItemAttributeEntity option = InstantiateOrderItemAttribute(item);

            option.Name = XPathUtility.Evaluate(xpath, "Cell[@Name='*ORDER ATTRIBUTE LINE NAME*']", "");
            option.Description = XPathUtility.Evaluate(xpath, "Cell[@Name='*ORDER ATTRIBUTE LINE VALUE*']", "");
            option.UnitPrice  = XPathUtility.Evaluate(xpath, "Cell[@Name='*ORDER ATTRIBUTE LINE PRICE*']", 0.0M);

            return option.UnitPrice;
        }

        // Load notes from the order
        private void LoadNotes(OrderEntity order, XPathNavigator xpath)
        {
            // Customer comments
            string customerComments = XPathUtility.Evaluate(xpath, "Cell[@Name='*ORDER NOTES*']", "");
            if (customerComments.Length > 0)
            {
                InstantiateNote(order, CleanNoteText(customerComments), order.OrderDate, NoteVisibility.Public);
            }

            // private notes
            XPathNodeIterator notes = xpath.Select("Note");
            while (notes.MoveNext())
            {
                XPathNavigator xpathNote = notes.Current;

                string note = "";

                note += XPathUtility.Evaluate(xpathNote, "Cell[@Name='*ORDER NOTE LINE TYPE*']", "") + ":\n";
                note += XPathUtility.Evaluate(xpathNote, "Cell[@Name='*ORDER NOTE LINE NOTE*']", "");

                // Pointless note
                if (note.IndexOf("Buyer's IP Number") != -1)
                {
                    continue;
                }

                InstantiateNote(order, CleanNoteText(note), order.OrderDate, NoteVisibility.Internal);
            }
        }

        /// <summary>
        /// Removes characters we don't want
        /// </summary>
        private string CleanNoteText(string infopiaNote)
        {
            // Infopia's new line indicator
            string note = infopiaNote.Replace("~}", "\n");
            note = note.Replace("<br>", "\n");
            note = note.Replace("&nbsp;", " ");
            note = note.Replace("\r\n", "\n");

            return note;
        }

        /// <summary>
        /// Load billing and shipping addresses from the provided xpath navigator
        /// </summary>
        private void LoadAddresses(OrderEntity order, XPathNavigator xpath)
        {
            LoadAddress(order, xpath, "Bill", "BILLING");
            LoadAddress(order, xpath, "Ship", "SHIPPING");
        }

        /// <summary>
        /// Loads an address
        /// </summary>
        private void LoadAddress(OrderEntity order, XPathNavigator xpath, string dbPrefix, string xmlAddressType)
        {
            order.SetNewFieldValue(dbPrefix + "FirstName", XPathUtility.Evaluate(xpath, "Cell[@Name='*ORDER " + xmlAddressType + " FIRST NAME*']", ""));
            order.SetNewFieldValue(dbPrefix + "LastName", XPathUtility.Evaluate(xpath, "Cell[@Name='*ORDER " + xmlAddressType + " LAST NAME*']", ""));
            order.SetNewFieldValue(dbPrefix + "NameParseStatus", (int)PersonNameParseStatus.Simple);

            order.SetNewFieldValue(dbPrefix + "Company", XPathUtility.Evaluate(xpath, "Cell[@Name='*ORDER " + xmlAddressType + " COMPANY*']", ""));
            order.SetNewFieldValue(dbPrefix + "Street1", XPathUtility.Evaluate(xpath, "Cell[@Name='*ORDER " + xmlAddressType + " STREET*']", ""));
            order.SetNewFieldValue(dbPrefix + "Street2", XPathUtility.Evaluate(xpath, "Cell[@Name='*ORDER " + xmlAddressType + " STREET2*']", ""));

            order.SetNewFieldValue(dbPrefix + "City", XPathUtility.Evaluate(xpath, "Cell[@Name='*ORDER " + xmlAddressType + " CITY*']", ""));
            order.SetNewFieldValue(dbPrefix + "StateProvCode", Geography.GetStateProvCode(XPathUtility.Evaluate(xpath, "Cell[@Name='*ORDER " + xmlAddressType + " STATE-REGION*']", "")));
            order.SetNewFieldValue(dbPrefix + "PostalCode", XPathUtility.Evaluate(xpath, "Cell[@Name='*ORDER " + xmlAddressType + " ZIP-POSTAL CODE*']", ""));
            order.SetNewFieldValue(dbPrefix + "CountryCode", Geography.GetCountryCode(XPathUtility.Evaluate(xpath, "Cell[@Name='*ORDER " + xmlAddressType + " COUNTRY*']", "")));

            order.SetNewFieldValue(dbPrefix + "Phone", XPathUtility.Evaluate(xpath, "Cell[@Name='*ORDER " + xmlAddressType + " PHONE*']", ""));
            order.SetNewFieldValue(dbPrefix + "Email", XPathUtility.Evaluate(xpath, "Cell[@Name='*ORDER " + xmlAddressType + " EMAIL*']", ""));
        }
    }
}
