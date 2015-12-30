using System;
using System.Data.SqlClient;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.SqlServer.Common.Data;

namespace ShipWorks.Data.Utility
{
    /// <summary>
    /// A lock is taken on the given resource name, preventing any other connection also requesting a lock from working
    /// with the resource name.
    /// </summary>
    public class SqlAppResourceLock : IDisposable
    {
        ILifetimeScope lifetimeScope;
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
            // Let the lifetime scope manage disposal of the SQL connection so that we can use a single connection
            // if necessary
            lifetimeScope = IoC.BeginLifetimeScope();
            con = lifetimeScope.Resolve<SqlConnection>();

            if (!SqlAppLockUtility.AcquireLock(con, lockName))
            {
                lifetimeScope.Dispose();
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

                con = null;
            }

            lifetimeScope?.Dispose();
            lifetimeScope = null;
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
