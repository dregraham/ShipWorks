﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content.OrderCombinerActions;

namespace ShipWorks.Stores.Platforms.Yahoo.Content
{
    /// <summary>
    /// Combination action that is specific to Yahoo
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificOrderCombinerAction), StoreTypeCode.Yahoo)]
    public class YahooOrderCombinerAction : IStoreSpecificOrderCombinerAction
    {
        /// <summary>
        /// Perform the platform specific action
        /// </summary>
        public Task Perform(OrderEntity combinedOrder, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            var recordCreator = new SearchRecordMerger<IYahooOrderEntity>(combinedOrder, orders, sqlAdapter);

            return recordCreator.Perform(YahooOrderSearchFields.OrderID,
                x => new YahooOrderSearchEntity
                {
                    OrderID = combinedOrder.OrderID,
                    YahooOrderID = x.YahooOrderID
                });
        }
    }
}