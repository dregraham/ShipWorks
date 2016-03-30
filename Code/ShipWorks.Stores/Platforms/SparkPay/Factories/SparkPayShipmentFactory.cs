using Autofac.Features.Indexed;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Stores.Platforms.SparkPay.DTO;
using System;

namespace ShipWorks.Stores.Platforms.SparkPay.Factories
{
    public class SparkPayShipmentFactory
    {
        IIndex<ShipmentTypeCode, ShipmentType> shipmentTypeFactory;
        IShippingManager shippingManager;

        public SparkPayShipmentFactory(IIndex<ShipmentTypeCode, ShipmentType> shipmentTypeFactory, IShippingManager shippingManager)
        {
            this.shipmentTypeFactory = shipmentTypeFactory;
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
            shippingManager.EnsureShipmentLoaded(shipment);
            
            switch (shipmentTypeCode)
            {
                case ShipmentTypeCode.UpsOnLineTools:
                case ShipmentTypeCode.UpsWorldShip:
                    return "UPS";
                case ShipmentTypeCode.Usps:
                case ShipmentTypeCode.Endicia:
                case ShipmentTypeCode.PostalWebTools:
                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Express1Usps:
                    return "USPS";
                case ShipmentTypeCode.FedEx:
                    return "FedEx";
                case ShipmentTypeCode.OnTrac:
                case ShipmentTypeCode.iParcel:
                    return EnumHelper.GetDescription(shipmentTypeCode);
                case ShipmentTypeCode.Other:
                case ShipmentTypeCode.BestRate:
                case ShipmentTypeCode.Amazon:
                case ShipmentTypeCode.None:
                default:
                    return string.Empty;
            }
        }
    }
}
