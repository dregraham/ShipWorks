using System;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Import;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content;
using ShipWorks.Warehouse;
using ShipWorks.Warehouse.DTO.Orders;

namespace ShipWorks.Stores.Platforms.Overstock.Warehouse
{
    /// <summary>
    /// Overstock warehouse order factory
    /// </summary>
    [KeyedComponent(typeof(IWarehouseOrderFactory), StoreTypeCode.Overstock)]
    public class OverstockWarehouseOrderFactory : WarehouseOrderFactory
    {
        private const string OverstockEntryKey = "overstock";
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public OverstockWarehouseOrderFactory(IOrderElementFactory orderElementFactory,
                                                   Func<Type, ILog> logFactory) : base(orderElementFactory)
        {
            log = logFactory(typeof(OverstockWarehouseOrderFactory));
        }

        /// <summary>
        /// Create an order entity with a Overstock identifier
        /// </summary>
        protected override async Task<GenericResult<OrderEntity>> CreateStoreOrderEntity(IStoreEntity store, StoreType storeType, WarehouseOrder warehouseOrder)
        {
            // get the order instance
            GenericResult<OrderEntity> result = await orderElementFactory
                .CreateOrder(new AlphaNumericOrderIdentifier(warehouseOrder.OrderNumber)).ConfigureAwait(false);

            if (result.Failure)
            {
                log.InfoFormat("Skipping order '{0}': {1}.", warehouseOrder.OrderNumber, result.Message);
            }

            return result;
        }

        /// <summary>
        /// Load Overstock order details
        /// </summary>
        protected override void LoadStoreOrderDetails(IStoreEntity store, OrderEntity orderEntity, WarehouseOrder warehouseOrder)
        {
            OverstockOrderEntity OverstockOrderEntity = (OverstockOrderEntity) orderEntity;
            var OverstockWarehouseOrder = warehouseOrder.AdditionalData[OverstockEntryKey].ToObject<OverstockWarehouseOrder>();

            OverstockOrderEntity.SalesChannelName = OverstockWarehouseOrder.SalesChannelName;
            OverstockOrderEntity.WarehouseCode = OverstockWarehouseOrder.WarehouseCode;
            OverstockOrderEntity.SofsCreatedDate = OverstockWarehouseOrder.SofsCreatedDate;
        }

        /// <summary>
        /// Load Overstock item details
        /// </summary>
        protected override void LoadStoreItemDetails(IStoreEntity store, OrderItemEntity itemEntity, WarehouseOrderItem warehouseItem)
        {
            OverstockOrderItemEntity OverstockItemEntity = (OverstockOrderItemEntity) itemEntity;
            var OverstockWarehouseItem = warehouseItem.AdditionalData[OverstockEntryKey].ToObject<OverstockWarehouseItem>();

            OverstockItemEntity.SalesChannelLineNumber = OverstockWarehouseItem.SalesChannelLineNumber;
        }
    }
}
