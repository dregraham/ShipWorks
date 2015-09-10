using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Connection
{
    /// <summary>
    /// Exception thrown when an entity could not be saved due to a FK constraint
    /// </summary>
    public class SqlForeignKeyException : Exception
    {
        public SqlForeignKeyException()
        {

        }

        public SqlForeignKeyException(string message)
            : base(message)
        {

        }

        public SqlForeignKeyException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
