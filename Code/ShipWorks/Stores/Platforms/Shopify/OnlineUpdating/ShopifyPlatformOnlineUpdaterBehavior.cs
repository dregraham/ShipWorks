using System;
using Interapptive.Shared.ComponentRegistration;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Platform.OnlineUpdating;
using ShipWorks.Stores.Platforms.ShipEngine.Apollo;
using ShipWorks.Stores.Platforms.Shopify.Enums;

namespace ShipWorks.Stores.Platforms.Shopify.OnlineUpdating
{
	[KeyedComponent(typeof(IPlatformOnlineUpdaterBehavior), StoreTypeCode.Shopify)]
	public class ShopifyPlatformDownloaderBehavior : DefaultPlatformOnlineUpdaterBehavior
	{
		public override bool UseSwatId => true;

		public override bool IncludeSalesOrderItems => true;

		public override bool SetOrderStatusesOnShipmentNotify => true;

		public override void SetOrderStatuses(OrderEntity order, UnitOfWork2 unitOfWork)
		{
			ShopifyOrderEntity shopifyOrder = (ShopifyOrderEntity) order;
			shopifyOrder.OnlineStatus = OrderSourceSalesOrderStatus.Completed.ToString();
			shopifyOrder.FulfillmentStatusCode = (int) ShopifyFulfillmentStatus.Fulfilled;
			
			unitOfWork.AddForSave(shopifyOrder);
		}
	}
}
