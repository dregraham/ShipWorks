using System.Data.Common;
using System.Data.SqlClient;
using SD.Tools.OrmProfiler.Interceptor;

namespace ShipWorks.Data
{
    /// <summary>
    /// Extension methods for DbConnection
    /// </summary>
    public static class DbConnectionExtensions
    {
        /// <summary>
        /// Attempt to get the connection as a SqlConnection
        /// </summary>
        /// <remarks>This should be used instead of manually casting so that we can get a SqlConnection
        /// even if the profile is attached while debugging. In release mode, this should be no different
        /// than doing the cast manually</remarks>
        public static SqlConnection AsSqlConnection(this DbConnection connection)
        {
            SqlConnection sqlConn = connection as SqlConnection;

#if DEBUG
            if (sqlConn == null)
            {
                sqlConn = (connection as ProfilerDbConnection)?.WrappedConnection as SqlConnection;
            }
#endif

            return sqlConn;
        }
    }
}
