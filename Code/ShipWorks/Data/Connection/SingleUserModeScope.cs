﻿using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using Interapptive.Shared.Data;
using log4net;

namespace ShipWorks.Data.Connection
{
    /// <summary>
    /// Creates a scope within which we need to maintain SINGLE_USER mode for sql server
    /// </summary>
    public class SingleUserModeScope : IDisposable
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(SingleUserModeScope));

        static AsyncLocal<bool> active = new AsyncLocal<bool>();
        static AsyncLocal<TimeSpan> reconnectTimeout = new AsyncLocal<TimeSpan>();
        static AsyncLocal<DbConnection> dbConnection = new AsyncLocal<DbConnection>();

        /// <summary>
        /// Constructor - initiates the scope
        /// </summary>
        public SingleUserModeScope(DbConnection connection, TimeSpan reconnectTimeout)
        {
            if (active.Value)
            {
                throw new InvalidOperationException("Can only have one active scope at a time.");
            }

            SingleUserModeScope.reconnectTimeout.Value = reconnectTimeout;

            log.Info("Entering SingleUserModeScope");

            dbConnection.Value = connection;
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            SqlUtility.SetSingleUser(connection, connection.Database);

            active.Value = true;
        }

        /// <summary>
        /// Timeout used for reconnecting when someone else steals the connection
        /// </summary>
        public static TimeSpan ReconnectTimeout = reconnectTimeout.Value;

        /// <summary>
        /// Indicates if a SingleUserModeScope is active on the current thread
        /// </summary>
        public static bool IsActive = active.Value;

        /// <summary>
        /// Dispose - get rid of single user mode
        /// </summary>
        public void Dispose()
        {
            if (!active.Value)
            {
                return;
            }

            RestoreMultiUserMode();
        }

        /// <summary>
        /// Return the database to multi user mode using the specified connection
        /// </summary>
        public static void RestoreMultiUserMode()
        {
            bool succeeded = false;
            int maxTries = 100;

            while (!succeeded && maxTries >= 0)
            {
                maxTries--;

                try
                {
                    SqlUtility.SetMultiUser(dbConnection.Value);

                    succeeded = true;
                }
                catch (Exception ex) when (ex is SingleUserModeException || ex is SqlException)
                {
                    log.Error("Failed to set database back to multi-user mode.", ex);
                    SqlConnection.ClearAllPools();
                    dbConnection.Value = SqlSession.Current.OpenConnection();
                }
            }

            // Deactivate the scope after we try to go back to multi-user mode in case someone else steals the connection
            // or we can't immediately re-connect
            active.Value = false;

            log.Info("Leaving SingleUserModeScope");
        }
    }
}
