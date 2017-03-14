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
    public class AutoPrintCompletionResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoPrintCompletionResult"/> class.
        /// </summary>
        public AutoPrintCompletionResult(long? orderID, IEnumerable<ProcessShipmentResult> processShipmentResults)
        {
            ProcessShipmentResults = processShipmentResults;
            OrderID = orderID;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoPrintCompletionResult"/> class.
        /// </summary>
        public AutoPrintCompletionResult(long? orderID)
            : this(orderID, new List<ProcessShipmentResult>())
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
