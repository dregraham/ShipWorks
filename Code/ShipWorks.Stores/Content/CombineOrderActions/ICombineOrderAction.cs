using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Content.OrderCombinerActions
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
        Task Perform(OrderEntity combinedOrder, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter);
    }
}