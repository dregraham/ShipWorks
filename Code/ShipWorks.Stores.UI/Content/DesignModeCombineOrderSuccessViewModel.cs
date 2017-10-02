using System.Collections.Generic;
using System.Linq;

namespace ShipWorks.Stores.UI.Content
{
    /// <summary>
    /// Design mode version of the order combination success view model
    /// </summary>
    public class DesignModeCombineOrderSuccessViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DesignModeCombineOrderSuccessViewModel()
        {
            OrderNumber = "72280-C";
            CombinedOrders = Enumerable.Range(72278, 3).Select(x => $"#{x}").ToList();
        }

        /// <summary>
        /// Order number
        /// </summary>
        public string OrderNumber { get; }

        /// <summary>
        /// Combined orders
        /// </summary>
        public IEnumerable<string> CombinedOrders { get; }
    }
}