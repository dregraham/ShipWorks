using System;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Common.IO.Hardware.Scanner;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Factory for creating message filters
    /// </summary>
    [Component]
    public class ScannerMessageFilterFactory : IScannerMessageFilterFactory
    {
        readonly Func<IScannerMessageFilter> createScannerMessageFilter;

        /// <summary>
        /// Constructor
        /// </summary>
        public ScannerMessageFilterFactory(Func<IScannerMessageFilter> createScannerMessageFilter)
        {
            this.createScannerMessageFilter = createScannerMessageFilter;
        }

        /// <summary>
        /// Create a scanner message filter
        /// </summary>
        public IScannerMessageFilter CreateMessageFilter() => createScannerMessageFilter();
    }
}
