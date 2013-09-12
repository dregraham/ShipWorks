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
        static readonly ILog log = LogManager.GetLogger(typeof(SqlSessionScope));

        [ThreadStatic]
        static bool active = false;

        /// <summary>
        /// Constructor - initiates the scope
        /// </summary>
        public SingleUserModeScope()
        {
            if (active)
            {
                throw new InvalidOperationException("Can only have one active scope at a time.");
            }

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
        /// Indicates if a SingleUserModeScope is active on the current thread
        /// </summary>
        public static bool IsActive
        {
            get { return active; }
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

            active = false;

            try
            {
                // Clear out the pool so any connection holding onto SINGLE_USER gets released
                SqlConnection.ClearAllPools();

                // Allow multiple connections again
                using (SqlConnection con = SqlSession.Current.OpenConnection())
                {
                    SqlUtility.SetMultiUser(con);
                }
            }
            catch (SingleUserModeException ex)
            {
                log.Error("Failed to set database back to multi-user mode.", ex);
            }
            catch (SqlException ex)
            {
                log.Error("Failed to set database back to multi-user mode.", ex);
            }
        }
    }
}
