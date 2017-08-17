﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content.CombineOrderActions;

namespace ShipWorks.Stores.Platforms.NetworkSolutionss.Content
{
    /// <summary>
    /// Combination action that is specific to NetworkSolutions
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificCombineOrderAction), StoreTypeCode.NetworkSolutions)]
    public class NetworkSolutionssCombinerAction : IStoreSpecificCombineOrderAction
    {
        /// <summary>
        /// Perform the platform specific action
        /// </summary>
        public Task Perform(OrderEntity combinedOrder, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            IEnumerable<NetworkSolutionsOrderSearchEntity> orderSearches = orders.Cast<INetworkSolutionsOrderEntity>()
                .Select(x => new NetworkSolutionsOrderSearchEntity
                {
                    OrderID = combinedOrder.OrderID,
                    NetworkSolutionsOrderID = x.NetworkSolutionsOrderID,
                    OriginalOrderID = x.OrderID
                });

            return sqlAdapter.SaveEntityCollectionAsync(orderSearches.ToEntityCollection());
        }
    }
}
