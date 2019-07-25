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

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.Warehouse
{
    /// <summary>
    /// ChannelAdvisor warehouse order factory
    /// </summary>
    [KeyedComponent(typeof(IWarehouseOrderFactory), StoreTypeCode.ChannelAdvisor)]
    public class ChannelAdvisorWarehouseOrderFactory : WarehouseOrderFactory
    {
        private const string channelAdvisorEntryKey = "channelAdvisor";
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public ChannelAdvisorWarehouseOrderFactory(IOrderElementFactory orderElementFactory,
                                                   Func<Type, ILog> logFactory) : base(orderElementFactory)
        {
            log = logFactory(typeof(ChannelAdvisorWarehouseOrderFactory));
        }

        /// <summary>
        /// Create an order entity with a ChannelAdvisor identifier
        /// </summary>
        protected override async Task<GenericResult<OrderEntity>> CreateStoreOrderEntity(IStoreEntity store, StoreType storeType, WarehouseOrder warehouseOrder)
        {
            long channelAdvisorOrderID = long.Parse(warehouseOrder.OrderNumber);

            // get the order instance
            GenericResult<OrderEntity> result = await orderElementFactory
                .CreateOrder(new OrderNumberIdentifier(channelAdvisorOrderID)).ConfigureAwait(false);

            if (result.Failure)
            {
                log.InfoFormat("Skipping order '{0}': {1}.", channelAdvisorOrderID, result.Message);
            }

            return result;
        }

        /// <summary>
        /// Load ChannelAdvisor order details
        /// </summary>
        protected override void LoadStoreOrderDetails(OrderEntity orderEntity, WarehouseOrder warehouseOrder)
        {
            ChannelAdvisorOrderEntity channelAdvisorOrderEntity = (ChannelAdvisorOrderEntity) orderEntity;
            var channelAdvisorWarehouseOrder = warehouseOrder.AdditionalData[channelAdvisorEntryKey].ToObject<ChannelAdvisorWarehouseOrder>();

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
        protected override void LoadStoreItemDetails(IStoreEntity store, OrderItemEntity itemEntity, WarehouseOrderItem warehouseItem)
        {
            ChannelAdvisorOrderItemEntity channelAdvisorItemEntity = (ChannelAdvisorOrderItemEntity) itemEntity;
            var channelAdvisorWarehouseItem = warehouseItem.AdditionalData[channelAdvisorEntryKey].ToObject<ChannelAdvisorWarehouseItem>();

            channelAdvisorItemEntity.MarketplaceName = channelAdvisorWarehouseItem.MarketplaceName;
            channelAdvisorItemEntity.MarketplaceStoreName = channelAdvisorWarehouseItem.MarketplaceStoreName;
            channelAdvisorItemEntity.MarketplaceBuyerID = channelAdvisorWarehouseItem.MarketplaceBuyerId;
            channelAdvisorItemEntity.MarketplaceSalesID = channelAdvisorWarehouseItem.MarketplaceSalesId;
            channelAdvisorItemEntity.Classification = channelAdvisorWarehouseItem.Classification;
            channelAdvisorItemEntity.DistributionCenter = channelAdvisorWarehouseItem.DistributionCenter;
            channelAdvisorItemEntity.IsFBA = channelAdvisorWarehouseItem.IsFBA;
            channelAdvisorItemEntity.DistributionCenterID = channelAdvisorWarehouseItem.DistributionCenterId;
            channelAdvisorItemEntity.DistributionCenterName = channelAdvisorWarehouseItem.DistributionCenterName;
        }
    }
}
