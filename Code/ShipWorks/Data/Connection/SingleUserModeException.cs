using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Connection
{
    /// <summary>
    /// Exception raised when ShipWorks can't connect to the database b\c another ShipWorks client is currently upgrading or restoring it.
    /// </summary>
    public class SingleUserModeException : Exception
    {
        public SingleUserModeException()
        {

        }

        public SingleUserModeException(string message)
            : base(message)
        {

        }

        public SingleUserModeException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
