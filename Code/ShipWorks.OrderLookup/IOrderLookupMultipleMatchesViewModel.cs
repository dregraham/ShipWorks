using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Represents a view model for multiple matched orders to confirm
    /// </summary>
    public interface IOrderLookupMultipleMatchesViewModel
    {
        /// <summary>
        /// The Orders
        /// </summary>
        IEnumerable<OrderEntity> Orders { get; set; }

        /// <summary>
        /// The SelectedOrder
        /// </summary>
        OrderEntity SelectedOrder { get; set; }
    }
}