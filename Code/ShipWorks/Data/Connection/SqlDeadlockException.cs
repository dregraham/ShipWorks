using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Connection
{
    /// <summary>
    /// Exception thrown when there is a deadlock detected by SQL Server
    /// </summary>
    class SqlDeadlockException : Exception
    {
        public SqlDeadlockException()
        {

        }

        public SqlDeadlockException(string message)
            : base(message)
        {

        }

        public SqlDeadlockException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
