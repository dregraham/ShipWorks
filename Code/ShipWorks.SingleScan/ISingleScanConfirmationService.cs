using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.Metrics;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Service to confirm shipments and orders
    /// </summary>
    public interface ISingleScanConfirmationService
    {
        /// <summary>
        /// Confirms that the order with the given orderId should be printed.
        /// </summary>
        bool ConfirmOrder(long orderId, int numberOfMatchedOrders, string scanText);

        /// <summary>
        /// Gets the Shipments that SingleScan should auto print/process
        /// </summary>
        Task<IEnumerable<ShipmentEntity>> GetShipments(long orderId, string scannedBarcode);
    }
}