using System;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Exception thrown by the Scanner Configuration Repository
    /// </summary>
    public class ScannerConfigurationRepositoryException : Exception
    {
        public ScannerConfigurationRepositoryException()
        {

        }

        public ScannerConfigurationRepositoryException(string message)
            : base(message)
        {

        }

        public ScannerConfigurationRepositoryException(string message, Exception innerException)
            : base(message, innerException)
        {


        }
    }
}