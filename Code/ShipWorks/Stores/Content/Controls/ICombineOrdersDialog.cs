using System;
using System.Collections.Generic;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Content.Controls
{
    /// <summary>
    /// Get order combination details from user
    /// </summary>
    public interface ICombineOrdersDialog : IDialog
    {
        /// <summary>
        /// Get order combination details from user
        /// </summary>
        GenericResult<Tuple<long, string>> GetCombinationDetailsFromUser(IEnumerable<IOrderEntity> orders);
    }
}
