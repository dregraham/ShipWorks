using System;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping;
using ShipWorks.Stores.Platforms.ShipEngine.Apollo;

namespace ShipWorks.Stores.Platforms.Platform.OnlineUpdating
{
	public class DefaultPlatformOnlineUpdaterBehavior : IPlatformOnlineUpdaterBehavior
	{
        public virtual bool UseSwatId => false;

		public virtual bool IncludeSalesOrderItems => false;

		public virtual bool SetOrderStatusesOnShipmentNotify => false;

		public virtual void SetOrderStatuses(OrderEntity order, UnitOfWork2 unitOfWork)
		{
			order.OnlineStatus = OrderSourceSalesOrderStatus.Completed.ToString();

			unitOfWork.AddForSave(order);
		}

        /// <summary>
        /// Gets the carrier name that is allowed in shipengine
        /// </summary>
        public virtual string GetCarrierName(IShippingManager shippingManager, ShipmentEntity shipment)
        {
            CarrierDescription otherDesc = null;
            ShipmentTypeCode shipmentTypeCode = shipment.ShipmentTypeCode;

            if (shipmentTypeCode == ShipmentTypeCode.Other)
            {
                otherDesc = shippingManager.GetOtherCarrierDescription(shipment);
            }

            string sfpName = string.Empty;
            if (shipmentTypeCode == ShipmentTypeCode.AmazonSFP)
            {
                sfpName = shipment.AmazonSFP.CarrierName.ToUpperInvariant();
            }

            switch (true)
            {
                case true when otherDesc?.IsDHL ?? false:
                case true when ShipmentTypeManager.ShipmentTypeCodeSupportsDhl(shipmentTypeCode) &&
                        ShipmentTypeManager.IsDhl((PostalServiceType) shipment.Postal.Service):
                    return "dhl_ecommerce";

                case true when shipmentTypeCode == ShipmentTypeCode.Endicia:
                case true when shipmentTypeCode == ShipmentTypeCode.Express1Endicia:
                    return "endicia";

                case true when ShipmentTypeManager.IsPostal(shipmentTypeCode):
                case true when otherDesc?.IsUSPS ?? false:
                case true when sfpName.Equals("USPS", StringComparison.OrdinalIgnoreCase):
                case true when sfpName.Equals("STAMPS_DOT_COM", StringComparison.OrdinalIgnoreCase):
                    return "stamps_com";

                case true when ShipmentTypeManager.IsFedEx(shipmentTypeCode):
                case true when otherDesc?.IsFedEx ?? false:
                case true when sfpName.Equals("FEDEX", StringComparison.OrdinalIgnoreCase):
                    return "fedex";

                case true when ShipmentTypeManager.IsUps(shipmentTypeCode):
                case true when otherDesc?.IsUPS ?? false:
                case true when sfpName.Equals("UPS", StringComparison.OrdinalIgnoreCase):
                    return "ups";

                case true when shipmentTypeCode == ShipmentTypeCode.DhlExpress:
                    return "dhl_express";

                case true when shipmentTypeCode == ShipmentTypeCode.DhlEcommerce:
                    return "dhl_global_mail";

                case true when shipmentTypeCode == ShipmentTypeCode.OnTrac:
                case true when sfpName.Equals("ONTRAC", StringComparison.OrdinalIgnoreCase):
                    return "ontrac";

                case true when shipmentTypeCode == ShipmentTypeCode.AmazonSWA:
                    return "amazon_shipping";

                case true when shipmentTypeCode == ShipmentTypeCode.Asendia:
                    return "asendia";

                default:
                    return "other";
            }
        }

        public virtual string GetTrackingUrl(ShipmentEntity shipment)
        {
            return null;
        }
    }
}
