using System;
using System.Collections.Generic;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Orders.Split
{
    /// <summary>
    /// View Model for the combine orders dialog
    /// </summary>
    public interface ISplitOrdersViewModel
    {
        /// <summary>
        /// Get order combination details from user
        /// </summary>
        GenericResult<Tuple<long, string>> GetSplitDetailsFromUser(IEnumerable<IOrderEntity> orders);
    }
}
