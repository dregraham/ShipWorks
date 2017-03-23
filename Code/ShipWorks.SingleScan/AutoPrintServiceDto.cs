using ShipWorks.Messaging.Messages.Filters;
using ShipWorks.Messaging.Messages.SingleScan;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// A holder for the FilterCountsUpdateMessage and ScanMessage
    /// </summary>
    public struct AutoPrintServiceDto
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AutoPrintServiceDto(SingleScanFilterUpdateCompleteMessage singleScanFilterUpdateCompleteMessage,
            ScanMessage scanMessage)
        {
            MatchedOrderCount = singleScanFilterUpdateCompleteMessage.FilterNodeContent.Count;
            ScannedBarcode = scanMessage.ScannedText;

            bool orderNotFound = singleScanFilterUpdateCompleteMessage.FilterNodeContent == null ||
                                     singleScanFilterUpdateCompleteMessage.FilterNodeContent.Count < 1 ||
                                     singleScanFilterUpdateCompleteMessage.OrderId == null;

            OrderID = orderNotFound ? (long?) null : singleScanFilterUpdateCompleteMessage.OrderId.Value;
        }

        /// <summary>
        /// The number of orders matching the scanned barcode
        /// </summary>
        public int MatchedOrderCount { get; set; }

        /// <summary>
        /// The scanned barcode.
        /// </summary>
        public string ScannedBarcode { get; set; }

        /// <summary>
        /// Get the order ID of the order we intend to process
        /// </summary>
        public long? OrderID { get; set; }
    }
}
