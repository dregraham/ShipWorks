using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Metrics;
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
            MagentoStoreEntity magentoStore = (MagentoStoreEntity) store;
            sqlAdapter = sqlAdapterRetryFactory.Create<SqlException>(5, -5, "MagentoRestDownloader.Download");
            log = logFactory(typeof(MagentoTwoRestDownloader));
            webClient = webClientFactory(magentoStore);
        }

        /// <summary>
        /// Download orders for the Magento store
        /// </summary>
        protected override void Download(TrackedDurationEvent trackedDurationEvent)
        {
            int currentPage = 1;
            int totalOrders = 0;
            int savedOrders = 0;

            DateTime? lastModifiedDate = GetOnlineLastModifiedStartingPoint();

            try
            {
                do
                {
                    OrdersResponse ordersResponse = webClient.GetOrders(lastModifiedDate, currentPage);
                    totalOrders = ordersResponse.TotalCount;
                    foreach (Order magentoOrder in ordersResponse.Orders)
                    {
                        MagentoOrderIdentifier orderIdentifier = new MagentoOrderIdentifier(magentoOrder.EntityId, "", "");
                        MagentoOrderEntity orderEntity = InstantiateOrder(orderIdentifier) as MagentoOrderEntity;
                        LoadOrder(orderEntity, magentoOrder);
                        sqlAdapter.ExecuteWithRetry(() => SaveDownloadedOrder(orderEntity));
                        savedOrders++;
                    }

                    currentPage++;
                } while (savedOrders < totalOrders);
            }
            catch (MagentoException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Loads the order.
        /// </summary>
        public void LoadOrder(MagentoOrderEntity orderEntity, Order magentoOrder)
        {
            orderEntity.OnlineLastModified =
                DateTime.SpecifyKind(DateTime.Parse(magentoOrder.UpdatedAt), DateTimeKind.Utc);
            orderEntity.OnlineStatus = magentoOrder.Status;
            orderEntity.RequestedShipping = magentoOrder.ShippingDescription;

            LoadAddresses(orderEntity, magentoOrder);
            LoadNotes(orderEntity, magentoOrder);

            if (orderEntity.IsNew)
            {
                orderEntity.OrderDate =
                    DateTime.SpecifyKind(DateTime.Parse(magentoOrder.CreatedAt), DateTimeKind.Utc);
                orderEntity.OrderNumber = magentoOrder.EntityId;
                orderEntity.OrderTotal = Convert.ToDecimal(magentoOrder.GrandTotal);
                orderEntity.MagentoOrderID = magentoOrder.EntityId;
                LoadItems(orderEntity, magentoOrder.Items);
                LoadOrderCharges(orderEntity, magentoOrder);
            }
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
        /// Loads the items.
        /// </summary>
        private void LoadItems(OrderEntity orderEntity, IEnumerable<Item> items)
        {
            foreach (Item item in items.Where(i => i.ProductType != "configurable"))
            {
                OrderItemEntity orderItem = InstantiateOrderItem(orderEntity);

                orderItem.Name = item.Name;
                orderItem.Quantity = item.QtyOrdered;
                orderItem.Code = item.ItemId.ToString();
                orderItem.SKU = item.Sku;
                orderItem.UnitPrice = Convert.ToDecimal(item.ParentItem?.Price ?? item.Price);
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
        private void AddCustomOptions(OrderItemEntity item, IEnumerable<CustomOption> customOptions)
        {
            CustomOption[] options = customOptions as CustomOption[] ?? customOptions.ToArray();

            if (options.Any())
            {
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
                    // we dont want options to keep the download from succeeding
                    log.Error($"Error getting Item Options {ex.Message}", ex);
                }
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
