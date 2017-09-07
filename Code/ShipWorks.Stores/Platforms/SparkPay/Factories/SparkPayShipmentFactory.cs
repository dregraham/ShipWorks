using System;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Stores.Platforms.SparkPay.DTO;

namespace ShipWorks.Stores.Platforms.SparkPay.Factories
{
    /// <summary>
    /// SparkPay shipment factory
    /// </summary>
    [Component]
    public class SparkPayShipmentFactory : ISparkPayShipmentFactory
    {
        readonly IShippingManager shippingManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public SparkPayShipmentFactory(IShippingManager shippingManager)
        {
            this.shippingManager = shippingManager;
        }

        /// <summary>
        /// Create a SparkPay shipment
        /// </summary>
        public Shipment Create(ShipmentEntity shipment, long orderNumber)
        {
            string carrierName = GetCarrierName(shipment);
            string service = shippingManager.GetOverriddenServiceUsed(shipment).RemoveSymbols();

            return new Shipment
            {
                ShippedAt = (DateTime) shipment.ProcessedDate,
                OrderId = orderNumber,
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
            ShipmentTypeCode shipmentTypeCode = (ShipmentTypeCode) shipment.ShipmentType;

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
