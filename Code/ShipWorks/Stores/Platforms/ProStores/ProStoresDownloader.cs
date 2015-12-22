using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Stores.Communication;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Net;
using ShipWorks.Data.Connection;
using System.Xml;
using System.Xml.XPath;
using System.Threading;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Content;
using Interapptive.Shared.Business;
using System.Text.RegularExpressions;
using Interapptive.Shared;
using Interapptive.Shared.Business.Geography;

namespace ShipWorks.Stores.Platforms.ProStores
{
    /// <summary>
    /// Downloader for ProStores stores
    /// </summary>
    public class ProStoresDownloader : StoreDownloader
    {
        // total download count
        int totalCount = 0;

        // Be optimistic
        bool isProVersion = true;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProStoresDownloader(ProStoresStoreEntity store)
            : base(store)
        {

        }

        /// <summary>
        /// Download orders from the ProStores online store
        /// </summary>
        protected override void Download()
        {
            try
            {
                Progress.Detail = "Checking for orders...";

                // For legacy login methods, checks the version has been updatd and tokens are now supported
                ProStoresWebClient.CheckTokenLoginMethodAvailability((ProStoresStoreEntity) Store);
                
                // Downloading baed on the last modified time
                DateTime? lastModified = GetOnlineLastModifiedStartingPoint();

                totalCount = ProStoresWebClient.GetOrderCount((ProStoresStoreEntity) Store, lastModified);

                if (totalCount == 0)
                {
                    Progress.Detail = "No orders to download.";
                    Progress.PercentComplete = 100;
                    return;
                }

                Progress.Detail = string.Format("Downloading {0} orders...", totalCount);

                // keep going until none are left
                while (true)
                {
                    // Check if it has been cancelled
                    if (Progress.IsCancelRequested)
                    {
                        return;
                    }

                    if (!DownloadNextOrdersPage())
                    {
                        return;
                    }
                }
            }
            catch (ProStoresException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
            catch (SqlForeignKeyException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Download the next page of orders until there are no more
        /// </summary>
        private bool DownloadNextOrdersPage()
        {
            try
            {
                XmlDocument response = ProStoresWebClient.GetNextOrderPage((ProStoresStoreEntity) Store, GetOnlineLastModifiedStartingPoint(), isProVersion);
                XPathNavigator xpath = response.CreateNavigator();

                XPathNodeIterator invoiceIterator = xpath.Select("/XTE/Response/Invoice");

                // see if there are any orders in the response
                if (invoiceIterator.Count > 0)
                {
                    LoadOrders(invoiceIterator);
                    return true;
                }
                else
                {
                    Progress.Detail = "Done";
                    return false;
                }
                
            }
            catch (ProStoresApiException ex)
            {
                // If we tried with gift wrap and failed, try again without
                if (isProVersion && ex.Type == "TextStoreAccessFailed")
                {
                    isProVersion = false;

                    return DownloadNextOrdersPage();
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Load all the "Invoices" out of the given iterator into ShipWorks orders
        /// </summary>
        private void LoadOrders(XPathNodeIterator invoiceIterator)
        {
            foreach (XPathNavigator xpathOrder in invoiceIterator)
            {
                if (Progress.IsCancelRequested)
                {
                    return;
                }

                // Update the status
                Progress.Detail = string.Format("Processing order {0}...", (QuantitySaved + 1));

                LoadOrder(xpathOrder);

                // Update progress
                Progress.PercentComplete = Math.Min(100, 100 * QuantitySaved / totalCount);
            }
        }

        /// <summary>
        /// Parses out the ProStores customer number into an Online Customer ID.
        /// 
        /// Customer Numbers can be changed by the store admin to include a prefix
        /// and a set number of digits in the order number.
        /// 
        /// Guest purchases always end with a dash and a 0 padded representation of -1,
        /// like ABC-000001.
        /// </summary>
        private static object GetOnlineCustomerID(string customerNumber)
        {
            if (Regex.IsMatch(customerNumber, "-0*1$"))
            {
                // -1 so this is a guest checkout
                return null;
            }
            else
            {
                return customerNumber;
            }
        }

        /// <summary>
        /// Extract the order from the xml
        /// </summary>
        [NDependIgnoreLongMethod]
        private void LoadOrder(XPathNavigator xpath)
        {
            // Now extract the Order#
            int orderNumber = XPathUtility.Evaluate(xpath, "InvoiceNumber", 0);

            ProStoresOrderEntity order = (ProStoresOrderEntity) InstantiateOrder(new OrderNumberIdentifier(orderNumber));

            // Setup the basic proprites
            order.OrderNumber = orderNumber;
            order.OrderDate = DateTime.Parse(XPathUtility.Evaluate(xpath, "EnterDate", "")).ToUniversalTime();
            order.OnlineLastModified = DateTime.Parse(XPathUtility.Evaluate(xpath, "LastModifiedDate", "")).ToUniversalTime();
            order.OrderTotal = XPathUtility.Evaluate(xpath, "Total", 0m);

            string customerNumber = XPathUtility.Evaluate(xpath, "Customer/CustomerNumberFull", "");
            order.OnlineCustomerID = GetOnlineCustomerID(customerNumber);

            // Requested shipping
            order.RequestedShipping = XPathUtility.Evaluate(xpath, "ShipMethodDescription", "");

            // Authorization
            if (XPathUtility.Evaluate(xpath, "Authorized", "false") == "true")
            {
                order.AuthorizedBy = XPathUtility.Evaluate(xpath, "AuthorizationBy", "");
                order.AuthorizedDate = DateTime.Parse(XPathUtility.Evaluate(xpath, "AuthorizationDate", "")).ToUniversalTime();
            }
            else
            {
                order.AuthorizedBy = "";
                order.AuthorizedDate = null;
            }

            // Confirmation number
            order.ConfirmationNumber = XPathUtility.Evaluate(xpath, "ConfirmationNumber", "");

            // Mark the order shipped if the status is shipped
            if (XPathUtility.Evaluate(xpath, "Status", "") == "Shipped")
            {
                order.LocalStatus = "Shipped";
            }

            // Load address info
            LoadAddressInfo(order, xpath);

            // Only do the rest of this stuff for brand new orders
            if (order.IsNew)
            {
                // Has gift wrap?
                bool giftWrap = XPathUtility.Evaluate(xpath, "GiftWrapEnabled", false);

                // Items
                foreach (XPathNavigator xpathItem in xpath.Select("Details/InvoiceDetail"))
                {
                    // Gift wrap will not have a product node or a name
                    if (giftWrap && xpathItem.Select("Product").Count == 0 && XPathUtility.Evaluate(xpath, "Name", "").Length == 0)
                    {
                        LoadItemGiftWrap(order, XPathUtility.Evaluate(xpath, "Message", ""), xpathItem);
                    }
                    else
                    {
                        LoadItem(order, xpathItem);
                    }
                } 
                
                // Load all the charges
                LoadOrderCharges(order, xpath);

                // Payment Details
                LoadPaymentDetails(order, xpath);
            }

            // Save the downloaded order
            SqlAdapterRetry<SqlException> retryAdapter = new SqlAdapterRetry<SqlException>(5, -5, "ProStoresDownloader.LoadOrder");
            retryAdapter.ExecuteWithRetry(() => SaveDownloadedOrder(order));
        }

        /// <summary>
        /// Load the appropriate address info from the XPath
        /// </summary>
        [NDependIgnoreLongMethod]
        private void LoadAddressInfo(ProStoresOrderEntity order, XPathNavigator xpath)
        {
            PersonName shipFullName = PersonName.Parse(XPathUtility.Evaluate(xpath, "Recipient", ""));

            order.ShipNameParseStatus = (int)shipFullName.ParseStatus;
            order.ShipUnparsedName = shipFullName.UnparsedName;
            order.ShipFirstName = shipFullName.First;
            order.ShipMiddleName = shipFullName.Middle;
            order.ShipLastName = shipFullName.Last;
            order.ShipCompany = XPathUtility.Evaluate(xpath, "Company", "");
            order.ShipStreet1 = XPathUtility.Evaluate(xpath, "ShipToStreet", "");
            order.ShipStreet2 = XPathUtility.Evaluate(xpath, "ShipToStreet2", "");
            order.ShipCity = XPathUtility.Evaluate(xpath, "ShipToCity", "");
            order.ShipStateProvCode = Geography.GetStateProvCode(XPathUtility.Evaluate(xpath, "ShipToState", ""));
            order.ShipPostalCode = XPathUtility.Evaluate(xpath, "ShipToPostalCode", "");
            order.ShipCountryCode = XPathUtility.Evaluate(xpath, "ShipToCountry/Code", XPathUtility.Evaluate(xpath, "ShipToCountry", ""));
            order.ShipPhone = XPathUtility.Evaluate(xpath, "ShipToPhone", "");

            order.BillFirstName = XPathUtility.Evaluate(xpath, "FirstName", "");
            order.BillLastName = XPathUtility.Evaluate(xpath, "LastName", "");
            order.BillNameParseStatus = (int)PersonNameParseStatus.Simple;
            order.BillUnparsedName = new PersonName(order.BillFirstName, "", order.BillLastName).FullName;
            order.BillCompany = XPathUtility.Evaluate(xpath, "BillToCompany", "");
            order.BillStreet1 = XPathUtility.Evaluate(xpath, "BillToStreet", "");
            order.BillStreet2 = XPathUtility.Evaluate(xpath, "BillToStreet2", "");
            order.BillCity = XPathUtility.Evaluate(xpath, "BillToCity", "");
            order.BillStateProvCode = Geography.GetStateProvCode(XPathUtility.Evaluate(xpath, "BillToState", ""));
            order.BillPostalCode = XPathUtility.Evaluate(xpath, "BillToPostalCode", "");
            order.BillCountryCode = XPathUtility.Evaluate(xpath, "BillToCountry/Code", XPathUtility.Evaluate(xpath, "BillToCountry", ""));
            order.BillPhone = XPathUtility.Evaluate(xpath, "BillToPhone", "");

            // This email would be there for no customer logged in orders
            string email = XPathUtility.Evaluate(xpath, "Email", "");

            // If its not there, check the email for the customer
            if (email.Length == 0)
            {
                email = XPathUtility.Evaluate(xpath, "Customer/Email", "");
            }

            order.BillEmail = email;

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
        /// Load the item information into the order
        /// </summary>
        private void LoadItem(ProStoresOrderEntity order, XPathNavigator xpath)
        {
            OrderItemEntity item = InstantiateOrderItem(order);

            item.Code = XPathUtility.Evaluate(xpath, "SKU", "");

            // SKU is not required. If not present use the numeric ProStores product number
            if (item.Code.Length == 0)
            {
                item.Code = XPathUtility.Evaluate(xpath, "Product/ProductNumber", "");
            }

            item.SKU = XPathUtility.Evaluate(xpath, "SKU", "");
            item.Name = XPathUtility.Evaluate(xpath, "Name", "");
            item.Description = XPathUtility.Evaluate(xpath, "Product/Description", "");
            item.Quantity = XPathUtility.Evaluate(xpath, "Quantity", 0d);
            item.UnitPrice = XPathUtility.Evaluate(xpath, "PriceBeforeTax", 0m);
            item.Weight = XPathUtility.Evaluate(xpath, "Weight", 0d);

            item.ISBN = XPathUtility.Evaluate(xpath, "Product/Isbn", "");
            item.UPC = XPathUtility.Evaluate(xpath, "Product/Upc", "");

            item.Image = XPathUtility.Evaluate(xpath, "Product/MarketplaceDisplayImageUrl", "");
            item.Thumbnail = XPathUtility.Evaluate(xpath, "Product/MarketplaceDisplayImageUrl", "");

            // If attribute level stuff exists
            if (xpath.Select("ProductAttribute").Count > 0)
            {
                item.UnitCost = XPathUtility.Evaluate(xpath, "ProductAttribute/Cost", 0m);
            }
            else
            {
                item.UnitCost = XPathUtility.Evaluate(xpath, "Product/Cost", 0m);
            }

            for (int i = 1; i <= 8; i++)
            {
                string label = XPathUtility.Evaluate(xpath, string.Format("Product/Attribute{0}Label", i), "");
                string value = XPathUtility.Evaluate(xpath, string.Format("AttributeText{0}", i), "");

                // Legacy
                if (value.Length == 0 && i == 1)
                {
                    value = XPathUtility.Evaluate(xpath, "ProductColor", "");
                }

                // Legacy
                if (value.Length == 0 && i == 2)
                {
                    value = XPathUtility.Evaluate(xpath, "ProductSize", "");
                }

                if (label.Length > 0 && value.Length > 0 && value != "All")
                {
                    LoadItemAttribute(item, label, value);
                }
            }
        }

        /// <summary>
        /// Load the specified attribute for the given item
        /// </summary>
        private void LoadItemAttribute(OrderItemEntity item, string label, string value)
        {
            OrderItemAttributeEntity attribute = InstantiateOrderItemAttribute(item);

            attribute.Name = label;
            attribute.Description = value;
            attribute.UnitPrice = 0;
        }

        /// <summary>
        /// Load the gift wrap item
        /// </summary>
        private void LoadItemGiftWrap(ProStoresOrderEntity order, string message, XPathNavigator xpath)
        {
            OrderItemEntity item = InstantiateOrderItem(order);

            item.Quantity = XPathUtility.Evaluate(xpath, "Quantity", 0);
            item.UnitPrice = XPathUtility.Evaluate(xpath, "PriceBeforeTax", 0m);

            item.Name = "Gift Wrap";
            item.Code = "GIFT";

            if (message.Length > 0)
            {
                LoadItemAttribute(item, "Message", message);
            }
        }

        /// <summary>
        /// Load the charges
        /// </summary>
        private void LoadOrderCharges(ProStoresOrderEntity order, XPathNavigator xpath)
        {
            // Charges - Promotion
            string promoCode = XPathUtility.Evaluate(xpath, "PromotionCode", "");
            if (promoCode.Length > 0)
            {
                LoadCharge(order, "Promotion", promoCode, -XPathUtility.Evaluate(xpath, "PromotionDiscount", 0m));
            }

            // Charges - Tax
            LoadCharge(order, "Tax", "Tax", XPathUtility.Evaluate(xpath, "Tax", 0m));

            // Charges - Shipping
            LoadCharge(order, "Shipping", "Shipping", XPathUtility.Evaluate(xpath, "Shipping", 0m));
        }

        /// <summary>
        /// Load the charge for the order
        /// </summary>
        private void LoadCharge(ProStoresOrderEntity order, string type, string name, decimal amount)
        {
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
        /// Load any of the payment details for the order
        /// </summary>
        private void LoadPaymentDetails(ProStoresOrderEntity order, XPathNavigator xpath)
        {
            string creditCard = XPathUtility.Evaluate(xpath, "CreditCard", "");

            if (creditCard.Length > 0)
            {
                LoadPaymentDetail(order, "Card Number", XPathUtility.Evaluate(xpath, "MaskedCreditCardNumber", ""));
                LoadPaymentDetail(order, "Card Holder", XPathUtility.Evaluate(xpath, "CreditCardHolderName", ""));
                LoadPaymentDetail(order, "Card Expiration", XPathUtility.Evaluate(xpath, "CreditCardExpiration", ""));
                LoadPaymentDetail(order, "Card Type", creditCard);
                LoadPaymentDetail(order, "Payment Type", "Credit Card");
            }

            else
            {
                LoadPaymentDetail(order, "Payment Type", XPathUtility.Evaluate(xpath, "PaymentMethod", ""));
            }
        }

        /// <summary>
        /// Load the payment detail for the order
        /// </summary>
        private void LoadPaymentDetail(ProStoresOrderEntity order, string name, string data)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(data))
            {
                return;
            }

            OrderPaymentDetailEntity detail = InstantiateOrderPaymentDetail(order);

            detail.Label = name;
            detail.Value = data;
        }
    }
}
