﻿using System.Collections.Generic;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// Order gateway for combining orders
    /// </summary>
    public interface ICombineOrdersGateway
    {
        /// <summary>
        /// Load information needed for combining the orders
        /// </summary>
        /// <remarks>
        /// We should change the return type from IOrderEntity to an
        /// actual projection class, depending on our needs
        /// </remarks>
        GenericResult<IEnumerable<IOrderEntity>> LoadOrders(IEnumerable<long> orderIDs);
    }
}
