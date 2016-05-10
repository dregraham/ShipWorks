using System;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// An exception thrown when ShipWorks encounters an ODBC related exceptional circumstance
    /// </summary>
    public class ShipWorksOdbcException : Exception
    {
        public ShipWorksOdbcException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ShipWorksOdbcException(string message)
            : base(message)
        {
        }
    }
}