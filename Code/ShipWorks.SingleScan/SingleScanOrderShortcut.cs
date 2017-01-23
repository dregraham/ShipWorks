using ShipWorks.Data;
using ShipWorks.Filters.Search;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Prefix that identifies a scan result as a ShipWorks order
    /// </summary>
    /// <seealso cref="ISingleScanOrderShortcut" />
    public class SingleScanOrderShortcut : ISingleScanOrderShortcut
    {
        private readonly IDataProvider dataProvider;

        // ShipWorks order prefix
        private const string ShipWorksOrderPrefix = "SWO";

        // ShipWorks order suffix - all ShipWorks order IDs end with 006
        private const string ShipWorksOrderSuffix = "006";

        // OrderID used when we cannot find an OrderID in the barcodeText
        private const long UnparsedOrderID = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleScanOrderShortcut"/> class.
        /// </summary>
        /// <param name="dataProvider"></param>
        public SingleScanOrderShortcut(IDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;
        }

        /// <summary>
        /// Scan result that is displayed to user. If the barcode is an order id with the ShipWorks order id prefix,
        /// display the order number, else display what was actually scanned.
        /// </summary>
        public string GetDisplayText(string barcodeText)
        {
            if (AppliesTo(barcodeText))
            {
                long orderId = GetOrderID(barcodeText);

                if (orderId != UnparsedOrderID)
                {
                    string orderNumber = dataProvider.GetOrderNumberComplete(orderId);

                    if (!string.IsNullOrWhiteSpace(orderNumber))
                    {
                        return orderNumber;
                    }
                }
            }

            return barcodeText;
        }

        /// <summary>
        /// Whether or not the scan result begins with the ShipWorks order prefix
        /// </summary>
        public bool AppliesTo(string barcodeText)
        {
            return barcodeText.StartsWith(ShipWorksOrderPrefix) && barcodeText.EndsWith(ShipWorksOrderSuffix);
        }

        /// <summary>
        /// Returns the OrderId from the given barcode
        /// </summary>
        public long GetOrderID(string barcodeText)
        {
            long orderID;
            if (AppliesTo(barcodeText) && long.TryParse(barcodeText.Remove(0, ShipWorksOrderPrefix.Length), out orderID))
            {
                return orderID;
            }
            return UnparsedOrderID;
        }
    }
}