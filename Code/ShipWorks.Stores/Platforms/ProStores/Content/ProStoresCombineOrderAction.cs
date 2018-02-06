﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Orders.Combine.Actions;

namespace ShipWorks.Stores.Platforms.ProStores.Content
{
    /// <summary>
    /// Combination action that is specific to ProStores
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificCombineOrderAction), StoreTypeCode.ProStores)]
    public class ProStoresCombineOrderAction : IStoreSpecificCombineOrderAction
    {
        /// <summary>
        /// Perform the platform specific action
        /// </summary>
        public Task Perform(OrderEntity combinedOrder, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            var recordCreator = new SearchRecordMerger<IProStoresOrderEntity>(combinedOrder, orders, sqlAdapter);

            return recordCreator.Perform(ProStoresOrderSearchFields.OrderID,
                x => new ProStoresOrderSearchEntity
                {
                    OrderID = combinedOrder.OrderID,
                    ConfirmationNumber = x.ConfirmationNumber,
                    OriginalOrderID = x.OrderID
                });
        }
    }
}