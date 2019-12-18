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
    /// Shopify warehouse order factory
    /// </summary>
    [KeyedComponent(typeof(IWarehouseOrderFactory), StoreTypeCode.Rakuten)]
    public class RakutenWarehouseOrderFactory : WarehouseOrderFactory
    {
        private const string rakutenEntryKey = "rakuten";
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
        /// Create an order entity from the Rakuten order number
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
        /// Load Rakuten order details
        /// </summary>
        protected override void LoadStoreOrderDetails(IStoreEntity store, OrderEntity orderEntity, WarehouseOrder warehouseOrder)
        {
            RakutenOrderEntity rakutenOrderEntity = (RakutenOrderEntity) orderEntity;
            var rakutenWarehouseOrder = warehouseOrder.AdditionalData[rakutenEntryKey].ToObject<RakutenWarehouseOrder>();

            rakutenOrderEntity.ChangeOrderNumber(warehouseOrder.OrderNumber);
            rakutenOrderEntity.RakutenPackageID = rakutenWarehouseOrder.PackageID;
        }
    }
}
