using System;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Exception thrown if DatabaseIdentifier not available.
    /// </summary>
    public class DatabaseIdentifierException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseIdentifierException"/> class.
        /// </summary>
        public DatabaseIdentifierException()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DatabaseIdentifierException(Exception exception)
            :base(exception.Message, exception)
        {
        }
    }
}