﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Orders.Combine.Actions
{
    /// <summary>
    /// Discreet action that is part of the combining process
    /// </summary>
    [Service]
    public interface ICombineOrderAction
    {
        /// <summary>
        /// Perform the action
        /// </summary>
        Task Perform(OrderEntity combinedOrder, long survivingOrderID, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter);
    }
}