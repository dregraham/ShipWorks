using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Utility
{
    /// <summary>
    /// Exception that is thrown when an app resource lock cannot be taken (its already taken on another open connection)
    /// </summary>
    public class SqlAppResourceLockException : Exception
    {
        string lockName;

        /// <summary>
        /// Constructor
        /// </summary>
        public SqlAppResourceLockException(string lockName)
        {
            this.lockName = lockName;
        }

        /// <summary>
        /// The name of the lock that tried to be acquired
        /// </summary>
        public string LockName
        {
            get { return lockName; }
        }

        /// <summary>
        /// The description of the exception
        /// </summary>
        public override string Message
        {
            get
            {
                return string.Format("A lock on could not be taken for '{0}'.", lockName);
            }
        }
    }
}
