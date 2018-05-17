﻿using System;
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

        /// <summary>
        /// Constructor - initiates the scope
        /// </summary>
        public SingleUserModeScope() :
            this(TimeSpan.FromSeconds(30))
        {

        }

        /// <summary>
        /// Constructor - initiates the scope
        /// </summary>
        public SingleUserModeScope(TimeSpan reconnectTimeout)
        {
            if (active.Value)
            {
                throw new InvalidOperationException("Can only have one active scope at a time.");
            }

            SingleUserModeScope.reconnectTimeout.Value = reconnectTimeout;

            log.Info("Entering SingleUserModeScope");

            // Start by disconnecting all users. We don't use this same connection the whole time, so its possible that
            // someone could sneak in and take the single connection in between us releasing and getting it.  But if that happened,
            // we would blowup, and the upgrade would just have to start over the next time.
            using (DbConnection con = SqlSession.Current.OpenConnection())
            {
                SqlUtility.SetSingleUser(con);
            }

            // Clear out the pool so that connection holding onto SINGLE_USER gets released
            SqlConnection.ClearAllPools();

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
        /// Return the database to multi user mode using the specified connection
        /// </summary>
        public static void RestoreMultiUserMode(DbConnection con)
        {
            if (!active.Value)
            {
                return;
            }

            RestoreMultiUserMode(con, false);
        }

        /// <summary>
        /// Dispose - get rid of single user mode
        /// </summary>
        public void Dispose()
        {
            if (!active.Value)
            {
                return;
            }

            RestoreMultiUserMode(null, true);
        }

        /// <summary>
        /// Return the database to multi user mode using the specified connection
        /// </summary>
        private static void RestoreMultiUserMode(DbConnection con, bool clearConnectionPool)
        {
            bool shouldDisposeConnection = false;
            bool succeeded = false;
            int maxTries = 100;

            while (!succeeded && maxTries >= 0)
            {
                maxTries--;

                try
                {
                    // Clear out the pool so any connection holding onto SINGLE_USER gets released
                    if (clearConnectionPool)
                    {
                        SqlConnection.ClearAllPools();
                    }

                    // Allow multiple connections again
                    if (con == null)
                    {
                        shouldDisposeConnection = true;
                        con = SqlSession.Current.OpenConnection();
                    }

                    SqlUtility.SetMultiUser(con);

                    succeeded = true;
                }
                catch (SingleUserModeException ex)
                {
                    log.Error("Failed to set database back to multi-user mode.", ex);
                }
                catch (SqlException ex)
                {
                    log.Error("Failed to set database back to multi-user mode.", ex);
                }
                finally
                {
                    // Dispose the connection if this function created it
                    if (shouldDisposeConnection && con != null)
                    {
                        con.Dispose();
                    }
                }
            }

            // Deactivate the scope after we try to go back to multi-user mode in case someone else steals the connection
            // or we can't immediately re-connect
            active.Value = false;

            log.Info("Leaving SingleUserModeScope");
        }
    }
}
