using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ShipWorks.SqlServer.Common.Data;

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
        public static bool AcquireCalculatingLock(SqlConnection con)
        {
            return AcquireCalculatingLock(con, TimeSpan.Zero);
        }

        /// <summary>
        /// Checks to see if this stored procedure is already running.  If it is not, a lock is taken to indicate that it is now.
        /// </summary>
        public static bool AcquireCalculatingLock(SqlConnection con, TimeSpan wait)
        {
            return SqlAppLockUtility.AcquireLock(con, DefaultLockName, wait);
        }

        /// <summary>
        /// Checks to see if this stored procedure is already running.  If it is not, a lock is taken to indicate that it is now.
        /// </summary>
        public static bool AcquireCalculatingLock(SqlConnection con, TimeSpan wait, string lockName)
        {
            return SqlAppLockUtility.AcquireLock(con, lockName, wait);
        }

        /// <summary>
        /// Releases a previously taken lock.
        /// </summary>
        public static void ReleaseCalculatingLock(SqlConnection con)
        {
            SqlAppLockUtility.ReleaseLock(con, DefaultLockName);
        }

        /// <summary>
        /// Releases a previously taken lock.
        /// </summary>
        public static void ReleaseCalculatingLock(SqlConnection con, string lockName)
        {
            SqlAppLockUtility.ReleaseLock(con, lockName);
        }
    }
}
