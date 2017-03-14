using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Result from AutoPrintService.Print()
    /// </summary>
    public struct AutoPrintResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoPrintResult"/> struct.
        /// </summary>
        public AutoPrintResult(string scannedBarcode, long? orderId)
        {
            ScannedBarcode = scannedBarcode;
            OrderId = orderId;
        }

        /// <summary>
        /// Gets the scanned barcode.
        /// </summary>
        public string ScannedBarcode { get; }

        /// <summary>
        /// Gets the order identifier.
        /// </summary>
        public long? OrderId { get; }
    }
}
