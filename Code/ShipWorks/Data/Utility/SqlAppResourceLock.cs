using System;
using System.Data.SqlClient;
using ShipWorks.Data.Connection;
using ShipWorks.SqlServer.Common.Data;

namespace ShipWorks.Data.Utility
{
    /// <summary>
    /// A lock is taken on the given resource name, preventing any other connection also requesting a lock from working
    /// with the resource name.
    /// </summary>
    public class SqlAppResourceLock : IDisposable
    {
        SqlConnection con;
        string lockName;

        /// <summary>
        /// A lock is taken on the given resource name, preventing any other connection also requesting a lock from working
        /// with the resource name.  Throws a SqlAppResourceLockException if the lock cannot be taken.
        /// </summary>
        public SqlAppResourceLock(string resourceName)
        {
            lockName = resourceName;

            AcquireLock();
        }

        /// <summary>
        /// Acquire the lock.  If the lock cannot be acquired, a SqlAppResourceLockException is thrown.
        /// </summary>
        private void AcquireLock()
        {
            con = SqlSession.Current.OpenConnection();

            if (!SqlAppLockUtility.AcquireLock(con, lockName))
            {
                con.Dispose();
                con = null;

                throw new SqlAppResourceLockException(lockName);
            }
        }

        /// <summary>
        /// Release the lock
        /// </summary>
        private void ReleaseLock()
        {
            if (con != null)
            {
                SqlAppLockUtility.ReleaseLock(con, lockName);

                con.Dispose();
                con = null;
            }
        }

        /// <summary>
        /// Dispose - release the lock
        /// </summary>
        public void Dispose()
        {
            ReleaseLock();
        }
    }
}
