﻿using System;
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

namespace ShipWorks.Stores.Platforms.Platform
{
    /// <summary>
    /// API Warehouse Order Factory
    /// </summary>
    [KeyedComponent(typeof(IWarehouseOrderFactory), StoreTypeCode.Api)]
    [KeyedComponent(typeof(IWarehouseOrderFactory), StoreTypeCode.BrightpearlHub)]
    [KeyedComponent(typeof(IWarehouseOrderFactory), StoreTypeCode.WalmartHub)]
    [KeyedComponent(typeof(IWarehouseOrderFactory), StoreTypeCode.ChannelAdvisorHub)]
    [KeyedComponent(typeof(IWarehouseOrderFactory), StoreTypeCode.VolusionHub)]
    [KeyedComponent(typeof(IWarehouseOrderFactory), StoreTypeCode.GrouponHub)]
    public class PlatformWarehouseOrderFactory : WarehouseOrderFactory
    {
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public PlatformWarehouseOrderFactory(IOrderElementFactory orderElementFactory,
            Func<Type, ILog> logFactory) : base(orderElementFactory)
        {
            log = logFactory(typeof(PlatformWarehouseOrderFactory));
        }

        /// <summary>
        /// Create an order entity with a Volusion identifier
        /// </summary>
        protected override async Task<GenericResult<OrderEntity>> CreateStoreOrderEntity(IStoreEntity store, StoreType storeType, WarehouseOrder warehouseOrder)
        {
            // get the order instance
            GenericResult<OrderEntity> result = await orderElementFactory
                .CreateOrder(new AlphaNumericOrderIdentifier(warehouseOrder.OrderNumber)).ConfigureAwait(false);
            
            return result.OnFailure(x => log.InfoFormat("Skipping order '{0}': {1}.", warehouseOrder.OrderNumber, x.Message));
        }

        /// <summary>
        /// Load Api order details
        /// </summary>
        protected override void LoadStoreOrderDetails(IStoreEntity store, OrderEntity orderEntity, WarehouseOrder warehouseOrder)
        {
            orderEntity.ChannelOrderID = warehouseOrder.ShipEngineSalesOrderId;
        }
    }
}
