using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.OrderLookup.Controls.OrderConfirmationDialog
{
    /// <summary>
    /// Represents a view model for multiple matched orders to confirm
    /// </summary>
    public interface IOrderConfirmationViewModel
    {
        /// <summary>
        /// The Orders
        /// </summary>
        IEnumerable<OrderEntity> Orders { get; set; }

        /// <summary>
        /// The SelectedOrder
        /// </summary>
        OrderEntity SelectedOrder { get; set; }

        /// <summary>
        /// The text that was searched for
        /// </summary>
        string SearchText { get; set; }
    }
}