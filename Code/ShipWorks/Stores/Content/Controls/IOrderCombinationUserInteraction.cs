using System;
using System.Collections.Generic;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Content.Controls
{
    /// <summary>
    /// Wraps user interaction required for the order combination process
    /// </summary>
    public interface IOrderCombinationUserInteraction
    {
        /// <summary>
        /// Get order combination details from user
        /// </summary>
        GenericResult<Tuple<long, string>> GetCombinationDetailsFromUser(IEnumerable<IOrderEntity> orders);

        /// <summary>
        /// Show a success confirmation
        /// </summary>
        void ShowSuccessConfirmation(string orderNumber, IEnumerable<IOrderEntity> orders);
    }
}
