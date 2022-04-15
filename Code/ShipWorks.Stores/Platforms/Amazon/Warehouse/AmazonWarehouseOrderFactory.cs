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

namespace ShipWorks.Stores.Platforms.Amazon.Warehouse
{
    /// <summary>
    /// Amazon warehouse order factory
    /// </summary>
    [KeyedComponent(typeof(IWarehouseOrderFactory), StoreTypeCode.Amazon)]
    public class AmazonWarehouseOrderFactory : WarehouseOrderFactory
    {
        private const string amazonEntryKey = "amazon";
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonWarehouseOrderFactory(IOrderElementFactory orderElementFactory, Func<Type, ILog> logFactory) :
            base(orderElementFactory)
        {
            log = logFactory(typeof(AmazonWarehouseOrderFactory));
        }

        /// <summary>
        /// Create an order entity with an Amazon identifier
        /// </summary>
        protected override async Task<GenericResult<OrderEntity>> CreateStoreOrderEntity(IStoreEntity store, StoreType storeType, WarehouseOrder warehouseOrder)
        {
            string amazonOrderID = warehouseOrder.AdditionalData[amazonEntryKey].ToObject<AmazonWarehouseOrder>().AmazonOrderID;

            // get the order instance
            GenericResult<OrderEntity> result = await orderElementFactory
                .CreateOrder(new AmazonOrderIdentifier(amazonOrderID)).ConfigureAwait(false);

            if (result.Success)
            {
                result.Value.ChangeOrderNumber(amazonOrderID);
            } 
            else
            {
                log.InfoFormat("Skipping order '{0}': {1}.", amazonOrderID, result.Message);
            }

            return result;
        }

        /// <summary>
        /// Load Amazon order details
        /// </summary>
        protected override void LoadStoreOrderDetails(IStoreEntity store, OrderEntity orderEntity, WarehouseOrder warehouseOrder)
        {
            AmazonOrderEntity amazonOrderEntity = (AmazonOrderEntity) orderEntity;
            AmazonWarehouseOrder amazonWarehouseOrder = warehouseOrder.AdditionalData[amazonEntryKey].ToObject<AmazonWarehouseOrder>();

            amazonOrderEntity.AmazonOrderID = amazonWarehouseOrder.AmazonOrderID;
            amazonOrderEntity.FulfillmentChannel = amazonWarehouseOrder.FulfillmentChannel;
            amazonOrderEntity.IsPrime = amazonWarehouseOrder.IsPrime;
            amazonOrderEntity.EarliestExpectedDeliveryDate = amazonWarehouseOrder.EarliestExpectedDeliveryDate;
            amazonOrderEntity.LatestExpectedDeliveryDate = amazonWarehouseOrder.LatestExpectedDeliveryDate;
            amazonOrderEntity.PurchaseOrderNumber = amazonWarehouseOrder.PurchaseOrderNumber;
            amazonOrderEntity.ChannelOrderID = warehouseOrder.ShipEngineSalesOrderId;
        }

        /// <summary>
        /// Load Amazon item details
        /// </summary>
        protected override void LoadStoreItemDetails(IStoreEntity store, OrderItemEntity itemEntity, WarehouseOrderItem warehouseItem)
        {
            AmazonOrderItemEntity amazonItemEntity = (AmazonOrderItemEntity) itemEntity;
            AmazonWarehouseItem amazonWarehouseItem = warehouseItem.AdditionalData[amazonEntryKey].ToObject<AmazonWarehouseItem>();

            amazonItemEntity.AmazonOrderItemCode = amazonWarehouseItem.AmazonOrderItemCode;
            amazonItemEntity.ASIN = amazonWarehouseItem.ASIN;
            amazonItemEntity.ConditionNote = amazonWarehouseItem.ConditionNote;
        }
    }
}
