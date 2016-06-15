using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.IO.Text.HtmlAgilityPack;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.ThreeDCart.RestApi.DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace ShipWorks.Stores.Platforms.ThreeDCart.RestApi
{

    /// <summary>
    /// Downloader for 3dcart that uses their REST API
    /// </summary>
    public class ThreeDCartRestDownloader : StoreDownloader
    {
        const int MissingCustomerID = 0;
        private readonly IThreeDCartRestWebClient restWebClient;
        private readonly ThreeDCartStoreEntity threeDCartStore;
        private readonly ISqlAdapterRetry sqlAdapterRetry;
        private readonly ILog log;
        private int newOrderCount;
        private DateTime modifiedOrderEndDate;
        private int totalCount;
        private int ordersProcessed;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="store">Store for which this downloader will operate</param>
        public ThreeDCartRestDownloader(ThreeDCartStoreEntity store)
            : this(store,
                new ThreeDCartRestWebClient(store),
                new SqlAdapterRetry<SqlException>(5, -5, "ThreeDCartRestDownloader.LoadOrder"),
                LogManager.GetLogger(typeof(ThreeDCartRestDownloader)))
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ThreeDCartRestDownloader"/> class.
        /// </summary>
        public ThreeDCartRestDownloader(ThreeDCartStoreEntity store, IThreeDCartRestWebClient restWebClient, ISqlAdapterRetry sqlAdapterRetry, ILog log) : base(store)
        {
            this.restWebClient = restWebClient;
            this.sqlAdapterRetry = sqlAdapterRetry;
            this.log = log;
            threeDCartStore = store;
        }

        /// <summary>
        /// Download orders for the 3dcart store
        /// </summary>
        protected override void Download()
        {
            try
            {
                // Need to give the web client the progress bar because if
                // we get throttled, we want to display it in the progress
                restWebClient.LoadProgressReporter(Progress);
                ordersProcessed = 0;

                DateTime? startDate = GetOrderDateStartingPoint();
                if (!startDate.HasValue)
                {
                    startDate = DateTime.Today;
                }

                modifiedOrderEndDate = startDate.Value;

                if (threeDCartStore.DownloadModifiedNumberOfDaysBack > 0)
                {
                    startDate = startDate.Value.AddDays(-threeDCartStore.DownloadModifiedNumberOfDaysBack);
                    Progress.Detail = "Checking for new and modified orders...";
                }
                else
                {
                    Progress.Detail = "Checking for new orders...";
                }

                totalCount = restWebClient.GetOrderCount(startDate.Value, 0);
                if (totalCount == 0)
                {
                    Progress.Detail = "Done - No new orders to download.";
                    Progress.PercentComplete = 100;
                    return;
                }

                DownloadOrders(startDate.Value);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (ex.GetType() == typeof(ThreeDCartException) || ex.GetType() == typeof(SqlForeignKeyException))
                {
                    throw new DownloadException(ex.Message, ex);
                }
                throw WebHelper.TranslateWebException(ex, typeof(DownloadException));
            }
        }

        /// <summary>
        /// Downloads orders on or after the startDate
        /// </summary>
        private void DownloadOrders(DateTime startDate)
        {
            int offset = 1;
            bool ordersToDownload = true;

            while (ordersToDownload)
            {
                IEnumerable<ThreeDCartOrder> orders = restWebClient.GetOrders(startDate, offset).ToList();
                if (!orders.Any())
                {
                    Progress.Detail = "Done.";
                    Progress.PercentComplete = 100;
                    ordersToDownload = false;
                }
                else
                {
                    LoadOrders(orders);
                    offset += orders.Count();
                }
            }
        }

        /// <summary>
        /// Creates the order identifier.
        /// </summary>
        /// <remarks>
        /// Extracted from ThreeDCartSoapDownloader
        /// </remarks>
        private ThreeDCartOrderIdentifier CreateOrderIdentifier(ThreeDCartOrder order, string invoiceNumberPostFix)
        {
            // Now extract the Invoice number and ThreeDCart Order Id
            long orderId = order.OrderID;

            // Invoice number is defined as an integer in the 3dcart schema
            // So we can safely remove the prefix to get to a long
            long invoiceNum;
            string invoiceNumber = order.InvoiceNumber.ToString();
            string invoiceNumberPrefix = order.InvoiceNumberPrefix;

            // I've seen invoice number as blank in one of the 3dcart test stores...  so instead of blank, we'll put the 3dcart Order ID
            if (string.IsNullOrWhiteSpace(invoiceNumber))
            {
                invoiceNumber = orderId.ToString();
            }
            else if (!string.IsNullOrWhiteSpace(invoiceNumberPrefix))
            {
                // 3dcart allows you to add a prefix to the invoice number.
                // The legacy order importer stripped the prefix, so we'll do that here too.
                invoiceNumber = invoiceNumber.Replace(invoiceNumberPrefix, string.Empty);
            }

            if (!long.TryParse(invoiceNumber, out invoiceNum))
            {
                log.ErrorFormat($"3dcart returned an invalid invoice number: {invoiceNumber}.");
                throw new ThreeDCartException("3dcart returned an invalid response while downloading orders");
            }

            // Create an order identifier without a prefix.  If we find an order, it must have been downloaded prior to
            // the upgrade.  If an order is found, we will not use the prefix.  If we don't find an order, we'll use the prefix.
            ThreeDCartOrderIdentifier threeDCartOrderIdentifier = new ThreeDCartOrderIdentifier(invoiceNum, string.Empty, invoiceNumberPostFix);
            OrderEntity orderEntity = FindOrder(threeDCartOrderIdentifier);
            if (orderEntity == null)
            {
                threeDCartOrderIdentifier = new ThreeDCartOrderIdentifier(invoiceNum, invoiceNumberPrefix, invoiceNumberPostFix);
            }

            return threeDCartOrderIdentifier;
        }

        /// <summary>
        /// Loads the orders.
        /// </summary>
        public void LoadOrders(IEnumerable<ThreeDCartOrder> orders)
        {
            foreach (ThreeDCartOrder threeDCartOrder in orders)
            {
                // Don't download abandoned orders
                if (threeDCartOrder.InvoiceNumber == 0 &&
                    threeDCartOrder.OrderStatusID == (int) Enums.ThreeDCartOrderStatus.NotCompleted)
                {
                    continue;
                }

                threeDCartOrder.IsSubOrder = false;
                threeDCartOrder.HasSubOrders = threeDCartOrder.ShipmentList.Count() > 1;
                int shipmentIndex = 1;

                foreach (ThreeDCartShipment shipment in threeDCartOrder.ShipmentList)
                {
                    string invoiceNumberPostFix = threeDCartOrder.HasSubOrders ? $"-{shipmentIndex}" : string.Empty;

                    ThreeDCartOrderIdentifier orderIdentifier = CreateOrderIdentifier(threeDCartOrder, invoiceNumberPostFix);

                    Progress.Detail = threeDCartOrder.OrderDate < modifiedOrderEndDate ?
                        $"Checking order {orderIdentifier} for modifications..." :
                        $"Processing new order {++newOrderCount}";

                    OrderEntity order = InstantiateOrder(orderIdentifier);

                    order = LoadOrder(order, threeDCartOrder, shipment, invoiceNumberPostFix);

                    sqlAdapterRetry.ExecuteWithRetry(() => SaveDownloadedOrder(order));

                    shipmentIndex++;
                    threeDCartOrder.IsSubOrder = true;

                    // check for cancellation
                    if (Progress.IsCancelRequested)
                    {
                        return;
                    }
                }
                ordersProcessed ++;
                Progress.PercentComplete = ordersProcessed/totalCount;
            }
        }

        /// <summary>
        /// Loads the order.
        /// </summary>
        public OrderEntity LoadOrder(OrderEntity order, ThreeDCartOrder threeDCartOrder, ThreeDCartShipment threeDCartShipment, string invoiceNumberPostFix)
        {
            MethodConditions.EnsureArgumentIsNotNull(threeDCartOrder, "order");

            // If this order does not have sub orders, set the order total to that which we received from 3dcart
            // If it does have sub orders, we'll calculate the order total after we add items for this shipment/charges/payment
            if (order.IsNew && !threeDCartOrder.HasSubOrders)
            {
                // Set the total.  It will be calculated and verified later.
                order.OrderTotal = threeDCartOrder.OrderAmount;
            }

            order.OnlineLastModified = DateTimeUtility.ConvertTimeToUtcForTimeZone(threeDCartOrder.LastUpdate, threeDCartStore.StoreTimeZone);
            order.OnlineStatusCode = threeDCartOrder.OrderStatusID;
            order.OnlineStatus = EnumHelper.GetDescription((Enums.ThreeDCartOrderStatus)threeDCartOrder.OrderStatusID);

            LoadAddress(order, threeDCartOrder, threeDCartShipment);

            LoadOrderNotes(order, threeDCartOrder);

            if (order.IsNew)
            {
                if (threeDCartOrder.CustomerID <= MissingCustomerID)
                {
                    order.OnlineCustomerID = null;
                }
                else
                {
                    order.OnlineCustomerID = threeDCartOrder.CustomerID;
                }

                // If user has upgraded from SOAP API, a previously downloaded order won't be ThreeDCartOrderEntity,
                // just a OrderEntity, so don't try and set the ThreeDCartOrderID.
                if (order is ThreeDCartOrderEntity)
                {
                    ((ThreeDCartOrderEntity) order).ThreeDCartOrderID = threeDCartOrder.OrderID;
                }

                order.RequestedShipping = threeDCartShipment.ShipmentMethodName;
                order.OrderDate = DateTimeUtility.ConvertTimeToUtcForTimeZone(threeDCartOrder.OrderDate, threeDCartStore.StoreTimeZone);

                LoadItems(order, threeDCartOrder, threeDCartShipment);

                if (!threeDCartOrder.IsSubOrder)
                {
                    LoadOrderCharges(order, threeDCartOrder);

                    LoadPaymentDetails(order, threeDCartOrder);
                }

                AdjustAndSetOrderTotal(order, threeDCartOrder);
            }

            return order;
        }

        /// <summary>
        /// Loads the payment details.
        /// </summary>
        private void LoadPaymentDetails(OrderEntity order, ThreeDCartOrder threeDCartOrder)
        {
            if (!string.IsNullOrWhiteSpace(threeDCartOrder.BillingPaymentMethod))
            {
                LoadPaymentDetail(order, "Payment Type", threeDCartOrder.BillingPaymentMethod);
            }

            if (!string.IsNullOrWhiteSpace(threeDCartOrder.CardType))
            {
                LoadPaymentDetail(order, "Card Type", threeDCartOrder.CardType);
            }
        }

        /// <summary>
        /// Loads the order charges.
        /// </summary>
        private void LoadOrderCharges(OrderEntity order, ThreeDCartOrder threeDCartOrder)
        {
            if (threeDCartOrder.OrderDiscount > 0)
            {
                LoadCharge(order, "Discount", "Discount", -threeDCartOrder.OrderDiscount);
            }

            // Still want to show tax even if it's 0
            LoadCharge(order, "Tax", "Tax", threeDCartOrder.SalesTax);

            if (threeDCartOrder.SalesTax2 > 0)
            {
                LoadCharge(order, "Tax", "Tax 2", threeDCartOrder.SalesTax2);
            }

            if (threeDCartOrder.SalesTax3 > 0)
            {
                LoadCharge(order, "Tax", "Tax 3", threeDCartOrder.SalesTax3);
            }

            IEnumerable<ThreeDCartShipment> shipments = threeDCartOrder.ShipmentList;

            foreach (ThreeDCartShipment shipment in shipments)
            {
                if (shipment.ShipmentCost > 0)
                {
                    LoadCharge(order, "Shipping", shipment.ShipmentMethodName, shipment.ShipmentCost);
                }
            }
        }

        /// <summary>
        /// Adjusts the total for kit items and sets the order total.
        /// </summary>
        private void AdjustAndSetOrderTotal(OrderEntity order, ThreeDCartOrder threeDCartOrder)
        {
            decimal total = new OrderManager().CalculateOrderTotal(order);
             
            var items = order.OrderItems;

            bool hasKitItems = items.Any(x => x.Name.StartsWith("KIT ITEM:", StringComparison.OrdinalIgnoreCase));

            if (hasKitItems && threeDCartOrder.OrderAmount != total)
            {
                AddKitAdjustment(order, threeDCartOrder.OrderAmount - total);
            }

            // If this is a sub order, or it's the first order that has sub orders, calculate the total
            if (threeDCartOrder.IsSubOrder || threeDCartOrder.HasSubOrders)
            {
                order.OrderTotal = total;
            }
        }

        /// <summary>
        /// Loads the items.
        /// </summary>
        private void LoadItems(OrderEntity order, ThreeDCartOrder threeDCartOrder, ThreeDCartShipment shipment)
        {
            IEnumerable<ThreeDCartOrderItem> shipmentItems = threeDCartOrder.OrderItemList.Where(i => i.ItemShipmentID == shipment.ShipmentID);

            foreach (ThreeDCartOrderItem item in shipmentItems)
            {
                LoadItem(order, item);
            }
        }

        /// <summary>
        /// Loads the item.
        /// </summary>
        private void LoadItem(OrderEntity order, ThreeDCartOrderItem threeDCartItem)
        {
            ThreeDCartOrderItemEntity item = (ThreeDCartOrderItemEntity)InstantiateOrderItem(order);

            item.Code = threeDCartItem.ItemID;
            item.SKU = item.Code;
            item.Quantity = threeDCartItem.ItemQuantity;
            item.UnitCost = threeDCartItem.ItemUnitCost;
            item.UnitPrice = threeDCartItem.ItemUnitPrice;
            item.Weight = threeDCartItem.ItemWeight;
            item.ThreeDCartShipmentID = threeDCartItem.ItemShipmentID;

            LoadProductImagesAndLocation(item, threeDCartItem.CatalogID);
            LoadItemNameAndAttributes(item, threeDCartItem);

            // There are some cases where discounts don't show in order discount field and actually appear as a seperate item.
            // When this happens, it has no item price, but an item option price, which we usually ignore since we
            // extract item attribute costs from the item description. So if an item matches that criteria, set the
            // item's price to the attribute price.
            if (threeDCartItem.ItemUnitPrice == 0 && threeDCartItem.ItemOptionPrice != 0 &&
                !item.OrderItemAttributes.Any() && threeDCartItem.ItemDescription.Contains("Discount"))
            {
                // Discount can come as percentage which has come down with more that 2 decimal places, so round it.
                item.UnitPrice = Math.Round(threeDCartItem.ItemOptionPrice, 2);
            }
        }

        /// <summary>
        /// Loads the item name and attributes.
        /// </summary>
        private void LoadItemNameAndAttributes(ThreeDCartOrderItemEntity item, ThreeDCartOrderItem threeDCartItem)
        {
            string itemDescription = threeDCartItem.ItemDescription;
            string[] splitDescription = itemDescription.Split(new[] { "<br>" }, StringSplitOptions.RemoveEmptyEntries);

            item.Name = splitDescription[0];

            for (int i = 1; i < splitDescription.Length; i++)
            {
                string optionHtml = splitDescription[i];

                HtmlAgilityDocument htmlDoc = new HtmlAgilityDocument();
                htmlDoc.LoadHtml(optionHtml);

                // get optionName
                HtmlNode optionNameNode = htmlDoc.DocumentNode.SelectSingleNode(@"/b");
                if (optionNameNode == null)
                {
                    continue;
                }
                string optionName = optionNameNode.InnerHtml.Trim().TrimEnd(':');
                string optionNameAndPrice = optionNameNode.SelectSingleNode("./following-sibling::text()").InnerText.Trim();

                // Get unit price
                Regex pricePattern = new Regex(@"\$\d+(?:\.\d+)?");
                Match match = pricePattern.Match(optionNameAndPrice);
                decimal unitPrice = 0;
                if (match.Groups.Count == 1)
                {
                    string amount = match.Groups[0].Value;
                    decimal.TryParse(amount, NumberStyles.Currency, null, out unitPrice);
                }

                // Get description
                Regex removePricePattern = new Regex(@"\-\ \$\d+(?:\.\d+)?");
                string description =
                    removePricePattern.Replace(optionNameAndPrice, string.Empty).Trim().Replace(@"&nbsp;", string.Empty);


                OrderItemAttributeEntity attribute = InstantiateOrderItemAttribute(item);
                attribute.Name = optionName;
                attribute.Description = description;
                attribute.UnitPrice = unitPrice;
            }
        }

        /// <summary>
        /// Loads the product images and location.
        /// </summary>
        /// <remarks>Attempts to load from cache before reaching out to 3dcart</remarks>
        private void LoadProductImagesAndLocation(ThreeDCartOrderItemEntity item, int catalogID)
        {
            ThreeDCartProduct product = restWebClient.GetProduct(catalogID);

            if (product != null)
            {
                if (!string.IsNullOrEmpty(product.ThumbnailFile))
                {
                    item.Thumbnail = $"{threeDCartStore.StoreUrl}/{product.ThumbnailFile}";
                }

                if (!string.IsNullOrEmpty(product.MainImageFile))
                {
                    item.Image = $"{threeDCartStore.StoreUrl}/{product.MainImageFile}";
                }

                item.Location = product.WarehouseBin;
            }
        }

        /// <summary>
        /// Loads the order notes.
        /// </summary>
        private void LoadOrderNotes(OrderEntity order, ThreeDCartOrder threeDCartOrder)
        {
            InstantiateNote(order, threeDCartOrder.CustomerComments, DateTime.Now, NoteVisibility.Public, true);
            InstantiateNote(order, threeDCartOrder.InternalComments, DateTime.Now, NoteVisibility.Internal, true);
            InstantiateNote(order, threeDCartOrder.ExternalComments, DateTime.Now, NoteVisibility.Internal, true);

            foreach (ThreeDCartQuestion question in threeDCartOrder.QuestionList)
            {
                string questionNote = question.QuestionTitle;
                if (!string.IsNullOrWhiteSpace(question.QuestionAnswer))
                {
                    questionNote += $" : {question.QuestionAnswer}";
                }

                InstantiateNote(order, questionNote, DateTime.Now, NoteVisibility.Internal, true);
            }
        }

        /// <summary>
        /// Loads the address.
        /// </summary>
        private void LoadAddress(OrderEntity order, ThreeDCartOrder threeDCartOrder, ThreeDCartShipment shipment)
        {
            PersonAdapter billAdapter = new PersonAdapter(order, "Bill")
            {
                FirstName = threeDCartOrder.BillingFirstName,
                LastName = threeDCartOrder.BillingLastName,
                Company = threeDCartOrder.BillingCompany,
                Phone = threeDCartOrder.BillingPhoneNumber,
                Email = threeDCartOrder.BillingEmail,
                Street1 = threeDCartOrder.BillingAddress,
                Street2 = threeDCartOrder.BillingAddress2,
                City = threeDCartOrder.BillingCity,
                StateProvCode = Geography.GetStateProvCode(threeDCartOrder.BillingState),
                PostalCode = threeDCartOrder.BillingZipCode,
                CountryCode = Geography.GetCountryCode(threeDCartOrder.BillingCountry)
            };

            PersonAdapter shipAdapter = new PersonAdapter(order, "Ship")
            {
                FirstName = shipment.ShipmentFirstName,
                LastName = shipment.ShipmentLastName,
                Company = shipment.ShipmentCompany,
                Phone = shipment.ShipmentPhone,
                Email = shipment.ShipmentEmail,
                Street1 = shipment.ShipmentAddress,
                Street2 = shipment.ShipmentAddress2,
                City = shipment.ShipmentCity,
                StateProvCode = Geography.GetStateProvCode(shipment.ShipmentState),
                PostalCode = shipment.ShipmentZipCode,
                CountryCode = Geography.GetCountryCode(shipment.ShipmentCountry)
            };
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
    }
}