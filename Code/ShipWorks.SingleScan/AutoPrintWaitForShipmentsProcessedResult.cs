using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Messaging.Messages.Shipping;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Results from waiting for shipments processed message
    /// </summary>
    public class AutoPrintWaitForShipmentsProcessedResult
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoPrintWaitForShipmentsProcessedResult"/> class.
        /// </summary>
        public AutoPrintWaitForShipmentsProcessedResult(long? orderID, IEnumerable<ProcessShipmentResult> processShipmentResults)
        {
            ProcessShipmentResults = processShipmentResults;
            OrderID = orderID;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoPrintWaitForShipmentsProcessedResult"/> class.
        /// </summary>
        public AutoPrintWaitForShipmentsProcessedResult(long? orderID): this(orderID, null)
        {
        }

        /// <summary>
        /// Gets or sets the process shipment results.
        /// </summary>
        public IEnumerable<ProcessShipmentResult> ProcessShipmentResults { get; }

        /// <summary>
        /// Gets or sets the order identifier.
        /// </summary>
        public long? OrderID { get; }
    }
}
