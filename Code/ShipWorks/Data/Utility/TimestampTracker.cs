using System;
using System.Data.Common;
using System.Data.SqlClient;
using Interapptive.Shared.Data;
using log4net;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Connection;

namespace ShipWorks.Data.Utility
{
    /// <summary>
    /// Used for tracking changes to the last @@DBTS of the database
    /// </summary>
    public class TimestampTracker
    {
        static ILog log = LogManager.GetLogger(typeof(TimestampTracker));
        long timestamp = 0;

        /// <summary>
        /// String representation of the class is the timestamp value.
        /// </summary>
        public override string ToString() => Timestamp.ToString();

        /// <summary>
        /// The current timestamp value
        /// </summary>
        public long Timestamp => timestamp;

        /// <summary>
        /// Gets the latest timestamp value from the database, and returns true if it is different.
        /// </summary>
        public bool CheckForChange()
        {
            using (DbConnection con = SqlSession.Current.OpenConnection())
            {
                long current = 0;

                try
                {
                    SqlAdapterRetry<SqlException> sqlAdapterRetry = new SqlAdapterRetry<SqlException>(3, -5, "TimestapTracker.CheckForChange");
                    sqlAdapterRetry.ExecuteWithRetry(() => current = DbCommandProvider.ExecuteScalar<long>(con, "SELECT CAST(@@DBTS AS BIGINT)"));
                }
                catch (SystemException ex)
                {
                    // During a reconnect or after an upgrade, ExecuteScalar can throw an
                    // InvalidOperationException or InvalidCastException. If that happens, we'll assume no changes
                    // for the moment and let the next run through of the heartbeat pick it up.
                    log.Error("Could not update timestamp", ex);
                }

                if (current > timestamp)
                {
                    timestamp = current;
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Reset the tracker to its initial state (Timestamp = 0)
        /// </summary>
        public void Reset()
        {
            timestamp = 0;
        }
    }
}
