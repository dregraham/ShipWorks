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

        // ShipWorks order prefix
        private const string ShipWorksOrderPrefix = "SWO";

        // ShipWorks order postfix - all ShipWorks order IDs end with 006
        private const string ShipWorksOrderPostFix = "006";

        // OrderID used when we cannot find an OrderID in the barcodeText
        private const long UnparsedOrderID = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleScanOrderPrefix"/> class.
        /// </summary>
        /// <param name="orderManager">The order manager.</param>
        public SingleScanOrderPrefix(IOrderManager orderManager)
        {
            this.orderManager = orderManager;
        }

        /// <summary>
        /// Scan result that is displayed to user. If the barcode is an order id with the ShipWorks order id prefix,
        /// display the order number, else display what was actually scanned.
        /// </summary>
        public string GetDisplayText(string barcodeText)
        {
            if (Contains(barcodeText))
            {
                long orderId = GetOrderID(barcodeText);

                if (orderId  != UnparsedOrderID)
                {
                    OrderEntity order = orderManager.FetchOrder(GetOrderID(barcodeText));

                    if (order != null && !order.IsNew)
                    {
                        return order.OrderNumberComplete;
                    }
                }
            }

            return barcodeText;
        }

        /// <summary>
        /// Whether or not the scan result begins with the ShipWorks order prefix
        /// </summary>
        public bool Contains(string barcodeText)
        {
            return barcodeText.StartsWith(ShipWorksOrderPrefix) && barcodeText.EndsWith(ShipWorksOrderPostFix);
        }

        /// <summary>
        /// Returns the OrderId from the given barcode
        /// </summary>
        public long GetOrderID(string barcodeText)
        {
            long orderID;
            if (Contains(barcodeText) && long.TryParse(barcodeText.Remove(0, ShipWorksOrderPrefix.Length), out orderID))
            {
                return orderID;
            }
            return UnparsedOrderID;
        }
    }
}