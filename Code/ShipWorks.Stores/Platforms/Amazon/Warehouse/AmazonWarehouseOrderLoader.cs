using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Import;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
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
        /// Load an Amazon warehouse order into an Amazon order entity
        /// </summary>
        /// <exception cref="DownloadException">Throws download exception when given non amazon order</exception>
        public override void LoadOrder(OrderEntity orderEntity, WarehouseOrder warehouseOrder)
        {
            base.LoadOrder(orderEntity, warehouseOrder);

            if (orderEntity is AmazonOrderEntity amazonOrderEntity &&
                warehouseOrder is AmazonWarehouseOrder amazonWarehouseOrder)
            {
                amazonOrderEntity.AmazonOrderID = amazonWarehouseOrder.AmazonOrderID;
                amazonOrderEntity.FulfillmentChannel = amazonWarehouseOrder.FulfillmentChannel;
                amazonOrderEntity.IsPrime = amazonWarehouseOrder.IsPrime;
                amazonOrderEntity.EarliestExpectedDeliveryDate = amazonWarehouseOrder.EarliestExpectedDeliveryDate;
                amazonOrderEntity.LatestExpectedDeliveryDate = amazonWarehouseOrder.LatestExpectedDeliveryDate;
                amazonOrderEntity.PurchaseOrderNumber = amazonWarehouseOrder.PurchaseOrderNumber;
            }
            else
            {
                throw new DownloadException(
                    $"Failed to load warehouse order with order number {warehouseOrder.OrderNumber}");
            }
        }

        /// <summary>
        /// Load an Amazon warehouse order item into an Amazon order item entity
        /// </summary>
        /// <exception cref="DownloadException">Throws download exception when given non amazon items</exception>
        protected override void LoadItem(OrderItemEntity itemEntity, WarehouseOrderItem warehouseItem)
        {
            base.LoadItem(itemEntity, warehouseItem);

            if (itemEntity is AmazonOrderItemEntity amazonItemEntity &&
                warehouseItem is AmazonWarehouseItem amazonWarehouseItem)
            {
                amazonItemEntity.AmazonOrderItemCode = amazonWarehouseItem.AmazonOrderItemCode;
                amazonItemEntity.ASIN = amazonWarehouseItem.ASIN;
                amazonItemEntity.ConditionNote = amazonWarehouseItem.ConditionNote;
            }
            else
            {
                throw new DownloadException(
                    $"Failed to load items for warehouse order with order number {itemEntity.Order.OrderNumberComplete}");
            }
        }
    }
}
