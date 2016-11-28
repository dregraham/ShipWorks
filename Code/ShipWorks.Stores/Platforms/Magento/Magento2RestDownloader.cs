using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.Magento.DTO;
using ShipWorks.Stores.Platforms.Magento.Enums;

namespace ShipWorks.Stores.Platforms.Magento
{
    /// <summary>
    /// Downloader for Magento 2 REST API
    /// </summary>
    /// <seealso cref="ShipWorks.Stores.Communication.StoreDownloader" />
    [KeyedComponent(typeof(StoreDownloader), MagentoVersion.MagentoTwoREST, true)]
    public class Magento2RestDownloader : StoreDownloader
    {
        private readonly IMagentoTwoRestClient webClient;
        private readonly ISqlAdapterRetry sqlAdapter;
        private readonly Uri storeUrl;
        private readonly MagentoStoreEntity magentoStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="Magento2RestDownloader" /> class.
        /// </summary>
        /// <param name="store"></param>
        public Magento2RestDownloader(StoreEntity store) :
            this(store, new MagentoTwoRestClient(), new SqlAdapterRetry<SqlException>(5, -5, "Magento2RestDownloader.Download"))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Magento2RestDownloader"/> class.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="webClient">The web client.</param>
        /// <param name="sqlAdapter">The SQL adapter.</param>
        public Magento2RestDownloader(StoreEntity store, IMagentoTwoRestClient webClient, ISqlAdapterRetry sqlAdapter) : base(store)
        {
            magentoStore = (MagentoStoreEntity) store;
            storeUrl = new Uri(magentoStore.ModuleUrl);
            this.webClient = webClient;
            this.sqlAdapter = sqlAdapter;
        }

        /// <summary>
        /// Download orders for the Magento store
        /// </summary>
        protected override void Download(TrackedDurationEvent trackedDurationEvent)
        {
            string token = webClient.GetToken(storeUrl, magentoStore.ModuleUsername,
                SecureText.Decrypt(magentoStore.ModulePassword, magentoStore.ModuleUsername));

            while (true)
            {
                DateTime? lastModifiedDate = GetOnlineLastModifiedStartingPoint();

                OrdersResponse ordersResponse = webClient.GetOrders(lastModifiedDate.Value, storeUrl, token);

                foreach (Order magentoOrder in ordersResponse.Orders)
                {
                    MagentoOrderIdentifier orderIdentifier = new MagentoOrderIdentifier(magentoOrder.entity_id, "", "");
                    OrderEntity orderEntity = InstantiateOrder(orderIdentifier);
                    LoadOrder(orderEntity, magentoOrder);
                    sqlAdapter.ExecuteWithRetry(() => SaveDownloadedOrder(orderEntity));
                }

                if (ordersResponse.Orders.None())
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Loads the order.
        /// </summary>
        public void LoadOrder(OrderEntity orderEntity, Order magentoOrder)
        {
            orderEntity.OnlineLastModified = DateTime.Parse(magentoOrder.updated_at).ToUniversalTime();
            orderEntity.OnlineStatus = magentoOrder.status;
            orderEntity.RequestedShipping = magentoOrder.shipping_description;

            LoadAddresses(orderEntity, magentoOrder);

            if (orderEntity.IsNew)
            {
                orderEntity.OrderDate = DateTime.Parse(magentoOrder.created_at).ToUniversalTime();
                orderEntity.OrderNumber = magentoOrder.entity_id;
                orderEntity.OrderTotal = Convert.ToDecimal(magentoOrder.grand_total);
                ((MagentoOrderEntity) orderEntity).MagentoOrderID = magentoOrder.entity_id;
                LoadItems(orderEntity, magentoOrder.items);
                LoadOrderCharges(orderEntity, magentoOrder);
            }
        }

        /// <summary>
        /// Loads the addresses.
        /// </summary>
        private void LoadAddresses(OrderEntity orderEntity, Order magentoOrder)
        {
            Address shippingAddress =
                magentoOrder.extension_attributes.shipping_assignments.FirstOrDefault()?.shipping.address;

            if (shippingAddress != null)
            {
                new PersonAdapter(orderEntity, "Ship")
                {
                    FirstName = shippingAddress.firstname,
                    MiddleName = shippingAddress.middlename,
                    LastName = shippingAddress.lastname,
                    Company = shippingAddress.company,
                    Phone = shippingAddress.telephone,
                    Email = shippingAddress.email,
                    Street1 = shippingAddress.street.ElementAtOrDefault(0),
                    Street2 = shippingAddress.street.ElementAtOrDefault(1),
                    Street3 = shippingAddress.street.ElementAtOrDefault(2),
                    City = shippingAddress.city,
                    StateProvCode = Geography.GetStateProvCode(shippingAddress.region_code),
                    PostalCode = shippingAddress.postcode,
                    CountryCode = Geography.GetCountryCode(shippingAddress.country_id),
                };
            }

            BillingAddress billingAddress = magentoOrder.billing_address;

            if (billingAddress != null)
            {
                new PersonAdapter(orderEntity, "Bill")
                {
                    FirstName = billingAddress.firstname,
                    MiddleName = billingAddress.middlename,
                    LastName = billingAddress.lastname,
                    Company = billingAddress.company,
                    Phone = billingAddress.telephone,
                    Email = billingAddress.email,
                    Street1 = billingAddress.street.ElementAtOrDefault(0),
                    Street2 = billingAddress.street.ElementAtOrDefault(1),
                    Street3 = billingAddress.street.ElementAtOrDefault(2),
                    City = billingAddress.city,
                    StateProvCode = Geography.GetStateProvCode(billingAddress.region_code),
                    PostalCode = billingAddress.postcode,
                    CountryCode = Geography.GetCountryCode(billingAddress.country_id)
                };
            }
        }

        /// <summary>
        /// Loads the items.
        /// </summary>
        private void LoadItems(OrderEntity orderEntity, List<Item> items)
        {
            foreach (Item item in items)
            {
                OrderItemEntity orderItem = InstantiateOrderItem(orderEntity);

                orderItem.Name = item.name;
                orderItem.Quantity = item.qty_ordered;
                orderItem.Code = item.item_id.ToString();
                orderItem.SKU = item.sku;
                orderItem.UnitPrice = item.price;
                orderItem.Weight = item.weight;
            }
        }

        /// <summary>
        /// Loads the order charges.
        /// </summary
        private void LoadOrderCharges(OrderEntity orderEntity, Order magentoOrder)
        {
            InstantiateOrderCharge(orderEntity, "TAX", "tax", Convert.ToDecimal(magentoOrder.tax_amount));
            InstantiateOrderCharge(orderEntity, "SHIPPING", "shipping", Convert.ToDecimal(magentoOrder.shipping_amount));
            InstantiateOrderCharge(orderEntity, "DISCOUNT", magentoOrder.discount_description ?? "discount", Convert.ToDecimal(magentoOrder.discount_amount));
        }
    }
}
