using System.Collections.Generic;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Orders.Combine
{
    /// <summary>
    /// View model for the combination success dialog
    /// </summary>
    public interface ICombineOrderSuccessViewModel
    {
        /// <summary>
        /// Show the combination successful dialog
        /// </summary>
        void ShowSuccessConfirmation(string orderNumber, IEnumerable<IOrderEntity> orders);
    }
}