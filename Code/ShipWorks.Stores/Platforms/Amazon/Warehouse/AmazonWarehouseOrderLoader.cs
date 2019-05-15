using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Import;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Warehouse;
using ShipWorks.Warehouse.DTO.Orders;

namespace ShipWorks.Stores.Platforms.Amazon.Warehouse
{
    /// <summary>
    /// Amazon specific warehouse order loader
    /// </summary>
    [KeyedComponent(typeof(IWarehouseOrderLoader), StoreTypeCode.Amazon)]
    public class AmazonWarehouseOrderLoader : WarehouseOrderLoader
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonWarehouseOrderLoader(IOrderElementFactory orderElementFactory) : 
            base(orderElementFactory)
        {
        }

        /// <summary>
        /// Create an order entity with an Amazon identifier
        /// </summary>
        protected override async Task<GenericResult<OrderEntity>> CreateOrderEntity(WarehouseOrder warehouseOrder)
        {
            AmazonWarehouseOrder amazonWarehouseOrder = (AmazonWarehouseOrder) warehouseOrder;

            string amazonOrderID = amazonWarehouseOrder.AmazonOrderID;

            // get the order instance
            GenericResult<OrderEntity> result = await orderElementFactory
                .CreateOrder(new AmazonOrderIdentifier(amazonOrderID)).ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Load Amazon order details
        /// </summary>
        protected override void LoadStoreSpecificOrderDetails(OrderEntity orderEntity, WarehouseOrder warehouseOrder)
        {
            AmazonOrderEntity amazonOrderEntity = (AmazonOrderEntity) orderEntity;
            AmazonWarehouseOrder amazonWarehouseOrder = (AmazonWarehouseOrder) warehouseOrder;
            
            amazonOrderEntity.AmazonOrderID = amazonWarehouseOrder.AmazonOrderID;
            amazonOrderEntity.FulfillmentChannel = amazonWarehouseOrder.FulfillmentChannel;
            amazonOrderEntity.IsPrime = amazonWarehouseOrder.IsPrime;
            amazonOrderEntity.EarliestExpectedDeliveryDate = amazonWarehouseOrder.EarliestExpectedDeliveryDate;
            amazonOrderEntity.LatestExpectedDeliveryDate = amazonWarehouseOrder.LatestExpectedDeliveryDate;
            amazonOrderEntity.PurchaseOrderNumber = amazonWarehouseOrder.PurchaseOrderNumber;        
        }

        /// <summary>
        /// Load Amazon item details
        /// </summary>
        protected override void LoadStoreSpecificItemDetails(OrderItemEntity itemEntity, WarehouseOrderItem warehouseItem)
        {
            AmazonOrderItemEntity amazonItemEntity = (AmazonOrderItemEntity) itemEntity;
            AmazonWarehouseItem amazonWarehouseItem = (AmazonWarehouseItem) warehouseItem;
            
            amazonItemEntity.AmazonOrderItemCode = amazonWarehouseItem.AmazonOrderItemCode;
            amazonItemEntity.ASIN = amazonWarehouseItem.ASIN;
            amazonItemEntity.ConditionNote = amazonWarehouseItem.ConditionNote;
        }
    }
}
