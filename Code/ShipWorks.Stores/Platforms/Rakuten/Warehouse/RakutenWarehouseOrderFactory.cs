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

namespace ShipWorks.Stores.Platforms.Rakuten.Warehouse
{
    /// <summary>
    /// Rakuten warehouse order factory
    /// </summary>
    [KeyedComponent(typeof(IWarehouseOrderFactory), StoreTypeCode.Rakuten)]
    public class RakutenWarehouseOrderFactory : WarehouseOrderFactory
    {
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public RakutenWarehouseOrderFactory(IOrderElementFactory orderElementFactory,
                                                   Func<Type, ILog> logFactory) : base(orderElementFactory)
        {
            log = logFactory(typeof(RakutenWarehouseOrderFactory));
        }

        /// <summary>
        /// Create an order entity with a Rakuten identifier
        /// </summary>
        protected override async Task<GenericResult<OrderEntity>> CreateStoreOrderEntity(IStoreEntity store, StoreType storeType, WarehouseOrder warehouseOrder)
        {
            long RakutenOrderID = long.Parse(warehouseOrder.OrderNumber);

            // get the order instance
            GenericResult<OrderEntity> result = await orderElementFactory
                .CreateOrder(new OrderNumberIdentifier(RakutenOrderID)).ConfigureAwait(false);

            if (result.Failure)
            {
                log.InfoFormat("Skipping order '{0}': {1}.", RakutenOrderID, result.Message);
            }

            return result;
        }
    }
}
