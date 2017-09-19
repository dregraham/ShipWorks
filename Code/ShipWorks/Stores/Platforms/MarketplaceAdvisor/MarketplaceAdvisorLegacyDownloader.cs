using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor
{
    /// <summary>
    /// Downloader for legacy MarketplaceAdvisor store types
    /// </summary>
    /// <remarks>
    /// THIS STORE IS DEAD
    /// This store is scheduled for removal as it no longer exists. Do not update this store when making
    /// all-platform changes.
    /// </remarks>
    [Component]
    public class MarketplaceAdvisorLegacyDownloader : StoreDownloader, IMarketplaceAdvisorLegacyDownloader
    {
        static readonly ILog log = LogManager.GetLogger(typeof(MarketplaceAdvisorLegacyDownloader));

        /// <summary>
        /// Constructor
        /// </summary>
        public MarketplaceAdvisorLegacyDownloader(MarketplaceAdvisorStoreEntity store)
            : base(store)
        {
            MethodConditions.EnsureArgumentIsNotNull(store, nameof(store));
        }

        /// <summary>
        /// Download the orders
        /// </summary>
        /// <param name="trackedDurationEvent">The telemetry event that can be used to
        /// associate any store-specific download properties/metrics.</param>
        protected override async Task Download(TrackedDurationEvent trackedDurationEvent)
        {
            try
            {
                Progress.Detail = "Checking for orders...";

                int currentPage = 1;

                // Keep going until no more to download
                while (true)
                {
                    // Check if it has been canceled
                    if (Progress.IsCancelRequested)
                    {
                        return;
                    }

                    bool morePages = await DownloadNextOrdersPage(currentPage++).ConfigureAwait(false);
                    if (!morePages)
                    {
                        return;
                    }
                }
            }
            catch (MarketplaceAdvisorException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
            catch (SqlForeignKeyException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Download the next page of MarketplaceAdvisor orders.  Returns false if there are no more orders to download.
        /// </summary>
        private async Task<bool> DownloadNextOrdersPage(int currentPage)
        {
            MarketplaceAdvisorLegacyClient client = MarketplaceAdvisorLegacyClient.Create((MarketplaceAdvisorStoreEntity) Store);
            XElement ordersResponse = XElement.Parse(client.GetOrders(currentPage));

            XPathNavigator xpath = ordersResponse.CreateNavigator();
            XPathNodeIterator orders = xpath.Select("//Order");

            if (orders.Count == 0)
            {
                if (currentPage == 1)
                {
                    Progress.Detail = "No orders to download.";
                    Progress.PercentComplete = 100;
                    return false;
                }
                else
                {
                    Progress.Detail = "Done";
                    return false;
                }
            }
            else
            {
                await LoadOrders(xpath).ConfigureAwait(false);

                return true;
            }
        }

        /// <summary>
        /// Load all the orders contained in the iterator
        /// </summary>
        private async Task LoadOrders(XPathNavigator xpath)
        {
            XPathNodeIterator orders = xpath.Select("//Order");

            List<long> markAsProcessed = new List<long>();

            int pageSize = XPathUtility.Evaluate(xpath, "//PerPage", 1);
            int currentPage = XPathUtility.Evaluate(xpath, "//PageNumber", 1);
            int totalPages = XPathUtility.Evaluate(xpath, "//TotalPages", 1);

            bool totalIsEstimated;
            int totalRecords;

            // Equals the page size - may not be the last page
            if (currentPage < totalPages)
            {
                totalIsEstimated = true;
                totalRecords = (totalPages - 1) * pageSize;
            }
            else
            {
                totalIsEstimated = false;
                totalRecords = (totalPages - 1) * pageSize + orders.Count;
            }

            // Go through each order in the XML Document
            while (orders.MoveNext())
            {
                // check for cancel
                if (Progress.IsCancelRequested)
                {
                    // We can't just return, since we still have to mark the ones we did download as processed
                    break;
                }

                // Update the status
                Progress.Detail = string.Format("Processing order {0} of {1}{2}...",
                    QuantitySaved + 1,
                    totalIsEstimated ? "about " : "",
                    totalRecords);

                XPathNavigator order = orders.Current.Clone();
                GenericResult<long> orderNumber = await LoadOrder(order).ConfigureAwait(false);

                if (orderNumber.Success)
                {
                    markAsProcessed.Add(orderNumber.Value);
                }

                // update the status
                Progress.PercentComplete = 100 * QuantitySaved / totalRecords;
            }

            if (MarketplaceAdvisorUtility.MarkProcessedAfterDownload)
            {
                if (markAsProcessed.Count > 0)
                {
                    MarketplaceAdvisorLegacyClient client = MarketplaceAdvisorLegacyClient.Create((MarketplaceAdvisorStoreEntity) Store);
                    client.MarkOrdersProcessed(markAsProcessed);
                }
            }
        }

        /// <summary>
        /// Extract the order from the XML
        /// </summary>
        private async Task<GenericResult<long>> LoadOrder(XPathNavigator xpath)
        {
            // Now extract the Order#
            long orderNumber = XPathUtility.Evaluate(xpath, "Number", (long) 0);

            // Create a new order instance
            GenericResult<OrderEntity> result = await InstantiateOrder(new MarketplaceAdvisorOrderNumberIdentifier(orderNumber)).ConfigureAwait(false);
            if (result.Failure)
            {
                log.InfoFormat("Skipping order '{0}': {1}.", orderNumber, result.Message);
                return GenericResult.FromError<long>(result.Message);
            }

            MarketplaceAdvisorOrderEntity order = (MarketplaceAdvisorOrderEntity) result.Value;

            // MarketplaceAdvisor sends shit down locally to their own server, stupid
            DateTime date = DateTime.Parse(XPathUtility.Evaluate(xpath, "Date", ""));
            date += TimeSpan.FromHours(4);

            // Setup the basic properties
            order.OrderNumber = orderNumber;
            order.OrderDate = date;

            order.BuyerNumber = XPathUtility.Evaluate(xpath, "Buyer/BuyerNumber", (long) 0);
            order.InvoiceNumber = XPathUtility.Evaluate(xpath, "InvoiceNumber", "");
            order.SellerOrderNumber = XPathUtility.Evaluate(xpath, "UserNumber", (long) 0);

            // Only used for OMS
            order.ParcelID = 0;

            // Requested shipping
            order.RequestedShipping = XPathUtility.Evaluate(xpath, "LineItems/Item[1]/ShipProfileMethodName", "");

            if (order.IsNew)
            {
                // Notes
                string notes = XPathUtility.Evaluate(xpath, "Notes", "");
                if (notes.Length > 0)
                {
                    await InstantiateNote(order, notes, order.OrderDate, NoteVisibility.Public).ConfigureAwait(false);
                }

                // Load address info
                LoadAddressInfo(order, xpath);

                // Items
                XPathNodeIterator itemNodes = xpath.Select("LineItems/Item");
                while (itemNodes.MoveNext())
                {
                    LoadItem(order, itemNodes.Current);
                }

                // Load all the charges
                LoadOrderCharges(order, xpath);

                // Payment details
                LoadPaymentDetails(order, xpath);

                // Update the total
                order.OrderTotal = OrderUtility.CalculateTotal(order);
            }

            // Save the order
            SqlAdapterRetry<SqlException> retryAdapter = new SqlAdapterRetry<SqlException>(5, -5, "MarketplaceAdvisorLegacyDownloader.LoadOrder");
            await retryAdapter.ExecuteWithRetryAsync(() => SaveDownloadedOrder(order)).ConfigureAwait(false);

            return GenericResult.FromSuccess(orderNumber);
        }

        /// <summary>
        /// Load the payment details for the order.
        /// </summary>
        private void LoadPaymentDetails(MarketplaceAdvisorOrderEntity order, XPathNavigator xpath)
        {
            string methodCode = XPathUtility.Evaluate(xpath, "Payment/MethodCode", "");

            // Extras for CC
            if (MarketplaceAdvisorUtility.IsMethodCC(methodCode))
            {
                string auth = XPathUtility.Evaluate(xpath, "Payment/CreditCard/CCAuthorizationCode", "");
                if (auth.Length > 0)
                {
                    LoadPaymentDetail(order, "Authorization", auth);
                }

                LoadPaymentDetail(order, "Verification", XPathUtility.Evaluate(xpath, "Payment/CreditCard/CCVerificationNumber", ""));

                LoadPaymentDetail(order, "Expiration",
                    XPathUtility.Evaluate(xpath, "Payment/CreditCard/CCExpMonth", "") + "/" +
                    XPathUtility.Evaluate(xpath, "Payment/CreditCard/CCExpYear", ""));

                LoadPaymentDetail(order, "Cardholder", XPathUtility.Evaluate(xpath, "Payment/CreditCard/CCName", ""));

                LoadPaymentDetail(order, "Card Number", XPathUtility.Evaluate(xpath, "Payment/CreditCard/CCNumber", ""));
            }

            // Extras for PayPal
            if (MarketplaceAdvisorUtility.IsMethodPayPal(methodCode))
            {
                LoadPaymentDetail(order, "Authorization", XPathUtility.Evaluate(xpath, "Payment/Paypal/AuthorizationCode", ""));

                string transID = XPathUtility.Evaluate(xpath, "Payment/Paypal/TransactionID", "");
                transID = transID.Replace("Paypal Transaction ID#", "");
                LoadPaymentDetail(order, "Transaction ID", transID);
            }

            // Create a detail item for the method
            LoadPaymentDetail(order, "Method", MarketplaceAdvisorUtility.GetMethodName(methodCode));
        }

        /// <summary>
        /// Load the payment detail for the order
        /// </summary>
        private void LoadPaymentDetail(MarketplaceAdvisorOrderEntity order, string name, string data)
        {
            // Handle new OMS case where CC data sometimes comes in null
            if (data == null)
            {
                return;
            }

            OrderPaymentDetailEntity detail = InstantiateOrderPaymentDetail(order);

            detail.Label = name;
            detail.Value = data;
        }

        /// <summary>
        /// Load all charges for the order
        /// </summary>
        private void LoadOrderCharges(MarketplaceAdvisorOrderEntity order, XPathNavigator xpath)
        {
            // Apparently this is already included in the total.
            // LoadCharge(order, "ClickOut", "ClickOut", XPathUtility.Evaluate(xpath, "ClickOutTotal", 0.0), true);

            // Apparently so is this
            // LoadCharge(order, "Handling", "Handling", XPathUtility.Evaluate(xpath, "HandlingTotal", 0.0), true);

            double discount = XPathUtility.Evaluate(xpath, "DiscountTotal", 0.0);
            LoadCharge(order, "Discount", "Discount", -discount, true);

            LoadCharge(order, "Insurance", "Insurance", XPathUtility.Evaluate(xpath, "InsuranceTotal", 0.0), true);

            // Shipping
            LoadCharge(order, "Shipping", "Shipping", XPathUtility.Evaluate(xpath, "ShippingTotal", 0.0), false);

            // Tax
            LoadCharge(order, "Tax", "Tax", XPathUtility.Evaluate(xpath, "TaxTotal", 0.0), false);
        }

        /// <summary>
        /// Load the charge for the order
        /// </summary>
        private void LoadCharge(MarketplaceAdvisorOrderEntity order, string type, string name, double amount, bool ignoreZeroAmount)
        {
            if (amount.IsEquivalentTo(0) && ignoreZeroAmount)
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
            charge.Amount = (decimal) amount;
        }

        /// <summary>
        /// Load the item information into the order
        /// </summary>
        private void LoadItem(MarketplaceAdvisorOrderEntity order, XPathNavigator xpath)
        {
            OrderItemEntity item = InstantiateOrderItem(order);

            int itemNumber = XPathUtility.Evaluate(xpath, "ItemNumber", 0);

            item.Code = XPathUtility.Evaluate(xpath, "StockNumber", "");
            item.Name = XPathUtility.Evaluate(xpath, "Title", "");
            item.Quantity = XPathUtility.Evaluate(xpath, "Quantity", 0);
            item.UnitPrice = XPathUtility.Evaluate(xpath, "PriceEach", (decimal) 0.0);
            item.Location = XPathUtility.Evaluate(xpath, "Location", "");

            // Get the details for the item
            MarketplaceAdvisorLegacyClient client = MarketplaceAdvisorLegacyClient.Create((MarketplaceAdvisorStoreEntity) Store);
            MarketplaceAdvisorInventoryItem inventoryItem = client.GetInventoryItem(itemNumber);

            item.Description = inventoryItem.Description;
            item.Image = inventoryItem.ImageUrl;
            item.Thumbnail = inventoryItem.ImageUrl;
            item.Weight = inventoryItem.WeightLbs;
            item.UnitCost = inventoryItem.Cost;
            item.ISBN = inventoryItem.ISBN;
            item.SKU = inventoryItem.SKU;
            item.UPC = inventoryItem.UPC;
        }

        /// <summary>
        /// Load the appropriate address info from the XPath
        /// </summary>
        private void LoadAddressInfo(MarketplaceAdvisorOrderEntity order, XPathNavigator xpath)
        {
            LoadAddressInfo(new PersonAdapter(order, "Ship"), xpath, "ShipTo");
            LoadAddressInfo(new PersonAdapter(order, "Bill"), xpath, "BillTo");

            // If a bunch are blank, assume it wasnt filled in.
            if (order.BillLastName.Length == 0 &&
                order.BillStreet1.Length == 0 &&
                order.BillCity.Length == 0 &&
                order.BillPostalCode.Length == 0)
            {
                PersonAdapter.Copy(order, "Ship", order, "Bill");
            }

            order.BillEmail = XPathUtility.Evaluate(xpath, "Buyer/EmailAddress", "");

            // MW does not currently have company for billing.  If ship\bill look the same, go ahead
            // and copy it over
            if (order.BillLastName == order.ShipLastName && order.BillStreet1 == order.ShipStreet1)
            {
                order.ShipEmail = order.BillEmail;
            }
        }

        /// <summary>
        /// Load the address info out of the given xpath node into the person
        /// </summary>
        private void LoadAddressInfo(PersonAdapter person, XPathNavigator xpath, string node)
        {
            string nodeBase = string.Format("Buyer/{0}/", node);

            person.Company = XPathUtility.Evaluate(xpath, nodeBase + "CompanyName", "");
            person.FirstName = XPathUtility.Evaluate(xpath, nodeBase + "FirstName", "");
            person.LastName = XPathUtility.Evaluate(xpath, nodeBase + "LastName", "");
            person.NameParseStatus = PersonNameParseStatus.Simple;
            person.Street1 = XPathUtility.Evaluate(xpath, nodeBase + "AddressLine1", "");
            person.Street2 = XPathUtility.Evaluate(xpath, nodeBase + "AddressLine2", "");
            person.City = XPathUtility.Evaluate(xpath, nodeBase + "City", "");
            person.StateProvCode = Geography.GetStateProvCode(XPathUtility.Evaluate(xpath, nodeBase + "State", ""));
            person.PostalCode = XPathUtility.Evaluate(xpath, nodeBase + "ZipCode", "");
            person.CountryCode = Geography.GetCountryCode(XPathUtility.Evaluate(xpath, nodeBase + "Country", ""));
            person.Phone = XPathUtility.Evaluate(xpath, nodeBase + "PhoneNumber", "");
            person.Fax = XPathUtility.Evaluate(xpath, nodeBase + "FaxNumber", "");

            // Do province
            string shipProv = Geography.GetStateProvCode(XPathUtility.Evaluate(xpath, nodeBase + "Province", ""));
            if (shipProv.Length > 0)
            {
                if (person.StateProvCode.Length == 0)
                {
                    person.StateProvCode = shipProv;
                }
                else
                {
                    person.Street3 = shipProv;
                }
            }
        }
    }
}
