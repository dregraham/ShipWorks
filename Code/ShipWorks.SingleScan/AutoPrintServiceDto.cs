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
            SingleScanFilterUpdateCompleteMessage = singleScanFilterUpdateCompleteMessage;
            ScanMessage = scanMessage;
        }

        /// <summary>
        /// Gets the filter counts updated message.
        /// </summary>
        public SingleScanFilterUpdateCompleteMessage SingleScanFilterUpdateCompleteMessage { get; }

        /// <summary>
        /// Gets the scan message.
        /// </summary>
        public ScanMessage ScanMessage { get; }
    }
}
