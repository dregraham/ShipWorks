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

namespace ShipWorks.Stores.Platforms.BigCommerce.Warehouse
{
    /// <summary>
    /// BigCommerce warehouse order factory
    /// </summary>
    [KeyedComponent(typeof(IWarehouseOrderFactory), StoreTypeCode.BigCommerce)]
    public class BigCommerceWarehouseOrderFactory : WarehouseOrderFactory
    {
        private const string bigCommerceEntryKey = "bigCommerce";
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public BigCommerceWarehouseOrderFactory(IOrderElementFactory orderElementFactory,
                                                   Func<Type, ILog> logFactory) : base(orderElementFactory)
        {
            log = logFactory(typeof(BigCommerceWarehouseOrderFactory));
        }

        /// <summary>
        /// Create an order entity with a BigCommerce identifier
        /// </summary>
        protected override async Task<GenericResult<OrderEntity>> CreateStoreOrderEntity(IStoreEntity store, StoreType storeType, WarehouseOrder warehouseOrder)
        {
            long bigCommerceOrderID = long.Parse(warehouseOrder.OrderNumber);

            // get the order instance
            GenericResult<OrderEntity> result = await orderElementFactory
                .CreateOrder(new OrderNumberIdentifier(bigCommerceOrderID)).ConfigureAwait(false);

            if (result.Failure)
            {
                log.InfoFormat("Skipping order '{0}': {1}.", bigCommerceOrderID, result.Message);
            }

            return result;
        }

        /// <summary>
        /// Load BigCommerce order details
        /// </summary>
        protected override void LoadStoreOrderDetails(OrderEntity orderEntity, WarehouseOrder warehouseOrder)
        {
            // BigCommerce doesn't have its own Order table.
        }

        /// <summary>
        /// Load BigCommerce item details
        /// </summary>
        protected override void LoadStoreItemDetails(OrderItemEntity itemEntity, WarehouseOrderItem warehouseItem)
        {
            BigCommerceOrderItemEntity bigCommerceItemEntity = (BigCommerceOrderItemEntity) itemEntity;
            var bigCommerceWarehouseItem = warehouseItem.AdditionalData[bigCommerceEntryKey].ToObject<BigCommerceWarehouseItem>();

            bigCommerceItemEntity.OrderAddressID = bigCommerceWarehouseItem.OrderAddressID;
            bigCommerceItemEntity.OrderProductID = bigCommerceWarehouseItem.OrderProductID;
            bigCommerceItemEntity.ParentOrderProductID = bigCommerceWarehouseItem.ParentOrderProductID;
            bigCommerceItemEntity.IsDigitalItem = bigCommerceWarehouseItem.IsDigitalItem;
            bigCommerceItemEntity.EventDate = bigCommerceWarehouseItem.EventDate;
            bigCommerceItemEntity.EventName = bigCommerceWarehouseItem.EventName;
        }
    }
}
