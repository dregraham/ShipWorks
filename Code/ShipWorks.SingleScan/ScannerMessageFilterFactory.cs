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
        /// <summary>
        /// Create a scanner message filter
        /// </summary>
        public IScannerMessageFilter CreateMessageFilter()
        {
            throw new NotImplementedException();
        }
    }
}
