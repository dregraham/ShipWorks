using ShipWorks.OrderLookup.Controls.OrderLookupSearchControl;

namespace ShipWorks.OrderLookup.Controls.OrderLookupControl
{
    /// <summary>
    /// Main view model for the OrderLookup UI Mode
    /// </summary>
    public class OrderLookupViewModel
    {
        private readonly IOrderLookupMessageBus messageBus;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="messageBus"></param>
        public OrderLookupViewModel(IOrderLookupMessageBus messageBus)
        {
            this.messageBus = messageBus;
            OrderLookupSearchViewModel = new OrderLookupSearchViewModel(messageBus);
        }

        /// <summary>
        /// View Model for the search section of the OrderLookup UI Mode
        /// </summary>
        public OrderLookupSearchViewModel OrderLookupSearchViewModel { get; set; }
    }
}
