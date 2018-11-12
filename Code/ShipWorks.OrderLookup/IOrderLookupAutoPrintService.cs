using System.Threading.Tasks;
using ShipWorks.Messaging.Messages.SingleScan;
using ShipWorks.SingleScan;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Represents the auto print service for order lookup
    /// </summary>
    public interface IOrderLookupAutoPrintService
    {
        /// <summary>
        /// Print shipments for the orderid
        /// </summary>
        Task<AutoPrintCompletionResult> AutoPrintShipment(long orderID, SingleScanMessage message);
    }
}