using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Stores.Communication;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;
using System.Xml.Linq;
using System.Xml.XPath;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Content;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Stores.Platforms.Sears
{
    /// <summary>
    /// Downloader for sears.com
    /// </summary>
    public class SearsDownloader : StoreDownloader
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(SearsDownloader));

        /// <summary>
        /// Constructor
        /// </summary>
        public SearsDownloader(SearsStoreEntity store)
            : base(store)
        {

        }

        /// <summary>
        /// Downloader for sears
        /// </summary>
        protected override void Download()
        {
            Progress.Detail = "Checking for orders...";

            DateTime? orderDate = GetOrderDateStartingPoint();
            if (!orderDate.HasValue)
            {
                // If they chose to go back forever, still limit to 30 days
                orderDate = DateTime.UtcNow.AddDays(-30);
            }
            else
            {
                DateTime minDaysBack = DateTime.UtcNow.AddDays(-7);

                // Go back at least 7 days, so that we can get updated status information for stuff that has changed
                orderDate = (minDaysBack < orderDate.Value) ? minDaysBack : orderDate.Value;
            }

            SearsWebClient client = new SearsWebClient((SearsStoreEntity) Store);
            client.InitializeForDownload(orderDate.Value);

            try
            {
                Progress.Detail = "Downloading orders...";

                while (DownloadNextOrdersPage(client))
                {
                    // check for cancellation
                    if (Progress.IsCancelRequested)
                    {
                        return;
                    }
                }

                Progress.Detail = "Done";
            }
            catch (SearsException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
            catch (SqlForeignKeyException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Downloads and imports the next batch of orders into ShipWorks
        /// </summary>
        private bool DownloadNextOrdersPage(SearsWebClient webClient)
        {
            SearsOrdersPage page = webClient.GetNextOrdersPage();

            Progress.PercentComplete = (page.PageNumber * 100) / page.TotalPages;

            XPathNodeIterator orderNodes = page.XPathResponse.Select("//purchase-order");

            // see if there are any orders in the response
            if (orderNodes.Count > 0)
            {
                // import the downloaded orders
                LoadOrders(orderNodes);
            }

            return !page.IsLastPage;
        }

        /// <summary>
        /// Imports the orders contained in the iterator
        /// </summary>
        private void LoadOrders(XPathNodeIterator orderNodes)
        {
            // go through each order in the batch
            while (orderNodes.MoveNext())
            {
                // check for cancel again
                if (Progress.IsCancelRequested)
                {
                    return;
                }

                XPathNavigator order = orderNodes.Current.Clone();
                LoadOrder(order);
            }
        }

        /// <summary>
        /// Load the given order
        /// </summary>
        private void LoadOrder(XPathNavigator xpath)
        {
            // extract the order number
            long orderNumber = XPathUtility.Evaluate(xpath, "customer-order-confirmation-number", 0L);
            string poNumber = XPathUtility.Evaluate(xpath, "po-number", "");

            // get the order instance, creates one if neccessary
            SearsOrderEntity order = (SearsOrderEntity) InstantiateOrder(new SearsOrderIdentifier(orderNumber, poNumber));

            order.OrderDate = GetOrderDate(xpath);
            order.OnlineCustomerID = null;

            // Sears specific
            order.PoNumberWithDate = XPathUtility.Evaluate(xpath, "po-number-with-date", "");
            order.LocationID = XPathUtility.Evaluate(xpath, "location-id", 0);
            order.Commission = XPathUtility.Evaluate(xpath, "total-commission", 0m);
            order.CustomerPickup = XPathUtility.Evaluate(xpath, "count(lmp-details)", 0) > 0;

            order.OnlineStatus = XPathUtility.Evaluate(xpath, "po-status", "");
            order.RequestedShipping = XPathUtility.Evaluate(xpath, "shipping-detail/shipping-method", "");

            // shipping/billing address
            LoadAddressInfo(order, xpath);

            // Items
            XPathNodeIterator itemNodes = xpath.Select("po-line");

            // do the remaining only on new orders
            if (order.IsNew)
            {
                while (itemNodes.MoveNext())
                {
                    LoadItem(order, itemNodes.Current);
                }

                // Load all of the charges
                LoadOrderCharges(order, xpath);

                order.OrderTotal = OrderUtility.CalculateTotal(order);
            }
            else
            {
                // We have to update existing items online status
                UpdateItems(order, itemNodes);
            }

            // save it
            SqlAdapterRetry<SqlException> retryAdapter = new SqlAdapterRetry<SqlException>(5, -5, "SearsDownloader.LoadOrder");
            retryAdapter.ExecuteWithRetry(() => SaveDownloadedOrder(order));

        }

        /// <summary>
        /// Determine the order date from the given order XPath
        /// </summary>
        private DateTime GetOrderDate(XPathNavigator xpath)
        {
            string date = XPathUtility.Evaluate(xpath, "po-date", "");
            string time = XPathUtility.Evaluate(xpath, "po-time", "");

            DateTime searsDate = DateTime.Parse(date + " " + time);

            return SearsUtility.ConvertSearsTimeZoneToUTC(searsDate);
        }

        /// <summary>
        /// Load the address info from the given order node into the order
        /// </summary>
        private void LoadAddressInfo(SearsOrderEntity order, XPathNavigator xpath)
        {
            string customerEmail = XPathUtility.Evaluate(xpath, "customer-email", "");

            // Ship Name
            PersonName shipName = PersonName.Parse(XPathUtility.Evaluate(xpath, "shipping-detail/ship-to-name", ""));
            order.ShipFirstName = shipName.First;
            order.ShipMiddleName = shipName.Middle;
            order.ShipLastName = shipName.Last;
            order.ShipNameParseStatus = (int) shipName.ParseStatus;
            order.ShipUnparsedName = shipName.UnparsedName;

            // Ship Email (Using customer)
            order.ShipEmail = customerEmail;

            // Ship Address
            order.ShipStreet1 = XPathUtility.Evaluate(xpath, "shipping-detail/address", "");
            order.ShipCity = XPathUtility.Evaluate(xpath, "shipping-detail/city", "");
            order.ShipStateProvCode = Geography.GetStateProvCode(XPathUtility.Evaluate(xpath, "shipping-detail/state", ""));
            order.ShipPostalCode = XPathUtility.Evaluate(xpath, "shipping-detail/zipcode", "");
            order.ShipCountryCode = "US";
            order.ShipPhone = XPathUtility.Evaluate(xpath, "shipping-detail/phone", "");
            
            // Use the shipping info for the customer info as well
            PersonAdapter.Copy(order, "Ship", order, "Bill");

            // Then override with the customer name
            PersonName billName = PersonName.Parse(XPathUtility.Evaluate(xpath, "customer-name", ""));
            order.BillFirstName = billName.First;
            order.BillMiddleName = billName.Middle;
            order.BillLastName = billName.Last;
            order.BillNameParseStatus = (int) billName.ParseStatus;
            order.BillUnparsedName = billName.UnparsedName;
        }

        /// <summary>
        /// Load all item informatino from the given XPath into the order
        /// </summary>
        private void LoadItem(SearsOrderEntity order, XPathNavigator xpath)
        {
            XPathNavigator header = xpath.SelectSingleNode("po-line-header");
            XPathNavigator detail = xpath.SelectSingleNode("po-line-detail");

            SearsOrderItemEntity item = (SearsOrderItemEntity) InstantiateOrderItem(order);

            // Sears specific
            item.LineNumber = XPathUtility.Evaluate(header, "line-number", 0);
            item.ItemID = XPathUtility.Evaluate(header, "item-id", "");
            item.Commission = XPathUtility.Evaluate(header, "commission", 0m);
            item.Shipping = XPathUtility.Evaluate(header, "shipping-and-handling", 0m);
            item.OnlineStatus = XPathUtility.Evaluate(detail, "po-line-status", "");

            item.Name = XPathUtility.Evaluate(header, "item-name", "");
            item.SKU = item.ItemID;
            item.Code = item.ItemID;
            item.Quantity = XPathUtility.Evaluate(detail, "quantity", 0);
            item.UnitPrice = XPathUtility.Evaluate(header, "selling-price-each", 0m);
        }

        /// <summary>
        /// Update exsting items online status
        /// </summary>
        private void UpdateItems(SearsOrderEntity order, XPathNodeIterator itemNodes)
        {
            // Fetch all the items for the order
            List<SearsOrderItemEntity> items = DataProvider.GetRelatedEntities(order.OrderID, EntityType.OrderItemEntity).OfType<SearsOrderItemEntity>().ToList();

            while (itemNodes.MoveNext())
            {
                int lineNumber = XPathUtility.Evaluate(itemNodes.Current, "po-line-header/line-number", 0);

                SearsOrderItemEntity orderItem = items.FirstOrDefault(i => i.LineNumber == lineNumber);
                if (orderItem != null)
                {
                    orderItem.OnlineStatus = XPathUtility.Evaluate(itemNodes.Current, "po-line-detail/po-line-status", "");

                    // Attach it to the order to be saved when the order is saved
                    order.OrderItems.Add(orderItem);
                }
            }
        }

        /// <summary>
        /// Load all charges from the given XPath into the order
        /// </summary>
        private void LoadOrderCharges(SearsOrderEntity order, XPathNavigator xpath)
        {
            LoadCharge(order, "Tax", XPathUtility.Evaluate(xpath, "sales-tax", 0m));
            LoadCharge(order, "Shipping", XPathUtility.Evaluate(xpath, "total-shipping-handling", 0m));
        }

        /// <summary>
        /// Load the charge with the given name for the order
        /// </summary>
        private void LoadCharge(SearsOrderEntity order, string name, decimal amount)
        {
            OrderChargeEntity charge = InstantiateOrderCharge(order);

            charge.Type = name.ToUpperInvariant();
            charge.Description = name;
            charge.Amount = amount;
        }
    }
}
