using System;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Import;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Warehouse;
using ShipWorks.Warehouse.DTO.Orders;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Factory for creating Odbc orders from warehouse orders
    /// </summary>
    [KeyedComponent(typeof(IWarehouseOrderFactory), StoreTypeCode.Odbc)]
    public class OdbcWarehouseOrderFactory : WarehouseOrderFactory
    {
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcWarehouseOrderFactory(IOrderElementFactory orderElementFactory, Func<Type, ILog> logFactory) :
            base(orderElementFactory)
        {
            log = logFactory(typeof(OdbcWarehouseOrderFactory));
        }

        /// <summary>
        /// Create an order entity with the store specific identifier
        /// </summary>
        protected override Task<GenericResult<OrderEntity>> CreateStoreOrderEntity(IStoreEntity store, StoreType storeType, WarehouseOrder warehouseOrder)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Load store specific order details
        /// </summary>
        protected override void LoadStoreItemDetails(IStoreEntity store, OrderItemEntity itemEntity, WarehouseOrderItem warehouseItem)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Load store specific item details
        /// </summary>
        protected override void LoadStoreOrderDetails(OrderEntity orderEntity, WarehouseOrder warehouseOrder)
        {
            throw new NotImplementedException();
        }
    }
}
