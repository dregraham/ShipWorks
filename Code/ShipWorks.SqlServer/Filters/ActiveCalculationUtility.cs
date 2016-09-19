using System;
using ShipWorks.SqlServer.Common.Data;
using System.Data.Common;

namespace ShipWorks.SqlServer.Filters
{
    /// <summary>
    /// Utility functions for dealing with active filter calculations
    /// </summary>
    public static class ActiveCalculationUtility
    {
        public const string DefaultLockName = "FilterCountCalculator";
        public const string QuickFilterLockName = "QuickFilterCountCalculator";

        /// <summary>
        /// Checks to see if this stored procedure is already running.  If it is not, a lock is taken to indicate that it is now.
        /// </summary>
        public static bool AcquireCalculatingLock(DbConnection con)
        {
            return AcquireCalculatingLock(con, TimeSpan.Zero);
        }

        /// <summary>
        /// Checks to see if this stored procedure is already running.  If it is not, a lock is taken to indicate that it is now.
        /// </summary>
        public static bool AcquireCalculatingLock(DbConnection con, TimeSpan wait)
        {
            return SqlAppLockUtility.AcquireLock(con, DefaultLockName, wait);
        }

        /// <summary>
        /// Checks to see if this stored procedure is already running.  If it is not, a lock is taken to indicate that it is now.
        /// </summary>
        public static bool AcquireCalculatingLock(DbConnection con, TimeSpan wait, string lockName)
        {
            return SqlAppLockUtility.AcquireLock(con, lockName, wait);
        }

        /// <summary>
        /// Releases a previously taken lock.
        /// </summary>
        public static void ReleaseCalculatingLock(DbConnection con)
        {
            SqlAppLockUtility.ReleaseLock(con, DefaultLockName);
        }

        /// <summary>
        /// Releases a previously taken lock.
        /// </summary>
        public static void ReleaseCalculatingLock(DbConnection con, string lockName)
        {
            SqlAppLockUtility.ReleaseLock(con, lockName);
        }
    }
}
