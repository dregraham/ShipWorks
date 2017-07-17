﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Metrics;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.SparkPay.DTO;

namespace ShipWorks.Stores.Platforms.SparkPay
{
    /// <summary>
    /// Spark pay downloader
    /// </summary>
    public class SparkPayDownloader : StoreDownloader
    {
        readonly SparkPayStoreEntity store;
        readonly SparkPayWebClient webClient;
        readonly SparkPayStatusCodeProvider statusProvider;

        /// <summary>
        /// The spark pay downloader
        /// </summary>
        public SparkPayDownloader(StoreEntity store, SparkPayWebClient webClient, Func<SparkPayStoreEntity, SparkPayStatusCodeProvider> statusProviderFactory)
            : base(store)
        {
            this.webClient = webClient;
            this.store = (SparkPayStoreEntity) store;
            statusProvider = statusProviderFactory(this.store);
        }

        /// <summary>
        /// Downloads orders for the given store using the newest last modified or store default or 30.
        /// </summary>
        /// <param name="trackedDurationEvent">The telemetry event that can be used to
        /// associate any store-specific download properties/metrics.</param>
        protected override async Task Download(TrackedDurationEvent trackedDurationEvent)
        {
            try
            {
                // update online statuses
                UpdateOrderStatuses();

                while (true)
                {
                    if (Progress.IsCancelRequested)
                    {
                        return;
                    }

                    DateTime start = GetOnlineLastModifiedStartingPoint().GetValueOrDefault(DateTime.UtcNow.AddDays(-30));
                    OrdersResponse response = webClient.GetOrders(store, start, Progress);

                    if (response.TotalCount > 0)
                    {
                        if (Progress.IsCancelRequested)
                        {
                            return;
                        }

                        await LoadOrders(response.Orders).ConfigureAwait(false);
                    }
                    else
                    {
                        return;
                    }
                }
            }
            catch (SparkPayException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Load all downloaded orders
        /// </summary>
        private async Task LoadOrders(IEnumerable<Order> sparkPayOrders)
        {
            if (Progress.IsCancelRequested || !sparkPayOrders.Any())
            {
                return;
            }

            foreach (Order sparkPayOrder in sparkPayOrders)
            {
                await LoadOrder(sparkPayOrder).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Load an individual order
        /// </summary>
        private async Task LoadOrder(Order sparkPayOrder)
        {
            if (Progress.IsCancelRequested || sparkPayOrder == null)
            {
                return;
            }

            OrderEntity order = await InstantiateOrder(new OrderNumberIdentifier(sparkPayOrder.Id.Value)).ConfigureAwait(false);

            order.OnlineStatus = statusProvider.GetCodeName((int) sparkPayOrder.OrderStatusId);
            order.OnlineStatusCode = sparkPayOrder.OrderStatusId.ToString();
            order.OnlineLastModified = sparkPayOrder.UpdatedAt.GetValueOrDefault(DateTime.UtcNow).UtcDateTime;
            order.RequestedShipping = sparkPayOrder.SelectedShippingMethod;

            if (order.IsNew)
            {
                order.OrderDate = sparkPayOrder.OrderedAt.GetValueOrDefault(DateTime.UtcNow).UtcDateTime;
                order.OrderNumber = sparkPayOrder.Id.Value;
                order.OrderTotal = sparkPayOrder.GrandTotal.GetValueOrDefault(0);

                LoadOrderItems(order, sparkPayOrder.Items);
                LoadOrderCharges(order, sparkPayOrder);
                await LoadOrderGiftMessages(order, sparkPayOrder).ConfigureAwait(false);

                LoadOrderPayments(order, sparkPayOrder);
            }

            LoadAddresses(order, sparkPayOrder);
            await LoadOrderNotes(order, sparkPayOrder).ConfigureAwait(false);

            ISqlAdapterRetry retryAdapter = new SqlAdapterRetry<SqlException>(5, -5, "SparkPayDownloader.LoadOrder");
            await retryAdapter.ExecuteWithRetryAsync(() => SaveDownloadedOrder(order)).ConfigureAwait(false);
        }

        /// <summary>
        /// Loads the orders payment information
        /// </summary>
        private static void LoadOrderPayments(OrderEntity order, Order sparkPayOrder)
        {
            Payment payment = sparkPayOrder.Payments.FirstOrDefault();

            if (payment != null)
            {
                OrderPaymentDetailEntity detail = new OrderPaymentDetailEntity { Order = order };

                if (payment.PaymentType == "CreditCard")
                {
                    detail.Label = payment.PaymentMethodName;
                    detail.Value = payment?.CardType ?? string.Empty;
                }
                else
                {
                    detail.Label = payment.PaymentMethodName;
                    detail.Value = string.Empty;
                }
            }
        }

        /// <summary>
        /// Loads the orders notes
        /// </summary>
        private async Task LoadOrderNotes(OrderEntity order, Order sparkPayOrder)
        {
            if (!string.IsNullOrWhiteSpace(sparkPayOrder.PublicComments))
            {
                await InstantiateNote(order, sparkPayOrder.PublicComments, order.OrderDate, NoteVisibility.Public).ConfigureAwait(false);
            }

            if (!string.IsNullOrWhiteSpace(sparkPayOrder.AdminComments))
            {
                await InstantiateNote(order, sparkPayOrder.AdminComments, order.OrderDate, NoteVisibility.Internal).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Loads the orders gift message
        /// </summary>
        /// <param name="order"></param>
        /// <param name="sparkPayOrder"></param>
        private async Task LoadOrderGiftMessages(OrderEntity order, Order sparkPayOrder)
        {
            if (!string.IsNullOrWhiteSpace(sparkPayOrder.GiftMessage))
            {
                await InstantiateNote(order, $"Gift Message: {sparkPayOrder.GiftMessage}", order.OrderDate, NoteVisibility.Public).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Loads the orders charges
        /// </summary>
        private static void LoadOrderCharges(OrderEntity order, Order sparkPayOrder)
        {
            LoadOrderCharge(order, "SHIPPING", "Shipping", sparkPayOrder.ShippingTotal.GetValueOrDefault(0));
            LoadOrderCharge(order, "HANDLING", "Handling Fee", sparkPayOrder.HandlingTotal.GetValueOrDefault(0));
            LoadOrderCharge(order, "TAX", "Tax", sparkPayOrder.TaxTotal.GetValueOrDefault(0));
            LoadOrderCharge(order, "ADDITIONAL FEES", "Additional Fees", sparkPayOrder.AdditionalFees.GetValueOrDefault(0));
            LoadOrderCharge(order, "DISCOUNT", "Discount", -sparkPayOrder.DiscountTotal.GetValueOrDefault(0));
        }

        private static void LoadOrderCharge(OrderEntity order, string type, string description, decimal amount)
        {
            OrderChargeEntity charge = new OrderChargeEntity();
            charge.Order = order;

            charge.Type = type;
            charge.Description = description;
            charge.Amount = amount;
        }

        /// <summary>
        /// Loads the order items into the order
        /// </summary>
        private void LoadOrderItems(OrderEntity order, IEnumerable<Item> sparkPayItems)
        {
            foreach (Item sparkPayItem in sparkPayItems)
            {
                OrderItemEntity orderItem = InstantiateOrderItem(order);

                // populate the basics
                orderItem.Name = sparkPayItem.ItemName;
                orderItem.Quantity = sparkPayItem.Quantity.GetValueOrDefault(0);
                orderItem.UnitPrice = sparkPayItem.Price.GetValueOrDefault(0);
                orderItem.Code = sparkPayItem.ItemNumber;
                orderItem.SKU = sparkPayItem.ItemNumber;
                orderItem.Location = sparkPayItem.WarehouseId.ToString();

                // see if we need to add any attributes
                string giftMessage = sparkPayItem.GiftMessage;

                if (!string.IsNullOrWhiteSpace(giftMessage))
                {
                    OrderItemAttributeEntity attribute = InstantiateOrderItemAttribute(orderItem);
                    attribute.Name = "Gift Message";
                    attribute.Description = giftMessage;
                    attribute.UnitPrice = 0;
                }

                // see if we need to add any attributes
                string lineItemNote = sparkPayItem.LineItemNote;

                if (!string.IsNullOrWhiteSpace(lineItemNote))
                {
                    OrderItemAttributeEntity attribute = InstantiateOrderItemAttribute(orderItem);
                    attribute.Name = "Note";
                    attribute.Description = lineItemNote;
                    attribute.UnitPrice = 0;
                }
            }
        }

        /// <summary>
        /// Loads the orders addresses into the order
        /// </summary>
        private void LoadAddresses(OrderEntity order, Order sparkPayOrder)
        {
            Address shipAddress = null;
            Address billAddress = null;

            if (sparkPayOrder.OrderShippingAddressId != null)
            {
                shipAddress = webClient.GetAddress(store, sparkPayOrder.OrderShippingAddressId.Value, Progress).Addresses.FirstOrDefault();
                SetStreetAddress(new PersonAdapter(order, "Ship"), shipAddress);
            }

            if (sparkPayOrder.OrderBillingAddressId == null)
            {
                billAddress = shipAddress;
            }
            else if (sparkPayOrder.OrderBillingAddressId != null)
            {
                billAddress = sparkPayOrder.OrderBillingAddressId == sparkPayOrder.OrderShippingAddressId ?
                    shipAddress :
                    webClient.GetAddress(store, sparkPayOrder.OrderBillingAddressId.Value, Progress).Addresses.FirstOrDefault();
            }

            SetStreetAddress(new PersonAdapter(order, "Bill"), billAddress);
        }

        /// <summary>
        /// Sets the orders address
        /// </summary>
        private static void SetStreetAddress(PersonAdapter personAdapter, Address sparkPayAddress)
        {
            if (sparkPayAddress != null)
            {
                personAdapter.FirstName = sparkPayAddress.FirstName;
                personAdapter.LastName = sparkPayAddress.LastName;
                personAdapter.Street1 = sparkPayAddress.AddressLine1;
                personAdapter.Street2 = sparkPayAddress.AddressLine2;
                personAdapter.City = sparkPayAddress.City;
                personAdapter.StateProvCode = Geography.GetStateProvCode(sparkPayAddress.State);
                personAdapter.PostalCode = sparkPayAddress.PostalCode;
                personAdapter.CountryCode = Geography.GetCountryCode(sparkPayAddress.Country);
                personAdapter.Phone = sparkPayAddress.Phone;
                personAdapter.Company = sparkPayAddress.Company;
                personAdapter.Fax = sparkPayAddress.Fax;
            }
        }

        /// <summary>
        /// Update the local order status provider
        /// </summary>
        private void UpdateOrderStatuses()
        {
            Progress.Detail = "Updating status codes...";

            // refresh the status codes from SparkPay
            statusProvider.UpdateFromOnlineStore();
        }
    }
}
