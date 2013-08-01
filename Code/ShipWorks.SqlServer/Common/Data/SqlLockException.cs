using System;

namespace ShipWorks.SqlServer.Common.Data
{
    public class SqlLockException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlLockException"/> class. 
        /// </summary>
        public SqlLockException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlLockException"/> class. 
        /// </summary>
        public SqlLockException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
