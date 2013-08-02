using System;

namespace ShipWorks.SqlServer.Common.Data
{
    /// <summary>
    /// Exception that is thrown when a sql app lock can not be acquired.
    /// </summary>
    public class SqlLockException : Exception
    {
        private readonly string requestedLockName;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlLockException"/> class. 
        /// </summary>
        /// <param name="lockName">Name of the lock that was requested that caused the exception.</param>
        public SqlLockException(string lockName)
            : this(lockName, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlLockException"/> class. 
        /// </summary>
        /// <param name="lockName">Name of the lock that was requested that caused the exception.</param>
        /// <param name="innerException">Specifies any inner exception that might have caused this exception.</param>
        public SqlLockException(string lockName, Exception innerException)
            : base(string.Format("Could not acquire applock: {0}", lockName), innerException)
        {
            requestedLockName = lockName;
        }

        /// <summary>
        /// Gets the name of the lock that was requested that caused the exception
        /// </summary>
        public string RequestedLockName
        {
            get { return requestedLockName; }
        }
    }
}
