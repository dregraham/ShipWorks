using System;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Import;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content;
using ShipWorks.Warehouse.Orders;
using ShipWorks.Warehouse.Orders.DTO;

namespace ShipWorks.Stores.Platforms.Volusion.Warehouse
{
    /// <summary>
    /// Volusion warehouse order factory
    /// </summary>
    [KeyedComponent(typeof(IWarehouseOrderFactory), StoreTypeCode.Volusion)]
    public class VolusionWarehouseOrderFactory : WarehouseOrderFactory
    {
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public VolusionWarehouseOrderFactory(IOrderElementFactory orderElementFactory,
            Func<Type, ILog> logFactory) : base(orderElementFactory)
        {
            log = logFactory(typeof(VolusionWarehouseOrderFactory));
        }

        /// <summary>
        /// Create an order entity with a Volusion identifier
        /// </summary>
        protected override async Task<GenericResult<OrderEntity>> CreateStoreOrderEntity(IStoreEntity store, StoreType storeType, WarehouseOrder warehouseOrder)
        {
            long orderNumber = long.Parse(warehouseOrder.OrderNumber);

            // get the order instance
            GenericResult<OrderEntity> result = await orderElementFactory
                .CreateOrder(new OrderNumberIdentifier(orderNumber)).ConfigureAwait(false);

            return result.OnFailure(x => log.InfoFormat("Skipping order '{0}': {1}.", warehouseOrder.OrderNumber, x.Message));
        }

        protected override void LoadOrderData(WarehouseOrder warehouseOrder, OrderEntity orderEntity)
        {
            base.LoadOrderData(warehouseOrder, orderEntity);
            if (storeEntity != null)
            {
                var shippingMethods = new VolusionShippingMethods((VolusionStoreEntity) storeEntity);
                if (int.TryParse(orderEntity.RequestedShipping, out int id))
                {
                    orderEntity.RequestedShipping = shippingMethods.GetShippingMethods(id);
                }
            }
        }
    }
}
