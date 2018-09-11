using ShipWorks.OrderLookup.Controls.OrderLookupSearchControl;

namespace ShipWorks.OrderLookup.Controls.OrderLookupControl
{
    /// <summary>
    /// Main view model for the OrderLookup UI Mode
    /// </summary>
    public class OrderLookupViewModel
    {
        private readonly IOrderLookupDataService dataService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataService"></param>
        public OrderLookupViewModel(IOrderLookupDataService dataService)
        {
            this.dataService = dataService;
            OrderLookupSearchViewModel = new OrderLookupSearchViewModel(dataService);
        }

        /// <summary>
        /// View Model for the search section of the OrderLookup UI Mode
        /// </summary>
        public OrderLookupSearchViewModel OrderLookupSearchViewModel { get; set; }
    }
}
