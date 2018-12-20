using System;
using System.Data.Common;

namespace Interapptive.Shared.Data
{
    /// <summary>
    /// Interface for SQL App Lock class
    /// </summary>
    public interface ISqlAppLock
    {
        /// <summary>
        /// Take a SqlAppLock
        /// </summary>
        IDisposable Take(DbConnection connection, string appLockName, TimeSpan timeSpan);

        /// <summary>
        /// Has a lock been acquired?
        /// </summary>
        bool LockAcquired { get; }
    }
}