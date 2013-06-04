using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using System.Transactions;
using ShipWorks.SqlServer.Common.Data;

namespace ShipWorks.Stores.Communication
{
    /// <summary>
    /// Class used to gate customer creation, so that a race condition cannot occur where a duplicate
    /// customer is created.
    /// </summary>
    public class CustomerAcquisitionLock : IDisposable
    {
        SqlConnection con;
        static string lockName = "CustomerAcquisition";

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerAcquisitionLock()
        {
            AcquireLock();
        }

        /// <summary>
        /// Acquire the lock.  If the lock cannot be acquired, a CustomerAcquisitionLockException is thrown.
        /// </summary>
        private void AcquireLock()
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                con = SqlSession.Current.OpenConnection();
            }

            if (!SqlAppLockUtility.AcquireLock(con, lockName, TimeSpan.FromMinutes(1)))
            {
                con.Dispose();
                con = null;

                throw new CustomerAcquisitionLockException("Failed to acquire lock.");
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
