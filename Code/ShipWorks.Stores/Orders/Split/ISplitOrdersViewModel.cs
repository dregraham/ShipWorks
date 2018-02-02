using System;
using System.Collections.Generic;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Orders.Split
{
    /// <summary>
    /// View Model for the split orders dialog
    /// </summary>
    public interface ISplitOrdersViewModel
    {
        /// <summary>
        /// Get order split details from user
        /// </summary>
        GenericResult<SplitOrderDefinition> GetSplitDetailsFromUser(IOrderEntity order);
    }
}
