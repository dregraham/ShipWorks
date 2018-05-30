using System;
using System.Data.Common;
using System.Diagnostics;
using Interapptive.Shared.Data;
using log4net;

namespace ShipWorks.Data.Connection
{
    /// <summary>
    /// Get the date and time as provided by SQL server
    /// </summary>
    public class SqlDateTimeProvider : ISqlDateTimeProvider
    {
        private static readonly ISqlDateTimeProvider current = new SqlDateTimeProvider();
        private static readonly ILog log = LogManager.GetLogger(typeof(SqlDateTimeProvider));
        private DateTime serverDateLocal;
        private DateTime serverDateUtc;
        private Stopwatch timeSinceTimeTaken;
        private bool resetCache = true;

        /// <summary>
        /// Current instance of the SQL date time lookup
        /// </summary>
        public static ISqlDateTimeProvider Current => current;

        /// <summary>
        /// Get the latest time information from the server. Uses a cache mechanism for efficiency, so
        /// we don't go to the server every invocation.
        ///
        /// If the time has been retrieved from the server withing the past 30 minutes, then the current time
        /// is estimated by adding the last retrieved time plus the elapsed time.
        /// </summary>
        public DateTime GetLocalDate()
        {
            RefreshSqlDateTime();
            return serverDateLocal + timeSinceTimeTaken.Elapsed;
        }

        /// <summary>
        /// Get the latest UTC time information from the server. Uses a cache mechanism for efficiency, so
        /// we don't go to the server every invocation.
        ///
        /// If the time has been retrieved from the server withing the past 30 minutes, then the current time
        /// is estimated by adding the last retrieved time plus the elapsed time.
        /// </summary>
        public DateTime GetUtcDate()
        {
            RefreshSqlDateTime();
            return serverDateUtc + timeSinceTimeTaken.Elapsed;
        }

        /// <summary>
        /// Reset the cached time
        /// </summary>
        public void ResetCache() => resetCache = true;

        /// <summary>
        /// Ensure that the cached SQL server time is reasonably fresh
        /// </summary>
        private void RefreshSqlDateTime()
        {
            if (resetCache || timeSinceTimeTaken?.Elapsed > TimeSpan.FromMinutes(30))
            {
                // Get the server times if our cache is stale
                ExistingConnectionScope.ExecuteWithCommand(cmd =>
                {
                    cmd.CommandText = "SELECT GETDATE() AS [ServerDateLocal], GETUTCDATE() AS [ServerDateUtc]";

                    using (DbDataReader reader = DbCommandProvider.ExecuteReader(cmd))
                    {
                        reader.Read();

                        serverDateLocal = (DateTime) reader["ServerDateLocal"];
                        serverDateUtc = (DateTime) reader["ServerDateUtc"];

                        log.Info($"Server LocalDate ({serverDateLocal}), UTC ({serverDateUtc})");

                        timeSinceTimeTaken = Stopwatch.StartNew();
                        resetCache = false;
                    }
                });
            }
        }
    }
}
