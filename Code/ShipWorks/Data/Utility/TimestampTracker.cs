using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using ShipWorks.Data.Connection;
using Interapptive.Shared.Data;

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
                long current = (long) SqlCommandProvider.ExecuteScalar(con, "SELECT CAST(@@DBTS AS BIGINT)");

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
