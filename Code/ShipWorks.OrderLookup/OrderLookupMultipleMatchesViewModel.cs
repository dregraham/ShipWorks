using System.Collections.Generic;
using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// View Model for confirming multiple matched orders
    /// </summary>
    [Component]
    public class OrderLookupMultipleMatchesViewModel : IOrderLookupMultipleMatchesViewModel
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

        /// <summary>
        /// Load the orders into the view model
        /// </summary>
        public void Load(IEnumerable<OrderEntity> orders)
        {

        }
    }
}
