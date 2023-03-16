using System;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Platform.OnlineUpdating;
using ShipWorks.Stores.Platforms.Shopify.Enums;
using ShipWorks.Warehouse.Orders;

namespace ShipWorks.Stores.Platforms.Shopify.OnlineUpdating
{
	[KeyedComponent(typeof(IPlatformOnlineUpdater), StoreTypeCode.Shopify)]
	internal class ShopifyPlatformOnlineUpdater : PlatformOnlineUpdater
	{
		public ShopifyPlatformOnlineUpdater(IOrderManager orderManager, IShippingManager shippingManager, ISqlAdapterFactory sqlAdapterFactory, Func<IWarehouseOrderClient> createWarehouseOrderClient, IIndex<StoreTypeCode, IOnlineUpdater> legacyStoreSpecificOnlineUpdaterFactory, IIndex<StoreTypeCode, IPlatformOnlineUpdaterBehavior> platformOnlineUpdateBehavior)
			: base(orderManager, shippingManager, sqlAdapterFactory, createWarehouseOrderClient, legacyStoreSpecificOnlineUpdaterFactory, platformOnlineUpdateBehavior)
		{ }

		protected override void SetOrderStatuses(OrderEntity order)
		{
			base.SetOrderStatuses(order);
			ShopifyOrderEntity shopifyOrder = (ShopifyOrderEntity) order;
			shopifyOrder.FulfillmentStatusCode = (int) ShopifyFulfillmentStatus.Fulfilled;
		}
	}
}
