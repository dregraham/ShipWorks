using System;
using System.Collections.Generic;
using System.Text;

namespace ShipWorks.Data.Connection
{
    /// <summary>
    /// Exception thrown when the connection to the database is permenantly lost.
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
