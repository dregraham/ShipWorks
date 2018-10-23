using System.Collections.Generic;
using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.OrderLookup.Controls.OrderConfirmationDialog;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// View Model for confirming multiple matched orders
    /// </summary>
    [Component]
    public class OrderConfirmationViewModel : IOrderConfirmationViewModel
    {
        /// <summary>
        /// The selected order
        /// </summary>
        [Obfuscation(Exclude = true)]
        public OrderEntity SelectedOrder { get; set; }

        /// <summary>
        /// The list of orders
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEnumerable<OrderEntity> Orders { get; set; }
    }
}
