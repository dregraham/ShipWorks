using System;

namespace ShipWorks.Data.Connection
{
    /// <summary>
    /// Exception thrown when the connection to the database is permanently lost.
    /// </summary>
    class ConnectionLostException : Exception
    {
        public ConnectionLostException()
        {

        }

        public ConnectionLostException(string message)
            : base(message)
        {

        }

        public ConnectionLostException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
