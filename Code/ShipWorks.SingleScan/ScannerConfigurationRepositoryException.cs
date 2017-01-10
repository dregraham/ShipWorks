using System;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Exception thrown by the Scanner Configuration Repository
    /// </summary>
    public class ScannerConfigurationRepositoryException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ScannerConfigurationRepositoryException()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ScannerConfigurationRepositoryException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ScannerConfigurationRepositoryException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}