using ShipWorks.Messaging.Messages.Filters;
using ShipWorks.Messaging.Messages.SingleScan;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// A holder for the FilterCountsUpdateMessage and ScanMessage
    /// </summary>
    public struct FilterCountsUpdatedAndScanMessages
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FilterCountsUpdatedAndScanMessages(FilterCountsUpdatedMessage filterCountsUpdatedMessage,
            ScanMessage scanMessage)
        {
            FilterCountsUpdatedMessage = filterCountsUpdatedMessage;
            ScanMessage = scanMessage;
        }

        /// <summary>
        /// Gets the filter counts updated message.
        /// </summary>
        public FilterCountsUpdatedMessage FilterCountsUpdatedMessage { get; }

        /// <summary>
        /// Gets the scan message.
        /// </summary>
        public ScanMessage ScanMessage { get; }
    }
}
