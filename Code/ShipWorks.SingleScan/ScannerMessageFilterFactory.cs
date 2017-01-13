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
        private readonly Func<RegisteredScannerInputHandler> createScannerMessageFilter;
        private readonly Func<ScannerRegistrationMessageFilter> createFindScannerMessageFilter;

        /// <summary>
        /// Constructor
        /// </summary>
        public ScannerMessageFilterFactory(Func<RegisteredScannerInputHandler> createScannerMessageFilter, Func<ScannerRegistrationMessageFilter> createFindScannerMessageFilter)
        {
            this.createScannerMessageFilter = createScannerMessageFilter;
            this.createFindScannerMessageFilter = createFindScannerMessageFilter;
        }

        /// <summary>
        /// Create a scanner message filter
        /// </summary>
        public IScannerMessageFilter CreateMessageFilter() => createScannerMessageFilter();

        /// <summary>
        /// Create a find scanner message filter
        /// </summary>
        public IScannerMessageFilter CreateFindScannerMessageFilter() => createFindScannerMessageFilter();
    }
}
