using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Import;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Warehouse;
using ShipWorks.Warehouse.DTO.Orders;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.Warehouse
{
    /// <summary>
    /// ChannelAdvisor specific warehouse order loader
    /// </summary>
    [KeyedComponent(typeof(IWarehouseOrderLoader), StoreTypeCode.ChannelAdvisor)]
    public class ChannelAdvisorWarehouseOrderLoader : WarehouseOrderLoader
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelAdvisorWarehouseOrderLoader(IOrderElementFactory orderElementFactory) : base(orderElementFactory)
        {
        }

        /// <summary>
        /// Create an order entity with a ChannelAdvisor identifier
        /// </summary>
        protected override async Task<GenericResult<OrderEntity>> CreateOrderEntity(WarehouseOrder warehouseOrder)
        {
            ChannelAdvisorWarehouseOrder channelAdvisorWarehouseOrder = (ChannelAdvisorWarehouseOrder) warehouseOrder;

            long channelAdvisorOrderID = long.Parse(channelAdvisorWarehouseOrder.OrderNumber);

            // get the order instance
            GenericResult<OrderEntity> result = await orderElementFactory
                .CreateOrder(new OrderNumberIdentifier(channelAdvisorOrderID)).ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Load ChannelAdvisor order details
        /// </summary>
        protected override void LoadStoreSpecificOrderDetails(OrderEntity orderEntity, WarehouseOrder warehouseOrder)
        {
            ChannelAdvisorOrderEntity channelAdvisorOrderEntity = (ChannelAdvisorOrderEntity) orderEntity;
            ChannelAdvisorWarehouseOrder channelAdvisorWarehouseOrder = (ChannelAdvisorWarehouseOrder) warehouseOrder;
            
            channelAdvisorOrderEntity.CustomOrderIdentifier = channelAdvisorWarehouseOrder.CustomOrderIdentifier;
            channelAdvisorOrderEntity.ResellerID = channelAdvisorWarehouseOrder.ResellerID;
            channelAdvisorOrderEntity.OnlineShippingStatus = channelAdvisorWarehouseOrder.OnlineShippingStatus;
            channelAdvisorOrderEntity.OnlineCheckoutStatus = channelAdvisorWarehouseOrder.OnlineCheckoutStatus;
            channelAdvisorOrderEntity.OnlinePaymentStatus = channelAdvisorWarehouseOrder.OnlinePaymentStatus;
            channelAdvisorOrderEntity.FlagStyle = channelAdvisorWarehouseOrder.FlagStyle;
            channelAdvisorOrderEntity.FlagDescription = channelAdvisorWarehouseOrder.FlagDescription;
            channelAdvisorOrderEntity.FlagType = channelAdvisorWarehouseOrder.FlagType;
            channelAdvisorOrderEntity.MarketplaceNames = channelAdvisorWarehouseOrder.MarketplaceNames;
            channelAdvisorOrderEntity.IsPrime = channelAdvisorWarehouseOrder.IsPrime;        
        }

        /// <summary>
        /// Load ChannelAdvisor item details
        /// </summary>
        protected override void LoadStoreSpecificItemDetails(OrderItemEntity itemEntity, WarehouseOrderItem warehouseItem)
        {
            ChannelAdvisorOrderItemEntity channelAdvisorItemEntity = (ChannelAdvisorOrderItemEntity) itemEntity;
            ChannelAdvisorWarehouseItem channelAdvisorWarehouseItem = (ChannelAdvisorWarehouseItem) warehouseItem;
            
            channelAdvisorItemEntity.MarketplaceName = channelAdvisorWarehouseItem.MarketplaceName;
            channelAdvisorItemEntity.MarketplaceStoreName = channelAdvisorWarehouseItem.MarketplaceStoreName;
            channelAdvisorItemEntity.MarketplaceBuyerID = channelAdvisorWarehouseItem.MarketplaceBuyerID;
            channelAdvisorItemEntity.MarketplaceSalesID = channelAdvisorWarehouseItem.MarketplaceSalesID;
            channelAdvisorItemEntity.Classification = channelAdvisorWarehouseItem.Classification;
            channelAdvisorItemEntity.DistributionCenter = channelAdvisorWarehouseItem.DistributionCenter;
            channelAdvisorItemEntity.IsFBA = channelAdvisorWarehouseItem.IsFBA;
            channelAdvisorItemEntity.DistributionCenterID = channelAdvisorWarehouseItem.DistributionCenterID;
            channelAdvisorItemEntity.DistributionCenterName = channelAdvisorWarehouseItem.DistributionCenterName;
        }
    }
}
