﻿using System;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Import;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.Shopify.Enums;
using ShipWorks.Warehouse;
using ShipWorks.Warehouse.DTO.Orders;

namespace ShipWorks.Stores.Platforms.Shopify.Warehouse
{
    /// <summary>
    /// Shopify warehouse order factory
    /// </summary>
    [KeyedComponent(typeof(IWarehouseOrderFactory), StoreTypeCode.Shopify)]
    public class ShopifyWarehouseOrderFactory : WarehouseOrderFactory
    {
        private const string shopifyEntryKey = "shopify";
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyWarehouseOrderFactory(IOrderElementFactory orderElementFactory,
            Func<Type, ILog> logFactory) : base(orderElementFactory)
        {
            log = logFactory(typeof(ShopifyWarehouseOrderFactory));
        }

        /// <summary>
        /// Create an order entity with a Shopify identifier
        /// </summary>
        protected override async Task<GenericResult<OrderEntity>> CreateStoreOrderEntity(IStoreEntity store, StoreType storeType, WarehouseOrder warehouseOrder)
        {
            var shopifyWarehouseOrder = warehouseOrder.AdditionalData[shopifyEntryKey].ToObject<ShopifyWarehouseOrder>();

            var identifier = new ShopifyOrderIdentifier(long.Parse(shopifyWarehouseOrder.ShopifyOrderID));

            // get the order instance
            GenericResult<OrderEntity> result = await orderElementFactory
                .CreateOrder(identifier).ConfigureAwait(false);

            if (result.Failure)
            {
                log.InfoFormat("Skipping order '{0}': {1}.", warehouseOrder.OrderNumber, result.Message);
            }

            return result;
        }

        /// <summary>
        /// Load Shopify order details
        /// </summary>
        protected override void LoadStoreOrderDetails(IStoreEntity store, OrderEntity orderEntity, WarehouseOrder warehouseOrder)
        {
            ShopifyOrderEntity shopifyOrderEntity = (ShopifyOrderEntity) orderEntity;
            var shopifyWarehouseOrder = warehouseOrder.AdditionalData[shopifyEntryKey].ToObject<ShopifyWarehouseOrder>();

            shopifyOrderEntity.ChangeOrderNumber(warehouseOrder.OrderNumber);
            shopifyOrderEntity.FulfillmentStatusCode = (int) EnumHelper.GetEnumByApiValue<ShopifyFulfillmentStatus>(shopifyWarehouseOrder.FulfillmentStatus);
            shopifyOrderEntity.PaymentStatusCode = (int) EnumHelper.GetEnumByApiValue<ShopifyPaymentStatus>(shopifyWarehouseOrder.PaymentStatus);

            var shopifyStore = (IShopifyStoreEntity) store;

            var requestedShippingField = (ShopifyRequestedShippingField) shopifyStore.ShopifyRequestedShippingOption;

            shopifyOrderEntity.RequestedShipping = requestedShippingField == ShopifyRequestedShippingField.Code ?
                shopifyWarehouseOrder.RequestedShippingCode : shopifyWarehouseOrder.RequestedShippingTitle;
        }

        /// <summary>
        /// Load Shopify item details
        /// </summary>
        protected override void LoadStoreItemDetails(IStoreEntity store, OrderItemEntity itemEntity, WarehouseOrderItem warehouseItem)
        {
            ShopifyOrderItemEntity ShopifyItemEntity = (ShopifyOrderItemEntity) itemEntity;
            var shopifyWarehouseItem = warehouseItem.AdditionalData[shopifyEntryKey].ToObject<ShopifyWarehouseItem>();

            ShopifyItemEntity.ShopifyOrderItemID = shopifyWarehouseItem.ShopifyOrderItemID;
            ShopifyItemEntity.ShopifyProductID = shopifyWarehouseItem.ShopifyProductID;
            ShopifyItemEntity.InventoryItemID = shopifyWarehouseItem.ShopifyInventoryItemID;
        }
    }
}
