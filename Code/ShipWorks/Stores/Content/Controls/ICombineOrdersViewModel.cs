using System;
using System.Collections.Generic;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Core.Stores.Content
{
    /// <summary>
    /// View Model for the combine orders dialog
    /// </summary>
    public interface ICombineOrdersViewModel
    {
        /// <summary>
        /// Get order combination details from user
        /// </summary>
        GenericResult<Tuple<long, string>> GetCombinationDetailsFromUser(IEnumerable<IOrderEntity> orders);
    }
}
