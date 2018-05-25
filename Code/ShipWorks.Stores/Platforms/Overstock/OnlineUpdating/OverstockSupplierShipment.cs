using System;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.Overstock.OnlineUpdating
{
    /// <summary>
    /// Shipment as needed to upload data to Overstock
    /// </summary>
    public class OverstockSupplierShipment
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OverstockSupplierShipment(OverstockOrderDetail order, IShipmentEntity shipment, IOverstockOrderItemEntity item)
        {
            IsManual = order.IsManual;
            SalesChannelOrderNumber = order.OrderNumberComplete;
            SalesChannelName = order.SalesChannelName;
            WarehouseCode = order.WarehouseCode;
            SalesChannelLineNumber = item.SalesChannelLineNumber;
            Quantity = item.Quantity;
            ShipDate = shipment.ShipDate;
            TrackingNumber = shipment.TrackingNumber;
            (CarrierCode, ServiceLevelCode) = OverstockCarrierServiceTranslator.GetCarrierValues(shipment);
        }

        /// <summary>
        /// Is the order manual
        /// </summary>
        public bool IsManual { get; set; }

        /// <summary>
        /// Name of the sales channel
        /// </summary>
        public string SalesChannelName { get; set; }

        /// <summary>
        /// Sales channel order number
        /// </summary>
        public string SalesChannelOrderNumber { get; set; }

        /// <summary>
        /// Sales channel line number
        /// </summary>
        public long SalesChannelLineNumber { get; set; }

        /// <summary>
        /// Name of the warehouse
        /// </summary>
        public string WarehouseCode { get; set; }

        /// <summary>
        /// Carrier code
        /// </summary>
        public string CarrierCode { get; set; }

        /// <summary>
        /// Service level
        /// </summary>
        public string ServiceLevelCode { get; set; }

        /// <summary>
        /// Tracking number of the shipment
        /// </summary>
        public string TrackingNumber { get; set; }

        /// <summary>
        /// Quantity of the item
        /// </summary>
        public double Quantity { get; set; }

        /// <summary>
        /// Date of the shipment
        /// </summary>
        public DateTime ShipDate { get; set; }
    }
}
