﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Orders.Combine.Actions;

namespace ShipWorks.Stores.Platforms.ThreeDCart.Content
{
    /// <summary>
    /// Combination action that is specific to ThreeDCart
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificCombineOrderAction), StoreTypeCode.ThreeDCart)]
    public class ThreeDCartCombineOrderAction : IStoreSpecificCombineOrderAction
    {
        /// <summary>
        /// Perform the platform specific action
        /// </summary>
        public Task Perform(OrderEntity combinedOrder, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            var recordCreator = new SearchRecordMerger<IThreeDCartOrderEntity>(combinedOrder, orders, sqlAdapter);

            return recordCreator.Perform(ThreeDCartOrderSearchFields.OrderID,
                x => new ThreeDCartOrderSearchEntity
                {
                    OrderID = combinedOrder.OrderID,
                    ThreeDCartOrderID = x.ThreeDCartOrderID,
                    OriginalOrderID = x.OrderID
                });
        }
    }
}