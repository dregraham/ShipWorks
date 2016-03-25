using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.ThreeDCart.RestApi.DTO;

namespace ShipWorks.Stores.Platforms.ThreeDCart.RestApi
{
    public class ThreeDCartRestDownloader : StoreDownloader
    {
        private IThreeDCartRestWebClient restWebClient;
        private ThreeDCartStoreEntity threeDCartStore;
        private int totalCount;
        private ISqlAdapterRetry sqlAdapterRetry;
        private readonly ILog log;
        const int MissingCustomerID = 0;
        private int newOrderCount;
        private int modifiedOrderCount;
        private DateTime modifiedOrderEndDate;

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
            threeDCartStore = store;
            totalCount = 0;
        }

        public ThreeDCartRestDownloader(ThreeDCartStoreEntity store, IThreeDCartRestWebClient restWebClient, ISqlAdapterRetry sqlAdapterRetry, ILog log) : base(store)
        {
            this.restWebClient = restWebClient;
            this.sqlAdapterRetry = sqlAdapterRetry;
            this.log = log;
        }

        /// <summary>
        /// Download orders for the 3D Cart store
        /// </summary>
        protected override void Download()
        {
            try
            {
                restWebClient.LoadProgressReporter(Progress);

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

                totalCount = restWebClient.GetOrderCount(startDate.Value);

                if (totalCount != 0)
                {
                    IEnumerable<ThreeDCartOrder> orders = DownloadOrders(startDate.Value);

                    LoadOrders(orders);
                }
                else
                {
                    Progress.Detail = "No orders to download.";
                }

                newOrderCount = 0;
                modifiedOrderCount = 0;
                // Update the progress bar for the new count
                Progress.PercentComplete = QuantitySaved/totalCount;
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
        /// Creates the order identifier.
        /// </summary>
        /// <remarks>
        /// Extracted from ThreeDCartSoapDownloader
        /// </remarks>
        private ThreeDCartOrderIdentifier CreateOrderIdentifier(ThreeDCartOrder order, string invoiceNumberPostFix)
        {
            // Now extract the Invoice number and ThreeDCart Order Id
            long orderId = order.OrderID;

            // Invoice number is defined as an integer in the 3D Cart schema
            // So we can safely remove the prefix to get to a long
            long invoiceNum = 0;
            string invoiceNumber = order.InvoiceNumber.ToString();
            string invoiceNumberPrefix = order.InvoiceNumberPrefix;

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
                log.ErrorFormat($"3D Cart returned an invalid invoice number: {invoiceNumber}.");
                throw new ThreeDCartException("3D Cart returned an invalid response while downloading orders");
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
            int newOrderTotal = 0;

            foreach (ThreeDCartOrder threeDCartOrder in orders)
            {
                threeDCartOrder.isSubOrder = false;
                threeDCartOrder.hasSubOrders = threeDCartOrder.ShipmentList.Count() > 1;
                int shipmentIndex = 1;

                foreach (ThreeDCartShipment shipment in threeDCartOrder.ShipmentList)
                {

                    string invoiceNumberPostFix = threeDCartOrder.hasSubOrders ? $"-{shipmentIndex}" : string.Empty;

                    ThreeDCartOrderIdentifier orderIdentifier = CreateOrderIdentifier(threeDCartOrder, invoiceNumberPostFix);

                    if (threeDCartOrder.OrderDate < modifiedOrderEndDate)
                    {
                        Progress.Detail = $"Checking order {orderIdentifier} for modifications...";
                        ++modifiedOrderCount;
                    }
                    else
                    {
                        if (newOrderCount == 0)
                        {
                            newOrderTotal = orders.Count() - modifiedOrderCount;
                        }

                        Progress.Detail = $"Processing new order {++newOrderCount} of {newOrderTotal}";
                    }

                    ThreeDCartOrderEntity order = (ThreeDCartOrderEntity) InstantiateOrder(orderIdentifier);

                    order = LoadOrder(order, threeDCartOrder, shipment, invoiceNumberPostFix);

                    sqlAdapterRetry.ExecuteWithRetry(() => SaveDownloadedOrder(order));

                    shipmentIndex++;
                    threeDCartOrder.isSubOrder = true;
                }
            }
        }

        /// <summary>
        /// Loads the order.
        /// </summary>
        public ThreeDCartOrderEntity LoadOrder(ThreeDCartOrderEntity order, ThreeDCartOrder threeDCartOrder, ThreeDCartShipment threeDCartShipment, string invoiceNumberPostFix)
        {
            MethodConditions.EnsureArgumentIsNotNull(threeDCartOrder, "order");

            // If this order does not have sub orders, set the order total to that which we received from 3D Cart
            // If it does have sub orders, we'll calculate the order total after we add items for this shipment/charges/payment
            if (order.IsNew && !threeDCartOrder.hasSubOrders)
            {
                // Set the total.  It will be calculated and verified later.
                order.OrderTotal = threeDCartOrder.OrderAmount;
            }

            if (order.CustomerID <= MissingCustomerID)
            {
                order.OnlineCustomerID = null;
            }
            else
            {
                order.OnlineCustomerID = order.CustomerID;
            }
            order.ThreeDCartOrderID = threeDCartOrder.OrderID;
            order.OrderDate = threeDCartOrder.OrderDate;
            order.OnlineLastModified = threeDCartOrder.LastUpdate;
            order.OnlineStatusCode = threeDCartOrder.OrderStatusID;
            order.OnlineStatus = EnumHelper.GetDescription((Enums.ThreeDCartOrderStatus)threeDCartOrder.OrderStatusID);
            order.RequestedShipping = threeDCartShipment.ShipmentMethodName;

            LoadAddress(order, threeDCartOrder, threeDCartShipment);

            LoadOrderNotes(order, threeDCartOrder);

            if (order.IsNew)
            {
                LoadItems(order, threeDCartOrder, threeDCartShipment);

                if (!threeDCartOrder.isSubOrder)
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
            if (threeDCartOrder.OrderDiscount < 0)
            {
                LoadCharge(order, "Discount", "Discount", threeDCartOrder.OrderDiscount);
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
            if (threeDCartOrder.isSubOrder || threeDCartOrder.hasSubOrders)
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
        }

        /// <summary>
        /// Loads the item name and attributes.
        /// </summary>
        private void LoadItemNameAndAttributes(ThreeDCartOrderItemEntity item, ThreeDCartOrderItem threeDCartItem)
        {
            string description = threeDCartItem.ItemDescription;

            string[] splitDescription = description.Split(new[] { "<br><b>" }, StringSplitOptions.RemoveEmptyEntries);

            item.Name = splitDescription[0];

            if (splitDescription.Count() > 1)
            {
                for (int i = 1; i < splitDescription.Count(); i++)
                {
                    string[] option = splitDescription[i].Split(new[] { "</b>&nbsp;" }, StringSplitOptions.RemoveEmptyEntries);

                    if (option[0].EndsWith(":"))
                    {
                       option[0] = option[0].Remove(option[0].Length - 1);
                    }
                    string optionName = option[0];

                    int lastDashIndex = option[1].LastIndexOf('-');

                    string optionValue = option[1].Substring(0, lastDashIndex).Trim();
                    string optionPriceString = option[1].Substring(lastDashIndex + 1).Trim();
                    optionPriceString = Regex.Replace(optionPriceString, "[^.0-9]", string.Empty);

                    decimal optionPrice;
                    decimal.TryParse(optionPriceString, out optionPrice);

                    OrderItemAttributeEntity attribute = InstantiateOrderItemAttribute(item);
                    attribute.Name = optionName;
                    attribute.Description = optionValue;
                    attribute.UnitPrice = optionPrice;
                }
            }
        }

        /// <summary>
        /// Loads the product images and location.
        /// </summary>
        /// <remarks>Attempts to load from cache before reaching out to 3DCart</remarks>
        private void LoadProductImagesAndLocation(ThreeDCartOrderItemEntity item, int catalogID)
        {
            ThreeDCartProduct product = restWebClient.GetProduct(catalogID);

            if (product != null)
            {
                item.Thumbnail = product.ThumbnailFile;
                item.Image = product.MainImageFile ?? string.Empty;
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
                Street1 = shipment.ShipmentAddress,
                Street2 = shipment.ShipmentAddress2,
                City = shipment.ShipmentCity,
                StateProvCode = Geography.GetStateProvCode(shipment.ShipmentState),
                PostalCode = shipment.ShipmentZipCode,
                CountryCode = Geography.GetCountryCode(shipment.ShipmentCountry)
            };
        }

        /// <summary>
        /// Downloads the orders.
        /// </summary>
        public IEnumerable<ThreeDCartOrder> DownloadOrders(DateTime startDate)
        {
            return restWebClient.GetOrders(startDate);
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