using System;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Import;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Warehouse;
using ShipWorks.Warehouse.DTO.Orders;

namespace ShipWorks.Stores.Platforms.GenericModule.Warehouse
{
    /// <summary>
    /// Generic Module warehouse order factory
    /// </summary>
    [KeyedComponent(typeof(IWarehouseOrderFactory), StoreTypeCode.GenericModule)]
    [Component(RegistrationType.Self)]
    public class GenericModuleWarehouseOrderFactory : WarehouseOrderFactory
    {
        private const string genericModuleEntryKey = "genericModule";
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericModuleWarehouseOrderFactory(IOrderElementFactory orderElementFactory, Func<Type, ILog> logFactory) :
            base(orderElementFactory)
        {
            log = logFactory(typeof(GenericModuleWarehouseOrderFactory));
        }

        /// <summary>
        /// Create an order entity with an Amazon identifier
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
            var genericModuleOrderEntity = (GenericModuleOrderEntity) orderEntity;
            var genericModuleWarehouseOrder = warehouseOrder.AdditionalData[genericModuleEntryKey].ToObject<GenericModuleWarehouseOrder>();

            genericModuleOrderEntity.AmazonOrderID = genericModuleWarehouseOrder.AmazonOrderID;
            genericModuleOrderEntity.IsFBA = genericModuleWarehouseOrder.IsFBA;
            genericModuleOrderEntity.IsPrime = genericModuleWarehouseOrder.IsPrime == 1 ? AmazonIsPrime.Yes : AmazonIsPrime.No;
            genericModuleOrderEntity.IsSameDay = genericModuleWarehouseOrder.IsSameDay;
        }

        /// <summary>
        /// Load store specific item details
        /// </summary>
        protected override void LoadStoreItemDetails(OrderItemEntity itemEntity, WarehouseOrderItem warehouseItem)
        {
            var genericModuleItemEntity = (GenericModuleOrderItemEntity) itemEntity;
            var genericModuleWarehouseItem = warehouseItem.AdditionalData[genericModuleEntryKey].ToObject<GenericcModuleWarehouseItem>();

            genericModuleItemEntity.AmazonOrderItemCode = genericModuleWarehouseItem.AmazonOrderItemCode;
        }
    }
}
