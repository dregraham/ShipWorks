using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Magento.DTO.MagnetoTwoRestOrder;
using ShipWorks.Stores.Platforms.Magento.Enums;

namespace ShipWorks.Stores.Platforms.Magento
{
    /// <summary>
    /// Downloader for Magento 2 REST API
    /// </summary>
    /// <seealso cref="ShipWorks.Stores.Communication.StoreDownloader" />
    [KeyedComponent(typeof(StoreDownloader), MagentoVersion.MagentoTwoREST, ExternallyOwned = false)]
    public class MagentoTwoRestDownloader : StoreDownloader
    {
        private readonly ISqlAdapterRetry sqlAdapter;
        private readonly ILog log;
        private readonly IMagentoTwoRestClient webClient;
        private readonly MagentoStoreEntity magentoStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="MagentoTwoRestDownloader"/> class.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="sqlAdapterRetryFactory">The SQL adapter.</param>
        /// <param name="webClientFactory"></param>
        /// <param name="logFactory"></param>
        public MagentoTwoRestDownloader(StoreEntity store, ISqlAdapterRetryFactory sqlAdapterRetryFactory,
            Func<MagentoStoreEntity, IMagentoTwoRestClient> webClientFactory, Func<Type, ILog> logFactory) : base(store)
        {
            magentoStore = (MagentoStoreEntity) store;
            sqlAdapter = sqlAdapterRetryFactory.Create<SqlException>(5, -5, "MagentoRestDownloader.Download");
            log = logFactory(typeof(MagentoTwoRestDownloader));
            webClient = webClientFactory(magentoStore);
        }

        /// <summary>
        /// Download orders for the Magento store
        /// </summary>
        protected override void Download(TrackedDurationEvent trackedDurationEvent)
        {
            trackedDurationEvent.AddProperty("Magento", ((MagentoVersion) magentoStore.MagentoVersion).ToString());
            Progress.Detail = "Checking for orders...";

            try
            {
                OrdersResponse ordersResponse;
                do
                {
                    ordersResponse = webClient.GetOrders(GetStartDate(), 1);
                    int totalOrders = ordersResponse.TotalCount;

                    if (totalOrders == 0 && QuantitySaved == 0)
                    {
                        Progress.Detail = "No orders to download.";
                        Progress.PercentComplete = 100;
                        return;
                    }

                    // Check if it has been cancelled
                    if (Progress.IsCancelRequested)
                    {
                        return;
                    }

                    Progress.Detail = $"Downloading {totalOrders} orders...";

                    foreach (Order magentoOrder in ordersResponse.Orders)
                    {
                        MagentoOrderIdentifier orderIdentifier = new MagentoOrderIdentifier(magentoOrder.EntityId, "", "");
                        MagentoOrderEntity orderEntity = InstantiateOrder(orderIdentifier) as MagentoOrderEntity;

                        LoadOrder(orderEntity, magentoOrder, Progress);
                    }
                } while (ordersResponse.TotalCount > 0);

                Progress.Detail = "Done";
            }
            catch (MagentoException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Get the start date for the download cycle
        /// </summary>
        /// <remarks>if we have not saved any orders yet use the start date minus 5 minutes</remarks>
        private DateTime? GetStartDate()
        {
            DateTime? onlineLastModifiedStartingPoint = GetOnlineLastModifiedStartingPoint();

            // If we haven't saved any orders yet use the start date minus 5 minutes
            // add a 5 min buffer to overlap possible server time issues
            //Note: onlineLastModifiedStartingPoint may be null. In this case, we should return null.
            return QuantitySaved == 0 ?
                onlineLastModifiedStartingPoint?.AddMinutes(-5) :
                onlineLastModifiedStartingPoint;
        }

        /// <summary>
        /// Loads the order.
        /// </summary>
        public void LoadOrder(MagentoOrderEntity orderEntity, Order magentoOrder, IProgressReporter progressReporter)
        {
            // Check if it has been cancelled
            if (progressReporter.IsCancelRequested)
            {
                return;
            }

            // Update the status
            progressReporter.Detail = $"Processing order {QuantitySaved + 1}...";

            DateTime lastModifiedDate;
            if (DateTime.TryParse(magentoOrder.UpdatedAt, out lastModifiedDate))
            {
                orderEntity.OnlineLastModified = DateTime.SpecifyKind(lastModifiedDate, DateTimeKind.Utc);
            }

            orderEntity.OnlineStatus = magentoOrder.Status;
            orderEntity.RequestedShipping = magentoOrder.ShippingDescription;

            LoadAddresses(orderEntity, magentoOrder);
            LoadNotes(orderEntity, magentoOrder);

            if (orderEntity.IsNew)
            {
                DateTime createdDate;
                if (DateTime.TryParse(magentoOrder.CreatedAt, out createdDate))
                {
                    orderEntity.OrderDate =
                        DateTime.SpecifyKind(createdDate, DateTimeKind.Utc);
                }
                orderEntity.OrderNumber = magentoOrder.EntityId;
                orderEntity.OrderTotal = Convert.ToDecimal(magentoOrder.GrandTotal);
                orderEntity.MagentoOrderID = magentoOrder.EntityId;
                orderEntity.OnlineCustomerID = magentoOrder.CustomerId;
                LoadItems(orderEntity, magentoOrder.Items);
                LoadOrderCharges(orderEntity, magentoOrder);
                LoadOrderPayment(orderEntity, magentoOrder);
            }

            sqlAdapter.ExecuteWithRetry(() => SaveDownloadedOrder(orderEntity));
        }

        /// <summary>
        /// Load the orders notes
        /// </summary>
        private void LoadNotes(OrderEntity orderEntity, Order magentoOrder)
        {
            foreach (StatusHistory history in magentoOrder.StatusHistories)
            {
                DateTime noteDate;
                if (!DateTime.TryParse(history.CreatedAt, out noteDate))
                {
                    noteDate = DateTime.UtcNow;
                }

                InstantiateNote(orderEntity, history.Comment, noteDate, NoteVisibility.Internal, true);
            }
        }

        /// <summary>
        /// Loads the addresses.
        /// </summary>
        private void LoadAddresses(OrderEntity orderEntity, Order magentoOrder)
        {
            ShippingAddress shippingAddress =
                magentoOrder.ExtensionAttributes.ShippingAssignments.FirstOrDefault()?.Shipping.Address;

            if (shippingAddress != null)
            {
                orderEntity.ShipFirstName = shippingAddress.Firstname;
                orderEntity.ShipMiddleName = shippingAddress.Middlename;
                orderEntity.ShipLastName = shippingAddress.Lastname;
                orderEntity.ShipCompany = shippingAddress.Company;
                orderEntity.ShipPhone = shippingAddress.Telephone;
                orderEntity.ShipEmail = shippingAddress.Email;
                orderEntity.ShipStreet1 = shippingAddress.Street.ElementAtOrDefault(0);
                orderEntity.ShipStreet2 = shippingAddress.Street.ElementAtOrDefault(1);
                orderEntity.ShipStreet3 = shippingAddress.Street.ElementAtOrDefault(2);
                orderEntity.ShipCity = shippingAddress.City;
                orderEntity.ShipStateProvCode = Geography.GetStateProvCode(shippingAddress.RegionCode);
                orderEntity.ShipPostalCode = shippingAddress.Postcode;
                orderEntity.ShipCountryCode = Geography.GetCountryCode(shippingAddress.CountryId);
            }

            BillingAddress billingAddress = magentoOrder.BillingAddress;

            if (billingAddress != null)
            {
                orderEntity.BillFirstName = billingAddress.Firstname;
                orderEntity.BillMiddleName = billingAddress.Middlename;
                orderEntity.BillLastName = billingAddress.Lastname;
                orderEntity.BillCompany = billingAddress.Company;
                orderEntity.BillPhone = billingAddress.Telephone;
                orderEntity.BillEmail = billingAddress.Email;
                orderEntity.BillStreet1 = billingAddress.Street.ElementAtOrDefault(0);
                orderEntity.BillStreet2 = billingAddress.Street.ElementAtOrDefault(1);
                orderEntity.BillStreet3 = billingAddress.Street.ElementAtOrDefault(2);
                orderEntity.BillCity = billingAddress.City;
                orderEntity.BillStateProvCode = Geography.GetStateProvCode(billingAddress.RegionCode);
                orderEntity.BillPostalCode = billingAddress.Postcode;
                orderEntity.BillCountryCode = Geography.GetCountryCode(billingAddress.CountryId);
            }
        }

        /// <summary>
        /// Loads the orders payment information
        /// </summary>
        private void LoadOrderPayment(OrderEntity order, Order magentoOrder)
        {
            OrderPaymentDetailEntity orderPayment = InstantiateOrderPaymentDetail(order);
            orderPayment.Label = "Payment Method";
            orderPayment.Value = magentoOrder?.Payment?.Method;
        }

        /// <summary>
        /// Loads the items.
        /// </summary>
        private void LoadItems(OrderEntity orderEntity, IEnumerable<Item> items)
        {
            if (items == null)
            {
                return;
            }

            IEnumerable<Item> itemList = items as IList<Item> ?? items.ToList();
            foreach (Item item in itemList.Where(i => i.ProductType != "configurable"))
            {
                OrderItemEntity orderItem = InstantiateOrderItem(orderEntity);

                orderItem.Name = item.Name;
                orderItem.Quantity = item.QtyOrdered;
                orderItem.Code = item.ItemId.ToString();
                orderItem.SKU = item.Sku;
                orderItem.UnitPrice = Convert.ToDecimal(item.ParentItem?.Price ?? item.Price);
                if (orderItem.UnitPrice == 0m)
                {
                    Item configurableItemWithPrice = itemList.FirstOrDefault(i => i.ProductType == "configurable" && i.Sku == orderItem.SKU && (i.ParentItem?.Price ?? i.Price) > 0D);
                    if (configurableItemWithPrice != null)
                    {
                        orderItem.UnitPrice = Convert.ToDecimal(configurableItemWithPrice.ParentItem?.Price ?? configurableItemWithPrice.Price);
                    }
                }

                orderItem.Weight = item.Weight;

                Item magentoOrderItem = webClient.GetItem(item.ItemId);

                if (magentoOrderItem?.ProductOption?.ExtensionAttributes != null)
                {
                    AddCustomOptions(orderItem, magentoOrderItem.ProductOption.ExtensionAttributes.CustomOptions);
                }

                if (magentoOrderItem?.ParentItemId != null)
                {
                    Item magentoParentOrderItem = webClient.GetItem(magentoOrderItem.ParentItemId.Value);

                    if (magentoParentOrderItem?.ProductOption?.ExtensionAttributes != null)
                    {
                        AddCustomOptions(orderItem, magentoParentOrderItem.ProductOption.ExtensionAttributes.CustomOptions);
                    }
                }
            }
        }

        /// <summary>
        /// Add all of the custom options for the order item
        /// </summary>
        private void AddCustomOptions(OrderItemEntity item, IEnumerable<CustomOption> options)
        {
            if (options == null || options.None())
            {
                return;
            }

            try
            {
                // We have to get the option from magento to get the option title
                Product product = webClient.GetProduct(item.SKU);

                foreach (CustomOption option in options)
                {
                    ProductOptionDetail optionDetail = product.Options.FirstOrDefault(o => o.OptionID == option.OptionID);

                    OrderItemAttributeEntity orderItemAttribute = InstantiateOrderItemAttribute(item);
                    orderItemAttribute.Description = option.OptionValue;
                    orderItemAttribute.Name = optionDetail?.Title ?? "Option";
                    orderItemAttribute.UnitPrice = 0;
                }
            }
            catch (MagentoException ex)
            {
                // if there is an issue getting product options keep going
                // we don't want options to keep the download from succeeding
                log.Error($"Error getting Item Options {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Loads the order charges.
        /// </summary>
        private void LoadOrderCharges(OrderEntity orderEntity, Order magentoOrder)
        {
            if (!magentoOrder.TaxAmount.IsEquivalentTo(0))
            {
                InstantiateOrderCharge(orderEntity, "TAX", "tax", Convert.ToDecimal(magentoOrder.TaxAmount));
            }

            if (!magentoOrder.ShippingAmount.IsEquivalentTo(0))
            {
                InstantiateOrderCharge(orderEntity, "SHIPPING", "shipping",
                    Convert.ToDecimal(magentoOrder.ShippingAmount));
            }

            if (!magentoOrder.DiscountAmount.IsEquivalentTo(0))
            {
                InstantiateOrderCharge(orderEntity, "DISCOUNT", magentoOrder.DiscountDescription ?? "discount",
                    Convert.ToDecimal(magentoOrder.DiscountAmount));
            }
        }
    }
}
