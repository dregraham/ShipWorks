using Interapptive.Shared.ComponentRegistration;
using ShipWorks.OrderLookup.Controls.OrderLookupSearchControl;

namespace ShipWorks.OrderLookup.Controls.OrderLookupControl
{
    /// <summary>
    /// Main view model for the OrderLookup UI Mode
    /// </summary>
    [Component(RegistrationType.Self)]
    public class OrderLookupViewModel
    {
        private readonly IOrderLookupMessageBus messageBus;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupViewModel(IOrderLookupMessageBus messageBus, OrderLookupSearchViewModel orderLookupSearchViewModel)
        {
            this.messageBus = messageBus;
            OrderLookupSearchViewModel = orderLookupSearchViewModel;
        }

        /// <summary>
        /// View Model for the search section of the OrderLookup UI Mode
        /// </summary>
        public OrderLookupSearchViewModel OrderLookupSearchViewModel { get; set; }
    }
}
