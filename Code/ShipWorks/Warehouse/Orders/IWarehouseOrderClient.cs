using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms.Amazon.OnlineUpdating.DTO;
using ShipWorks.Warehouse.Orders.DTO;

namespace ShipWorks.Warehouse.Orders
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
        /// Upload shipment notification information to Hub. This will only work for orders from shipengine
        /// </summary>
        Task<Result> NotifyShipped(string salesOrderId, string trackingNumber, string carrier, bool useSwatId);

        /// <summary>
        /// Upload a order to the hub
        /// </summary>
        Task<GenericResult<IEnumerable<WarehouseUploadOrderResponse>>> UploadOrders(IEnumerable<OrderEntity> orders, IStoreEntity store, bool assignBatch);

        /// <summary>
        /// Send a shipment to the hub
        /// </summary>
        Task<Result> UploadShipment(ShipmentEntity shipmentEntity, Guid hubOrderID, string tangoShipmentID);

        /// <summary>
        /// Send void to the hub
        /// </summary>
        Task<Result> UploadVoid(long shipmentID, Guid hubOrderID, string tangoShipmentID);

        /// <summary>
        /// Reroute order items for the given warehouse ID from the ShipWorks Warehouse app
        /// </summary>
        Task<Result> RerouteOrderItems(Guid hubOrderID, ItemsToReroute itemsToReroute);

        /// <summary>
        /// Upload Amazon shipments
        /// </summary>
        Task<Result> UploadAmazonShipments(AmazonBulkUploadShipmentsRequest body);
    }
}
