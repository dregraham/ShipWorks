using System;
using Interapptive.Shared.Win32;
using log4net;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Common;
using ShipWorks.Common.IO.Hardware.Scanner;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Factory for creating message filters
    /// </summary>
    [Component]
    public class ScannerMessageFilterFactory : IScannerMessageFilterFactory
    {
        private readonly IScannerIdentifier scannerIdentifier;
        private readonly IUser32Input user32Input;
        private readonly IScanBuffer scanBuffer;
        private readonly IWindowsMessageFilterRegistrar windowsMessageFilterRegistrar;
        private readonly Func<Type, ILog> getLogger;

        /// <summary>
        /// Constructor
        /// </summary>
        public ScannerMessageFilterFactory(IScannerIdentifier scannerIdentifier,
            IUser32Input user32Input,
            IScanBuffer scanBuffer,
            IWindowsMessageFilterRegistrar windowsMessageFilterRegistrar,
            Func<Type, ILog> getLogger)
        {
            this.scannerIdentifier = scannerIdentifier;
            this.user32Input = user32Input;
            this.scanBuffer = scanBuffer;
            this.windowsMessageFilterRegistrar = windowsMessageFilterRegistrar;
            this.getLogger = getLogger;
        }

        /// <summary>
        /// Create a scanner message filter
        /// </summary>
        public IScannerMessageFilter CreateRegisteredScannerInputHandler() =>
            new RegisteredScannerInputHandler(scannerIdentifier, user32Input, scanBuffer, windowsMessageFilterRegistrar,
                getLogger);

        /// <summary>
        /// Create a find scanner message filter
        /// </summary>
        public IScannerMessageFilter CreateScannerRegistrationMessageFilter() =>
            new ScannerRegistrationMessageFilter(user32Input, scanBuffer, windowsMessageFilterRegistrar, getLogger);
    }
}
