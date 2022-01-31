using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Shipping.Tracking.DTO;
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
        Task SendShipment(string trackingNumber, string carrierCode, string warehouseID);

        /// <summary>
        /// Get tracking information from the hub
        /// </summary>
        Task<IEnumerable<TrackingNotification>> GetTracking(string WarehouseID, DateTime lastUpdateDate);
    }
}