using System;
using System.Collections.Generic;
using ShipWorks.Stores.Warehouse;

namespace ShipWorks.Shipping.Tracking
{
    /// <summary>
    /// Send and get tracking number information
    /// </summary>
    public interface IPlatformShipmentTrackerClient
    {
        /// <summary>
        /// Send Shipment information to the hub
        /// </summary>
        void SendShipment(string trackingNumber, string carrierCode, string warehouseID);

        /// <summary>
        /// Get tracking information from the hub
        /// </summary>
        IEnumerable<TrackingNotification> GetShipments(string WarehouseID, DateTime lastUpdateDate);
    }
}