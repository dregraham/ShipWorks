using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.HelperClasses;
using System.Data.SqlClient;
using log4net;

namespace ShipWorks.Data.Connection
{
    /// <summary>
    /// Can be used to temporarily switch the SqlSession that will be used to connect to the database.
    /// </summary>
    public class SqlSessionScope : IDisposable
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(SqlSessionScope));

        // Only one possible at a time
        static SqlSession scopedSession;

        /// <summary>
        /// Constructs a new SqlSessionScope.  The given SqlSession is active until
        /// the SqlSessionScope object is disposed.
        /// </summary>
        public SqlSessionScope(SqlSession scopedSession)
        {
            if (SqlSessionScope.scopedSession != null)
            {
                throw new InvalidOperationException("Only one SqlSessionScope may be active at a time.");
            }

            log.InfoFormat("Entering SqlSessionScope ({0} - {1})", scopedSession.ServerInstance, scopedSession.DatabaseName);

            SqlSessionScope.scopedSession = scopedSession;
        }

        /// <summary>
        /// Returns the SqlSession that is currently in scope.  Returns null if there is not
        /// currently a SqlSessionScope active.
        /// </summary>
        public static SqlSession ScopedSqlSession
        {
            get { return scopedSession; }
        }

        /// <summary>
        /// Reset the connection string
        /// </summary>
        public void Dispose()
        {
            SqlSessionScope.scopedSession = null;

            log.InfoFormat("Leaving SqlSessionScope");
        }
    }
}
