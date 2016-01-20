using System;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Exception thrown if DatabaseIdentifier not available.
    /// </summary>
    public class DatabaseIdentifierException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DatabaseIdentifierException(Exception exception)
            :base(exception.Message, exception)
        {
            
        }
    }
}