using System;
using System.Collections.Generic;
using System.Text;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Exception thrown when the database we are connected to is known not to be a ShipWorks database.
    /// </summary>
    public class InvalidShipWorksDatabaseException : Exception
    {
        public InvalidShipWorksDatabaseException()
        {

        }

        public InvalidShipWorksDatabaseException(string message)
            : base(message)
        {

        }

        public InvalidShipWorksDatabaseException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
