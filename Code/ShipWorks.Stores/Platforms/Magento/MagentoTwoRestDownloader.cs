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
    public class MagentoTwoRestDownloader : StoreDownloader
    {
        private readonly IMagentoTwoRestClient webClient;
        private readonly ISqlAdapterRetry sqlAdapter;
        private readonly Uri storeUrl;
        private readonly MagentoStoreEntity magentoStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="MagentoTwoRestDownloader" /> class.
        /// </summary>
        /// <param name="store"></param>
        public MagentoTwoRestDownloader(StoreEntity store) :
            this(store, new MagentoTwoRestClient(), new SqlAdapterRetry<SqlException>(5, -5, "Magento2RestDownloader.Download"))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MagentoTwoRestDownloader"/> class.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="webClient">The web client.</param>
        /// <param name="sqlAdapter">The SQL adapter.</param>
        public MagentoTwoRestDownloader(StoreEntity store, IMagentoTwoRestClient webClient, ISqlAdapterRetry sqlAdapter) : base(store)
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
                    MagentoOrderIdentifier orderIdentifier = new MagentoOrderIdentifier(magentoOrder.EntityId, "", "");
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
            orderEntity.OnlineLastModified =
                DateTime.SpecifyKind(DateTime.Parse(magentoOrder.UpdatedAt), DateTimeKind.Utc);
			orderEntity.OnlineStatus = magentoOrder.Status;
			orderEntity.RequestedShipping = magentoOrder.ShippingDescription;

            LoadAddresses(orderEntity, magentoOrder);

            if (orderEntity.IsNew)
            {
                orderEntity.OrderDate = 
                    DateTime.SpecifyKind(DateTime.Parse(magentoOrder.CreatedAt), DateTimeKind.Utc);
                orderEntity.OrderNumber = magentoOrder.EntityId;
                orderEntity.OrderTotal = Convert.ToDecimal(magentoOrder.GrandTotal);
                ((MagentoOrderEntity) orderEntity).MagentoOrderID = magentoOrder.EntityId;
                LoadItems(orderEntity, magentoOrder.Items);
                LoadOrderCharges(orderEntity, magentoOrder);
            }
        }

        /// <summary>
        /// Loads the addresses.
        /// </summary>
        private void LoadAddresses(OrderEntity orderEntity, Order magentoOrder)
        {
            Address shippingAddress =
                magentoOrder.ExtensionAttributes.ShippingAssignments.FirstOrDefault()?.Shipping.Address;

            if (shippingAddress != null)
            {
                new PersonAdapter(orderEntity, "Ship")
                {
                    FirstName = shippingAddress.Firstname,
                    MiddleName = shippingAddress.Middlename,
                    LastName = shippingAddress.Lastname,
                    Company = shippingAddress.Company,
                    Phone = shippingAddress.Telephone,
                    Email = shippingAddress.Email,
                    Street1 = shippingAddress.Street.ElementAtOrDefault(0),
                    Street2 = shippingAddress.Street.ElementAtOrDefault(1),
                    Street3 = shippingAddress.Street.ElementAtOrDefault(2),
                    City = shippingAddress.City,
                    StateProvCode = Geography.GetStateProvCode(shippingAddress.RegionCode),
                    PostalCode = shippingAddress.Postcode,
                    CountryCode = Geography.GetCountryCode(shippingAddress.CountryId),
                };
            }

            BillingAddress billingAddress = magentoOrder.BillingAddress;

            if (billingAddress != null)
            {
                new PersonAdapter(orderEntity, "Bill")
                {
                    FirstName = billingAddress.Firstname,
                    MiddleName = billingAddress.Middlename,
                    LastName = billingAddress.Lastname,
                    Company = billingAddress.Company,
                    Phone = billingAddress.Telephone,
                    Email = billingAddress.Email,
                    Street1 = billingAddress.Street.ElementAtOrDefault(0),
                    Street2 = billingAddress.Street.ElementAtOrDefault(1),
                    Street3 = billingAddress.Street.ElementAtOrDefault(2),
                    City = billingAddress.City,
                    StateProvCode = Geography.GetStateProvCode(billingAddress.RegionCode),
                    PostalCode = billingAddress.Postcode,
                    CountryCode = Geography.GetCountryCode(billingAddress.CountryId)
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

                orderItem.Name = item.Name;
                orderItem.Quantity = item.QtyOrdered;
                orderItem.Code = item.ItemId.ToString();
                orderItem.SKU = item.Sku;
                orderItem.UnitPrice = Convert.ToDecimal(item.Price);
                orderItem.Weight = item.Weight;
            }
        }

        /// <summary>
        /// Loads the order charges.
        /// </summary>
        private void LoadOrderCharges(OrderEntity orderEntity, Order magentoOrder)
        {
            if (Math.Abs(magentoOrder.TaxAmount) > .001)
            {
                InstantiateOrderCharge(orderEntity, "TAX", "tax", Convert.ToDecimal(magentoOrder.TaxAmount));
            }

            if (Math.Abs(magentoOrder.ShippingAmount) > .001)
            {
                InstantiateOrderCharge(orderEntity, "SHIPPING", "shipping",
                    Convert.ToDecimal(magentoOrder.ShippingAmount));
            }

            if (Math.Abs(magentoOrder.DiscountAmount) > .001)
            {
                InstantiateOrderCharge(orderEntity, "DISCOUNT", magentoOrder.DiscountDescription ?? "discount",
                    Convert.ToDecimal(magentoOrder.DiscountAmount));
            }
        }
    }
}
