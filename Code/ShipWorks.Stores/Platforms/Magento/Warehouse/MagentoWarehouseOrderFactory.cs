using System;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Import;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Stores.Platforms.GenericModule.Warehouse;
using ShipWorks.Warehouse;
using ShipWorks.Warehouse.DTO.Orders;

namespace ShipWorks.Stores.Platforms.Magento.Warehouse
{
    /// <summary>
    /// Magento warehouse order factory
    /// </summary>
    [KeyedComponent(typeof(IWarehouseOrderFactory), StoreTypeCode.Magento)]
    public class MagentoWarehouseOrderFactory : GenericModuleWarehouseOrderFactory
    {
        private const string MagentoEntryKey = "Magento";
        private readonly ILog log;

        //private readonly MagentoDownloader magentoDownloader;

        /// <summary>
        /// Constructor
        /// </summary>
        public MagentoWarehouseOrderFactory(IOrderElementFactory orderElementFactory, Func<Type, ILog> logFactory) :
            base(orderElementFactory, logFactory)
        {
            log = logFactory(typeof(MagentoWarehouseOrderFactory));
        }

        /// <summary>
        /// Create an order entity with an Magento identifier
        /// </summary>
        protected override async Task<GenericResult<OrderEntity>> CreateStoreOrderEntity(IStoreEntity store, StoreType storeType, WarehouseOrder warehouseOrder)
        {
            var genericStore = MethodConditions.EnsureArgumentIsNotNull(store as IGenericModuleStoreEntity, nameof(store));
            var genericStoreType = MethodConditions.EnsureArgumentIsNotNull(storeType as GenericModuleStoreType, nameof(storeType));

            // get the order instance
            var identifier = GenericModuleDownloader.CreateOrderIdentifier(
                (GenericStoreDownloadStrategy) genericStore.ModuleDownloadStrategy,
                genericStoreType,
                warehouseOrder.OrderNumber,
                warehouseOrder.OrderNumberPrefix,
                warehouseOrder.OrderNumberPostfix);

            var result = await orderElementFactory.CreateOrder(identifier).ConfigureAwait(false);

            if (result.Failure)
            {
                log.InfoFormat("Skipping order '{0}': {1}.", warehouseOrder.OrderNumber, result.Message);
            }

            return result;
        }

        /// <summary>
        /// Load store specific order details
        /// </summary>
        protected override void LoadStoreOrderDetails(OrderEntity orderEntity, WarehouseOrder warehouseOrder)
        {
            base.LoadStoreOrderDetails(orderEntity, warehouseOrder);

            var magentoOrderEntity = (MagentoOrderEntity) orderEntity;
            var magentoWarehouseOrder = warehouseOrder.AdditionalData[MagentoEntryKey].ToObject<MagentoWarehouseOrder>();

            magentoOrderEntity.MagentoOrderID = magentoWarehouseOrder.MagentoOrderID;
        }
    }
}
