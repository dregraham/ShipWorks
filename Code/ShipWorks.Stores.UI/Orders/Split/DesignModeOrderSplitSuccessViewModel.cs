using System.Collections.Generic;

namespace ShipWorks.Stores.UI.Orders.Split
{
    /// <summary>
    /// Design mode version of the order split success view model
    /// </summary>
    public class DesignModeOrderSplitSuccessViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DesignModeOrderSplitSuccessViewModel()
        {
            SplitOrders = new[] { "72278", "72278-1" };
        }

        /// <summary>
        /// Split orders
        /// </summary>
        public IEnumerable<string> SplitOrders { get; }
    }
}