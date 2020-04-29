using System;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Import;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Warehouse;
using ShipWorks.Warehouse.DTO.Orders;

namespace ShipWorks.Stores.Platforms.Walmart.Warehouse
{
    /// <summary>
    /// Walmart warehouse order factory
    /// </summary>
    [KeyedComponent(typeof(IWarehouseOrderFactory), StoreTypeCode.Walmart)]
    public class WalmartWarehouseOrderFactory : WarehouseOrderFactory
    {
        private const string walmartEntryKey = "walmart";
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public WalmartWarehouseOrderFactory(IOrderElementFactory orderElementFactory, Func<Type, ILog> logFactory) :
            base(orderElementFactory)
        {
            log = logFactory(typeof(WalmartWarehouseOrderFactory));
        }

        /// <summary>
        /// Create an order entity with a Walmart identifier
        /// </summary>
        protected override async Task<GenericResult<OrderEntity>> CreateStoreOrderEntity(IStoreEntity store, StoreType storeType, WarehouseOrder warehouseOrder)
        {
            var walmartOrderId = warehouseOrder.AdditionalData[walmartEntryKey].ToObject<WalmartWarehouseOrder>().PurchaseOrderId;

            // get the order instance
            GenericResult<OrderEntity> result = await orderElementFactory
                .CreateOrder(new WalmartOrderIdentifier(walmartOrderId)).ConfigureAwait(false);

            if (result.Failure)
            {
                log.InfoFormat("Skipping order '{0}': {1}.", walmartOrderId, result.Message);
            }

            return result;
        }

        /// <summary>
        /// Load Walmart order details
        /// </summary>
        protected override void LoadStoreOrderDetails(IStoreEntity store, OrderEntity orderEntity, WarehouseOrder warehouseOrder)
        {
            var walmartOrderEntity = (WalmartOrderEntity) orderEntity;
            var walmartWarehouseOrder = warehouseOrder.AdditionalData[walmartEntryKey].ToObject<WalmartWarehouseOrder>();

            walmartOrderEntity.OrderNumber = long.Parse(walmartWarehouseOrder.PurchaseOrderId);
            walmartOrderEntity.PurchaseOrderID = walmartWarehouseOrder.PurchaseOrderId;
            walmartOrderEntity.CustomerOrderID = walmartWarehouseOrder.CustomerOrderId;
            walmartOrderEntity.EstimatedDeliveryDate = walmartWarehouseOrder.EstimatedDeliveryDate;
            walmartOrderEntity.EstimatedShipDate = walmartWarehouseOrder.EstimatedShipDate;
            walmartOrderEntity.RequestedShippingMethodCode = walmartWarehouseOrder.RequestedShippingMethodCode;
        }

        /// <summary>
        /// Load Walmart item details
        /// </summary>
        protected override void LoadStoreItemDetails(IStoreEntity store, OrderItemEntity itemEntity, WarehouseOrderItem warehouseItem)
        {
            var walmartItemEntity = (WalmartOrderItemEntity) itemEntity;
            var walmartWarehouseItem = warehouseItem.AdditionalData[walmartEntryKey].ToObject<WalmartWarehouseItem>();

            walmartItemEntity.LineNumber = walmartWarehouseItem.LineNumber;
            walmartItemEntity.OnlineStatus = walmartWarehouseItem.OnlineStatus;
        }

        /// <summary>
        /// Load any additional store-specific details
        /// </summary>
        protected override void LoadAdditionalDetails(IStoreEntity store, OrderEntity orderEntity, WarehouseOrder warehouseOrder)
        {
            foreach (WarehouseOrderItem item in warehouseOrder.Items)
            {
                var orderItem = orderEntity.OrderItems.FirstOrDefault(x => x.HubItemID == item.ID);

                if (orderItem == null)
                {
                    // This should never happen
                    continue;
                }

                var walmartOrderItem = (WalmartOrderItemEntity) orderItem;
                var walmartWarehouseItem = item.AdditionalData[walmartEntryKey].ToObject<WalmartWarehouseItem>();

                walmartOrderItem.OnlineStatus = walmartWarehouseItem.OnlineStatus;
            }
        }
    }
}
