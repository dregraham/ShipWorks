using System;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Import;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Warehouse.Orders;
using ShipWorks.Warehouse.Orders.DTO;

namespace ShipWorks.Stores.Platforms.ThreeDCart.Warehouse
{
    /// <summary>
    /// ThreeDCart warehouse order factory
    /// </summary>
    [KeyedComponent(typeof(IWarehouseOrderFactory), StoreTypeCode.ThreeDCart)]
    public class ThreeDCartWarehouseOrderFactory : WarehouseOrderFactory
    {
        private const string threeDCartEntryKey = "threeDCart";
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public ThreeDCartWarehouseOrderFactory(IOrderElementFactory orderElementFactory,
                                                   Func<Type, ILog> logFactory) : base(orderElementFactory)
        {
            log = logFactory(typeof(ThreeDCartWarehouseOrderFactory));
        }

        /// <summary>
        /// Create an order entity with a ThreeDCart identifier
        /// </summary>
        protected override async Task<GenericResult<OrderEntity>> CreateStoreOrderEntity(IStoreEntity store, StoreType storeType, WarehouseOrder warehouseOrder)
        {
            if (!long.TryParse(warehouseOrder.OrderNumber, out long orderNumber))
            {
                return GenericResult.FromError<OrderEntity>($"Skipping order because order number {warehouseOrder.OrderNumber} could not be parsed as a number");
            }

            // get the order instance
            GenericResult<OrderEntity> result = await orderElementFactory
                .CreateOrder(new ThreeDCartOrderIdentifier(orderNumber, warehouseOrder.OrderNumberPrefix, warehouseOrder.OrderNumberPostfix)).ConfigureAwait(false);

            if (result.Failure)
            {
                log.InfoFormat("Skipping order '{0}': {1}.", warehouseOrder.OrderNumber, result.Message);
            }

            return result;
        }

        /// <summary>
        /// Load ThreeDCart order details
        /// </summary>
        protected override void LoadStoreOrderDetails(IStoreEntity store, OrderEntity orderEntity, WarehouseOrder warehouseOrder)
        {
            ThreeDCartOrderEntity threeDCartOrderEntity = (ThreeDCartOrderEntity) orderEntity;
            var threeDCartWarehouseOrder = warehouseOrder.AdditionalData[threeDCartEntryKey].ToObject<ThreeDCartWarehouseOrder>();

            threeDCartOrderEntity.ThreeDCartOrderID = threeDCartWarehouseOrder.ThreeDCartOrderID;
        }

        /// <summary>
        /// Load ThreeDCart item details
        /// </summary>
        protected override void LoadStoreItemDetails(IStoreEntity store, OrderItemEntity itemEntity, WarehouseOrderItem warehouseItem)
        {
            ThreeDCartOrderItemEntity threeDCartItemEntity = (ThreeDCartOrderItemEntity) itemEntity;
            var threeDCartWarehouseItem = warehouseItem.AdditionalData[threeDCartEntryKey].ToObject<ThreeDCartWarehouseItem>();

            threeDCartItemEntity.ThreeDCartShipmentID = threeDCartWarehouseItem.ThreeDCartShipmentId;
        }
    }
}
