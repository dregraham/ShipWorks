using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Interapptive.Shared;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.ThreeDCart.RestApi.DTO;

namespace ShipWorks.Stores.Platforms.ThreeDCart.RestApi
{
    /// <summary>
    /// Downloader for 3dcart that uses their REST API
    /// </summary>
    [Component]
    public class ThreeDCartRestDownloader : StoreDownloader, IThreeDCartRestDownloader
    {
        const int MissingCustomerID = 0;
        private readonly IThreeDCartRestWebClient restWebClient;
        private readonly ThreeDCartStoreEntity threeDCartStore;
        private readonly ISqlAdapterRetry sqlAdapterRetry;
        private readonly IOrderManager orderManager;
        private readonly ILog log;
        private int newOrderCount;
        private DateTime modifiedOrderEndDate;
        private int totalCount;
        private int ordersProcessed;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreeDCartRestDownloader"/> class.
        /// </summary>
        [NDependIgnoreTooManyParams(Justification =
            "These parameters are dependencies the store downloader already had, they're just explicit now")]
        public ThreeDCartRestDownloader(ThreeDCartStoreEntity store,
            Func<ThreeDCartStoreEntity, IThreeDCartRestWebClient> restWebClientFactory,
            ISqlAdapterRetryFactory sqlAdapterRetryFactory,
            IOrderManager orderManager,
            Func<StoreEntity, ThreeDCartStoreType> getStoreType,
            IConfigurationData configurationData,
            ISqlAdapterFactory sqlAdapterFactory,
            Func<Type, ILog> createLogger) :
            base(store, getStoreType(store), configurationData, sqlAdapterFactory)
        {
            this.restWebClient = restWebClientFactory(store);
            this.sqlAdapterRetry = sqlAdapterRetryFactory.Create<SqlException>(5, -5, "ThreeDCartRestDownloader.LoadOrder");
            this.orderManager = orderManager;
            this.log = createLogger(GetType());
            threeDCartStore = store;
        }

        /// <summary>
        /// Download orders for the 3dcart store
        /// </summary>
        /// <param name="trackedDurationEvent">The telemetry event that can be used to
        /// associate any store-specific download properties/metrics.</param>
        protected override async Task Download(TrackedDurationEvent trackedDurationEvent)
        {
            try
            {
                // Need to give the web client the progress bar because if
                // we get throttled, we want to display it in the progress
                restWebClient.LoadProgressReporter(Progress);
                ordersProcessed = 0;

                // Get the number of days back that we should check for modified orders.
                int numberOfDaysBack = threeDCartStore.DownloadModifiedNumberOfDaysBack > 0 ? threeDCartStore.DownloadModifiedNumberOfDaysBack : 0;

                DateTime? startDate = GetOrderDateStartingPoint();
                if (!startDate.HasValue)
                {
                    // There's no orders or the user wanted to download all orders
                    // Set the start date to a long time ago and set number of days back
                    // to 0 so that all orders look "new"
                    startDate = DateTime.Today.AddYears(-20);
                    numberOfDaysBack = 0;
                }

                // Set the modified order end date to the current start date so that orders before it look "modified" and after look "new"
                modifiedOrderEndDate = startDate.Value;

                // We aren't supposed to download all orders, so adhere to the number of days back for modified orders.
                if (numberOfDaysBack > 0)
                {
                    startDate = startDate.Value.AddDays(-numberOfDaysBack);
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

                await DownloadOrders(startDate.Value).ConfigureAwait(false);
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
        private async Task DownloadOrders(DateTime startDate)
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
                    await LoadOrders(orders).ConfigureAwait(false);
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
        private async Task<ThreeDCartOrderIdentifier> CreateOrderIdentifier(ThreeDCartOrder order, string invoiceNumberPostFix)
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
            OrderEntity orderEntity = await FindOrder(threeDCartOrderIdentifier).ConfigureAwait(false);
            if (orderEntity == null)
            {
                threeDCartOrderIdentifier = new ThreeDCartOrderIdentifier(invoiceNum, invoiceNumberPrefix, invoiceNumberPostFix);
            }

            return threeDCartOrderIdentifier;
        }

        /// <summary>
        /// Loads the orders.
        /// </summary>
        public async Task LoadOrders(IEnumerable<ThreeDCartOrder> orders)
        {
            foreach (ThreeDCartOrder threeDCartOrder in orders)
            {
                // Don't download abandoned orders
                if (threeDCartOrder.OrderStatusID == (int) Enums.ThreeDCartOrderStatus.NotCompleted)
                {
                    continue;
                }

                threeDCartOrder.IsSubOrder = false;
                threeDCartOrder.HasSubOrders = threeDCartOrder.ShipmentList.Count() > 1;
                int shipmentIndex = 1;

                foreach (ThreeDCartShipment shipment in threeDCartOrder.ShipmentList)
                {
                    string invoiceNumberPostFix = threeDCartOrder.HasSubOrders ? $"-{shipmentIndex}" : string.Empty;

                    ThreeDCartOrderIdentifier orderIdentifier = await CreateOrderIdentifier(threeDCartOrder, invoiceNumberPostFix).ConfigureAwait(false);

                    Progress.Detail = threeDCartOrder.OrderDate < modifiedOrderEndDate ?
                        $"Checking order {orderIdentifier} for modifications..." :
                        $"Processing new order {++newOrderCount}";

                    OrderEntity order = await InstantiateOrder(orderIdentifier).ConfigureAwait(false);

                    order = await LoadOrder(order, threeDCartOrder, shipment).ConfigureAwait(false);

                    await sqlAdapterRetry.ExecuteWithRetryAsync(() => SaveDownloadedOrder(order)).ConfigureAwait(false);

                    shipmentIndex++;
                    threeDCartOrder.IsSubOrder = true;

                    // check for cancellation
                    if (Progress.IsCancelRequested)
                    {
                        return;
                    }
                }
                ordersProcessed++;
                Progress.PercentComplete = ordersProcessed / totalCount;
            }
        }

        /// <summary>
        /// Loads the order.
        /// </summary>
        public async Task<OrderEntity> LoadOrder(OrderEntity order, ThreeDCartOrder threeDCartOrder, ThreeDCartShipment threeDCartShipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(threeDCartOrder, nameof(threeDCartOrder));

            // If this order does not have sub orders, set the order total to that which we received from 3dcart
            // If it does have sub orders, we'll calculate the order total after we add items for this shipment/charges/payment
            if (order.IsNew && !threeDCartOrder.HasSubOrders)
            {
                // Set the total.  It will be calculated and verified later.
                order.OrderTotal = threeDCartOrder.OrderAmount;
            }

            order.OnlineLastModified = DateTimeUtility.ConvertTimeToUtcForTimeZone(threeDCartOrder.LastUpdate, threeDCartStore.StoreTimeZone);
            order.OnlineStatusCode = threeDCartOrder.OrderStatusID;
            order.OnlineStatus = EnumHelper.GetDescription((Enums.ThreeDCartOrderStatus) threeDCartOrder.OrderStatusID);

            LoadAddress(order, threeDCartOrder, threeDCartShipment);

            await LoadOrderNotes(order, threeDCartOrder).ConfigureAwait(false);

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
            decimal total = orderManager.CalculateOrderTotal(order);

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
            ThreeDCartOrderItemEntity item = (ThreeDCartOrderItemEntity) InstantiateOrderItem(order);

            item.Code = threeDCartItem.ItemID;
            item.SKU = item.Code;
            item.Quantity = threeDCartItem.ItemQuantity;
            item.UnitCost = threeDCartItem.ItemUnitCost;
            item.UnitPrice = threeDCartItem.ItemUnitPrice;
            item.Weight = threeDCartItem.ItemWeight;
            item.ThreeDCartShipmentID = threeDCartItem.ItemShipmentID;

            LoadProductImagesAndLocation(item, threeDCartItem.CatalogID);

            // Each option line (name, selected values and their prices) is separated by <br><b>
            string[] descriptionLines = threeDCartItem.ItemDescription.Split(new[] { "<br><b>" }, StringSplitOptions.RemoveEmptyEntries);

            // First line of description is the item name, the rest are options/attributes
            item.Name = descriptionLines[0];

            LoadItemAttributes(item, descriptionLines.Skip(1));

            // There are some cases where discounts don't show in order discount field and actually appear as a separate item.
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
        private void LoadItemAttributes(ThreeDCartOrderItemEntity item, IEnumerable<string> itemOptionLines)
        {
            foreach (string descriptionLine in itemOptionLines)
            {
                // The option name is always followed by </b>&nbsp;
                string[] optionLine = descriptionLine.Split(new [] { "</b>&nbsp;"}, StringSplitOptions.RemoveEmptyEntries);

                // Remove the : because we already add one in the grid
                string optionName = optionLine[0].Trim().TrimEnd(':');

                foreach (string optionValues in optionLine.Skip(1))
                {
                    // If an item has multiple values for a single option, they are split by <br>
                    string[] optionValueLines = optionValues.Split(new[] {"<br>"}, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string optionValueLine in optionValueLines)
                    {
                        LoadAttribute(item, optionName, optionValueLine);
                    }
                }
            }
        }

        /// <summary>
        /// Parses the option value and price out of the option value string and loads the item attribute
        /// </summary>
        private void LoadAttribute(ThreeDCartOrderItemEntity item, string optionName, string optionValue)
        {
            // Get unit price
            Regex pricePattern = new Regex(@"\$\d+(?:\.\d+)?");
            Match match = pricePattern.Match(optionValue);
            decimal unitPrice = 0;
            if (match.Groups.Count == 1)
            {
                string amount = match.Groups[0].Value;
                decimal.TryParse(amount, NumberStyles.Currency, null, out unitPrice);
            }

            // Get description
            Regex removePricePattern = new Regex(@" \$\d+(?:\.\d+)?");
            string description =
                removePricePattern.Replace(optionValue, string.Empty).Trim('-', ' ');

            OrderItemAttributeEntity attribute = InstantiateOrderItemAttribute(item);
            attribute.Name = optionName;
            attribute.Description = description;
            attribute.UnitPrice = unitPrice;
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
        private async Task LoadOrderNotes(OrderEntity order, ThreeDCartOrder threeDCartOrder)
        {
            await InstantiateNote(order, threeDCartOrder.CustomerComments, DateTime.Now, NoteVisibility.Public, true).ConfigureAwait(false);
            await InstantiateNote(order, threeDCartOrder.InternalComments, DateTime.Now, NoteVisibility.Internal, true).ConfigureAwait(false);
            await InstantiateNote(order, threeDCartOrder.ExternalComments, DateTime.Now, NoteVisibility.Internal, true).ConfigureAwait(false);

            foreach (ThreeDCartQuestion question in threeDCartOrder.QuestionList)
            {
                string questionNote = question.QuestionTitle;
                if (!string.IsNullOrWhiteSpace(question.QuestionAnswer))
                {
                    questionNote += $" : {question.QuestionAnswer}";
                }

                await InstantiateNote(order, questionNote, DateTime.Now, NoteVisibility.Internal, true).ConfigureAwait(false);
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
        /// Load the given payment detail into the order
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