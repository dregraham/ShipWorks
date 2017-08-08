﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content.CombineOrderActions;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.Content
{
    /// <summary>
    /// Combination action that is specific to ChannelAdvisor
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificCombineOrderAction), StoreTypeCode.ChannelAdvisor)]
    public class ChannelAdvisorCombineOrderAction : IStoreSpecificCombineOrderAction
    {
        /// <summary>
        /// Perform the platform specific action
        /// </summary>
        public Task Perform(OrderEntity combinedOrder, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            var recordCreator = new SearchRecordMerger<IChannelAdvisorOrderEntity>(combinedOrder, orders, sqlAdapter);

            return recordCreator.Perform(ChannelAdvisorOrderSearchFields.OrderID,
                x => new ChannelAdvisorOrderSearchEntity
                {
                    OrderID = combinedOrder.OrderID,
                    CustomOrderIdentifier = x.CustomOrderIdentifier,
                    OriginalOrderID = x.OrderID
                });
        }
    }
}