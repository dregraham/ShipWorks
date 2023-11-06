using Interapptive.Shared.ComponentRegistration;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal;
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

        public override string GetCarrierName(IShippingManager shippingManager, ShipmentEntity shipment)
        {
            return GetTrackingCompany(shipment);
        }

        /// <summary>
        /// Gets the tracking company for uploading tracking to Shopify
        /// </summary>
        public static string GetTrackingCompany(ShipmentEntity shipment)
        {
            if (ShipmentTypeManager.IsPostal(shipment.ShipmentTypeCode))
            {
                ShippingManager.EnsureShipmentLoaded(shipment);
                if (ShipmentTypeManager.IsDhl((PostalServiceType) shipment.Postal.Service))
                {
                    return "DHL eCommerce";
                }
            }

            if (shipment.ShipmentTypeCode == ShipmentTypeCode.Other)
            {
                ShippingManager.EnsureShipmentLoaded(shipment);
                string carrier = shipment.Other.Carrier;

                if (string.IsNullOrWhiteSpace(carrier))
                {
                    return "Other";
                }

                return carrier;
            }

			if (shipment.ShipmentTypeCode == ShipmentTypeCode.Asendia)
			{
				return "Asendia USA";
			}

            return ShippingManager.GetCarrierName((ShipmentTypeCode) shipment.ShipmentType);
        }

        public override string GetTrackingUrl(ShipmentEntity shipment)
        {
            ShipmentType shipmentType = ShipmentTypeManager.GetType(shipment);
            shipmentType.LoadShipmentData(shipment, true);

            return shipmentType.GetCarrierTrackingUrl(shipment);
        }
    }
}
