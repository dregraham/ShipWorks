using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
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

        [ThreadStatic]
        static bool active;

        [ThreadStatic]
        static TimeSpan reconnectTimeout;

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
            if (active)
            {
                throw new InvalidOperationException("Can only have one active scope at a time.");
            }

            SingleUserModeScope.reconnectTimeout = reconnectTimeout;

            log.Info("Entering SingleUserModeScope");

            // Start by disconnecting all users. We don't use this same connection the whole time, so its possible that
            // someone could sneak in and take the single connection in between us releasing and getting it.  But if that happened, 
            // we would blowup, and the upgrade would just have to start over the next time.
            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                SqlUtility.SetSingleUser(con);
            }

            // Clear out the pool so that connection holding onto SINGLE_USER gets released
            SqlConnection.ClearAllPools();

            active = true;
        }

        /// <summary>
        /// Timeout used for reconnecting when someone else steals the connection
        /// </summary>
        public static TimeSpan ReconnectTimeout
        {
            get
            {
                return reconnectTimeout; 
            }
        }

        /// <summary>
        /// Indicates if a SingleUserModeScope is active on the current thread
        /// </summary>
        public static bool IsActive
        {
            get { return active; }
        }

        /// <summary>
        /// Return the database to multi user mode using the specified connection
        /// </summary>
        public static void RestoreMultiUserMode(SqlConnection con)
        {
            if (!active)
            {
                return;
            }

            RestoreMultiUserMode(con, false);
        }

        /// <summary>
        /// Dispose - get rid of single user moede
        /// </summary>
        public void Dispose()
        {
            if (!active)
            {
                return;
            }

            RestoreMultiUserMode(null, true);
        }

        /// <summary>
        /// Return the database to multi user mode using the specified connection
        /// </summary>
        private static void RestoreMultiUserMode(SqlConnection con, bool clearConnectionPool)
        {
            bool shouldDisposeConnection = false;

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

            // Deactivate the scope after we try to go back to multi-user mode in case someone else steals the connection
            // or we can't immediately re-connect
            active = false;

            log.Info("Leaving SingleUserModeScope");
        }
    }
}
