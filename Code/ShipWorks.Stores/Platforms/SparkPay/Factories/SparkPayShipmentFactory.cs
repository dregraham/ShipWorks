using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Stores.Platforms.SparkPay.DTO;
using System;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.Postal;

namespace ShipWorks.Stores.Platforms.SparkPay.Factories
{
    public class SparkPayShipmentFactory
    {
        readonly IShippingManager shippingManager;

        public SparkPayShipmentFactory(IShippingManager shippingManager)
        {
            this.shippingManager = shippingManager;
        }

        public Shipment Create(ShipmentEntity shipment)
        {
            string carrierName = GetCarrierName(shipment);
            string service = shippingManager.GetServiceUsed(shipment);

            return new Shipment
            {
                ShippedAt = (DateTime)shipment.ProcessedDate,
                OrderId = shipment.Order.OrderNumber,
                TrackingNumbers = shipment.TrackingNumber,
                ShippingMethod = $"{carrierName} {service}",
                ShipmentName = $"{carrierName} {service}"
            };
        }

        /// <summary>
        /// Get the carrier name for the given shipment type
        /// </summary>
        private string GetCarrierName(ShipmentEntity shipment)
        {
            ShipmentTypeCode shipmentTypeCode = (ShipmentTypeCode)shipment.ShipmentType;

            if (PostalUtility.IsPostalShipmentType(shipmentTypeCode))
            {
                return "USPS";
            }

            switch (shipmentTypeCode)
            {
                case ShipmentTypeCode.UpsOnLineTools:
                case ShipmentTypeCode.UpsWorldShip:
                    return "UPS";
                case ShipmentTypeCode.FedEx:
                    return "FedEx";
                default:
                    return EnumHelper.GetDescription(shipmentTypeCode);
            }
        }
    }
}
