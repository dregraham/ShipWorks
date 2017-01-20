using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters.Search;
using ShipWorks.Stores.Content;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Prefix that identifies a scan result as a ShipWorks order
    /// </summary>
    /// <seealso cref="ShipWorks.Filters.Search.ISingleScanOrderPrefix" />
    public class SingleScanOrderPrefix : ISingleScanOrderPrefix
    {
        private readonly IOrderManager orderManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleScanOrderPrefix"/> class.
        /// </summary>
        /// <param name="orderManager">The order manager.</param>
        public SingleScanOrderPrefix(IOrderManager orderManager)
        {
            this.orderManager = orderManager;
        }

        // ShipWorks order prefix
        private const string ShipWorksOrderPrefix = "SWO";

        // ShipWorks order postfix - all ShipWorks order IDs end with 006
        private const string ShipWorksOrderPostFix = "006";

        /// <summary>
        /// Scan result that is displayed to user. If the barcode is an order id with the ShipWorks order id prefix,
        /// display the order number, else display what was actually scanned.
        /// </summary>
        public string GetDisplayText(string barcodeText)
        {
            string displayText = barcodeText;

            if (Contains(displayText))
            {
                long orderID;
                if (long.TryParse(displayText.Remove(0, ShipWorksOrderPrefix.Length), out orderID))
                {
                    OrderEntity order = orderManager.FetchOrder(orderID);

                    if (order != null)
                    {
                        displayText = order.OrderNumberComplete;
                    }
                }
            }

            return displayText;
        }

        /// <summary>
        /// Whether or not the scan result begins with the ShipWorks order prefix
        /// </summary>
        public bool Contains(string barcodeText)
        {
            return barcodeText.StartsWith(ShipWorksOrderPrefix) && barcodeText.EndsWith(ShipWorksOrderPostFix);
        }
    }
}