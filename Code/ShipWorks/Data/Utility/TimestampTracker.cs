using System.Data.SqlClient;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Connection;

namespace ShipWorks.Data.Utility
{
    /// <summary>
    /// Used for tracking changes to the last @@DBTS of the database
    /// </summary>
    public class TimestampTracker
    {
        long timestamp = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        public TimestampTracker()
        {

        }

        /// <summary>
        /// String representation of the class is the timestamp value.
        /// </summary>
        public override string ToString()
        {
            return Timestamp.ToString();
        }

        /// <summary>
        /// The current timestamp value
        /// </summary>
        public long Timestamp
        {
            get { return timestamp; }
        }

        /// <summary>
        /// Gets the latest timestamp value from the database, and returns true if it is different.
        /// </summary>
        public bool CheckForChange()
        {
            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                long current = 0;
                object value = null;

                SqlAdapterRetry<SqlException> sqlAdapterRetry = new SqlAdapterRetry<SqlException>(3, -5, "TimestapTracker.CheckForChange");
                using (SqlCommand sqlCommand = new SqlCommand("SELECT CAST(@@DBTS AS BIGINT)"))
                {
                    IRetrievalQuery query = new RetrievalQuery(sqlCommand);
                    sqlAdapterRetry.ExecuteWithRetry(adapter => value = adapter.ExecuteScalarQuery(query));    
                }

                // During a reconnect or after an upgrade, the timestamp is sometimes returned as null
                // If that happens, we'll assume no changes for the moment and let the next run through
                // of the heartbeat pick it up.
                if (value is long)
                {
                    current = (long) value;
                }

                if (current > timestamp)
                {
                    timestamp = current;
                    return true;
                }
                else
                {
                    return false;
                }
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
