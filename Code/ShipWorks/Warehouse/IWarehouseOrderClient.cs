using System;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using ShipWorks.Warehouse.DTO.Orders;

namespace ShipWorks.Warehouse
{
    /// <summary>
    /// Client for retrieving orders from the ShipWorks Warehouse app
    /// </summary>
    public interface IWarehouseOrderClient
    {
        /// <summary>
        /// Get orders for the given warehouse store ID from the ShipWorks Warehouse app
        /// </summary>
        Task<WarehouseGetOrdersResponse> GetOrders(string warehouseID, string warehouseStoreID, long mustRecentSequence,
                                                    StoreTypeCode storeType, Guid batchId);

        /// <summary>
        /// Send a shipment to the hub
        /// </summary>
        Task UploadShipment(ShipmentEntity shipmentEntity, Guid hubOrderID, string tangoShipmentID);
    }
}
