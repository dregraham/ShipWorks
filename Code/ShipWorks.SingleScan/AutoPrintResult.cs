using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.SingleScan
{
    public struct AutoPrintResult
    {
        public AutoPrintResult(string scannedBarcode, long? orderId, string errorMessage, bool processShipmentsMessageSent)
        {
            ScannedBarcode = scannedBarcode;
            OrderId = orderId;
            ErrorMessage = errorMessage;
            ProcessShipmentsMessageSent = processShipmentsMessageSent;
        }

        public string ScannedBarcode { get; }
        public long? OrderId { get; }
        public string ErrorMessage { get; }
        public bool ProcessShipmentsMessageSent { get; }
    }
}
