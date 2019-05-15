using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Import;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
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
        /// Load a ChannelAdvisor warehouse order into a ChannelAdvisor order entity
        /// </summary>
        /// <exception cref="DownloadException">Throws download exception when given non ChannelAdvisor order</exception>
        public override void LoadOrder(OrderEntity orderEntity, WarehouseOrder warehouseOrder)
        {
            base.LoadOrder(orderEntity, warehouseOrder);

            if (orderEntity is ChannelAdvisorOrderEntity channelAdvisorOrderEntity &&
                warehouseOrder is ChannelAdvisorWarehouseOrder channelAdvisorWarehouseOrder)
            {
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
            else
            {
                throw new DownloadException(
                    $"Failed to load warehouse order with order number {warehouseOrder.OrderNumber}");
            }
        }

        /// <summary>
        /// Load a ChannelAdvisor warehouse order item into a ChannelAdvisor order item entity
        /// </summary>
        /// <exception cref="DownloadException">Throws download exception when given non ChannelAdvisor items</exception>
        protected override void LoadItem(OrderItemEntity itemEntity, WarehouseOrderItem warehouseItem)
        {
            base.LoadItem(itemEntity, warehouseItem);

            if (itemEntity is ChannelAdvisorOrderItemEntity channelAdvisorItemEntity &&
                warehouseItem is ChannelAdvisorWarehouseItem channelAdvisorWarehouseItem)
            {
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
            else
            {
                throw new DownloadException(
                    $"Failed to load items for warehouse order with order number {itemEntity.Order.OrderNumberComplete}");
            }
        }
    }
}
